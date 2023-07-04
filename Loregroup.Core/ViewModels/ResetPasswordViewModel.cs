using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class ResetPasswordViewModel
    {
        [Display(Name="User Name :")]
       [Required(ErrorMessage="*")]
        public string UserName { get; set; }
        [Display(Name="Current Password :")]
        [Required(ErrorMessage="*")]
        public string CurrentPassword { get; set; }
        [Display(Name="New Password :")]
        [Required(ErrorMessage="*")]
        public string NewPassword { get; set; }
        [Display(Name="Confirm Password :")]
        [Required(ErrorMessage="*")]
        public string ConfirmNewPassword { get; set; }
    }
}
