using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class TransferPOViewModel 
    {
       public TransferPOViewModel()
       {
           FromColorList = new List<ColourViewModel>(); 
           FromProductModel = new ProductViewModel();

           ToColorList = new List<ColourViewModel>();
           ToProductModel = new ProductViewModel();

           FromSizeList = new List<SizeViewModel>();
           ToSizeList = new List<SizeViewModel>();
           WareHouseList = new List<WareHouseViewModel>();
       }

       public string POReferenceNo { get; set; }
       public Int64 POId { get; set; }
       //public 
       public ProductViewModel FromProductModel { get; set; }
       public string FromProductName { get; set; }
       public Int64 FromProductId { get; set; }
       public Int64 FromColorId { get; set; }
       public Int64 FromSizeId { get; set; }
       public Int64 FromProductPODetailId { get; set; }
       public List<SizeViewModel> FromSizeList { get; set; }
       public List<ColourViewModel> FromColorList { get; set; }

       public ProductViewModel ToProductModel { get; set; }
       public string ToProductName { get; set; }
       public Int64 ToProductId { get; set; }
       public Int64 ToColorId { get; set; }
       public Int64 ToSizeId { get; set; }
       public List<SizeViewModel> ToSizeList { get; set; }
       public List<ColourViewModel> ToColorList { get; set; }

       public Int64 AvailableQty { get; set; }
       public Int64 TransferQty { get; set; }

       public Int64 WareHouseId { get; set; }
       public string WareHouseName { get; set; }
       public List<WareHouseViewModel> WareHouseList { get; set; }
       public Int64 CreatedById { get; set; }

        public Int64 ToWareHouseId { get; set; }

    }
}
