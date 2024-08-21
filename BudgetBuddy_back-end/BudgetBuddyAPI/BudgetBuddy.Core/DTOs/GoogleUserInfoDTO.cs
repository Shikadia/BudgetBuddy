using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.DTOs
{
    public class GoogleUserInfoDTO
    {
        public string Sub { get; set; } // Google user ID
        public string Name { get; set; }
        public string Given_Name { get; set; }
        public string Family_Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public bool Email_Verified { get; set; }
        public string Locale { get; set; }
    }

  

}
