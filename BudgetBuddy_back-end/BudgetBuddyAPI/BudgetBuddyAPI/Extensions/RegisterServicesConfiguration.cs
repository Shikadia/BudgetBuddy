using BudgetBuddy.Core.AppSettings;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Services;
using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Infrastructure.ExternalService;
using BudgetBuddy.Infrastructure.Repository;
using FluentValidation;
using Microsoft.Extensions.Options;
using Serilog;

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
            services.AddScoped(v => v.GetRequiredService<IOptions<NotificationSettings>>().Value);
            services.Configure<NotificationSettings>(configuration.GetSection(nameof(NotificationSettings)));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDigitTokenService, DigitTokenService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddTransient<IValidator<ResetPasswordDTO>, ResetPasswordValidator>();
            services.AddTransient<IValidator<ForgotPasswordDTO>, EmailValidator>();
            services.AddTransient<IValidator<LoginDTO>, LoginUserValidator>();
            services.AddTransient<IValidator<SignUpDTO>, UserValidator>();
            
           
          

        }
    }
}
