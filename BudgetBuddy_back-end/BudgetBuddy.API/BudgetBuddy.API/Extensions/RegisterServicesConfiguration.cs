using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Services;
using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Infrastructure.Repository;
using FluentValidation;
using Serilog;
using System.Reflection;

namespace BudgetBuddyAPI.Extensions
{
    public static class RegisterServicesConfiguration
    {
        public static void AddRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );
            });

            services.AddSingleton(Log.Logger);

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IValidator<LoginDTO>, LoginUserValidator>();
            services.AddTransient<IValidator<SignUpDTO>, UserValidator>();            
        }
    }
}
