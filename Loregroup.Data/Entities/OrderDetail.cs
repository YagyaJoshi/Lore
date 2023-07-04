using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class OrderDetail : BaseEntity
    {
       public Int64 OrderMasterId { get; set; }
       public Int64 ProductId { get; set; }
       public Int64 SizeId { get; set; }
       public string SizeUK { get; set; }
       public decimal OrderPrice { get; set; }
       public Int64? ColourId { get; set; }
       public Int64 Qty { get; set; }
       public Int64 DispatchQty { get; set; }
    }
}
