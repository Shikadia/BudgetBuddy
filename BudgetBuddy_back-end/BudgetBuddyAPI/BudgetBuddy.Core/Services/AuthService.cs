using AutoMapper;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Domain.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using ILogger = Serilog.ILogger;

namespace BudgetBuddy.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthService(IServiceProvider provider, IUnitOfWork unitOfWork, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = provider.GetRequiredService<UserManager<AppUser>>();
            _tokenService = provider.GetRequiredService<ITokenService>();
            _unitOfWork = unitOfWork;
            _logger = provider.GetRequiredService<ILogger>();
            _configuration = configuration;
            _roleManager = roleManager;
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
                return ResponseDto<RefreshTokenResponseDTO>.Fail("Invalid credentials", (int)HttpStatusCode.BadRequest);
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

                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                }

                await _userManager.AddToRoleAsync(userModel,model.Role);

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
                        FirstName = payLoad.GivenName,
                        LastName = payLoad.FamilyName,
                        PhoneNumber = "",
                        Email = payLoad.Email,
                        Balance = 0,
                        IsActive = true,
                        UserName = payLoad.GivenName + payLoad.FamilyName,
                        RefreshToken = "",
                        EmailConfirmed = true
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
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string token)
        {
            try
            {
                var config = _configuration.GetSection("google_auth");
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { config.GetSection("ClientId").Value }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
                _logger.Information($"Google token successfully validated {payload.Email}");
                return payload;

            }
            catch (Exception ex)
            {
                _logger.Error($"something went wrong when verifying google token {ex.Message}");
                return null;
            }
        }
    }
}