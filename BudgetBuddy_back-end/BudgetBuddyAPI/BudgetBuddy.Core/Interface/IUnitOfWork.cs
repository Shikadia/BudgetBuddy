using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IIncomeRepository IncomeRepository { get; }
        IExpenseRepository ExpenseRepository { get; }
        IAddressRepository AddressRepository { get; }
        Task Save();
    }
}
