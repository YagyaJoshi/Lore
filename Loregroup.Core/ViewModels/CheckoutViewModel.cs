using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            ProductList = new List<ProductViewModel>();
            UserModel = new CustomerViewModel();
            CartList = new List<TempCartViewModel>();
        }

        [Display(Name = "please type your notes here and requested delivery date")]
        public string OrderNotes { get; set; }

        public List<ProductViewModel> ProductList { get; set; }
        public CustomerViewModel UserModel { get; set; }
        public List<TempCartViewModel> CartList { get; set; }
        public Int64 OrderMasterId { get; set; }
        public string OrderNo { get; set; }

        [Display(Name = "Wear Date")]
        public string WearDate { get; set; }
        public string WearDateString { get; set; }

        [Display(Name = "Delivery Date")]
        public string DeliveryDate { get; set; }
        public string DeliveryDateString { get; set; }

        [Display(Name = "Bride's Name")]
        public string BridesName { get; set; }

        [Display(Name = "Purchase Order Number")]
        public string PurchaseOrderNumber { get; set; }


        // Neww Added by sulabh
        public string Countrynme { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal ShippingCharge { get; set; }
        public decimal GrandTotalAmount { get; set; }
    }
}
