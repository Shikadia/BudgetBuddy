using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class CategoryRepostory : GenericRepository<Category>, ICategoryRepository
    {
        private readonly BudgetBuddyDbContext _context;
        public CategoryRepostory(BudgetBuddyDbContext context) : base(context)
        {

            _context = context;

        }
        public IQueryable<Category> GetAllCategory()
        {
            return _context.Category;
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
