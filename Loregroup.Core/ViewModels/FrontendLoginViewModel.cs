using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class FrontendLoginViewModel
    {

        [Required]
        [Display(Name = "Username")]
        //[RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please Enter valid Email Id!")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$", ErrorMessage = "Password Must be combination of capital & small alphabets & number, and password length is 8 digit.")]
        public string Password { get; set; }

        public Int64 Id { get; set; }

    }
}
