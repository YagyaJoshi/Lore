using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class PermissionMatrix:BaseEntity
    {
       public Int64 PermissionId { get; set; }
       public Int64 RoleId { get; set; }
       public bool PermissionStatus { get; set; }

       public String TypeHold { get; set; }
       public Int64 AllNAvigationsId { get; set; }

     
       public virtual Role Role { get; set; }
    }
}
