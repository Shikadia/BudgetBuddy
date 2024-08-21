using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Identity;
using ILogger = Serilog.ILogger;
using System.Globalization;
//using Microsoft.IdentityModel.Tokens;

namespace BudgetBuddy.Core.Services
{
    public class DigitTokenService : PhoneNumberTokenProvider<AppUser>, IDigitTokenService
    {
        ILogger _Logger;
        public DigitTokenService(ILogger Logger)
        {
            _Logger = Logger;
        }

        public const string DIGITPHONE = "DigitPhone";
        public const string DIGITEMAIL = "DigitEmail";

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
        => Task.FromResult(false);

        public override async Task<string> GenerateAsync(string purpose, UserManager<AppUser> manager, AppUser user)
        {
            var token = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
            var tokenData = BitConverter.ToString(token.GetDataNoClone());
            var modifier = await GetUserModifierAsync(purpose, manager, user);
            _Logger.Information($"generated token:  {tokenData} , Modifier : {modifier}");
            var code = Rfc6238AuthenticationProvider.GenerateCode(token, modifier).ToString("D6", CultureInfo.InvariantCulture);
            return code;
        }

        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<AppUser> manager, AppUser user)
        {
            if (!Int32.TryParse(token, out int code))
                return false;

            var securityToken = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
            var tokenData = BitConverter.ToString(securityToken.GetDataNoClone());
            var modifier = await GetUserModifierAsync(purpose, manager, user);
            _Logger.Information($"Validated token:  {tokenData} , Modifier : {modifier}");
            var valid = Rfc6238AuthenticationProvider.ValidateCode(securityToken, code, modifier, token.Length);
            return valid;
        }

        public override Task<string> GetUserModifierAsync(string purpose, UserManager<AppUser> manager, AppUser user)
        {
            return base.GetUserModifierAsync(purpose, manager, user);
        }
    }
}
