using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Utilities;
using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Utilities
{
    public class Session:ISession {
        private readonly IUserProvider _userProvider;
        private readonly ICacheService _dataCache;

        public Session(ICacheService dataCache, IUserProvider accountProvider) {
            _dataCache = dataCache;
            _userProvider = accountProvider;
        }

        public SessionUser CurrentUser {
            get {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) {
                    String cacheKey = String.Format("SessionUser_{0}", System.Web.HttpContext.Current.User.Identity.Name);
                    SessionUser currentUser = new SessionUser();
                    currentUser = _dataCache.Get(cacheKey, () => _userProvider.GetDomainUser(Int64.Parse(System.Web.HttpContext.Current.User.Identity.Name)));
                    return currentUser;
                } else
                    return null;
            }
        }
    }
}
