using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Services;
using BudgetBuddy.Domain.Models;
using BudgetBuddy.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddyAPI.Extensions
{
    public static class ConnectionConfiguration
    {
        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddDbContextPool<BudgetBuddyDbContext>(options =>
            {
                string connStr;
                if (env.IsProduction())
                {
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                options.UseSqlServer(connStr);
            });

            var builder = services.AddIdentity<AppUser, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 8;
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = true;
                x.Password.RequireLowercase = true;
                x.User.RequireUniqueEmail = true;
                x.SignIn.RequireConfirmedEmail = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            _ = builder.AddEntityFrameworkStores<BudgetBuddyDbContext>()
            .AddTokenProvider<DigitTokenService>(DigitTokenService.DIGITEMAIL)
            .AddDefaultTokenProviders();
        }
    }
}
