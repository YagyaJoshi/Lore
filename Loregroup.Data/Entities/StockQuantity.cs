using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class StockQuantity : BaseEntity
    {

        public Int64 ReferenceId { get; set; }
        public string InventoryType { get; set; }
        public Int64 ProductId { get; set; }
        public string SizeUK { get; set; }
        public Int64 ColourId { get; set; }
        public Int64 WareHouseId { get; set; }
        public Int64 Qty { get; set; }
        public Int64 ReceivedQty { get; set; }
        public Int64 DispatchedQty { get; set; }

    }
}
