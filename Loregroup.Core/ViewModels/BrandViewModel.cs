using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class BrandViewModel : BaseViewModel
    {
        public BrandViewModel() 
        {
            BrandList = new List<BrandViewModel>();
        }

        [Required]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        
        [Display(Name = "Description")]
        public string Description { get; set; }
       
        public string Edit { get; set; }
        public string Delete { get; set; }

        public List<BrandViewModel> BrandList { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
