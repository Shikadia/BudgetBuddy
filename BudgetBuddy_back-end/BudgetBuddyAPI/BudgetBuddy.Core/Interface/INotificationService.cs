﻿using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface INotificationService
    {
        Task<bool> SendAsync(NotificationType type, NotificationContext context);
    }
}
