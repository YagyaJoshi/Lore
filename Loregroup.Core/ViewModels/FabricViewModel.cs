using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class FabricViewModel : BaseViewModel
    {
        public FabricViewModel()
       {
           FabricList = new List<FabricViewModel>();
       }

       [Required]
       [Display(Name = "Fabric")]
       public string FabricName { get; set; }

       [Display(Name = "Description")]
       public string Description { get; set; }

       public string Edit { get; set; }
       public string Delete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<FabricViewModel> FabricList { get; set; }
    }
}
