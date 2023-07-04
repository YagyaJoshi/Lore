using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces.Utilities;

namespace Loregroup.Core.ViewModels
{
    public class BaseViewModel {
        public BaseViewModel() {
            IUtilities utility = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUtilities>();
            CreatedById = 0;
            CreationDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            ModifiedById = 0;
            ModificationDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Status = (Status) StatusId;
            StatusId = (int) Status;
            //StatusId = 1;
        }

        public Int64 Id { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public Int64 CreatedById { get; set; }
        public Int64 ModifiedById { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}