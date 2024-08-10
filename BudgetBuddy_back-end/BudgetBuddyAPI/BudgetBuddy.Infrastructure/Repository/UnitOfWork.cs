using BudgetBuddy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BudgetBuddyDbContext _context;
        private CategoryRepostory _categoryRepostory { get; set; } = null!;
        private AppUserRepository _appUserRepository { get; set; } = null!;
        private IncomeRepository _incomeRepository { get; set; } = null!;
        private ExpenseRepository _expenseRepository { get; set; } = null!;

        public UnitOfWork(BudgetBuddyDbContext context)
        {
            _context = context;
        }
        public IAppUserRepository AppUserRepository => _appUserRepository ??= new AppUserRepository(_context);

        public ICategoryRepository CategoryRepository => _categoryRepostory ??= new CategoryRepostory(_context);

        public IIncomeRepository IncomeRepository => _incomeRepository ??= new IncomeRepository(_context); 

        public IExpenseRepository ExpenseRepository => _expenseRepository ??= new ExpenseRepository(_context);

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
