using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Data.Entities;


namespace Loregroup.Data {
    public class AppContext : DbContext {
        public AppContext()
            : base("name=DefaultConnection")
        {
        }        
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
       
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<UserDesignation> UserDesignations { get; set; }
       
        public DbSet<Role> Roles { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<SubNavigation> SubNavigations { get; set; }
        //public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
       // public DbSet<DashBoradWidget> Widgets { get; set; }
        
        public DbSet<PermissionMatrix> PermissionMatrixs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<MasterUser> MasterUsers { get; set; }
        public DbSet<Block> Blocks { get; set; }
      
        public DbSet<Tbl_AccountInfo> Tbl_AccountInfo { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<ConsumersTemp> ConsumersTemp { get; set; }
        public DbSet<RefillRate> RefillRates { get; set; }

        public DbSet<DSLog> DSLogs { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CollectionYear> CollectionYears { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<CutOfDress> CutOfDresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Fabric> Fabrics { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FileLibrary> FileLibraries { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderMaster> OrderMasters { get; set; }

        public DbSet<WareHouse> WareHouses { get; set; }        
        public DbSet<PurchaseOrderMaster> PurchaseOrderMasters { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public DbSet<StockQuantity> StockQuantities { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<FrontendContent> FrontendContents { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<ShippingDetail> ShippingDetails { get; set; }
        public DbSet<TempCart> TempCarts { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }

        public DbSet<OrderLocator> OrderLocators { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
