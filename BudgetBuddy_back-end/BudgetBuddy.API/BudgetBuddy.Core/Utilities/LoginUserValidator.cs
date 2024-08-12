using BudgetBuddy.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Utilities
{
    public class LoginUserValidator : AbstractValidator<LoginDTO>
    {
        public LoginUserValidator()
        {
            RuleFor(LoginDTO => LoginDTO.Email).NotEmpty().WithMessage("Email must not be empty").EmailAddress();
            RuleFor(LoginDTO => LoginDTO.Password).NotEmpty().WithMessage("password must not be empty").Password();
        }
    }
}
