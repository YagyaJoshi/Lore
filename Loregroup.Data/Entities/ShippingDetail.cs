using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class ShippingDetail: BaseEntity
    {
        public Int64 MasterUsersId {get; set;}
        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingEmailId { get; set; }
        public string ShopName { get; set; }
        public string ShippingMobileNo { get; set; }

        public string ShippingAddressLine1 { get; set; }
        public string ShippingAddressLine2 { get; set; }
        public string ShippingCompany { get; set; }
        public string ShippingZipCode { get; set; }
        public Int64 ShippingCountryId { get; set; }
        public Int64 ShippingStateId { get; set; }
        public string ShippingStateName { get; set; }

        public string ShippingCity { get; set; }
        public string ShippingFax { get; set; }
        public string ShippingWorkphone { get; set; }
              
    }
}
