using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.DTOs
{
    public class RefreshTokenRequestDTO
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
