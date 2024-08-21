using BudgetBuddy.Core.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Utilities
{
    public class NotificationContext
    {
        public string Address { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Payload { get; set; } = null!;
        public NotificationSettings NotificationSettings { get; set; } = null!;
    }
}
