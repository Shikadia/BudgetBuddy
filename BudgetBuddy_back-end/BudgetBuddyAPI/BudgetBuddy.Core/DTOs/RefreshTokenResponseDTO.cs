namespace BudgetBuddy.Core.DTOs
{
    public class RefreshTokenResponseDTO
    {
        public string NewAccessToken { get; set; }
        public string NewRefreshToken { get; set; }
    }
}
