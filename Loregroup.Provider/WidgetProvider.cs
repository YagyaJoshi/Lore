using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;

namespace Loregroup.Provider
{
   public class WidgetProvider:IWidgetProvider
   {
       private readonly AppContext _context;

       public WidgetProvider(AppContext context){
        _context = context; }

       public WidgetViewModel ToWidgetViewModel(DashBoradWidget widgets, int depth)
       {
           if (depth == 1)
           {
               return new WidgetViewModel()
               {
                   Id = widgets.Id,
                   WidgetName = widgets.WidgetName,
                   DisplayonDashboard=widgets.DisplayonDashboard,
                   CreatedById = widgets.CreatedById,
                   CreationDate = widgets.CreationDate,
                   ModifiedById = widgets.ModifiedById,
                   ModificationDate = widgets.ModificationDate,
                   StatusId=widgets.StatusId
               };
           }
           return new WidgetViewModel()
           {
               Id = widgets.Id,
               WidgetName = widgets.WidgetName,
               DisplayonDashboard = widgets.DisplayonDashboard,
               CreatedById = widgets.CreatedById,
               CreationDate = widgets.CreationDate,
               ModifiedById = widgets.ModifiedById,
               ModificationDate = widgets.ModificationDate,
               StatusId = widgets.StatusId
           };
       }

       public WidgetViewModels GetAllWidgets(Status? status, int page = 0, int records = 0)
       {
           var widgetPredicate = PredicateBuilder.True<DashBoradWidget>();
           if (status != null)
           {
               widgetPredicate.And(x => x.StatusId == (int)status);
           }
          
           return new WidgetViewModels()
           {
              
           };
       }

       public List<WidgetViewModel> GetAllWidget(Status? status, int page = 0, int records = 0)
       {
           var widgetPredicate = PredicateBuilder.True<DashBoradWidget>();

           if (status != null)
           {
               widgetPredicate.And(x => x.StatusId == (int)status);
           }
           if (page > 0)
           {
              
           }

         
           return new List<WidgetViewModel>();
       }

        public void EditWidgetControll(int Widgetid, int Userid)
        {
                        
        }
        
        public void EditWidgetControll(string widgetid, int Userid)
        {
           

            int Widgetid = 0;
            if (widgetid.Contains(','))
            {
                string[] arr = widgetid.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    Widgetid = Convert.ToInt32(arr[i]);
                    
                }
            }
            else
            {
                Widgetid = Convert.ToInt32(widgetid);
                          }
        }

         public List<WidgetViewModel> GetAllWidget1(Status? status, int page = 0, int records = 0)
         {
             var widgetPredicate = PredicateBuilder.True<DashBoradWidget>();

             if (status != null)
             {
                 widgetPredicate.And(x => x.StatusId == (int)status);
             }
             if (page > 0)
             {
                           }

                          return new List<WidgetViewModel>();
         }
    }
}
