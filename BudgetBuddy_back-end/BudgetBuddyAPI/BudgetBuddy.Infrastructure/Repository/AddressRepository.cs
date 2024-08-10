using BudgetBuddy.Core.Interface;
using BudgetBuddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Infrastructure.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(BudgetBuddyDbContext context) : base(context)
        {
            
        }
    }
}
