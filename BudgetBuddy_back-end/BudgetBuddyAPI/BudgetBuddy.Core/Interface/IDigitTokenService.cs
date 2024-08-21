﻿using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Core.Interface
{
    public interface IDigitTokenService
    {
        Task<string> GenerateAsync(string purpose, UserManager<AppUser> manager, AppUser user);
        Task<bool> ValidateAsync(string purpose, string token, UserManager<AppUser> manager, AppUser user);
    }
}
