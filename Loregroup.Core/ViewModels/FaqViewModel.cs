using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class FaqViewModel : BaseViewModel
    {
        public FaqViewModel()
        {
            FaqList = new List<FaqViewModel>();
        }

        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }

        [Required]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<FaqViewModel> FaqList { get; set; }

    }
}
