using System;
using Loregroup.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Loregroup.Core.ViewModels
{
    public class BaseEntityViewModel : BaseViewModel
    {
        public BaseEntityViewModel() 
        {
            BaseEntityList = new List<BaseEntityViewModel>();
        }
      
        [Required(ErrorMessage = "Data is required.")]
        [Display(Name = "Text Data")]
        [AllowHtml]
        public string TextData { get; set; }
      
        public string SystemName { get; set; }
        
        public string Image { get; set; }

        public string Save { get; set; }
        public string Cancel { get; set; }

        public List<BaseEntityViewModel> BaseEntityList { get; set; }

    }
}
