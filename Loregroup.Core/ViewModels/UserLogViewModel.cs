using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class UserLogViewModel : BaseViewModel
    {
        public Int64 RoleId { get; set; }
        public string RoleName { get; set; }
        public string Module { get; set; }
        public string MessageLog { get; set; }
        public string UserName { get; set; }
        public string ShopName { get; set; }
        public string CreatedDateString { get; set; }
    }
}
