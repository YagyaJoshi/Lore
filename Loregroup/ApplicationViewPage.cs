using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loregroup
{
    public abstract class ApplicationViewPage<T> : WebViewPage<T>
    {
        private ISession _session;
        private ICacheService _dataCache;
        private INavigationProvider _navigationProvider;
        private IMasterProvider _MasterProvider;
        private ILoreProvider _loreProvider;
        private IUserProvider _userProvider;

        protected ApplicationViewPage()
        {
        }

        protected override void InitializePage()
        {
            SetViewBagDefaultProperties();
            base.InitializePage();
        }

        private void SetViewBagDefaultProperties()
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            ViewBag.CurrentUrl = url.Content("~/");
            if (url.Content("~/") == "/")
            {
                ViewBag.CurrentUrl = "";
            }

            _session = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISession>();
            _dataCache = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICacheService>();
            _navigationProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<INavigationProvider>();
            _MasterProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IMasterProvider>();
            _loreProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoreProvider>();
            _userProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserProvider>();

            if (ViewBag.LayoutModel == null)
            {
                LayoutModel model = new LayoutModel();
                FrontendLayoutModel frontModel = new FrontendLayoutModel();

                frontModel.CategoryList = _loreProvider.GetAllCategoryList().OrderBy(x=>x.Category).ToList();

                try
                {
                    frontModel.UserId = Convert.ToInt32(Session["FrontendUserId"]);
                    frontModel.CartCount = _loreProvider.GetTempcartCount(Convert.ToInt32(Session["FrontendUserId"]));
                    if (frontModel.UserId > 0)
                    {
                        frontModel.CurrencyId = _userProvider.GetUser(frontModel.UserId).CurrencyId;
                        frontModel.DistributorPointId = _userProvider.GetUser(frontModel.UserId).DistributionPointId;
                    }
                }
                catch (Exception ex)
                {
                    frontModel.UserId = 0;
                    frontModel.CurrencyId = 0;
                    frontModel.CartCount = 0;
                }

                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (_session.CurrentUser != null)
                    {
                        model.SessionUser = _session.CurrentUser;
                        model.SessionUser.Profimage = _MasterProvider.Getprofimage(_session.CurrentUser.Id);

                        frontModel.SessionUser = _session.CurrentUser;
                        frontModel.SessionUser.Profimage = model.SessionUser.Profimage;

                        model.LeftSideMenu = _navigationProvider.GetNavigations(_session.CurrentUser.RoleId);
                    }
                    else
                    {
                        _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.HttpContext.Current.Response.Redirect("/", true);
                        System.Web.HttpContext.Current.Response.End();
                    }
                }

                ViewBag.LayoutModel = model;
                ViewBag.FrontLayoutModel = frontModel;

            }
        }



    }
}

