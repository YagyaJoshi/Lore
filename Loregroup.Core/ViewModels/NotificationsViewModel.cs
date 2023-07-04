using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;

namespace Loregroup.Core.ViewModels
{
    public class NotificationsViewModel {
        public NotificationsViewModel() {
           // Storeviewmodel = new StoreMasterViewModel();
        }
        public long notid { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Int64 SenderId { get; set; }
        public Int64 ReceiverId { get; set; }
        public Int64 NotificationType { get; set; }
        public string NotificationMessage { get; set; }
        public Int64 PackageId { get; set; }
        public Int64 StatusId { get; set; }
        public String Date { get; set; }
        public string To { get; set; }
        public Int64 StoreId { get; set; }

       // public StoreMasterViewModel Storeviewmodel { get; set; }
    }

    public class NotificationViewModel : BaseViewModel
    {
        public NotificationViewModel() {
            
        }


        public String Name { get; set; }
        public String Description { get; set; }
        public Int64 SenderId { get; set; }

        public String Role { get; set; }
        public Int64 RoleId { get; set; }
        public NotificationTypes NotificationsType { get; set; }

        public String showdate { get; set; }


        public Int64 Id { get; set; }

        public UserBriefViewModel From { get; set; }
        public UserBriefViewModel To { get; set; }

        public String Message { get; set; }
        public int TaskPercent { get; set; }

        public DateTime Timestamp { get; set; }

        public Int64 ImageId { get; set; }
        public NotificationType NotificationType { get; set; }

        public String ActionUrl { get; set; }
        public HttpRequestType ActionUrlRequestType { get; set; }
    }

    public class NotificationGridViewModel : BaseViewModel
    {
        public Int64 SenderId { get; set; }
        public Int64 ReceiverId { get; set; }
        public string UniqueCode { get; set; }
        public Int64 NotificationType { get; set; }
        public string NotificationMessage { get; set; }
        public Int64 PackageId { get; set; }
        public Int64 RoleId { get; set; }
        public string notifyDate { get; set; }
    }

}