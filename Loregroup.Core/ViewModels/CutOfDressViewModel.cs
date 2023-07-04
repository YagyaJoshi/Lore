using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{   
    public class CutOfDressViewModel : BaseViewModel
    {
        public CutOfDressViewModel()
        {
            CutOfDressList = new List<CutOfDressViewModel>();
        }

        [Required]
        [Display(Name = "Cut Of Dress")]
        public string CutOfDress { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<CutOfDressViewModel> CutOfDressList { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
    }
}
