using BudgetBuddy.Domain.Interface;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Domain.Models
{
    public class AppUser : IdentityUser, IAuditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        public bool? IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public decimal? Balance { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Income>? Incomes { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
        public DateTimeOffset CreatedAt {  get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public AppUser() => Addresses = new HashSet<Address>();
    }
}
