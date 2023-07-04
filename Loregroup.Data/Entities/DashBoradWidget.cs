using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class DashBoradWidget : BaseEntity
    {
       public String WidgetName { get; set; }
       public bool DisplayonDashboard { get; set; }
       public string Name { get; set; }
       public string DisplayName { get; set; }
       public string TypeHold { get; set; }
       public string Text { get; set; }
    }
}
