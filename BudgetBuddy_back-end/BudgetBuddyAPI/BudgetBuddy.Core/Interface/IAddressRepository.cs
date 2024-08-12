using BudgetBuddy.Domain.Models;

namespace BudgetBuddy.Core.Interface
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        IQueryable<Address> GetAllAddressByAppUserId(string UserId);
    }
}
