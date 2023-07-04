using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class AddressViewModel : BaseViewModel
    {
        public AddressViewModel()
        {
            CustomerList = new List<CustomerViewModel>();
            ShippingModel = new ShippingViewModel();
            AddressList = new List<AddressViewModel>();
            CountryList = new List<CountryViewModel>();
            StateList = new List<StateViewModel>();
        }

        public List<CustomerViewModel> CustomerList { get; set; }
        public ShippingViewModel ShippingModel { get; set; }
        public List<AddressViewModel> AddressList { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please Enter Address")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Display(Name = "Email Id")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please Enter valid Email Id!")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        public string EmailId { get; set; }

        public List<CountryViewModel> CountryList { get; set; }
        [Display(Name = "Country")]
        public Int64? CountryId { get; set; }
        public string Country { get; set; }

        public List<StateViewModel> StateList { get; set; }
        [Display(Name = "State")]
        public Int64? StateId { get; set; }
        public string State { get; set; }

        [Display(Name = "State (Other than US)")]
        public string StateName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }



        //For Different Shipping Address

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string ShippingFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string ShippingLastName { get; set; }

        [Display(Name = "Zip Code")]
        public string ShippingZipCode { get; set; }

        [Display(Name = "Fax")]
        public string ShippingFax { get; set; }

        [Display(Name = "Address Line 1")]
        public string ShippingAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string ShippingAddressLine2 { get; set; }

        [Display(Name = "Country")]
        public Int64? ShippingCountryId { get; set; }
        public string ShippingCountry { get; set; }

        [Display(Name = "State")]
        public Int64? ShippingStateId { get; set; }
        public string ShippingState { get; set; }

        [Display(Name = "State (Other than US)")]
        public string ShippingStateName { get; set; }

        [Display(Name = "City")]
        public string ShippingCity { get; set; }

        [Display(Name = "WorkPhone")]
        public string ShippingTelephoneNo { get; set; }

        [Display(Name = "Mobile No.")]
        public string ShippingMobileNo { get; set; }

        [Display(Name = "Email Id")]
        public string ShippingEmailId { get; set; }


    }
}
