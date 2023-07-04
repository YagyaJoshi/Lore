using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class TaxViewModel : BaseViewModel
    {
            public TaxViewModel()
           {
               TaxList = new List<TaxViewModel>();
           }
           
           [Required]
           [Display(Name = "Tax(%)")]
           public decimal TaxPercentage { get; set; }

           [Display(Name = "Description")]
           public string Description { get; set; }

           public string Edit { get; set; }
           public string Delete { get; set; }
    
        public List<TaxViewModel> TaxList { get; set; }
    }
}
