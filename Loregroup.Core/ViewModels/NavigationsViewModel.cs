using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{

    public class NavigationsViewModel {
        public NavigationsViewModel() {
            Navigations = new List<NavigationViewModel>();
        }
        public List<NavigationViewModel> Navigations { get; set; }
    }

    public class NavigationViewModel {
        public NavigationViewModel() {
            Badge = new BadgeDetails();
            SubNavigations = new List<SubNavigationViewModel>();
        }
        public Int64 Id { get; set; }
        public bool HasSubMenu { get; set; }
        public String Icon { get; set; }
        public String Text { get; set; }
        public int Order { get; set; }
        public BadgeDetails Badge { get; set; }
        public List<SubNavigationViewModel> SubNavigations { get; set; }

        public String ActionUrl { get; set; }
        public HttpRequestType ActionUrlRequestType { get; set; }
        public List<NavigationViewModel> PermissionList { get; set; }
        public List<RoleViewModel> RoleList { get; set; }
        public List<PermissionViewModel> FormalPermission { get; set; }
        public Int64 RoleId { get; set; }
        public String TypeHold { get; set; }
        public Int64 CompanyId { get; set; }
        public List<NavigationViewModel> NavList { get; set; }
        public List<NavigationViewModel> SubNavList { get; set; }
        public List<NavigationViewModel> ZNavList { get; set; }
        public List<WidgetViewModel> WidgetList { get; set; }
    }

    public class SubNavigationViewModel {
        public SubNavigationViewModel()
        {
            Badge = new BadgeDetails();
           // SubNavigationsChildModels = new List<SubNavigationsChildViewModel>();
        }
        public bool HasChildMenu { get; set; }
        public Int64 Id { get; set; }
        public String Icon { get; set; }
        public String Text { get; set; }
        public BadgeDetails Badge { get; set; }
      //  public List<SubNavigationsChildViewModel> SubNavigationsChildModels { get; set; }

        public String ActionUrl { get; set; }
        public HttpRequestType ActionUrlRequestType { get; set; }
    }

    public class BadgeDetails {
        public String IconClass { get; set; }
        public String Text { get; set; }
    }
}
