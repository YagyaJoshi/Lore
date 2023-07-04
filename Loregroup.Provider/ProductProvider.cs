using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System;
using Loregroup.Data;
using System.Collections.Generic;
using Loregroup.Core.Helpers;
using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Interfaces;
using Loregroup.Core;
using Loregroup.Core.Interfaces.Providers;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace Loregroup.Provider
{
    public class ProductProvider : IProductProvider
    {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly ISession _session;
        private readonly IUtilities _utilities;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        int timeout = 3600;
        public ProductProvider(AppContext context, ISecurity security, IUtilities utilities, ISession session)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _session = session;
        }

        #region: Product

        public string SaveProduct(ProductViewModel model)
        {
            string res = "";
            try
            {
                var chkname = _context.Products.FirstOrDefault(x => x.ProductName.ToLower() == model.ProductName.ToLower() && x.Id != model.Id && x.StatusId != 4);

                if (chkname != null)
                {
                    res = "Style Number already exist";
                    return res;
                };
                var chkproduct = _context.Products.FirstOrDefault(x => x.Id == model.Id);

                // var pro = _context.Products.Where(x => x.Id == model.Id).FirstOrDefault();

                if (chkproduct == null)
                {
                    Product product = new Product()
                    {
                        ProductName = model.ProductName,
                        Title = model.Title,
                        Description = model.Description,
                        BrandId = model.BrandId,
                        ColourId = model.SampleColourId,
                        CutOfDressId = model.CutOfDressId,
                        CollectionYearId = model.CollectionYearId,
                        Style = model.Style,
                        FabricId = model.FabricId,
                        CategoryId = model.CategoryId,
                        PriceUSD = model.PriceUSD,
                        PriceEURO = model.PriceEURO,
                        PriceGBP = model.PriceGBP,
                        Picture1 = model.Picture1,
                        Picture2 = model.Picture2,
                        Picture3 = model.Picture3,
                        Picture4=model.Picture4,
                        SeletedCategoryIds = model.SeletedCategoryIds,
                        SeletedCategoryNames = model.SeletedCategoryNames,
                        SeletedBrandIds = model.SeletedBrandIds,
                        SeletedBrandNames = model.SeletedBrandNames,
                        SeletedColorIds = model.SeletedColorIds,
                        SeletedColorNames = model.SeletedColorNames,
                        SelectedFabricIds = model.SelectedFabricIds,
                        SelectedFabricNames = model.SelectedFabricNames,
                        Publish = false,
                        PublishId = model.PublishId,
                        VideoImage = model.VideoImage,
                        VideoLink= model.VideoLink,
                        PurchasePrice=model.PurchasePrice
                        //StatusId = model.StatusId
                    };
                    _context.Products.Add(product);
                }
                else
                {
                    chkproduct.ProductName = model.ProductName;
                    chkproduct.Title = model.Title;
                    chkproduct.Description = model.Description;
                    chkproduct.BrandId = model.BrandId;
                    chkproduct.ColourId = model.SampleColourId;
                    chkproduct.CutOfDressId = model.CutOfDressId;
                    chkproduct.CollectionYearId = model.CollectionYearId;
                    chkproduct.Style = model.Style;
                    chkproduct.FabricId = model.FabricId;
                    chkproduct.CategoryId = model.CategoryId;
                    chkproduct.PriceUSD = model.PriceUSD;
                    chkproduct.PriceEURO = model.PriceEURO;
                    chkproduct.PriceGBP = model.PriceGBP;
                    chkproduct.Picture1 = model.Picture1;
                    chkproduct.Picture2 = model.Picture2;
                    chkproduct.Picture3 = model.Picture3;
                    chkproduct.Picture4 = model.Picture4;
                    chkproduct.SeletedCategoryIds = model.SeletedCategoryIds;
                    chkproduct.SeletedCategoryNames = model.SeletedCategoryNames;
                    chkproduct.SeletedBrandIds = model.SeletedBrandIds;
                    chkproduct.SeletedBrandNames = model.SeletedBrandNames;
                    chkproduct.SeletedColorIds = model.SeletedColorIds;
                    chkproduct.SeletedColorNames = model.SeletedColorNames;
                    chkproduct.SelectedFabricIds = model.SelectedFabricIds;
                    chkproduct.SelectedFabricNames = model.SelectedFabricNames;
                    chkproduct.PublishId = model.PublishId;
                    chkproduct.Publish = false;
                    chkproduct.VideoImage = model.VideoImage;
                    chkproduct.VideoLink = model.VideoLink;
                    chkproduct.ModifiedById = _session.CurrentUser.Id;
                    chkproduct.ModificationDate = DateTime.UtcNow;
                    chkproduct.PurchasePrice = model.PurchasePrice;
                }
                _context.SaveChanges();
                res = "Success";
                return res;
            }
            catch (Exception ex)
            {
                res = "Some error occurred";
                return res;
            }            
        }

        public List<ProductViewModel> GetAllProducts(int start, int length, string search, int filtercount)
        {
            List<ProductViewModel> newData = new List<ProductViewModel>();
            try
            {
                int skip = start * length;
                List<ProductViewModel> result = new List<ProductViewModel>();
                if (search == String.Empty)
                {
                    result = _context.Products.Where(x => x.StatusId != 4).OrderByDescending(x => x.Id)
                       .ToList().Skip(skip).Take(length).Select(ToProductViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Products.Where(p => p.ProductName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList().Skip(skip).Take(length)
                        .Select(ToProductViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
            string BrandName = _context.Brands.Where(x => x.Id == product.BrandId).Where(x => x.StatusId == 1).Select(x => x.BrandName).FirstOrDefault();
            string CollectionYear = _context.CollectionYears.Where(x => x.Id == product.CollectionYearId).Where(x => x.StatusId == 1).Select(x => x.Year).FirstOrDefault();
            string CategoryName = _context.Categories.Where(x => x.Id == product.CategoryId).Where(x => x.StatusId == 1).Select(x => x.CategoryName).FirstOrDefault();
            string FabricName = _context.Fabrics.Where(x => x.Id == product.FabricId).Where(x => x.StatusId == 1).Select(x => x.FabricName).FirstOrDefault();
            string CutOfDressName = _context.CutOfDresses.Where(x => x.Id == product.CutOfDressId && x.StatusId == 1).Select(x => x.CutOfDressName).FirstOrDefault();
            string smplecol = null;
            if (product.ColourId > 0)
            {
                smplecol = _context.Colours.Where(x => x.Id == product.ColourId && x.StatusId != 4).Select(x => x.ColourName).FirstOrDefault();
            }
            string publishValue = "";
            if (product.PublishId == 0)
                publishValue = "Not Published";
            else if (product.PublishId == 1)
                publishValue = "To Everyone";
            else if (product.PublishId == 2)
                publishValue = "To Customers";

            Int64 catid = 1;
            try
            {
                catid = Convert.ToInt64(product.SeletedCategoryIds.Split(',')[0]);
            }
            catch (Exception ex)
            { }

            return new ProductViewModel()
            {
                //UserId = User.Id,
                Id = product.Id,
                ProductName = product.ProductName,
                Title = product.Title,
                Description = product.Description,
                BrandId = product.BrandId,
                BrandName = BrandName,
                ColourId = product.ColourId,
                SampleColourId = product.ColourId,
                CutOfDressId = product.CutOfDressId,
                CutOfDress = CutOfDressName,
                CollectionYearId = product.CollectionYearId,
                CollectionYear = CollectionYear,
                Style = product.Style,
                FabricId = product.FabricId,
                FabricName = FabricName,
                PriceUSD = product.PriceUSD,
                PriceEURO = product.PriceEURO,
                PriceGBP = product.PriceGBP,
                Picture1 = product.Picture1,
                Picture2 = product.Picture2,
                Picture3 = product.Picture3,
                Picture4 = product.Picture4,
                SeletedCategoryIds = product.SeletedCategoryIds,
                SeletedCategoryNames = product.SeletedCategoryNames,
                SeletedBrandIds = product.SeletedBrandIds,
                SeletedBrandNames = product.SeletedBrandNames,
                SeletedColorIds = product.SeletedColorIds,
                SeletedColorNames = product.SeletedColorNames,
                SelectedFabricIds = product.SelectedFabricIds,
                SelectedFabricNames = product.SelectedFabricNames,
                Publish = product.Publish,
                PublishString = publishValue,
                PublishId = product.PublishId.Value,
                VideoImage = product.VideoImage,
                VideoLink = product.VideoLink,
                CategoryId = catid,
                CategoryName = CategoryName,
                SampleColourName = smplecol,
                CreatedById = Convert.ToInt64(product.CreatedById),
                CreationDate = Convert.ToDateTime(product.CreationDate),
                ModifiedById = Convert.ToInt64(product.ModifiedById),
                ModificationDate = Convert.ToDateTime(product.ModificationDate),
                StatusId = Convert.ToInt16(product.StatusId),
                PurchasePrice = product.PurchasePrice,
                Edit = "<a href='/Product/AddNewProduct?id=" + product.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteProduct?id=" + product.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>"
            };
        }


        public ProductViewModel GetProduct(Int64 id)
        {
            // ProductViewModel editproduct = new ProductViewModel();
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product != null)
            {
                return ToProductViewModel(product);
            }
            else
            {
                return null;
            }
        }

        public void DeleteProduct(Int64 id)
        {
            Product product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            product.StatusId = 4;
            product.ModificationDate = DateTime.UtcNow;
            product.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        public List<ProductViewModel> GetAllProductsForReceivePO(Int64 POMasterId)
        {
            List<ProductViewModel> ProductList = new List<ProductViewModel>();
            try
            {
                List<Int64> IdList = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == POMasterId && x.StatusId != 4).Select(x => x.ProductId).Distinct().ToList();

                foreach (var id in IdList)
                {
                    ProductViewModel Product = _context.Products.Where(x => x.Id == id && x.StatusId != 4).Select(ToProductViewModel).FirstOrDefault();
                    ProductList.Add(Product);
                }
                return ProductList;
            }
            catch
            {
                return ProductList;
            }
        }

        public List<ProductViewModel> GetAllProductsForDispatchOrder(Int64 OrderMasterId)
        {
            List<ProductViewModel> ProductList = new List<ProductViewModel>();
            try
            {
                List<Int64> IdList = _context.OrderDetails.Where(x => x.OrderMasterId == OrderMasterId && x.StatusId != 4).Select(x => x.ProductId).Distinct().ToList();

                foreach (var id in IdList)
                {
                    ProductViewModel Product = _context.Products.Where(x => x.Id == id && x.StatusId != 4).Select(ToProductViewModel).FirstOrDefault();
                    ProductList.Add(Product);
                }
                return ProductList;
            }
            catch
            {
                return ProductList;
            }
        }

        public List<ProductViewModel> GetProductsOfPO(Int64 POId)
        {
            List<ProductViewModel> Productlist = new List<ProductViewModel>();

            try
            {
                var ProductIds = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == POId && x.StatusId != 4).Select(y => y.ProductId).Distinct().ToList();
                Int64[] ProductIdList = ProductIds.ToArray();
                Productlist = _context.Products.Where(x => ProductIdList.Contains(x.Id)).ToList().Select(ToProductViewModel).ToList();
                return Productlist;
            }
            catch
            {
                return null;
            }
        }

        #endregion
               
        #region Events

        public Int64 SaveEvents(EventViewModel model)
        {
            Int64 i = 0;

            try
            {
                var chkEvents = _context.Events.FirstOrDefault(x => x.Id == model.Id);

                if (chkEvents == null)
                {
                    Events events = new Events()
                    {

                        CustomerId = model.CustomerModel.Id,
                        Title = model.Title,
                        FromDate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Address = model.Address,
                        State = model.State,
                        city = model.city,
                        Zipcode = model.Zipcode,
                        mobileno = model.mobileno,
                        EventTypeId = (int)model.EventTypeId,
                        //EventTypeId = 1,
                        StatusId = 1,
                        image = model.image,
                        BoothNumber = model.BoothNumber,
                        WebsiteUrl = model.WebsiteUrl,
                        AddressOthr=model.Address1,

                    };
                    _context.Events.Add(events);
                }
                else
                {
                    if(model.image==null)
                    {
                        model.image = chkEvents.image;
                    }
                    chkEvents.Title = model.Title;
                    chkEvents.FromDate = Convert.ToDateTime(DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    chkEvents.EndDate = Convert.ToDateTime(DateTime.ParseExact(model.EndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    chkEvents.Address = model.Address;
                    chkEvents.State = model.State;
                    chkEvents.city = model.city;
                    chkEvents.Zipcode = model.Zipcode;
                    chkEvents.mobileno = model.mobileno;
                    chkEvents.EventTypeId = (int)model.EventTypeId;
                    chkEvents.ModificationDate = DateTime.UtcNow;
                    chkEvents.ModifiedById = _session.CurrentUser.Id;
                    chkEvents.image = model.image;
                    chkEvents.CustomerId = model.CustomerModel.Id;
                    chkEvents.BoothNumber = model.BoothNumber;
                    chkEvents.WebsiteUrl = model.WebsiteUrl;
                    chkEvents.AddressOthr = model.Address1;
                }
                _context.SaveChanges();

                return i;

            }
            catch (Exception ex)
            {
                return i;
            }
        }
        
        public EventViewModel ToEventViewModel(Events Event)
        {
            try
            {
                var color = "";
                if ((int)Event.EventTypeId == 1)
                {
                    color = "#3c8dbc"; //status new, color blue
                }
                else if ((int)Event.EventTypeId == 2)
                {
                    color = "#9709c9"; //status In Progress, color Teel
                }

                string fdate = Event.FromDate.ToString("dd-MM-yyyy");
                string edate = Event.EndDate.ToString("dd-MM-yyyy");

                return new EventViewModel()
                {
                    BoothNumber = Event.BoothNumber,
                    WebsiteUrl = Event.WebsiteUrl,
                    CustomerId = Event.CustomerId,
                    Id = Event.Id,
                    Title = Event.Title,
                    Address = Event.Address,
                    State = Event.State,
                    city = Event.city,
                    Zipcode = Event.Zipcode,
                    mobileno = Event.mobileno,
                    image = Event.image,
                    EventTypeId = (EventType)Event.EventTypeId,
                    FromDate = fdate,   //Event.FromDate.ToShortDateString(),
                    EndDate =  edate,   //Event.EndDate.ToShortDateString(),
                    CreatedById = Convert.ToInt64(Event.CreatedById),
                    CreationDate = Convert.ToDateTime(Event.CreationDate),
                    ModifiedById = Convert.ToInt64(Event.ModifiedById),
                    ModificationDate = Convert.ToDateTime(Event.ModificationDate),
                    Address1 = Event.AddressOthr,
                    //StatusId = Convert.ToInt16(Event.StatusId),                    
                    EventType = "<span style='color:white;background: " + color + "; font-weight:bold;border-radius: 4px;padding: 3px;'>" + (EventType)Event.EventTypeId + "</span>",
                    Edit = "<a href='/Product/AddNewEvents?id=" + Event.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/Product/DeleteEvents?id=" + Event.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new EventViewModel();
            }

        }

        public List<EventViewModel> GetAllEvents(int start, int length, string search, int filtercount)
        {
            List<EventViewModel> newData = new List<EventViewModel>();

            try
            {
                List<EventViewModel> result = new List<EventViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Events.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToEventViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Events.Where(p => p.Title.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToEventViewModel).ToList();
                    
                    filtercount = result.Count;
                    newData = result;
                }

                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }
        
        public List<EventViewModel> GetAllEventsList()
        {
            var result = _context.Events.Where(x => x.StatusId != 4)
                .Select(ToEventViewModel).ToList();
            return result;
        }

        public List<EventViewModel> GetAllEventsListByEventtype(string eventtype)
        {
            if (eventtype == "TradeShows")
            {
                var result = _context.Events.Where(x => x.StatusId != 4 && x.EventTypeId == (int)Core.Enumerations.EventType.TradeShows && x.EndDate >= DateTime.Now)
                    .OrderBy(y => y.FromDate)
                .Select(ToEventViewModel).ToList();
                return result;
            }
            else
            {
                var result = _context.Events.Where(x => x.StatusId != 4 && x.EventTypeId == (int)Core.Enumerations.EventType.TrunkShows && x.EndDate >= DateTime.Now)
                .OrderBy(y => y.FromDate)
                    .Select(ToEventViewModel).ToList();
                return result;
            }            
        }

        public EventViewModel GetEvents(Int64 id)
        {
            EventViewModel editEvent = new EventViewModel();
            Events Event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (Event != null)
            {
                return ToEventViewModel(Event);
            }
            else
            {
                return null;
            }
        }

        public void DeleteEvents(Int64 id)
        {
            Events Event = _context.Events.Where(x => x.Id == id).FirstOrDefault();
            Event.StatusId = 4;
            Event.ModificationDate = DateTime.UtcNow;
            Event.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region Contact
        public Int64 SaveContact(ContactViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkContact = _context.Contacts.FirstOrDefault(x => x.Id == model.Id);
                if (chkContact == null)
                {
                    Contact contact = new Contact()
                    {
                        OfficeName = model.OfficeName,
                        Address = model.Address,
                        Contactno = model.Contactno,
                        Email = model.Email
                    };
                    _context.Contacts.Add(contact);
                }
                else
                {
                    chkContact.OfficeName = model.OfficeName;
                    chkContact.Address = model.Address;
                    chkContact.Email = model.Email;
                    chkContact.Contactno = model.Contactno;
                    chkContact.ModificationDate = DateTime.UtcNow;
                    chkContact.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public List<ContactViewModel> GetAllContact(int start, int length, string search, int filtercount)
        {
            List<ContactViewModel> newData = new List<ContactViewModel>();

            try
            {
                List<ContactViewModel> result = new List<ContactViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Contacts.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4).ToList().Select(ToContactViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Contacts.Where(p => p.OfficeName.Contains(value)).OrderByDescending(x => x.Id).ToList().Select(ToContactViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public ContactViewModel ToContactViewModel(Contact contact)
        {
            try
            {
                return new ContactViewModel()
                {
                    OfficeName = contact.OfficeName,
                    Id = contact.Id,
                    Address = contact.Address,
                    Contactno = contact.Contactno,
                    Email = contact.Email,
                    CreatedById = Convert.ToInt64(contact.CreatedById),
                    CreationDate = Convert.ToDateTime(contact.CreationDate),
                    ModifiedById = Convert.ToInt64(contact.ModifiedById),
                    ModificationDate = Convert.ToDateTime(contact.ModificationDate),
                    StatusId = Convert.ToInt16(contact.StatusId),
                    Edit = "<a href='/Product/AddNewContact?id=" + contact.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/Product/DeleteContact?id=" + contact.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new ContactViewModel();
            }

        }

        public List<ContactViewModel> GetAllContactList()
        {

            var result = _context.Contacts.Where(x => x.StatusId != 4).Select(ToContactViewModel).ToList();
            return result;

        }

        public ContactViewModel GetContact(Int64 id)
        {
            ContactViewModel editEvent = new ContactViewModel();
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Id == id);

            if (contact != null)
            {
                return ToContactViewModel(contact);
            }
            else
            {
                return null;
            }
        }

        public void DeleteContact(Int64 id)
        {
            Contact contact = _context.Contacts.Where(x => x.Id == id).FirstOrDefault();
            contact.StatusId = 4;
            contact.ModificationDate = DateTime.UtcNow;
            contact.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }
        #endregion

        #region Faq
        public Int64 SaveFaq(FaqViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkFaq = _context.Faqs.FirstOrDefault(x => x.Id == model.Id);
                if (chkFaq == null)
                {
                    Faq faq = new Faq()
                    {
                        Question = model.Question,
                        Answer = model.Answer,

                    };
                    _context.Faqs.Add(faq);
                }
                else
                {
                    chkFaq.Question = model.Question;
                    chkFaq.Answer = model.Answer;
                    chkFaq.ModificationDate = DateTime.UtcNow;
                    chkFaq.ModifiedById = _session.CurrentUser.Id;

                }
                _context.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public List<FaqViewModel> GetAllFaq(int start, int length, string search, int filtercount)
        {
            List<FaqViewModel> newData = new List<FaqViewModel>();

            try
            {
                List<FaqViewModel> result = new List<FaqViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Faqs.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4).ToList().Select(ToFaqViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Faqs.Where(p => p.Question.Contains(value)).OrderByDescending(x => x.Id).ToList().Select(ToFaqViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }
                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }
        
        public FaqViewModel ToFaqViewModel(Faq faq)
        {
            try
            {
                return new FaqViewModel()
                {
                    Id = faq.Id,
                    Question = faq.Question,
                    Answer = faq.Answer,
                    CreatedById = Convert.ToInt64(faq.CreatedById),
                    CreationDate = Convert.ToDateTime(faq.CreationDate),
                    ModifiedById = Convert.ToInt64(faq.ModifiedById),
                    ModificationDate = Convert.ToDateTime(faq.ModificationDate),
                    StatusId = Convert.ToInt16(faq.StatusId),
                    Edit = "<a href='/Product/AddNewFaq?id=" + faq.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/Product/DeleteFaq?id=" + faq.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new FaqViewModel();
            }

        }

        public List<FaqViewModel> GetAllFaqList()
        {

            var result = _context.Faqs.Where(x => x.StatusId != 4).Select(ToFaqViewModel).ToList();
            return result;

        }

        public FaqViewModel GetFaq(Int64 id)
        {
            FaqViewModel editFaq = new FaqViewModel();
            Faq faq = _context.Faqs.FirstOrDefault(x => x.Id == id);

            if (faq != null)
            {
                return ToFaqViewModel(faq);
            }
            else
            {
                return null;
            }
        }

        public void DeleteFaq(Int64 id)
        {
            Faq faq = _context.Faqs.Where(x => x.Id == id).FirstOrDefault();
            faq.StatusId = 4;
            faq.ModificationDate = DateTime.UtcNow;
            faq.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();

        }

        #endregion
        public Int64 SaveAgents(AgentViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkAgents = _context.Agents.FirstOrDefault(x => x.Id == model.Id);


                if (chkAgents == null)
                {
                    Agent agent = new Agent()
                    {
                        territory = model.territory,
                        Description = model.Description,
                        AgentsId= Convert.ToInt64(model.CustomerId)
                        //StatusId = model.StatusId
                    };
                    _context.Agents.Add(agent);
                }
                else
                {
                    chkAgents.territory = model.territory;
                    chkAgents.Description = model.Description;
                    chkAgents.AgentsId = Convert.ToInt64(model.CustomerId);
                    chkAgents.ModificationDate = DateTime.UtcNow;
                    chkAgents.ModifiedById = _session.CurrentUser.Id;
                }

                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }
       public List<AgentViewModel> GetAllAgents(int start, int length, string search, int filtercount)
        {
            List<AgentViewModel> newData = new List<AgentViewModel>();

            try
            {
                List<AgentViewModel> result = new List<AgentViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Agents.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToAgentsViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Agents.Where(p => p.territory.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToAgentsViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }
        public AgentViewModel ToAgentsViewModel(Agent agent)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            AgentViewModel ab = new AgentViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }



            var Agenty = _context.MasterUsers.Where(x => x.Id == agent.AgentsId).Where(x => x.StatusId == 1).Select(y => new AgentViewModel { AgentsName = y.FirstName + " " + y.LastName }).FirstOrDefault();
            if (Agenty == null)
            {
                Agenty.AgentsName = "Null";
            }
            return new AgentViewModel()
            {
                //UserId = User.Id,
                
                Id = agent.Id,
                territory = agent.territory,
                Description = agent.Description,
                CustomerId = agent.AgentsId,
                AgentsName = Agenty.AgentsName,
                CreatedById = Convert.ToInt64(agent.CreatedById),
                CreationDate = Convert.ToDateTime(agent.CreationDate),
                ModifiedById = Convert.ToInt64(agent.ModifiedById),
                ModificationDate = Convert.ToDateTime(agent.ModificationDate),
                StatusId = Convert.ToInt16(agent.StatusId),
                Edit = "<a href='/Product/AddNewAgents?id=" + agent.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteAgent?id=" + agent.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive = ab.IsAdmin,
            };
        }
        public AgentViewModel GetAgents(Int64 id)
        {
            AgentViewModel editbrand = new AgentViewModel();
            Agent agent = _context.Agents.FirstOrDefault(x => x.Id == id);

            if (agent != null)
            {
                return ToAgentsViewModel(agent);
            }
            else
            {
                return null;
            }
        }
        public void DeleteAgents(Int64 id)
        {
            Agent agent = _context.Agents.Where(x => x.Id == id).FirstOrDefault();
            agent.StatusId = 4;
            agent.ModificationDate = DateTime.UtcNow;
            agent.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }
        public List<AgentViewModel> GetAllAgentList()
        {

            var result = _context.Agents.Where(x => x.StatusId != 4)
                .Select(ToAgentsViewModel).ToList();
            return result;

        }
        #region: Brand
        public Int64 SaveBrand(BrandViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chkbrand = _context.Brands.FirstOrDefault(x => x.Id == model.Id);


                if (chkbrand == null)
                {
                    Brand brand = new Brand()
                    {
                        BrandName = model.BrandName,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.Brands.Add(brand);
                }
                else
                {
                    chkbrand.BrandName = model.BrandName;
                    chkbrand.Description = model.Description;
                    chkbrand.ModificationDate = DateTime.UtcNow;
                    chkbrand.ModifiedById = _session.CurrentUser.Id;
                }

                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }
       
        public List<BrandViewModel> GetAllBrands(int start, int length, string search, int filtercount)
        {
            List<BrandViewModel> newData = new List<BrandViewModel>();

            try
            {
                List<BrandViewModel> result = new List<BrandViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Brands.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToBrandViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Brands.Where(p => p.BrandName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToBrandViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public BrandViewModel ToBrandViewModel(Brand brand)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            BrandViewModel ab = new BrandViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            return new BrandViewModel()
            {
                //UserId = User.Id,
                Id = brand.Id,
                BrandName = brand.BrandName,
                Description = brand.Description,
                CreatedById = Convert.ToInt64(brand.CreatedById),
                CreationDate = Convert.ToDateTime(brand.CreationDate),
                ModifiedById = Convert.ToInt64(brand.ModifiedById),
                ModificationDate = Convert.ToDateTime(brand.ModificationDate),
                StatusId = Convert.ToInt16(brand.StatusId),
                Edit = "<a href='/Product/AddNewBrand?id=" + brand.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteBrand?id=" + brand.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<BrandViewModel> GetAllBrandList()
        {

            var result = _context.Brands.Where(x => x.StatusId != 4)
                .Select(ToBrandViewModel).ToList();
            return result;

        }

        public BrandViewModel GetBrand(Int64 id)
        {
            BrandViewModel editbrand = new BrandViewModel();
            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);

            if (brand != null)
            {
                return ToBrandViewModel(brand);
            }
            else
            {
                return null;
            }
        }

        public void DeleteBrand(Int64 id)
        {
            Brand brand = _context.Brands.Where(x => x.Id == id).FirstOrDefault();
            brand.StatusId = 4;
            brand.ModificationDate = DateTime.UtcNow;
            brand.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }
        #endregion

        #region: Order Locator
        public Int64 SaveOrderLocator(OrderLocatorViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chkbrand = _context.OrderLocators.FirstOrDefault(x => x.Id == model.Id);


                if (chkbrand == null)
                {
                    OrderLocator brand = new OrderLocator()
                    {
                        OrderLocatorName = model.OrderLocatorName,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.OrderLocators.Add(brand);
                }
                else
                {
                    chkbrand.OrderLocatorName = model.OrderLocatorName;
                    chkbrand.Description = model.Description;
                    chkbrand.ModificationDate = DateTime.UtcNow;
                    chkbrand.ModifiedById = _session.CurrentUser.Id;
                }

                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<OrderLocatorViewModel> GetAllOrderLocator(int start, int length, string search, int filtercount)
        {
            List<OrderLocatorViewModel> newData = new List<OrderLocatorViewModel>();

            try
            {
                List<OrderLocatorViewModel> result = new List<OrderLocatorViewModel>();

                if (search == String.Empty)
                {
                    result = _context.OrderLocators.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToOrderLocatorViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.OrderLocators.Where(p => p.OrderLocatorName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToOrderLocatorViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public OrderLocatorViewModel ToOrderLocatorViewModel(OrderLocator Orederloct)
        {
            OrderLocatorViewModel ab = new OrderLocatorViewModel();
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            return new OrderLocatorViewModel()
            {
                //UserId = User.Id,
                Id = Orederloct.Id,
                OrderLocatorName = Orederloct.OrderLocatorName,
                OrderLocatorNameDesc = Orederloct.OrderLocatorName + "-" + Orederloct.Description,
                Description = Orederloct.Description,
                CreatedById = Convert.ToInt64(Orederloct.CreatedById),
                CreationDate = Convert.ToDateTime(Orederloct.CreationDate),
                ModifiedById = Convert.ToInt64(Orederloct.ModifiedById),
                ModificationDate = Convert.ToDateTime(Orederloct.ModificationDate),
                StatusId = Convert.ToInt16(Orederloct.StatusId),
                Edit = "<a href='/Product/AddNewOrderlocator?id=" + Orederloct.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteOrderlocator?id=" + Orederloct.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<OrderLocatorViewModel> GetAllOrderLocatorList()
        {

            var result = _context.OrderLocators.Where(x => x.StatusId != 4)
                .Select(ToOrderLocatorViewModel).ToList();
            return result;

        }

        public OrderLocatorViewModel GetOrderLocator(Int64 id)
        {
            OrderLocatorViewModel editbrand = new OrderLocatorViewModel();
            OrderLocator brand = _context.OrderLocators.FirstOrDefault(x => x.Id == id);

            if (brand != null)
            {
                return ToOrderLocatorViewModel(brand);
            }
            else
            {
                return null;
            }
        }

        public void DeleteOrderLocator(Int64 id)
        {
            OrderLocator brand = _context.OrderLocators.Where(x => x.Id == id).FirstOrDefault();
            brand.StatusId = 4;
            brand.ModificationDate = DateTime.UtcNow;
            brand.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }
        #endregion

        #region: Collection year

        public Int64 SaveCollectionYear(CollectionYearViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.CollectionYears.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    CollectionYear collectionYear = new CollectionYear()
                    {
                        Year = model.CollectionYear,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.CollectionYears.Add(collectionYear);

                }
                else
                {
                    chk.Year = model.CollectionYear;
                    chk.Description = model.Description;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<CollectionYearViewModel> GetAllCollectionYears(int start, int length, string search, int filtercount)
        {
            List<CollectionYearViewModel> newData = new List<CollectionYearViewModel>();

            try
            {
                List<CollectionYearViewModel> result = new List<CollectionYearViewModel>();

                if (search == String.Empty)
                {
                    result = _context.CollectionYears.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToCollectionYearViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.CollectionYears.Where(p => p.Year.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToCollectionYearViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public CollectionYearViewModel ToCollectionYearViewModel(CollectionYear collectionYear)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            CollectionYearViewModel ab = new CollectionYearViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            return new CollectionYearViewModel()
            {
                //UserId = User.Id,
                Id = collectionYear.Id,
                CollectionYear = collectionYear.Year,
                Description = collectionYear.Description,
                CreatedById = Convert.ToInt64(collectionYear.CreatedById),
                CreationDate = Convert.ToDateTime(collectionYear.CreationDate),
                ModifiedById = Convert.ToInt64(collectionYear.ModifiedById),
                ModificationDate = Convert.ToDateTime(collectionYear.ModificationDate),
                StatusId = Convert.ToInt16(collectionYear.StatusId),
                Edit = "<a href='/Product/AddNewCollectionYear?id=" + collectionYear.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteCollectionYear?id=" + collectionYear.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<CollectionYearViewModel> GetAllCollectionYearList()
        {
            var result = _context.CollectionYears.Where(x => x.StatusId != 4)
                .Select(ToCollectionYearViewModel).ToList();
            return result;
        }

        public CollectionYearViewModel GetCollectionYear(Int64 id)
        {
            CollectionYearViewModel editcollectioncear = new CollectionYearViewModel();
            CollectionYear collectionyear = _context.CollectionYears.FirstOrDefault(x => x.Id == id);

            if (collectionyear != null)
            {
                return ToCollectionYearViewModel(collectionyear);
            }
            else
            {
                return null;
            }
        }

        public void DeleteCollectionYear(Int64 id)
        {
            CollectionYear collectionYear = _context.CollectionYears.Where(x => x.Id == id).FirstOrDefault();
            collectionYear.StatusId = 4;
            collectionYear.ModificationDate = DateTime.UtcNow;
            collectionYear.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: Colour

        public Int64 SaveColour(ColourViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.Colours.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    Colour colour = new Colour()
                    {
                        ColourName = model.Colour,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.Colours.Add(colour);
                }
                else
                {
                    chk.ColourName = model.Colour;
                    chk.Description = model.Description;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<ColourViewModel> GetAllColours(int start, int length, string search, int filtercount)
        {
            List<ColourViewModel> newData = new List<ColourViewModel>();

            try
            {
                List<ColourViewModel> result = new List<ColourViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Colours.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToColourViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Colours.Where(p => p.ColourName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToColourViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public ColourViewModel ToColourViewModel(Colour colour)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            ColourViewModel ab = new ColourViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            return new ColourViewModel()
            {
                //UserId = User.Id,
                Id = colour.Id,
                Colour = colour.ColourName,
                Description = colour.Description,
                CreatedById = Convert.ToInt64(colour.CreatedById),
                CreationDate = Convert.ToDateTime(colour.CreationDate),
                ModifiedById = Convert.ToInt64(colour.ModifiedById),
                ModificationDate = Convert.ToDateTime(colour.ModificationDate),
                StatusId = Convert.ToInt16(colour.StatusId),
                Edit = "<a href='/Product/AddNewColour?id=" + colour.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteColour?id=" + colour.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive = ab.IsAdmin,
            };
        }

        public List<ColourViewModel> GetAllColourList()
        {

            var result = _context.Colours.Where(x => x.StatusId != 4)
                .Select(ToColourViewModel).ToList();
            return result;

        }

        public ColourViewModel GetColour(Int64 id)
        {
            ColourViewModel editcolour = new ColourViewModel();
            Colour colour = _context.Colours.FirstOrDefault(x => x.Id == id);

            if (colour != null)
            {
                return ToColourViewModel(colour);
            }
            else
            {
                return null;
            }
        }

        public void DeleteColour(Int64 id)
        {
            Colour colour = _context.Colours.Where(x => x.Id == id).FirstOrDefault();
            colour.StatusId = 4;
            colour.ModificationDate = DateTime.UtcNow;
            colour.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: Cut Of Dress

        public Int64 SaveCutOfDress(CutOfDressViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.CutOfDresses.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    CutOfDress cutOfDress = new CutOfDress()
                    {
                        CutOfDressName = model.CutOfDress,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.CutOfDresses.Add(cutOfDress);
                }
                else
                {
                    chk.CutOfDressName = model.CutOfDress;
                    chk.Description = model.Description;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<CutOfDressViewModel> GetAllCutOfDresses(int start, int length, string search, int filtercount)
        {
            List<CutOfDressViewModel> newData = new List<CutOfDressViewModel>();

            try
            {
                List<CutOfDressViewModel> result = new List<CutOfDressViewModel>();

                if (search == String.Empty)
                {
                    result = _context.CutOfDresses.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToCutOfDressViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.CutOfDresses.Where(p => p.CutOfDressName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToCutOfDressViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public CutOfDressViewModel ToCutOfDressViewModel(CutOfDress cutOfDress)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            CutOfDressViewModel ab = new CutOfDressViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            return new CutOfDressViewModel()
            {
                //UserId = User.Id,
                Id = cutOfDress.Id,
                CutOfDress = cutOfDress.CutOfDressName,
                Description = cutOfDress.Description,
                CreatedById = Convert.ToInt64(cutOfDress.CreatedById),
                CreationDate = Convert.ToDateTime(cutOfDress.CreationDate),
                ModifiedById = Convert.ToInt64(cutOfDress.ModifiedById),
                ModificationDate = Convert.ToDateTime(cutOfDress.ModificationDate),
                StatusId = Convert.ToInt16(cutOfDress.StatusId),
                Edit = "<a href='/Product/AddNewCutOfDress?id=" + cutOfDress.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteCutOfDress?id=" + cutOfDress.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<CutOfDressViewModel> GetAllCutOfDressList()
        {

            var result = _context.CutOfDresses.Where(x => x.StatusId != 4)
                .Select(ToCutOfDressViewModel).ToList();
            return result;

        }

        public CutOfDressViewModel GetCutOfDress(Int64 id)
        {
            CutOfDressViewModel editcutofdress = new CutOfDressViewModel();
            CutOfDress cutofdress = _context.CutOfDresses.FirstOrDefault(x => x.Id == id);

            if (cutofdress != null)
            {
                return ToCutOfDressViewModel(cutofdress);
            }
            else
            {
                return null;
            }
        }

        public void DeleteCutOfDress(Int64 id)
        {
            CutOfDress cutOfDress = _context.CutOfDresses.Where(x => x.Id == id).FirstOrDefault();
            cutOfDress.StatusId = 4;
            cutOfDress.ModificationDate = DateTime.UtcNow;
            cutOfDress.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: Category

        public Int64 SaveCategory(CategoryViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.Categories.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    Category category = new Category()
                    {
                        CategoryName = model.Category,
                        Description = model.Description,
                        ImagePath = model.ImagePath,
                        //StatusId = model.StatusId
                    };
                    _context.Categories.Add(category);
                }
                else
                {
                    chk.CategoryName = model.Category;
                    chk.Description = model.Description;
                    chk.ImagePath = model.ImagePath;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<CategoryViewModel> GetAllCategories(int start, int length, string search, int filtercount)
        {
            List<CategoryViewModel> newData = new List<CategoryViewModel>();

            try
            {
                List<CategoryViewModel> result = new List<CategoryViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Categories.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToCategoryViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Categories.Where(p => p.CategoryName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToCategoryViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public CategoryViewModel ToCategoryViewModel(Category category)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            CategoryViewModel ab = new CategoryViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
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
                StatusId = Convert.ToInt16(category.StatusId),
                Edit = "<a href='/Product/AddNewCategory?id=" + category.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteCategory?id=" + category.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<CategoryViewModel> GetAllCategoryList()
        {
            var result = _context.Categories.Where(x => x.StatusId != 4)
                .Select(ToCategoryViewModel).ToList();
            return result;

        }

        public CategoryViewModel GetCategory(Int64 id)
        {
            CategoryViewModel editcategory = new CategoryViewModel();
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category != null)
            {
                return ToCategoryViewModel(category);
            }
            else
            {
                return null;
            }
        }

        public void DeleteCategory(Int64 id)
        {
            Category category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            category.StatusId = 4;
            category.ModificationDate = DateTime.UtcNow;
            category.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();

        }

        #endregion

        #region: Size

        public Int64 SaveSize(SizeViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.Sizes.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    Size size = new Size()
                    {
                        SizeNameUS = model.SizeUS,
                        SizeNameUK = model.SizeUK,
                        SizeNameEU = model.SizeEU,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.Sizes.Add(size);
                }
                else
                {
                    chk.SizeNameUS = model.SizeUS;
                    chk.SizeNameUK = model.SizeUK;
                    chk.SizeNameEU = model.SizeEU;
                    chk.Description = model.Description;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<SizeViewModel> GetAllSizes(int start, int length, string search, int filtercount)
        {
            List<SizeViewModel> newData = new List<SizeViewModel>();

            try
            {
                List<SizeViewModel> result = new List<SizeViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Sizes.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToSizeViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Sizes.Where(p => p.SizeNameUS.Contains(value) || p.SizeNameUK.Contains(value) || p.SizeNameEU.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToSizeViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }


      
        public SizeViewModel ToSizeViewModel(Size size)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            SizeViewModel ab = new SizeViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            return new SizeViewModel()
            {
                //UserId = User.Id,
                Id = size.Id,
                SizeUS = size.SizeNameUS,
                SizeUK = size.SizeNameUK,
                SizeEU = size.SizeNameEU,
                Description = size.Description,
                CreatedById = Convert.ToInt64(size.CreatedById),
                CreationDate = Convert.ToDateTime(size.CreationDate),
                ModifiedById = Convert.ToInt64(size.ModifiedById),
                ModificationDate = Convert.ToDateTime(size.ModificationDate),
                StatusId = Convert.ToInt16(size.StatusId),
                Edit = "<a href='/Product/AddNewSize?id=" + size.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteSize?id=" + size.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<SizeViewModel> GetAllSizeList()
        {

            var result = _context.Sizes.Where(x => x.StatusId != 4)
                .Select(ToSizeViewModel).ToList();
            return result;

        }

        public SizeViewModel GetSize(Int64 id)
        {
            SizeViewModel editcategory = new SizeViewModel();
            Size size = _context.Sizes.FirstOrDefault(x => x.Id == id);

            if (size != null)
            {
                return ToSizeViewModel(size);
            }
            else
            {
                return null;
            }
        }

        public void DeleteSize(Int64 id)
        {
            Size size = _context.Sizes.Where(x => x.Id == id).FirstOrDefault();
            size.StatusId = 4;
            size.ModificationDate = DateTime.UtcNow;
            size.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: Fabric

        public Int64 SaveFabric(FabricViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.Fabrics.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    Fabric fabric = new Fabric()
                    {
                        FabricName = model.FabricName,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.Fabrics.Add(fabric);
                }
                else
                {
                    chk.FabricName = model.FabricName;
                    chk.Description = model.Description;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<FabricViewModel> GetAllFabrics(int start, int length, string search, int filtercount)
        {
            List<FabricViewModel> newData = new List<FabricViewModel>();

            try
            {
                List<FabricViewModel> result = new List<FabricViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Fabrics.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToFabricViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Fabrics.Where(p => p.FabricName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToFabricViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public FabricViewModel ToFabricViewModel(Fabric fabric)
        {

            FabricViewModel ab = new FabricViewModel();
            if (_session.CurrentUser != null)
            {
                if (_session.CurrentUser.RoleId == 1)
                {

                    ab.IsAdmin = true;
                }
                else
                {
                    ab.IsAdmin = false;
                }
            }
            else
            {
                ab.IsAdmin = false;
            }
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();

            return new FabricViewModel()
            {
                //UserId = User.Id,
                Id = fabric.Id,
                FabricName = fabric.FabricName,
                Description = fabric.Description,
                CreatedById = Convert.ToInt64(fabric.CreatedById),
                CreationDate = Convert.ToDateTime(fabric.CreationDate),
                ModifiedById = Convert.ToInt64(fabric.ModifiedById),
                ModificationDate = Convert.ToDateTime(fabric.ModificationDate),
                StatusId = Convert.ToInt16(fabric.StatusId),
                Edit = "<a href='/Product/AddNewFabric?id=" + fabric.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteFabric?id=" + fabric.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
                IsActive=ab.IsAdmin,
            };
        }

        public List<FabricViewModel> GetAllFabricList()
        {
            var result = _context.Fabrics.Where(x => x.StatusId != 4)
                .Select(ToFabricViewModel).ToList();
            return result;
        }

        public FabricViewModel GetFabric(Int64 id)
        {
            FabricViewModel editfabric = new FabricViewModel();
            Fabric fabric = _context.Fabrics.FirstOrDefault(x => x.Id == id);

            if (fabric != null)
            {
                return ToFabricViewModel(fabric);
            }
            else
            {
                return null;
            }
        }

        public void DeleteFabric(Int64 id)
        {
            Fabric fabric = _context.Fabrics.Where(x => x.Id == id).FirstOrDefault();
            fabric.StatusId = 4;
            fabric.ModificationDate = DateTime.UtcNow;
            fabric.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: Tax

        public Int64 SaveTax(TaxViewModel model)
        {

            Int64 i = 0;
            try
            {
                var chk = _context.Taxes.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    Tax Tax = new Tax()
                    {
                        TaxPercentage = model.TaxPercentage,
                        Description = model.Description,
                        //StatusId = model.StatusId
                    };
                    _context.Taxes.Add(Tax);
                }
                else
                {
                    chk.TaxPercentage = model.TaxPercentage;
                    chk.Description = model.Description;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();


                return i;
            }
            catch (Exception ex) { return i; }


        }

        public List<TaxViewModel> GetAllTaxes(int start, int length, string search, int filtercount)
        {
            List<TaxViewModel> newData = new List<TaxViewModel>();

            try
            {
                List<TaxViewModel> result = new List<TaxViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Taxes.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToTaxViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                //if (search != String.Empty)
                //{
                //    var value = search.Trim();
                //    result = _context.Taxes.Where(p => p.TaxPercentage.Contains(value))
                //        .OrderByDescending(x => x.Id).ToList()
                //        .Select(ToTaxViewModel).ToList();


                //    filtercount = result.Count;
                //    newData = result;
                //}

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public TaxViewModel ToTaxViewModel(Tax tax)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
            
            return new TaxViewModel()
            {
                //UserId = User.Id,
                Id = tax.Id,
                TaxPercentage = tax.TaxPercentage,
                Description = tax.Description,
                CreatedById = Convert.ToInt64(tax.CreatedById),
                CreationDate = Convert.ToDateTime(tax.CreationDate),
                ModifiedById = Convert.ToInt64(tax.ModifiedById),
                ModificationDate = Convert.ToDateTime(tax.ModificationDate),
                StatusId = Convert.ToInt16(tax.StatusId),
                Edit = "<a href='/Product/AddNewTax?id=" + tax.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteTax?id=" + tax.Id + "' title='Delete'  onclick='return Confirmation();'>Delete</a>",
             
            };
        }

        public List<TaxViewModel> GetAllTaxList()
        {

            var result = _context.Taxes.Where(x => x.StatusId != 4)
                .Select(ToTaxViewModel).ToList();
            return result;

        }

        public TaxViewModel GetTax(Int64 id)
        {
            TaxViewModel edittax = new TaxViewModel();
            Tax Tax = _context.Taxes.FirstOrDefault(x => x.Id == id);

            if (Tax != null)
            {
                return ToTaxViewModel(Tax);
            }
            else
            {
                return null;
            }
        }

        public void DeleteTax(Int64 id)
        {
            Tax tax = _context.Taxes.Where(x => x.Id == id).FirstOrDefault();
            tax.StatusId = 4;
            tax.ModificationDate = DateTime.UtcNow;
            tax.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: Update Inventory

        public List<SizeViewModel> GetSizeModelForInventory(PurchaseOrderViewModel model)
        {
            try
            {
                List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
                for (int i = 2; i <= 34; i += 2)
                {
                    SizeModelList.Add(new SizeViewModel()
                    {
                        SizeUK = i.ToString(),
                        Qty = 0,
                        ReceivedQty = 0
                    });
                }

                var con = new SqlConnection(connectionString);

                con.Open();
                SqlCommand cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + model.WareHouseId + " and ProductId= " + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd.CommandTimeout = timeout;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Table1");
                da.Fill(dt);
                con.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt.Rows[i][1]);
                        Int64 POQty = Convert.ToInt64(dt.Rows[i][2]);
                        string POQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId;
                        //  string POQtystr = dt.Rows[i][2].ToString();

                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }
                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQtyStr = POQtystr; }

                        #region New code 02-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();

                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != (int)OrderStatus.Cancelled && k.POStatusId != 4 && k.WareHouseId == model.WareHouseId).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                                    //        // var cusdetail = _context.MasterUsers.Where(k => k.Id == ordermastr.SupplierId).FirstOrDefault();
                                    //        modl.Customername = ordermastr.SupplierName;
                                    //        modl.OrderNo = ordermastr.OrderRefrence;
                                    //        modl.Quantity = item.Qty;
                                    //        modellist.Add(modl);
                                    //        model.IsDataAvailable = true;

                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQtyStr = POQtystr; }

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.ReceivedQty = ReceivedQty; }
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }


                    }
                }
                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                SqlCommand cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where  InventoryType='Stock' and StatusId!=4 and WareHouseId=" + model.WareHouseId + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd7.CommandTimeout = timeout;
                SqlDataAdapter da7 = new SqlDataAdapter(cmd7);
                DataTable dt7 = new DataTable("Table1");
                da7.Fill(dt7);
                con.Close();
                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++)
                    {
                        string SizeUK = dt7.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
                     
                                foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.ReceivedQty = ReceivedQty; }
                            }
                        
                        //  foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = tom.InStockQty + ReceivedQty; }
                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
                    
                }
                //List<StockQuantity> ProductStock = _context.StockQuantities.Where(x => x.ProductId == model.ProductModel.Id && x.InventoryType == "Stock" && x.ColourId == model.ColourId && x.WareHouseId == model.WareHouseId && x.StatusId != 4).ToList();
                //if (ProductStock != null)
                //{
                //    foreach (var data in ProductStock)
                //    {
                //        foreach (var tom in SizeModelList.Where(w => w.SizeUK == data.SizeUK)) { tom.ReceivedQty = data.ReceivedQty; }
                //    }
                //}

                return SizeModelList;
            }
            catch (Exception ex)
            {
                return new List<SizeViewModel>();
            }
        }

        public List<WareHouseViewModel> GetAllWareHouse()
        {
            // List<WareHouseViewModel> WareHouseList = ;
            List<WareHouseViewModel> WareHouseList = _context.WareHouses.Where(x => x.StatusId != 4).ToList().Select(ToWareHouseViewModel).OrderBy(x => x.Id).ToList();
            return WareHouseList;
        }

        public WareHouseViewModel ToWareHouseViewModel(WareHouse warehouse)
        {
            WareHouseViewModel model = new WareHouseViewModel()
            {
                Id = warehouse.Id,
                WareHouseName = warehouse.WareHouseName,
                Description = warehouse.Description
            };
            return model;
        }

        public bool SaveUpdatedInventory(PurchaseOrderViewModel Model)
        {
            try
            {
                foreach (var data in Model.ProductList)
                {
                    foreach (var list in data.SizeModel)
                    {
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "Stock" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == Model.WareHouseId && x.StatusId != 4).FirstOrDefault();

                        if (StockDetail == null)
                        {
                            StockQuantity SQ = new StockQuantity();
                            SQ.InventoryType = "Stock";
                            SQ.SizeUK = list.SizeUK;
                            SQ.ProductId = data.Product.Id;
                            SQ.ColourId = Convert.ToInt64(data.Product.ColourId);
                            SQ.WareHouseId = Model.WareHouseId;
                            SQ.ReceivedQty = list.ReceivedQty;
                            SQ.CreatedById = Model.CreatedById;
                            SQ.CreationDate = DateTime.UtcNow;

                            _context.StockQuantities.Add(SQ);
                        }
                        else
                        {
                            StockDetail.ReceivedQty = list.ReceivedQty;
                            StockDetail.ModifiedById = Model.CreatedById;
                            StockDetail.ModificationDate = DateTime.UtcNow;
                            //StockDetail.WareHouseId = Model.WareHouseId;

                        }

                        _context.SaveChanges();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

    }
}
