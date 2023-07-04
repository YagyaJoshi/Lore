using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loregroup.Core.ViewModels;
using Loregroup.Core.Logmodels;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using System.Net.Mail;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Enumerations;
using Loregroup.Data;
using Loregroup.Core;
using System.Web.Security;
using Loregroup.Data.Entities;
using System.Web.Script.Serialization;

namespace Loregroup.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWidgetProvider _widgetProvider;
        //   private readonly IUserSettingsProvider _AppUsersettingsProvider;
        private readonly IUserProvider _userProvider;
        private readonly ISession _session;
        private readonly IErrorHandler _errorHandler;
        private readonly INotificationProvider _notificationprovider;
        private readonly IMasterProvider _masterProvider;
        private readonly AppContext _context;
        private readonly ICacheService _dataCache;
        private readonly IDeliverySlipProvider _deliveryslipProvider;

        //  IWebServiceProvider _webServiceProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IWebServiceProvider>();
        public HomeController(IDeliverySlipProvider deliveryslipProvider, AppContext context, IErrorHandler errorHandler, ISession session, IUserProvider userProvider, IWidgetProvider widgetProvider, INotificationProvider notificationprovider, IMasterProvider masterProvider, ICacheService dataCache)
        {
            _deliveryslipProvider = deliveryslipProvider;
            _errorHandler = errorHandler;
            _session = session;
            _userProvider = userProvider;
            _widgetProvider = widgetProvider;
            //    _AppUsersettingsProvider = AppUsersettingsProvider;
            _notificationprovider = notificationprovider;
            _masterProvider = masterProvider;
            _context = context;
            _dataCache = dataCache;
        }

        public ActionResult NotPermit()
        {
            // Add By 
            TempData["userMessage"] = "You Do not Have Permission For This";
            return View();
        }

        public ActionResult Index()
        {
            // Add By 
            WidgetViewModels model = new WidgetViewModels();
            try
            {
                LogMe.SetupLogger(LogMeCommonMng.LogPath, "Loregroup");
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Dashboard",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                try
                {
                    //var userid = _session.CurrentUser.Id;
                    //var res = _userProvider.GetMaster_User(userid);
                    //Int64 pid = _masterProvider.GetPackageId(_session.CurrentUser.Id);
                    model.RoleId = (int)_session.CurrentUser.Role;                                    
                                    
                    LogMe.Log("HomeController", LogMeCommonMng.LogType.Info, "dashboard open success.");
                    _errorHandler.HandleInfo("sir i am success");
                    return View(model);
                }
                catch (Exception ex)
                {
                    //LogMe.Log("HomeController", LogMeCommonMng.LogType.Error, ex.Message);
                    Session.Abandon();
                    _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                Session.Abandon();
                _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }
        }

        #region: FrontEnd

        public ActionResult Home()
        {
            return View();
        }

        #endregion

        //old code....
        //public ActionResult Index()
        //{
        //    // Add By 
        //    WidgetViewModels model = new WidgetViewModels();
        //    try
        //    {
        //        LogMe.SetupLogger(LogMeCommonMng.LogPath, "Loregroup");
        //        var breadCrumbModel = new BreadCrumbModel()
        //        {
        //            Url = "/Home/",
        //            Title = "Dashboard",
        //            SubBreadCrumbModel = null
        //        };
        //        ViewBag.BreadCrumb = breadCrumbModel;
        //        try
        //        {

        //            {
        //                model.RoleId = (int)_session.CurrentUser.Role;
        //                if (model.RoleId == (int)UserRole.SuperAdmin)
        //                {
        //                    try
        //                    {
        //                        //model.AppUsersCount = 8;
        //                        //model.AllAdvisors = _masterProvider.GetAllUser(null, 0, 0).Count();
        //                        //model.TotalUser = _context.AppUsers.Count();
        //                        //model.TotalMall = _context.LeadRegistrations.Count();
        //                        //model.TotalPendingUser = _context.AppUsers.Where(x => x.IsOTP == false).Count();
        //                        //model.TotalNewLead = _context.LeadAllotments.Where(x => x.Status == 1).Count();
        //                        //model.TotalHotLead = _context.LeadAllotments.Where(x => x.Status == 2).Count();
        //                        //model.TotalColdLead = _context.LeadAllotments.Where(x => x.Status == 3).Count();
        //                        //model.TotalTerminatedLead = _context.LeadAllotments.Where(x => x.Status == 4).Count();
        //                        //model.TotalSuspendLead = _context.LeadAllotments.Where(x => x.Status == 5).Count();
        //                        //model.TotalSuccessLead = _context.LeadAllotments.Where(x => x.Status == 6).Count();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        //model.AppUsersCount = 40;
        //                        //model.AllAdvisors = 315;
        //                        //model.PendingAdvisors = 220;
        //                    }
        //                }
        //                else if (model.RoleId == (int)UserRole.Mall)
        //                {

        //                }
        //                else if (model.RoleId == (int)UserRole.Store)
        //                {

        //                }
        //                else if (model.RoleId == (int)UserRole.User)
        //                {
        //                    List<NotificationsViewModel> Classified = _userProvider.GetUsernotification(_session.CurrentUser.Id, null, 0, 0);
        //                    Int64 notcount = Classified.Where(k => k.StatusId == 1).Count();
        //                    model.NotificationCount = notcount;
        //                }
        //                else if (model.RoleId == (int)UserRole.SuperAdmin)
        //                {

        //                }
        //            }

        //            {
        //                if (model.RoleId == 5)
        //                {
        //                    model.RoleId = (int)UserRole.MallandUser;
        //                }

        //                if (model.RoleId == 6)
        //                {
        //                    model.RoleId = (int)UserRole.MallandUserandStore;
        //                }
        //                if (model.RoleId == 7)
        //                {
        //                    model.RoleId = (int)UserRole.StoreandUser;
        //                }
        //                if (model.RoleId == 8)
        //                {
        //                    model.RoleId = (int)UserRole.MallAndStore;
        //                }

        //            }

        //            LogMe.Log("HomeController", LogMeCommonMng.LogType.Info, "dashboard open success.");
        //            _errorHandler.HandleInfo("sir i am success");
        //            return View(model);
        //        }
        //        catch (Exception ex)
        //        {
        //            //LogMe.Log("HomeController", LogMeCommonMng.LogType.Error, ex.Message);
        //            Session.Abandon();
        //            _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
        //            FormsAuthentication.SignOut();
        //            return RedirectToAction("Login", "Account");
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        Session.Abandon();
        //        _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Login", "Account");
        //    }
        //}

        public ActionResult Chart()
        {
            return View();
        }

        #region : CheckStatus


        public ActionResult CheckStatus()
        {

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Check Status",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            CheckStatusViewModel model = new CheckStatusViewModel();
            model.MsgAccToStatus = "";
            return View(model);

        }

        [HttpPost]
        public ActionResult CheckStatus(CheckStatusViewModel model)
        {


            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Check Status",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            Consumer c = new Consumer();
            c = _deliveryslipProvider.GetConsumerStatus(model.ConsumerNo);
            Int64 p = 0;
            if (model.ConsumerNo != null)
            {
                if (c == null)
                {
                    model.MsgAccToStatus = "बुकिंग प्राप्त नहीं हुई है|";

                }

                else if (c != null && c.BStatus != "Open")
                {
                    model.MsgAccToStatus = "अपने हॉकर से संपर्क करें|";
                    return View(model);
                }
                else if (c.BStatus == "Open" && (c.InstallationOnBooking == "-" || c.InstallationOnBooking == ""))
                {
                    model.ConsumerNo = c.ConsumerNo;
                    p = _masterProvider.GetPackageId(c.CreatedById);
                    model.Amount = _deliveryslipProvider.GetRefillAmount(p);
                    string d = c.BookingDate.Value.ToShortDateString();
                    model.onlydate = d;
                    // model.BookingDate = c.BookingDate;
                    model.BookingNo = c.BookNo;
                    model.declaration = "उपरोक्त रेट में मुझे सही वज़न का सिल पैक सिलिंडर प्राप्त हुआ|";
                    model.Name = c.Name;
                    model.bstatus = "Success";
                    var User = _masterProvider.GetUser(c.CreatedById);
                    model.UserFullName = User.FirstName + User.LastName;
                    model.UserCity = _masterProvider.GetCityName(User.CityId);
                    model.Username = User.UserName + "          ";
                    model.forgap = "";

                }

            }
            //  "PackageId_" + p + " " + "Amount_" + model.Amount + " " + "BookingNo_ " + model.BookingNo + " " + "Distributor_" + model.UserFullName + " " + "City_" + model.UserCity + " " + "Status_" + model.Status + " " + "Deaclaration" + model.declaration;
            model.dslog.Message = new JavaScriptSerializer().Serialize(model);
            _masterProvider.SaveDSLog(model);

            return View(model);
        }


        #endregion


    }
}