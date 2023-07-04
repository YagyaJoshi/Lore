using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class CityViewModel : BaseViewModel
    {
        //Add By 
       public CityViewModel()
        {
            StateViewModel = new StateViewModel();
            CityList = new List<CityViewModel>();
            DistrictsViewModel = new DistrictsViewModel();
            CountryList = new List<CountryViewModel>();
        }
       // public Int64 Id { get; set; }
        [Display(Name = "State")]
        [Required(ErrorMessage = "Please Select State")]
        public List<StateViewModel> StateList { get; set; }

        public string StateName { get; set; }

        [Required]
        [Display(Name = "State")]
        public Int64 StateId { get; set; }

       [Display(Name = "City")]
       [Required(ErrorMessage = "Please Enter City Name")]
        public string Cityname { get; set; }
       public bool checkbox { get; set; }
       public string Education { get; set; }
       public string OtherEducation { get; set; }
       
       public StateViewModel StateViewModel { get; set; }
       public DistrictsViewModel DistrictsViewModel { get; set; }

       public List<CityViewModel> CityList { get; set; }
       public int? StatusId { get; set; }
       [Display(Name = "District")]
       public Int64 DistricId { get; set; }
       public string DistrictName { get; set; }

       public List<CountryViewModel> CountryList { get; set; }
       [Required]
       [Display(Name = "Country")]
       public Int64 CountryId { get; set; }

       public string CountryName { get; set; }


       public string Edit { get; set; }
       public string Delete { get; set; }
    }
   
   public class CityViewModels
   {
       public List<CityViewModel> Cities { get; set; }
   }
}
