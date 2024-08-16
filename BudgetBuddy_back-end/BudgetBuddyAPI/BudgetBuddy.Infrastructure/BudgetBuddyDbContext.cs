using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace BudgetBuddy.Infrastructure
{
    public class BudgetBuddyDbContext : IdentityDbContext<AppUser>
    {
        private const string UPDATEDAT = "UpdatedAt";
        private const string CREATEDAT = "createdAt";

        public BudgetBuddyDbContext(DbContextOptions<BudgetBuddyDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<Income> Income { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is AppUser appUser)
                {
                    AuditPropertiesChange(item.State, appUser);
                }
                else if (item.Entity is Address address)
                {
                    AuditPropertiesChange(item.State, address);
                }
                else if (item.Entity is Expense expense)
                {
                    AuditPropertiesChange(item.State, expense);
                }
                else if (item.Entity is Income income)
                {
                    AuditPropertiesChange(item.State, income);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public static void AuditPropertiesChange<T>(EntityState state, T obj) where T : class
        {
            PropertyInfo? value;
            switch (state)
            {
                case EntityState.Modified:
                    value = obj.GetType().GetProperty(UPDATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTimeOffset.UtcNow);
                    break;
                case EntityState.Added:
                    value = obj.GetType().GetProperty(CREATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTimeOffset.UtcNow);
                    value = obj.GetType().GetProperty(UPDATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTimeOffset.UtcNow);
                    break;
                default:
                    break;
            }
        }

    }
}
