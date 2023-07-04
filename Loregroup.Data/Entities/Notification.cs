using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Notification :BaseEntity
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Int64 SenderId { get; set; }
        public Int64 ReceiverId { get; set; }
        public Int64 NotificationType { get; set; }
        public string NotificationMessage { get; set; }
        public Int64 PackageId { get; set; }
        /// <summary>
        /// based on enum NotificationTypes
        /// </summary>
        //public Int64 NotificationType { get; set; }
        public Int64 RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
