using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{    
    public class CountryViewModel : BaseViewModel
    {
        public CountryViewModel()
        {
            CountryList = new List<CountryViewModel>();
            CurrencyList = new List<CountryViewModel>();
        }


        //public Int64 Id { get; set; }
        [Required(ErrorMessage = "Please Enter Country Name")]
        [Display(Name = "Country Name")]
        public string countryName { get; set; }
        [Required(ErrorMessage = "Please Enter Currency Name")]
        [Display(Name = "Currency Name")]
        public string currencyName { get; set; }
        [Required(ErrorMessage = "Please Enter Currency Symbol")]
        [Display(Name = "Currency Symbol")]
        public string currencySymbol { get; set; }

        public List<CountryViewModel> CountryList { get; set; }
        public List<CountryViewModel> CurrencyList { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
    }
}
