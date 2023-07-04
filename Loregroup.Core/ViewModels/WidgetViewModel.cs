using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
namespace Loregroup.Core.ViewModels
{
    public class WidgetViewModel : BaseViewModel
    {        
        public String WidgetName { get; set; }
        public bool DisplayonDashboard { get; set; }
        public bool check { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string TypeHold { get; set; }
        public string Text { get; set; }
        public List<RoleViewModel> RoleList{get;set;}

        public WidgetViewModels WidgetViewModels { get; set; }
       // public NotificationGridViewModel NotificationLists { get; set; }        
    }

    public class WidgetViewModels
    {
        public WidgetViewModels()
        {
            //  PackageMasterList = new List<PackageMasterViewModel>();
            AllUsersCount = 0;
            AllDistributorsCount = 0;
            AllMarketingManagersCount = 0;
            AllAdminsCount = 0;
            RefillRate = 0;
        }
        
        public List<WidgetViewModel> Widgets { get; set; }
        public List<NotificationViewModel> NotificationDash { get; set; }
        
        public Int64 RoleId { get; set; }

        public int AllUsersCount { get; set; }
        public int AllDistributorsCount { get; set; }
        public int AllMarketingManagersCount { get; set; }
        public decimal RefillRate { get; set; }
        public int AllAdminsCount { get; set; }
               
    }
}
