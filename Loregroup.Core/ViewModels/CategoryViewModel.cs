using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Loregroup.Core.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        public CategoryViewModel()
        {
            CategoryList = new List<CategoryViewModel>();
        }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Category Image")]
        public string ImagePath { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<CategoryViewModel> CategoryList { get; set; }
    }
}
