using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Loregroup.Core.Interfaces.Providers;
using System.IO;

namespace Loregroup.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {
        }

        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound() {
            return View();
        }

        public ActionResult AccessDenied() {
            return View();
        }
    }
}