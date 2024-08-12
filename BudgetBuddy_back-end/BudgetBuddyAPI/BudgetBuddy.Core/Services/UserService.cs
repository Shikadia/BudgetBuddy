using AutoMapper;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using ILogger = Serilog.ILogger;

namespace BudgetBuddy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public UserService(IUnitOfWork unitOfWork, UserManager<AppUser>  userManager,IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<ResponseDto<string>> AddAddress(AddAddressRequestDTO requestDTO, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.Information("User not found when trying to Add Address");
                return ResponseDto<string>.Fail("user not foun", (int)HttpStatusCode.BadRequest);
            }

            requestDTO.AppUserId = id;
            var address = new Address()
            {
                AppUserId = user.Id,
                Name = requestDTO.Name,
                City = requestDTO.City,
                State = requestDTO.State,
            };

            await _unitOfWork.AddressRepository.InsertAsync(address);
            await _unitOfWork.Save();

            return ResponseDto<string>.Success("User's Address added", " ",(int)HttpStatusCode.OK);
        }

        public async Task<ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>> GetAllAddressByAppUserId(string appUserId, int pageSize, int pageNumber)
        {
           try
            {
                var user = await _userManager.FindByIdAsync(appUserId);
                if (user == null)
                {
                    _logger.Information("User not found when trying to get address");
                    return ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>.Fail("User not found when trying to get address", (int)HttpStatusCode.NotFound);
                }

                var address = _unitOfWork.AddressRepository.GetAllAddressByAppUserId(appUserId);

                if (address == null)
                {
                    _logger.Information($"NO address found for user {user.Id}");
                    return ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>.Fail("No address found", (int)HttpStatusCode.NotFound);
                }

                var paginatedResult = await Paginator.PaginationAsync<Address, AddressResponseDTO>
                       (address, pageSize, pageNumber, _mapper);

                return ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>.Success("address found", paginatedResult, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ResponseDto<PaginationResult<IEnumerable<AddressResponseDTO>>>.Fail("No address found", (int)HttpStatusCode.NotFound);
            }
        }
    }
}
