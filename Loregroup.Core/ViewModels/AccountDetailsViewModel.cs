using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Loregroup.Core.ViewModels
{
    public class AccountDetailsViewModel : BaseViewModel
    {
        public AccountDetailsViewModel()
        {
            CustomerList = new List<CustomerViewModel>();
            order = new OrderViewModel();
            AccountList = new List<AccountDetailsViewModel>();
            OrderList = new List<OrderViewModel>();
        }

        public List<OrderViewModel> OrderList { get; set; }
        public List<CustomerViewModel> CustomerList { get; set; }

        public List<AccountDetailsViewModel> AccountList { get; set; }

        public OrderViewModel order { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "New Password")]
        //[Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        [Display(Name = "Email Id (User Name)")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Please Enter valid Email Id!")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        public string EmailId { get; set; }

        public string ConfirmPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
