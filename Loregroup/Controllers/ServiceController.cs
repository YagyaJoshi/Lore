using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Exceptions;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
//using Loregroup.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Loregroup.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IUserProvider _userProvider;
        private readonly ICacheService _dataCache;
        private readonly ISession _session;
        private readonly ICommonProvider _commonProvider;

        public ServiceController(IUserProvider userProvider, ICacheService dataCache, ISession session, ICommonProvider commonProvider)
        {
            _userProvider = userProvider;
            _dataCache = dataCache;
            _session = session;
            _commonProvider = commonProvider;
        }

        
       
        public ActionResult SignOut() {
            _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        
    }
}