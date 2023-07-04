using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class StoreLocatorViewModel
    {
        public StoreLocatorViewModel()
        {
            CountryList = new List<CountryViewModel>();
            StateList = new List<StateViewModel>();
            CustomerList = new List<CustomerViewModel>();
            MapList = new List<UserLocation>();
        }

        public List<CountryViewModel> CountryList { get; set; }
        public List<StateViewModel> StateList { get; set; }
        public List<CustomerViewModel> CustomerList { get; set; }
        public List<UserLocation> MapList { get; set; }

        [Display(Name = "Country")]
        public Int64 CountryId { get; set; }
        public string CountryName { get; set; }

        [Display(Name = "State")]
        public Int64 StateId { get; set; }
        public string StateName { get; set; }

        public string Zipcode { get; set; }

    }


    public class UserLocationViewModel
    {
        public string UserName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Statename { get; set; }
        public Int64 CountryId { get; set; }
    }


}
