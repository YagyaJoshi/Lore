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
using System.Net;
using System.Xml;


namespace Loregroup.Provider
{
    public class SettingProvider : ISettingProvider
    {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly ISession _session;
        private readonly IUtilities _utilities;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        int timeout = 3600;

        public SettingProvider(AppContext context, ISecurity security, IUtilities utilities, ISession session)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _session = session;
        }

        #region Registration Frontend

        public string SaveRegistration(FrontendCustomerViewModel model)
        {
            //var Result = "";
            try
            {
                string countryName = _context.Countries.FirstOrDefault(x => x.Id == model.CountryId).CountryName;

                //Get Lat Long Values
                try
                {
                    //to Read the Stream
                    StreamReader sr = null;

                    //The Google Maps API Either return JSON or XML. We are using XML Here
                    //Saving the url of the Google API 
                    string url = String.Format("http://maps.googleapis.com/maps/api/geocode/xml?address=" + model.AddressLine1 + ", " + model.AddressLine2 + ", " + model.City + ", " + countryName + "&sensor=false");

                    //to Send the request to Web Client 
                    WebClient wc = new WebClient();
                    try
                    {
                        sr = new StreamReader(wc.OpenRead(url));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("The Error Occured" + ex.Message);
                    }

                    try
                    {
                        XmlTextReader xmlReader = new XmlTextReader(sr);
                        bool latread = false;
                        bool longread = false;

                        while (xmlReader.Read())
                        {
                            xmlReader.MoveToElement();
                            switch (xmlReader.Name)
                            {
                                case "lat":

                                    if (!latread)
                                    {
                                        xmlReader.Read();
                                        model.Latitude = xmlReader.Value.ToString();
                                        latread = true;

                                    }
                                    break;
                                case "lng":
                                    if (!longread)
                                    {
                                        xmlReader.Read();
                                        model.Longitude = xmlReader.Value.ToString();
                                        longread = true;
                                    }

                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        model.Latitude = "0";
                        model.Longitude = "0";
                        //throw new Exception("An Error Occured"+ ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    model.Latitude = "0";
                    model.Longitude = "0";
                }

            

                if (model.CountryId == 2)
                {
                    model.StateName = _context.States.FirstOrDefault(w => w.Id == model.StateId).Statename;
                }
                else
                {
                    model.StateId = 0;
                }

                String salt = Guid.NewGuid().ToString().Replace("-", "");
                string password = _security.ComputeHash(model.Password, salt);

                var chk = _context.MasterUsers.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    var ShopName = _context.MasterUsers.Where(x => x.ShopName == model.ShopName && x.StatusId != 4).FirstOrDefault();
                    if (ShopName != null)
                    {
                        return "Shop Name Aleardy Exist";
                    }
                    var tax = false;
                    if (model.CountryId == 3)
                    {
                        tax = true;
                    }
                    MasterUser user = new MasterUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.EmailId,
                        Password = password,
                        Keyword = model.Password,
                        RoleId = 6,         // model.RoleId,
                        ShopName = model.ShopName,
                        CompanyName = model.CompanyName,
                        CompanyTaxId = model.CompanyTaxId,
                        CountryId = model.CountryId,
                        StateId = model.StateId,
                        CityId = 0,         // model.TownId,
                        ZipCode = model.ZipCode,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        City = model.City,
                        StateName = model.StateName,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Mobile = model.MobileNo,
                        TelephoneNo = model.TelephoneNo,
                        TaxEnabled = tax,
                        Email = model.EmailId,
                        Fax = model.Fax,
                        Salt = salt,
                        CurrencyId = (int)model.Currency,
                        DistributionPointId = (int)model.DistributionPoint,
                        TaxId = 0,      //model.TaxId,
                        CreatedById = model.CreatedById,
                        CreationDate = DateTime.UtcNow,
                        StatusId = (int)Status.Pending,
                        ShopWebsite=model.WebSite,
                        //StatusId = model.StatusId
                    };
                    _context.MasterUsers.Add(user);
                    _context.SaveChanges();

                    //Save In shipping Table
                    ShippingDetail ship = new ShippingDetail()
                    {        
                        MasterUsersId = user.Id,
                        ShippingFirstName = model.ShippingModel.FirstName,
                        ShippingLastName = model.ShippingModel.LastName,
                        ShippingEmailId = model.ShippingModel.EmailId,
                        ShopName = model.ShippingModel.ShopName,
                        ShippingMobileNo = model.ShippingModel.MobileNo,
                        ShippingAddressLine1 = model.ShippingModel.AddressLine1,
                        ShippingAddressLine2 = model.ShippingModel.AddressLine2,
                        ShippingCompany = model.ShippingModel.CompanyName,
                        ShippingZipCode = model.ShippingModel.ZipCode,
                        ShippingWorkphone = model.ShippingModel.TelephoneNo,
                        ShippingFax = model.ShippingModel.Fax,
                        ShippingCountryId = model.ShippingModel.CountryId,
                        ShippingStateId = model.ShippingModel.StateId,
                        ShippingStateName = model.ShippingModel.StateName,
                        ShippingCity = model.ShippingModel.City,
                        CreatedById = model.CreatedById,
                        CreationDate = DateTime.UtcNow,
                        StatusId = (int)Status.Active
                        //StatusId = model.StatusId
                    };
                    _context.ShippingDetails.Add(ship);
                    _context.SaveChanges();

                }
                else
                {                   
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        
        public List<CustomerViewModel> GetRegistrationByCountry(Int64 CountryId)
        {
            List<CustomerViewModel> UsersList = new List<CustomerViewModel>();

            UsersList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.CountryId == CountryId && x.RoleId != 1 && x.StatusId != 4)
                      .ToList().Select(ToRegistrationCustomerViewModel).ToList();
            if (UsersList != null)
            {
                return UsersList;
            }
            else
            {
                return new List<CustomerViewModel>();
            }
        }

        public CustomerViewModel ToRegistrationCustomerViewModel(MasterUser registration)
        {
            try
            {
                // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
                var CountryName = _context.Countries.Where(x => x.Id == registration.CountryId).Where(x => x.StatusId == 1).Select(x => x.CountryName).FirstOrDefault();
                //var TownName = _context.Cities.Where(x => x.Id == user.CityId).Where(x => x.StatusId == 1).Select(x => x.CityName).FirstOrDefault();
                //var State = _context.States.Where(x => x.Id == user.StateId).Where(x => x.StatusId == 1).Select(x => x.Statename).FirstOrDefault();
                var RoleName = _context.Roles.Where(x => x.Id == registration.RoleId && x.StatusId != 4).Select(x => x.Name).FirstOrDefault();
                var Currency = (Currency)registration.CurrencyId;
                return new CustomerViewModel()
                {
                    //UserId = User.Id,
                    Id = registration.Id,
                    UserName = registration.UserName,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    //Salt = registration.Salt,
                    Password = registration.Password,
                    Keyword = registration.Password,
                    RoleId = registration.RoleId,
                    ShopName = registration.ShopName,
                    CompanyName = registration.CompanyName,
                    CompanyTaxId = registration.CompanyTaxId,
                    CountryId = registration.CountryId,
                    Country = CountryName,
                    Town = registration.City,         //TownName,
                    StateId = registration.StateId,
                    StateName = registration.StateName,
                    ShippingState = registration.StateName,         //State,
                    TownId = registration.CityId,
                    ZipCode = registration.ZipCode,
                    RoleName = RoleName,
                    AddressLine1 = registration.AddressLine1,
                    AddressLine2 = registration.AddressLine2,
                    City = registration.City,
                    //  Latitude = registration.Latitude,
                    //  Longitude = registration.Longitude,
                    MobileNo = registration.Mobile,
                    TelephoneNo = registration.TelephoneNo,
                    EmailId = registration.Email,
                    Currency = (Currency)registration.CurrencyId,
                    CurrencyId = registration.CurrencyId,
                    CustomerFullName = registration.FirstName + " " + registration.LastName,
                    CurrencyName = Currency.ToString(),
                    DistributionPointId = registration.DistributionPointId,
                    DistributionPoint = (DistributionPoint)registration.DistributionPointId,
                    Tax = registration.TaxEnabled,
                    TaxId = registration.TaxId,
                    CreatedById = Convert.ToInt64(registration.CreatedById),
                    CreationDate = Convert.ToDateTime(registration.CreationDate),
                    ModifiedById = Convert.ToInt64(registration.ModifiedById),
                    ModificationDate = Convert.ToDateTime(registration.ModificationDate),
                    StatusId = Convert.ToInt16(registration.StatusId),
                    //Edit = "<a href='/User/CreateUser?id=" + user.Id + "' title='Edit'>Edit</a>",
                    //Delete = "<a href='/User/DeleteUser?id=" + user.Id + "' title='Delete' onclick='return Confirmation();' >Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new CustomerViewModel();
            }
        }

        public CustomerViewModel GetRegistration(Int64 id)
        {
            UserViewModel editcategory = new UserViewModel();
            MasterUser registration = _context.MasterUsers.FirstOrDefault(x => x.Id == id);

            if (registration != null)
            {
                return ToRegistrationCustomerViewModel(registration);
            }
            else
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
                        FromDate = DateTime.ParseExact(model.FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(model.EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                        Address = model.Address,
                        city = model.city,
                        mobileno = model.mobileno,
                        EventTypeId = (int)model.EventTypeId,
                        //EventTypeId = 1,
                        StatusId = 1,
                        image = model.image,
                        BoothNumber = model.BoothNumber,
                        WebsiteUrl = model.WebsiteUrl,
                        AddressOthr=model.Address1,
                        State=model.State,
                        Zipcode=model.Zipcode,

                    };
                    _context.Events.Add(events);
                }
                else
                {
                    chkEvents.Title = model.Title;
                    chkEvents.FromDate = Convert.ToDateTime(DateTime.ParseExact(model.FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    chkEvents.EndDate = Convert.ToDateTime(DateTime.ParseExact(model.EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    chkEvents.Address = model.Address;
                    chkEvents.city = model.city;
                    chkEvents.mobileno = model.mobileno;
                    chkEvents.EventTypeId = (int)model.EventTypeId;
                    chkEvents.ModificationDate = DateTime.UtcNow;
                    chkEvents.ModifiedById = _session.CurrentUser.Id;
                    chkEvents.image = model.image;
                    chkEvents.CustomerId = model.CustomerModel.Id;
                    chkEvents.BoothNumber = model.BoothNumber;
                    chkEvents.WebsiteUrl = model.WebsiteUrl;
                    chkEvents.AddressOthr = model.Address1;
                    chkEvents.State = model.State;
                    chkEvents.Zipcode = model.Zipcode;
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
                EventViewModel ab = new EventViewModel();
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
                return new EventViewModel()
                {
                    BoothNumber = Event.BoothNumber,
                    WebsiteUrl = Event.WebsiteUrl,
                    CustomerId = Event.CustomerId,
                    Id = Event.Id,
                    Title = Event.Title,
                    Address = Event.Address,
                    city = Event.city,
                    mobileno = Event.mobileno,
                    image = Event.image,
                    EventTypeId = (EventType)Event.EventTypeId,
                    FromDate = Event.FromDate.ToShortDateString(),
                    EndDate = Event.EndDate.ToShortDateString(),
                    CreatedById = Convert.ToInt64(Event.CreatedById),
                    CreationDate = Convert.ToDateTime(Event.CreationDate),
                    ModifiedById = Convert.ToInt64(Event.ModifiedById),
                    ModificationDate = Convert.ToDateTime(Event.ModificationDate),
                    Address1=Event.AddressOthr,
                    State=Event.State,
                    Zipcode=Event.Zipcode,
                    //StatusId = Convert.ToInt16(Event.StatusId),                    
                    EventType = "<span style='color:white;background: " + color + "; font-weight:bold;border-radius: 4px;padding: 3px;'>" + (EventType)Event.EventTypeId + "</span>",
                    Edit = "<a href='/Product/AddNewEvents?id=" + Event.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/Product/DeleteEvents?id=" + Event.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>",
                    IsActive=ab.IsAdmin,
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
                ContactViewModel ab = new ContactViewModel();
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
                    Delete = "<a href='/Product/DeleteContact?id=" + contact.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>",
                    IsActive=ab.IsAdmin,
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
                FaqViewModel ab = new FaqViewModel();
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
                    Delete = "<a href='/Product/DeleteFaq?id=" + faq.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>",
                    IsActive=ab.IsAdmin,
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

        #region BaseEntity

        public Int64 SaveBaseEntity(BaseEntityViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkbase = _context.FrontendContents.FirstOrDefault(x => x.SystemName == model.SystemName);

                if (chkbase == null)
                {
                    FrontendContent Base = new FrontendContent()
                    {
                        TextData = model.TextData,
                        //SystemName = model.SystemName,
                        Image = model.Image,
                    };
                    _context.FrontendContents.Add(Base);
                }
                else
                {
                    chkbase.TextData = model.TextData;
                    chkbase.ModificationDate = DateTime.UtcNow;
                    chkbase.ModifiedById = _session.CurrentUser.Id;
                    chkbase.Image = model.Image;
                    chkbase.SystemName = model.SystemName;
                }
                _context.SaveChanges();

                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public BaseEntityViewModel ToBaseEntity(FrontendContent Base)
        {
            try
            {
                return new BaseEntityViewModel()
                {
                    SystemName = Base.SystemName,
                    TextData = Base.TextData,
                    Id = Base.Id,
                    CreatedById = Convert.ToInt64(Base.CreatedById),
                    CreationDate = Convert.ToDateTime(Base.CreationDate),
                    ModifiedById = Convert.ToInt64(Base.ModifiedById),
                    ModificationDate = Convert.ToDateTime(Base.ModificationDate),
                    StatusId = Convert.ToInt16(Base.StatusId),
                    Save = "<a href='/Product/AddNewBaseEntity?id=" + Base.Id + "' title='Save'>Save</a>",
                };
            }
            catch (Exception ex)
            {
                return new BaseEntityViewModel();
            }

        }

        public BaseEntityViewModel GetBaseEntity(Int64 id)
        {
            BaseEntityViewModel editbase = new BaseEntityViewModel();
            FrontendContent Base = _context.FrontendContents.FirstOrDefault(x => x.Id == id);

            if (Base != null)
            {
                return ToBaseEntity(Base);
            }
            else
            {
                return null;
            }
        }

        public BaseEntityViewModel GetBaseEntityById(Int64 id)
        {
            FrontendContent Base = _context.FrontendContents.Where(x => x.Id == id && x.StatusId != 4).FirstOrDefault();
            if (Base != null)
            {
                return ToBaseEntity(Base);
            }
            else
            {
                return new BaseEntityViewModel();
            }
        }

        public BaseEntityViewModel GetBaseEntityByName(string name)
        {
            BaseEntityViewModel model = new BaseEntityViewModel();

            FrontendContent Base = _context.FrontendContents.FirstOrDefault(x => x.SystemName == name);
            if (Base != null)
            {
                return ToBaseEntity(Base);
            }
            else
            {
                return new BaseEntityViewModel();
            }
        }

        #endregion

        #region AboutUs
        public Int64 SaveAboutUs(BaseEntityViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkbase = _context.FrontendContents.FirstOrDefault(x => x.Id == model.Id);

                if (chkbase == null)
                {
                    FrontendContent Base = new FrontendContent()
                    {
                        TextData = model.TextData,
                        SystemName = model.SystemName,
                        Image = model.Image,
                    };
                    _context.FrontendContents.Add(Base);
                }
                else
                {
                    chkbase.TextData = model.TextData;
                    chkbase.ModificationDate = DateTime.UtcNow;
                    chkbase.ModifiedById = _session.CurrentUser.Id;
                    chkbase.Image = model.Image;
                    chkbase.SystemName = model.SystemName;

                }
                _context.SaveChanges();

                return i;

            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public BaseEntityViewModel ToAboutUs(FrontendContent Base)
        {
            try
            {

                return new BaseEntityViewModel()
                {
                    SystemName = Base.SystemName,
                    TextData = Base.TextData,
                    Id = Base.Id,
                    CreatedById = Convert.ToInt64(Base.CreatedById),
                    CreationDate = Convert.ToDateTime(Base.CreationDate),
                    ModifiedById = Convert.ToInt64(Base.ModifiedById),
                    ModificationDate = Convert.ToDateTime(Base.ModificationDate),
                    StatusId = Convert.ToInt16(Base.StatusId),
                    Save = "<a href='/Product/AddNewBaseEntity?id=" + Base.Id + "' title='Save'>Save</a>",
                };
            }
            catch (Exception ex)
            {
                return new BaseEntityViewModel();
            }

        }

        public BaseEntityViewModel GetAboutUs(Int64 id)
        {
            BaseEntityViewModel editbase = new BaseEntityViewModel();
            FrontendContent Base = _context.FrontendContents.FirstOrDefault(x => x.Id == id);

            if (Base != null)
            {
                return ToAboutUs(Base);
            }
            else
            {
                return null;
            }
        }

        public BaseEntityViewModel GetAboutUsById(Int64 id)
        {
            FrontendContent Base = _context.FrontendContents.Where(x => x.Id == id && x.StatusId != 4).FirstOrDefault();
            if (Base != null)
            {
                return ToAboutUs(Base);
            }
            else
            {
                return new BaseEntityViewModel();
            }
        }

        #endregion

        #region ReturnPolicy
        public Int64 SaveReturnPolicy(BaseEntityViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkbase = _context.FrontendContents.FirstOrDefault(x => x.Id == model.Id);

                if (chkbase == null)
                {
                    FrontendContent Base = new FrontendContent()
                    {
                        TextData = model.TextData,
                        SystemName = model.SystemName,
                        Image = model.Image,
                    };
                    _context.FrontendContents.Add(Base);
                }
                else
                {
                    chkbase.TextData = model.TextData;
                    chkbase.ModificationDate = DateTime.UtcNow;
                    chkbase.ModifiedById = _session.CurrentUser.Id;
                    chkbase.Image = model.Image;
                    chkbase.SystemName = model.SystemName;

                }
                _context.SaveChanges();

                return i;

            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public BaseEntityViewModel ToReturnPolicy(FrontendContent Base)
        {
            try
            {

                return new BaseEntityViewModel()
                {
                    SystemName = Base.SystemName,
                    TextData = Base.TextData,
                    Id = Base.Id,
                    CreatedById = Convert.ToInt64(Base.CreatedById),
                    CreationDate = Convert.ToDateTime(Base.CreationDate),
                    ModifiedById = Convert.ToInt64(Base.ModifiedById),
                    ModificationDate = Convert.ToDateTime(Base.ModificationDate),
                    StatusId = Convert.ToInt16(Base.StatusId),
                    Save = "<a href='/Product/AddNewBaseEntity?id=" + Base.Id + "' title='Save'>Save</a>",
                };
            }
            catch (Exception ex)
            {
                return new BaseEntityViewModel();
            }

        }

        public BaseEntityViewModel GetReturnPolicy(Int64 id)
        {
            BaseEntityViewModel editbase = new BaseEntityViewModel();
            FrontendContent Base = _context.FrontendContents.FirstOrDefault(x => x.Id == id);

            if (Base != null)
            {
                return ToReturnPolicy(Base);
            }
            else
            {
                return null;
            }
        }

        public BaseEntityViewModel GetReturnPolicyById(Int64 id)
        {
            FrontendContent Base = _context.FrontendContents.Where(x => x.Id == id && x.StatusId != 4).FirstOrDefault();
            if (Base != null)
            {
                return ToReturnPolicy(Base);
            }
            else
            {
                return new BaseEntityViewModel();
            }
        }

        #endregion

        #region Terms & Conditions
        public Int64 SaveTermsandConditions(BaseEntityViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkbase = _context.FrontendContents.FirstOrDefault(x => x.Id == model.Id);

                if (chkbase == null)
                {
                    FrontendContent Base = new FrontendContent()
                    {
                        TextData = model.TextData,
                        SystemName = model.SystemName,
                        Image = model.Image,
                    };
                    _context.FrontendContents.Add(Base);
                }
                else
                {
                    chkbase.TextData = model.TextData;
                    chkbase.ModificationDate = DateTime.UtcNow;
                    chkbase.ModifiedById = _session.CurrentUser.Id;
                    chkbase.Image = model.Image;
                    chkbase.SystemName = model.SystemName;

                }
                _context.SaveChanges();

                return i;

            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public BaseEntityViewModel ToTermsandConditions(FrontendContent Base)
        {
            try
            {
                return new BaseEntityViewModel()
                {
                    SystemName = Base.SystemName,
                    TextData = Base.TextData,
                    Id = Base.Id,
                    CreatedById = Convert.ToInt64(Base.CreatedById),
                    CreationDate = Convert.ToDateTime(Base.CreationDate),
                    ModifiedById = Convert.ToInt64(Base.ModifiedById),
                    ModificationDate = Convert.ToDateTime(Base.ModificationDate),
                    StatusId = Convert.ToInt16(Base.StatusId),
                    Save = "<a href='/Product/AddNewBaseEntity?id=" + Base.Id + "' title='Save'>Save</a>",
                };
            }
            catch (Exception ex)
            {
                return new BaseEntityViewModel();
            }

        }

        public BaseEntityViewModel GetTermsandConditions(Int64 id)
        {
            BaseEntityViewModel editbase = new BaseEntityViewModel();
            FrontendContent Base = _context.FrontendContents.FirstOrDefault(x => x.Id == id);

            if (Base != null)
            {
                return ToTermsandConditions(Base);
            }
            else
            {
                return null;
            }
        }

        public BaseEntityViewModel GetTermsandConditionById(Int64 id)
        {
            FrontendContent Base = _context.FrontendContents.Where(x => x.Id == id && x.StatusId != 4).FirstOrDefault();
            if (Base != null)
            {
                return ToTermsandConditions(Base);
            }
            else
            {
                return new BaseEntityViewModel();
            }
        }

        #endregion

        #region Gallery
        public Int64 SaveGallery(GalleryViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkGallery = _context.Galleries.FirstOrDefault(x => x.Id == model.Id);
                if (chkGallery == null)
                {
                    Gallery gallery = new Gallery()
                    {
                        Title = model.Title,
                        image = model.image,
                        VideoUrl = model.VideoUrl,
                        GalleryTypeId = (int)model.GalleryTypeId,
                    };
                    _context.Galleries.Add(gallery);
                }
                else
                {
                    chkGallery.Title = model.Title;
                    chkGallery.VideoUrl = model.VideoUrl;
                    chkGallery.image = model.image;
                    chkGallery.GalleryTypeId = (int)model.GalleryTypeId;
                    chkGallery.ModificationDate = DateTime.UtcNow;
                    chkGallery.ModifiedById = _session.CurrentUser.Id;
                }
                _context.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public List<GalleryViewModel> GetAllGallery(int start, int length, string search, int filtercount, Int64 GalleryTypeId)
        {
            List<GalleryViewModel> newData = new List<GalleryViewModel>();
            try
            {
                List<GalleryViewModel> result = new List<GalleryViewModel>();

                IQueryable<Gallery> query = _context.Galleries.OrderByDescending(x => x.Id).Where(y => y.StatusId != 4);

                if (GalleryTypeId != 0)
                {
                    query = query.Where(x => x.GalleryTypeId == GalleryTypeId);
                }

                //Search Condition
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    query = query.Where(p => p.Title.Contains(value));
                }

                result = query.ToList().Select(ToGalleryViewModel).ToList();
                newData = result;

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public GalleryViewModel ToGalleryViewModel(Gallery gallery)
        {
            try
            {
                GalleryViewModel ab = new GalleryViewModel();
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
                return new GalleryViewModel()
                {
                    Title = gallery.Title,
                    Id = gallery.Id,
                    image = gallery.image,
                    VideoUrl = gallery.VideoUrl,
                    GalleryTypeId = (GalleryType)gallery.GalleryTypeId,
                    CreatedById = Convert.ToInt64(gallery.CreatedById),
                    CreationDate = Convert.ToDateTime(gallery.CreationDate),
                    ModifiedById = Convert.ToInt64(gallery.ModifiedById),
                    ModificationDate = Convert.ToDateTime(gallery.ModificationDate),
                    StatusId = Convert.ToInt16(gallery.StatusId),
                    Picture = "<img src='" + gallery.image + "'; font-weight:bold;border-radius: 4px;padding: 3px;' style='height: 100px;'/>",
                    Edit = "<a href='/Setting/AddGallery?id=" + gallery.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/Setting/DeleteGallery?id=" + gallery.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>",
                    IsActive=ab.IsAdmin,

                };
            }
            catch (Exception ex)
            {
                return new GalleryViewModel();
            }

        }

        public List<GalleryViewModel> GetAllGalleryList()
        {
            List<GalleryViewModel> GalleryList = _context.Galleries.Where(x => x.StatusId != 4).Select(ToGalleryViewModel).OrderBy(x => x.Id).ToList();
            return GalleryList;
        }

        public List<GalleryViewModel> GetAllGalleryList(int Typeid)
        {
            List<GalleryViewModel> GalleryList = _context.Galleries.Where(x => x.StatusId != 4 && x.GalleryTypeId == Typeid).Select(ToGalleryViewModel).OrderBy(x => x.Id).ToList();
           if(Typeid==2)
               GalleryList = _context.Galleries.Where(x => x.StatusId != 4 && x.GalleryTypeId == Typeid).Select(ToGalleryViewModel).OrderByDescending(x => x.Id).ToList();
            return GalleryList;
        }

        public GalleryViewModel GetGallery(Int64 id)
        {
            GalleryViewModel editGallery = new GalleryViewModel();
            Gallery gallery = _context.Galleries.FirstOrDefault(x => x.Id == id);

            if (gallery != null)
            {
                return ToGalleryViewModel(gallery);
            }
            else
            {
                return null;
            }
        }

        public void DeleteGallery(Int64 id)
        {
            Gallery gallery = _context.Galleries.Where(x => x.Id == id).FirstOrDefault();
            gallery.StatusId = 4;
            gallery.ModificationDate = DateTime.UtcNow;
            gallery.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region WishList
        public Int64 SaveWishlist(WishlistViewModel model)
        {
            Int64 i = 0;

            try
            {
                var chk = _context.Wishlists.FirstOrDefault(x => x.Id == model.Id);

                if (chk == null)
                {
                    Wishlist wish = new Wishlist()
                    {

                        ProductName = model.ProductName,
                        StockStatus = model.StockStatus,
                        Price = (int)model.Price,
                        ProductId = (int)model.ProductId,
                        UserId = (int)model.UserId,
                    };
                    _context.Wishlists.Add(wish);
                }
                else
                {
                    chk.ProductName = model.ProductName;
                    chk.ProductId = (int)model.ProductId;
                    chk.Price = (int)model.Price;
                    chk.UserId = (int)model.UserId;
                    chk.StockStatus = model.StockStatus;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = _session.CurrentUser.Id;

                }
                _context.SaveChanges();

                return i;

            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public WishlistViewModel ToWishlistViewModel(Wishlist wish)
        {
            try
            {
                string pImage = "";

                try
                {
                    pImage = _context.Products.FirstOrDefault(x => x.Id == wish.ProductId).Picture1;
                }
                catch (Exception ex)
                {
                }

                return new WishlistViewModel()
                {
                    Id = wish.Id,
                    ProductName = wish.ProductName,
                    ProductId = Convert.ToInt64(wish.ProductId),
                    ProductImage = pImage,
                    Price = Convert.ToInt64(wish.Price),
                    StockStatus = wish.StockStatus,
                    CreatedById = Convert.ToInt64(wish.CreatedById),
                    CreationDate = Convert.ToDateTime(wish.CreationDate),
                    ModifiedById = Convert.ToInt64(wish.ModifiedById),
                    ModificationDate = Convert.ToDateTime(wish.ModificationDate),
                    StatusId = Convert.ToInt16(wish.StatusId),
                    Delete = "<a href='/Setting/DeleteWishlist?id=" + wish.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new WishlistViewModel();
            }

        }

        public List<WishlistViewModel> GetAllWishList(Int64 userid)
        {
            var records = _context.Wishlists.Where(x => x.UserId == userid).ToList();
            List<WishlistViewModel> wishlist = new List<WishlistViewModel>();
            Int64 currencyid = 0;
            try
            {
                currencyid = _context.MasterUsers.FirstOrDefault(x => x.Id == userid).CurrencyId.Value;
            }
            catch (Exception)
            {
            }

            foreach (var item in records)
            {
                try
                {
                    string pImage = "";
                    var p = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
                    try
                    {
                        pImage = p.Picture1;
                    }
                    catch (Exception ex)
                    {
                    }

                    decimal priceValue = 0;
                    if(currencyid == 1)
                    {
                        priceValue = p.PriceUSD;
                    }
                    else if (currencyid == 2)
                    {
                        priceValue = p.PriceEURO;
                    }
                    else if (currencyid == 3)
                    {
                        priceValue = p.PriceGBP;
                    }
                    else
                    {
                        priceValue = p.PriceUSD;
                    }

                    wishlist.Add(new WishlistViewModel()
                    {
                        Id = item.Id,
                        ProductName = item.ProductName,
                        ProductId = Convert.ToInt64(item.ProductId),
                        ProductImage = pImage,
                        Price = Convert.ToInt64(priceValue),
                        StockStatus = item.StockStatus,
                        CreatedById = Convert.ToInt64(item.CreatedById),
                        CreationDate = Convert.ToDateTime(item.CreationDate),
                        ModifiedById = Convert.ToInt64(item.ModifiedById),
                        ModificationDate = Convert.ToDateTime(item.ModificationDate),
                        StatusId = Convert.ToInt16(item.StatusId),
                        Delete = "<a href='/Setting/DeleteWishlist?id=" + item.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>"
                    });
                }
                catch (Exception ex)
                {
                }
            }
                //.Select(ToWishlistViewModel).ToList();
            return wishlist;
        }

        public WishlistViewModel GetWishlist(Int64 id)
        {
            WishlistViewModel editEvent = new WishlistViewModel();
            Wishlist wish = _context.Wishlists.FirstOrDefault(x => x.Id == id);

            if (wish != null)
            {
                return ToWishlistViewModel(wish);
            }
            else
            {
                return null;
            }
        }


        public void DeleteWishlist(Int64 Id)
        {
            Wishlist wish = _context.Wishlists.FirstOrDefault(x => x.Id == Id);
            _context.Wishlists.Remove(wish);
            //var itemToRemove = _context.Wishlists.SingleOrDefault(x => x.Id == 1 && x.UserId!= 1); 

            //     if (itemToRemove != null) {
            //     _context.Wishlists.Remove(itemToRemove);
            _context.SaveChanges();
        }


        #endregion


        #region StoreLocator :

        public List<UserLocation> GetAllLocationsForStoreLocator(Int64 countryid, Int64 stateid, string zipcd)
        {
            List<UserLocation> UsersList = new List<UserLocation>();

            //Only Shop Users
            IQueryable<MasterUser> query = _context.MasterUsers.Where(x => x.RoleId == 3 && x.StatusId != 4 && x.ShowOnMap == true);


            //UsersList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.RoleId != 1 && x.StatusId != 4)
            //          .ToList().Select(ToUserLocation).ToList();

            if (countryid > 0)
            {
                query = query.Where(x=>x.CountryId == countryid);
            }

            if (stateid > 0)
            {
                query = query.Where(x => x.StateId == stateid);
            }
            if (zipcd != "" && zipcd!=null)
            {
                query = query.Where(x => x.ZipCode == zipcd);
            }
            UsersList = query.ToList().Select(ToUserLocation).ToList();

            if (UsersList != null)
            {
                return UsersList;
            }
            else
            {
                return new List<UserLocation>();
            }
        }

        private UserLocation ToUserLocation(MasterUser user)
        {
            try
            {
                return new UserLocation()
                {
                    ShopName = user.ShopName,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude,
                    UserName = user.FirstName + " " + user.LastName,
                };
            }
            catch (Exception ex)
            {
                return new UserLocation();
            }
        }

        public string SaveStoreLocator(StoreViewModel model)
        {

            try
            {
                string countryName = _context.Countries.FirstOrDefault(x => x.Id == model.CountryId).CountryName;

                if (model.CountryId == 2)
                {
                    model.StateName = _context.States.FirstOrDefault(w => w.Id == model.StateId).Statename;
                }
                else
                {
                    model.StateId = 0;
                }

                var chk = _context.MasterUsers.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    var ShopName = _context.MasterUsers.Where(x => x.ShopName == model.ShopName).FirstOrDefault();
                    if (ShopName != null)
                    {
                        return "Shop Name Aleardy Exist";
                    }
                    var tax = false;
                    if (model.CountryId == 3)
                    {
                        tax = true;
                    }
                    MasterUser user = new MasterUser()
                    {
                        UserName = model.UserName,
                        ShopName = model.ShopName,
                        CountryId = model.CountryId,
                        StateId = model.StateId,
                        CityId = 0,
                        ZipCode = model.ZipCode,
                        City = model.City,
                        StateName = model.StateName,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        CreatedById = model.CreatedById,
                        CreationDate = DateTime.UtcNow

                    };
                    _context.MasterUsers.Add(user);
                }
                else
                {
                    var ShopName = _context.MasterUsers.Where(x => x.ShopName == model.ShopName && x.Id != model.Id && x.StatusId != 4).FirstOrDefault();
                    if (ShopName != null)
                    {
                        return "Shop Name Aleardy Exist";
                    }

                    var tax = false;
                    if (model.CountryId == 3)
                    {
                        tax = true;
                    }

                    chk.UserName = model.UserName;
                    chk.ShopName = model.ShopName;
                    chk.CountryId = model.CountryId;
                    chk.StateId = model.StateId;
                    chk.CityId = 0;//model.TownId;
                    chk.ZipCode = model.ZipCode;
                    chk.Longitude = model.Longitude;
                    chk.Latitude = model.Latitude;
                    chk.StateName = model.StateName;
                    chk.City = model.City;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = model.ModifiedById;// _session.CurrentUser.Id;
                }
                _context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public List<StoreViewModel> GetAllShopForMap()
        {
            List<StoreViewModel> ShopList = new List<StoreViewModel>();

            ShopList = _context.MasterUsers.OrderByDescending(x => x.UserName).Where(x => x.Id != 1 && x.StatusId != 4).ToList().Select(ToStoreLocatorStoreViewModel).ToList();
            if (ShopList != null)
            {
                return ShopList;
            }
            else
            {
                return new List<StoreViewModel>();
            }
        }

        //public StoreViewModel GetAllShopnameForMap(string name)
        //{
        //    StoreViewModel model = new StoreViewModel();

        //    MasterUser Base = _context.MasterUsers.FirstOrDefault(x => x.ShopName == name);
        //    if (Base != null)
        //    {
        //        return ToShopLocation(Base);
        //    }
        //    else
        //    {
        //        return new StoreViewModel();
        //    }
        //}


        //private StoreViewModel ToShopLocation(MasterUser user)
        //{
        //    try
        //    {
        //        return new StoreViewModel()
        //        {
        //            CountryId = user.CountryId ,
        //            Latitude = user.Latitude,
        //            Longitude = user.Longitude,
        //            State=user.StateName
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new StoreViewModel();
        //    }
        //}


        public List<StoreViewModel> GetStoreLocatorByCountry(Int64 CountryId)
        {
            List<StoreViewModel> ShopList = new List<StoreViewModel>();

            ShopList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.CountryId == CountryId && x.RoleId != 1 && x.StatusId != 4)
                      .ToList().Select(ToStoreLocatorStoreViewModel).ToList();
            if (ShopList != null)
            {
                return ShopList;
            }
            else
            {
                return new List<StoreViewModel>();
            }

        }


        public List<StoreViewModel> GetStoreLocatorByState(Int64 StateId)
        {
            List<StoreViewModel> ShopList = new List<StoreViewModel>();

            ShopList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.StateId == StateId && x.RoleId != 1 && x.StatusId != 4)
                      .ToList().Select(ToStoreLocatorStoreViewModel).ToList();
            if (ShopList != null)
            {
                return ShopList;
            }
            else
            {
                return new List<StoreViewModel>();
            }

        }


        public StoreViewModel ToStoreLocatorStoreViewModel(MasterUser store)
        {
            try
            {
                var CountryName = _context.Countries.Where(x => x.Id == store.CountryId).Where(x => x.StatusId == 1).Select(x => x.CountryName).FirstOrDefault();
                var TownName = _context.Cities.Where(x => x.Id == store.CityId).Where(x => x.StatusId == 1).Select(x => x.CityName).FirstOrDefault();
                var State = _context.States.Where(x => x.Id == store.StateId).Where(x => x.StatusId == 1).Select(x => x.Statename).FirstOrDefault();

                return new StoreViewModel()
                {
                    Id = store.Id,
                    UserName = store.UserName,
                    ShopName = store.ShopName,
                    CountryId = store.CountryId,
                    // Town = store.City,         
                    StateId = store.StateId,
                    StateName = store.StateName,
                    //TownId = store.CityId,
                    ZipCode = store.ZipCode,
                    City = store.City,
                    //Latitude = store.Latitude,
                    //Longitude = store.Longitude,                  
                    CreatedById = Convert.ToInt64(store.CreatedById),
                    CreationDate = Convert.ToDateTime(store.CreationDate),
                    ModifiedById = Convert.ToInt64(store.ModifiedById),
                    ModificationDate = Convert.ToDateTime(store.ModificationDate),
                    StatusId = Convert.ToInt16(store.StatusId),
                };
            }
            catch (Exception ex)
            {
                return new StoreViewModel();
            }
        }

        public StoreViewModel GetStoreLocator(Int64 id)
        {
            UserViewModel editcategory = new UserViewModel();
            MasterUser store = _context.MasterUsers.FirstOrDefault(x => x.Id == id);

            if (store != null)
            {
                return ToStoreLocatorStoreViewModel(store);
            }
            else
            {
                return null;
            }
        }

        public List<StoreViewModel> GetAllStoreLocator(Int64 CountryId, Int64 StateId)
        {
            List<StoreViewModel> newData = new List<StoreViewModel>();

            try
            {
                List<StoreViewModel> result = new List<StoreViewModel>();
                IQueryable<MasterUser> query = _context.MasterUsers.OrderByDescending(x => x.Id).Where(y => y.StatusId != 4);

                if (CountryId == 0)
                {
                    query = query.Where(x => x.CountryId == CountryId);
                }

                if (StateId != 0)
                {
                    query = query.Where(x => x.StateId == StateId);
                }

                result = query.ToList().Select(ToStoreLocatorStoreViewModel).ToList();
                newData = result;

                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }


        public List<StoreViewModel> GetAllStoreList()
        {
            List<StoreViewModel> UsersList = new List<StoreViewModel>();

            UsersList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.RoleId != 1 && x.StatusId != 4)
                      .ToList().Select(ToStoreLocatorStoreViewModel).ToList();
            if (UsersList != null)
            {
                return UsersList;
            }
            else
            {
                return new List<StoreViewModel>();
            }
        }

        #endregion

        #region Slider Methods

        public Int64 SaveSlider(SliderViewModel model)
        {
            Int64 i = 0;
            try
            {
                var chkSlider = _context.Sliders.FirstOrDefault(x => x.Id == model.Id);
                if (chkSlider == null)
                {                   
                }
                else
                {
                    chkSlider.ImageUrl = model.ImageUrl;
                    chkSlider.FirstText = model.FirstText;
                    chkSlider.SecondText = model.SecondText;
                    chkSlider.IsVisible = model.IsVisible;
                }
                _context.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                return i;
            }
        }

        public List<SliderViewModel> GetAllSliders(int start, int length, string search, int filtercount)
        {
            List<SliderViewModel> newData = new List<SliderViewModel>();
            try
            {
                List<SliderViewModel> result = new List<SliderViewModel>();

                IQueryable<Slider> query = _context.Sliders.OrderBy(x => x.Id);
                               
                //Search Condition
                if (search != String.Empty)
                {
                    //var value = search.Trim();
                    //query = query.Where(p => p.Title.Contains(value));
                }

                result = query.ToList().Select(ToSliderViewModel).ToList();
                newData = result;

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public SliderViewModel ToSliderViewModel(Slider slider)
        {
            try
            {
                return new SliderViewModel()
                {
                    Id = slider.Id,
                    ImageUrl = slider.ImageUrl,
                    FirstText = slider.FirstText,
                    SecondText = slider.SecondText,
                    IsVisible = slider.IsVisible,
                    Picture = "<img src='" + slider.ImageUrl + "'; font-weight:bold;border-radius: 4px;padding: 3px;' style='height: 100px;'/>",
                    Edit = "<a href='/Setting/AddSlider?id=" + slider.Id + "' title='Edit'>Edit</a>",
                };
            }
            catch (Exception ex)
            {
                return new SliderViewModel();
            }

        }

        public List<SliderViewModel> GetAllSlidersList()
        {
            List<SliderViewModel> SlidersList = _context.Sliders.Where(x => x.IsVisible == true).Select(ToSliderViewModel).OrderBy(x => x.Id).ToList();
            return SlidersList;
        }
               
        public SliderViewModel GetSlider(Int64 id)
        {
            SliderViewModel edit = new SliderViewModel();
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider != null)
            {
                return ToSliderViewModel(slider);
            }
            else
            {
                return null;
            }
        }

        #endregion


    }

}
