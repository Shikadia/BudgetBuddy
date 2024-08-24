namespace BudgetBuddy.Core.DTOs
{
    public class EditTransactionRequestDTO
    {
        public string id {  get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Tag { get; set; }
    }
}
