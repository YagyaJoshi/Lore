using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Cache;
using Loregroup.Core.Utilities;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces;
using Loregroup.Provider;

namespace Loregroup
{
    public static class UnityConfig
    {
        public static IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<ICacheService, InMemoryCache>();
            container.RegisterType<ISecurity, Security>();
            container.RegisterType<IUtilities, Utilities>();
            container.RegisterType<IErrorHandler, ErrorHandler>();
            container.RegisterType<ISession, Session>();
            container.RegisterType<IImageUtilities, ImageUtilities>();
            container.RegisterType<IConfigSettingProvider, ConfigSettingProvider>();
            container.RegisterType<INavigationProvider, NavigationProvider>();
            container.RegisterType<IUserProvider, UserProvider>();
            container.RegisterType<IContentProvider, ContentProvider>();
            container.RegisterType<ICommonProvider, CommonProvider>();
            container.RegisterType<IWidgetProvider, WidgetProvider>();
            //container.RegisterType<IUserSettingsProvider, UserSettingsProvider>();
            container.RegisterType<INotificationProvider, NotificationProvider>();
            container.RegisterType<IMasterProvider, MasterProvider>();

            //container.RegisterType<ICheckingAppProvider, CheckingAppProvider>();
            //container.RegisterType<IWebServiceProvider, WebServiceProvider>();
            container.RegisterType<IUploadFileProvider, UploadFileProvider>();
            container.RegisterType<IDeliverySlipProvider, DeliverySlipProvider>();
            container.RegisterType<IProductProvider, ProductProvider>();
            container.RegisterType<IOrderProvider, OrderProvider>();
            container.RegisterType<ILoreProvider, LoreProvider>();
            container.RegisterType<ISettingProvider, SettingProvider>();

            return container;
        }
    }
}