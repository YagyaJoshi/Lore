using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class RefillRate :BaseEntity
    {

       public decimal OldRefillRate { get; set; }
       public decimal NewRefillRate { get; set; }
       public DateTime  DateFrom {get;set;}
       public Int64 PackageId { get; set; }
      
           
    }
}
