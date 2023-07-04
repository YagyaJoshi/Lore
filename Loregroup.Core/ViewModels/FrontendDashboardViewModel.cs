using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class FrontendDashboardViewModel
    {
        public int CompletedOrdersCount { get; set; }
        public int InprogressOrdersCount { get; set; }
        public int CancelledOrdersCount { get; set; }

        public string Fullname { get; set; }
        public string Shopname { get; set; }

        public string EmailId { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }

        public Int64 UserId { get; set; }
    }
}
