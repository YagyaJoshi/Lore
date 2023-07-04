using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class MasterUserViewModel :BaseViewModel
    {

       public MasterUserViewModel()
       {
           M_CityViewModels = new CityViewModel();
        
           DistrictsViewModel = new DistrictsViewModel();
           UserViewModel = new UserViewModel();
       }

       [Required(ErrorMessage = "Please Enter First Name")]
       [Display(Name = "First Name")]
        public String FirstName { get; set; }

       [Required(ErrorMessage = "Please Enter Last Name")]
       [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Please Select Gender")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

       [EmailAddress(ErrorMessage = "Invalid Email Address")]
       [Required(ErrorMessage = "Please Enter Email Address")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "User Name")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public String Password { get; set; }

        public String Salt { get; set; }
     
        public String Fax { get; set; }

       [Required(ErrorMessage = "Please Enter Mobile No.")]
        [StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)] 
        public String Mobile { get; set; }

       public String Country { get; set; }

       public List<StateViewModel> StateList { get; set; }       
       public Int64? SelectedStateId { get; set; }      
        public string StateName { get; set; }

        [Display(Name = "District")]
        public Int64? SelectedDistrictId { get; set; }
        public List<DistrictsViewModel> DistrictList { get; set; }
        public string District { get; set; }

        public List<CityViewModel> CityList { get; set; }
        public Int64? CityId { get; set; }
        [Display(Name = "City")]
        public String City { get; set; }

        [Display(Name = "Role")]     
        public List<RoleViewModel> RoleList { get; set; }
        public Int64 RoleId { get; set; }      
        public String RoleName { get; set; }
        public string Keyword { get; set; }
        public Int64 SuperAdminRoleId { get; set; }
        public Int64 CurrentUser { get; set; }


        public Int64 PackageId { get; set; }
        public CityViewModel M_CityViewModels { get; set; }
        public DistrictsViewModel DistrictsViewModel { get; set; } 
        public UserViewModel UserViewModel { get; set; }

    }
   public class RoleViewModel : BaseViewModel
   {
       public RoleViewModel()
       {

           RoleList = new List<RoleViewModel>();
       }
       [Required]
       //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "LengthLessthan50")]
       //[RegularExpression("^[A-Za-z0-9- ]+$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SpecialChrsNotAllowed")]
       [Display(Name = "Role Name")]
       public String Name { get; set; }

       public bool Value { get; set; }
       public List<RoleViewModel> RoleList { get; set; }

   }

   public class RoleViewModels
   {
       public RoleViewModels()
       {
           Roles = new List<RoleViewModel>();
       }
       public List<RoleViewModel> Roles { get; set; }
   }

   public class PermissionViewModel : BaseViewModel
   {
       public PermissionViewModel()
       {
           this.RoleList = new List<RoleViewModel>();
       }
       [Required]
       //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "LengthLessthan50")]
       //[RegularExpression("^[A-Za-z0-9- ]+$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SpecialChrsNotAllowed")]
       [Display(Name = "Permission Name")]
       public String Name { get; set; }
       [Required]
       //[StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "LengthLessthan50")]
       //[RegularExpression("^[A-Za-z0-9- ]+$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SpecialChrsNotAllowed")]
       [Display(Name = "Display Name")]
       public String DisplayName { get; set; }
       public String Description { get; set; }

       public List<RoleViewModel> RoleList { get; set; }
   }

   public class RolePermissionShowViewModel : BaseViewModel
   {
       public RolePermissionShowViewModel()
       {
           this.RoleList = new List<RoleViewModel>();
           this.PermissionList = new List<PermissionViewModel>();
       }
       public List<PermissionViewModel> PermissionList { get; set; }
       public List<RoleViewModel> RoleList { get; set; }
   }

   //public class PermissionMatrixViewModel : BaseViewModel
   //{
   //    public PermissionMatrixViewModel()
   //    {
   //        this.PermissionList = new List<PermissionViewModel>();
   //    }
   //    public List<PermissionViewModel> PermissionList { get; set; }
   //}

   public class RoleToPermisoinSetViewModel : BaseViewModel
   {
       public Int64 RoleId { get; set; }
       public Int64[] Permissions { get; set; }
   }

}
