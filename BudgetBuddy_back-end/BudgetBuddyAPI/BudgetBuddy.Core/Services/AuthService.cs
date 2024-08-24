using AutoMapper;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ILogger = Serilog.ILogger;

namespace BudgetBuddy.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IDigitTokenService _digitTokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthService(IServiceProvider provider, IUnitOfWork unitOfWork, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            IDigitTokenService digitTokenService, IEmailService emailService, INotificationService notificationService)
        {
            _userManager = provider.GetRequiredService<UserManager<AppUser>>();
            _tokenService = provider.GetRequiredService<ITokenService>();
            _unitOfWork = unitOfWork;
            _logger = provider.GetRequiredService<ILogger>();
            _configuration = configuration;
            _roleManager = roleManager;
            _digitTokenService = digitTokenService;
            _emailService = emailService;
            _notificationService = notificationService;
        }
        public async Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User does not exist", (int)HttpStatusCode.NotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return ResponseDto<CredentialResponseDTO>.Fail("Invalid user credential", (int)HttpStatusCode.BadRequest);
            }

            if (!user.EmailConfirmed)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User's account is not confirmed", (int)HttpStatusCode.BadRequest);
            }

            else if ((bool)!user.IsActive)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User's account is deactivated", (int)HttpStatusCode.BadRequest);
            }

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
            var credentialResponse = new CredentialResponseDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                RefreshToken = user.RefreshToken,
            };

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.Information($"user : {user.FirstName + "," + user.LastName} successfully logged in ");
                return ResponseDto<CredentialResponseDTO>.Success("Login successful", credentialResponse);
            }
            _logger.Information($"An error occured when logining {result.Errors}");
            return ResponseDto<CredentialResponseDTO>.Fail("login failed", (int)HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken)
        {
            var tokenToBeRefreshed = refreshToken.RefreshToken;
            var userId = refreshToken.UserId;

            var user = await _userManager.FindByIdAsync(userId);
            int value = DateTime.Compare((DateTime)user?.RefreshTokenExpiryTime!, DateTime.Now);
            if (user.RefreshToken != tokenToBeRefreshed || value < 0)
            {
                return ResponseDto<RefreshTokenResponseDTO>.Fail("Invalid credentials RT", (int)HttpStatusCode.BadRequest);
            }
            var refreshMapping = new RefreshTokenResponseDTO
            {
                NewAccessToken = await _tokenService.GenerateToken(user),
                NewRefreshToken = _tokenService.GenerateRefreshToken()
            };

            user.RefreshToken = refreshMapping.NewRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return ResponseDto<RefreshTokenResponseDTO>.Success("Token refreshed successfully", refreshMapping);
        }

        public async Task<ResponseDto<SignUpResponseDTO>> SignUp(SignUpDTO model)
        {
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(model.Email);
                if (checkEmail != null)
                {
                    return ResponseDto<SignUpResponseDTO>.Fail("Email already Exists", (int)HttpStatusCode.BadRequest);
                }
                if (model.Role == null)
                {
                    return ResponseDto<SignUpResponseDTO>.Fail("Role field can not be empty", (int)HttpStatusCode.BadRequest);
                }
                var userModel = new AppUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email.ToLower().Trim(),
                    Balance = 0,
                    IsActive = true,
                    UserName = model.FirstName + model.LastName,
                    RefreshToken = "",
                };

                var result = await _userManager.CreateAsync(userModel, model.Password);
                if (!result.Succeeded)
                {
                    return ResponseDto<SignUpResponseDTO>.Fail("An Error occured when creating user", (int)HttpStatusCode.BadRequest);
                }

                if (!await _roleManager.RoleExistsAsync(model.Role.ToUpperInvariant()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role.ToUpperInvariant()));
                }
                await _userManager.AddToRoleAsync(userModel,model.Role.ToUpperInvariant());

                var sendEmailResponse = await SendEmail(userModel);
                if (sendEmailResponse == null || !sendEmailResponse.Status)
                    return ResponseDto<SignUpResponseDTO>.Fail("Registration successful, but resent otp for email verification",
                        sendEmailResponse.StatusCode);

                return ResponseDto<SignUpResponseDTO>.Success("Registration Successful",
                 new SignUpResponseDTO { Id = userModel.Id, Email = userModel.Email },
                 (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return ResponseDto<SignUpResponseDTO>.Fail("An Error occured when creating user", (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDto<string>> ChangePassword(ChangePasswordDTO model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResponseDto<string>.Fail("User not found.");
            }

            var isPasswordConfirmed = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isPasswordConfirmed)
            {
                return ResponseDto<string>.Fail("Current password is incorrect.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return ResponseDto<string>.Success("Successful!", "Password has been updated");
            }

            return IdentityResultErrors<string>(result);
        }
        private ResponseDto<T> IdentityResultErrors<T>(IdentityResult result)
        {
            return ResponseDto<T>.Fail(GetErrors(result), (int)HttpStatusCode.InternalServerError);
        }
        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (curr, err) => curr + err.Description + "\n");
        }

        public async Task<ResponseDto<CredentialResponseDTO>> GoogleSignInUp(string token, string role)
        {
            try
            {
                var payLoad = await VerifyGoogleTokenAsync(token);
                if (payLoad == null)
                {
                    return ResponseDto<CredentialResponseDTO>.Fail("Something went wrong payload", (int)HttpStatusCode.InternalServerError);
                }

                var user = await _userManager.FindByEmailAsync(payLoad.Email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        FirstName = payLoad.Given_Name,
                        LastName = payLoad.Family_Name,
                        PhoneNumber = "",
                        Email = payLoad.Email,
                        Balance = 0,
                        IsActive = true,
                        UserName = payLoad.Given_Name + payLoad.Family_Name,
                        RefreshToken = "",
                        EmailConfirmed = payLoad.Email_Verified
                    };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(role));
                        }
                        await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                    }
                }

                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
                var credentialResponse = new CredentialResponseDTO()
                {
                    Id = user.Id,
                    Token = await _tokenService.GenerateToken(user),
                    Email = user.Email,
                    RefreshToken = user.RefreshToken,
                };

                var updateUser = await _userManager.UpdateAsync(user);
                if (updateUser.Succeeded)
                {
                    _logger.Information($"user : {user.FirstName + "," + user.LastName} successfully logged in ");
                    return ResponseDto<CredentialResponseDTO>.Success("Login successful", credentialResponse);
                }
                _logger.Information($"An error occured when logining {updateUser.Errors}");
                return ResponseDto<CredentialResponseDTO>.Fail("login failed", (int)HttpStatusCode.InternalServerError);

            }
            catch (Exception ex)
            {
                _logger.Error("ERROR: ", ex.Message);
                return ResponseDto<CredentialResponseDTO>.Fail("something went wrong", (int)HttpStatusCode.InternalServerError);
            }

        }
        private async Task<GoogleUserInfoDTO> VerifyGoogleTokenAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
                var x = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadFromJsonAsync<GoogleUserInfoDTO>();
                    return userInfo;
                }
                else
                {
                    _logger.Error("Failed to retrieve user info from Google. ");
                    return null;
                }
            }
        }

        public async Task<ResponseDto<string>> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDTO.EmailAddress);
            if (user == null)
            {
                return ResponseDto<string>.Fail("User not found", (int)HttpStatusCode.NotFound);
            }
            var purpose = UserManager<AppUser>.ConfirmEmailTokenPurpose;
            var result = await _digitTokenService.ValidateAsync(purpose, confirmEmailDTO.Token, _userManager, user);
            if (result)
            {
                user.EmailConfirmed = true;
                user.IsActive = true;
                var update = await _userManager.UpdateAsync(user);
                if (update.Succeeded)
                {
                    return ResponseDto<string>.Success("Email Confirmation successful", user.Id, (int)HttpStatusCode.OK);
                }
            }
            return ResponseDto<string>.Fail("Email Confirmation not successful", (int)HttpStatusCode.Unauthorized);
        }

        public async Task<ResponseDto<string>> ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user is null)
                return ResponseDto<string>.Fail("This email does not exist on this app", (int)HttpStatusCode.NotFound);
            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var token = await _digitTokenService.GenerateAsync(purpose, _userManager, user);
            var mailBody = await EmailBodyBuilder.GetEmailBody(user, "StaticFiles/ForgotPassword.html", token);
            var emailNotification = new EmailDTO
            {
                ToEmail = user.Email,
                Subject = "Reset Password",
                Message = mailBody,
            };

            try
            {
                var response = await _emailService.SendEmail(emailNotification);

                return ResponseDto<string>.Success($"This email is successfully to: {model.EmailAddress}",
                        $"A reset link was successfully sent to {model.EmailAddress}", (int)HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return ResponseDto<string>.Fail("Service is not available, please try again later.", (int)HttpStatusCode.ServiceUnavailable);
            }
        }

        public async Task<ResponseDto<string>> ResendOTP(ResendOtpDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ResponseDto<string>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
            }

            var purpose = (model.Purpose == "ConfirmEmail") ? UserManager<AppUser>.ConfirmEmailTokenPurpose
                : UserManager<AppUser>.ResetPasswordTokenPurpose;
            var token = await _digitTokenService.GenerateAsync(purpose, _userManager, user);

            var mailBody = await EmailBodyBuilder.GetEmailBody(user, emailTempPath: (model.Purpose == "ConfirmEmail") ?
                "StaticFiles/EmailConfirmation.html" : "StaticFiles/ForgotPassword.html", token);

            var emailNotification = new EmailDTO
            {
                ToEmail = user.Email,
                Subject = "Email Verification",
                Message = mailBody,
            };

           var response = await _emailService.SendEmail(emailNotification);

            if (response.Data)
                if (!user.IsActive)
                {
                    return ResponseDto<string>.Success($"This email is successfully to: {model.Email}",
                        $"OTP was successfully resent to {model.Email}");
                }
            return ResponseDto<string>.Fail("Sending OTP was not successful", (int)HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var validator = new ResetPasswordValidator();
            await validator.ValidateAsync(resetPasswordDTO);
            _logger.Information("Reset password attempt");
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return ResponseDto<string>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
            }
            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var isValidToken = await _digitTokenService.ValidateAsync(purpose, resetPasswordDTO.Token, _userManager, user);
            var result = new IdentityResult();
            var hasher = new PasswordHasher<AppUser>();
            if (isValidToken)
            {
                var hash = hasher.HashPassword(user, resetPasswordDTO.NewPassword);
                user.PasswordHash = hash;
                result = await _userManager.UpdateAsync(user);
            }
            if (result.Succeeded)
            {
                return ResponseDto<string>.Success("Password has been reset successfully", user.Id, (int)HttpStatusCode.OK);
            }
            return ResponseDto<string>.Fail("Invalid Token", (int)HttpStatusCode.BadRequest);
        }
        private async Task<ResponseDto<bool>> SendEmail(AppUser userModel)
        {
            var purpose = UserManager<AppUser>.ConfirmEmailTokenPurpose;
            string token = await _digitTokenService.GenerateAsync(purpose, _userManager, userModel);
            var mailBody = await EmailBodyBuilder.GetEmailBody(userModel, "StaticFiles/EmailConfirmation.html", token);
            var sendEmail = new EmailDTO
            {
                ToEmail = userModel.Email,
                Subject = "Email Verification",
                Message = mailBody
            };

            try
            {
                return await _emailService.SendEmail(sendEmail);
            }
            catch (Exception)
            {
                return ResponseDto<bool>.Fail("Service is not available, please try again later.",
                    (int)HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}