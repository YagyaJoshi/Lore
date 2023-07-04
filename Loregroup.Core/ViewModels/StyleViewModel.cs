using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class StyleViewModel : BaseViewModel
    {
        public StyleViewModel() 
        {
            StyleList = new List<StyleViewModel>();
        }
        public string StyleNo { get; set; }
        public new List<StyleViewModel> StyleList { get; set; }
    }
}
