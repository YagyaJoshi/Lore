using DataTables.Mvc;
using Loregroup.Core;
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Logmodels;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
//using System.Web.Mail;
using System.Web.Mvc;

namespace Loregroup.Controllers
{
    public class UserController : Controller
    {
        private readonly IProductProvider _productProvider;
        private readonly IUserProvider _userProvider;
        private readonly ICommonProvider _commonProvider;
        private readonly ISession _session;
        private readonly AppContext _context;
        private readonly IMasterProvider _MasterProvider;
        private readonly ISecurity _security;

        public UserController(AppContext context, IMasterProvider masterProvider, ICommonProvider commonProvider, ISession session, IUserProvider userProvider, IProductProvider productProvider, ISecurity security)

        { 
               _productProvider = productProvider;
            _commonProvider = commonProvider;
            _session = session;
            _userProvider = userProvider;
            _MasterProvider = masterProvider;
            _context = context;
            _security = security;
        }

        public ActionResult User()
        {
            try
            {

                var Current = _session.CurrentUser.Id;
            //UserViewModel model = new UserViewModel();
            ////Add by 
            //model = _userProvider.GetUser(id);
            //UserModelForMall model = new UserModelForMall();
            //model = _userProvider.GetUsermall(id);
            //var breadCrumbModel = new BreadCrumbModel()
            //{
            //    Url = "/Home/",
            //    Title = "Profile",
            //    SubBreadCrumbModel = null
            //};

            //ViewBag.BreadCrumb = breadCrumbModel;
            return View();//model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //[HttpPost]
        //public ActionResult User(UserModelForMall model)
        //{
        //    _userProvider.UpdateUser(model);
        //    return View(model);
        //}

        #region Customer For Alpha.Lore-Group

        public ActionResult CreateUser(Int64 id = 0)
        {
            try {
                var Current = _session.CurrentUser.Id;
            CustomerViewModel model = new CustomerViewModel();
            model.ModifiedById = _session.CurrentUser.Id;
            model.CreatedById = _session.CurrentUser.Id;
            
            if (id > 0)
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Edit Customer",
                    SubBreadCrumbModel = null
                };

                ViewBag.BreadCrumb = breadCrumbModel;
                ViewBag.RleId = _session.CurrentUser.RoleId;

                model = _userProvider.GetUser(id);
                model.StateList = _MasterProvider.GetStates(Convert.ToInt64(model.CountryId));
            }
            else
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Create New Customer",
                    SubBreadCrumbModel = null
                };

                ViewBag.BreadCrumb = breadCrumbModel;
                ViewBag.RleId = _session.CurrentUser.RoleId;
            }
            model.RoleList = _userProvider.GetAllRoles();
            model.RoleList.Insert(0, new RoleViewModel
            {
                Id = 0,
                Name = "--Select Customer Type--"
            });
          
            model.CountryList = _MasterProvider.getAllCountries();
            model.CountryList.Insert(0, new CountryViewModel
            {
                Id = 0,
                countryName = "--Select Country--"
            });

                model.AgentsList = _productProvider.GetAllAgentList();
                model.AgentsList.Insert(0,new AgentViewModel
                {
                    Id = 0,
                    territory = "--Select Agents--"
                });

            model.StateList = _MasterProvider.GetAllStates();
            model.StateList.Insert(0, new StateViewModel
            {
                Id = 0,
                Statename = "--Select State--"
            });

            //model.CityList = _MasterProvider.GetAllCity(null, 0, 1);
            //model.CityList.Insert(0, new CityViewModel
            //{
            //    Id = 0,
            //    Cityname = "--Select Town--"
            //});

           

          //  model.RoleId = model.RoleId;

            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult CreateUser(CustomerViewModel model)
        {
            try
            {
                var result = _userProvider.SaveUser(model);

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

                model.CityList = _MasterProvider.GetAllCity(null, 0, 1);
                model.CityList.Insert(0, new CityViewModel
                {
                    Id = 0,
                    Cityname = "--Select Town--"
                });

                model.RoleList = _userProvider.GetAllRoles();
                model.RoleList.Insert(0, new RoleViewModel
                {
                    Id = 0,
                    Name = "--Select Customer Type--"
                });
                model.AgentsList = _productProvider.GetAllAgentList();
                model.AgentsList.Insert(0, new AgentViewModel
                {
                    Id = 0,
                    territory = "--Select Agents--"
                });
                //model.AgentsList = _userProvider.GetAllRoles();
                //model.AgentsList.Insert(0, new AgentViewModel
                //{
                //    Id = 0,
                //    territory = "--Select Customer Type--"
                //});
                if (result == "Success")
                {
                    _MasterProvider.SaveUserLog("Manage Users", "Add/Edit User : " + model.Id + " " + model.FirstName + " " + model.LastName, _session.CurrentUser.Id);

                    return RedirectToAction("GetAllUsers");
                }
                else if (result == "Shop Name Aleardy Exist")
                {
                    TempData["ShopNameExist"] = "Shop name already exist use different name.";
                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("GetAllUsers");
            }
        }

        public ActionResult GetAllUsers()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Users",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            CustomerViewModel model = new CustomerViewModel();
            model.RoleList = _userProvider.GetAllRoles();
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                Int64 loginRoleId = _context.MasterUsers.FirstOrDefault(x => x.Id == CurrentUserId).RoleId;
                if (loginRoleId == 1)
                { }
                else
                {
                    model.RoleList.Remove(model.RoleList.FirstOrDefault(x => x.Id == 1));
                    model.RoleList.Remove(model.RoleList.FirstOrDefault(x => x.Id == 2));
                    model.RoleList.Remove(model.RoleList.FirstOrDefault(x => x.Id == 7));
                }
                model.RoleList.Insert(0, new RoleViewModel
                {
                    Id = 0,
                    Name = "--Select Role--"
                });

                model.CountryList = _MasterProvider.getAllCountries();
                model.CountryList.Insert(0, new CountryViewModel
                {
                    Id = 0,
                    countryName = "--Select Country--"
                });


                return View(model);
            }
            catch (Exception e)
            {
               return   RedirectToAction("Login","Account");
            }
        }


    

