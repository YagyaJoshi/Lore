using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities {
    
    public class Role:BaseEntity {
        /// <summary>
        /// based on enum UserRole
        /// </summary>
        public String Name { get; set; }
        public int StatusId { get; set; }
       // public virtual ICollection<Permission> Permissions { get; set; }
    }
}
