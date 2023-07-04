using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class PurchaseOrderDetail : BaseEntity
    {
       public Int64 PurchaseOrderMasterId { get; set; }
       public Int64 ProductId { get; set; }
       public Int64 SizeId { get; set; }
       public string SizeUK { get; set; }
       public Int64? ColourId { get; set; }
       public Int64 Qty { get; set; }
       public Int64 ReceivedQty { get; set; }
       public bool IsInventoryAdded { get; set; }
       
    }
}
