using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class StoreViewModel : BaseViewModel
    {
        public StoreViewModel()
          {
             
              CountryList = new List<CountryViewModel>();
              StateList = new List<StateViewModel>();
              CityList = new List<CityViewModel>();
              ShopList = new List<StoreViewModel>();
              UserList = new List<UserViewModel>();
              location = new List<UserLocation>();
              CustomersList = new List< StoreViewModel>();
          }
       
        public List<CountryViewModel> CountryList { get; set; }
        public List<StateViewModel> StateList { get; set; }
        public List<CityViewModel> CityList { get; set; }
        public List<StoreViewModel> ShopList { get; set; }
        public List<UserViewModel> UserList { get; set; }
        public List<StoreViewModel> CustomersList { get; set; }


        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }

        [Display(Name = "Country")]
        public Int64? CountryId { get; set; }

        public string Countryname { get; set; }


        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Address Line 1")]
     
        public string AddressLine1 { get; set; }

     
        [Display(Name = "State")]
        public Int64? StateId { get; set; }
        public string State { get; set; }

        [Display(Name = "State (Other than US)")]
        public string StateName { get; set; }

        public Int64 Id { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public List<UserLocation> location { get; set; }

        [Display(Name = "Email Id (User Name)")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please Enter valid Email Id!")]
        public string EmailId { get; set; }

        public long Country { get; set; }

        [Display(Name = "Mobile No.")]        
        public string MobileNo { get; set; }


    }
}
