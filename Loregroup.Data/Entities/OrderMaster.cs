using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
   public class OrderMaster : BaseEntity
    {
        public Int64 CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string ZipCode { get; set; }
        public string CurrencyName { get; set; }
        public string MobileNo { get; set; }
        public decimal ShippingCharge { get; set; }
        public bool TaxEnable { get; set; }
        public Int64 TotalProduct { get; set; }
        public string EmailId { get; set; }
        public string OrderNo { get; set; }
        public string OrderNote { get; set; }
        public string SystemNotes { get; set; }
        public bool IsPOPlaced { get; set; }
        public Int64 WareHouseId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string ShippingState { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public int OrderStatusId { get; set; }

        public DateTime? WearDate { get; set; }
        public string BridesName { get; set; }
        public DateTime? UserSelectDeliveryDate { get; set; }
        public string PONumber { get; set; }

        public decimal Rushfee { get; set; }

        public Int64 OrderlocatorId { get; set; }

        public decimal Extracharges { get; set; }

        public bool Isdeposit { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
