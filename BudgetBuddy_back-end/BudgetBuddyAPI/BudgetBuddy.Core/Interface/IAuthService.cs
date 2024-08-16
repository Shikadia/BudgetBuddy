using BudgetBuddy.Core.DTOs;

namespace BudgetBuddy.Core.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model);
        Task<ResponseDto<SignUpResponseDTO>> SignUp(SignUpDTO model);
        Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken);
        Task<ResponseDto<string>> ChangePassword(ChangePasswordDTO model, string userId);
        Task<ResponseDto<CredentialResponseDTO>> GoogleSignInUp(string token, string role);
    }
}
