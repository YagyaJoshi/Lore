using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class OrderLocatorViewModel : BaseViewModel
    {
        public OrderLocatorViewModel()
        {
            OrderLocatorList = new List<OrderLocatorViewModel>();
        }

        [Required]
        [Display(Name = "Locator Name")]
        public string OrderLocatorName { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }

        public string OrderLocatorNameDesc { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

        public List<OrderLocatorViewModel> OrderLocatorList { get; set; }
    }
}
