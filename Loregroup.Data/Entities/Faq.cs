using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{

   public class Faq: DBaseEntity
    {        
        public Int64 Id { get; set; }
        public string Question { get; set; }      
        public string Answer { get; set; }
       
    }
    
    
}
