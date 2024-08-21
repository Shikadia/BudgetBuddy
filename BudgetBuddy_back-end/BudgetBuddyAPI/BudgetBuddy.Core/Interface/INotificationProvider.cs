using BudgetBuddy.Core.Utilities;

namespace BudgetBuddy.Core.Interface
{
    public interface INotificationProvider
    {
        Task<bool> SendAsync(NotificationContext context);
    }
}
