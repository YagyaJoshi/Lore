using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Provider
{
    public class LoreProvider : ILoreProvider
    {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ICommonProvider _commonProvider;
        private IContentProvider _contentProvider;
        public static Int64? roleIdvalue = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        int timeout = 3600;
        public string Classifiedpath;

        public LoreProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ICommonProvider commonProvider)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _configProvider = configProvider;
            _commonProvider = commonProvider;
        }

        #region: FrontEnd Menu

        private CategoryViewModel ToCategoryViewModel(Category category)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();

            return new CategoryViewModel()
            {
                //UserId = User.Id,
                Id = category.Id,
                Category = category.CategoryName,
                ImagePath = category.ImagePath,
                Description = category.Description,
                CreatedById = Convert.ToInt64(category.CreatedById),
                CreationDate = Convert.ToDateTime(category.CreationDate),
                ModifiedById = Convert.ToInt64(category.ModifiedById),
                ModificationDate = Convert.ToDateTime(category.ModificationDate),
                StatusId = Convert.ToInt16(category.StatusId)
            };
        }

        public List<CategoryViewModel> GetAllCategoryList()
        {
            try
            {
                var result = _context.Categories.Where(x => x.StatusId != 4)
                .Select(ToCategoryViewModel).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<CategoryViewModel>();
            }
        }

        #endregion

        public ShippingViewModel GetUserShippingDetails(Int64 userid)
        {
            try
            {
                var record = _context.ShippingDetails.FirstOrDefault(x => x.MasterUsersId == userid);
                return ToShippingViewModel(record);
            }
            catch (Exception ex)
            {
                return new ShippingViewModel();
            }
        }

        private ShippingViewModel ToShippingViewModel(ShippingDetail ship)
        {
            try
            {
                return new ShippingViewModel()
                {
                    AddressLine1 = ship.ShippingAddressLine1,
                    AddressLine2 = ship.ShippingAddressLine2,
                    City = ship.ShippingCity,
                    CompanyName = ship.ShippingCompany,
                    CountryId = ship.ShippingCountryId,
                    EmailId = ship.ShippingEmailId,
                    Fax = ship.ShippingFax,
                    FirstName = ship.ShippingFirstName,
                    Id = ship.Id,
                    LastName = ship.ShippingLastName,
                    MobileNo = ship.ShippingMobileNo,
                    MasterUsersId = ship.MasterUsersId,
                    ShopName = ship.ShopName,
                    ZipCode = ship.ShippingZipCode,
                    StateId = ship.ShippingStateId,
                    StateName = ship.ShippingStateName,
                    CreatedById = ship.CreatedById,
                    CreationDate = DateTime.UtcNow,
                    StatusId = ship.StatusId
                };
            }
            catch (Exception ex)
            {
                return new ShippingViewModel();
            }
        }


        #region: DashBoard

        public String SaveBillingAddresses(AddressViewModel model)
        {
            try
            {
                var ShipChk = _context.MasterUsers.FirstOrDefault(x => x.Id == model.Id);
                if (ShipChk != null)
                {
                    ShipChk.FirstName = model.FirstName;
                    ShipChk.LastName = model.LastName;
                    ShipChk.AddressLine1 = model.AddressLine1;
                    ShipChk.AddressLine2 = model.AddressLine2;
                    ShipChk.CountryId = model.CountryId;
                    ShipChk.StateId = model.StateId;
                    ShipChk.StateName = model.StateName;
                    ShipChk.City = model.City;
                    ShipChk.ZipCode = model.ZipCode;
                    ShipChk.Mobile = model.MobileNo;
                    ShipChk.ModificationDate = DateTime.UtcNow;
                    ShipChk.ModifiedById = model.ModifiedById;
                }
                _context.SaveChanges();
                return "Success";
            }

            catch (Exception ex)
            {
                return "error";
            }

        }

        public String SaveAddresses(AddressViewModel model)
        {
            try
            {
                var ShipChk = _context.ShippingDetails.FirstOrDefault(x => x.Id == model.Id);
                if (ShipChk == null)
                {
                    var FirstName = _context.ShippingDetails.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (FirstName != null)
                    {
                        return "Name Aleardy Exist";
                    }

                    ShippingDetail ship = new ShippingDetail()
                    {
                        ShippingFirstName = model.ShippingFirstName,
                        ShippingLastName = model.ShippingLastName,
                        ShippingEmailId = model.ShippingEmailId,
                        ShippingAddressLine1 = model.ShippingAddressLine1,
                        ShippingMobileNo = model.ShippingMobileNo,
                        CreatedById = model.CreatedById,
                        CreationDate = DateTime.UtcNow,
                    };

                    _context.ShippingDetails.Add(ship);
                }
                else
                {
                    var ShopName = _context.ShippingDetails.Where(x => x.Id == model.Id && x.Id != model.Id && x.StatusId != 4).FirstOrDefault();
                    if (ShopName != null)
                    {
                        return "Name Aleardy Exist";
                    }

                    ShipChk.ShippingFirstName = model.ShippingFirstName;
                    ShipChk.ShippingLastName = model.ShippingLastName;
                    ShipChk.ShippingAddressLine1 = model.ShippingAddressLine1;
                    ShipChk.ShippingMobileNo = model.ShippingMobileNo;
                    ShipChk.ModificationDate = DateTime.UtcNow;
                    ShipChk.ModifiedById = model.ModifiedById;
                }
                _context.SaveChanges();
                return "Success";
            }

            catch (Exception ex)
            {
                return "error";
            }

        }

        public String SaveAccountDetailsFirst(FrontendAccountdetailViewModel model)
        {
            try
            {
                var chk = _context.MasterUsers.FirstOrDefault(x => x.Id == model.Id);
                if (chk != null)
                {
                    chk.FirstName = model.FirstName;
                    chk.LastName = model.LastName;
                    chk.Email = model.EmailId;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = model.Id;
                }
                _context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public AccountDetailsViewModel ToAccountDetailsViewModel(MasterUser user)
        {
            try
            {
                return new AccountDetailsViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    EmailId = user.Email,
                    CreatedById = Convert.ToInt64(user.CreatedById),
                    CreationDate = Convert.ToDateTime(user.CreationDate),
                    ModifiedById = Convert.ToInt64(user.ModifiedById),
                    ModificationDate = Convert.ToDateTime(user.ModificationDate),
                    StatusId = Convert.ToInt16(user.StatusId),
                };
            }
            catch (Exception ex)
            {
                return new AccountDetailsViewModel();
            }

        }

        public List<AccountDetailsViewModel> GetAllAccountList()
        {
            var result = _context.MasterUsers.Where(x => x.Id != 4)
                .Select(ToAccountDetailsViewModel).ToList();
            return result;
        }

        public AccountDetailsViewModel GetAccountDetails(Int64 id)
        {
            MasterUser user = _context.MasterUsers.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                return ToAccountDetailsViewModel(user);
            }
            else
            {
                return null;
            }
        }

        #endregion

        public List<TempCartViewModel> GetTempcartList(Int64 userid)
        {
            try
            {
                return _context.TempCarts.Where(x => x.UserId == userid).ToList().Select(ToTempcartViewModel).ToList();
            }
            catch (Exception ex)
            {
                return new List<TempCartViewModel>();
            }
        }

        private TempCartViewModel ToTempcartViewModel(TempCart data)
        
        {
            try
            {
                string colorname = "";
                try
                {
                    colorname = _context.Colours.FirstOrDefault(x => x.Id == data.ColourId).ColourName;
                }
                catch (Exception ex)
                {
                }

                return new TempCartViewModel()
                {
                    ColourId = data.ColourId,
                    ColourName = colorname,
                    //CreatedById = data.CreatedById,
                    Id = data.Id,
                    Picture1 = data.Picture1,
                    PriceUSD = data.PriceUSD,
                    ProductId = data.ProductId,
                    ProductName = data.ProductName,
                    Quantity = data.Quantity,
                    SizeUK = data.SizeUK,
                    Total = data.Total,
                    UserId = data.UserId
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetTempcartCount(Int64 userid)
        {
            try
            {
                return _context.TempCarts.Where(x => x.UserId == userid).ToList().Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string SaveFrontendOrder(CheckoutViewModel Model)
        {
            try
            {
                decimal total = 0;
                int totqty = 0;
                foreach (var data in Model.CartList)
                {
                    total = total + data.Total;
                    totqty = totqty + data.Quantity;
                }
                //Model.Amount = total;
               
               
                decimal shippingcharge = 0;
                decimal Tax = 0;
                decimal TotalAmount = 0;
         
                var totlamt = total;
                try
                {
              ;
                    decimal charge = (totlamt * 5) / 100;
                    if (charge < 50)
                    {
                        shippingcharge = 50;
                    }
                    else
                    {
                       // model.ShippingCharge = charge;
                        shippingcharge = charge;
                    }
                    TotalAmount = totlamt + shippingcharge;
                }
                catch (Exception)
                {
                    TotalAmount = totlamt;
                }

       //         string deliverydate = DateTime.UtcNow.AddDays(60).ToString("MM/dd/yyyy");
                DateTime? wearDate, userSelectDeliveryDate = null;
                try
                {
                    wearDate = (!String.IsNullOrEmpty(Model.WearDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.WearDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                }
                catch (Exception ex)
                {   wearDate = null;   }

                try
                {
                    userSelectDeliveryDate = (!String.IsNullOrEmpty(Model.DeliveryDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.DeliveryDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                }
                catch (Exception ex)
                { userSelectDeliveryDate = null; }

                OrderMaster Master = new OrderMaster()
                {
                    CustomerId = Model.UserModel.Id,
                    CustomerFullName = Model.UserModel.CustomerFullName,
                    ZipCode = Model.UserModel.ZipCode,
                    CurrencyName = Model.UserModel.CurrencyName.Trim(),
                    MobileNo = Model.UserModel.MobileNo,
                    ShippingCharge = shippingcharge,
                    //TaxEnable = Model.TaxEnable,
                    Tax = Tax,
                    EmailId = Model.UserModel.EmailId,
                    OrderNote = Model.OrderNotes,
                    WearDate = wearDate,
                    UserSelectDeliveryDate = userSelectDeliveryDate,
                    BridesName = Model.BridesName,
                    PONumber = Model.PurchaseOrderNumber,
                    OrderDate = DateTime.UtcNow,        //Convert.ToDateTime(DateTime.ParseExact(Model.OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)),
                    WareHouseId = Convert.ToInt64(Model.UserModel.DistributionPointId),
                    TotalProduct = totqty,// Model.ProductList.Count,
                   // DeliveryDate = deliverydate,         //Model.DeliveryDate,
                    ShippingState = Model.UserModel.ShippingState,
                    Amount = total,
                    TotalAmount = TotalAmount,
                    OrderStatusId = 1,
                    StatusId = 1,
                    CreationDate = DateTime.UtcNow,
                    CreatedById = Model.UserModel.Id,
                    OrderlocatorId=10,
                };
                _context.OrderMasters.Add(Master);
                _context.SaveChanges();

                //Update Order Number
                string orderno = "";
                OrderMaster om = _context.OrderMasters.FirstOrDefault(x => x.Id == Master.Id);
                if (om != null)
                {                    
                    if (Model.UserModel.DistributionPointId == 2)
                    {
                        orderno = "RO" + Master.Id + "UK";
                    }
                    else
                    {
                        orderno = "RO" + Master.Id + "W";
                    }

                    om.OrderNo = orderno;
                    _context.SaveChanges();
                }
                Model.OrderMasterId = Master.Id;

                //Update Stock Inventory
                UpdateOrderInventory(Model);
                var userdetail = _context.MasterUsers.Where(k => k.Id == Model.UserModel.Id).FirstOrDefault();
                var CountryName = _context.Countries.Where(x => x.Id == userdetail.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
               
                    //Save Details
                    foreach (var list in Model.CartList)
                {
                    string sizeuk = Convert.ToString(list.SizeUK);
                    if (CountryName == "United States")
                    {
                        string sizuk = _context.Sizes.Where(k => k.SizeNameUS == sizeuk).Select(k => k.SizeNameUK).FirstOrDefault();
                        sizeuk = sizuk;
                    }
                    else if (CountryName == "Austria" || CountryName == "Belgium" || CountryName == "Cyprus" || CountryName == "Estonia" || CountryName == "Finland" || CountryName == "France" || CountryName == "Germany" || CountryName == "Greece" || CountryName == "Ireland" || CountryName == "Italy" || CountryName == "Latvia" || CountryName == "Lithuania" || CountryName == "Luxembourg" || CountryName == "Malta" || CountryName == "Netherlands" || CountryName == "Portugal" || CountryName == "Slovakia" || CountryName == "Slovenia and Spain")
                    {
                        string sizuk = _context.Sizes.Where(k => k.SizeNameEU == sizeuk).Select(k => k.SizeNameUK).FirstOrDefault();
                        sizeuk = sizuk;
                    }
                    OrderDetail Details = new OrderDetail()
                    {
                        OrderMasterId = Model.OrderMasterId,
                        ProductId = list.ProductId,
                        //SizeId
                        OrderPrice = list.PriceUSD,
                        SizeUK = sizeuk,
                        ColourId = list.ColourId,
                        Qty = list.Quantity,
                        StatusId = 1,
                        CreatedById = Model.UserModel.Id,
                        CreationDate = DateTime.UtcNow
                    };
                    _context.OrderDetails.Add(Details);
                    _context.SaveChanges();
                }

                return orderno;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public bool UpdateOrderInventory(CheckoutViewModel Model)
        {
            try
            {
                Int64 distributionpoint = Convert.ToInt64(Model.UserModel.DistributionPointId);
                var stoqua = _context.StockQuantities.Where(k => k.ReferenceId == Model.OrderMasterId).ToList();
                foreach (var item in stoqua)
                {
                    _context.StockQuantities.Remove(item);
                    _context.SaveChanges();
                }

                var userdetail = _context.MasterUsers.Where(k => k.Id == Model.UserModel.Id).FirstOrDefault();
                var CountryName = _context.Countries.Where(x => x.Id == userdetail.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();

                foreach (var list in Model.CartList)
                {
                    string sizeuk = Convert.ToString(list.SizeUK);
                    if (CountryName == "United States")
                    {
                        string sizuk = _context.Sizes.Where(k => k.SizeNameUS == sizeuk).Select(k => k.SizeNameUK).FirstOrDefault();
                        sizeuk = sizuk;
                    }
                    else if (CountryName == "Austria" || CountryName == "Belgium" || CountryName == "Cyprus" || CountryName == "Estonia" || CountryName == "Finland" || CountryName == "France" || CountryName == "Germany" || CountryName == "Greece" || CountryName == "Ireland" || CountryName == "Italy" || CountryName == "Latvia" || CountryName == "Lithuania" || CountryName == "Luxembourg" || CountryName == "Malta" || CountryName == "Netherlands" || CountryName == "Portugal" || CountryName == "Slovakia" || CountryName == "Slovenia and Spain")
                    {
                        string sizuk = _context.Sizes.Where(k => k.SizeNameEU == sizeuk).Select(k => k.SizeNameUK).FirstOrDefault();
                        sizeuk = sizuk;
                    }
                    StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "Order" && x.SizeUK == sizeuk && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == distributionpoint && x.StatusId != 4).FirstOrDefault();
                    OrderDetail orderDetail = _context.OrderDetails.FirstOrDefault(x => x.OrderMasterId == Model.OrderMasterId && x.SizeUK == sizeuk && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.StatusId != 4);

                   // if (StockDetail == null)
                    {
                        StockQuantity SQ = new StockQuantity();
                        SQ.InventoryType = "Order";
                        SQ.SizeUK = sizeuk;
                        SQ.ProductId = list.ProductId;
                        SQ.ColourId = list.ColourId;
                        SQ.WareHouseId = distributionpoint;
                        SQ.Qty = list.Quantity;
                        SQ.CreatedById = Model.UserModel.Id;
                        SQ.CreationDate = DateTime.UtcNow;
                        SQ.ReferenceId = Model.OrderMasterId;
                        _context.StockQuantities.Add(SQ);
                    }
              
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

    }
}
