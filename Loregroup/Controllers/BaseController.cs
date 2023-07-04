using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Loregroup.Core.Helpers;
using Loregroup.Core.Helpers.Attributes;
using System.Web.Routing;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Logmodels;

namespace Loregroup.Controllers
{

    [HandleError]
    [AuthorizeUser]
    public abstract class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    try
        //    {
        //        base.Initialize(requestContext);
        //        var actionName = requestContext.RouteData.Values["action"];
        //        string area = Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
        //        string controller = Request.RequestContext.RouteData.Values["controller"].ToString();
        //        string action = Request.RequestContext.RouteData.Values["action"].ToString();
        //        IUserProvider _userProvid = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserProvider>();

        //      //  Int64 ca = _userProvid.CheckAllPermission(controller, action);

        //        if (ca == 0)
        //        {
        //            base.Initialize(requestContext);
        //            requestContext.HttpContext.Response.Clear();
        //            requestContext.HttpContext.Response.Redirect(Url.Action("NotPermit", "Home"));
        //            requestContext.HttpContext.Response.End();
        //        }
        //        // ActionExecutingContext filterContext;
        //        //var control = filterContext.Controller as SettingsController;
        //        //control.();
        //    }
        //    catch { }
        //}

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                Exception e = filterContext.Exception;

                //Log Exception e
                filterContext.ExceptionHandled = true;
                //filterContext.Result = new ErrorController().Index();
                //filterContext.RouteData.Values["controller"] = "ErrorController";
                //filterContext.RouteData.Values["action"] = "index";
                //new ViewResult() {
                //    ViewName = "Error"
                //};
                filterContext.Result = this.RedirectToAction("Index", "Error");
                base.OnException(filterContext);
                LogMe.Log("BaseController", LogMeCommonMng.LogType.Info, "base controller in exception show success.");
            }
            catch (Exception ex)
            {
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Error, ex.Message);                
            }
        }

    }
}