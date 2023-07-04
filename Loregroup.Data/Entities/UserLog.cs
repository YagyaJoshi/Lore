using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class UserLog : BaseEntity
    {
        public Int64 RoleId { get; set; }
        public string Module { get; set; }
        public string MessageLog { get; set; }
    }
}
