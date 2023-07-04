using Loregroup.Core.Enumerations;
using Loregroup.Core.Exceptions;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Provider
{
    public class NotificationProvider : INotificationProvider
    {

        private readonly AppContext _context;

        public NotificationProvider(AppContext context)
        {
            _context = context;
        }

        public NotificationViewModel ToNotificationViewModel(Notification notification, int depth = 0)
        {
            return new NotificationViewModel()
            {
                Id = notification.Id,
                Name = notification.Name,
                Description = notification.Description,
                SenderId = notification.SenderId,
                CreatedById = notification.CreatedById,

                NotificationsType = (NotificationTypes)notification.NotificationType,
                Role = notification.Role.Name,
                RoleId = notification.RoleId,
                //                NotificationType = notification.NotificationType,
                StatusId = notification.StatusId,
                Status = (Status)notification.StatusId,
                ModifiedById = notification.ModifiedById,
                ModificationDate = notification.ModificationDate,
                CreationDate = notification.CreationDate,
                showdate = notification.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")
                //                NotificationDate = notification.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")
            };
        }

        public List<NotificationViewModel> GetAllNotifications(Status? status, int page = 0, int records = 0)
        {
            try
            {
                var notificationPredicate = PredicateBuilder.True<Notification>();

                if (status != null)
                {
                    notificationPredicate.And(x => x.StatusId == (int)status);
                }
                if (page > 0)
                {
                    return _context.Notifications.Where(notificationPredicate).Skip(page * records).Take(records).Take(3)
                        .OrderByDescending(t => t.Id)
                        .ToList()
                        .Select(ToNotificationViewModel)
                        .ToList();
                }

                return _context.Notifications.Where(x => x.StatusId == (int)Status.Active).Take(3)
                    .OrderByDescending(t => t.Id)
                    .ToList()
                    .Select(ToNotificationViewModel)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new NotificationNotGettingException();
            }
        }


        public NotificationsViewModel GetNotifications(Status? status, int page = 0, int records = 0)
        {
            try
            {
                var notificationPredicate = PredicateBuilder.True<Notification>();

                if (status != null)
                {
                    notificationPredicate.And(x => x.StatusId == (int)status);
                }
                if (page > 0)
                {
                    
                }
               
                NotificationsViewModel abc = new NotificationsViewModel();
                return abc;
            }
            catch (Exception ex)
            {
                throw new NotificationNotGettingException();
            }
        }
    }
}