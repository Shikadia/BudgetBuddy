namespace BudgetBuddy.Core.Interface
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUserRepository { get; }
        IIncomeRepository IncomeRepository { get; }
        IExpenseRepository ExpenseRepository { get; }
        IAddressRepository AddressRepository { get; }
        Task Save();
    }
}
