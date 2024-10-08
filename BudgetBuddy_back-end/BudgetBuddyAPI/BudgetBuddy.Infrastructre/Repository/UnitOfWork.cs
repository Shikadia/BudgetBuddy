﻿using BudgetBuddy.Core.Interface;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BudgetBuddyDbContext _context;
        private AppUserRepository _appUserRepository { get; set; } = null!;
        private IncomeRepository _incomeRepository { get; set; } = null!;
        private ExpenseRepository _expenseRepository { get; set; } = null!;
        private AddressRepository _addressRepository { get; set; } = null;

        public UnitOfWork(BudgetBuddyDbContext context)
        {
            _context = context;
        }
        public IAppUserRepository AppUserRepository => _appUserRepository ??= new AppUserRepository(_context);

        public IIncomeRepository IncomeRepository => _incomeRepository ??= new IncomeRepository(_context); 

        public IExpenseRepository ExpenseRepository => _expenseRepository ??= new ExpenseRepository(_context);
        public IAddressRepository AddressRepository => _addressRepository ??= new AddressRepository(_context);

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
