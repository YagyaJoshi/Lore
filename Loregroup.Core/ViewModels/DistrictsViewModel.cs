using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
     public class DistrictsViewModel
    {
         public DistrictsViewModel()
        {
            StateViewModel = new StateViewModel();
            DistrictList = new List<DistrictsViewModel>();
        }
        public Int64 Id { get; set; }


        [Display(Name = "State")]
        [Required(ErrorMessage = "Please Select State")]
        public Int64 StateId { get; set; }

       [Display(Name = "Distric Name")]
       [Required(ErrorMessage = "Please Enter Distric Name")]
        public string DistrictName { get; set; }
      
       
       public StateViewModel StateViewModel { get; set; }
       public List<DistrictsViewModel> DistrictList { get; set; }
       public int? StatusId { get; set; }
    }
}
