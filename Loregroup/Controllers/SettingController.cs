using DataTables.Mvc;
using Loregroup.Core;
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Logmodels;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Loregroup.Controllers
{
    public class SettingController : Controller
    {
        private readonly ISettingProvider _settingProvider;
        private readonly ICommonProvider _commonProvider;
        private readonly ISession _session;
        private readonly AppContext _context;
        private readonly IMasterProvider _MasterProvider;
        private readonly IUserProvider _userProvider;

        public SettingController(AppContext context, IMasterProvider masterProvider, ICommonProvider commonProvider, ISession session, ISettingProvider settingProvider, IUserProvider userProvider)
        {
            _commonProvider = commonProvider;
            _session = session;
            _settingProvider = settingProvider;
            _MasterProvider = masterProvider;
            _context = context;
            _userProvider = userProvider;
        }

        #region Events
        public ActionResult AddNewEvents(Int64 id = 0)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Create New Event",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            try {
                var Current = _session.CurrentUser.Id;
                EventViewModel model = new EventViewModel();

                if (id > 0)
                {
                    model = _settingProvider.GetEvents(id);
                    model.CustomerModel = _userProvider.GetUser(model.CustomerId);
                    //model.EventTypeId = (EventType)EventType.EventTypeId;
                }

                return View(model);

            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        public ActionResult AddNewEvents(EventViewModel model, HttpPostedFileBase Img1)
        {
            try
            {
                int? status = _context.Events.Where(x => x.Title.ToLower().Trim() == model.Title.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {

                    if (Img1 != null)
                    {
                        model.image = SaveImageEvent(Img1);
                    }

                    _settingProvider.SaveEvents(model);
                    return RedirectToAction("GetAllEvents");
                }
            }
            catch
            {
                return RedirectToAction("GetAllEvents");
            }
        }

        public string SaveImageEvent(HttpPostedFileBase file)
        {
            string imagepath = null;
            string newimagename = "";
            string ImagePathForDb = "";

            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpeg" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        //ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else
                    {
                        newimagename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("ddmmyyyyss") + Path.GetExtension(file.FileName);

                        imagepath = Path.Combine(Server.MapPath("/Content/EventImages"), newimagename);
                        ImagePathForDb = "/Content/EventImages/" + newimagename;
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 1300)
                            img.Resize(1000, 600);
                        img.Save(imagepath);
                    }
                }
            }
            catch (Exception ex) { }
            return ImagePathForDb;
        }

        public ActionResult GetAllEvents()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Events",
                SubBreadCrumbModel = null
            };
            try {
                var Current = _session.CurrentUser.Id;
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllEvents([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<EventViewModel> list = new List<EventViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<EventViewModel> Report = new JsonListModel<EventViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                EventViewModel model = new EventViewModel();
                model.EventList = _settingProvider.GetAllEvents(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.EventList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteEvents(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _settingProvider.DeleteEvents(Id);
            }
            else
            {


                TempData["Message22"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllEvents");
        }

        [HttpPost]
        public JsonResult GetCustomerById(Int32 number)
        {
            try
            {
                CustomerViewModel model = _userProvider.GetUser(number);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                CustomerViewModel model = new CustomerViewModel();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SearchByShopName(string Prefix)
        {
            try
            {
                List<CustomerViewModel> customer = _context.MasterUsers.Where(x => x.ShopName.Contains(Prefix) && x.StatusId != 4 && x.Id != 1).ToList()
                                .Select(y => new CustomerViewModel { ShopName = y.ShopName, Id = y.Id }).ToList();
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Contact
        public ActionResult AddNewContact(Int64 id = 0)
        {
            try
            {
                var cont = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Create New Contact",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                ContactViewModel model = new ContactViewModel();

                if (id > 0)
                {
                    model = _settingProvider.GetContact(id);
                }

                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddNewContact(ContactViewModel model)
        {
            try
            {
                int? status = _context.Contacts.Where(x => x.OfficeName.ToLower().Trim() == model.OfficeName.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _settingProvider.SaveContact(model);
                    return RedirectToAction("GetAllContact");
                }
            }
            catch
            {
                return RedirectToAction("GetAllContact");
            }
        }

        public ActionResult GetAllContact()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Contact",
                SubBreadCrumbModel = null
            };
            try
            {
                var current = _session.CurrentUser.Id;
            ViewBag.BreadCrumb = breadCrumbModel;
            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllContact([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<ContactViewModel> list = new List<ContactViewModel>();
            var totalCount = 1;
            int filterCount = 0;

            JsonListModel<ContactViewModel> Report = new JsonListModel<ContactViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                ContactViewModel model = new ContactViewModel();
                model.ContactList = _settingProvider.GetAllContact(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.ContactList;
                filterCount = list.Count;
            }
            catch (Exception ex)
            {

            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteContact(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _settingProvider.DeleteContact(Id);
        }
            else
            {


                TempData["Message23"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllContact");
        }

        #endregion

        #region Faq
        public ActionResult AddNewFaq(Int64 id = 0)
        {
            try
            {
                var current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Create New Faq",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                FaqViewModel model = new FaqViewModel();

                if (id > 0)
                {
                    model = _settingProvider.GetFaq(id);

                }

                return View(model);
            }

            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddNewFaq(FaqViewModel model)
        {
            try
            {
                int? status = _context.Faqs.Where(x => x.Question.ToLower().Trim() == model.Question.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _settingProvider.SaveFaq(model);
                    return RedirectToAction("GetAllFaq");
                }
            }
            catch
            {
                return RedirectToAction("GetAllFaq");
            }
        }


        public ActionResult GetAllFaq()
        {
            try
            {
                var cont = _session.CurrentUser.Id;
            
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Faq",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            return View();
        }
             catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllFaq([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<FaqViewModel> list = new List<FaqViewModel>();
            var totalCount = 1;
            int filterCount = 0;

            JsonListModel<FaqViewModel> Report = new JsonListModel<FaqViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                FaqViewModel model = new FaqViewModel();
                model.FaqList = _settingProvider.GetAllFaq(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.FaqList;
                filterCount = list.Count;
            }
            catch (Exception ex)
            { }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFaq(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _settingProvider.DeleteFaq(Id);
            }
            else
            {


                TempData["Message24"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllFaq");
        }

        #endregion

        #region BaseEntity
        public ActionResult AddNewBaseEntity(string name)
        {
            try
            {
                var cost = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "AddNewSettings",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                BaseEntityViewModel model = new BaseEntityViewModel();

                if (name != "")
                {
                    model.TextData = _context.FrontendContents.FirstOrDefault(x => x.SystemName == name).TextData;
                    model.SystemName = name;
                }

                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewBaseEntity(BaseEntityViewModel model)
        {
            try
            {
                int? status = _context.FrontendContents.Where(x => x.TextData.ToLower().Trim() == model.TextData.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _settingProvider.SaveBaseEntity(model);
                    return RedirectToAction("AddNewBaseEntity", new { name = model.SystemName });
                }
            }
            catch
            {
                return RedirectToAction("AddNewBaseEntity", new { name = model.SystemName });
            }
        }

        [HttpPost]
        public JsonResult GetBaseEntityByName(string name)
        {
            try
            {
                List<BaseEntityViewModel> Base = _context.FrontendContents.Where(x => x.SystemName.Contains(name)).ToList()
                                .Select(y => new BaseEntityViewModel { SystemName = y.SystemName }).ToList();
                return Json(Base, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        //#region About Us
        //public ActionResult AddNewSettings (Int64 id = 0)
        //{
        //    var breadCrumbModel = new BreadCrumbModel()
        //    {
        //        Url = "/Home/",
        //        Title = "About Us",
        //        SubBreadCrumbModel = null
        //    };
        //    ViewBag.BreadCrumb = breadCrumbModel;

        //    BaseEntityViewModel model = new BaseEntityViewModel();

        //    if (id > 0)
        //    {
        //        model = _settingProvider.GetAboutUs(id);
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddNewSettings (BaseEntityViewModel model)
        //{
        //    try
        //    {
        //        int? status = _context.FrontendContents.Where(x => x.TextData.ToLower().Trim() == model.TextData.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
        //        if (status == 1 && model.Id == 0)
        //        {
        //            TempData["Error1"] = "Duplicate data found!";
        //            return View(model);
        //        }
        //        else
        //        {
        //            _settingProvider.SaveAboutUs(model);
        //            return RedirectToAction("AddNewSettings");
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("AddNewSettings");
        //    }

        //}

        //#endregion

        //#region Return Policy
        //public ActionResult AddNewSettings (Int64 id = 0)
        //{
        //    var breadCrumbModel = new BreadCrumbModel()
        //    {
        //        Url = "/Home/",
        //        Title = "Return Policy",
        //        SubBreadCrumbModel = null
        //    };
        //    ViewBag.BreadCrumb = breadCrumbModel;

        //    BaseEntityViewModel model = new BaseEntityViewModel();

        //    if (id > 0)
        //    {
        //        model = _settingProvider.GetReturnPolicy(id);
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddNewSettings (BaseEntityViewModel model)
        //{
        //    try
        //    {
        //        int? status = _context.FrontendContents.Where(x => x.TextData.ToLower().Trim() == model.TextData.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
        //        if (status == 1 && model.Id == 0)
        //        {
        //            TempData["Error1"] = "Duplicate data found!";
        //            return View(model);
        //        }
        //        else
        //        {
        //            _settingProvider.SaveReturnPolicy(model);
        //            return RedirectToAction("AddNewSettings");
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("AddNewSettings");
        //    }

        //}

        //#endregion

        //#region Terms and condition
        //public ActionResult AddNewSettings (Int64 id = 0)
        //{
        //    var breadCrumbModel = new BreadCrumbModel()
        //    {
        //        Url = "/Home/",
        //        Title = "Terms & Conditions",
        //        SubBreadCrumbModel = null
        //    };
        //    ViewBag.BreadCrumb = breadCrumbModel;

        //    BaseEntityViewModel model = new BaseEntityViewModel();

        //    if (id > 0)
        //    {
        //        model = _settingProvider.GetTermsandConditions(id);
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddNewSettings (BaseEntityViewModel model)
        //{
        //    try
        //    {
        //        int? status = _context.FrontendContents.Where(x => x.TextData.ToLower().Trim() == model.TextData.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
        //        if (status == 1 && model.Id == 0)
        //        {
        //            TempData["Error1"] = "Duplicate data found!";
        //            return View(model);
        //        }
        //        else
        //        {
        //            _settingProvider.SaveTermsandConditions(model);
        //            return RedirectToAction("AddNewSettings");
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("AddNewSettings");
        //    }

        //}

        //#endregion

        #region Registration:

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult Registration(Int64 id = 0)
        {
            FrontendCustomerViewModel model = new FrontendCustomerViewModel();
            //model.ModifiedById = _session.CurrentUser.Id;
            //model.CreatedById = _session.CurrentUser.Id;

            try
            {
                model.CountryList = _MasterProvider.getAllCountries();
                model.CountryList.Insert(0, new CountryViewModel
                {
                    Id = 0,
                    countryName = "--Select Country--"
                });

                model.StateList.Insert(0, new StateViewModel
                {
                    Id = 0,
                    Statename = "--Select State--"
                });
            }
            catch (Exception ex)
            {

            }

            //if (id > 0)
            //{
            //    var breadCrumbModel = new BreadCrumbModel()
            //    {
            //        Url = "/Frontend/",
            //        Title = "Registration",
            //        SubBreadCrumbModel = null
            //    };
            //    ViewBag.BreadCrumb = breadCrumbModel;
            //    model = _settingProvider.GetRegistration(id);
            //    model.StateList = _MasterProvider.GetStates(Convert.ToInt64(model.CountryId));
            //}
            //else
            //{
            //    var breadCrumbModel = new BreadCrumbModel()
            //    {
            //        Url = "/Frontend/",
            //        Title = "Registration",
            //        SubBreadCrumbModel = null
            //    };
            //    ViewBag.BreadCrumb = breadCrumbModel;
            //}


            //model.CityList = _MasterProvider.GetAllCity(null, 0, 1);
            //model.CityList.Insert(0, new CityViewModel
            //{
            //    Id = 0,
            //    Cityname = "--Select Town--"
            //});

            //model.RoleList = _userProvider.GetAllRoles();
            //model.RoleList.Insert(0, new RoleViewModel
            //{
            //    Id = 0,
            //    Name = "--Select Customer Type--"
            //});

            return View(model);
        }

        [HttpPost]
        public ActionResult Registration(FrontendCustomerViewModel model)
        {
            try
            {
                try
                {
                    var result = _settingProvider.SaveRegistration(model);
                    
                    //model.CityList = _MasterProvider.GetAllCity(null, 0, 1);
                    //model.CityList.Insert(0, new CityViewModel
                    //{
                    //    Id = 0,
                    //    Cityname = "--Select Town--"
                    //});

                    if (result == "Success")
                    {
                        string r = LoreGroupSendEmail(model.FirstName, model.EmailId, "", "");
                        return RedirectToAction("ThankYou");
                    }
                    else if (result == "Shop Name Aleardy Exist")
                    {
                        model.CountryList = _MasterProvider.getAllCountries();
                        model.CountryList.Insert(0, new CountryViewModel
                        {
                            Id = 0,
                            countryName = "--Select Country--"
                        });

                        model.StateList.Insert(0, new StateViewModel
                        {
                            Id = 0,
                            Statename = "--Select State--"
                        });

                        TempData["ShopNameExist"] = "Shop name already exist use different name.";
                        return View(model);
                    }
                    else
                    {
                        return View("ThankYou");
                    }
                }
                catch (Exception)
                {
                    return View(model);
                }

                return RedirectToAction("ThankYou");
            }
            catch
            {
                return RedirectToAction("Registration");
            }
        }

        public string LoreGroupSendEmail(string Name, string EmailId, string Subject, string Message)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello,</p></td></tr>"
                                 + "<tr><td align='left'><p>A New Customer has been registered in your system. Details :</p></td></tr><br>"
                                 + "<tr><td align='left'><p>Name : <b>" + Name + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Email-Id : <b>" + EmailId + "</b></p></td></tr>"
                    //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Backend Link : <b> http://lorefashions.com </b></p></td></tr>"
                                 + "<tr><td align='left'><p>&nbsp;</p></td></tr>"
                                 + "<tr><td align='right'><p>Thanks And Regards,</p></td></tr>"
                                 + "<tr><td align='right'><p>Lore-Group.</p></td></tr>";

                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));

                // set the network credentials
                mailClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);

                // enable ssl
                //mailClient.EnableSsl = true;

                // Create the Mail Message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["confirmationEmail"], "Loré Fashions");
                //mailMessage.To.Add(ConfigurationManager.AppSettings["confirmationEmail"]);
                mailMessage.To.Add("sales@lore-group.com");
                mailMessage.Subject = "Lore : New Customer Request";
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
        
        [HttpPost]
        public JsonResult SearchCountry(string Prefix)
        {
            try
            {
                List<CountryViewModel> Countries = _context.Countries.Where(x => x.CountryName.Contains(Prefix) && x.StatusId != 4).ToList()
                                                 .Select(y => new CountryViewModel { countryName = y.CountryName, Id = y.Id }).ToList();
                return Json(Countries, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult GetRegistrationByCountry(Int64 CountryId)
        {
            CustomerViewModel Model = new CustomerViewModel();
            List<UserLocation> Locations = new List<UserLocation>();
            try
            {
                Model.CustomerList = _settingProvider.GetRegistrationByCountry(CountryId);
                //Array[] Locations;

                foreach (var data in Model.CustomerList)
                {
                    //Locations [data.CustomerFullName,data.];
                    Locations.Add(new UserLocation
                    {
                        UserName = data.CustomerFullName,
                        Latitude = data.Latitude,
                        Longitude = data.Longitude
                    });
                    //Model.location = Locations;
                }
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Model.CustomerList = new List<CustomerViewModel>();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
        }

        public int IsEmailExists(string EmailId)
        {
            try
            {
                int rs = 0;
                bool isemailval = IsValidEmail(EmailId);
                if (isemailval == true)
                {
                    rs = _userProvider.IsEmailExists(EmailId);
                }
                else
                {
                    rs = 3;
                }
                return rs;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Gallery
        public ActionResult AddGallery(Int64 id = 0)
        {
            try
            {
                var cot = _session.CurrentUser.Id;

                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "New Gallery ",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                GalleryViewModel model = new GalleryViewModel();

                if (id > 0)
                {
                    model = _settingProvider.GetGallery(id);
                    model.GalleryList = _settingProvider.GetAllGalleryList();
                }

                return View(model);
            }catch(Exception c)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddGallery(GalleryViewModel model, HttpPostedFileBase Img1)
        {
            try
            {
                int? status = _context.Galleries.Where(x => x.Title.ToLower().Trim() == model.Title.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();

                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    if (Img1 != null)
                    {
                        model.image = SaveImageGallery(Img1);
                    }

                    _settingProvider.SaveGallery(model);
                    return RedirectToAction("GetAllGallery");
                }
            }
            catch
            {
                return RedirectToAction("GetAllGallery");
            }
        }

        public string SaveImageGallery(HttpPostedFileBase file)
        {
            string imagepath = null;
            string newimagename = "";
            string ImagePathForDb = "";

            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpeg" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        //ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else
                    {
                        newimagename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("ddmmyyyyss") + Path.GetExtension(file.FileName);

                        imagepath = Path.Combine(Server.MapPath("/Content/GalleryImages"), newimagename);
                        ImagePathForDb = "/Content/GalleryImages/" + newimagename;
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 1300)
                            img.Resize(1000, 600);
                        img.Save(imagepath);
                    }
                }
            }
            catch (Exception ex) { }
            return ImagePathForDb;
        }

        public ActionResult GetAllGallery()
        {
            //GalleryViewModel model = new GalleryViewModel();
            //model.GalleryList = _settingProvider.GetAllGalleryList();

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "GetGallery",
                SubBreadCrumbModel = null
            };
            try
            {
                var Current = _session.CurrentUser.Id;
            ViewBag.BreadCrumb = breadCrumbModel;
            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllGallery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0)
        {
            List<GalleryViewModel> list = new List<GalleryViewModel>();
            var totalCount = 1;
            int filterCount = 0;

            JsonListModel<GalleryViewModel> Report = new JsonListModel<GalleryViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                GalleryViewModel model = new GalleryViewModel();
                model.GalleryList = _settingProvider.GetAllGallery(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, rid);
                list = model.GalleryList;
                filterCount = list.Count;
            }
            catch (Exception ex)
            {

            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DeleteGallery(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _settingProvider.DeleteGallery(Id);
        }
            else
            {


                TempData["Message25"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllGallery");
        }

        #endregion
        
        #region Store Locator:

        public ActionResult StoreLocator(Int64 id = 0)
        {
            StoreLocatorViewModel model = new StoreLocatorViewModel();

            model.CountryList = _MasterProvider.getAllCountries();
            model.CountryList.Insert(0, new CountryViewModel
            {
                Id = 0,
                countryName = "--Select Country--"
            });

            model.StateList.Insert(0, new StateViewModel
            {
                Id = 0,
                Statename = "--Select State--"
            });

            model.CustomerList = _userProvider.GetAllUsersForStoreLocator();           //_settingProvider.GetAllStoreList();
            //model.MapList = _userProvider.GetAllUsersForMap();

            return View(model);
        }

        [HttpPost]
        public ActionResult StoreLocator(StoreLocatorViewModel model)
        {
            try
            {
                var CustomerList = _userProvider.GetAllUsersForStoreLocator(model.CountryId, model.StateId,model.Zipcode);

                model.CountryList = _MasterProvider.getAllCountries();
                model.CountryList.Insert(0, new CountryViewModel
                {
                    Id = 0,
                    countryName = "--Select Country--"
                });
                //_MasterProvider.GetStates(CountryId);
                if (model.CountryId > 0)
                {
                    model.StateList = _MasterProvider.GetStates(model.CountryId);
                    model.StateList.Insert(0, new StateViewModel
                    {
                        Id = 0,
                        Statename = "--Select State--"
                    });
                }
                else
                {
                    model.StateList.Insert(0, new StateViewModel
                    {
                        Id = 0,
                        Statename = "--Select State--"
                    });
                }
                model.CustomerList = CustomerList;
                return View(model);
            }
            catch
            {
                return RedirectToAction("StoreLocator");
            }
        }

        public JsonResult GetAllLocationsForStoreLocator(Int64 CountryId, Int64 StateId,string Zipcode)
        {
            List<UserLocation> Locations = new List<UserLocation>();
            try
            {
                Locations = _settingProvider.GetAllLocationsForStoreLocator(CountryId, StateId,Zipcode);
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAllShop([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 CountryId, Int64 StateId)
        {
            List<StoreViewModel> list = new List<StoreViewModel>();
            var totalCount = 1;
            int filterCount = 0;
            var CurrentUserId = _session.CurrentUser.Id;
            JsonListModel<StoreViewModel> Report = new JsonListModel<StoreViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                StoreViewModel model = new StoreViewModel();
                model.CustomersList = _settingProvider.GetAllStoreLocator(CountryId, StateId);

                list = model.CustomersList;
                filterCount = list.Count;
            }
            catch (Exception ex)
            { }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreLocatorByCountry(Int64 CountryId)
        {
            StoreViewModel Model = new StoreViewModel();
            List<UserLocation> Locations = new List<UserLocation>();
            try
            {
                Model.ShopList = _settingProvider.GetStoreLocatorByCountry(CountryId);
                //Array[] Locations;

                foreach (var data in Model.ShopList)
                {

                    Locations.Add(new UserLocation
                    {

                        Latitude = data.Latitude,
                        Longitude = data.Longitude,
                        CountryId = data.Country,
                        //Statename=data.State

                    });
                    Model.location = Locations;
                }
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Model.ShopList = new List<StoreViewModel>();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SearchState(string Id)
        {
            try
            {
                List<StateViewModel> Countries = _context.States.Where(x => x.Statename.Contains(Id) && x.StatusId != 4).ToList()
                                                 .Select(y => new StateViewModel { Statename = y.Statename, Id = y.Id }).ToList();
                // return Json(Countries, JsonRequestBehavior.AllowGet);
                return Json(new SelectList(Countries, "StateId", "Statename"));
            }
            catch
            {
                return null;
            }
        }


        public JsonResult GetAllShopForMap()
        {
            List<StoreViewModel> Locations = new List<StoreViewModel>();
            try
            {
                Locations = _settingProvider.GetAllStoreList();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetState(string Id)
        {
            StateViewModel model = new StateViewModel();
            var State = _context.States.Where(x => x.Statename.Contains(Id) && x.StatusId != 4).ToList()
                                                            .Select(y => new StateViewModel { Statename = y.Statename, Id = y.Id }).ToList();
            return Json(State, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreLocatorByState(Int64 StateId)
        {
            StoreViewModel Model = new StoreViewModel();
            List<UserLocation> Locations = new List<UserLocation>();
            try
            {
                Model.ShopList = _settingProvider.GetStoreLocatorByState(StateId);
                //Array[] Locations;

                foreach (var data in Model.ShopList)
                {
                    //Locations [data.CustomerFullName,data.];
                    Locations.Add(new UserLocation
                    {
                        Latitude = data.Latitude,
                        Longitude = data.Longitude,
                        Statename = data.StateName
                    });
                    //Model.location = Locations;
                }
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Model.ShopList = new List<StoreViewModel>();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCountry(string Id)
        {
            CountryViewModel entities = new CountryViewModel();
            var countries = _context.Countries.Where(x => x.CountryName.Contains(Id) && x.StatusId != 4).ToList()
                                                 .Select(y => new CountryViewModel { countryName = y.CountryName, Id = y.Id }).ToList();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        public ActionResult DeleteWishlist(Int64 Id = 0)
        {
            try
            {
                _settingProvider.DeleteWishlist(Id);
                return RedirectToAction("Wishlist", "Lore");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Wishlist", "Lore");
            }
        }

        #region Sliders

        public ActionResult AddSlider(Int64 id = 0)
        {

            try
            {
                var cont = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Add Edit Slider",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                SliderViewModel model = new SliderViewModel();

                if (id > 0)
                {
                    model = _settingProvider.GetSlider(id);
                    model.SliderList = _settingProvider.GetAllSlidersList();
                }

                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddSlider(SliderViewModel model, HttpPostedFileBase Img1)
        {
            try
            {                
                    if (Img1 != null)
                    {
                        model.ImageUrl = SaveImageSlider(Img1);
                    }

                    _settingProvider.SaveSlider(model);
                    return RedirectToAction("GetAllSliders");                
            }
            catch
            {
                return RedirectToAction("GetAllSliders");
            }
        }

        public string SaveImageSlider(HttpPostedFileBase file)
        {
            string imagepath = null;
            string newimagename = "";
            string ImagePathForDb = "";

            try
            {
                
                if (file != null && file.ContentLength > 0)
                {
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpeg" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        //ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }
                    else
                    {
                        newimagename = Path.GetFileNameWithoutExtension(file.FileName.Replace(' ','_')) + "_" + DateTime.Now.ToString("ddmmyyyyss") + Path.GetExtension(file.FileName);

                        imagepath = Path.Combine(Server.MapPath("/Content/SliderImages"), newimagename);
                        ImagePathForDb = "/Content/SliderImages/" + newimagename;

                        //WebImage img = new WebImage(file.InputStream);
                        ////if (img.Width > 1300)
                        ////    img.Resize(1000, 600);
                        //img.Save(imagepath);

                        Image sourceimage = Image.FromStream(file.InputStream);
                        //Image photo = Image.FromFile(file.FileName,true);
                        SaveJpeg(imagepath, sourceimage, 75);

                    }
                }
            }
            catch (Exception ex) { }
            return ImagePathForDb;
        }

        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

        public ActionResult GetAllSliders()
        {
            //GalleryViewModel model = new GalleryViewModel();
            //model.GalleryList = _settingProvider.GetAllGalleryList();
            try {
                var cot = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Sliders",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");

            }
        }
        [HttpPost]
        public JsonResult GetAllSliders([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0)
        {
            List<SliderViewModel> list = new List<SliderViewModel>();
            var totalCount = 1;
            int filterCount = 0;

            JsonListModel<SliderViewModel> Report = new JsonListModel<SliderViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                SliderViewModel model = new SliderViewModel();
                model.SliderList = _settingProvider.GetAllSliders(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.SliderList;
                filterCount = list.Count;
            }
            catch (Exception ex)
            {

            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        #endregion

        // Insert Customers List
        public void InsertCustomersExcelSheet()
        {            
            int totalcount = 0;
            int processedcount = 0;
            int failedcount = 0;

            int AlreadyUploadedCount = 0;
            
            DataSet ds = new DataSet();
            int uid = Convert.ToInt32(Session["UserId"]);            

            //Model.Check = true;
            string date2 = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (AlreadyUploadedCount == 0)
            {
                string fileLocation = @"D:\My Work\LoreGroup\Loregroup\Content\usa_migration_20200507_110122848.xlsx";            
                string fileExtension = System.IO.Path.GetExtension(fileLocation);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {                                        
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        //return null;
                    }
                    excelConnection.Close();

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }

                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);

                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                        excelConnection1.Close();
                    }
                }

                try
                {
                    DataTable dt1 = null;

                    #region Lead
                    if (ds != null)
                    {
                        dt1 = ds.Tables[0];
                        if (dt1 != null)
                        {
                            totalcount = dt1.Rows.Count;
                            foreach (DataRow dr in dt1.Rows)
                            {
                                string em = dr[4].ToString().Trim();
                                var user = _context.MasterUsers.FirstOrDefault(x => x.Email == em);
                                if (user != null)
                                {
                                    continue;
                                }
                                Int64 cId = 0;
                                Int64 sId = 0;
                                
                                try
                                {
                                    string cname = dr[19].ToString().Trim();
                                    cId = _context.Countries.FirstOrDefault(x => x.CountryName.ToLower().Contains(cname.ToLower())).Id;

                                    string sn = dr[15].ToString().Trim();
                                    sId = _context.States.FirstOrDefault(x => x.CountryId == cId && x.Statename.ToLower().Contains(sn.ToLower())).Id;
                                }
                                catch (Exception ex)
                                {  }
                                    
                                try
                                {
                                    MasterUser data = new MasterUser();
                                    data.FirstName = dr[2].ToString().Trim();
                                    data.LastName = dr[3].ToString().Trim();
                                    data.Email = dr[4].ToString().Trim();
                                    data.UserName = dr[4].ToString().Trim();
                                    data.CompanyName = dr[6].ToString().Trim();
                                    data.RoleId = 3;  //Shop
                                    data.ShopName = dr[8].ToString().Trim();
                                    data.Mobile = dr[10].ToString();
                                    data.TelephoneNo = dr[11].ToString();
                                    data.AddressLine1 = dr[12].ToString().Trim();
                                    data.AddressLine2 = dr[13].ToString().Trim();
                                    data.City = dr[14].ToString().Trim();
                                    data.CityId = 0;
                                    data.TaxEnabled = false;
                                    data.ZipCode = dr[16].ToString();
                                    data.StateName = dr[15].ToString().Trim();
                                    data.District = "by excelsheet";
                                    data.CurrencyId = 1;
                                    data.DistributionPointId = 1;
                                    data.CountryId = cId;
                                    data.StateId = sId;
                                    data.StatusId = 1;
                                    data.CreatedById = 48;
                                    data.CreationDate = System.DateTime.UtcNow;                                    
                                    _context.MasterUsers.Add(data);
                                    _context.SaveChanges();
                                    processedcount++;                                    
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }

                            failedcount = totalcount - processedcount;                           
                        }

                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    TempData["Error1"] = "Excel File Template does not match";                    
                }
            }            
        }

        // Insert Customers List - 19 Sep 2020  &  13 Oct 2020
        public void InsertCustomersExcelSheetNew()
        {
            int totalcount = 0;
            int processedcount = 0;
            int failedcount = 0;

            int AlreadyUploadedCount = 0;

            DataSet ds = new DataSet();
            int uid = Convert.ToInt32(Session["UserId"]);

            //Model.Check = true;
            string date2 = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (AlreadyUploadedCount == 0)
            {
                
                //string fileLocation = @"D:\My Work\LoreGroup\Loregroup\Content\TIFFANY STOCKISTS_20200919_191417008.xlsx";
                string fileLocation = @"D:\My Work\LoreGroup\Loregroup\Content\Attendee List CREDIT 2021_canada.xlsx";
                string fileExtension = System.IO.Path.GetExtension(fileLocation);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        //return null;
                    }
                    excelConnection.Close();

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }

                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);

                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                        excelConnection1.Close();
                    }
                }

                try
                {
                    DataTable dt1 = null;

                    #region Lead
                    if (ds != null)
                    {
                        dt1 = ds.Tables[0];
                        if (dt1 != null)
                        {
                            totalcount = dt1.Rows.Count;
                            foreach (DataRow dr in dt1.Rows)
                            {
                                string em = dr[4].ToString().Trim();
                                if(string.IsNullOrEmpty(em))
                                {
                                    continue;
                                }
                                var user = _context.MasterUsers.FirstOrDefault(x => x.Email.ToLower() == em.ToLower());
                                if (user != null)
                                {
                                    continue;
                                }
                                Int64 cId = 0;
                                Int64 sId = 0;

                                try
                                {
                                    //string cname = dr[19].ToString().Trim();
                                    //cId = _context.Countries.FirstOrDefault(x => x.CountryName.ToLower().Contains(cname.ToLower())).Id;
                                    cId = 36;
                                    string sn = dr[9].ToString().Trim();
                                    sId = _context.States.FirstOrDefault(x => x.CountryId == cId && x.Statename.ToLower().Contains(sn.ToLower())).Id;
                                }
                                catch (Exception ex)
                                { }

                                try
                                {
                                    MasterUser data = new MasterUser();
                                    data.FirstName = dr[2].ToString().Trim();
                                    data.LastName = dr[3].ToString().Trim();
                                    data.Email = dr[4].ToString().Trim().ToLower();
                                    data.UserName = dr[4].ToString().Trim().ToLower();
                                    data.CompanyName = dr[2].ToString().Trim();
                                    data.RoleId = 3;  //Shop
                                    data.ShopName = dr[0].ToString().Trim();
                                    data.Mobile = dr[11].ToString();
                                    data.TelephoneNo = dr[11].ToString();
                                    data.AddressLine1 = dr[5].ToString().Trim();
                                    data.AddressLine2 = dr[6].ToString().Trim();
                                    data.City = dr[7].ToString().Trim();
                                    data.CityId = 0;
                                    data.TaxEnabled = false;
                                    data.ZipCode = dr[10].ToString();
                                    data.StateName = dr[9].ToString().Trim();
                                    data.District = "by excelsheet";
                                    data.CurrencyId = 1;
                                    data.DistributionPointId = 1;
                                    data.CountryId = cId;
                                    data.StateId = sId;
                                    data.StatusId = 1;
                                    data.CreatedById = 48;
                                    data.CreationDate = System.DateTime.UtcNow;
                                    _context.MasterUsers.Add(data);
                                    _context.SaveChanges();
                                    processedcount++;
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }
                            }

                            failedcount = totalcount - processedcount;
                        }

                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    TempData["Error1"] = "Excel File Template does not match";
                }
            }
        }


    }
}