using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class FrontendAccountdetailViewModel
    {
        public Int64 Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Id (User Name)")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please Enter valid Email Id!")]
        [Required(ErrorMessage = "Please Enter Email Id")]
        public string EmailId { get; set; }

        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        //[Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
