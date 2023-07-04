using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{  
   public class ColourViewModel : BaseViewModel
   {
       public ColourViewModel()
       {
           ColourList = new List<ColourViewModel>();
       }

       [Required]
       [Display(Name = "Color")]
       public string Colour { get; set; }

       
       [Display(Name = "Description")]
       public string Description { get; set; }

       public string Edit { get; set; }
       public string Delete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<ColourViewModel> ColourList { get; set; }
   }
}