        [HttpPost]
        public JsonResult GetAllUsers([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 roleId, Int64 countryId, int currencyId, int distributionPointId)
        {
            List<CustomerViewModel> list = new List<CustomerViewModel>();
            var totalCount = 0;
            int filterCount = 0;
            var CurrentUserId = _session.CurrentUser.Id;
            try
            {
                //CustomerViewModel model = new CustomerViewModel();
                //model.CustomerList = _userProvider.GetAllUsers(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, CurrentUserId);
                IQueryable<MasterUser> query = _context.MasterUsers.OrderByDescending(x => x.Id);
                totalCount = query.Count();

                Int64 loginRoleId = _context.MasterUsers.FirstOrDefault(x => x.Id == CurrentUserId).RoleId;
                if (loginRoleId == 1)
                {
                    query = query.Where(x => x.RoleId != 1 && x.StatusId != 4 && x.Id != CurrentUserId);
                }
                else
                {
                    query = query.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 7 && x.StatusId != 4 && x.Id != CurrentUserId);
                }

                if (roleId > 0)
                {
                    query = query.Where(x => x.RoleId == roleId);
                }
                if (countryId > 0)
                {
                    query = query.Where(x => x.CountryId == countryId);
                }
                if (currencyId > 0)
                {
                    query = query.Where(x => x.CurrencyId == currencyId);
                }
                if (distributionPointId > 0)
                {
                    query = query.Where(x => x.DistributionPointId == distributionPointId);
                }

                if (requestModel.Search.Value != String.Empty)
                {
                    var value = requestModel.Search.Value.Trim();
                    query = query.Where(p => p.UserName.Contains(value) || p.ShopName.Contains(value) || p.Email.Contains(value));
                }
                filterCount = query.Count();

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);
                list = query.ToList().Select(ToCustomerViewModel).ToList();
            }
            catch (Exception ex)
            {
            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllUsersbycountry()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Customers By Country",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            CustomerViewModel model = new CustomerViewModel();
            model.RoleList = _userProvider.GetAllRoles();
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                Int64 loginRoleId = _context.MasterUsers.FirstOrDefault(x => x.Id == CurrentUserId).RoleId;
                if (loginRoleId == 1)
                { }
                else
                {
                    model.RoleList.Remove(model.RoleList.FirstOrDefault(x => x.Id == 1));
                    model.RoleList.Remove(model.RoleList.FirstOrDefault(x => x.Id == 2));
                    model.RoleList.Remove(model.RoleList.FirstOrDefault(x => x.Id == 7));
                }
                model.RoleList.Insert(0, new RoleViewModel
                {
                    Id = 0,
                    Name = "--Select Role--"
                });

                model.CountryList = _MasterProvider.getAllCountries();
                model.CountryList.Insert(0, new CountryViewModel
                {
                    Id = 0,
                    countryName = "--Select Country--"
                });

                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }
       

