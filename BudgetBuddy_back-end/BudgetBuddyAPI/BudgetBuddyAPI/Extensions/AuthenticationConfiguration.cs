using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BudgetBuddyAPI.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt");
            var googleConfig = configuration.GetSection("google_auth");
            var x = googleConfig.GetSection("ClientId").Value;
            services.AddAuthentication(v =>
            {
                v.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                v.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(v =>
             {
                 v.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateLifetime = true,
                     ValidateAudience = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Environment.GetEnvironmentVariable("Jwt_Issuer"),
                     ValidAudience = Environment.GetEnvironmentVariable("Jwt_Audience"),
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt_Token")))
                 };
             })
             .AddGoogle(options =>
             {
                 options.ClientId = Environment.GetEnvironmentVariable("google_auth_ClientId");
                 options.ClientSecret = Environment.GetEnvironmentVariable("google_auth_secret");
                 options.CallbackPath = "/auth/google-callback";
             });
        }
    }
}
