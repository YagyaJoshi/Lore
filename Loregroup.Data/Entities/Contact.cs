using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Contact: DBaseEntity
    {        
        public Int64 Id { get; set; }
        public string OfficeName { get; set; }      
        public string Address { get; set; }
        public string Contactno { get; set; }
        public string Email { get; set; }
       
    }
}
