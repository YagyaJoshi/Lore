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
   public interface IWidgetProvider
    {
       WidgetViewModel ToWidgetViewModel(DashBoradWidget widgets, int depth);
       WidgetViewModels GetAllWidgets(Status? status, int page = 0, int records = 0);
       List<WidgetViewModel> GetAllWidget(Status? status, int page = 0, int records = 0);
       void EditWidgetControll(int Widgetid, int Userid);
       List<WidgetViewModel> GetAllWidget1(Status? status, int page = 0, int records = 0);
       void EditWidgetControll(string widgetid, int Userid);
    }
}
