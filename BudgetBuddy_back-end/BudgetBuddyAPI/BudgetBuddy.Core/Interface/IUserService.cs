using BudgetBuddy.Core.DTOs;

namespace BudgetBuddy.Core.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<string>> AddAddress(AddAddressRequestDTO requestDTO, string id);
        Task<ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>> GetAllAddressByAppUserId(string appUserId, int pageSize, int pageNumebr);
    }
}
