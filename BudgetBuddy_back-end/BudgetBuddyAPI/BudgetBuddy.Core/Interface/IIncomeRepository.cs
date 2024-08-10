using BudgetBuddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface IIncomeRepository : IGenericRepository<Income>
    {
        /// <summary>
        /// Retrives an expense with a particular Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An object of expnse if found, or null if not found</returns>
        Task<Income> GetIncomeByIdAsync(string id);
        IQueryable<Income> GetIncomeByDateAysnc(DateTime date);
        IQueryable<Income> GetAllIncomeAsync();
        IQueryable<Income> GetAllEIncomeByDateRangeAsync(DateTime start, DateTime end);
    }
}
