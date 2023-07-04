using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class FrontendHomeViewModel
    {
        public FrontendHomeViewModel()
        {
            CategoryList = new List<CategoryViewModel>();
            ProductList = new List<ProductViewModel>();
            SlidersList = new List<SliderViewModel>();
        }
        
        public List<CategoryViewModel> CategoryList { get; set; }
        public List<ProductViewModel> ProductList { get; set; }
            public List<SliderViewModel> SlidersList { get; set; }
    }
}
