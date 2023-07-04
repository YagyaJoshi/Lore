using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface INavigationProvider
    {
        NavigationViewModel ToNavigationViewModel(Navigation navigation);
        SubNavigationViewModel ToSubNavigationViewModel(SubNavigation subNavigation);
        NavigationsViewModel GetNavigations(UserRole role);
        NavigationsViewModel GetNavigations(Int64? roleid);
    }
}