        [HttpPost]
        public JsonResult GetAllUsersbycountry([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel,  Int64 countryId)
        {
            List<CustomerViewModel> list = new List<CustomerViewModel>();
            var totalCount = 0;
            int filterCount = 0;
            var CurrentUserId = _session.CurrentUser.Id;
            try
            {
                //CustomerViewModel model = new CustomerViewModel();
                //model.CustomerList = _userProvider.GetAllUsers(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, CurrentUserId);
                IQueryable<MasterUser> query = _context.MasterUsers.Where(k=>k.CountryId== countryId && k.StatusId!=4).OrderByDescending(x => x.Id);
                totalCount = query.Count();

                //Int64 loginRoleId = _context.MasterUsers.FirstOrDefault(x => x.Id == CurrentUserId).RoleId;
                //if (loginRoleId == 1)
                //{
                //    query = query.Where(x => x.RoleId != 1 && x.StatusId != 4 && x.Id != CurrentUserId);
                //}
                //else
                //{
                //    query = query.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 7 && x.StatusId != 4 && x.Id != CurrentUserId);
                //}

                if (requestModel.Search.Value != String.Empty)
                {
                    var value = requestModel.Search.Value.Trim();
                    query = query.Where(p => p.UserName.Contains(value) || p.ShopName.Contains(value) || p.Email.Contains(value));
                }
                filterCount = query.Count();

                // Paging
               // query = query.Skip(requestModel.Start).Take(requestModel.Length);
                list = query.ToList().Select(ToCustomerViewModelbycountry).ToList();
            }
            catch (Exception ex)
            {
            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public CustomerViewModel ToCustomerViewModel(MasterUser user)
        {
            try
            {
                // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
                var CountryName = _context.Countries.Where(x => x.Id == user.CountryId).Where(x => x.StatusId == 1).Select(x => x.CountryName).FirstOrDefault();
                //var TownName = _context.Cities.Where(x => x.Id == user.CityId).Where(x => x.StatusId == 1).Select(x => x.CityName).FirstOrDefault();
                //var State = _context.States.Where(x => x.Id == user.StateId).Where(x => x.StatusId == 1).Select(x => x.Statename).FirstOrDefault();
                var RoleName = _context.Roles.Where(x => x.Id == user.RoleId && x.StatusId != 4).Select(x => x.Name).FirstOrDefault();
                var Currency = (Currency)user.CurrencyId;
                CustomerViewModel ab = new CustomerViewModel();
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
                return new CustomerViewModel()
                {
                    //UserId = User.Id,
                    Id = user.Id,
                    UserName = user.UserName,
                    //  FirstName = user.FirstName,
                    FirstName = "<a href='/Order/GetOrderListByCustomer?id=" + user.Id + "' title='Edit'>" + user.FirstName + "</a>",
                    LastName = user.LastName,
                    Salt = user.Salt,
                    Password = user.Password,
                    Keyword = user.Password,
                    RoleId = user.RoleId,
                    ShopName = "<a href='/Order/GetOrderListByCustomer?id=" + user.Id + "' title='Check All Orders'>" + user.ShopName + "</a>",
                    CompanyName = user.CompanyName,
                    CompanyTaxId = user.CompanyTaxId,
                    CountryId = user.CountryId,
                    Country = CountryName,
                    Town = user.City,         //TownName,
                    StateId = user.StateId,
                    StateName = user.StateName,
                    ShippingState = user.StateName,         //State,
                    TownId = user.CityId,
                    ZipCode = user.ZipCode,
                    RoleName = RoleName,
                    AddressLine1 = user.AddressLine1,
                    AddressLine2 = user.AddressLine2,
                    City = user.City,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude,
                    MobileNo = user.Mobile,
                    TelephoneNo = user.TelephoneNo,
                    EmailId = user.Email,
                    Currency = (Currency)user.CurrencyId,
                    CurrencyId = user.CurrencyId,
                    CustomerFullName = "<a href='/Order/GetOrderListByCustomer?id=" + user.Id + "' title='Check All Orders'>" + user.FirstName + " " + user.LastName + "</a>",
                    CurrencyName = Currency.ToString(),
                    DistributionPointId = user.DistributionPointId,
                    DistributionPoint = (DistributionPoint)user.DistributionPointId,
                    Tax = user.TaxEnabled,
                    TaxId = user.TaxId,
                    CreatedById = Convert.ToInt64(user.CreatedById),
                    CreationDate = Convert.ToDateTime(user.CreationDate),
                    ModifiedById = Convert.ToInt64(user.ModifiedById),
                    ModificationDate = Convert.ToDateTime(user.ModificationDate),
                    StatusId = Convert.ToInt16(user.StatusId),
                    StatusString = GetStatus(user.StatusId),
                    WebSite = user.ShopWebsite,
                    Edit = "<a href='/User/CreateUser?id=" + user.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/User/DeleteUser?id=" + user.Id + "' title='Delete' onclick='return Confirmation();' >Delete</a>",
                    IsActive = ab.IsAdmin,

                };
            }
            catch (Exception ex)
            {
                return new CustomerViewModel();
            }
        }

        public CustomerViewModel ToCustomerViewModelbycountry(MasterUser user)
        {
            try
            {
                // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
                var CountryName = _context.Countries.Where(x => x.Id == user.CountryId).Where(x => x.StatusId == 1).Select(x => x.CountryName).FirstOrDefault();
                //var TownName = _context.Cities.Where(x => x.Id == user.CityId).Where(x => x.StatusId == 1).Select(x => x.CityName).FirstOrDefault();
                //var State = _context.States.Where(x => x.Id == user.StateId).Where(x => x.StatusId == 1).Select(x => x.Statename).FirstOrDefault();
                var RoleName = _context.Roles.Where(x => x.Id == user.RoleId && x.StatusId != 4).Select(x => x.Name).FirstOrDefault();
                var Currency = (Currency)user.CurrencyId;
                return new CustomerViewModel()
                {
                    //UserId = User.Id,
                    Id = user.Id,
                    UserName = user.UserName,
                    //  FirstName = user.FirstName,
                    FirstName =  user.FirstName,
                    LastName = user.LastName,
                    Salt = user.Salt,
                    Password = user.Password,
                    Keyword = user.Password,
                    RoleId = user.RoleId,
                    ShopName = user.ShopName,
                    CompanyName = user.CompanyName,
                    CompanyTaxId = user.CompanyTaxId,
                    CountryId = user.CountryId,
                    Country = CountryName,
                    Town = user.City,         //TownName,
                    StateId = user.StateId,
                    StateName = user.StateName,
                    ShippingState = user.StateName,         //State,
                    TownId = user.CityId,
                    ZipCode = user.ZipCode,
                    RoleName = RoleName,
                    AddressLine1 = user.AddressLine1,
                    AddressLine2 = user.AddressLine2,
                    City = user.City,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude,
                    MobileNo = user.Mobile,
                    TelephoneNo = user.TelephoneNo,
                    EmailId = user.Email,
                    Currency = (Currency)user.CurrencyId,
                    CurrencyId = user.CurrencyId,
                    CustomerFullName = user.FirstName + " " + user.LastName,
                    CurrencyName = Currency.ToString(),
                    DistributionPointId = user.DistributionPointId,
                    DistributionPoint = (DistributionPoint)user.DistributionPointId,
                    Tax = user.TaxEnabled,
                    TaxId = user.TaxId,
                    CreatedById = Convert.ToInt64(user.CreatedById),
                    CreationDate = Convert.ToDateTime(user.CreationDate),
                    ModifiedById = Convert.ToInt64(user.ModifiedById),
                    ModificationDate = Convert.ToDateTime(user.ModificationDate),
                    StatusId = Convert.ToInt16(user.StatusId),
                    StatusString = GetStatus(user.StatusId),
                    WebSite = user.ShopWebsite,
                    Edit = "<a href='/User/CreateUser?id=" + user.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/User/DeleteUser?id=" + user.Id + "' title='Delete' onclick='return Confirmation();' >Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new CustomerViewModel();
            }
        }
        public string GetStatus(int id)
        {
            if (id == 1)
            {
                return "<span class='label label-success'>" + ((Status)id).ToString() + "</span>";
            }
            else if (id == 2)
            {
                return "<span class='label label-info'>" + ((Status)id).ToString() + "</span>";
            }
            else if (id == 3)
            {
                return "<span class='label label-warning'>" + ((Status)id).ToString() + "</span>";
            }
            else if (id == 4)
            {
                return "<span class='label label-danger'>" + ((Status)id).ToString() + "</span>";
            }
            else
            {
                return ((Status)id).ToString();
            }
            //return ((Status)id).ToString();
        }

        public ActionResult DeleteUser(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _userProvider.DeleteUser(Id);
            }
            else
            {


                TempData["Message13"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllUsers");
        }

        public ActionResult UserMap()
        {
            try {
                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Users Location",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            CustomerViewModel Model = new CustomerViewModel();
            return View(Model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
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

        public JsonResult GetUserByCountry(Int64 CountryId)
        {
            CustomerViewModel Model = new CustomerViewModel();
            List<UserLocation> Locations = new List<UserLocation>();
            try
            {
                Model.CustomerList = _userProvider.GetUserByCountry(CountryId);
                //Array[] Locations;

                foreach (var data in Model.CustomerList)
                {
                    int isonmp = 0;
                    if (data.ShowOnMap == true)
                        isonmp = 1;
                    //Locations [data.CustomerFullName,data.];

                    string addinfo = null;
                    if (data.AddressLine1 != null && data.AddressLine1 != "")
                        addinfo = ", " + data.AddressLine1;
                    if (data.AddressLine2 != null && data.AddressLine2 != "")
                        addinfo = addinfo + ", " + data.AddressLine2;
                    if (data.EmailId != null && data.EmailId != "")
                        addinfo = addinfo + ", " + data.EmailId;
                    if (data.TelephoneNo != null && data.TelephoneNo != "")
                        addinfo = addinfo + ", " + data.TelephoneNo;

                    Locations.Add(new UserLocation
                    {
                        UserName = data.CustomerFullName,
                        ShopName = data.ShopName + " " + addinfo,
                        Latitude = data.Latitude,
                        Longitude = data.Longitude,
                        isonmap = isonmp
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

        public JsonResult GetAllUsersForMap()

        {

            List<UserLocation> Locations = new List<UserLocation>();
            try
            {
                Locations = _userProvider.GetAllUsersForMap();
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Locations, JsonRequestBehavior.AllowGet);
            }
        }

        public string ActivateUser(Int64 id)
        {
            try
            {
                var record = _context.MasterUsers.FirstOrDefault(x => x.Id == id);
                if (record != null)
                {
                    record.StatusId = (int)Status.Active;
                    _context.SaveChanges();
                    var userdetail = _context.MasterUsers.Where(k => k.Id == id).FirstOrDefault();
                    if(userdetail!=null)
                    {
                        SendEmailActivation(userdetail.FirstName, userdetail.Email, userdetail.Keyword);
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }


        public string SendEmailActivation(string Name, string EmailId, string Password)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello " + Name + ",</p></td></tr>"
                                 + "<tr><td align='left'><p>Thank You for registering your account with us. After Completing our vetting checks we are delighted to confirm your account is live and active. Please login with the following credentials :</p></td></tr>"
                                 + "<tr><td align='left'><p>Username : <b>" + EmailId + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>Password : <b>" + Password + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                                 + "<tr><td align='left'><p>website : <b> http://lorefashions.com/Lore/Login </b></p></td></tr>"
                                
                                 + "<tr><td align='left'><p>Once again we thank you for signing up and look forward to having you onboard as a Loré Stockists. Should you have any queries please do not hesitate to contact us.</p></td></tr><br>"
                                 + "<tr><td align='left'><p>Best Regards,</p></td></tr>"
                                 + "<tr><td align='left'><p>Lore-Group.</p></td></tr>";

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
                mailMessage.To.Add(EmailId);
                mailMessage.Subject = "Lore : Account Activated";
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



        public string changePassword(Int64 id, string keyword)
        {
            try
            {
                var record = _context.MasterUsers.FirstOrDefault(x => x.Id == id);
                if (record != null)
                {
                    String salt = Guid.NewGuid().ToString().Replace("-", "");
                    string password = _security.ComputeHash(keyword, salt);

                    record.Password = password;
                    record.Keyword = keyword;
                    record.Salt = salt;
                    record.ModificationDate = DateTime.Now;
                    _context.SaveChanges();

                    return "Success";
                }
                else
                {
                    return "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }


        #endregion

        public bool IsValidBannerimage(object value)
        {
            bool isimg = true;

            if (value == null)
            {
                return true;
            }
            else
            {
                Int64 Width = 400;
                Int64 Height = 400;
                var file333 = value as HttpPostedFileBase;
                try
                {
                    var image = new Bitmap(file333.InputStream);
                    //if (image == null)
                    isimg = true;

                    if (image.Width < Width)
                        isimg = false;

                    if (image.Height < Height)
                        isimg = false;
                }
                catch (Exception ex)
                {
                    isimg = false;
                }
                return isimg;
            }
        }

        public FileContentResult MyImage()
        {

            // Load an existing image
            using (var img = Image.FromFile(Server.MapPath("\\Content\\Customer\\icon-user-default.png")))
            using (var g = Graphics.FromImage(img))
            {

                using (var stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Png);
                    return File(stream.ToArray(), "image/png");
                }
            }
        }

        public bool IsValidBannerimage1(object value)
        {
            bool isimg = true;

            if (value == null)
            {
                return true;
            }
            else
            {
                Int64 Width = 200;
                Int64 Height = 300;
                var file333 = value as HttpPostedFileBase;
                try
                {
                    var image = new Bitmap(file333.InputStream);
                    //if (image == null)
                    isimg = true;

                    if (image.Width < Width)
                        isimg = false;

                    if (image.Height < Height)
                        isimg = false;
                }
                catch (Exception ex)
                {
                    isimg = false;
                }
                return isimg;
            }
        }

        public bool IsValidBannerimage2(object value)
        {
            bool isimg = true;

            if (value == null)
            {
                return true;
            }
            else
            {
                Int64 Width = 100;
                Int64 Height = 100;
                var file333 = value as HttpPostedFileBase;
                try
                {
                    var image = new Bitmap(file333.InputStream);
                    //if (image == null)
                    isimg = true;

                    if (image.Width < Width)
                        isimg = false;

                    if (image.Height < Height)
                        isimg = false;
                }
                catch (Exception ex)
                {
                    isimg = false;
                }
                return isimg;
            }
        }

        // URL reguler expression
        public bool Ismatch(string input)
        {
            bool result = true;
            var regex = @"((http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?|(http(s)?://local/)([\w-/]*)?)";
            //string url=@"";
            var match = Regex.Match(input, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                result = false;
            }
            return result;
        }

        //public ActionResult GenerateOTP()
        //{
        //    UserRegistrationViewModel model = new UserRegistrationViewModel();
        //    string Phoneno = _context.AppUsers.Where(x => x.Id == CommonClass.Userid).Select(x => x.PhoneNumber).FirstOrDefault();
        //    model.PhoneNumber = Phoneno;
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult GenerateOTP(UserRegistrationViewModel Model)
        //{
        //    return View();
        //}

        public ActionResult Check()
        {
            try {
                var Current = _session.CurrentUser.Id;
            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Templete()
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public bool IsValid(object value)
        {
            bool isimg = true;

            if (value == null)
            {
                return true;
            }
            else
            {
                //Int64 Width = 1000;
                //Int64 Height = 200;
                var file333 = value as HttpPostedFileBase;
                try
                {
                    var image = new Bitmap(file333.InputStream);
                    //if (image == null)
                    isimg = true;

                    //if (image.Width < Width)
                    //    isimg = false;

                    //if (image.Height < Height)
                    //    isimg = false;
                }
                catch (Exception ex)
                {
                    isimg = false;
                }
                return isimg;
            }
        }

        public bool IsValidForOffer(object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                Int64 Width = 375;
                Int64 Height = 254;
                var file333 = value as HttpPostedFileBase;
                var image = new Bitmap(file333.InputStream);
                if (image == null)
                    return true;

                if (image.Width < Width)
                    return false;

                if (image.Height < Height)
                    return false;

                return true;
            }
        }

        public FileContentResult OfferImage()
        {
            // Load an existing image
            using (var img = Image.FromFile(Server.MapPath("\\Content\\Customer\\Offer.jpg")))

            // Write the resulting image to the response stream
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }
        }

        //public JsonResult GetLocalities(int Id)
        //{
        //    var StoreTypes = _context.Localities.Where(w => w.CityId == Id).Select(c => new
        //    {
        //        StoreID = c.Id,
        //        StoreName = c.LocalityName,
        //    }).ToList();
        //    ViewBag.StoreTypes = new MultiSelectList(StoreTypes, "StoreID", "StoreName");

        //    //return Json(new SelectList(model.CityViewModel.CityList, "id", "City", model.CityId));
        //    return Json(ViewBag.StoreTypes, JsonRequestBehavior.AllowGet);
        //}        

        //sulabh
        //convert image to bytearray

        public byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        public Image VariousQuality(object value, int no)
        {
            //Int64 Width = 1000;
            //Int64 Height = 200;
            Image retunimg = null;
            var file333 = value as HttpPostedFileBase;
            var original = new Bitmap(file333.InputStream);
            string ImagePath = Server.MapPath("~/Content/images1/");
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            if (jpgEncoder != null)
            {
                Encoder encoder = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                long quality = 100;
                //for (long quality = 10; quality <= 100; quality += 10)
                //{
                EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;

                string fileOut = Path.Combine(ImagePath, "quality_" + no + ".jpeg");

                FileStream ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                original.Save(ms, jpgEncoder, encoderParameters);
                //WebImage photo = new WebImage(@"~\Content/img/quality_40.jpeg");
                //retunimg = Image.FromFile(Path.Combine(Server.MapPath("/Content/img"), "quality_40.jpeg"));
                //var imgee = Server.MapPath("~/Content/img/quality_40.jpeg");

                //retunimg = original;
                //original.Dispose();
                //if (System.IO.File.Exists(fileOut))
                //    System.IO.File.Delete(fileOut);
                ms.Flush();
                ms.Close();
                //}
            }
            return retunimg;
        }

        public FileContentResult ProfilePic()
        {
            // Load an existing image
            using (var img = Image.FromFile(Server.MapPath("\\Content\\Customer\\icon-user-default.png")))
            using (var g = Graphics.FromImage(img))
            {
                using (var stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Png);
                    return File(stream.ToArray(), "image/png");
                }
            }
        }

        public ActionResult ShowAllClassified()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Classified", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        public ActionResult GetUserClassified()
        {
            try
            {

                var current = _session.CurrentUser.Id;
            Int64 Id = 0;

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "All Classified",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            try
            {
                Id = Convert.ToInt64(Session["EditClassified"].ToString());
            }
            catch
            {

            }
            if (Id == 1)
            {
                TempData["Error155"] = "U can't Edit Classified.. ";
                Session["EditClassified"] = 0;
            }
            if (Id == 2)
            {
                TempData["Error155"] = "Please select category first";
                Session["EditClassified"] = 0;
            }
            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #region: Image
        //Start Image Upload: CS

        private const int ImageStoredWidth = 150;  // ToDo - Change the size of the stored image
        private const int ImageStoredHeight = 150; // ToDo - Change the size of the stored image
        private const int ImageScreenWidth = 400;  // ToDo - Change the value of the width of the image on the screen

        private const string TempFolder = "/Temp";
        private const string MapTempFolder = "~" + TempFolder;
        private const string ImagePath = "/ImagePath";

        private readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };

        [HttpGet]
        public ActionResult _Upload()
        {
            return PartialView();
        }

        [ValidateAntiForgeryToken]
        public ActionResult _Upload(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null || !files.Any()) return Json(new { success = false, errorMessage = "No file uploaded." });
            var file = files.FirstOrDefault();  // get ONE only
            if (file == null || !IsImage(file)) return Json(new { success = false, errorMessage = "File is of wrong format." });
            if (file.ContentLength <= 0) return Json(new { success = false, errorMessage = "File cannot be zero length." });
            var webPath = GetTempSavedFilePath(file);
            return Json(new { success = true, fileName = webPath.Replace("/", "\\") }); // success
        }

        [HttpPost]
        public ActionResult Save(string t, string l, string h, string w, string fileName)
        {
            try
            {
                // Calculate dimensions
                var top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
                var left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
                var height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
                var width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

                // Get file from temporary folder
                var fn = Path.Combine(Server.MapPath(MapTempFolder), Path.GetFileName(fileName));
                // ...get image and resize it, ...
                var img = new WebImage(fn);
                img.Resize(width, height);

                int getheight = img.Height - top - ImageStoredHeight;
                int getwidth = img.Width - left - ImageStoredWidth;
                if (getheight < 0)
                {
                    getheight = 0;
                }
                if (getwidth < 0)
                {
                    getwidth = 0;
                }

                //img.Crop(top, left, img.Height - top - ImageStoredHeight, img.Width - left - ImageStoredWidth);
                img.Crop(top, left, getheight, getwidth);
                // ... delete the temporary file,...
                System.IO.File.Delete(fn);
                // ... and save the new one.
                String File_extension = Path.GetExtension(fn);
                String New_fname = _session.CurrentUser.Id.ToString();// +"." + File_extension;
                //var newFileName = Path.Combine(ImagePath, Path.GetFileName(fn));
                var newFileName1 = Path.Combine(ImagePath, New_fname);
                var newFileLocation = HttpContext.Server.MapPath(newFileName1);
                if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));
                }
                string formattype = System.Drawing.Imaging.ImageFormat.Jpeg.ToString();
                img.Save(newFileLocation, formattype, true);
                return Json(new { success = true, avatarFileLocation = newFileName1 });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Unable to upload file.\nERRORINFO: " + ex.Message });
            }
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private string GetTempSavedFilePath(HttpPostedFileBase file)
        {
            // Define destination
            var serverPath = HttpContext.Server.MapPath(TempFolder);
            if (Directory.Exists(serverPath) == false)
            {
                Directory.CreateDirectory(serverPath);
            }

            // Generate unique file name
            var fileName = Path.GetFileName(file.FileName);
            fileName = SaveTemporaryFileImage(file, serverPath, fileName);

            // Clean up old files after every save
            CleanUpTempFolder(1);
            return Path.Combine(TempFolder, fileName);
        }

        private static string SaveTemporaryFileImage(HttpPostedFileBase file, string serverPath, string fileName)
        {
            var img = new WebImage(file.InputStream);
            var ratio = img.Height / (double)img.Width;
            img.Resize(ImageScreenWidth, (int)(ImageScreenWidth * ratio));

            var fullFileName = Path.Combine(serverPath, fileName);
            if (System.IO.File.Exists(fullFileName))
            {
                System.IO.File.Delete(fullFileName);
            }

            img.Save(fullFileName);
            return Path.GetFileName(img.FileName);
        }

        private void CleanUpTempFolder(int hoursOld)
        {
            try
            {
                var currentUtcNow = DateTime.UtcNow;
                var serverPath = HttpContext.Server.MapPath("/Temp");
                if (!Directory.Exists(serverPath)) return;
                var fileEntries = Directory.GetFiles(serverPath);
                foreach (var fileEntry in fileEntries)
                {
                    var fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                    var res = currentUtcNow - fileCreationTime;
                    if (res.TotalHours > hoursOld)
                    {
                        System.IO.File.Delete(fileEntry);
                    }
                }
            }
            catch
            {
                // Deliberately empty.
            }
        }

        //End Image Upload
        #endregion

        //show user advertisment
        public ActionResult ShowAllAdvertisement()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Advertisement", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        //[HttpPost]
        //public JsonResult GetuserAdvertisement(int size, int page)
        //{

        //    //var Offer = _MasterProvider.GetAllMobileApp(null,_session.CurrentUser.Id, page, size);
        //    var advertise = _userProvider.GetuserAdvertisement(null, _session.CurrentUser.Id, page, size);
        //    JsonListModel<AdvertisementViewModel> model = new JsonListModel<AdvertisementViewModel>
        //    {
        //        List = advertise,
        //        Message = "records fetched successfully",
        //        Result = true
        //    };

        //    return Json(model);
        //}

        //[HttpPost]
        //public JsonResult Deleteadvertisement(string id)
        //{
        //    try
        //    {
        //        int Id = 0;
        //        if (id.Contains(','))
        //        {
        //            string[] arr = id.Split(',');
        //            for (int i = 0; i < arr.Length; i++)
        //            {
        //                Id = Convert.ToInt32(arr[i]);
        //                _userProvider.DeleteAdvertisement(Id);


        //            }
        //        }
        //        else
        //        {
        //            Id = Convert.ToInt32(id);
        //            _userProvider.DeleteAdvertisement(Id);



        //        }
        //        LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Delete advertisement record success.");
        //        return Json(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
        //        return Json(id);
        //    }
        //}




        //Show Store notification
        public ActionResult ShowUserNotification()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Notifications", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public JsonResult GetUsernotification(int size, int page)
        {
            //Int64 storeid = _storeProvider.GetstoreId(_session.CurrentUser.Id);
            var Classified = _userProvider.GetUsernotification(_session.CurrentUser.Id, null, 0, 0);

            JsonListModel<NotificationsViewModel> model = new JsonListModel<NotificationsViewModel>
            {
                List = Classified,
                Message = "records fetched successfully",
                Result = true
            };
            return Json(model);
        }

        #region: PermissionMatrix

        public ActionResult PermissionMatrix()
        {

            try
            {
                var Current = _session.CurrentUser.Id;
                try
            {
                NavigationViewModel model = new NavigationViewModel();
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Title = "Add/Edit Roles Permission",
                    SubBreadCrumbModel = new BreadCrumbModel()
                    {
                        SubBreadCrumbModel = null,
                        //Url = "/Settings/PermissionMatrix/",
                        //Title = "Add/Edit Roles Permission"
                    }
                };
                ViewBag.BreadCrumb = breadCrumbModel;
               
                

                model.RoleList = _userProvider.GetAllRoles(null, 0, 0);

                //model.RoleList = _userProvider.GetAllRoles();
                model.RoleList.Insert(0, new RoleViewModel
                {
                    Id = 0,
                    Name = "--Select Role--",
                   
                });

                return View(model);
            }
            catch (Exception ex)
            {
                var model = new PermissionMatrixViewModel();
                return View(model);
            }
        }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult PermissionMatrix(NavigationViewModel model, String Command)
        {
            try
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Title = "Add/Edit Roles Permission",
                    SubBreadCrumbModel = new BreadCrumbModel()
                    {
                        SubBreadCrumbModel = null,
                        //Url = "/Settings/PermissionMatrix/",
                        //Title = "Add/Edit Roles Permission"
                    }
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                if (Command == "Show")
                {
                    NavigationViewModel model1 = new NavigationViewModel();
                    if (model.RoleId != 0)
                    {
                        model1 = _userProvider.GetNavigationList(model.RoleId);
                        model1.NavList = model1.PermissionList.Where(x => x.TypeHold == "Nav").ToList();
                        model1.SubNavList = model1.PermissionList.Where(x => x.TypeHold == "SubNav").ToList();
                        model1.WidgetList = _userProvider.GetWidgetList(model.RoleId);
                        //model1.ZNavList = model1.PermissionList.Where(x => x.TypeHold == "ZNav").ToList();
                    }

                    model1.RoleList = _userProvider.GetAllRoles(null, 0, 0);

                    //model1.RoleList = _userProvider.GetAllRoles();
                    model1.RoleList.Insert(0, new RoleViewModel
                    {
                        Id = 0,
                        Name = "--Select Role--"
                    });
                    return View("PermissionMatrix", model1);
                }
                else if (Command == "Save")
                {
                    _userProvider.SavePermissionMatrices(model);

                    //model = _userProvider.GetNavigationList(model.RoleId);

                    NavigationViewModel model2 = new NavigationViewModel();
                    model2.RoleList = _userProvider.GetAllRoles(null, 0, 0);
                    model2.RoleList.Insert(0, new RoleViewModel
                    {
                        Id = 0,
                        Name = "--Select Role--"
                    });
                    model2.RoleId = 0;
                    ViewBag.Message1 = "Permissions has been updated successfully";

                    return View("PermissionMatrix", model2);
                }
                return RedirectToAction("PermissionMatrix");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.StackTrace);
                return View();
            }
        }

        #endregion

        #region User Logs

        public ActionResult UserLogs()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "User Logs",
                SubBreadCrumbModel = null
            };
            try
            {
                var Current = _session.CurrentUser.Id;
                DateTime lastdate = DateTime.UtcNow.AddDays(-90);
                var logrec = _context.UserLogs.Where(k => k.CreationDate < lastdate).ToList();
                foreach (var item in logrec)
                {
                    _context.UserLogs.Remove(item);
                    _context.SaveChanges();
                }
            
            ViewBag.BreadCrumb = breadCrumbModel;

            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllUserLogs([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<UserLogViewModel> list = new List<UserLogViewModel>();
            var totalCount = 0;
            int filterCount = 0;
            var CurrentUserId = _session.CurrentUser.Id;
            try
            {
                IQueryable<UserLog> query = _context.UserLogs.OrderByDescending(x => x.Id);
                totalCount = query.Count();

                Int64 roleid = Convert.ToInt64(rid);
                if (roleid > 0)
                {
                    query = query.Where(x => x.RoleId == roleid);
                }

                if (requestModel.Search.Value == String.Empty)
                { }

                if (requestModel.Search.Value != String.Empty)
                {
                    var value = requestModel.Search.Value.Trim();
                    query = query.Where(p => p.Module.Contains(value) || p.MessageLog.Contains(value));
                }
                filterCount = query.Count();

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);
                list = query.ToList().Select(ToUserLogViewModel).ToList();
            }
            catch (Exception ex)
            {
            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public UserLogViewModel ToUserLogViewModel(UserLog log)
        {
            try
            {
                var roleName = _context.Roles.Where(x => x.Id == log.RoleId && x.StatusId != 4).Select(x => x.Name).FirstOrDefault();
                var user = _context.MasterUsers.FirstOrDefault(x => x.Id == log.CreatedById);
                return new UserLogViewModel()
                {
                    Id = log.Id,
                    Module = log.Module,
                    MessageLog = log.MessageLog,
                    UserName = user.FirstName + " " + user.LastName,
                    ShopName = user.ShopName,
                    RoleName = roleName,
                    CreatedById = log.CreatedById,
                    CreationDate = log.CreationDate,
                    CreatedDateString = (log.CreationDate != null) ? (log.CreationDate.AddHours(5)).ToString("dd/MM/yyyy HH:mm") : "",
                };
            }
            catch (Exception ex)
            {
                return new UserLogViewModel();
            }
        }


        #endregion

    }
}