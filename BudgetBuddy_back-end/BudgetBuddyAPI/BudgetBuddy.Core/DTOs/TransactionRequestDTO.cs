using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.DTOs
{
    public class TransactionRequestDTO
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount {  get; set; }
        public string Tag { get; set; }
    }
}
