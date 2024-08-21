using BudgetBuddy.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface IEmailService
    {
        Task<ResponseDto<bool>> SendEmail(EmailDTO model);
    }
}
