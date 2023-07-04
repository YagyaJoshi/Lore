using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;

namespace Loregroup.Provider
{
    public class NavigationProvider:INavigationProvider {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ISession _sess;
        public static Int64? roleIdvalue = 0;



        #region : Navigation 

        public NavigationProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ISession sess)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _configProvider = configProvider;
            _sess = sess;
        }
        
        public NavigationsViewModel GetNavigations(UserRole role)
        {
            
             if (role == UserRole.SuperAdmin)
            {
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Where(x => x.StatusId == 1).ToList().OrderBy(x => x.Order).Select(ToNavigationViewModel).ToList()
                };
            }
            else if (role == UserRole.Shop)
            {
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Where(x => x.StatusId == 1).ToList().OrderBy(x => x.Order).Select(ToNavigationViewModel).ToList()
                };
            }
            else if (role == UserRole.Admin)
            {
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Where(x => x.StatusId == 1).ToList().OrderBy(x => x.Order).Select(ToNavigationViewModel).ToList()
                };
            }
            else if (role == UserRole.Staff)
            {
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Where(x => x.StatusId == 1).ToList().OrderBy(x => x.Order).Select(ToNavigationViewModel).ToList()
                };
            }
            else if (role == UserRole.Supplier)
            {
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Where(x => x.StatusId == 1).ToList().OrderBy(x => x.Order).Select(ToNavigationViewModel).ToList()
                };
            }
           
            return new NavigationsViewModel();
        }
      
        public NavigationViewModel ToNavigationViewModel(Navigation navigation)
        {
            return new NavigationViewModel()
            {
               
                Id = navigation.Id,
                ActionUrl = navigation.ActionUrl,
                ActionUrlRequestType = (HttpRequestType)navigation.ActionUrlRequestType,
                Icon = navigation.Icon,
                Text = navigation.Text,
                HasSubMenu = navigation.HasSubMenu,
                SubNavigations =
                    _context.SubNavigations.Join(_context.PermissionMatrixs.Where(c => c.RoleId == roleIdvalue && c.TypeHold == "SubNav" && c.PermissionStatus == true), s => s.Id, d => d.AllNAvigationsId, (s, d) => s)
                        .Where(x => x.NavigationId == navigation.Id && x.StatusId == (int)Status.Active)
                        .ToList()
                        .OrderBy(x => x.Order)
                        .Select(ToSubNavigationViewModel)
                        .ToList()
            };
        }

        public SubNavigationViewModel ToSubNavigationViewModel(SubNavigation subNavigation)
        {
            return new SubNavigationViewModel()
            {
                Id = subNavigation.Id,
                ActionUrl = subNavigation.ActionUrl,
                ActionUrlRequestType = (HttpRequestType)subNavigation.ActionUrlRequestType,
                Icon = subNavigation.Icon,
                Text = subNavigation.Text
                
            };
        }

        public NavigationsViewModel GetNavigations(Int64? roleid)
        {
            try
            {
                roleIdvalue = roleid;
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Join(_context.PermissionMatrixs.Where(x => x.RoleId == roleid && x.TypeHold == "Nav" && x.PermissionStatus == true), s => s.Id, d => d.AllNAvigationsId, (s, d) => s)
                        .OrderBy(y => y.Order)
                        .ToList()
                        .Select(ToNavigationViewModel).ToList()
                };
            }
            catch (Exception)
            {
                return new NavigationsViewModel();
                //throw new NavigationDoesNotExistsException();
            }
        }

        #endregion


    }
}
