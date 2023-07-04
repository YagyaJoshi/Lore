using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.BusinessEntities
{
    public class RequestObjectModel {
        public String Controller { get; set; }
        public String Action { get; set; }
        public object Parameters { get; set; }
    }
}
