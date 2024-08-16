using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BudgetBuddy.Infrastructure
{
    public class BudgetBuddyDbInitializer
    {
        public static async Task Seed(IApplicationBuilder builder)
        {
            try
            {
                using var serviceScope = builder.ApplicationServices.CreateScope();
                var context = serviceScope.ServiceProvider.GetService<BudgetBuddyDbContext>();
                string filePath = "C:\\workspace\\ALX\\AlxProject\\BudgetBuddy\\BudgetBuddy_back-end\\BudgetBuddyAPI\\BudgetBuddy.Infrastructure\\Data\\Roles.json"; /*Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"BudgetBuddy.Infrastructure\Data\");*/
                if (await context.Database.EnsureCreatedAsync()) return;

                if (!context.Roles.Any())
                {
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var readText = await File.ReadAllTextAsync(filePath + "Roles.json");
                    var Roles = JsonConvert.DeserializeObject<List<IdentityRole>>(readText);
                    var createRoleTasks = Roles.Select(role => roleManager.CreateAsync(role));
                    await Task.WhenAll(createRoleTasks);
                }
                if (!context.AppUser.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var readText = await File.ReadAllTextAsync(filePath + "AppUsers.json");
                    var users = JsonConvert.DeserializeObject<List<AppUser>>(readText);
                    users.ForEach(delegate (AppUser user) {
                        userManager.CreateAsync(user, "%Alvin2024");
                        userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                        context.AppUser.AddAsync(user);
                    });
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
    }
}
