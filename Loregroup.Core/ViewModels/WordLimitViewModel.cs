using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class WordLimitViewModel
    {
        public WordLimitViewModel()
        {

            WordLimitList = new List<WordLimitViewModel>();
        }
        public Int64 Id { get; set; }
        [Display(Name = "Work Limit")]
        [Required(ErrorMessage = "Please Enter WordLimit")]
        public string WordLimit { get; set; }

        public int? StatusId { get; set; }

        public List<WordLimitViewModel> WordLimitList { get; set; }

    }
}
