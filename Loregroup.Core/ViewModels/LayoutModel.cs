using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class LayoutModel
    {
        public LayoutModel()
        {
            SessionUser = new SessionUser();
            Notifications = new NotificationsViewModel();
            LeftSideMenu = new NavigationsViewModel();
            CityViewModel = new CityViewModel();
        }
        public SessionUser SessionUser { get; set; }
        public NotificationsViewModel Notifications { get; set; }
        public NavigationsViewModel LeftSideMenu { get; set; }
        public CityViewModel CityViewModel { get; set; }
        public Int64 CityId { get; set; }
    }

    public class BreadCrumbModel
    {
        public String Url { get; set; }
        public String Title { get; set; }
        public BreadCrumbModel SubBreadCrumbModel { get; set; }
    }

}
