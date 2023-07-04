using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel
    {
        public Int64 OrderMasterId { get; set; }
        public Int64 ProductId { get; set; }
        public string ProductName { get; set; }
        public Int64 SizeId { get; set; }
        public string SizeUK { get; set; }
        public Int64? ColourId { get; set; }
        public string ColourName { get; set; }
        public Int64 Qty { get; set; }
        public Int64 DispatchedQty { get; set; }
        public Int64 AvailableQty { get; set; }

        public string SizeUS { get; set; }

        public string SizeEU { get; set; }

    }
}
