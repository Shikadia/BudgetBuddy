﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                     ValidIssuer = jwtConfig.GetSection("Issuer").Value,
                     ValidAudience = jwtConfig.GetSection("Audience").Value,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")))
                 };
             })
             .AddGoogle(options =>
             {
                 options.ClientId = googleConfig.GetSection("ClientId").Value;
                 options.ClientSecret = googleConfig.GetSection("secret").Value;
                 options.CallbackPath = "/auth/google-callback";
             });
        }
    }
}
