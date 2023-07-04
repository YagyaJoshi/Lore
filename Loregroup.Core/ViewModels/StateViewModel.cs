using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class StateViewModel
    {
        public StateViewModel()
        {
            StateList = new List<StateViewModel>();
            CountryList = new List<CountryViewModel>();
        }
        public Int64 Id { get; set; }

       [Display(Name = "State name")]
       [Required(ErrorMessage = "Please Enter State Name")]
       public string Statename { get; set; }

       public List<StateViewModel> StateList { get; set; }
       public List<CountryViewModel> CountryList { get; set; }

       [Required]
       [Display(Name = "Country")]
       public Int64 CountryId { get; set; }

       public string Countryname { get; set; }

       public string Edit { get; set; }

       public string Delete { get; set; }

       public int? StatusId { get; set; }
      
    }
}
