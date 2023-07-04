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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Loregroup.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductProvider _productProvider;
        private readonly ICommonProvider _commonProvider;
        private readonly ISession _session;
        private readonly AppContext _context;
        private readonly IMasterProvider _MasterProvider;
        private readonly IUserProvider _userProvider;

        public ProductController(AppContext context, IMasterProvider masterProvider, ICommonProvider commonProvider, ISession session, IProductProvider productProvider, IUserProvider userProvider)
        {
            _commonProvider = commonProvider;
            _session = session;
            _productProvider = productProvider;
            _MasterProvider = masterProvider;
            _context = context;
            _userProvider = userProvider;
        }

        #region Product

        public ActionResult AddNewProduct(Int64 id = 0)
        {
            try {
                var current = _session.CurrentUser.Id;
            ProductViewModel model = new ProductViewModel();
            if (id > 0)
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Edit Product",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                model = _productProvider.GetProduct(id);
            }
            else
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Add New Product",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
            }
            model.BrandList = _productProvider.GetAllBrandList();
           

            model.ColourList = _productProvider.GetAllColourList();
           

            model.CutOfDressList = _productProvider.GetAllCutOfDressList();
            model.CutOfDressList.Insert(0, new CutOfDressViewModel
            {
                Id = 0,
                CutOfDress = "--Select Cut Of Dress--"
            });

            model.SampleColourList = _productProvider.GetAllColourList();
            model.SampleColourList.Insert(0, new ColourViewModel
            {
                Id = 0,
                Colour = "--Select Sample Colour--"
            });


            model.CollectionYearList = _productProvider.GetAllCollectionYearList();
            model.CollectionYearList.Insert(0, new CollectionYearViewModel
            {
                Id = 0,
                CollectionYear = "--Select Collection Year--"
            });

            model.CategoryList = _productProvider.GetAllCategoryList();
            

            model.FabricList = _productProvider.GetAllFabricList();
           
            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddNewProduct(ProductViewModel model, HttpPostedFileBase Img1, HttpPostedFileBase Img2, HttpPostedFileBase Img3, HttpPostedFileBase Img4, HttpPostedFileBase VideoImg)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Add New Product",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            try
            {
                string response = "";
                var chkProductStyle = _context.Products.FirstOrDefault(x => x.ProductName.ToLower() == model.ProductName.ToLower() && x.Id != model.Id && x.StatusId != 4);

                if (chkProductStyle != null)
                {
                    response = "Style Number already exist";
                }
                else
                {
                    if (Img1 != null)
                    {
                        model.Picture1 = SaveImage(Img1);
                    }
                    if (Img2 != null)
                    {
                        model.Picture2 = SaveImage(Img2);
                    }
                    if (Img3 != null)
                    {
                        model.Picture3 = SaveImage(Img3);
                    }
                    if (Img4 != null)
                    {
                        model.Picture4 = SaveImage(Img4);
                    }
                    if (VideoImg != null)
                    {
                        model.VideoImage = SaveImage(VideoImg);
                    }

                    response = _productProvider.SaveProduct(model);
                    if (response == "Success")
                    {
                        _MasterProvider.SaveUserLog("Manage Product", "Add/Edit Product : " + model.ProductName, _session.CurrentUser.Id);

                     
                        return RedirectToAction("GetAllProduct");
                    }
                    else
                    {
                    }
                }                

                model.BrandList = _productProvider.GetAllBrandList();
                model.ColourList = _productProvider.GetAllColourList();
                model.CutOfDressList = _productProvider.GetAllCutOfDressList();
                model.CutOfDressList.Insert(0, new CutOfDressViewModel
                {
                    Id = 0,
                    CutOfDress = "--Select Cut Of Dress--"
                });

                model.CollectionYearList = _productProvider.GetAllCollectionYearList();
                model.CollectionYearList.Insert(0, new CollectionYearViewModel
                {
                    Id = 0,
                    CollectionYear = "--Select Collection Year--"
                });
                model.SampleColourList = _productProvider.GetAllColourList();
                model.SampleColourList.Insert(0, new ColourViewModel
                {
                    Id = 0,
                    Colour = "--Select Sample Colour--"
                });

                model.CategoryList = _productProvider.GetAllCategoryList();
                model.FabricList = _productProvider.GetAllFabricList();

                TempData["ResponseMsg"] = response;
                return View(model);
            }
            catch
            {
                model.BrandList = _productProvider.GetAllBrandList();
                model.ColourList = _productProvider.GetAllColourList();
                model.CutOfDressList = _productProvider.GetAllCutOfDressList();
                model.CutOfDressList.Insert(0, new CutOfDressViewModel
                {
                    Id = 0,
                    CutOfDress = "--Select Cut Of Dress--"
                });

                model.CollectionYearList = _productProvider.GetAllCollectionYearList();
                model.CollectionYearList.Insert(0, new CollectionYearViewModel
                {
                    Id = 0,
                    CollectionYear = "--Select Collection Year--"
                });
                model.SampleColourList = _productProvider.GetAllColourList();
                model.SampleColourList.Insert(0, new ColourViewModel
                {
                    Id = 0,
                    Colour = "--Select Sample Colour--"
                });

                model.CategoryList = _productProvider.GetAllCategoryList();
                model.FabricList = _productProvider.GetAllFabricList();

                TempData["ResponseMsg"] = "Some error occurred";
                
                return View(model);
            }
        }

        public ActionResult GetAllProduct()
       {
            try {
                var current = _session.CurrentUser.Id;
            ProductViewModel model = new ProductViewModel();
            model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
            model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
            model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });
            model.ColourList = _productProvider.GetAllColourList().OrderBy(x => x.Colour).ToList();
            model.ColourList.Insert(0, new ColourViewModel { Id = 0, Colour = "--Select--" });
            model.CutOfDressList = _productProvider.GetAllCutOfDressList().OrderBy(x => x.CutOfDress).ToList();
            model.CutOfDressList.Insert(0, new CutOfDressViewModel { Id = 0, CutOfDress = "--Select--" });
            model.FabricList = _productProvider.GetAllFabricList().OrderBy(x => x.FabricName).ToList();
            model.FabricList.Insert(0, new FabricViewModel { Id = 0, FabricName = "--Select--" });            

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Products",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllProduct([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 categoryId, Int64 collectionYearId, Int64 cutOfDressId, Int64 colourId, Int64 fabricId)
        
        
        {
            List<ProductViewModel> list = new List<ProductViewModel>();
            var totalCount = 0;        
            int filterCount = 0;

            JsonListModel<ProductViewModel> Report = new JsonListModel<ProductViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                
                IQueryable<Product> query = _context.Products.OrderByDescending(x => x.Id);
                totalCount = query.Count();

                if (categoryId > 0)
                {
                    string value = categoryId.ToString();
                    query = query.Where(x => x.SeletedCategoryIds.Contains(value));
                }
                if (collectionYearId > 0)
                {
                    query = query.Where(x => x.CollectionYearId == collectionYearId);
                }
                if (cutOfDressId > 0)
                {
                    query = query.Where(x => x.CutOfDressId == cutOfDressId);
                }
                if (colourId > 0)
                {
                    string value = colourId.ToString();
                    query = query.Where(x => x.SeletedColorIds.Contains(value));
                }
                if (fabricId > 0)
                {
                    string value = fabricId.ToString();
                    query = query.Where(x => x.SelectedFabricIds.Contains(value));
                }

                if (requestModel.Search.Value == String.Empty)
                { }

                if (requestModel.Search.Value != String.Empty)
                {
                    var value = requestModel.Search.Value.Trim();
                    query = query.Where(p => p.ProductName.Contains(value));
                }
                filterCount = query.Count();


                query = query.Skip(requestModel.Start).Take(requestModel.Length);
                list = query.ToList().Select(ToProductViewModel).ToList();
            }
            catch (Exception ex)
            {  }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
      
            string CollectionYear = _context.CollectionYears.Where(x => x.Id == product.CollectionYearId).Where(x => x.StatusId == 1).Select(x => x.Year).FirstOrDefault();
            
            string FabricName = _context.Fabrics.Where(x => x.Id == product.FabricId).Where(x => x.StatusId == 1).Select(x => x.FabricName).FirstOrDefault();
            string CutOfDressName = _context.CutOfDresses.Where(x => x.Id == product.CutOfDressId && x.StatusId == 1).Select(x => x.CutOfDressName).FirstOrDefault();
            string publishValue = "";
            if (product.PublishId == 0)
                publishValue = "Not Published";
            else if (product.PublishId == 1)
                publishValue = "To Everyone";
            else if (product.PublishId == 2)
                publishValue = "To Customers";
       
            ProductViewModel ab = new ProductViewModel();
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
            var  bbb = _context.MasterUsers.Where(x => x.UserName == _session.CurrentUser.UserName && x.RoleId == _session.CurrentUser.RoleId).FirstOrDefault();
             var abb= bbb.DistributionPointId;
            // ab.DistributionId = Convert.ToInt16(abb);
            return new ProductViewModel()
            {
                //UserId = User.Id,
                Id = product.Id,
                ProductName = product.ProductName,
                Title = product.Title,
                Description = product.Description,
                BrandId = product.BrandId,
                //BrandName = BrandName,
                ColourId = product.ColourId,
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
                SeletedCategoryIds = product.SeletedCategoryIds,
                SeletedCategoryNames = product.SeletedCategoryNames,
                SeletedBrandIds = product.SeletedBrandIds,
                SeletedBrandNames = product.SeletedBrandNames,
                SeletedColorIds = product.SeletedColorIds,
                SeletedColorNames = product.SeletedColorNames,
                SelectedFabricIds = product.SelectedFabricIds,
                SelectedFabricNames = (!String.IsNullOrEmpty(product.SelectedFabricNames)) ? product.SelectedFabricNames : FabricName,
                Publish = product.Publish,
                PublishString = publishValue,
                PublishId = product.PublishId.Value,
                VideoImage = product.VideoImage,
                VideoLink = product.VideoLink,
                //CategoryId = catid,
                //CategoryName = CategoryName,
                CreatedById = Convert.ToInt64(product.CreatedById),
                CreationDate = Convert.ToDateTime(product.CreationDate),
                ModifiedById = Convert.ToInt64(product.ModifiedById),
                ModificationDate = Convert.ToDateTime(product.ModificationDate),
                StatusId = Convert.ToInt16(product.StatusId),
                PurchasePrice = product.PurchasePrice,
                Edit = "<a href='/Product/AddNewProduct?id=" + product.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Product/DeleteProduct?id=" + product.Id + "' title='Delete' onclick='return Confirmation();'>Delete</a>",
                IsActive = ab.IsAdmin,
                DistributionId = Convert.ToInt16(abb),
            };
        }

        public ActionResult DeleteProduct(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteProduct(Id);
            }
            else
            {


                TempData["Message11"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllProduct");
        }

        public string SaveImage(HttpPostedFileBase file)
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
                        newimagename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyyymmddMMss") + Path.GetExtension(file.FileName);
                       
                        imagepath = Path.Combine(Server.MapPath("/Content/ProductImages"), newimagename);
                        ImagePathForDb = "/Content/ProductImages/" + newimagename;

                        
                        Image sourceimage = Image.FromStream(file.InputStream);
                        SaveJpeg(imagepath, sourceimage, 75);
                    }
                }
            }
            catch (Exception exception) { }
            return ImagePathForDb;
        }

        #endregion

        #region Events
        public ActionResult AddNewEvents(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Event", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                EventViewModel model = new EventViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetEvents(id);
                    model.CustomerModel = _userProvider.GetUser(model.CustomerId);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
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

                    _productProvider.SaveEvents(model);
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
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Events", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
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
               

                //model.StateList = _masterProvider.GetAllStates(start, length, search, filtercount);
                model.EventList = _productProvider.GetAllEvents(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
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
                _productProvider.DeleteEvents(Id);
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

        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Contact", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                ContactViewModel model = new ContactViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetContact(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
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
                    _productProvider.SaveContact(model);
                    return RedirectToAction("GetAllContact");
                }
            }
            catch
            {
                return RedirectToAction("GetAllContact");
            }
        }

        public ActionResult GetAllContact()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Contact", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
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
                model.ContactList = _productProvider.GetAllContact(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
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
            _productProvider.DeleteContact(Id);
            return RedirectToAction("GetAllContact");
        }

        #endregion

        #region Faq
        public ActionResult AddNewFaq(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Faq", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                FaqViewModel model = new FaqViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetFaq(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
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
                    _productProvider.SaveFaq(model);
                    return RedirectToAction("GetAllFaq");
                }
            }
            catch
            {
                return RedirectToAction("GetAllFaq");
            }
        }


        public ActionResult GetAllFaq()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Faq", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
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
                model.FaqList = _productProvider.GetAllFaq(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.FaqList;
                filterCount = list.Count;
            }
            catch (Exception ex)
            {

            }
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFaq(Int64 Id = 0)
        {
            _productProvider.DeleteFaq(Id);
            return RedirectToAction("GetAllFaq");
        }

        #endregion
       
        public ActionResult AddNewAgents(Int64 id = 0)
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Agents", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                AgentViewModel model = new AgentViewModel();
                try
                {
                    if (id > 0)
                    {
                        model = _productProvider.GetAgents(id);
                    }
                    string rname = "Agent";
                    Int64 rid = _context.Roles.FirstOrDefault(x => x.StatusId == 1 && x.Name.ToLower().Contains(rname)).Id;
                    List<CustomerViewModel> customer = _context.MasterUsers.Where(x => x.StatusId != 4 && x.RoleId == rid).ToList()
                                    .Select(y => new CustomerViewModel { UserName = y.FirstName + " " + y.LastName, Id = y.Id }).ToList();
                    customer.Insert(0, new CustomerViewModel { Id = 0, UserName = "--Select--" });
                    model.CustomerList = customer;
                }
                catch (Exception ex)
                {
                    return null;
                }
               
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }
        [HttpPost]
        public ActionResult AddNewAgents(AgentViewModel model)
        {
            try {
                if (model.CustomerId == 0)
                {
                    TempData["msg_Vide"] = "Select Agents!";
                    return RedirectToAction("AddNewAgents");
                }
            int? status = _context.Agents.Where(x => x.territory.ToLower().Trim() == model.territory.ToLower().Trim() && x.StatusId != 4 && x.AgentsId==model.CustomerId).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
            {
                TempData["Error1"] = "Duplicate data found!";
                return RedirectToAction("AddNewAgents");
            }
            else
            {
                _productProvider.SaveAgents(model);
                return RedirectToAction("GetAllAgent");
            }
        }
            catch(Exception e)
            {
                return RedirectToAction("GetAllAgent");
    }
}
        public ActionResult GetAllAgent()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Agent",
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
        public JsonResult GetAllAgent([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<AgentViewModel> list = new List<AgentViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<AgentViewModel> Report = new JsonListModel<AgentViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                AgentViewModel model = new AgentViewModel();

                model.AgentList = _productProvider.GetAllAgents(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);

                list = model.AgentList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAgent(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteAgents(Id);
            }
            else
            {


                TempData["Message14"] = "You are not authorize.";
            }

            return RedirectToAction("GetAllAgents");

        }

        #region Brand
        public ActionResult AddNewBrand(Int64 id = 0)
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Brand", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                BrandViewModel model = new BrandViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetBrand(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewBrand(BrandViewModel model)
        {
            try
            {

                int? status = _context.Brands.Where(x => x.BrandName.ToLower().Trim() == model.BrandName.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveBrand(model);
                    return RedirectToAction("GetAllBrand");
                }
            }
            catch
            {
                return RedirectToAction("GetAllBrand");
            }
        }

        public ActionResult GetAllBrand()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Brands",
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
        public JsonResult GetAllBrand([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<BrandViewModel> list = new List<BrandViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<BrandViewModel> Report = new JsonListModel<BrandViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                BrandViewModel model = new BrandViewModel();
            
                model.BrandList = _productProvider.GetAllBrands(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
              
                list = model.BrandList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteBrand(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteBrand(Id);
            }
            else
            {


                TempData["Message14"] = "You are not authorize.";
            }
          
            return RedirectToAction("GetAllBrand");

        }

        #endregion


        #region Order Locator
        public ActionResult AddNewOrderlocator(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Order Locator", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                OrderLocatorViewModel model = new OrderLocatorViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetOrderLocator(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewOrderlocator(OrderLocatorViewModel model)
        {
            try
            {

                int? status = _context.OrderLocators.Where(x => x.OrderLocatorName.ToLower().Trim() == model.OrderLocatorName.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveOrderLocator(model);
                    return RedirectToAction("GetAllOrderlocator");
                }
            }
            catch
            {
                return RedirectToAction("GetAllOrderlocator");
            }
        }

        public ActionResult GetAllOrderlocator()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Order Locator",
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
        public JsonResult GetAllOrderlocator([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<OrderLocatorViewModel> list = new List<OrderLocatorViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<OrderLocatorViewModel> Report = new JsonListModel<OrderLocatorViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                OrderLocatorViewModel model = new OrderLocatorViewModel();
             
                model.OrderLocatorList = _productProvider.GetAllOrderLocator(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.OrderLocatorList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteOrderlocator(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteOrderLocator(Id);
            }
            else
            {


                TempData["Message21"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllOrderlocator");
        }

        #endregion


        #region Category
        public ActionResult AddNewCategory(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Category", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                CategoryViewModel model = new CategoryViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetCategory(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewCategory(CategoryViewModel model, HttpPostedFileBase Img1)
        {
            try
            {
                int? status = _context.Categories.Where(x => x.CategoryName.ToLower().Trim() == model.Category.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    if (Img1 != null)
                    {
                        model.ImagePath = SaveImagecategory(Img1);
                    }

                    _productProvider.SaveCategory(model);
                    return RedirectToAction("GetAllCategory");
                }
            }
            catch
            {
                return RedirectToAction("GetAllCategory");
            }
        }

        public string SaveImagecategory(HttpPostedFileBase file)
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
                        newimagename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyyymmddMMss") + Path.GetExtension(file.FileName);

                        imagepath = Path.Combine(Server.MapPath("/Content/CategoryImages"), newimagename);
                        ImagePathForDb = "/Content/CategoryImages/" + newimagename;

                  

                        Image sourceimage = Image.FromStream(file.InputStream);
                        SaveJpeg(imagepath, sourceimage, 75);
                    }
                }
            }
            catch (Exception exception) { }
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

        public ActionResult GetAllCategory()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Categories",
                SubBreadCrumbModel = null
            };
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                ViewBag.BreadCrumb = breadCrumbModel;
            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllCategory([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<CategoryViewModel> list = new List<CategoryViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<CategoryViewModel> Report = new JsonListModel<CategoryViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                CategoryViewModel model = new CategoryViewModel();
              
              

                //model.StateList = _masterProvider.GetAllStates(start, length, search, filtercount);
                model.CategoryList = _productProvider.GetAllCategories(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.CategoryList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCategory(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteCategory(Id);
        }
            else
            {


                TempData["Message15"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllCategory");
        }
        #endregion

        #region Colour
        public ActionResult AddNewColour(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Create New Color", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                ColourViewModel model = new ColourViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetColour(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewColour(ColourViewModel model)
        {
            try
            {
                int? status = _context.Colours.Where(x => x.ColourName.ToLower().Trim() == model.Colour.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveColour(model);
                    return RedirectToAction("GetAllColour");
                }

            }
            catch
            {
                return RedirectToAction("GetAllColour");
            }
        }

        public ActionResult GetAllColour()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Colors",
                SubBreadCrumbModel = null
            };
            try {
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
        public JsonResult GetAllColour([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<ColourViewModel> list = new List<ColourViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<ColourViewModel> Report = new JsonListModel<ColourViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                ColourViewModel model = new ColourViewModel();
               
                model.ColourList = _productProvider.GetAllColours(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.ColourList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteColour(Int64 Id = 0)
        {
            
                var current = _session.CurrentUser.RoleId;
                if (current == 1)
                {
                    _productProvider.DeleteColour(Id);
        }
            else
            {


                TempData["Message17"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllColour");
        }
        #endregion

        #region Size
        public ActionResult AddNewSize(Int64 id = 0)
        {
            try
            {

                var Cont = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Create New Size",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                SizeViewModel model = new SizeViewModel();

                if (id > 0)
                {
                    model = _productProvider.GetSize(id);
                }

                 

                return View(model);
            }
            catch(Exception e)
            { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewSize(SizeViewModel model)
        {
            try
            {
                int? status = _context.Sizes.Where(x => (x.SizeNameUS.ToLower().Trim() == model.SizeUS.ToLower().Trim() || x.SizeNameUK.ToLower().Trim() == model.SizeUK.ToLower().Trim() || x.SizeNameEU.ToLower().Trim() == model.SizeEU.ToLower().Trim()) && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {

                    _productProvider.SaveSize(model);
                    return RedirectToAction("GetAllSize");
                }
            }
            catch
            {
                return RedirectToAction("GetAllSize");
            }
        }

        public ActionResult GetAllSize()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Size",
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
        public JsonResult GetAllSize([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<SizeViewModel> list = new List<SizeViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<SizeViewModel> Report = new JsonListModel<SizeViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                SizeViewModel model = new SizeViewModel();
            

                model.SizeList = _productProvider.GetAllSizes(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.SizeList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSize(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteSize(Id);
        }
            else
            {


                TempData["Message20"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllSize");
        }
        #endregion

        #region CollectionOfYear
        public ActionResult AddNewCollectionYear(Int64 id = 0)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Create New Collection Year",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            try {
                var Current = _session.CurrentUser.Id;
            CollectionYearViewModel model = new CollectionYearViewModel();

            if (id > 0)
            {
                model = _productProvider.GetCollectionYear(id);
            }
           

            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddNewCollectionYear(CollectionYearViewModel model)
        {
            try
            {
                int? status = _context.CollectionYears.Where(x => x.Year.ToLower().Trim() == model.CollectionYear.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveCollectionYear(model);
                    return RedirectToAction("GetAllCollectionYear");
                }
            }
            catch
            {
                return RedirectToAction("GetAllCollectionYear");
            }
        }


        public ActionResult GetAllCollectionYear()
        {
            
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Collection Year",
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
        public JsonResult GetAllCollectionYear([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<CollectionYearViewModel> list = new List<CollectionYearViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<CollectionYearViewModel> Report = new JsonListModel<CollectionYearViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                CollectionYearViewModel model = new CollectionYearViewModel();
              
                model.CollectionYearList = _productProvider.GetAllCollectionYears(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.CollectionYearList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCollectionYear(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteCollectionYear(Id);
            }
            else
            {


                TempData["Message16"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllCollectionYear");
        }

        #endregion

        #region Fabric
        public ActionResult AddNewFabric(Int64 id = 0)
        {
            try {
                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Add New Fabric",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            FabricViewModel model = new FabricViewModel();

            if (id > 0)
            {
                model = _productProvider.GetFabric(id);
            }

           
            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddNewFabric(FabricViewModel model)
        {
            try
            {
                int? status = _context.Fabrics.Where(x => x.FabricName.ToLower().Trim() == model.FabricName.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveFabric(model);
                    return RedirectToAction("GetAllFabric");
                }
            }
            catch
            {
                return RedirectToAction("GetAllFabric");
            }
        }

        public ActionResult GetAllFabric()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Fabric",
                SubBreadCrumbModel = null
            };
            try {
                var curernt = _session.CurrentUser.Id;
            ViewBag.BreadCrumb = breadCrumbModel;
            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetAllFabric([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<FabricViewModel> list = new List<FabricViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<FabricViewModel> Report = new JsonListModel<FabricViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                FabricViewModel model = new FabricViewModel();
                
                model.FabricList = _productProvider.GetAllFabrics(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.FabricList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFabric(Int64 Id = 0)
        {

            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _productProvider.DeleteFabric(Id);
            }
            else
            {


                TempData["Message19"] = "You are not authorize.";
            }
            return RedirectToAction("GetAllFabric");
        }
        #endregion

        #region Tax
        public ActionResult AddNewTax(Int64 id = 0)
        {
            try
            {

                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Add New Tax",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            TaxViewModel model = new TaxViewModel();

            if (id > 0)
            {
                model = _productProvider.GetTax(id);
            }
           
            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddNewTax(TaxViewModel model)
        {
            try
            {
                int? status = _context.Taxes.Where(x => x.TaxPercentage == model.TaxPercentage && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveTax(model);
                    return RedirectToAction("GetAllTax");
                }
            }
            catch
            {
                return RedirectToAction("GetAllTax");
            }
        }


        public ActionResult GetAllTax()
        {
            try {
                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Tax",
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
        public JsonResult GetAllTax([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<TaxViewModel> list = new List<TaxViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<TaxViewModel> Report = new JsonListModel<TaxViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                TaxViewModel model = new TaxViewModel();
             
                model.TaxList = _productProvider.GetAllTaxes(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.TaxList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTax(Int64 Id = 0)
        {
            _productProvider.DeleteTax(Id);
            return RedirectToAction("GetAllTax");
        }
        #endregion

        #region CutOfDress
        public ActionResult AddNewCutOfDress(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Add New Cut Of Dress", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                CutOfDressViewModel model = new CutOfDressViewModel();
                if (id > 0)
                {
                    model = _productProvider.GetCutOfDress(id);
                }
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewCutOfDress(CutOfDressViewModel model)
        {
            try
            {
                int? status = _context.CutOfDresses.Where(x => x.CutOfDressName.ToLower().Trim() == model.CutOfDress.ToLower().Trim() && x.StatusId != 4).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1 && model.Id == 0)
                {
                    TempData["Error1"] = "Duplicate data found!";
                    return View(model);
                }
                else
                {
                    _productProvider.SaveCutOfDress(model);
                    return RedirectToAction("GetAllCutOfDresses");
                }
            }
            catch
            {
                return RedirectToAction("GetAllCutOfDresses");
            }
        }


        public ActionResult GetAllCutOfDresses()
        {

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Cut Of Dress",
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
        public JsonResult GetAllCutOfDresses([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<CutOfDressViewModel> list = new List<CutOfDressViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<CutOfDressViewModel> Report = new JsonListModel<CutOfDressViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                CutOfDressViewModel model = new CutOfDressViewModel();
             
                model.CutOfDressList = _productProvider.GetAllCutOfDresses(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                list = model.CutOfDressList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCutOfDress(Int64 Id = 0)
        {
            
                var current = _session.CurrentUser.RoleId;
                if (current == 1)
                {
                    _productProvider.DeleteCutOfDress(Id);
                }
                else
                {


                    TempData["Message18"] = "You are not authorize.";
                }
                return RedirectToAction("GetAllCutOfDresses");
        }
        #endregion

        #region : Update Inventory

        public ActionResult UpdateInventory()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Update Inventory",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
            Model.WareHouseList = _productProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });

            return View(Model);

            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult UpdateInventory(PurchaseOrderViewModel Model)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Update Inventory",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
           
                Model.WareHouseList = _productProvider.GetAllWareHouse();
          
                Model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });

            Model.SizeList = _productProvider.GetSizeModelForInventory(Model);

            return View(Model);
         
        }

        [HttpPost]
        public JsonResult SaveUpdatedInventory(string model)
        {
            try
            {
                PurchaseOrderViewModel Model = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(model);
                //decimal total = 0;
                foreach (var data in Model.ProductList)
                {
                    List<SizeViewModel> sizelist = JsonConvert.DeserializeObject<List<SizeViewModel>>(data.SizeModelString);
                    foreach (var list in sizelist)
                    {
                        data.SizeModel.Add(list);
                        
                    }
                }
                Model.CreatedById = _session.CurrentUser.Id;

                bool result = false;

               
                result = _productProvider.SaveUpdatedInventory(Model);
               

                if (result == true)
                {
                    string pNo = _context.Products.FirstOrDefault(x => x.Id == Model.ProductList[0].Product.Id).ProductName;
                    string cName = _context.Colours.FirstOrDefault(x => x.Id == Model.ProductList[0].Product.ColourId).ColourName;
                    string warehouse = (Model.WareHouseId == 1) ? "US" : ((Model.WareHouseId == 2) ? "UK" : "");
                    _MasterProvider.SaveUserLog("Manage Inventory/Stock", "Update Inventory : Id- " + Model.ProductList[0].Product.Id + ". Product Name-" + pNo + ". Color- " + cName + ". Warehouse- " + warehouse, _session.CurrentUser.Id);

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}