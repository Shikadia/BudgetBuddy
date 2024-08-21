using BudgetBuddy.Core.DTOs;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model);
        Task<ResponseDto<SignUpResponseDTO>> SignUp(SignUpDTO model);
        Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken);
        Task<ResponseDto<string>> ChangePassword(ChangePasswordDTO model, string userId);
        Task<ResponseDto<CredentialResponseDTO>> GoogleSignInUp(string token, string role);
        Task<ResponseDto<string>> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO);
        Task<ResponseDto<string>> ForgotPassword(ForgotPasswordDTO model);
        Task<ResponseDto<string>> ResendOTP(ResendOtpDTO model);
        Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
    }
}

