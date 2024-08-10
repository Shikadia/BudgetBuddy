using BudgetBuddy.Domain.Common;
using BudgetBuddy.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Domain.Models
{
    public class Expense : IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AppUSerID {  get; set; }
        public AppUser AppUser { get; set; }
        public string CategoryID {  get; set; } 
        public Category Category { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; }
        public DateTimeOffset UpdatedAt { get ; set ; }
    }
}
