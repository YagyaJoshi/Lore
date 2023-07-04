using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class PurchaseOrderMaster :BaseEntity
    {
        public Int64 WareHouseId { get; set; }
        public Int64 SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string MobileNo { get; set; }
        public string OrderNote { get; set; }
        public string EmailId { get; set; }
       // public Int64 CurrncyId { get; set; }
        public string CurrencyName { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string OrderRefrence { get; set; }       
        public decimal TotalAmount { get; set; }
        public Int64 TotalProduct { get; set; }
        public Int64 POStatusId { get; set; }
        public string Address { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
