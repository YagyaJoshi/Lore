using Loregroup.Core.Interfaces.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loregroup.Core.Helpers.Attributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                ISession _session = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISession>();

                string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();

                switch (_session.CurrentUser.Role)
                {
                    case Loregroup.Core.Enumerations.UserRole.SuperAdmin:
                        break;
                    case Loregroup.Core.Enumerations.UserRole.Admin:
                        break;
                    case Loregroup.Core.Enumerations.UserRole.Shop:
                        break;
                    case Loregroup.Core.Enumerations.UserRole.Staff:
                        break;
                    case Loregroup.Core.Enumerations.UserRole.Public:
                        break;
                    case Loregroup.Core.Enumerations.UserRole.Agent:
                        break;
                    case Loregroup.Core.Enumerations.UserRole.Supplier:
                        break;

                        if (controllerName == "usermanagement")
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        }
                        break;
                    //case Loregroup.Core.Enumerations.UserRole.User:
                    //    if (controllerName == "usermanagement") {
                    //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    //    }
                    //    break;
                    default:
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        break;
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "DSCApp", action = "LoginPage" }));
            }

        }
    }
}
