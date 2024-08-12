using BudgetBuddy.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<string>> AddAddress(AddAddressRequestDTO requestDTO, string id);
        Task<ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>> GetAllAddressByAppUserId(string appUserId, int pageSize, int pageNumebr);
    }
}
