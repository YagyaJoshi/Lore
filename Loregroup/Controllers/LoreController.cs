using Ionic.Zip;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Loregroup.Controllers
{
    public class LoreController : Controller
    {
        private readonly ISecurity _security;
        private readonly IUserProvider _userProvider;
        private readonly ISession _session;
        private readonly IErrorHandler _errorHandler;
        private readonly INotificationProvider _notificationprovider;
        private readonly AppContext _context;
        private readonly ICacheService _dataCache;
        private readonly ILoreProvider _loreProvider;
        private readonly IProductProvider _productProvider;
        private readonly ISettingProvider _settingProvider;
        private readonly IOrderProvider _orderProvider;
        private readonly IMasterProvider _masterProvider;

        public const int PageSize = 12;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        int timeout = 3600;

        public LoreController(AppContext context, IErrorHandler errorHandler, ISession session, IUserProvider userProvider, INotificationProvider notificationprovider, ICacheService dataCache, ILoreProvider loreProvider, IProductProvider productProvider, ISettingProvider settingProvider, IOrderProvider orderProvider, ISecurity security, IMasterProvider masterProvider)
        {
            _security = security;
            _errorHandler = errorHandler;
            _session = session;
            _userProvider = userProvider;
            _notificationprovider = notificationprovider;
            _context = context;
            _dataCache = dataCache;
            _loreProvider = loreProvider;
            _productProvider = productProvider;
            _settingProvider = settingProvider;
            _orderProvider = orderProvider;
            _masterProvider = masterProvider;
        }

        // GET: Lore        //FrontendLayoutModel
        public ActionResult Index()
        {
            FrontendHomeViewModel model = new FrontendHomeViewModel();
            model.CategoryList = _loreProvider.GetAllCategoryList();
            model.SlidersList = _settingProvider.GetAllSlidersList();
            model.ProductList = _context.Products.Where(x => x.StatusId == (int)Status.Active && x.PublishId == 1).Take(10).ToList().Select(ToProductViewModel).ToList();

            return View(model);
        }

        public ActionResult ContactUs()
        {
            ContactViewModel model = new ContactViewModel();
            model.ContactList = _productProvider.GetAllContactList().OrderByDescending(y => y.Id).ToList();
            return View(model);
        }

        //[HttpGet]
        public string LoreGroupContactUs(string name, string emailid, string message)
        {
            string returnvalue = "";
            try
            {
                string returnresult = LoreGroupSendEmail(name, emailid, "", message);
                if (returnresult == "Email Sent")
                {
                    returnvalue = "Thankyou! We will contact you soon..";
                }
                else
                {
                    returnvalue = returnresult;
                }
            }
            catch (Exception ex)
            {
                returnvalue = "Some error occured.";
            }
            return returnvalue;
        }

        public string LoreGroupSendEmail(string Name, string EmailId, string Subject, string Message)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello,</p></td></tr>"
                                 + "<tr><td align='left'><p>A user/customer want to contacting us. User's Detail -</p></td></tr><br>"
                                 + "<tr><td align='left'><p>Name : <b>" + Name + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Email-Id : <b>" + EmailId + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Message : <b>" + Message + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>&nbsp;</p></td></tr>"
                                 + "<tr><td align='right'><p>Thanks And Regards,</p></td></tr>"
                                 + "<tr><td align='right'><p>Lore-Group.</p></td></tr>";

                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));

                // set the network credentials
                mailClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);

                // enable ssl
                mailClient.EnableSsl = true;

                // Create the Mail Message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["confirmationEmail"], "Loré Fashions");
                mailMessage.To.Add(ConfigurationManager.AppSettings["confirmationEmail"]);
                mailMessage.To.Add("sales@lore-group.com");
                // mailMessage.CC.Add(usermailid);
                mailMessage.Subject = "User/Customer send a Message";
                mailMessage.Body = mailFormat;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                //Send the mail
                mailClient.Send(mailMessage);
                return "Email Sent";
            }
            catch (Exception ex)
            {
                return "Fail - " + ex.Message;
            }
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Products(Int64 CategoryId = 0)
        {
            ProductCollectionViewModel model = new ProductCollectionViewModel();
            Int64 UserId = 0;
            try
            {
                UserId = Convert.ToInt32(Session["FrontendUserId"]);
                model.Userid = UserId;
              
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Lore");
            }

            string catid = CategoryId.ToString();
            IQueryable<Data.Entities.Product> query = _context.Products.Where(x => x.StatusId == (int)Status.Active).Where(x => x.PublishId != 0);
            if (UserId > 0)
            {
                query = query.Where(x => x.PublishId == 1 || x.PublishId == 2);
            }
            else
            {
                query = query.Where(x => x.PublishId == 1);
            }
            model.ProductList.Data = query.Where(x => x.SeletedCategoryIds.Contains(catid)).OrderByDescending(p => p.Id).Take(PageSize).ToList().Select(ToProductViewModel).ToList();
            model.ProductList.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)query.Where(x => x.SeletedCategoryIds.Contains(catid)).ToList().Count / PageSize));
            model.ProductList.CurrentPage = 1;
            model.ProductList.TableStyle = "isotope-item";

            try
            {
                model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
                model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
                model.CollectionList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
                model.CollectionList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });
                model.ColourList = _productProvider.GetAllColourList().OrderBy(x => x.Colour).ToList();
                model.ColourList.Insert(0, new ColourViewModel { Id = 0, Colour = "--Select--" });
                model.CutofdressList = _productProvider.GetAllCutOfDressList().OrderBy(x => x.CutOfDress).ToList();
                model.CutofdressList.Insert(0, new CutOfDressViewModel { Id = 0, CutOfDress = "--Select--" });
                model.FabricList = _productProvider.GetAllFabricList().OrderBy(x => x.FabricName).ToList();
                model.FabricList.Insert(0, new FabricViewModel { Id = 0, FabricName = "--Select--" });
            }
            catch (Exception ex)
            {
            }
            Session["CollectionId"] = 0;
            Session["CutofdressId"] = 0;
            Session["colorid"] = 0;
            Session["fabricid"] = 0;
            return View(model);
        }

        [HttpPost]
        public ActionResult Products(ProductCollectionViewModel model)
        {
            List<ProductViewModel> dataabc = new List<ProductViewModel>();
            var people = new PagedData<ProductViewModel>();

            //people.Data = _context.Products.OrderBy(p => p.Id).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            //var records = _context.Products.OrderBy(p => p.Id).Skip(PageSize * (page - 1)).Take(PageSize).ToList().Select(ToProductViewModel).ToList();

            List<Data.Entities.Product> records = new List<Data.Entities.Product>();
            string catid = model.CategoryId.ToString();
            string colorid = model.ColourId.ToString();
            string fabricid = model.FabricId.ToString();

            Int64 UserId = 0;
            try
            {
                UserId = Convert.ToInt32(Session["FrontendUserId"]);
                model.Userid = UserId;
            }
            catch (Exception ex)
            { }

            IQueryable<Data.Entities.Product> query = _context.Products.Where(x => x.StatusId == (int)Status.Active).Where(x => x.PublishId != 0);
            if (UserId > 0)
            {
                query = query.Where(x => x.PublishId == 1 || x.PublishId == 2);
            }
            else
            {
                query = query.Where(x => x.PublishId == 1);
            }

            if (model.CategoryId > 0)
            {
                query = query.Where(x => x.SeletedCategoryIds.Contains(catid));
            }
            
            if (model.CollectionId > 0)
            {
                query = query.Where(x => x.CollectionYearId == model.CollectionId);
            }
            if (model.CutofdressId > 0)
            {
                query = query.Where(x => x.CutOfDressId == model.CutofdressId);
            }
            if (model.ColourId > 0)
            {
                query = query.Where(x => x.SeletedColorIds.Contains(colorid));
            }
            if (model.FabricId > 0)
            {
                query = query.Where(x => x.SelectedFabricIds.Contains(fabricid));
            }

            if (model.ProductList.CallingPage > 0)
            {
                records = query.OrderByDescending(p => p.Id).Skip(PageSize * (model.ProductList.CallingPage - 1)).Take(PageSize).ToList();
            }
            else
            {
                records = query.OrderByDescending(p => p.Id).Take(PageSize).ToList();     //.Select(ToProductViewModel).ToList();                
            }

            int i = 0;
            foreach (var item in records)
            {
                i++;
                ProductViewModel p = new ProductViewModel();
                p.Id = item.Id;
                p.ProductName = item.ProductName;
                p.PriceUSD = item.PriceUSD;
                p.PriceEURO = item.PriceEURO;
                p.PriceGBP = item.PriceGBP;
                p.Picture1 = item.Picture1;
                p.styleset = GetStyleSet(i);
                dataabc.Add(p);
            }

            people.Data = dataabc;
            people.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)query.ToList().Count / PageSize));
            try
            {
                if (Convert.ToInt64(Session["CollectionId"]) != model.CollectionId)
                {
                    model.ProductList.CallingPage = 0;
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                if (Convert.ToInt64(Session["CutofdressId"]) != model.CutofdressId)
                {
                    model.ProductList.CallingPage = 0;
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                if (Convert.ToString(Session["colorid"]) != colorid)
                {
                    model.ProductList.CallingPage = 0;
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                if (Convert.ToString(Session["fabricid"]) != fabricid)
                {
                    model.ProductList.CallingPage = 0;
                }
            }
            catch (Exception ex)
            {

            }
            if (model.ProductList.CallingPage > 0)
            {
                people.CurrentPage = model.ProductList.CallingPage;
            }
            else
            {
                people.CurrentPage = 1;
            }

            people.TableStyle = "isotope-item";

            model.ProductList = people;

            try
            {
                model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
                model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
                model.CollectionList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
                model.CollectionList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });
                model.ColourList = _productProvider.GetAllColourList().OrderBy(x => x.Colour).ToList();
                model.ColourList.Insert(0, new ColourViewModel { Id = 0, Colour = "--Select--" });
                model.CutofdressList = _productProvider.GetAllCutOfDressList().OrderBy(x => x.CutOfDress).ToList();
                model.CutofdressList.Insert(0, new CutOfDressViewModel { Id = 0, CutOfDress = "--Select--" });
                model.FabricList = _productProvider.GetAllFabricList().OrderBy(x => x.FabricName).ToList();
                model.FabricList.Insert(0, new FabricViewModel { Id = 0, FabricName = "--Select--" });
            }
            catch (Exception ex)
            {
            }
            Session["CollectionId"] = model.CollectionId;
            Session["CutofdressId"] = model.CutofdressId;
            Session["colorid"] = colorid;
            Session["fabricid"] = fabricid;
            return View(model);
        }

        public ActionResult ProductList(int page)
        {
            ProductCollectionViewModel model = new ProductCollectionViewModel();

            List<ProductViewModel> dataabc = new List<ProductViewModel>();
            var people = new PagedData<ProductViewModel>();

            //people.Data = _context.Products.OrderBy(p => p.Id).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            //var records = _context.Products.OrderBy(p => p.Id).Skip(PageSize * (page - 1)).Take(PageSize).ToList().Select(ToProductViewModel).ToList();

            var records = _context.Products.OrderByDescending(p => p.Id).Skip(PageSize * (page - 1)).Take(PageSize).ToList();

            int i = 0;
            foreach (var item in records)
            {
                i++;
                ProductViewModel p = new ProductViewModel();
                p.Id = item.Id;
                p.ProductName = item.ProductName;
                p.PriceUSD = item.PriceUSD;
                p.Picture1 = item.Picture1;
                p.styleset = GetStyleSet(i);
                dataabc.Add(p);
            }

            people.Data = dataabc;
            people.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)_context.Products.Count() / PageSize));
            //people.NumberOfPages = Convert.ToInt32(people.Data.Count() / PageSize);
            people.CurrentPage = page;
            people.TableStyle = "isotope-item";

            model.ProductList = people;
            return PartialView(model);
        }

        public string GetStyleSet(int j)
        {
            if (j == 1)
            {
                return "position: absolute; left: 0%; top: 0px;";
            }
            else if (j == 2)
            {
                return "position: absolute; left: 25%; top: 0px;";
            }
            else if (j == 3)
            {
                return "position: absolute; left: 50%; top: 0px;";
            }
            else if (j == 4)
            {
                return "position: absolute; left: 75%; top: 0px;";
            }
            else if (j == 5)
            {
                return "position: absolute; left: 0%; top: 445px;";
            }
            else if (j == 6)
            {
                return "position: absolute; left: 25%; top: 445px;";
            }
            else if (j == 7)
            {
                return "position: absolute; left: 50%; top: 445px;";
            }
            else if (j == 8)
            {
                return "position: absolute; left: 75%; top: 445px;";
            }
            else
            {
                return "";
            }
        }

        private ProductViewModel ToProductViewModel(Data.Entities.Product data)
        {
            return new ProductViewModel()
            {
                Id = data.Id,
                ProductName = data.ProductName,
                PriceUSD = data.PriceUSD,
                PriceEURO = data.PriceEURO,
                PriceGBP = data.PriceGBP,
                Picture1 = data.Picture1
            };
        }

        public ActionResult ProductDetails(Int64 ProductId)
        {
            try
            {
                ProductViewModel model = _productProvider.GetProduct(ProductId);

                List<ColourViewModel> clst = new List<ColourViewModel>();
                try
                {
                    string[] values = model.SeletedColorIds.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        int id = 0;
                        try
                        {
                            id = Convert.ToInt32(values[i].Trim());
                        }
                        catch (Exception ex)
                        { }

                        if (id > 0)
                        {
                            var data = _context.Colours.FirstOrDefault(x => x.Id == id);
                            ColourViewModel cm = new ColourViewModel();
                            cm.Id = data.Id;
                            cm.Colour = data.ColourName;
                            clst.Add(cm);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                clst.Insert(0, new ColourViewModel
                {
                    Id = 0,
                    Colour = "Select Color"
                });
                model.ColourList = clst;

                //// Add Size drop-down List
                ///
                Int64 Userid = 0;
                List<SizeFrontendViewModel> slst = new List<SizeFrontendViewModel>();
                try
                {
                    Userid = Convert.ToInt64(Session["FrontendUserId"]);
                    if (Userid > 0)
                    {
                        var sizelist = _productProvider.GetAllSizeList();
                        var userdetail = _context.MasterUsers.Where(k => k.Id == Userid).FirstOrDefault();
                        var CountryName = _context.Countries.Where(x => x.Id == userdetail.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                        if (CountryName == "United States")
                        {
                            foreach (var item in sizelist)
                            {
                                //if (item.SizeUS != "0")
                                {
                                    SizeFrontendViewModel sm = new SizeFrontendViewModel();
                                    sm.Id = Convert.ToInt32(item.SizeUS);// Convert.ToInt32(item.Id);// i;
                                    sm.SizeUk = Convert.ToInt32(item.SizeUS);
                                    sm.SizeString = "US-" + item.SizeUS+"/"+ "UK-" + item.SizeUK + "/" + "EU-" + item.SizeEU;
                                    slst.Add(sm);
                                }
                            }
                        }
                        else if (CountryName == "United Kingdom")
                        {
                            foreach (var item in sizelist)
                            {
                                // if (item.SizeUK != "0" && item.SizeEU != "0")
                                {
                                    SizeFrontendViewModel sm = new SizeFrontendViewModel();
                                    sm.Id = Convert.ToInt32(item.SizeUK);// Convert.ToInt32(item.Id);// i;
                                    sm.SizeUk = Convert.ToInt32(item.SizeUK);
                                    sm.SizeString = "UK-" + item.SizeUK + "/" + "EU-" + item.SizeEU+"/"+ "US-" + item.SizeUS;
                                    slst.Add(sm);
                                }
                            }
                            //if (CountryName == "Austria" || CountryName == "Belgium" || CountryName == "Cyprus" || CountryName == "Estonia" || CountryName == "Finland" || CountryName == "France" || CountryName == "Germany" || CountryName == "Greece" || CountryName == "Ireland" || CountryName == "Italy" || CountryName == "Latvia" || CountryName == "Lithuania" || CountryName == "Luxembourg" || CountryName == "Malta" || CountryName == "Netherlands" || CountryName == "Portugal" || CountryName == "Slovakia" || CountryName == "Slovenia and Spain")
                            //{

                            //foreach (var item in sizelist)
                            //{
                            //    if (item.SizeEU != "0")
                            //    {
                            //        SizeFrontendViewModel sm = new SizeFrontendViewModel();
                            //        sm.Id = Convert.ToInt32(item.Id);// i;
                            //        sm.SizeUk = Convert.ToInt32(item.Id);
                            //        sm.SizeString = "EU-" + item.SizeEU;
                            //        slst.Add(sm);
                            //    }
                            //}
                            // }
                        }
                        else if (CountryName == "Austria" || CountryName == "Belgium" || CountryName == "Cyprus" || CountryName == "Estonia" || CountryName == "Finland" || CountryName == "France" || CountryName == "Germany" || CountryName == "Greece" || CountryName == "Ireland" || CountryName == "Italy" || CountryName == "Latvia" || CountryName == "Lithuania" || CountryName == "Luxembourg" || CountryName == "Malta" || CountryName == "Netherlands" || CountryName == "Portugal" || CountryName == "Slovakia" || CountryName == "Slovenia" || CountryName== "Spain")
                        {

                            foreach (var item in sizelist)
                            {
                                //  if (item.SizeEU != "0" && item.SizeUK != "0")
                                {
                                    SizeFrontendViewModel sm = new SizeFrontendViewModel();
                                    sm.Id = Convert.ToInt32(item.SizeEU);// Convert.ToInt32(item.Id);// i;
                                    sm.SizeUk = Convert.ToInt32(item.SizeEU);
                                    // sm.SizeString = "EU-" + item.SizeEU;
                                    sm.SizeString = "UK-" + item.SizeUK + "/" + "EU-" + item.SizeEU + "/" + "US-" + item.SizeUS;
                                    slst.Add(sm);
                                }

                            }
                            //foreach (var item in sizelist)
                            //{
                            //    if (item.SizeUK != "0")
                            //    {
                            //        SizeFrontendViewModel sm = new SizeFrontendViewModel();
                            //        sm.Id = Convert.ToInt32(item.Id);// i;
                            //        sm.SizeUk = Convert.ToInt32(item.Id);
                            //        sm.SizeString = "UK-" + item.SizeUK;
                            //        slst.Add(sm);
                            //    }
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {

                }



                //for (int i = 2; i <= 34; i = i + 2)
                //{
                //    SizeFrontendViewModel sm = new SizeFrontendViewModel();
                //    sm.Id = i;
                //    sm.SizeUk = i;
                //    sm.SizeString = "UK-" + i;
                //    slst.Add(sm);
                //}
                slst.Insert(0, new SizeFrontendViewModel
                {
                    Id = 0,
                    SizeUk = 0,
                    SizeString = "Select Size"
                });
                model.SizeList = slst;

                try
                {
                    model.FrontendLoginUserId = Convert.ToInt64(Session["FrontendUserId"]);
                    if (model.FrontendLoginUserId > 0)
                    {
                        Int64 WarehouseId = _context.MasterUsers.FirstOrDefault(x => x.Id == model.FrontendLoginUserId).DistributionPointId.Value;
                        model.ProductAvailabilityList = _orderProvider.GetProductInventoryCount(ProductId, (int)WarehouseId);
                    }
                }
                catch (Exception)
                {
                    model.FrontendLoginUserId = 0;
                }

                _masterProvider.SaveUserLog("Product Detail", "User visits the product : " + model.ProductName, model.FrontendLoginUserId);
                return View(model);
            }
            catch (Exception ex)
            {
                ProductViewModel model = new ProductViewModel();
                return View(model);
            }
        }

        public ActionResult ProductPopDetail(int Id)
        {
            ProductViewModel model = _productProvider.GetProduct(Id);

            List<ColourViewModel> clst = new List<ColourViewModel>();
            try
            {
                string[] values = model.SeletedColorIds.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    int id = 0;
                    try
                    {
                        id = Convert.ToInt32(values[i].Trim());
                    }
                    catch (Exception ex)
                    { }

                    if (id > 0)
                    {
                        var data = _context.Colours.FirstOrDefault(x => x.Id == id);
                        ColourViewModel cm = new ColourViewModel();
                        cm.Id = data.Id;
                        cm.Colour = data.ColourName;
                        clst.Add(cm);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            clst.Insert(0, new ColourViewModel
            {
                Id = 0,
                Colour = "Select Color"
            });
            model.ColourList = clst;

            return PartialView("_ProductPopup", model);
        }

        public ActionResult TradeShows()
        {
            EventViewModel model = new EventViewModel();
            model.EventList = _productProvider.GetAllEventsListByEventtype("TradeShows");
            return View(model);
        }

        public ActionResult TrunkShows()
        {
            EventViewModel model = new EventViewModel();
            model.EventList = _productProvider.GetAllEventsListByEventtype("TrunkShows");
            return View(model);
        }

        public ActionResult FAQ()
        {
            FaqViewModel model = new FaqViewModel();
            model.FaqList = _productProvider.GetAllFaqList();
            return View(model);
        }

        public ActionResult FrontendContent()
        {
            BaseEntityViewModel model = new BaseEntityViewModel();
            model = _settingProvider.GetBaseEntityByName("PrivacyPolicy");
            return View(model);
        }

        public ActionResult FrontAboutUs()
        {
            BaseEntityViewModel model = new BaseEntityViewModel();
            model = _settingProvider.GetBaseEntityByName("AboutUs");
            //SearchByBaseEntityName(string PrivacyPolicy);
            return View(model);
        }

        public ActionResult FrontReturnPolicy()
        {
            BaseEntityViewModel model = new BaseEntityViewModel();
            model = _settingProvider.GetBaseEntityByName("ReturnPolicy");
            return View(model);
        }

        public ActionResult FrontTermsandConditions()
        {
            BaseEntityViewModel model = new BaseEntityViewModel();
            model = _settingProvider.GetBaseEntityByName("TermsandCondition");
            return View(model);
        }

        public ActionResult GalleryImages()
        {
            GalleryViewModel model = new GalleryViewModel();
            try
            {
                model.GalleryList = _settingProvider.GetAllGalleryList(1);
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        public ActionResult GalleryVideos()
        {
            GalleryViewModel model = new GalleryViewModel();
            try
            {
                model.GalleryList = _settingProvider.GetAllGalleryList(2);
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        public ActionResult Wishlist()
        {
            WishlistViewModel model = new WishlistViewModel();

            try
            {
                model.UserId = Convert.ToInt32(Session["FrontendUserId"]);

                model.WishlistList = _settingProvider.GetAllWishList(model.UserId);
                //  model.UserId = 1;
                return View(model);
            }
            catch (Exception ex)
            {
                model.UserId = 0;
                return View(model);
            }
        }

        public int AddIntoWishlist(Int64 PId, string PName, decimal PPriceUsd)
        {
            try
            {
                Int64 userid = Convert.ToInt32(Session["FrontendUserId"]);

                Wishlist w = new Wishlist()
                {
                    CreatedById = userid,
                    CreationDate = DateTime.Now,
                    Price = PPriceUsd,
                    ProductId = PId,
                    UserId = userid,
                    ProductName = PName,
                    StatusId = 1,
                    StockStatus = ""
                };
                _context.Wishlists.Add(w);
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ActionResult Checkout()
        {
            Int64 uid = Convert.ToInt64(Session["FrontendUserId"]);

            CheckoutViewModel model = new CheckoutViewModel();
            model.CartList = _loreProvider.GetTempcartList(uid);
            model.UserModel = _userProvider.GetUser(uid);
            try
            {
                var CountryName = _context.Countries.Where(x => x.Id == model.UserModel.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                if (CountryName == "United States")
                {
                    model.Countrynme = "US";
                }
                else if (CountryName == "United Kingdom")
                {
                    model.Countrynme = "UK";
                }
                else if (CountryName == "Austria" || CountryName == "Belgium" || CountryName == "Cyprus" || CountryName == "Estonia" || CountryName == "Finland" || CountryName == "France" || CountryName == "Germany" || CountryName == "Greece" || CountryName == "Ireland" || CountryName == "Italy" || CountryName == "Latvia" || CountryName == "Lithuania" || CountryName == "Luxembourg" || CountryName == "Malta" || CountryName == "Netherlands" || CountryName == "Portugal" || CountryName == "Slovakia" || CountryName == "Slovenia and Spain")
                {
                    model.Countrynme = "EU";
                }
            }
            catch (Exception)
            {

            }
            try
            {
                var totlamt = model.CartList.Sum(k => k.Total);
                model.TotalAmount = totlamt;
                decimal charge = (totlamt * 5) / 100;
                decimal finalchrge = 0;
                if(charge<50)
                {
                    model.ShippingCharge = 50;
                    finalchrge = 50;
                }
                else
                {
                    model.ShippingCharge = charge;
                    finalchrge = charge;
                }
                model.GrandTotalAmount = totlamt + finalchrge;
            }
            catch (Exception)
            {

            }
            //return View(model);
            return View(model);
        }

        public int SaveTempCart(string pname, Int64 pid, string ppicture, decimal ppriceusd, int pqty, int psize, Int64 pcolourid)
        {
            try
            {
                Int64 uid = Convert.ToInt64(Session["FrontendUserId"]);

                TempCart cart = new TempCart()
                {
                    ColourId = pcolourid,
                    Picture1 = ppicture,
                    PriceUSD = ppriceusd,
                    ProductId = pid,
                    ProductName = pname,
                    Quantity = pqty,
                    SizeUK = psize,
                    StatusId = 1,
                    Total = (psize >= 24) ? ((ppriceusd + calculateSurcharge(ppriceusd, 10)) * pqty) : (ppriceusd * pqty),
                    UserId = uid,
                    CreatedById = uid,
                    CreationDate = DateTime.Now,
                };
                _context.TempCarts.Add(cart);
                _context.SaveChanges();

                //return _userProvider.IsEmailExists(EmailId);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private decimal calculateSurcharge(decimal productPrice, decimal surchargeValue)
        {
            return Math.Round(((productPrice * surchargeValue) / 100), 2);
        }

        //Delete Item From TempCart
        public int DeleteItemFromTempcart(int TempId)
        {
            try
            {
                TempCart cart = _context.TempCarts.FirstOrDefault(x => x.Id == TempId);
                _context.TempCarts.Remove(cart);
                _context.SaveChanges();
                //return _userProvider.IsEmailExists(EmailId);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //Save Order From Customer Interface
        public int SaveFrontendOrder(string bridesName, string purchaseOrderNumber, string wearDate, string deliveryDate, string Notes)
        {
            try
            {
                Int64 uid = Convert.ToInt64(Session["FrontendUserId"]);

                CheckoutViewModel model = new CheckoutViewModel();
                model.CartList = _loreProvider.GetTempcartList(uid);
                model.UserModel = _userProvider.GetUser(uid);
                model.OrderNotes = Notes;
                model.BridesName = bridesName;
                model.WearDate = wearDate;
                model.PurchaseOrderNumber = purchaseOrderNumber;
                model.DeliveryDate = deliveryDate;

                decimal total = 0;
                string result = "";
                //if (Model.Id > 0)
                //{
                //    result = _orderProvider.UpdateOrder(Model);
                //}
                //else
                //{

                result = _loreProvider.SaveFrontendOrder(model);
                //}
                if (result != "")
                {
                    var carts = _context.TempCarts.Where(x => x.UserId == uid).ToList();
                    _context.TempCarts.RemoveRange(carts);
                    _context.SaveChanges();

                    FrontendOrderConfirmEmail(result, "", "", model.UserModel.ShopName);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string FrontendOrderConfirmEmail(string Name, string EmailId, string Password, string Message)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 //+ "<tr><td align='left' style='width:100%;'><p>Hello, " + Name + "</p></td></tr>"
                                 //+ "<tr><td align='left'><p>Your password has been reset successfully. New Password is : " + Password + "</p></td></tr><br>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello, </p></td></tr>"
                                 + "<tr><td align='left' style='width:100%;'></td></tr>"
                                 + "<tr><td align='left'><p>A new order comes. Please check out the backend Admin panel.</p></td></tr><br>"
                                 + "<tr><td align='left'><p>Shopname : <b>" + Message + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Email-Id : <b>" + EmailId + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Login : <b> http://lorefashions.com/Home/Index </b></p></td></tr>"
                                 + "<tr><td align='left'><p>&nbsp;</p></td></tr>"
                                 + "<tr><td align='right'><p>Thanks And Regards,</p></td></tr>"
                                 + "<tr><td align='right'><p>Lore-Group.</p></td></tr></table>";

                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));

                // set the network credentials
                mailClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);

                // enable ssl
                //mailClient.EnableSsl = true;

                // Create the Mail Message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["confirmationEmail"], "Loré Fashions");
                mailMessage.To.Add(ConfigurationManager.AppSettings["confirmationEmail"]);
                // mailMessage.To.Add("orders@lore-group.com");
                // mailMessage.CC.Add(usermailid);
                mailMessage.Subject = "Lore : New Order - " + Name;
                mailMessage.Body = mailFormat;
                mailMessage.IsBodyHtml = true;
                //mailMessage.Priority = MailPriority.High;

                //Se the mail
                mailClient.Send(mailMessage);
                return "Email Sent";
            }
            catch (Exception ex)
            {
                return "Fail - " + ex.Message;
            }
        }

        //For pagination


        //public ActionResult ProductList()
        //{
        //    var people = new PagedData<Product>();
        //    //using (var ctx = new AjaxPagingContext())
        //    //{
        //    people.Data = _context.Products.OrderBy(p => p.Id).Take(PageSize).ToList();
        //    people.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)_context.Products.Count() / PageSize));
        //    people.CurrentPage = 1;
        //    //}
        //    return View(people);
        //}

        //public ActionResult ProductListPaging(int page)
        //{
        //    var people = new PagedData<Product>();
        //    //using (var ctx = new AjaxPagingContext())
        //    //{
        //    people.Data = _context.Products.OrderBy(p => p.Id).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
        //    people.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)_context.Products.Count() / PageSize));
        //    people.CurrentPage = page;
        //    //}
        //    return PartialView(people);
        //}

        #region Frontend Login - Logout

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FrontendLoginViewModel model)
        {
            try
            {
                var con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("spLogin", con);
                cmd.CommandTimeout = timeout;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", model.Username);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                Int64 userId = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                if (userId == -1)
                {
                    TempData["LoginMsg"] = "Invalid Email-Id (Username)";
                    return View(model);
                }
                if (userId == -2)
                {
                    TempData["LoginMsg"] = "Invalid Password";
                    return View(model);
                }
                if (userId > 0)
                {
                    Int64 roleId = _context.MasterUsers.FirstOrDefault(x => x.Id == userId).RoleId;
                    if (roleId == 7)
                    {
                        TempData["LoginMsg"] = "Access Denied! Only Shop/Customers can access it.";
                        return View(model);
                    }

                    Session["FrontendUserId"] = userId;
                    _masterProvider.SaveUserLog("Account", "User Logins", userId);
                    return RedirectToAction("Dashboard", "Lore");
                  //  return RedirectToAction("Products", "Lore", new { CategoryId = 12 });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["LoginMsg"] = ex.Message.ToString();
                return View(model);
            }
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            Session.Abandon();

            try
            {
                Int64 userId = Convert.ToInt64(Session["FrontendUserId"]);
                _masterProvider.SaveUserLog("Account", "User Logout.", userId);
                Session["FrontendUserId"] = null;
            }
            catch (Exception ex)
            { }

            return RedirectToAction("Index", "Lore");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public int ResetPasswordSendEmail(string EmailId)
        {
            try
            {
                var record = _context.MasterUsers.FirstOrDefault(x => x.Email == EmailId);
                if (record != null)
                {
                    Random generator = new Random();
                    string s = generator.Next(100000, 999999).ToString("D6");
                    string newPass = record.FirstName.Replace(" ",string.Empty) + s;
                    String salt = Guid.NewGuid().ToString().Replace("-", "");
                    string password = _security.ComputeHash(newPass, salt);

                    record.Password = password;
                    record.Keyword = newPass;
                    record.Salt = salt;

                    _context.SaveChanges();

                    LoreGroupSendEmailResetPassword(record.FullName, record.Email, newPass, "");

                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }

        public string LoreGroupSendEmailResetPassword(string Name, string EmailId, string Password, string Message)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello, " + Name + "</p></td></tr>"
                                 + "<tr><td align='left'><p>Your password has been reset successfully. New Password is : " + Password + "</p></td></tr><br>"
                                 //+ "<tr><td align='left'><p>Name : <b>" + Name + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Email-Id : <b>" + EmailId + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Login : <b> http://lorefashions.com/Lore/Login </b></p></td></tr>"
                                 + "<tr><td align='left'><p>&nbsp;</p></td></tr>"
                                 + "<tr><td align='right'><p>Thanks And Regards,</p></td></tr>"
                                 + "<tr><td align='right'><p>Lore-Group.</p></td></tr>";

                string from = ConfigurationManager.AppSettings["confirmationEmail"];
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, "Lore Group");
                mail.To.Add(EmailId);
                mail.Subject = "Lore : Password Reset Successfully.";
                mail.Body = mailFormat;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));
                // set the network credentials
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = true;
                smtp.Send(mail);
                return "Email Sent";
            }
            catch (Exception ex)
            {
                return "Fail - " + ex.Message;
            }
        }

        #endregion        

        #region DashBoard: My Account

        public ActionResult DashBoard()
        {
            FrontendDashboardViewModel model = new FrontendDashboardViewModel();

            try
            {
                //Session["FrontendUserId"] = 48;
                var user = _userProvider.GetUser(Convert.ToInt32(Session["FrontendUserId"]));

                model.Fullname = user.FirstName + " " + user.LastName;
                model.EmailId = user.EmailId;
                model.BillingAddress = user.AddressLine1 + " " + user.AddressLine2 + " " + user.State + " " + user.City + " " + user.ZipCode;
                model.ShippingAddress = user.AddressLine1 + " " + user.AddressLine2 + " " + user.State + " " + user.City + " " + user.ZipCode;
                model.Shopname = user.ShopName;
                model.CompletedOrdersCount = _context.OrderMasters.Where(x => x.CustomerId == user.Id && (x.OrderStatusId == 4 || x.OrderStatusId == 6)).ToList().Count;
                model.InprogressOrdersCount = _context.OrderMasters.Where(x => x.CustomerId == user.Id && x.OrderStatusId == 2).ToList().Count;
                model.CancelledOrdersCount = _context.OrderMasters.Where(x => x.CustomerId == user.Id && x.OrderStatusId == 5).ToList().Count;
                model.UserId = user.Id;
                return View(model);
            }
            catch (Exception ex)
            {
                model.UserId = 0;
                return View(model);
            }
        }

        public ActionResult Addresses(Int64 id = 0)
        {
            AddressViewModel model = new AddressViewModel();
            id = Convert.ToInt32(Session["FrontendUserId"]);
            var record = _userProvider.GetUser(id);

            model.FirstName = record.FirstName;
            model.LastName = record.LastName;
            model.EmailId = record.EmailId;
            model.MobileNo = record.MobileNo;
            model.AddressLine1 = record.AddressLine1;
            model.AddressLine2 = record.AddressLine2;
            model.CountryId = record.CountryId;
            model.StateId = record.StateId;
            model.StateName = record.StateName;
            model.City = record.City;
            model.ZipCode = record.ZipCode;

            var shipdata = _loreProvider.GetUserShippingDetails(id);
            model.ShippingFirstName = shipdata.FirstName;
            model.ShippingLastName = shipdata.LastName;
            model.ShippingEmailId = shipdata.EmailId;
            model.ShippingMobileNo = shipdata.MobileNo;
            model.ShippingAddressLine1 = shipdata.AddressLine1;
            model.ShippingAddressLine2 = shipdata.AddressLine2;
            model.ShippingCountryId = shipdata.CountryId;
            model.ShippingStateId = shipdata.StateId;
            model.ShippingStateName = shipdata.StateName;
            model.ShippingCity = shipdata.City;
            model.ShippingZipCode = shipdata.ZipCode;

            model.CountryList = _masterProvider.getAllCountries();
            model.CountryList.Insert(0, new CountryViewModel
            {
                Id = 0,
                countryName = "--Select Country--"
            });


            if (record.CountryId == 2)
                model.StateList = _masterProvider.GetStates(Convert.ToInt64(model.CountryId));
            model.StateList.Insert(0, new StateViewModel
            {
                Id = 0,
                Statename = "--Select State--"
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult Addresses(AddressViewModel model)
        {
            try
            {
                var result = _loreProvider.SaveAddresses(model);
                return RedirectToAction("Addresses");
            }
            catch
            {
                return RedirectToAction("Addresses");
            }
        }

        [HttpPost]
        public ActionResult BillingAddresses(AddressViewModel model)
        {
            try
            {
                Int64 id = Convert.ToInt32(Session["FrontendUserId"]);
                model.Id = id;
                var result = _loreProvider.SaveBillingAddresses(model);
                return RedirectToAction("Addresses");
            }
            catch
            {
                return RedirectToAction("Addresses");
            }
        }

        public int changeShippingAddressFunction(string ShipFN, string ShipLN, string ShipEmail, string ShipAddress, string ShipMobile, string ShipAddress2, Int64 ShipCountryId, Int64 ShipStateId, string ShipStateName, string ShipCity, string ShipZipcode)
        {
            try
            {
                Int64 id = Convert.ToInt32(Session["FrontendUserId"]);
                var record = _context.ShippingDetails.FirstOrDefault(x => x.MasterUsersId == id);
                if (record != null)
                {
                    record.ShippingFirstName = ShipFN;
                    record.ShippingLastName = ShipLN;
                    record.ShippingEmailId = ShipEmail;
                    record.ShippingAddressLine1 = ShipAddress;
                    record.ShippingAddressLine2 = ShipAddress2;
                    record.ShippingCountryId = ShipCountryId;
                    record.ShippingStateId = ShipStateId;
                    record.ShippingStateName = ShipStateName;
                    record.ShippingCity = ShipCity;
                    record.ShippingZipCode = ShipZipcode;
                    record.ShippingMobileNo = ShipMobile;
                    _context.SaveChanges();

                    return 1;
                }
                else
                {
                    ShippingDetail ship = new ShippingDetail()
                    {
                        MasterUsersId = id,
                        ShippingFirstName = ShipFN,
                        ShippingLastName = ShipLN,
                        ShippingEmailId = ShipEmail,
                        ShopName = "",
                        ShippingMobileNo = ShipMobile,
                        ShippingAddressLine1 = ShipAddress,
                        ShippingAddressLine2 = ShipAddress2,
                        ShippingCompany = "",
                        ShippingZipCode = ShipZipcode,
                        ShippingWorkphone = "",
                        ShippingFax = "",
                        ShippingCountryId = ShipCountryId,
                        ShippingStateId = ShipStateId,
                        ShippingStateName = ShipStateName,
                        ShippingCity = ShipCity,
                        CreatedById = 1,
                        CreationDate = DateTime.UtcNow,
                        StatusId = 1
                        //StatusId = model.StatusId
                    };
                    _context.ShippingDetails.Add(ship);
                    _context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ActionResult AccountDetails(Int64 id = 0)
        {
            FrontendAccountdetailViewModel model = new FrontendAccountdetailViewModel();
            var user = _userProvider.GetUser(Convert.ToInt32(Session["FrontendUserId"]));

            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.EmailId = user.EmailId;
            return View(model);
        }

        [HttpPost]
        public ActionResult AccountDetails(FrontendAccountdetailViewModel model)
        {
            try
            {
                model.Id = Convert.ToInt32(Session["FrontendUserId"]);
                var result = _loreProvider.SaveAccountDetailsFirst(model);
                return RedirectToAction("AccountDetails");
            }
            catch
            {
                return RedirectToAction("AccountDetails");
            }
        }

        public int ChangePasswordFunction(string OldPassword, string NewPassword)
        {
            try
            {
                Int64 id = Convert.ToInt32(Session["FrontendUserId"]);
                var record = _context.MasterUsers.FirstOrDefault(x => x.Id == id);
                if (record != null)
                {
                    if (record.Keyword == OldPassword)
                    {
                        String salt = Guid.NewGuid().ToString().Replace("-", "");
                        string password = _security.ComputeHash(NewPassword, salt);

                        record.Password = password;
                        record.Keyword = NewPassword;
                        record.Salt = salt;
                        _context.SaveChanges();

                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 3;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ActionResult OrderHistory()
        {
            Int64 id = Convert.ToInt32(Session["FrontendUserId"]);
            OrderViewModel model = new OrderViewModel();
            model.OrderList = _orderProvider.GetAllOrderListByUserid(id);                  //new List<OrderViewModel>();          //_orderProvider.GetAllOrderList();
            return View(model);
        }

        public ActionResult DownloadImages()
        {
            Int64 id = Convert.ToInt32(Session["FrontendUserId"]);
            OrderViewModel model = new OrderViewModel();
            model.ProductImageList = _orderProvider.GetAllOrderImageListByUserid(id);                  //new List<OrderViewModel>();          //_orderProvider.GetAllOrderList();
           // model.ImagePath = "https://www.w3schools.com/w3images/wedding.jpg";
            return View(model);
        }


        public ActionResult DownloadFiles()
        {

            try
            {
                //Define file Type
                string fileType = "application/octet-stream";
                OrderViewModel model = new OrderViewModel();
                var id = _userProvider.GetUser(Convert.ToInt32(Session["FrontendUserId"]));
                Int64 Id = id.Id;
                model.ProductImageList = _orderProvider.GetAllOrderImageListByUserid(Id);
                //Define Output Memory Stream
                var outputStream = new MemoryStream();
                var res = new FileStreamResult(outputStream, fileType);
                //Create object of ZipFile library
                using (ZipFile zipFile = new ZipFile())
                {

                    //Add Root Directory Name "Files" or Any string name
                    zipFile.AddDirectoryByName("Files");

                    //Get all filepath from folder
                    foreach (var ab in model.ProductImageList)
                    {
                        if (ab.ImagePath != null)
                        {
                            string bb = ab.ImagePath2.TrimStart('/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/');

                            var lblgcode = Server.MapPath(@" ~/Content/" + bb);

                            //var bd = ab.ImagePath2.TrimStart('/','C','o','n','t','e','n','t');
                            //var abc = "/" + bd;
                            //String[] files = Directory.GetFiles(Server.MapPath(@"~/Content" + abc));
                            //foreach (var file in lblgcode)
                            //    {
                            //        var filePath = file;

                            //Adding files from filepath into Zip
                            zipFile.AddFile(lblgcode, "Files");
                        }
                        if (ab.ImagePath3 != null)
                        {
                            string bb = ab.ImagePath3.TrimStart('/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/');

                            var lblgcode = Server.MapPath(@" ~/Content/" + bb);

                            //var bd = ab.ImagePath2.TrimStart('/','C','o','n','t','e','n','t');
                            //var abc = "/" + bd;
                            //String[] files = Directory.GetFiles(Server.MapPath(@"~/Content" + abc));
                            //foreach (var file in lblgcode)
                            //    {
                            //        var filePath = file;

                            //Adding files from filepath into Zip
                            zipFile.AddFile(lblgcode, "Files");
                        }
                        if (ab.ImagePath4 != null)
                        {
                            string bb = ab.ImagePath4.TrimStart('/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/');

                            var lblgcode = Server.MapPath(@" ~/Content/" + bb);

                            //var bd = ab.ImagePath2.TrimStart('/','C','o','n','t','e','n','t');
                            //var abc = "/" + bd;
                            //String[] files = Directory.GetFiles(Server.MapPath(@"~/Content" + abc));
                            //foreach (var file in lblgcode)
                            //    {
                            //        var filePath = file;

                            //Adding files from filepath into Zip
                            zipFile.AddFile(lblgcode, "Files");
                        }

                    }
                    Response.ClearContent();
                    Response.ClearHeaders();

                    //Set zip file name
                    Response.AppendHeader("content-disposition", "attachment; filename=LoreZip.zip");

                    //Save the zip content in output stream
                    zipFile.Save(outputStream);
                }

                //Set the cursor to start position
                outputStream.Position = 0;

                //Dispance the stream
                return new FileStreamResult(outputStream, fileType);
            }

            catch (Exception Ex)
            {


                TempData["ResponseMsg12"] = "Some Images are not found";
                return RedirectToAction("DownloadImages");
            }

        }

        public ActionResult Actionby(Int64 ProductId)
        {

            try
            {
                string fileType = "application/octet-stream";
                OrderViewModel model = new OrderViewModel();
                //Define Output Memory Stream
                var outputStream = new MemoryStream();
                var res = new FileStreamResult(outputStream, fileType);
                //Create object of ZipFile library
                using (ZipFile zipFile = new ZipFile())
                {

                    //Add Root Directory Name "Files" or Any string name
                    zipFile.AddDirectoryByName("Files");

                    // Int64 ab = Convert.ToInt64(Name);
                    model.ProductImageList = _orderProvider.GetDownloadImagesbyProduct(ProductId);
                    foreach (var bb in model.ProductImageList)
                    {
                        if (bb.ImagePath2 != null)
                        {
                            string bbc = bb.ImagePath2.TrimStart('/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/');

                            var lblgcode = Server.MapPath(@" ~/Content/" + bbc);

                            zipFile.AddFile(lblgcode, "Files");
                        }
                        if (bb.ImagePath3 != null)
                        {
                            string bbc = bb.ImagePath3.TrimStart('/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/');

                            var lblgcode = Server.MapPath(@" ~/Content/" + bbc);


                            zipFile.AddFile(lblgcode, "Files");
                        }
                        if (bb.ImagePath4 != null)
                        {
                            string bbc = bb.ImagePath4.TrimStart('/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/');

                            var lblgcode = Server.MapPath(@" ~/Content/" + bbc);

                            zipFile.AddFile(lblgcode, "Files");
                        }

                    }
                    Response.ClearContent();
                    Response.ClearHeaders();
                    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    //Set zip file name
                    Response.AppendHeader("content-disposition", "attachment; filename=LoreZip.zip");

                    //Save the zip content in output stream
                    zipFile.Save(outputStream);
                }

                //Set the cursor to start position
                outputStream.Position = 0;

                //Dispance the stream
                return File(outputStream, fileType);
            }

            catch (Exception Ex)
            {


                TempData["ResponseMsg12"] = "Some Images are not found";
                return RedirectToAction("DownloadImages");
            }
        }
        //[HttpPost]
        //public  ActionResult PictureExport(OrderViewModel model)
        //{


        //    var id = _userProvider.GetUser(Convert.ToInt32(Session["FrontendUserId"]));
        //       Int64 Id = id.Id;
        //    model.OrderList = _orderProvider.GetAllOrderListByUserid(Id);
        //    //"db" is a DataContext and UserPicture is the model used for uploaded pictures.
        //    DateTime today = DateTime.Now;
        //    string fileName = "attachment;filename=AllUploadedPicturesAsOf:" + today.ToString() + ".zip";
        //    this.Response.Clear();
        //    this.Response.ContentType = "application/zip";
        //    this.Response.AddHeader("Content-Disposition", fileName);

        //    using (ZipFile zipFile = new ZipFile())
        //    {
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            foreach (ProductImage abc in model.ProductImageList)
        //            {
        //                stream.Seek(0, SeekOrigin.Begin);
        //                string pictureName = abc.ImagePath2 + ".png";
        //                using (MemoryStream tempstream = new MemoryStream())
        //                {
        //                     userImage = //method that returns Drawing.Image from byte[];   
        //                  userImage.Save(tempstream, ImageFormat.Png);
        //                    tempstream.Seek(0, SeekOrigin.Begin);
        //                    byte[] imageData = new byte[tempstream.Length];
        //                    tempstream.Read(imageData, 0, imageData.Length);
        //                    zipFile.AddEntry(pictureName, imageData);
        //                }
        //            }

        //            zipFile.Save(Response.OutputStream);

        //        zipFile.Save(Response.OutputStream);
        //        }

        //    }

        //    this.Response.End();
        //    return RedirectToAction("Home");
        //}
        //[HttpPost]
        //public ActionResult DownloadAllImages(string[] Name)
        //{
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //        zip.AddDirectoryByName("Files");
        //        foreach (string file in Name)
        //        {
        //            string bb = file.TrimStart('C', ':', '/', 'C', 'o', 'n', 't', 'e', 'n', 't', '/', 'P', 'r', 'o', 'd', 'u', 'c', 't', 'I', 'm', 'a', 'g', 'e', 's', '/');

        //            var lblgcode = Server.MapPath( @" ~/Content/ProductImages/" + bb);

        //            if (lblgcode != string.Empty)
        //            {
        //                zip.AddFile(lblgcode, "Files");
        //            }
        //        }

        //            Response.Clear();
        //    Response.BufferOutput = false;
        //    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //    Response.ContentType = "application/zip";
        //    Response.AddHeader("content-disposition", "attachment; filename=" + zipName); 
        //    zip.Save(Response.OutputStream);
        //    Response.End();
        //}
        //        return File(Response.OutputStream, "application/zip" , "filename");
        //    }




        //        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));

        //                MemoryStream memoryStream = new MemoryStream();
        //       zip.Save(memoryStream);
        //       return File(memoryStream.ToArray(), "application/zip", zipName);

        //   }
        //}

        //[HttpPost]
        //public ActionResult DownloadAllImages(string[] name)
        //{
        //    var files = Directory.GetFiles("");

        //    foreach (string ab in name)
        //    {


        //        var contentPath = Server.MapPath(ab);

        //        files = Directory.GetFiles(contentPath);
        //    }

        //    var zipFileMemoryStream = new MemoryStream();
        //    using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
        //    {
        //        foreach (string file in files)
        //        {
        //            var entry = archive.CreateEntry(Path.GetFileName(file));
        //            using (var entryStream = entry.Open())
        //            using (var fileStream = System.IO.File.OpenRead(file))
        //            {
        //                fileStream.CopyTo(entryStream);
        //            }
        //        }
        //    }

        //    zipFileMemoryStream.Seek(0, SeekOrigin.Begin);

        //    return File(zipFileMemoryStream, "application/octet-stream", "Bots.zip");

        //}


        //[HttpPost]
        //public ActionResult DownloadImages(OrderViewModel model)
        //{




        //    //Session["FrontendUserId"] = 48;


        //        var id = _userProvider.GetUser(Convert.ToInt32(Session["FrontendUserId"]));
        //    Int64 Id = id.Id;
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //        zip.AddDirectoryByName("Files");



        //        model.ProductImageList = _orderProvider.GetAllOrderImageListByUserid(Id);

        //        foreach (ProductImage abc in model.ProductImageList)
        //        {


        //            //_orderProvider.GetAllOrderList();



        //                zip.AddFile(abc.ImagePath2, "Files");

        //            }

        //        // model.ImagePath = "https://www.w3schools.com/w3images/wedding.jpg";
        //        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            zip.Save(memoryStream);
        //            return File(memoryStream.ToArray(), "application/zip", zipName);

        //        }
        //    }            
        //}
        //public FileResult DownloadZipFile()
        //{

        //    var fileName = string.Format("{0}_ImageFiles.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_1");
        //    var tempOutPutPath = Server.MapPath(Url.Content("/TempImages/")) + fileName;

        //    using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
        //    {
        //        s.SetLevel(9); // 0-9, 9 being the highest compression  

        //        byte[] buffer = new byte[4096];

        //        var ImageList = new List<string>();

        //        ImageList.Add(Server.MapPath("/Images/01.jpg"));
        //        ImageList.Add(Server.MapPath("/Images/02.jpg"));


        //        for (int i = 0; i < ImageList.Count; i++)
        //        {
        //            ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
        //            entry.DateTime = DateTime.Now;
        //            entry.IsUnicodeText = true;
        //            s.PutNextEntry(entry);

        //            using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
        //            {
        //                int sourceBytes;
        //                do
        //                {
        //                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
        //                    s.Write(buffer, 0, sourceBytes);
        //                } while (sourceBytes > 0);
        //            }
        //        }
        //        s.Finish();
        //        s.Flush();
        //        s.Close();

        //    }

        //    byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
        //    if (System.IO.File.Exists(tempOutPutPath))
        //        System.IO.File.Delete(tempOutPutPath);

        //    if (finalResult == null || !finalResult.Any())
        //        throw new Exception(String.Format("No Files found with Image"));

        //    return File(finalResult, "application/zip", fileName);

        //}
        //public ActionResult CheckOut()
        //{
        //    return View();
        //}

        #endregion

        #region : Stock Inventory Report

        public ActionResult StockReport()
        {            
            PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel() { Id = 0, WareHouseName = "--Select Ware House--" });
            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
            //Model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
            //Model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });
            return View(Model);
        }

        [HttpPost]
        public ActionResult StockReport(PurchaseOrderViewModel Model)
        {
            List<SizeViewModel> finalSizeModel = new List<SizeViewModel>();
            
            string categoryId = Model.CategoryId.ToString();
            //var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.CollectionYearId == Model.CollectionYearId).OrderBy(x=>x.ProductName).Skip(skipValue).Take(takeValue).ToList();
            //var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.CollectionYearId == Model.CollectionYearId).OrderBy(x => x.ProductName).ToList();
            var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId)).OrderBy(x => x.ProductName).ToList();
            Int64 total = 0;
            foreach (var product in productsList)
            {
                List<long> colorIds = product.SeletedColorIds.TrimEnd(',').Split(',').Select(long.Parse).ToList();

                foreach (long? colorid in colorIds)
                {
                    total = 0;
                    if (colorid.HasValue)
                    {
                        if (colorid.Value > 0)
                        {
                            List<SizeViewModel> sizemodel = _orderProvider.GetStockReportForInventory(product.Id, product.ProductName, colorid.Value, Model.WareHouseId);
                            if (sizemodel.Count > 0)
                            {
                                total = sizemodel.Sum(x => x.InStockQty - x.Qty + x.POQty);
                                if (total > 0)
                                    finalSizeModel.AddRange(sizemodel);
                            }
                            //finalSizeModel.AddRange(sizemodel);
                        }
                    }
                }
            }

            Model.ProductListModel.StockSizeModel = LinqHelper.ChunkBy(finalSizeModel, 17);

            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel() { Id = 0, WareHouseName = "--Select Ware House--" });
            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
            //Model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
            //Model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });

            return View(Model);
        }

        #endregion

        public void sendhtmlmail()
        {
            try
            {
                string from = "orders@lorefashions.com"; //any valid GMail ID
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, "Loré Fashions");

                //mail.To.Add(Model.CustomerModel.EmailId);
                mail.To.Add("chandrashekhar.bairagi@connekt.in");
                mail.Subject = "Test";
                mail.Body = "<h2>Hello</h2>";
                mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "mail.lorefashions.com";        //"cmx5.my-hosting-panel.com";
                //smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = "orders@lorefashions.com";
                networkCredential.Password = "ATlanta99!!";
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 2525;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
            }

        }

        public void GetLatLong(string address)
        {
            try
            {
                string latitude = "";
                string longitude = "";

                var users = _context.MasterUsers.Where(x => x.Id > 40093).ToList();
                foreach (var item in users)
                {
                    try
                    {
                        string countryName = _context.Countries.FirstOrDefault(x => x.Id == item.CountryId).CountryName;

                        string AddressData = "";
                        if (!String.IsNullOrEmpty(item.AddressLine1))
                            AddressData += item.AddressLine1;
                        if (!String.IsNullOrEmpty(item.AddressLine2))
                            AddressData += " , " + item.AddressLine2;
                        if (!String.IsNullOrEmpty(item.City))
                            AddressData += " , " + item.City;
                        if (!String.IsNullOrEmpty(item.StateName))
                            AddressData += " , " + item.StateName;
                        if (!String.IsNullOrEmpty(countryName))
                            AddressData += " , " + countryName;

                        string url = "https://maps.google.com/maps/api/geocode/xml?address=" + AddressData + "&sensor=false&key=AIzaSyDnLyynMxlsJHqXcRFXxn1D_7-vj4jnQmw";
                        WebRequest request = WebRequest.Create(url);

                        using (WebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                DataSet dsResult = new DataSet();
                                dsResult.ReadXml(reader);
                                //    DataTable dtCoordinates = new DataTable();
                                //    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                                //new DataColumn("Address", typeof(string)),
                                //new DataColumn("Latitude",typeof(string)),
                                //new DataColumn("Longitude",typeof(string)) });
                                foreach (DataRow row in dsResult.Tables["result"].Rows)
                                {
                                    string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                    DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                    latitude = location[0].ToString();
                                    longitude = location[1].ToString();
                                    //dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                                }
                                //var Data = dtCoordinates;
                            }
                        }

                        item.Latitude = latitude;
                        item.Longitude = longitude;
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


    }

}