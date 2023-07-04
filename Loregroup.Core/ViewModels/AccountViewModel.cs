using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class AccountViewModel 
    {

    }

    public class LoginViewModel {
        //[Required]
        [Display(Name="User ID")]
        public string UserName { get; set; }

        //[Required]
        [Display(Name="Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool IsRemember { get; set; }

        public bool check { get; set; }




    }

    public class ForgetPasswordViewModel {
        public Int64 Id { get; set; }
        [Display(Name = "Email Id :")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "*")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "*")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class ClientViewModel
    {
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        public String Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }
    }


  
}
