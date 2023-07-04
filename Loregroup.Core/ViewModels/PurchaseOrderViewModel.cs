using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Loregroup.Core.ViewModels
{
    public class PurchaseOrderViewModel : BaseViewModel
    {
        public PurchaseOrderViewModel()
        {
            ProductList = new List<ProductListModel>();
            ColourList = new List<ColourViewModel>();
            ProductModel = new ProductViewModel();
            CustomerModel = new CustomerViewModel();
            WareHouseList = new List<WareHouseViewModel>();
            ProductListModel = new ProductListModel();
            DetailsList = new List<PurchaseOrderDetailViewModel>();
            StockPaging = new List<SelectListItem>();
            CollectionYearList = new List<CollectionYearViewModel>();
            CategoryList = new List<CategoryViewModel>();
            CustomerList = new List<CustomerViewModel>();
          
        }

        public ProductViewModel ProductModel { get; set; }
        public CustomerViewModel CustomerModel { get; set; }

        public List<CustomerViewModel> CustomerList { get; set; }
        public Int64? CustomerId { get; set; }

        public List<CategoryViewModel> CategoryList { get; set; }
        public Int64? CategoryId { get; set; }

        public List<CollectionYearViewModel> CollectionYearList { get; set; }
        [Display(Name = "Collection Year")]        
        public Int64? CollectionYearId { get; set; }

        public Int64 WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public List<WareHouseViewModel> WareHouseList { get; set; }


       

        public Int64 SupplierId { get; set; }
        public string SupplierName { get; set; }

        public string Address { get; set; }

        public Int64 CurrncyId { get; set; }
        public string CurrencyName { get; set; }

        //public Int64 OrderId { get; set; }

        public string OrderDate { get; set; }

        public string DeliveryDate { get; set; }

        public string OrderDateString { get; set; }

        public string OrderNote { get; set; }

        [Display(Name = "Order Refrence")]
        public string OrderRefrence { get; set; }
        
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

        public List<ProductListModel> ProductList { get; set; }

        public ProductListModel ProductListModel { get; set; }

        public string SingleProductQtyList { get; set; }
        public string SizeQtyList { get; set; }

        [Display(Name = "Purchase Order Status")]
        public PurchaseOrderStatus PurchaseOrderStatusId { get; set; }

        public List<PurchaseOrderDetailViewModel> DetailsList { get; set; }

        public string CustomerFullName { get; set; }

        public bool IsDataAvailable { get; set; }

        [Display(Name = "Send Mail")]
        public bool SendMail { get; set; }
                
        public string ImgUrl1 { get; set; }
        public string ImgUrl2 { get; set; }
        public string ImgUrl3 { get; set; }
         
        public string PrintPreview { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }

        public Int64 DistributionPoint { get; set; }
        public string InsertFrom { get; set; }

        public bool IsReceivedAll { get; set; }

        public List<SelectListItem> StockPaging { get; set; }
        public string PageValue { get; set; }


        public int TotalItem { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class Getpodetail
    {
        public Getpodetail()
        {
            Quantitylist = new List<CustomerorderQuantityDetail>();
        }
        public string ColorName { get; set; }
        public string Styleno { get; set; }

        public string Wharehouse { get; set; }
        public bool IsDataAvailable { get; set; }

        public  List<CustomerorderQuantityDetail> Quantitylist { get; set; }

    }
    public class CustomerorderQuantityDetail
    {
        public string Customername { get; set; }
        public string OrderNo { get; set; }

        public Int64 Quantity { get; set; }
    }
   
}
