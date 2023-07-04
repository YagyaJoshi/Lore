using Loregroup.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class ProductCollectionViewModel
    {
        public ProductCollectionViewModel()
        {
            ProductList = new PagedData<ProductViewModel>();

            CollectionList = new List<CollectionYearViewModel>();
            ColourList = new List<ColourViewModel>();
            FabricList = new List<FabricViewModel>();
            CategoryList = new List<CategoryViewModel>();
            CutofdressList = new List<CutOfDressViewModel>();
        }

        public PagedData<ProductViewModel> ProductList { get; set; }

        public List<CollectionYearViewModel> CollectionList { get; set; }
        public List<ColourViewModel> ColourList { get; set; }
        public List<FabricViewModel> FabricList { get; set; }
        public List<CategoryViewModel> CategoryList { get; set; }
        public List<CutOfDressViewModel> CutofdressList { get; set; }

        [Display(Name="Collection Year")]
        public Int64 CollectionId { get; set; }
        [Display(Name = "Color")]
        public Int64 ColourId { get; set; }
        [Display(Name = "Fabric")]
        public Int64 FabricId { get; set; }
        [Display(Name = "Category")]
        public Int64 CategoryId { get; set; }
        [Display(Name = "Cut Of Dress")]
        public Int64 CutofdressId { get; set; }

        public Int64 Userid { get; set; }
    }
}
