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
