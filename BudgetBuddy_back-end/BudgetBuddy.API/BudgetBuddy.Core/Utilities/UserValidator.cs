using BudgetBuddy.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Utilities
{
    public class UserValidator : AbstractValidator<SignUpDTO>
    {
        public UserValidator()
        {
            RuleFor(RegistrationDTO => RegistrationDTO.Password)
                .Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Passwords do not match");
            RuleFor(RegistrationDTO => RegistrationDTO.FirstName)
                .HumanName();
            RuleFor(RegistrationDTO => RegistrationDTO.LastName)
                .HumanName();
            RuleFor(RegistrationDTO => RegistrationDTO.PhoneNumber)
                .PhoneNumber();
        }
    }
}
