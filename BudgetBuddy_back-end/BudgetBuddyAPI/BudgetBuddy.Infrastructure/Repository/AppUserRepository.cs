using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        private readonly BudgetBuddyDbContext _context;
        private readonly DbSet<AppUser> _db;

        public AppUserRepository(BudgetBuddyDbContext context) : base(context)
        {
            _context = context;
            _db = context.Set<AppUser>();
        }
    }
}
