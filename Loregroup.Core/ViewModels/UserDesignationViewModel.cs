using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class UserDesignationViewModel:BaseViewModel {
        public String Name { get; set; }
    }

    public class UserDesignationViewModels {
        public UserDesignationViewModels() {
            DesignationViewModels = new List<UserDesignationViewModel>();
        }

        public List<UserDesignationViewModel> DesignationViewModels { get; set; }
    }
}
