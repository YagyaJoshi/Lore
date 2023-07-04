using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Permission:BaseEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        //public virtual ICollection<Role> Roles { get; set; }
    }
}
