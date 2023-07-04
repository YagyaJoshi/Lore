using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface INotificationProvider
    {
        NotificationViewModel ToNotificationViewModel(Notification notification, int depth = 0);
        List<NotificationViewModel> GetAllNotifications(Status? status, int page = 0, int records = 0);
        NotificationsViewModel GetNotifications(Status? status, int page = 0, int records = 0);

    }
}
