﻿using BudgetBuddy.Domain.Interface;

namespace BudgetBuddy.Domain.Models
{
    public class Expense : IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AppUSerID {  get; set; }
        public AppUser AppUser { get; set; }
        public string Tag { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; }
        public DateTimeOffset UpdatedAt { get ; set ; }
    }
}
