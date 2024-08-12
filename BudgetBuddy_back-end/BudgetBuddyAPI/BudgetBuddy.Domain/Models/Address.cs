using BudgetBuddy.Domain.Interface;

namespace BudgetBuddy.Domain.Models
{
    public class Address : IAuditable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
