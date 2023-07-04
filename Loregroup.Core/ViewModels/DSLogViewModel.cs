using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class DSLogViewModel : BaseViewModel
    {
        public string Message { get; set; }

        public string Number { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
