using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly BudgetBuddyDbContext _context;
        public AddressRepository(BudgetBuddyDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Address> GetAllAddressByAppUserId(string UserId)
        {
            return _context.Address.Where(x => x.AppUserId == UserId);
        }
    }
}
