using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public class ViewCountViewModel
    {
        public Int64 Id { get; set; }

        public Int64 PostId { get; set; }
       public Int64 Count { get; set; }
     
        public string Type { get; set; }

    }
}
