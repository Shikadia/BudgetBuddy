using BudgetBuddy.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model);
        Task<ResponseDto<SignUpResponseDTO>> SignUp(SignUpDTO model);
        Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken);
        Task<ResponseDto<string>> ChangePassword(ChangePasswordDTO model, string userId);
    }
}
