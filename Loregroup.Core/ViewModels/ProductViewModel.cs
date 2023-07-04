using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        public ProductViewModel()
        {
            ProductList = new List<ProductViewModel>();
            ColourList = new List<ColourViewModel>();
            BrandList = new List<BrandViewModel>();
            CutOfDressList = new List<CutOfDressViewModel>();
            CollectionYearList = new List<CollectionYearViewModel>();
            CategoryList = new List<CategoryViewModel>();
            FabricList = new List<FabricViewModel>();
            SizeList = new List<SizeFrontendViewModel>();
            ProductAvailabilityList = new List<List<SizeViewModel>>();
            SampleColourList = new List<ColourViewModel>();
        }

        [Display(Name = "Style No.")]
        [Required(ErrorMessage = "Please Enter Style No.")]
        public string ProductName { get; set; }


        public string ProductSalesbyName { get; set; }
        [Display(Name = "Product Title")]
        [Required(ErrorMessage = "Please Enter Product Title")]
        public string Title { get; set; }

        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "Please Enter Product Description")]
        public string Description { get; set; }

        public List<BrandViewModel> BrandList { get; set; }
        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Please Select Brand")]
        public Int64? BrandId { get; set; }

        public string BrandName { get; set; }

        public List<ColourViewModel> ColourList { get; set; }
        [Display(Name = "Color")]
        [Required(ErrorMessage = "Please Select Colour")]
        public Int64? ColourId { get; set; }

        public string ColourName { get; set; }


        public List<ColourViewModel> SampleColourList { get; set; }
        [Display(Name = "Sample Colour")]
        [Required(ErrorMessage = "Please Select Sample Colour")]
        public Int64? SampleColourId { get; set; }

        public string SampleColourName { get; set; }

        public List<CutOfDressViewModel> CutOfDressList { get; set; }
        [Display(Name = "Cut Of Dress")]
        [Required(ErrorMessage = "Please Select Cut Of Dress")]
        public Int64? CutOfDressId { get; set; }
        public string CutOfDress { get; set; }

        public List<CollectionYearViewModel> CollectionYearList { get; set; }
        [Display(Name = "Collection Year")]
        [Required(ErrorMessage = "Please Enter Collection Year")]
        public Int64? CollectionYearId { get; set; }

        public string CollectionYear { get; set; }

        [Display(Name = "Style No.")]
        [Required(ErrorMessage = "Please Enter Product Style Number")]
        public string Style { get; set; }

        [Display(Name = "Fabric")]
        [Required(ErrorMessage = "Please Select Fabric Type")]
        public Int64? FabricId { get; set; }

        public string FabricName { get; set; }               
             
        [Display(Name = "Price($)")]
        [Required(ErrorMessage = "Please Enter Price In USD")]
        public decimal PriceUSD { get; set; }

        [Display(Name = "Purchase Price")]
        [Required(ErrorMessage = "Please Enter Purchase Price")]
        public string PurchasePrice { get; set; }

        [Display(Name = "Price(€)")]
        [Required(ErrorMessage = "Please Enter Price In EURO")]
        public decimal PriceEURO { get; set; }

        [Display(Name = "Price(£)")]
        [Required(ErrorMessage = "Please Enter Price In GBP")]
        public decimal PriceGBP { get; set; }

        public decimal CustomerPrice { get; set; }
        
        public decimal OrderPrice { get; set; }
        
        [Display(Name = "Picture 1")]
        public string Picture1 { get; set; }

        [Display(Name = "Picture 2")]
        public string Picture2 { get; set; }

        [Display(Name = "Picture 3")]
        public string Picture3 { get; set; }

        [Display(Name = "Picture 4")]
        public string Picture4 { get; set; }

        [Display(Name = "Video Link Image")]
        public string VideoImage { get; set; }

        [Display(Name = "Video Link (vimeo)")]
        public string VideoLink { get; set; }

        public bool Publish { get; set; }

        public Int64? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }

        public string SeletedBrandIds { get; set; }
        public string SeletedBrandNames { get; set; }

        public string SeletedColorIds { get; set; }
        public string SeletedColorNames { get; set; }

        public string SeletedCategoryIds { get; set; }
        public string SeletedCategoryNames { get; set; }

        public string SelectedFabricIds { get; set; }
        public string SelectedFabricNames { get; set; }

        [Display(Name = "Publish Type")]
        [Required(ErrorMessage = "Please Select Publish Type")]
        public int PublishId { get; set; }

        public string PublishString { get; set; }

        public List<ProductViewModel> ProductList { get; set; }
        public List<CategoryViewModel> CategoryList { get; set; }
        public List<FabricViewModel> FabricList { get; set; }
        public List<SizeFrontendViewModel> SizeList { get; set; }
        public string styleset { get; set; }

        public int SizeUk { get; set; }

        public Int64 FrontendLoginUserId { get; set; }
        public List<List<SizeViewModel>> ProductAvailabilityList { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public int DistributionId { get; set;}
    }


    public class SizeFrontendViewModel
    {
        public int Id { get; set; }
        public int SizeUk { get; set; }
        public string SizeString { get; set; }
    }

    
}
