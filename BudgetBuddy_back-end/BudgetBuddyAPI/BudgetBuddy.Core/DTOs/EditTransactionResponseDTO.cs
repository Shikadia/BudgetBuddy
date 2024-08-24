using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.DTOs
{
    public class EditTransactionResponseDTO
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string CategoryOrTag { get; set; }
        public string Description { get; set; }
    }
}
