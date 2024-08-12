using BudgetBuddy.Domain.Models;

namespace BudgetBuddy.Core.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        /// <summary>
        /// Retrives a Category which has a matching Id property
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The category object if found, or null if not found</returns>
        Task<Category> GetCategoryByIdAsync(string id);
        /// <summary>
        /// Retrives all category from the database
        /// </summary>
        /// <returns>An IQueryable of the category object</returns>
        IQueryable<Category> GetAllCategory();
    }
}
