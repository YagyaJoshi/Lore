using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class FrontendLayoutModel
    {

        public FrontendLayoutModel()
        {
            SessionUser = new SessionUser();
            //Notifications = new NotificationsViewModel();
            //LeftSideMenu = new NavigationsViewModel();
            CityViewModel = new CityViewModel();
            CategoryList = new List<CategoryViewModel>();
        }

        public SessionUser SessionUser { get; set; }
        //public NotificationsViewModel Notifications { get; set; }
        //public NavigationsViewModel LeftSideMenu { get; set; }
        public CityViewModel CityViewModel { get; set; }
        public List<CategoryViewModel> CategoryList { get; set; }
        public Int64 CityId { get; set; }
        public Int64 UserId { get; set; }
        public int CartCount { get; set; }
        public Int64? CurrencyId { get; set; }

        public Int64? DistributorPointId { get; set; }

    }
}
