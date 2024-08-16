namespace BudgetBuddy.Core.DTOs
{
    public class TransactionResponseDTO
    {
        public PaginationResult<IEnumerable<ListOfTransactions>> ListOfTransactions { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ListOfTransactions
    {
        public string Id { get; set; }
        public string Type { get; set; } // "Income" or "Expense"
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public string CategoryOrTag { get; set; }
    }
}
