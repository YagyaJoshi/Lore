using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class PermissionMatrixViewModel :BaseViewModel
    {
        public Int64 PermissionId { get; set; }
        public Int64 RoleId { get; set; }
        public bool PermissionStatus { get; set; }

        public PermissionMatrixViewModel()
        {
            this.PermissionList = new List<PermissionViewModel>();
        }
        public List<PermissionViewModel> PermissionList { get; set; }

    }
}
