using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Logmodels;
using Loregroup.Core.Enumerations;
using System.Web.Security;
using Loregroup.Data;

namespace Loregroup.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: /Widgets/
        private readonly IUserProvider _userProvider;
        private readonly ISession _session;
        private readonly ICommonProvider _commonProvider;
        private readonly IWidgetProvider _widgetProvider;
        private readonly AppContext _context;
        public WidgetViewModel model = new WidgetViewModel();

        public WidgetsController(ISession session, IUserProvider userProvider, ICommonProvider commonProvider, IWidgetProvider widgetProvider, AppContext context)
        {
            _commonProvider = commonProvider;
            _session = session;
            _userProvider = userProvider;
            _widgetProvider = widgetProvider;
            _context = context;
        }

        public ActionResult Index()
        {
            try
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Widgets/",
                    Title = "Widgets",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                model.WidgetViewModels = _widgetProvider.GetAllWidgets(null);

                return View();
            }
            catch (Exception ex)
            {
                LogMe.Log("WidgetsController", LogMeCommonMng.LogType.Error, ex.Message);
                return View();
            }
            
        }

        [HttpPost]
        public JsonResult GetAllWidgets(int size, int page)
        {
            try
            {
                var widgets = _widgetProvider.GetAllWidget(null, page, size);
                JsonListModel<WidgetViewModel> model = new JsonListModel<WidgetViewModel>
                {
                    List = widgets,
                    Message = "records fetched successfully",
                    Result = true
                };
                LogMe.Log("WidgetsController", LogMeCommonMng.LogType.Info, "get all Widgets show success.");
                return Json(model);
            }
            catch (Exception ex)
            {
                JsonListModel<WidgetViewModel> model = new JsonListModel<WidgetViewModel>();
                LogMe.Log("WidgetsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(model);
            }
        }

        [HttpPost]
        public JsonResult EditWidgetControll(string widgetid)
        {
            try
            {
                _widgetProvider.EditWidgetControll(widgetid, (int)_session.CurrentUser.Id);
                
                LogMe.Log("WidgetsController", LogMeCommonMng.LogType.Info, "Widgets  update success.");
                return Json(widgetid);
            }
            catch (Exception ex)
            {
                LogMe.Log("WidgetsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(widgetid);
            }
        }
    }
}