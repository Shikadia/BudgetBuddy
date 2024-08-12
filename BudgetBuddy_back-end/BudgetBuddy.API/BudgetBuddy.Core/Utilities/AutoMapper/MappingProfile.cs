using AutoMapper;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
