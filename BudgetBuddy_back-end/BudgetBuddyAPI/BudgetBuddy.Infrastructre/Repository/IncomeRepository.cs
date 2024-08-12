using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Infrastructure.Repository
{
    internal class IncomeRepository : GenericRepository<Income>, IIncomeRepository
    {
        private readonly BudgetBuddyDbContext _context;
        public IncomeRepository(BudgetBuddyDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Income> GetAllEIncomeByDateRangeAsync(DateTime start, DateTime end)
        {
            return _context.Income.Where(e => e.CreatedAt >= start && e.CreatedAt <= end);
        }

        public IQueryable<Income> GetAllIncomeAsync()
        {
            return _context.Income;
        }

        public IQueryable<Income> GetIncomeByDateAysnc(DateTime date)
        {
            return _context.Income.Where(e => e.CreatedAt == date);
        }

        public Task<Income> GetIncomeByIdAsync(string id)
        {
            return _context.Income.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
