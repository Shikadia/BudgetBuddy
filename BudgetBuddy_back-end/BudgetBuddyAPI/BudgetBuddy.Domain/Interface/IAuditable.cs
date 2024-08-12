namespace BudgetBuddy.Domain.Interface
{
    public interface IAuditable
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
