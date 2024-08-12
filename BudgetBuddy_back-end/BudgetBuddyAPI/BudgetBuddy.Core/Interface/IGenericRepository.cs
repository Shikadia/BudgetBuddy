namespace BudgetBuddy.Core.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Adds an object of type T to the database context
        /// </summary>
        /// <param name="entity">Object of type T to be added to database context</param>
        /// <returns></returns>
        Task InsertAsync(T entity);
        /// <summary>
        /// Deletes an object of type T from the database context
        /// </summary>
        /// <param name="id">ID of the object to be deleted from the database context</param>
        /// <returns></returns>
        Task DeleteAsync(string id);
        /// <summary>
        /// Deletes a range of objects of type T from the database context
        /// </summary>
        /// <param name="ids">List of IDs of entities to be deleted from the database context</param>
        /// <returns></returns>
        void DeleteRangeAsync(IEnumerable<T> entities);
        /// <summary>
        /// Updates the value of an object of type T in the database context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(T entity);
    }
}
