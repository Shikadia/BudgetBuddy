﻿namespace BudgetBuddy.Core.DTOs
{
    public class TransactionResponseDTO
    {
        public IEnumerable<ListOfTransactions> ListOfTransactions {get; set;}
        public decimal TotalAmount { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
    }

    public class ListOfTransactions
    {
        public string Id { get; set; }
        public string Type { get; set; } // "Income" or "Expense"
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string CategoryOrTag { get; set; }
        public string Description { get; set; }
    }
}
