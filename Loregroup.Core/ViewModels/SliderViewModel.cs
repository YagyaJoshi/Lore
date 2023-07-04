using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class SliderViewModel
    {
        public SliderViewModel()
        {
            SliderList = new List<SliderViewModel>();
        }

        public List<SliderViewModel> SliderList { get; set; }


        
        public Int64 Id { get; set; }
        public string GalleryType { get; set; }

        [Display(Name = "Title")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Please Enter Image")]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "First Text")]
        public string FirstText { get; set; }
        public string SecondText { get; set; }

        public bool IsVisible { get; set; }

        public string Edit { get; set; }
        public string Picture { get; set; }
    }
}
