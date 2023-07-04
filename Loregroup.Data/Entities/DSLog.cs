using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class DSLog :BaseEntity
    {
       public string Message { get; set; }

       public string Number { get; set; }

       public DateTime TimeStamp { get; set; }
    }
}
