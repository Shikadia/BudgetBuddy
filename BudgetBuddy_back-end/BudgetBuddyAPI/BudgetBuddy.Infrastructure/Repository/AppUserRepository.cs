using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
