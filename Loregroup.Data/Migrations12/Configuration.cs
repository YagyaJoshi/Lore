//namespace PWA.Data.Migrations
//{
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

//    internal sealed class Configuration : DbMigrationsConfiguration<PWA.Data.PwaContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = false;
//            ContextKey = "PWA.Data.PwaContext";
//        }

//        protected override void Seed(PWA.Data.PwaContext context)
//        {
//            //  This method will be called after migrating to the latest version.

//            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
//            //  to avoid creating duplicate seed data. E.g.
//            //
//            //    context.People.AddOrUpdate(
//            //      p => p.FullName,
//            //      new Person { FullName = "Andrew Peters" },
//            //      new Person { FullName = "Brice Lambson" },
//            //      new Person { FullName = "Rowan Miller" }
//            //    );
//            //
//        }
//    }
//}



using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using PWA.Data.Entities;
namespace PWA.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PWA.Data.PwaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PWA.Data.PwaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            #region Navigation Seed
            context.Navigations.AddOrUpdate(x => x.Id,
                new Navigation()
                {
                    ActionUrl = "/Home/",
                    ActionUrlRequestType = 1,
                    HasSubMenu = false,
                    Icon = "fa-desktop",
                    Text = "Dashboard",
                    Order = 1,
                    StatusId = 1
                },
                new Navigation()
                {
                    ActionUrl = "#",
                    ActionUrlRequestType = 3,
                    HasSubMenu = true,
                    Icon = "fa-users",
                    Text = "Companies",
                    Order = 2,
                    StatusId = 1
                },
                 new Navigation()
                 {
                     ActionUrl = "#",
                     ActionUrlRequestType = 3,
                     HasSubMenu = true,
                     Icon = "fa-cogs",
                     Text = "Settings",
                     Order = 3,
                     StatusId = 1
                 },
                   new Navigation()
                   {
                       ActionUrl = "#",
                       ActionUrlRequestType = 1,
                       HasSubMenu = true,
                       Icon = "fa-print",
                       Text = "Reports",
                       Order = 4,
                       StatusId = 1
                   },
                 new Navigation()
                 {
                     ActionUrl = "#",
                     ActionUrlRequestType = 1,
                     HasSubMenu = true,
                     Icon = "fa-briefcase",
                     Text = "Widgets",
                     Order = 5,
                     StatusId = 1
                 }
            );

            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/CompanyManagement/",
                    ActionUrlRequestType = 1,
                    Icon = "fa-angle-double-right",
                    Text = "Registered Companies",
                    NavigationId = 2
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/CompanyManagement/AddEditCompany",
                    ActionUrlRequestType = 1,
                    Icon = "fa-angle-double-right",
                    Text = "Register New Company",
                    NavigationId = 2
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/CompanyManagement/CompaniesTree",
                    ActionUrlRequestType = 1,
                    Icon = "fa-angle-double-right",
                    Text = "Company Tree Structure",
                    NavigationId = 2
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
               new SubNavigation()
               {
                   ActionUrl = "/Settings/",
                   ActionUrlRequestType = 1,
                   Icon = "fa-angle-double-right",
                   Text = "Email Settings",
                   NavigationId = 3,
                   Order = 1
               });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
             new SubNavigation()
             {
                 ActionUrl = "/Settings/LogoSetting",
                 ActionUrlRequestType = 1,
                 Icon = "fa-angle-double-right",
                 Text = "Logo Settings",
                 NavigationId = 3,
                 Order = 2
             });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/Settings/LicenseSettings",
                    ActionUrlRequestType = 1,
                    Icon = "fa-archive",
                    Text = "License Settings",
                    NavigationId = 3,
                    Order = 3
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
               new SubNavigation()
               {
                   ActionUrl = "/Settings/UserSetting",
                   ActionUrlRequestType = 1,
                   Icon = "fa-puzzle-piece",
                   Text = "User Settings",
                   NavigationId = 3,
                   Order = 4
               });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/Settings/AccountSetting",
                    ActionUrlRequestType = 1,
                    Icon = "fa-archive",
                    Text = "Accounts Settings",
                    NavigationId = 3,
                    Order = 5
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
               new SubNavigation()
               {
                   ActionUrl = "/Settings/SoftwareSettings/",
                   ActionUrlRequestType = 1,
                   Icon = "fa-puzzle-piece",
                   Text = "Software Settings",
                   NavigationId = 3,
                   Order = 6
               });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "#",
                    ActionUrlRequestType = 1,
                    Icon = "fa-tasks",
                    Text = "Language Settings",
                    NavigationId = 3
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/Settings/Roles",
                    ActionUrlRequestType = 1,
                    Icon = "fa-archive",
                    Text = "Manage Roles",
                    NavigationId = 3
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/Settings/Permissions",
                    ActionUrlRequestType = 1,
                    Icon = "fa-archive",
                    Text = "Manage Permissions",
                    NavigationId = 3
                });
            context.SaveChanges();

            context.SubNavigations.AddOrUpdate(x => x.Id,
                new SubNavigation()
                {
                    ActionUrl = "/Settings/PermissionMatrix/",
                    ActionUrlRequestType = 1,
                    Icon = "fa-archive",
                    Text = "Permission Matrix",
                    NavigationId = 3
                });
            context.SaveChanges();

            #endregion

        }
    }
}
