using AutoMapper;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Domain.Models;

namespace BudgetBuddy.Core.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddAddressRequestDTO, Address>();
            CreateMap<Address, AddressResponseDTO>().ReverseMap();
        }
    }
}
