using BudgetBuddy.Domain.Common;
using BudgetBuddy.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Domain.Models
{
    public class Address : IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; }
        public DateTimeOffset UpdatedAt { get ; set ; }
    }
}
