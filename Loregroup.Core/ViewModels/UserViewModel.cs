using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
  public  class UserViewModel
    {

      public UserViewModel()
      {
          UserList = new List<UserViewModel>();

      }
      public Int64 Id { get; set; }
      [Display(Name = "User Name")]
      [Required(ErrorMessage = "Please Enter User Name")]
      public string UserName { get; set; }

      public List<UserViewModel> UserList { get; set; }

    }
}
