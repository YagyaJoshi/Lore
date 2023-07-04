using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public OrderViewModel()
        {
            OrderList = new List<OrderViewModel>();
            CustomerModel = new CustomerViewModel();
            StyleList = new List<StyleViewModel>();
            ColourList = new List<ColourViewModel>();
            SizeList = new List<SizeViewModel>();
            ProductModel = new ProductViewModel();
            ProductList = new List<ProductListModel>();
            ProductListModel = new ProductListModel();
            POmodel = new PurchaseOrderViewModel();
            DetailsList = new List<OrderDetailViewModel>();
            AccountList = new List<AccountDetailsViewModel>();
            ProductImageList = new List<ProductImage>();
            OrderlocatorList = new List<OrderLocatorViewModel>();
        }

        public List<AccountDetailsViewModel> AccountList { get; set; }
        public List<OrderViewModel> OrderList { get; set; }
        public ProductViewModel ProductModel { get; set; }
        public CustomerViewModel CustomerModel { get; set; }

        public List<ProductImage> ProductImageList { get; set; }

        [Display(Name = "Order Notes")]
        public string Notes { get; set; }

        [Display(Name = "System Notes (Only For Internal Use)")]
        public string SystemNotes { get; set; }

        [Display(Name = "Order Date")]
        public string OrderDate { get; set; }

        public string OrderDateString { get; set; }

        [Display(Name = "Due Date")]
        public string DeliveryDate { get; set; }

        [Display(Name = "Order No.")]
        public string OrderNo { get; set; }

        [Display(Name = "Shipping Charge")]
        public decimal ShippingCharge { get; set; }

        
        [Display(Name = "Rush Fee")]
        public decimal Rushfee { get; set; }

        [Display(Name = "Extra Charges")]
        public decimal Extracharges { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        [Display(Name = "Tax Enable")]
        public bool TaxEnable { get; set; }

        [Display(Name = "Tax")]
        public decimal Tax { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [Display(Name = "Order Note")]
        public string OrderNote { get; set; }

        public bool USD { get; set; }
        public bool EURO { get; set; }
        public bool GBP { get; set; }

        [Display(Name = "Quantity")]
        public Int64 Quantity { get; set; }

        public List<ColourViewModel> ColourList { get; set; }
        [Display(Name = "Colour")]
        public Int64 ColourId { get; set; }

        public List<StyleViewModel> StyleList { get; set; }
        [Display(Name = "Style")]
        public Int64 StyleId { get; set; }

        public List<SizeViewModel> SizeList { get; set; }
        [Display(Name = "Size")]
        public Int64 SizeId { get; set; }

        //[Display(Name = "Quantity")]
        //public string Quantity { get; set; }
        public string SingleProductQtyList { get; set; }
        public string SizeQtyList { get; set; }

        public List<ProductListModel> ProductList { get; set; }

        public ProductListModel ProductListModel { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalAmount { get; set; }


        public string PrintPreview { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }

        public Int64 TotalProducts { get; set; }

        public Int64 TotalOrderCount { get; set; }

        public int TotalItem { get; set; }

        public decimal Totamount { get; set; }

        public decimal GrandTotamount { get; set; }

        public bool IsPOPlaced { get; set; }

        public PurchaseOrderViewModel POmodel { get; set; }

        public List<OrderDetailViewModel> DetailsList { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus OrderStatusId { get; set; }

        public Int64 DistributionPoint { get; set; }

        public bool IsDispatchedAll { get; set; }

        public string OrderStatus { get; set; }
        public string StatusColor { get; set; }

        [Display(Name = "Wear Date")]
        public string WearDate { get; set; }
        public string WearDateString { get; set; }

        [Display(Name = "Delivery Date")]
        public string UserSelectDeliveryDate { get; set; }
        public string UserSelectDeliveryDateString { get; set; }
  

        [Display(Name = "Bride's Name")]
        public string BridesName { get; set; }

        [Display(Name = "Purchase Order Number")]
        public string PurchaseOrderNumber { get; set; }

        //public string ImagePath { get; set; }
         public bool IsSelected { get; set; }

        public Int64 OrderlocatorId { get; set; }
        public string OrderlocatorName { get; set; }
        public List<OrderLocatorViewModel> OrderlocatorList { get; set; }

        public int DepositId { get; set; }

        public bool IsPayment { get; set; }

        public bool issuperadmin { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class ProductListModel
    {
        public ProductListModel()
        {
            SizeModel = new List<SizeViewModel>();
            StockSizeModel = new List<List<SizeViewModel>>();
        }
        public ProductViewModel Product { get; set; }
        public List<SizeViewModel> SizeModel { get; set; }
        public List<List<SizeViewModel>> StockSizeModel { get; set; }
        public string SizeModelString { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProductImage
    {
        public Int64 ProductId { get; set; }
        public string ImagePath { get; set; }
        public string ImagePath11 { get; set; }
        public string ImagePath12 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImagePath4 { get; set; }
        public bool IsSelected { get; set; }
        public string ImagePath2 { get; set; }
    }
}
