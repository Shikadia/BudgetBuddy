using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly BudgetBuddyDbContext _context;
        public ExpenseRepository(BudgetBuddyDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Expense> GetAllExpensesAsync(string id)
        {
            return _context.Expense.Where(x => x.AppUSerID == id);
        }

        public IQueryable<Expense> GetAllExpensesByDateRangeAsync(DateTime start, DateTime end)
        {
            return _context.Expense.Where(e => e.CreatedAt >= start && e.CreatedAt <= end);
        }

        public IQueryable<Expense> GetExpenseByDateAysnc(DateTime date)
        {
            return _context.Expense.Where(e => e.CreatedAt ==  date);
        }

        public Task<Expense> GetExpenseByIdAsync(string id)
        {
            return _context.Expense.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
