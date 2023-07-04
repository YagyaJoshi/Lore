using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class SizeViewModel : BaseViewModel
    {
        public SizeViewModel()
        {
            SizeList = new List<SizeViewModel>();
          
        }
        public Int64 SerialNo { get; set; }

        public Int64 ProductId { get; set; }
        public string StyleNumber { get; set; }
        public int DistributionPointId { get; set; }
        public Int64 CategoryId { get; set; }

        [Required]
        [Display(Name = "Size(US)")]
        public string SizeUS { get; set; }

        [Required]
        [Display(Name = "Size(UK)")]
        public string SizeUK { get; set; }

        [Required]
        [Display(Name = "Size(EU)")]
        public string SizeEU { get; set; }

        [Required]
        [Display(Name = "Size(CN)")]
        public string SizeCN { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }

        public Int64 Qty { get; set; }

        public Int64 USQty { get; set; }
        public Int64 UKQty { get; set; }

        public Int64 EUQty { get; set; }
        public Int64 CNQty { get; set; }

        public Int64 ReceivedQty { get; set; }

        public Int64 DispatchQty { get; set; }

        public Int64 POQty { get; set; }

        public Int64 USPOQty { get; set; }
        public Int64 UKPOQty { get; set; }

        public Int64 EUPOQty { get; set; }

        public Int64 CNPOQty { get; set; }
        public string POQtyStr { get; set; }

        public string COQtyStr { get; set; }
        public Int64 InStockQty { get; set; }

        public Int64 USInStockQty { get; set; }
        public Int64 UKInStockQty { get; set; }

        public Int64 EUInStockQty { get; set; }

        public Int64 CNInStockQty { get; set; }
        public List<SizeViewModel> SizeList { get; set; }
//public List<SizeViewModel> SerialNo { get; set; }
        public decimal OrderPrice { get; set; }

        public Int64 FinalQty { get; set; }
        public Int64 ColourId { get; set; }
        public string ColourName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
