using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;

namespace Loregroup.Core.ViewModels
{
    public class GalleryViewModel : BaseViewModel
    {
        public GalleryViewModel()
        {
            GalleryList = new List<GalleryViewModel>();
        }

        public List<GalleryViewModel> GalleryList { get; set; }


        [Display(Name = "Gallery Type")]
        public GalleryType GalleryTypeId { get; set; }
        public string GalleryType { get; set; }

        [Required(ErrorMessage = "Please Enter Title Name")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Enter Image")]
        [Display(Name = "Image")]
        public string image { get; set; }

        [Required(ErrorMessage = "Please Enter Video")]
        [Display(Name = "Video")]
        public string VideoUrl { get; set; }

        public bool IsVideo { get; set; }

        public string Picture { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

    }
}
