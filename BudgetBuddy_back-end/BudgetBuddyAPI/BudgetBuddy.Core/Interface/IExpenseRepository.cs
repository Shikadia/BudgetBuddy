using BudgetBuddy.Domain.Models;

namespace BudgetBuddy.Core.Interface
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        /// <summary>
        /// Retrives an expense with a particular Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An object of expnse if found, or null if not found</returns>
        Task<Expense> GetExpenseByIdAsync(string id);
        IQueryable<Expense> GetExpenseByDateAysnc(DateTime date);
        IQueryable<Expense> GetAllExpensesAsync();
        IQueryable<Expense> GetAllExpensesByDateRangeAsync(DateTime start, DateTime end);
    }
}
