﻿namespace BudgetBuddy.Core.DTOs
{
    public class CredentialResponseDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
