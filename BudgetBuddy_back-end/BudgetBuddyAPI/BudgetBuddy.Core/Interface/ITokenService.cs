using BudgetBuddy.Domain.Models;

namespace BudgetBuddy.Core.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser user);
        string GenerateRefreshToken();
    }
}
