using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.DTOs
{
    public class RefreshTokenResponseDTO
    {
        public string NewAccessToken { get; set; }
        public string NewRefreshToken { get; set; }
    }
}
