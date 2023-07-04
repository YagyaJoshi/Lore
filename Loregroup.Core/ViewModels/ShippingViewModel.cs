using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class ShippingViewModel : BaseViewModel
    {
        public ShippingViewModel()
        {
            CustomerList = new List<CustomerViewModel>();
            CountryList = new List<CountryViewModel>();
            StateList = new List<StateViewModel>();
            CityList = new List<CityViewModel>();
            RoleList = new List<RoleViewModel>();
            location = new List<UserLocation>();
        }

        public Int64 MasterUsersId { get; set; }

        [Display(Name = "First Name")]
       [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "User Name")]
    //    [Required(ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
       // [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        public string Keyword { get; set; }

        public string Salt { get; set; }
        
        [Display(Name = "Last Name")]
       [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        public string CustomerFullName { get; set; }

        //[RegularExpression(@"\d+(\.\d{1,20})?", ErrorMessage = "Invalid Latitude")]
        //[RegularExpression(@"^(\-?\d+(\.\d+)?),\s*(\-?\d+(\.\d+)?)$", ErrorMessage = "Invalid Latitude")]
        public string Latitude { get; set; }

        //[RegularExpression(@"\d+(\.\d{1,20})?", ErrorMessage = "Invalid Longitude")]
        //[RegularExpression(@"^(\-?\d+(\.\d+)?),\s*(\-?\d+(\.\d+)?)$", ErrorMessage = "Invalid Longitude")]
        public string Longitude { get; set; }

        [Display(Name = "Customer Type")]
       // [Required(ErrorMessage = "Please Select Customer Type")]
        public CustomerType CustomerType { get; set; }

        [Display(Name = "Shop Name")]
        [Required(ErrorMessage = "Please Enter ShopName")]
        public string ShopName { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please Enter CompanyName")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Reg/Tax Id")]
        //[Required(ErrorMessage = "Please Enter Company Reg/Tax Id")]
        public string CompanyTaxId { get; set; }

        public List<CountryViewModel> CountryList { get; set; }
        [Display(Name = "Country")]
       // [Required(ErrorMessage = "Please Select Country")]
        public Int64 CountryId { get; set; }

        public string Country { get; set; }

        public List<StateViewModel> StateList { get; set; }

        [Display(Name = "State")]        
        public Int64 StateId { get; set; }

        public string State { get; set; }

        [Display(Name = "State (Other than US)")]
        public string StateName { get; set; }

        public List<CityViewModel> CityList { get; set; }
        [Display(Name = "Town")]
             
        public Int64? TownId { get; set; }
        public string Town { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Please Enter ZipCode")]
        public string ZipCode { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please Enter Address")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [Required(ErrorMessage = "Please Enter Address2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please Enter City")]
        public string City { get; set; }

        [Display(Name = "Mobile No.")]
        [Required(ErrorMessage = "Please Enter Mobile No.")]
        [RegularExpression(@"^(\+)\s?\d([- ]*\d){8,15}$", ErrorMessage = "Invalid Mobile Number!")]
        public string MobileNo { get; set; }

        [Display(Name = "Telephone No.")]
        //[RegularExpression(@"^(\+)\s?(\d{1,3})\s?\d([ ]*\d){8,15}$", ErrorMessage = "Please Enter valid Telephone number ")]
        //[RegularExpression(@"^(\+)\s?\d([- ]*\d){8,15}$", ErrorMessage = "Invalid Telephone Number!")]
        public string TelephoneNo { get; set; }

        [Display(Name = "Email Id (User Name)")]
       // [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please Enter valid Email Id!")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        public string EmailId { get; set; }
               
        [Display(Name = "Tax")]
       // [Required(ErrorMessage = "Please Select Tax Chargeable Or Not")]
        public bool Tax { get; set; }

        public Int64? TaxId { get; set; }

        [Display(Name = "Fax")]
        [Required(ErrorMessage = "Please Enter Fax")]
        public string Fax { get; set; }
        

        //For Different Shipping Address
        [Display(Name = "Shipping Country")]
        public string ShippingCountry { get; set; }

        [Display(Name = "Shipping State")]
        public string ShippingState { get; set; }

        [Display(Name = "Shipping Town")]
        public string ShippingTown { get; set; }

        [Display(Name = "Zip Code")]
       // [Required(ErrorMessage = "Please Enter Zip code")]
        public string ShippingZipCode { get; set; }

        [Display(Name = "Address Line 1")]
        public string ShippingAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string ShippingAddressLine2 { get; set; }

        [Display(Name = "Address Line 3")]
        public string ShippingAddressLine3 { get; set; }

        public List<CustomerViewModel> CustomerList { get; set; }

        public List<RoleViewModel> RoleList { get; set; }

        [Display(Name = "Customer Type")]
       // [Required(ErrorMessage = "Please Select Customer Type")]
        public Int64 RoleId { get; set; }

        public string RoleName { get; set; }

        public List<UserLocation> location { get; set; }

        public string Workphone { get; set; }

             
    }
  }

