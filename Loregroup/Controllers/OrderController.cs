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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Loregroup.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderProvider _orderProvider;
        private readonly ICommonProvider _commonProvider;
        private readonly ISession _session;
        private readonly AppContext _context;
        private readonly IMasterProvider _MasterProvider;
        private readonly IProductProvider _productProvider;
        private readonly IUserProvider _userProvider;

        public OrderController(AppContext context, IMasterProvider masterProvider, ICommonProvider commonProvider, ISession session, IOrderProvider orderProvider, IProductProvider productProvider, IUserProvider userProvider)
        {
            _commonProvider = commonProvider;
            _session = session;
            _orderProvider = orderProvider;
            _MasterProvider = masterProvider;
            _context = context;
            _productProvider = productProvider;
            _userProvider = userProvider;
        }



        #region :CustmerOrder

        public ActionResult CustmerOrder()
        {

            try
            {
                Int64 current = _session.CurrentUser.Id;
                OrderViewModel model = new OrderViewModel();
                model.OrderlocatorList = _productProvider.GetAllOrderLocatorList();
                model.OrderlocatorList.Insert(0, new OrderLocatorViewModel()
                {
                    Id = 0,
                    OrderLocatorName = "--Select Order Locator--"
                });
                // OrderViewModel Model = new OrderViewModel();
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Order/",
                    Title = "CustmerOrder",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction( "Order","CustmerOrder");
            }

        }
            [HttpPost]
            public JsonResult CustmerOrder([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0, Int64 olId = 0)
            {
                List<OrderListDisplayViewModel> List = new List<OrderListDisplayViewModel>();
                var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
                int filterCount = 0;
            Int64 current = _session.CurrentUser.Id;
            JsonListModel<OrderViewModel> Report = new JsonListModel<OrderViewModel>
                {
                    Message = "Failed",
                    Result = false
                };
                try
                {
                    OrderListDisplayViewModel model = new OrderListDisplayViewModel();
                   model.OrderDisplayList = _orderProvider.GetAllCustmerOrder(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, current, rid, olId);

                    List = model.OrderDisplayList;
                    filterCount = List.Count;
                }
                catch (Exception ex)
                {

                }

            return Json(new DataTablesResponse(requestModel.Draw, List, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }
            #endregion
            #region: Order
            public ActionResult AddNewOrder(int Id = 0)
        {
            try
            { var Current = _session.CurrentUser.Id;
            OrderViewModel model = new OrderViewModel();

            if (Id > 0)
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Edit Order",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                model = _orderProvider.GetOrderById(Id);

                model.POmodel.WareHouseList = _orderProvider.GetAllWareHouse();
                model.POmodel.WareHouseList.Insert(0, new WareHouseViewModel()
                {
                    Id = 0,
                    WareHouseName = "--Select Ware House--"
                });
                OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Id && x.StatusId != 4).FirstOrDefault();


                var WareHouse = _context.WareHouses.Where(x => x.Id == Master.WareHouseId && x.StatusId != 4).FirstOrDefault();
                model.POmodel.WareHouseId = WareHouse.Id;
            }
            else
            {

                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Create New Order",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                try
                {
                    model.TotalOrderCount = _context.OrderMasters.Max(x => x.Id);
                }
                catch (Exception ex)
                {
                    model.TotalOrderCount = 0;
                }

                model.TotalOrderCount += 1;
                //model.OrderNo = "";
                model.OrderDate = DateTime.UtcNow.ToString("MM/dd/yyyy");

            }

            model.ColourList.Insert(0, new ColourViewModel
            {
                Id = 0,
                Colour = "--Select Color--"
            });

            model.OrderlocatorList = _productProvider.GetAllOrderLocatorList();
            model.OrderlocatorList.Insert(0, new OrderLocatorViewModel()
            {
                Id = 0,
                //OrderLocatorName = "--Select Order Locator--",
                OrderLocatorNameDesc = "--Select Order Locator--"
            });
            if (_session.CurrentUser.RoleId == 1 || _session.CurrentUser.RoleId == 2)
                model.issuperadmin = true;
            else
                model.issuperadmin = false;
            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public JsonResult GetColoursForProduct(Int64 ProductId)
        {
            ColourViewModel Model = new ColourViewModel();
            try
            {
                Model.ColourList = _orderProvider.GetColoursForProduct(ProductId);

                return Json(new SelectList(Model.ColourList, "Id", "Colour", Model.Id));
            }
            catch (Exception ex)
            {
                Model.ColourList = new List<ColourViewModel>();
                return Json(new SelectList(Model.ColourList, "Id", "Colour", Model.Id));
            }
        }

        

        [HttpPost]
        public JsonResult SaveOrder(string model)
        {
            try
            {
                OrderViewModel Model = JsonConvert.DeserializeObject<OrderViewModel>(model);
                decimal total = 0;
                foreach (var data in Model.ProductList)
                {
                    List<SizeViewModel> sizelist = JsonConvert.DeserializeObject<List<SizeViewModel>>(data.SizeModelString);
                    foreach (var list in sizelist)
                    {
                        if (list.Qty > 0)
                        {
                            data.SizeModel.Add(list);
                        }
                    }
                    total = total + data.Amount;
                }
                Model.Amount = total;
                Model.CreatedById = _session.CurrentUser.Id;

                string result = "";

                if (Model.Id > 0)
                {
                    _MasterProvider.SaveUserLog("Manage Order", "Update Order : " + Model.OrderNo, _session.CurrentUser.Id);
                    result = _orderProvider.UpdateOrder(Model);
                }
                else
                {
                    _MasterProvider.SaveUserLog("Manage Order", "Save New Order : " + Model.OrderNo, _session.CurrentUser.Id);
                    result = _orderProvider.SaveOrder(Model);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult SearchByCustomerName(string Prefix)
        {
            try
            {
                List<CustomerViewModel> customer = _context.MasterUsers.Where(x => x.UserName.Contains(Prefix) && x.StatusId != 4 && x.Id != 1).ToList()
                                .Select(y => new CustomerViewModel { UserName = y.FirstName + " " + y.LastName, Id = y.Id }).ToList();
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
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
        public JsonResult SearchByProductName(string Prefix)
       {
            try
            {
                List<ProductViewModel> Product = _context.Products.Where(x => x.ProductName.Contains(Prefix) && x.StatusId != 4).ToList()
                                                 .Select(y => new ProductViewModel { ProductName = y.ProductName, Id = y.Id, Picture1 = y.Picture1 }).ToList();
                return Json(Product, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult SearchByProductNameFrontend(string Prefix)
        {
            try
            {
                Int64 UserId = 0;
                try
                {
                    UserId = Convert.ToInt32(Session["FrontendUserId"]);
                   

                }
                catch (Exception ex)
                {
                   
                }
                List<ProductViewModel> Product = new List<ProductViewModel>();
                if(UserId>0)
                Product = _context.Products.Where(x => x.ProductName.Contains(Prefix) && x.StatusId != 4 && (x.PublishId==1 || x.PublishId==2)).ToList()
                                                 .Select(y => new ProductViewModel { ProductName = y.ProductName, Id = y.Id, Picture1 = y.Picture1 }).ToList();
                else
                Product = _context.Products.Where(x => x.ProductName.Contains(Prefix) && x.StatusId != 4 && (x.PublishId == 1)).ToList()
                                                                     .Select(y => new ProductViewModel { ProductName = y.ProductName, Id = y.Id, Picture1 = y.Picture1 }).ToList();
                return Json(Product, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult SearchByPORefrenceNo(string Prefix)
        {
            try
            {
                List<PurchaseOrderMaster> Product = _context.PurchaseOrderMasters.Where(x => x.OrderRefrence.Contains(Prefix) && x.StatusId != 4).ToList()
                                                 .Select(y => new PurchaseOrderMaster { OrderRefrence = y.OrderRefrence, Id = y.Id }).ToList();
                return Json(Product, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetProductById(Int32 number, string currency)
        {
            try
            {
                ProductViewModel Product = _productProvider.GetProduct(number);
                if (currency == "GBP")
                {
                    Product.CustomerPrice = Product.PriceGBP;
                }
                else if (currency == "EURO")
                {
                    Product.CustomerPrice = Product.PriceEURO;
                }
                else
                {
                    Product.CustomerPrice = Product.PriceUSD;
                }

                return Json(Product, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ProductViewModel model = new ProductViewModel();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductsOfPO(Int64 POId)
        {
            ProductViewModel Model = new ProductViewModel();
            try
            {
                Model.ProductList = _productProvider.GetProductsOfPO(POId);

                return Json(new SelectList(Model.ProductList, "Id", "ProductName", Model.Id));
            }
            catch (Exception ex)
            {
                Model.ProductList = new List<ProductViewModel>();
                return Json(new SelectList(Model.ProductList, "Id", "ProductName", Model.Id));
            }
        }

                   

        public ActionResult GetOrderList()
        {
            try
            {

                var current = _session.CurrentUser.Id;
            OrderViewModel model = new OrderViewModel();

            model.POmodel.WareHouseList = _orderProvider.GetAllWareHouse();
            model.POmodel.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });
            model.OrderlocatorList = _productProvider.GetAllOrderLocatorList();
            model.OrderlocatorList.Insert(0, new OrderLocatorViewModel()
            {
                Id = 0,
                OrderLocatorNameDesc = "--Select Order Locator--"
            });
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Orders",
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
        public JsonResult GetOrderList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0, Int64 whid = 0,Int64 olId=0)
        {
            List<OrderListDisplayViewModel> List = new List<OrderListDisplayViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<OrderViewModel> Report = new JsonListModel<OrderViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                OrderListDisplayViewModel model = new OrderListDisplayViewModel();
                model.OrderDisplayList = _orderProvider.GetAllOrders(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, rid, whid, olId);

                List = model.OrderDisplayList;
                filterCount = List.Count;
            }
            catch (Exception ex)
            {

            }

            return Json(new DataTablesResponse(requestModel.Draw, List, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult GetOrderListByCustomer(int Id = 0)
        {
            try
            {
                var Current = _session.CurrentUser.Id;
            OrderViewModel model = new OrderViewModel();

            model.POmodel.WareHouseList = _orderProvider.GetAllWareHouse();
            model.POmodel.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Customer Orders",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            Session["CuatomerId"] = Id;
            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetOrderListByCustomer([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0, Int64 whid = 0)
        {
            List<OrderListDisplayViewModel> List = new List<OrderListDisplayViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;
            Int64 customerid = Convert.ToInt64(Session["CuatomerId"]);
            JsonListModel<OrderViewModel> Report = new JsonListModel<OrderViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                OrderListDisplayViewModel model = new OrderListDisplayViewModel();
                model.OrderDisplayList = _orderProvider.GetAllOrdersBtCustomerId(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, rid, whid, customerid);

                List = model.OrderDisplayList;
                filterCount = List.Count;
            }
            catch (Exception ex)
            {

            }

            return Json(new DataTablesResponse(requestModel.Draw, List, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetPoDetail(string records=null)
        {
            try {
                var Current = _session.CurrentUser.Id;
            Getpodetail model = new Getpodetail();
            string[] strsplt = records.Split('-');
            string sze = strsplt[0].ToString();
            string whrhsid= strsplt[1].ToString();
            Int64 wherehouseid = Convert.ToInt64(whrhsid);
            Int64 productid=Convert.ToInt64( strsplt[2].ToString());
            Int64 colorid =Convert.ToInt64( strsplt[3].ToString());
            List<CustomerorderQuantityDetail> modellist = new List<CustomerorderQuantityDetail>();
            CustomerorderQuantityDetail modl = new CustomerorderQuantityDetail();
           
            var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
            foreach (var item in orderdetail)
            {
                modl = new CustomerorderQuantityDetail();
                var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId!=4 && k.WareHouseId== wherehouseid).FirstOrDefault();
                if (ordermastr != null)
                {
                    // var cusdetail = _context.MasterUsers.Where(k => k.Id == ordermastr.SupplierId).FirstOrDefault();
                    if (item.Qty > item.ReceivedQty)
                    {
                        modl.Customername = ordermastr.SupplierName;
                        modl.OrderNo = ordermastr.OrderRefrence;
                        modl.Quantity = item.Qty-item.ReceivedQty;
                        modellist.Add(modl);
                        model.IsDataAvailable = true;
                    }
                }
            }
            var productdetail = _context.Products.Where(k => k.Id == productid).FirstOrDefault();
            model.Styleno = productdetail.ProductName;
            var colordetail = _context.Colours.Where(k => k.Id == colorid).FirstOrDefault();
            model.ColorName = colordetail.ColourName;
            var whrehse = _context.WareHouses.Where(k => k.Id == wherehouseid).FirstOrDefault();
            if (whrehse != null)
                model.Wharehouse = whrehse.WareHouseName;
            model.Quantitylist = modellist;
            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }


        public ActionResult GetCoDetail(string records = null)
        {
            try {
                var current = _session.CurrentUser.Id;
            Getpodetail model = new Getpodetail();
            string[] strsplt = records.Split('-');
            string sze = strsplt[0].ToString();
            string whrhsid = strsplt[1].ToString();
            Int64 wherehouseid = Convert.ToInt64(whrhsid);
            Int64 productid = Convert.ToInt64(strsplt[2].ToString());
            Int64 colorid = Convert.ToInt64(strsplt[3].ToString());
            List<CustomerorderQuantityDetail> modellist = new List<CustomerorderQuantityDetail>();
            CustomerorderQuantityDetail modl = new CustomerorderQuantityDetail();

            var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
          //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
            foreach (var item in orderdetail)
            {
                modl = new CustomerorderQuantityDetail();
                var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4  && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == wherehouseid).FirstOrDefault();
               // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                if (ordermastr != null)
                {
                    if (item.Qty > item.DispatchQty)
                    {
                        var cusdetail = _context.MasterUsers.Where(k => k.Id == ordermastr.CustomerId).FirstOrDefault();
                        modl.Customername = cusdetail.ShopName;
                        modl.OrderNo = ordermastr.OrderNo;
                        modl.Quantity = item.Qty-item.DispatchQty;
                        modellist.Add(modl);
                        model.IsDataAvailable = true;
                    }
                }
            }
            var productdetail = _context.Products.Where(k => k.Id == productid).FirstOrDefault();
            model.Styleno = productdetail.ProductName;
            var colordetail = _context.Colours.Where(k => k.Id == colorid).FirstOrDefault();
            model.ColorName = colordetail.ColourName;
            var whrehse = _context.WareHouses.Where(k => k.Id == wherehouseid).FirstOrDefault();
            if (whrehse != null)
                model.Wharehouse = whrehse.WareHouseName;
            model.Quantitylist = modellist;
            return View(model);
        }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
    }
}

        public ActionResult DeleteOrder(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _orderProvider.DeleteOrder(Id, _session.CurrentUser.Id);
            }else
            {
                

               TempData["Message"] = "You are not authorize.";
            }
            return RedirectToAction("GetOrderList");
        }

        public ActionResult GetPrintPreview(Int64 Id)
        {
           
            
            OrderViewModel model = new OrderViewModel();
            model = _orderProvider.GetPrintPreview(Id);
            return View(model);
            }
           
    
         

        public JsonResult GetOrderById(Int64 Id)
        {
            try
            {
                OrderViewModel model = _orderProvider.GetOrderById(Id);
                //var distributorid=_context.MasterUsers.Where(k=>k.Id==model.CustomerModel.Id)
                foreach (var data in model.ProductList)
                {
                    Int64 size2qty = 0, size4qty = 0, size6qty = 0,
                           size8qty = 0, size10qty = 0, size12qty = 0,
                           size14qty = 0, size16qty = 0, size18qty = 0,
                           size20qty = 0, size22qty = 0, size24qty = 0,
                           size26qty = 0, size28qty = 0, size30qty = 0,
                           size32qty = 0, size34qty = 0;

                    Int64 smallTotalQty = 0;
                    Int64 largeTotalQty = 0;
                    decimal OrderPrice = 0;
                    if (model.CustomerModel.DistributionPointId == 1)
                    {
                        foreach (var list in data.SizeModel)
                        {

                            if (list.SizeUK == "2")
                            {
                                size2qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "4")
                            {
                                size4qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "6")
                            {
                                size6qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "8")
                            {
                                size8qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "10")
                            {
                                size10qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "12")
                            {
                                size12qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "14")
                            {
                                size14qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "16")
                            {
                                size16qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "18")
                            {
                                size18qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "20")
                            {
                                size20qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "22")
                            {
                                size22qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;
                            }
                            else if (list.SizeUK == "24")
                            {
                                size24qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;
                            }
                            else if (list.SizeUK == "26")
                            {
                                size26qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "28")
                            {
                                size28qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "30")
                            {
                                size30qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "32")
                            {
                                size32qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "34")
                            {
                                size34qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            OrderPrice = list.OrderPrice;
                        }
                    }
                    else if (model.CustomerModel.DistributionPointId == 2 || model.CustomerModel.DistributionPointId == 3)
                    {
                        foreach (var list in data.SizeModel)
                        {

                            if (list.SizeUK == "2")
                            {
                                size2qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "4")
                            {
                                size4qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "6")
                            {
                                size6qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "8")
                            {
                                size8qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "10")
                            {
                                size10qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "12")
                            {
                                size12qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "14")
                            {
                                size14qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "16")
                            {
                                size16qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "18")
                            {
                                size18qty = list.Qty;
                                smallTotalQty = smallTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "20")
                            {
                                size20qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "22")
                            {
                                size22qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;
                            }
                            else if (list.SizeUK == "24")
                            {
                                size24qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;
                            }
                            else if (list.SizeUK == "26")
                            {
                                size26qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "28")
                            {
                                size28qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "30")
                            {
                                size30qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "32")
                            {
                                size32qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            else if (list.SizeUK == "34")
                            {
                                size34qty = list.Qty;
                                largeTotalQty = largeTotalQty + list.Qty;

                            }
                            OrderPrice = list.OrderPrice;
                        }
                    }
                    decimal totalamount = (smallTotalQty * OrderPrice) + ((OrderPrice + calculateSurcharge(OrderPrice, 10)) * largeTotalQty);
                    
                    var List = "[{ 'SizeUK': '2', 'Qty':" + size2qty + "}," + "{ 'SizeUK': '4', 'Qty':" + size4qty + "}," + "{ 'SizeUK': '6', 'Qty':" + size6qty +
                        "}," + "{ 'SizeUK': '8', 'Qty':" + size8qty + "}," + "{ 'SizeUK': '10', 'Qty':" + size10qty + "}," + "{ 'SizeUK': '12', 'Qty':" + size12qty +
                        "}," + "{ 'SizeUK': '14', 'Qty':" + size14qty + "}," + "{ 'SizeUK': '16', 'Qty':" + size16qty + "}," + "{ 'SizeUK': '18', 'Qty':" + size18qty +
                        "}," + "{ 'SizeUK': '20', 'Qty':" + size20qty + "}," + "{ 'SizeUK': '22', 'Qty':" + size22qty + "}," + "{ 'SizeUK': '24', 'Qty':" + size24qty +
                        "}," + "{ 'SizeUK': '26', 'Qty':" + size26qty + "}," + "{ 'SizeUK': '28', 'Qty':" + size28qty + "}," + "{ 'SizeUK': '30', 'Qty':" + size30qty +
                        "}," + "{ 'SizeUK': '32', 'Qty':" + size32qty + "}," + "{ 'SizeUK': '34', 'Qty':" + size34qty + "}]";

                    data.SizeModelString = "<tr><td>" + data.Product.ProductName + "</td><td style='display:none;'>" + data.Product.Id + "</td><td>" + data.Product.ColourName + "</td><td style='display:none;'>" +
                        data.Product.ColourId + "</td><td>" + OrderPrice + "</td><td></td>"+
                        "<td "+ ((size2qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size2qty + "</td>"+
                        "<td "+ ((size4qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size4qty + "</td>"+
                        "<td "+ ((size6qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size6qty + "</td>"+
                        "<td "+ ((size8qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size8qty + "</td>"+
                        "<td "+ ((size10qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size10qty + "</td>"+
                        "<td "+ ((size12qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size12qty + "</td>"+
                        "<td "+ ((size14qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size14qty + "</td>"+
                        "<td "+ ((size16qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size16qty + "</td>"+
                        "<td "+ ((size18qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size18qty + "</td>"+
                        "<td "+ ((size20qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size20qty + "</td>"+
                        "<td "+ ((size22qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size22qty + "</td>"+
                        "<td "+ ((size24qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size24qty + "</td>"+
                        "<td "+ ((size26qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size26qty + "</td>"+
                        "<td "+ ((size28qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size28qty + "</td>"+
                        "<td "+ ((size30qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size30qty + "</td>"+
                        "<td "+ ((size32qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size32qty + "</td>"+
                        "<td "+ ((size34qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size34qty + "</td>"+
                        "<td>" + totalamount + "</td><td style='display:none;'>" + List +
                        "</td><td><a data-itemId='0' href='#' class='deleteItem'>Remove</a></td><td><a data-itemId='0' href='#' class='editItem'>Edit</a></td></tr>";
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                OrderViewModel model = new OrderViewModel();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        private decimal calculateSurcharge(decimal productPrice, decimal surchargeValue)
        {
            return Math.Round(((productPrice * surchargeValue) / 100),2);
        }
        
        public ActionResult DispatchOrder(Int64 Id = 0)
        {
            try
            {
                var Current = _session.CurrentUser.Id;
            OrderViewModel Model = new OrderViewModel();
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Dispatch Order",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            try
            {
                Model.Id = Id;

                var Ordermaster = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                Model.DistributionPoint = Ordermaster.WareHouseId;
                if (Ordermaster.OrderStatusId > 0)
                {
                    Model.OrderStatusId = (OrderStatus)Ordermaster.OrderStatusId;
                }

                Model.DetailsList = _orderProvider.GetDetailsListForDispatch(Id);

                return View(Model);
            }
            catch (Exception ex)
            {
                return View(Model);
            }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult DispatchOrder(OrderViewModel Model)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Dispatch Order",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            try
            {
                Model.CreatedById = _session.CurrentUser.Id;

                bool result =  _orderProvider.DispatchOrderDetails(Model);
                if (result == true)
                {
                    TempData["POreceive"] = "Order Quantity Dispatched.";
                }
                else
                {
                    TempData["POreceive1"] = "Some Error Occured! please try later.";
                }

                var Ordermaster = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                if (Ordermaster.OrderStatusId > 0)
                {
                    Model.OrderStatusId = (OrderStatus)Ordermaster.OrderStatusId;
                }

                Model.DetailsList = _orderProvider.GetDetailsListForDispatch(Model.Id);

               // return View(Model);
                return RedirectToAction("DispatchOrder", new { Id = Model.Id });
            }
            catch
            {
                //return View(new OrderViewModel());
                return RedirectToAction("DispatchOrder", new { Id = Model.Id });
            }
        }

      

        [HttpPost]
        public JsonResult UpdateDispatchOrder(string model)
        {
            try
            {
                OrderViewModel Model = JsonConvert.DeserializeObject<OrderViewModel>(model);
                //decimal total = 0;
                foreach (var data in Model.ProductList)
                {
                    List<SizeViewModel> sizelist = JsonConvert.DeserializeObject<List<SizeViewModel>>(data.SizeModelString);
                    foreach (var list in sizelist)
                    {
                        if (list.DispatchQty > 0)
                        {
                            data.SizeModel.Add(list);
                        }
                    }
                    //total = total + data.Amount;
                }
                //Model.Amount = total;
                Model.CreatedById = _session.CurrentUser.Id;

                bool result = false;

                if (Model.Id > 0)
                {
                    result = _orderProvider.DispatchOrder(Model);
                }
                else
                {
                }

                if (result == true)
                {
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

        #region : Purchase Order

        public ActionResult AddNewPurchaseOrder(int Id = 0)
        {
            try{

                var Current = _session.CurrentUser.Id;
            PurchaseOrderViewModel Model = new PurchaseOrderViewModel();

            if (Id > 0)
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Edit Purchase Order",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                Model = _orderProvider.GetPurchaseOrderById(Id);
            }
            else
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Create New Purchase Order",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                Model.OrderDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
            }

            Int64 roleId = _session.CurrentUser.RoleId;
            if (roleId == 1 || roleId == 2)
                ViewBag.ShowSendMailButton = true;
            else
                ViewBag.ShowSendMailButton = false;

            //Model.ColourList = _productProvider.GetAllColourList();
            Model.ColourList.Insert(0, new ColourViewModel
            {
                Id = 0,
                Colour = "--Select Color--"
            });

            Model.WareHouseList = _orderProvider.GetAllWareHouse();
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
        public JsonResult SearchByCustomerNameForPO(string Prefix)
        {
            try
            {
                string rname = "supplier";
                Int64 rid = _context.Roles.FirstOrDefault(x => x.StatusId == 1 && x.Name.ToLower().Contains(rname)).Id;

                List<CustomerViewModel> customer1 = _context.MasterUsers.Where(x => x.UserName.Contains(Prefix) && x.StatusId != 4 && x.RoleId == rid).ToList()
                                .Select(y => new CustomerViewModel { UserName = y.FirstName + " " + y.LastName, Id = y.Id }).ToList();
                List<CustomerViewModel> customer = customer1.OrderBy(k => k.UserName).ToList();
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SavePurchaseOrder(string model)
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
                        if (list.Qty > 0)
                        {
                            data.SizeModel.Add(list);
                        }
                    }
                    //total = total + data.Amount;
                }
                //Model.Amount = total;
                Model.CreatedById = _session.CurrentUser.Id;

                bool result = false;
                string messageValue = "";
                if (Model.Id > 0)
                {
                   
                    if (Model.Id > 0)
                    {
                        _MasterProvider.SaveUserLog("Manage Purchase Order", "Save New Purchase Order : " + Model.Id + ". Order Reference- " + Model.OrderRefrence, _session.CurrentUser.Id);
                        result = _orderProvider.UpdatePurchaseOrder(Model);
                    }
                }
                else
                {
                    var poRecord = _context.PurchaseOrderMasters.FirstOrDefault(x => x.OrderRefrence.ToLower() == Model.OrderRefrence.ToLower() && x.StatusId != 4);
                    if (poRecord == null)
                    {
                        _MasterProvider.SaveUserLog("Manage Purchase Order", "Update Purchase Order : " + Model.Id + ". Order Reference- " + Model.OrderRefrence, _session.CurrentUser.Id);
                        result = _orderProvider.SavePurchaseOrder(Model);
                    }
                    else
                    {
                        result = false;
                        messageValue = "Purchase order number is already exists. please use other code.";
                    }
                }

                if (result == true)
                {                    
                    return Json(new { error = true, message = "Purchase order save successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (string.IsNullOrEmpty(messageValue))
                        messageValue = "Server error, Please contact Admin";
                    return Json(new { error = false, message = messageValue }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return null;
        }

        //Save Purchase Order From Order Module
        [HttpPost]
        public JsonResult SavePurchaseOrderFromOrderModule(string model)
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
                        if (list.Qty > 0)
                        {
                            data.SizeModel.Add(list);
                        }
                    }
                    //total = total + data.Amount;
                }
                //Model.Amount = total;
                Model.CreatedById = _session.CurrentUser.Id;

                bool result = false;

                Int64 Id = _context.PurchaseOrderMasters.Where(x => x.OrderRefrence == Model.OrderRefrence && x.StatusId != 4).Select(x => x.Id).FirstOrDefault();
                if (Id > 0)
                {
                    Model.Id = Id;
                }

                Model.InsertFrom = "Order";
                if (Model.Id > 0)
                {
                    result = _orderProvider.UpdatePurchaseOrder(Model);
                }
                else
                {
                    result = _orderProvider.SavePurchaseOrder(Model);
                }

                if (result == true)
                {
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
            //return null;
        }

        public ActionResult GetPurchaseOrderList()
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Purchase Orders",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            try
            {

                var current = _session.CurrentUser.Id;

            PurchaseOrderViewModel model = new PurchaseOrderViewModel();
            try
            {
                string rname = "supplier";
                Int64 rid = _context.Roles.FirstOrDefault(x => x.StatusId == 1 && x.Name.ToLower().Contains(rname)).Id;

                List<CustomerViewModel> customer1 = _context.MasterUsers.Where(x => x.StatusId != 4 && x.RoleId == rid).ToList()
                                .Select(y => new CustomerViewModel { UserName = y.FirstName + " " + y.LastName, Id = y.Id }).ToList();
                    List<CustomerViewModel> customer = customer1.OrderBy(k => k.UserName).ToList();
                customer.Insert(0, new CustomerViewModel { Id = 0, UserName = "--Select--" });
                model.CustomerList = customer;
            }
            catch (Exception ex)
            {
                return null;
            }
            model.WareHouseList = _orderProvider.GetAllWareHouse();
            model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });

            return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetPurchaseOrderList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0, Int64 whid = 0, Int64 cusid = 0)
        
        {
            List<OrderListDisplayViewModel> List = new List<OrderListDisplayViewModel>();
            var totalCount = 1;
            int filterCount = 0;

            JsonListModel<OrderViewModel> Report = new JsonListModel<OrderViewModel>
            {
                Message = "Failed",
                Result = false
            };

            try
            {
                OrderListDisplayViewModel model = new OrderListDisplayViewModel();
                model.OrderDisplayList = _orderProvider.GetAllPurchaseOrders(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount, rid, cusid,whid);

                List = model.OrderDisplayList;
                filterCount = List.Count;
            }
            catch (Exception ex)
            {
            }

            return Json(new DataTablesResponse(requestModel.Draw, List, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyPurchaseOrders()

        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Purchase Orders",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                PurchaseOrderViewModel model = new PurchaseOrderViewModel();
                try
                {
                    if (_session.CurrentUser.Id == 161814)
                    {
                        model.IsAdmin = true;
                    }
                    else
                    {
                        model.IsAdmin = false;
                    }

                    string rname = "supplier";
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
                model.WareHouseList = _orderProvider.GetAllWareHouse();
                model.WareHouseList.Insert(0, new WareHouseViewModel()
                {
                    Id = 0,
                    WareHouseName = "--Select Ware House--"
                });
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult GetSupplierPurchaseOrderList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Int64 rid = 0, Int64 cusid = 0, Int64 whid = 0)
        {
            List<OrderListDisplayViewModel> List = new List<OrderListDisplayViewModel>();
            var totalCount = 0;
            int filterCount = 0;
            var CurrentUserId = _session.CurrentUser.Id;

            try
            {
                IQueryable<PurchaseOrderMaster> query = _context.PurchaseOrderMasters.Where(x => x.SupplierId == CurrentUserId && x.StatusId != 4).OrderByDescending(x => x.Id);
              if(CurrentUserId== 161814)
                {
                    query= _context.PurchaseOrderMasters.Where(x=> x.StatusId != 4).OrderByDescending(x => x.Id);
                }
                totalCount = query.Count();

                //Int64 loginRoleId = _context.MasterUsers.FirstOrDefault(x => x.Id == CurrentUserId).RoleId;
                if (rid == 0)
                {
                    query = query.Where(x => x.POStatusId != (int)PurchaseOrderStatus.Completed);
                }
                else
                {
                    query = query.Where(x => x.POStatusId == rid);
                }
                if (cusid != 0)
                {
                    query = query.Where(x => x.SupplierId == cusid);
                }

                if (whid != 0)
                {
                    query = query.Where(x => x.WareHouseId == whid);
                }
                if (requestModel.Search.Value != String.Empty)
                {
                    var value = requestModel.Search.Value.Trim();
                    query = query.Where(p => p.SupplierName.Contains(value) || p.OrderRefrence.Contains(value));
                }
                filterCount = query.Count();

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);
                List = query.ToList().Select(ToPurchaseOrderListDisplayViewModel).ToList();


            }
            catch (Exception ex)
            {
            }
            return Json(new DataTablesResponse(requestModel.Draw, List, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public OrderListDisplayViewModel ToPurchaseOrderListDisplayViewModel(PurchaseOrderMaster POMaster)
        {
            var color = "";
            if ((int)POMaster.POStatusId == 1)
            {
                color = "#3c8dbc"; //status new, color blue
            }
            else if ((int)POMaster.POStatusId == 2)
            {
                color = "#C98209"; //status In Progress, color Orange
            }
            else if ((int)POMaster.POStatusId == 3)
            {
                color = "#9709c9"; //status In Payment Failed, color purple
            }
            else if ((int)POMaster.POStatusId == 4)
            {
                color = "#09c977"; //status Completed, color Green
            }
            else if ((int)POMaster.POStatusId == 5)
            {
                color = "#ff0000"; //status Cancelled, color red
            }
            else if ((int)POMaster.POStatusId == 6)
            {
                color = "#27AE5D"; //status Confirm, color Medium Green
            }
            else if ((int)POMaster.POStatusId == 7)
            {
                color = "#795C32"; //status ChinaWarehouse, color Brown
            }
            else if ((int)POMaster.POStatusId == 8)
            {
                color = "#FF3F3F"; //status Ready, color Orange Red
            }

            string WareHouse = "", shopName = "";
            if (POMaster.WareHouseId > 0)
            {
                WareHouse = _context.WareHouses.FirstOrDefault(x => x.Id == POMaster.WareHouseId).WareHouseName;
            }

            try
            {
                var orderDetails = _context.OrderMasters.Join(_context.MasterUsers, s => s.CustomerId, d => d.Id, (s, d) => new { s, d })
                                   .Where(x => x.s.OrderNo.ToLower() == POMaster.OrderRefrence.ToLower()).FirstOrDefault();
                if (orderDetails != null)
                {
                    shopName = orderDetails.d.ShopName;
                }
            }
            catch (Exception ex)
            { }

            return new OrderListDisplayViewModel()
            {
                CustomerFullName = POMaster.SupplierName,
                CurrencyName = POMaster.CurrencyName,
                MobileNo = POMaster.MobileNo,
                WareHouseName = WareHouse,
                ShopName = shopName,
                OrderRefrence = POMaster.OrderRefrence,
                //ZipCode = POMaster.ZipCode,
                EmailId = POMaster.EmailId,
                //OrderNo = POMaster.OrderNo,
                OrderDate = POMaster.OrderDate.ToShortDateString(),
                DeliveryDate = POMaster.DeliveryDate,
                //Amount = POMaster.Amount,
                TotalAmount = POMaster.TotalAmount,
                TotalProducts = POMaster.TotalProduct,
                Receive = "<a href='/Order/ReceivePOCounts?Id=" + POMaster.Id + "'title='Receive'>Receive</a>",
                OrderStatus = "<span style='color:white;background: " + color + "; font-weight:bold;border-radius: 4px;padding: 3px;'>" + (PurchaseOrderStatus)POMaster.POStatusId + "</span>",
                Edit = "<a href='/Order/AddNewPurchaseOrder?id=" + POMaster.Id + "' title='Edit'><img src='/Content/img/editicon.png'style='height: 15px;'/></a>",
                Delete = "<a href='/Order/DeletePurchaseOrder?id=" + POMaster.Id + "' title='Delete' onclick='return Confirmation();'><img src='/Content/img/deleteicon.png'style='height: 15px;'/></a>",
                PrintPreview = "<a href='/Order/GetPurchaseOrderPrintPreview?id=" + POMaster.Id + "' title='Print Preview'  target='_blank' ><img src='/Content/img/PrintPreview.png'style='height: 15px;'/></a>",
                ChangeStatus = ((int)POMaster.POStatusId == 6) ? "<a title='Ready To Ship' onclick='changeStatus(" + POMaster.Id + ")'>Ready To Ship</a>" : "",
            };
        }

        public ActionResult DeletePurchaseOrder(Int64 Id = 0)
        {
            var current = _session.CurrentUser.RoleId;
            if (current == 1)
            {
                _orderProvider.DeletePurchaseOrder(Id, _session.CurrentUser.Id);
            }
            else
            {


                TempData["Message12"] = "You are not authorize.";
            }
            return RedirectToAction("GetPurchaseOrderList");
        }

        public ActionResult GetPurchaseOrderPrintPreview(Int64 Id)
        { 
            try
            {
                var Current = _session.CurrentUser.Id;
                PurchaseOrderViewModel model = new PurchaseOrderViewModel();
                model = _orderProvider.GetPOPrintPreview(Id);
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        public JsonResult GetPurchaseOrderById(Int64 Id)
        {
            try
            {
                PurchaseOrderViewModel model = _orderProvider.GetPurchaseOrderById(Id);
                foreach (var data in model.ProductList)
                {
                    Int64 size2qty = 0, size4qty = 0, size6qty = 0,
                           size8qty = 0, size10qty = 0, size12qty = 0,
                           size14qty = 0, size16qty = 0, size18qty = 0,
                           size20qty = 0, size22qty = 0, size24qty = 0,
                           size26qty = 0, size28qty = 0, size30qty = 0,
                           size32qty = 0, size34qty = 0;

                    Int64 TotalQty = 0;
                    //decimal OrderPrice = 0;

                    foreach (var list in data.SizeModel)
                    {
                        if (list.SizeUK == "2")
                        {
                            size2qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "4")
                        {
                            size4qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "6")
                        {
                            size6qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "8")
                        {
                            size8qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "10")
                        {
                            size10qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "12")
                        {
                            size12qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "14")
                        {
                            size14qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "16")
                        {
                            size16qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "18")
                        {
                            size18qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "20")
                        {
                            size20qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "22")
                        {
                            size22qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "24")
                        {
                            size24qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "26")
                        {
                            size26qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "28")
                        {
                            size28qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "30")
                        {
                            size30qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "32")
                        {
                            size32qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }
                        else if (list.SizeUK == "34")
                        {
                            size34qty = list.Qty;
                            TotalQty = TotalQty + list.Qty;

                        }


                        //OrderPrice = list.OrderPrice;
                    }

                    //decimal totalamount = TotalQty * OrderPrice;

                    var List = "[{ 'SizeUK': '2', 'Qty':" + size2qty + "}," + "{ 'SizeUK': '4', 'Qty':" + size4qty + "}," + "{ 'SizeUK': '6', 'Qty':" + size6qty +
          "}," + "{ 'SizeUK': '8', 'Qty':" + size8qty + "}," + "{ 'SizeUK': '10', 'Qty':" + size10qty + "}," + "{ 'SizeUK': '12', 'Qty':" + size12qty +
          "}," + "{ 'SizeUK': '14', 'Qty':" + size14qty + "}," + "{ 'SizeUK': '16', 'Qty':" + size16qty + "}," + "{ 'SizeUK': '18', 'Qty':" + size18qty +
          "}," + "{ 'SizeUK': '20', 'Qty':" + size20qty + "}," + "{ 'SizeUK': '22', 'Qty':" + size22qty + "}," + "{ 'SizeUK': '24', 'Qty':" + size24qty +
          "}," + "{ 'SizeUK': '26', 'Qty':" + size26qty + "}," + "{ 'SizeUK': '28', 'Qty':" + size28qty + "}," + "{ 'SizeUK': '30', 'Qty':" + size30qty +
          "}," + "{ 'SizeUK': '32', 'Qty':" + size32qty + "}," + "{ 'SizeUK': '34', 'Qty':" + size34qty + "}]";

                    data.SizeModelString = "<tr><td>" + data.Product.ProductName + "</td><td style='display:none;'>" + data.Product.Id + "</td><td>" + data.Product.ColourName + "</td><td style='display:none;'>" +
                        data.Product.ColourId + "</td><td></td>"+
                         "<td "+ ((size2qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size2qty + "</td>"+
                         "<td "+ ((size4qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size4qty + "</td>"+
                         "<td "+ ((size6qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size6qty + "</td>"+
                        "<td "+ ((size8qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size8qty + "</td>" +
                        "<td "+ ((size10qty > 0) ? "style='color:red;font-weight:bold;'" :"") +">" + size10qty + "</td>"+
                        "<td "+ ((size12qty > 0) ? "style='color:red;font-weight:bold;'" :"") +">" + size12qty + "</td>"+
                        "<td "+ ((size14qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size14qty + "</td>"+
                        "<td "+ ((size16qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size16qty + "</td>"+
                        "<td "+ ((size18qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size18qty + "</td>"+
                        "<td "+ ((size20qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size20qty + "</td>"+
                        "<td "+ ((size22qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size22qty + "</td>"+
                        "<td "+ ((size24qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size24qty + "</td>"+
                        "<td "+ ((size26qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size26qty + "</td>"+
                        "<td "+ ((size28qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size28qty + "</td>"+
                        "<td "+ ((size30qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size30qty + "</td>"+
                        "<td "+ ((size32qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size32qty + "</td>"+
                        "<td "+ ((size34qty > 0) ? "style='color:red;font-weight:bold;'" : "") +">" + size34qty + "</td>"+
                        "<td style='display:none;'>" + List +
                        "</td><td><a data-itemId='0' href='#' class='deleteItem'>Remove</a></td><td><a data-itemId='0' href='#' class='editItem'>Edit</a></td></tr>";
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                OrderViewModel model = new OrderViewModel();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReceivePOCounts(Int64 Id = 0)
        {
            try {
                var Current = _session.CurrentUser.Id;
            PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
            try
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Receive Purchase Order",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                Model.Id = Id;

                var pomaster = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                Model.DistributionPoint = pomaster.WareHouseId;
                if (pomaster.POStatusId > 0)
                {
                    Model.PurchaseOrderStatusId = (PurchaseOrderStatus)pomaster.POStatusId;
                }

                Model.DetailsList = _orderProvider.GetDetailsListForReceivePO(Id);
                return View(Model);
            }
            catch (Exception ex)
            {
                return View(Model);
            }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult ReceivePOCounts(PurchaseOrderViewModel Model)
        {
            try
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Receive Purchase Order",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                Model.CreatedById = _session.CurrentUser.Id;

                bool result = _orderProvider.ReceivePODetails(Model);
                if (result == true)
                {
                    TempData["POreceive"] = "PO Quantity received.";
                }
                else
                {
                    TempData["POreceive"] = "Some Error Occured! please try later.";
                }

                var pomaster = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                if (pomaster.POStatusId > 0)
                {
                    Model.PurchaseOrderStatusId = (PurchaseOrderStatus)pomaster.POStatusId;
                }

                Model.DetailsList = _orderProvider.GetDetailsListForReceivePO(Model.Id);

                //return View(Model);
                return RedirectToAction("ReceivePOCounts", new { Id = Model.Id });
            }
            catch (Exception ex)
            {
                //return View(Model);
                return RedirectToAction("ReceivePOCounts", new { Id = Model.Id });
            }
        }

        #region : receive po old
        public ActionResult ReceivePO(Int64 Id = 0)
        { 
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Receive Purchase Order", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
                Model.Id = Id;
                Model.ProductModel.ProductList = _productProvider.GetAllProductsForReceivePO(Model.Id);
                Model.ProductModel.ProductList.Insert(0, new ProductViewModel() { Id = 0, ProductName = "--Select Product--" });
                return View(Model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult ReceivePO(PurchaseOrderViewModel Model)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Receive Purchase Order",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            try
            {
                List<SizeViewModel> sizemodel = _orderProvider.GetSizeModelForReceivePO(Model);
                Model.ProductListModel.SizeModel = sizemodel;

                var pomaster = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                if (pomaster.POStatusId > 0)
                {
                    Model.PurchaseOrderStatusId = (PurchaseOrderStatus)pomaster.POStatusId;
                }

                Model.ProductModel.ProductList = _productProvider.GetAllProductsForReceivePO(Model.Id);
                Model.ProductModel.ProductList.Insert(0, new ProductViewModel()
                {
                    Id = 0,
                    ProductName = "--Select Product--"
                });

                return View(Model);
            }
            catch
            {
                return View(new PurchaseOrderViewModel());
            }
        }
        #endregion

        [HttpPost]
        public JsonResult UpdateReceivePO(string model)
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
                        if (list.ReceivedQty > 0)
                        {
                            data.SizeModel.Add(list);
                        }
                    }
                    //total = total + data.Amount;
                }
                //Model.Amount = total;
                Model.CreatedById = _session.CurrentUser.Id;

                bool result = false;

                if (Model.Id > 0)
                {
                    result = _orderProvider.ReceivePO(Model);
                }
                else
                {
                }

                if (result == true)
                {
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

        public ActionResult ConfirmPO(string Id)
        {
            try
            {
                var POMasterId = Id.Split('#');
                Int64 POId = Convert.ToInt64(POMasterId[0]);

                PurchaseOrderMaster POMaster = _context.PurchaseOrderMasters.Where(x => x.Id == POId && (x.POStatusId == 1 || x.POStatusId == 2) && x.StatusId != 4).FirstOrDefault();
                if (POMaster != null)
                {
                    POMaster.POStatusId = 6; //PO Confirm

                    //Update Order Status
                    try
                    {
                        //Insert into Stock-Quantity Table
                        List<PurchaseOrderDetail> PoDetailList = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == POMaster.Id && x.StatusId != 4).ToList();
                        foreach (var item in PoDetailList)
                        {
                            StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == item.SizeUK && x.ColourId == item.ColourId && x.ProductId == item.ProductId && x.WareHouseId == POMaster.WareHouseId && x.StatusId != 4).FirstOrDefault();
                            if (StockDetail != null)
                            {
                                Int64 localQty = 0;
                                if (item.IsInventoryAdded != true)
                                {
                                    localQty = item.Qty;
                                }

                               
                                StockDetail.Qty = StockDetail.Qty + localQty;
                                //StockDetail.ModifiedById = _context;
                                StockDetail.ModificationDate = DateTime.UtcNow;
                                //StockDetail.WareHouseId = Model.WareHouseId;
                                item.IsInventoryAdded = true;
                            }
                            else
                            {
                                StockQuantity sq = new StockQuantity()
                                {
                                    ReferenceId = 0,
                                    InventoryType = "PO",
                                    ProductId = item.ProductId,
                                    SizeUK = item.SizeUK,
                                    ColourId = item.ColourId.Value,
                                    Qty = item.Qty,
                                    ReceivedQty = 0,
                                    DispatchedQty = 0,
                                    WareHouseId = POMaster.WareHouseId
                                };
                                _context.StockQuantities.Add(sq);
                                item.IsInventoryAdded = true;

                            }
                        }
                        _context.SaveChanges();

                        OrderViewModel OrderModel = new OrderViewModel();
                        OrderMaster OrderMaster = _context.OrderMasters.Where(x => x.OrderNo == POMaster.OrderRefrence && x.StatusId != 4).FirstOrDefault();
                        if (OrderMaster != null)
                        {
                            OrderMaster.OrderStatusId = 2;
                            _context.SaveChanges();

                            OrderModel = _orderProvider.GetOrderByOrderNo(OrderMaster.OrderNo);
                           
                            _orderProvider.SendInprogressMailToCustomer(OrderModel);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    TempData["ResponseMsg"] = "Thank You For Confirmation";
                }
                else
                {
                    TempData["ResponseMsg"] = "No Record found or It is already Confirmed.";
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["ResponseMsg"] = "Some Error Occured. Please try later.";
                return View();
            }
        }

        [HttpPost]
        public JsonResult sendPoMail(Int64 poId)
        {
            try
            {
                PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
                if (poId > 0)
                {
                    Model = _orderProvider.GetPurchaseOrderById(poId);
                  //  Model.CustomerModel.EmailId = "sulabh.saxena@connekt.in";
                    _orderProvider.SendMail(Model);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
                
        [HttpPost]
        public JsonResult ChangePOStatus(Int64 POMasterId, int poStatusId)
        {
            PurchaseOrderViewModel boss = new PurchaseOrderViewModel();
            try
            { 

                
            PurchaseOrderDetailViewModel obj = new PurchaseOrderDetailViewModel();
                if (POMasterId > 0)
                {
                    var PO = _context.PurchaseOrderMasters.FirstOrDefault(x => x.Id == POMasterId);
                    List<ProductListModel> ProductDetailsList = _orderProvider.GetPOProductDetailList(POMasterId);
                    


                    if (PO != null)
                    {
                        PO.POStatusId = poStatusId;
                        _context.SaveChanges();
                    }

                    PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
                    MasterUser user = _context.MasterUsers.Where(x => x.Id == PO.SupplierId).FirstOrDefault();
                    Model.CustomerModel.EmailId = PO.EmailId;
                    Model.OrderRefrence = PO.OrderRefrence;
                    Model.CustomerModel.AddressLine1 = user.AddressLine1;
                    Model.CustomerModel.CurrencyName = ((Currency)user.CurrencyId).ToString();
                    Model.CustomerModel.Country = _context.Countries.Where(x => x.Id == user.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                    Model.SupplierName = user.FirstName + " " + user.LastName;
                     Model.WareHouseName = _context.WareHouses.FirstOrDefault(x => x.Id == PO.WareHouseId).WareHouseName;
                    Model.OrderDate = PO.OrderDate.ToShortDateString();
                    Model.ProductList = ProductDetailsList;
                    if (poStatusId == 8 || poStatusId == 6)
                    {
                        _orderProvider.SendMailToReadyShip(Model);
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region : Transfer PO

        public ActionResult TransferPO()
        {
           
            TransferPOViewModel Model = new TransferPOViewModel();
            try
            {
                var current = _session.CurrentUser.Id;

                try
            {
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Transfer Product",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
               
                    Model.FromSizeList.Add(new SizeViewModel()
                    {
                        Id = 0,
                        SizeUK = "-- Select Size--"
                    });

                    for (int i = 2; i <= 34; i += 2)
                    {
                        //Int64 k = 0;
                        var SizeUS = "";
                        var SizeEU = (i + 28).ToString();
                        var SizeUK = i.ToString();
                        if ((i - 4) > 0)
                        {
                            SizeUS = (i - 4).ToString();
                        }
                        else if ((i - 4) == 0)
                        {
                            SizeUS = "0";
                        }
                        else
                        {
                            SizeUS = "00";
                        }
                        Model.FromSizeList.Add(new SizeViewModel()
                        {
                            Id = i,
                            SizeUK = SizeUS + "/" + SizeUK + "/" + SizeEU
                        });
                        //k += 1;
                    }

                    Model.WareHouseList = _productProvider.GetAllWareHouse();
                    Model.WareHouseList.Insert(0, new WareHouseViewModel()
                    {
                        Id = 0,
                        WareHouseName = "--Select Ware House--"
                    });

                }
                catch (Exception ex)
                {
                    Model.WareHouseList = _productProvider.GetAllWareHouse();
                    Model.WareHouseList.Insert(0, new WareHouseViewModel()
                    {
                        Id = 0,
                        WareHouseName = "--Select Ware House--"
                    });
                }
                return View(Model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult TransferPO(TransferPOViewModel model)
        {
            //TransferPOViewModel Model = new TransferPOViewModel();
            try
            {
                model.CreatedById = _session.CurrentUser.Id;

                #region: oldcode
               
                #endregion

                string ToSize = model.ToSizeId.ToString();
                string FromSize = model.FromSizeId.ToString();

                if (model.FromProductModel.Id != 0 && model.FromColorId != 0 && model.FromSizeId != 0 && model.WareHouseId != 0)
                {
                    StockQuantity SqFrom = _context.StockQuantities.FirstOrDefault(x => x.InventoryType == "Stock" && x.SizeUK == FromSize && x.ProductId == model.FromProductModel.Id && x.ColourId == model.FromColorId && x.WareHouseId == model.WareHouseId && x.StatusId != 4);

                    if (SqFrom != null)
                    {
                        SqFrom.ReceivedQty = SqFrom.ReceivedQty - model.TransferQty;
                        SqFrom.ModificationDate = DateTime.UtcNow;
                    }
                    else
                    {
                        StockQuantity SQ = new StockQuantity();
                        SQ.InventoryType = "Stock";
                        SQ.SizeUK = FromSize;
                        SQ.ProductId = model.FromProductModel.Id;
                        SQ.ColourId = model.FromColorId;
                        SQ.WareHouseId = model.WareHouseId;
                        SQ.ReceivedQty = 0;
                        //SQ.DispatchedQty = model.TransferQty;
                        SQ.CreatedById = model.CreatedById;
                        SQ.CreationDate = DateTime.UtcNow;
                        _context.StockQuantities.Add(SQ);
                    }
                    _context.SaveChanges();
                }
                #region: oldcode 05-11-22
              
                #endregion

                if (model.ToWareHouseId != 0)
                {
                    StockQuantity SqTo = _context.StockQuantities.FirstOrDefault(x => x.InventoryType == "Stock" && x.SizeUK == FromSize && x.ProductId == model.FromProductModel.Id && x.ColourId == model.FromColorId && x.WareHouseId == model.ToWareHouseId && x.StatusId != 4);

                    if (SqTo != null)
                    {
                        SqTo.ReceivedQty = SqTo.ReceivedQty + model.TransferQty;
                        SqTo.ModificationDate = DateTime.UtcNow;
                    }
                    else
                    {
                      

                        StockQuantity SQ = new StockQuantity();
                        SQ.InventoryType = "Stock";
                        SQ.SizeUK = FromSize;
                        SQ.ProductId = model.FromProductModel.Id;
                        SQ.ColourId = model.FromColorId;
                        SQ.WareHouseId = model.ToWareHouseId;
                        SQ.ReceivedQty = model.TransferQty;
                        //SQ.DispatchedQty = model.TransferQty;
                        SQ.CreatedById = model.CreatedById;
                        SQ.CreationDate = DateTime.UtcNow;
                        _context.StockQuantities.Add(SQ);
                    }
                    _context.SaveChanges();
                }


                TempData["TransferResult"] = "Quantity transferred successfully";
            }
            catch (Exception ex)
            {
                TempData["TransferResult"] = ex.Message;
            }
            return RedirectToAction("TransferPO");
        }

        public JsonResult GetOrderDetails(Int64 WhId, Int64 ProductId, Int64 ProductColorId, Int64 ProductSize)
        {
            //StockQuantity FromProduct = new StockQuantity();
            string size = ProductSize.ToString();
            var FromProduct = _context.StockQuantities.Where(x => x.ProductId == ProductId && x.InventoryType =="Stock" && x.ColourId == ProductColorId && x.SizeUK == size && x.WareHouseId == WhId && x.StatusId != 4)
                            .GroupBy(y => y.ProductId).Select(y => new
                            {
                                ReceivedQty = y.Sum(z => z.ReceivedQty),
                                //DispatchedQty = y.Sum(z => z.DispatchedQty)
                            }).ToList();

            Int64 AvailableQty = FromProduct[0].ReceivedQty;    // -FromProduct[0].DispatchedQty;

            if (FromProduct != null)
            {
                return Json(AvailableQty, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region : Inventory

        public ActionResult ManageInventory()
       {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Inventory",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                PurchaseOrderViewModel Model = new PurchaseOrderViewModel();

                Model.WareHouseList = _orderProvider.GetAllWareHouse();
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
        public ActionResult ManageInventory(PurchaseOrderViewModel Model)
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Inventory",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;

            List<SizeViewModel> sizemodel = _orderProvider.GetSizeModelForInventory(Model);

            Model.ProductListModel.SizeModel = sizemodel;
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });

            return View(Model);
        }

        #endregion

        public void sendhtmlmail()
        {

            string data = "<!DOCTYPE html>" +
                            "<html>" +
                            "<head>" +
                            "    <title>Order</title>" +
                            "    <meta http-equiv='content-type' content='text/html; charset=UTF-8' />" +
                            "	<meta name='viewport' content='width=device-width, initial-scale=1'>" +

                            "	<style>	" +

                            "table {" +
                            "    border-collapse: collapse;" +
                            "    border-spacing: 0;        " +
                            "}" +

                            "td {vertical-align:top;}" +

                            "th, td {" +
                            "    text-align: left;	" +
                            "}" +

                            "p {margin:0 0 10px;}" +
                            ".order-form-text{font-size:16px;}" +

                            ".logo-header {width: 900px; padding:5px; border: solid 1px #000000; vertical-align: top;}" +
                            ".logo-header img {width:130px; margin-right:10px;}" +

                            ".company-name{vertical-align: top; font-family: Times New Roman; font-size: 18pt; font-weight:bold; padding-bottom: 15px;}" +

                            ".address-block{font-size: 12px; width:250px;}" +

                            ".order-title {vertical-align: top; font-size: 20px; font-weight: 600;}" +

                            ".padded-border{border: solid 1px #000000; padding: 10px 10px 10px 10px; vertical-align: top;}" +

                            ".client-details {width: 500px; display: table;  border-collapse: separate; border-spacing: 2px; border-color: grey;}" +

                            ".detail {font-size: 12pt; border-bottom: solid 1px #000000; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;}" +
                            ".line {border-bottom:none}" +
                            ".header {font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;}" +
                            ".border {border: 1px solid black;}" +

                            ".product td th {padding:5px;}" +
                            ".product th {background:#dddddd;}" +

                            "@media (max-width:914px) {" +
                            "	.address-block {width:200px;}" +
                            "}"+

                            "@media (max-width:764px) {" +
                            "	.logo-header {width:600px;}" +
                            "	.logo-header img {width:80px;}" +
                            "	.company-name {font-size:17px;}" +
                            "	.order-title{font-size:15px;}" +
                            "	.address-block{font-size:10px;}" +
                            "	.client-details {width:310px;}" +
                            "	.padded-border {width:300px}" +
                            "	.detail{font-size:13px;}" +
                            "	.header{font-size:15px;}" +
                            "	.address-block {width:180px; font-size:11px;}" +
                            "}"+

                            "@media (max-width:658px) {" +
                            "	.address-block {width:140px; font-size:9px;}" +
                            "	.order-form-text{font-size:12px;}" +
                            "}" +

                            "@media (max-width:551px) {" +
                            "	.logo-header {width:600px;}" +
                            "	.logo-header img {width:70px;}" +
                            "	.company-name {font-size:15px;}" +
                            "	.order-title{font-size:12px;}" +
                            "	.address-block{font-size:10px}" +
                            "	.padded-border{width:200px}" +
                            "	.client-details {width:210px;}" +
                            "	.detail{font-size:11px;}" +
                            "	.header{font-size:13px;}" +
                            "	.address-block {width:110px; font-size:8px;}" +
                            "	.order-form-text{font-size:11px;}" +
                            "}" +

                            "	</style>" +

                            "</head>" +
                "<body style='font-family: Arial; font-size: 10.5pt; display: block; margin: 8px; line-height:1.1rem;'>" +

                        "<div> " +
                        "    <table>" +
                        "     <tbody>" +

                        "         <tr>" +
                        "             <td class='logo-header'>" +
                        "                 <table cellpadding='0' cellspacing='0' style='width: 100%;'>" +
                        "                     <tbody>" +
                        "                         <tr>" +
                        "                             <td>" +
                        "                                 <img src='http://lorefashions.com/Content/lorelogo.jpg'>" +
                        "                             </td>" +
                        "                             <td style='/* padding-left: 10px; */'>" +
                        "                                 <table cellpadding='0' cellspacing='0' class='order-header-contact-details'>" +
                        "                                     <tbody>" +

                        "                                         <tr>" +
                        "                                             <td>" +
                        "                                                 <p class='address-block'>" +
                        "                                                     <b><u>UNITED KINGDON</u></b><br>" +
                        "                                                     7 Central Business Centre<br>" +
                        "                                                     Iron Bridge Close<br>" +
                        "                                                     London NW10 0UR<br>" +
                        "                                                     TEL: +44 (0)845 224 9601<br>" +
                        "                                                     FAX: +44 (0)20 8998 0080<br>" +

                        "                                                 </p>" +
                        "                                             </td>" +
                        "                                             <td>" +
                        "                                                 <p class='address-block'>" +
                        "                                                          <b><u>U.S.A</u></b><br>" +
                        "                                                             America's MART<br>" +
                        "                                                             Showroom #10N102<br>" +
                        "                                                             250 Spring ST NW<br>" +
                        "                                                             ATLANTA, GA 30303<br>" +
                        "                                                             TEL: +1 (770) 238-1738<br>" +
                        "                                                             FAX: +1 (770) 238-1739<br>" +
                        "                                                 </p>" +
                        "                                             </td>" +
                        "                                             <td>" +
                        "                                                 <p class='address-block'>" +
                        "                                                     <b class='order-form-text'>ORDER FORM</b><br>" +
                        "                                                     Email: sales@lore-group.com<br>" +
                        "                                                     Web: www.lore-group.com" +
                        "                                                 </p>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                     </tbody>" +
                        "                                 </table>" +
                        "                             </td>" +
                        "                         </tr>" +
                        "                     </tbody>" +
                        "                 </table>" +
                        "             </td>" +
                        "         </tr>" +

                        "         <tr>" +
                        "             <td style='padding: 10px 0 10px 0; margin:0'>" +
                        "                 <table cellpadding='0' cellspacing='0'>" +
                        "    				<tbody>" +
                        "                         <tr>" +
                        "                             <td class='padded-border'>" +
                        "                                 <table cellpadding='0' cellspacing='0' class='client-details'>" +
                        "                                     <tbody>" +
                        "                                         <tr>" +
                        "                                             <td class='header'>" +
                        "                                                 Client Details:" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Company Name: <span id='lblcompname'>" +
                        "                                                     <label id='lblCompanyName'> RAVI BOUTIQUE LTD  </label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Client Name:" +
                        "                                                 <span id='lblcontact'>" +
                        "                                                     <label id='lblContactName'> RAVI PATEL</label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Address:" +
                        "                                                 <span id='lbladdress'>" +
                        "                                                     <label id='lblAddress'> SUNFLOWER COURT</label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 City:" +
                        "                                                 <span id='lblcity'>" +
                        "                                                     <label id='lblCity'> </label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 County:" +
                        "                                                 <span id='lblcounty'>" +
                        "                                                     <label id='lblCounty'> </label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Post Code:" +
                        "                                                 <span id='lblpost'>" +
                        "                                                     <label id='lblZipCode'> </label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Country:" +
                        "                                                 <span id='lblcountry'>" +
                        "                                                     <label id='lblCountry'> United Kingdom</label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Email:" +
                        "                                                 <span id='lblemail'>" +
                        "                                                     <label id='lblEmail'>raveen@lore-group.com</label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Tel1:" +
                        "                                                 <span id='lblph'>" +
                        "                                                     <label id='lblPhone'> +44 208 998 0098 </label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Mobile:<span id='lblmob'>" +
                        "                                                     <label id='lblMobile'> +447956313442 </label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 Fax:<span id='lblfax'>" +
                        "                                                     <label id='lblFax'></label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'>" +
                        "                                                 NIF/VAT:" +
                        "                                                 <span id='lblVat'>" +
                        "                                                     <label id='lblTaxID'> 4949494</label>" +
                        "                                                 </span>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                         <tr>" +
                        "                                             <td class='detail'></td>" +
                        "                                         </tr>" +
                        "                                     </tbody>" +
                        "                                 </table>" +
                        "                             </td>" +
                        "                             <td width='2%'></td>" +
                        "                             <td width='50%'>" +
                        "                                 <table cellpadding='0' cellspacing='0' class='fullwidth' style='width: 100%; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;'>" +
                        "                                     <tbody>" +
                        "                                         <tr>" +
                        "                                             <td class='border'>" +
                        "                                                 <table cellpadding='0' cellspacing='0' class='banking-details' style='display: table; border-collapse: separate; border-spacing: 2px;border-color: grey;'>" +
                        "                                                     <tbody>" +

                        "                                                         <tr>" +
                        "                                                             <td class='header' colspan='2'>Sterling Bank Details:</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>UK Bank:</td>" +
                        "                                                             <td class='detail line'>Santander</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>A/C:</td>" +
                        "                                                             <td class='detail line'>79664587</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>Sort Code:</td>" +
                        "                                                             <td class='detail line'>09-01-27</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>BIC:</td>" +
                        "                                                             <td class='detail line'>ABBYGB2LXXX</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>IBAN:</td>" +
                        "                                                             <td class='detail line'>GB54 ABBY 0901 2779 6645 87</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='header' colspan='2'>Euro Bank Details:</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>Spain Bank:</td>" +
                        "                                                             <td class='detail line'>Banco Bilbao Vizcava Argentaria</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>IBAN:</td>" +
                        "                                                             <td class='detail line'>ES19-0182-3379-73-0291557045</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>BIC:</td>" +
                        "                                                             <td class='detail line'>BBVAESMM</td>" +
                        "                                                         </tr>        " +
                        "                                                         <tr>" +
                        "                                                             <td class='header' colspan='2'>US Dollar bank details:</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>Beneficiary Bank:</td>" +
                        "                                                             <td class='detail line'>Bank of the Ozarks</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>Routing Number:</td>" +
                        "                                                             <td class='detail line'>0829 0727 3</td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='detail line'>Beneficiary A/C no.:</td>" +
                        "                                                             <td class='detail line'>107067745</td>" +
                        "                                                         </tr>        " +

                        "                                                     </tbody>" +
                        "                                                 </table>" +
                        "                                             </td>" +
                        "                                         </tr>" +

                        "                                         <tr>" +
                        "                                             <td align='right'>" +
                        "                                                 <table class='order-details' cellpadding='0' cellspacing='0' style='padding-top: 20px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;'>" +
                        "                                                     <tbody>" +
                        "                                                         <tr>" +
                        "                                                             <td class='header'>DATE OF ORDER:</td>" +
                        "                                                             <td class='detail' align='right' width='180px'>" +
                        "                                                                 <span id='lblOrderDate'>" +
                        "                                                                     <label id='lblOrderDate'> 14-05-2018</label>" +
                        "                                                                 </span>" +
                        "                                                             </td>" +
                        "                                                         </tr>" +
                        "                                                         <tr>" +
                        "                                                             <td class='header'>ORDER NO.:</td>" +
                        "                                                             <td class='detail' align='right'>" +
                        "                                                                 <span id='lblOrderNo'>" +
                        "                                                                     <label id='lblOrderReference'> RO32UK </label>" +
                        "                                                                 </span>" +
                        "                                                             </td>" +
                        "                                                         </tr>" +

                        "                                                     </tbody>" +
                        "                                                 </table>" +
                        "                                             </td>" +
                        "                                         </tr>" +
                        "                                     </tbody>" +
                        "                                 </table>" +
                        "                             </td>" +
                        "                         </tr>             " +

                        "    					</tbody>" +
                        "                 </table>" +
                        "             </td>" +
                        "         </tr>" +
                        "     </tbody>" +
                        " </table>" +
                        "    </div>" +



        "    <div style='overflow-x:auto;'>" +
        "        <table class='product' style='width: 70%;'>" +
        "        <tbody><tr style='/* width: 100%; */display: table-row;vertical-align: inherit;border-color: inherit;'>" +
        "        <td class='order-item-details' style='vertical-align: top; display: table-cell;'>" +
        "         <div style='overflow-x:auto;'> " +
        "		 <table id='detailsTable' style='width:100%; padding-bottom:0px; padding-top:0px; display: table;  border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +

        "                <thead style='display: table-header-group; vertical-align: middle; border-color: inherit;'>" +

        "				<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
        "                        <th rowspan='3'>Product</th>" +
        "                        <th rowspan='3'>Color</th>" +
        "                        <th rowspan='3'>Price</th>" +
        "                        <th>US</th>" +
        "                           <th>00</th>" +
        "                           <th>0</th>" +
        "                           <th>2</th>" +
        "							<th>4</th>" +
        "                           <th>6</th>" +
        "							<th>8</th>" +
        "							<th>10</th>" +
        "							<th>12</th>" +
        "							<th>14</th>" +
        "							<th>16</th>" +
        "							<th>18</th>" +
        "							<th>20</th>" +
        "							<th>22</th>" +
        "							<th>24</th>" +
        "							<th>26</th>" +
        "							<th>28</th>" +
        "							<th>30</th>" +
        "                        <th rowspan='3'>Quantity</th>" +
        "                        <th rowspan='3'>Total</th>" +
        "                        <th style='width:0%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd; display:none;'></th>" +
        "                    </tr>" +
        "					<tr>      " +
        "                        <th>UK</th>" +
        "                        <th>2</th>" +
        "							<th>4</th>" +
        "                           <th>6</th>" +
        "							<th>8</th> " +
        "							<th>10</th>" +
        "							<th>12</th>" +
        "							<th>14</th>" +
        "							<th>16</th>" +
        "							<th>18</th>" +
        "							<th>20</th>" +
        "							<th>22</th>" +
        "							<th>24</th>" +
        "							<th>26</th>" +
        "							<th>28</th>" +
        "							<th>30</th>" +
        "							<th>32</th>" +
        "							<th>34</th>" +
        "                        <th style='width:0%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd; display:none;'></th>" +
        "                    </tr>" +
        "					<tr>" +
        "                            <th>EU</th>" +
        "                            <th>30</th>" +
        "							 <th>32</th>" +
        "                            <th>34</th>" +
        "							<th>36</th> " +
        "							<th>38</th> " +
        "							<th>40</th> " +
        "							<th>42</th> " +
        "							<th>44</th> " +
        "							<th>46</th> " +
        "							<th>48</th> " +
        "							<th>50</th> " +
        "							<th>52</th> " +
        "							<th>54</th> " +
        "							<th>56</th> " +
        "							<th>58</th> " +
        "							<th>60</th> " +
        "							<th>62</th> " +
        "                        <th style='width:0%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd; display:none;'></th>" +
        "                    </tr>" +
        "                </thead>" +
        "                <tbody style='border:solid;border-color:black;border-width:inherit;'>" +

        "                        <tr>" +
        "                            <td> 61108</td>" +
        "                            <td> Red</td>  " +
        "                            <td> 199</td>" +
        "                            <td> </td>" +
        "                            <td></td>"  + 
        "							 <td></td> " +
        "							 <td></td> " +
        "							 <td></td> " +
        "							 <td>1</td>" +
        "							 <td></td> " +
        "                            <td>1</td>" +
        "                            <td>1</td>" +
        "                            <td></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'></td>" +
        "                           <td style='width:10%'> 3</td>" +
        "                           <td style='width:10%'> 597</td>" +
        "                        </tr>" +

        "                </tbody>" +
        "                <tfoot>" +
        "                    <tr>" +
        "                        <td colspan='20' rowspan='5' style='width:10%'>" +
        "                            <div style='padding: 15px;height: 60px;border: 1px solid black;'>" +
        "                                <b>Order Note:</b><br> " +
        "                            </div>" +
        "                        </td>" +
        "                    </tr>" +
        "                    <tr>" +
        "                        <td colspan='2'>Sub Total</td>" +
        "                        <td>597.00</td>" +
        "                    </tr>" +
        "                    <tr>" +
        "                        <td colspan='2'>Delivery</td>" +
        "                        <td>0.00</td>" +
        "                    </tr>" +
        "                    <tr>" +
        "                        <td colspan='2'> Tax </td>" +
        "                        <td>0.0000</td>" +
        "                    </tr>" +
        "                    <tr>" +
        "                        <td colspan='2'> Total </td>" +
        "                        <td> 597.00 </td>" +
        "                    </tr>" +
        "                </tfoot>" +
        "            </table>" +
        "		</div>" +
        "        </td>" +
        "    </tr>" +
        "</tbody></table>" +
        "</div>" +

        "</body>" +
        "</html>";

            string from = "chandrashekhar.bairagi@connekt.in"; //any valid GMail ID
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress(from, "Loré Fashions");
            mail.Body = data;
            //mail.To.Add(Model.CustomerModel.EmailId);
            mail.To.Add("chandrashekhar.bairagi@connekt.in");
            mail.Subject = "Confirmation from supplier";
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();
            networkCredential.UserName = "chandrashekhar.bairagi@connekt.in";
            networkCredential.Password = "connekt";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);

        }

        
        public void SendAmfiMail()
        {
            string img = "data:image/jpeg;base64," + GetBase64FromFile(Server.MapPath("\\amfinewCompressed.jpg"));

            string data = "<!DOCTYPE html>" +
                        "<html lang = 'en'>" +
                        " <head>" +
                        "     <meta charset = 'utf-8'>" +
                        "      <style>" +
                        "          body {" +
                //"                margin - left: auto;" +
                //"                margin - right: auto;" +
                        "                background - color: white;" +
                        "            }" +
                        "        .nowrap { white - space: nowrap; }" +
                        "        .overflow - wrap { overflow - wrap: break-word; }" +
                        "        .center {    display: block;    margin-left: auto;    margin-right: auto;    width: 50%; }" +
                        "    </style>" +
                        "</head>" +
                        "<body> " +
                        "<div><br/><img src='" + img + "' alt='AMFI' class='center' /><br/></div>" +
                        "</body></html>";


            string from = "chandrashekhar.bairagi@connekt.in"; //any valid GMail ID
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress(from, "AMFI");
            mail.Body = data;
            //mail.To.Add(Model.CustomerModel.EmailId);
            mail.To.Add("chandrashekhar.bairagi@connekt.in");
            mail.Subject = "Invitation from AMFI";
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;



            

            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();
            networkCredential.UserName = "chandrashekhar.bairagi@connekt.in";
            networkCredential.Password = "connekt";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);

        }

        public static string GetBase64FromFile(string path)
        {
            Byte[] bytes = System.IO.File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            return file;

        }
        #region:Sale by Show Report 

        public ActionResult SaleByShowReport()
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Sale by Show Report ",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                OrderViewModel Model = new OrderViewModel();
                Model.OrderlocatorList = _productProvider.GetAllOrderLocatorList();
                Model.OrderlocatorList.Insert(0, new OrderLocatorViewModel()
                {
                    Id = 0,
                    OrderLocatorName = "--Select Order Locator--"
                });
                Model.POmodel.WareHouseList = _orderProvider.GetAllWareHouse();
                Model.POmodel.WareHouseList.Insert(0, new WareHouseViewModel()
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
        public ActionResult SaleByShowReport(OrderViewModel Model)
        {


            OrderViewModel model = new OrderViewModel();
            List<SizeViewModel> finalSizeModel = new List<SizeViewModel>();
            List<SizeViewModel> sizemodel = _orderProvider.GetSalebyShowReportInventory(Model.OrderlocatorId,Model.POmodel.WareHouseId, Model.OrderDate, Model.OrderDateString);
            Int64 total = 0;
            if (sizemodel.Count > 0)
            {

                foreach (var x in sizemodel)
                {
                    total = x.Qty;
                    if (total > 0)
                    {
                        var alravail = finalSizeModel.Where(k => k.StyleNumber == x.StyleNumber && k.ColourName == x.ColourName).FirstOrDefault();
                        if (alravail == null)
                            finalSizeModel.AddRange(sizemodel);
                    }
                }

            }
            
            model.ProductListModel.StockSizeModel = LinqHelper.ChunkBy(finalSizeModel, 17);
            if (finalSizeModel.ToList().Count > 0)
                model.POmodel.IsDataAvailable = true;
            model.OrderlocatorList = _productProvider.GetAllOrderLocatorList();
            model.OrderlocatorList.Insert(0, new OrderLocatorViewModel()
            {
                Id = 0,
                OrderLocatorName = "--Select Order Locator--"
            });
            model.POmodel.WareHouseList = _orderProvider.GetAllWareHouse();
            model.POmodel.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });

         
                return View(model);
        }
            #endregion

            #region:Sale by Product Report 
            public ActionResult SalebyProductReport()
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Sale by Product Report ",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            try
            {
                var CurrentUserId = _session.CurrentUser.Id;
                PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
                Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
                Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
                Model.WareHouseList = _orderProvider.GetAllWareHouse();
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
        public ActionResult SalebyProductReport(PurchaseOrderViewModel Model)
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Sale by Product Report",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            List<SizeViewModel> finalSizeModel = new List<SizeViewModel>();
            string categoryId = Model.CategoryId.ToString();
            //var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.CollectionYearId == Model.CollectionYearId).OrderBy(x=>x.ProductName).Skip(skipValue).Take(takeValue).ToList();
           // var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.CollectionYearId == Model.CollectionYearId).OrderBy(x => x.ProductName).ToList();
        //    if (Model.CollectionYearId == 0)
           var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.ProductName==Model.ProductModel.ProductSalesbyName).ToList();
            if (Model.CategoryId == 0)
            {
                productsList = _context.Products.Where(x => x.StatusId != 4 && x.ProductName == Model.ProductModel.ProductSalesbyName).ToList();
            }
            else if (Model.ProductModel.ProductSalesbyName == null)
            {
                 productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId)).ToList();

            }
            else if (productsList == null)
            {
                Model.ProductListModel.StockSizeModel = LinqHelper.ChunkBy(finalSizeModel, 17);
                if (finalSizeModel.ToList().Count > 0)
                    Model.IsDataAvailable = true;
                Model.WareHouseList = _orderProvider.GetAllWareHouse();
                Model.WareHouseList.Insert(0, new WareHouseViewModel()
                {
                    Id = 0,
                    WareHouseName = "--Select Ware House--"
                });
                Model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
                Model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });
                Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
                Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
                return View(Model);


            }
            foreach (var ab in productsList)
            {
                Int64 total = 0;

                List<long> colorIds = ab.SeletedColorIds.TrimEnd(',').Split(',').Select(long.Parse).ToList();
                total = 0;
                foreach (long? colorid in colorIds)
                {
                    total = 0;
                    if (colorid.HasValue)
                    {
                        if (colorid.Value > 0)
                        {
                            List<SizeViewModel> sizemodel = _orderProvider.GetSalebyProductReportInventory(ab.Id, ab.ProductName, colorid.Value, Model.WareHouseId, Model.OrderDate, Model.OrderDateString);

                            if (sizemodel.Count > 0)
                            {

                                foreach (var x in sizemodel)
                                {
                                    total = x.Qty;
                                    if (total > 0)
                                    {
                                        var alravail = finalSizeModel.Where(k => k.StyleNumber == x.StyleNumber && k.ColourName == x.ColourName).FirstOrDefault();
                                        if (alravail == null)
                                            finalSizeModel.AddRange(sizemodel);
                                    }
                                }

                            }
                        }
                    }
                }

            }
            //List<SizeViewModel> sizemodel = _orderProvider.GetSizeModelForInventory(Model);
            Model.ProductListModel.StockSizeModel = LinqHelper.ChunkBy(finalSizeModel, 17);
            if (finalSizeModel.ToList().Count > 0)
                Model.IsDataAvailable = true;
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });
            Model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
            Model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });
            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
            return View(Model);
        }


        #endregion
        #region : Stock Inventory Report

        public ActionResult StockReport()
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Report/",
                Title = "Stock Report",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            try {
                var Current = _session.CurrentUser.Id;

            PurchaseOrderViewModel Model = new PurchaseOrderViewModel();

            int productCount = _context.Products.Where(x => x.StatusId != 4).Count();
            List<SelectListItem> productsSize = new List<SelectListItem>();
            if (productCount > 0)
                productsSize.Add(new SelectListItem { Text = "1-100", Value = "1" });
            if (productCount > 100)
                productsSize.Add(new SelectListItem { Text = "101-200", Value = "2" });
            if (productCount > 200)
                productsSize.Add(new SelectListItem { Text = "201-300", Value = "3" });
            if (productCount > 300)
                productsSize.Add(new SelectListItem { Text = "301-400", Value = "4" });
            if (productCount > 400)
                productsSize.Add(new SelectListItem { Text = "401-500", Value = "5" });
            if (productCount > 500)
                productsSize.Add(new SelectListItem { Text = "501-600", Value = "6" });
            if (productCount > 600)
                productsSize.Add(new SelectListItem { Text = "601-700", Value = "7" });
            if (productCount > 700)
                productsSize.Add(new SelectListItem { Text = "701-800", Value = "8" });

            Model.StockPaging = productsSize;
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel() { Id = 0, WareHouseName = "--Select Ware House--" });

            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
            Model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
            Model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });

            return View(Model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult StockReport(PurchaseOrderViewModel Model)
                {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Report/",
                Title = "Stock Report",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            List<SizeViewModel> finalSizeModel = new List<SizeViewModel>();

            int skipValue = (Convert.ToInt32(Model.PageValue) - 1) * 100;
            int takeValue = (Convert.ToInt32(Model.PageValue)) * 100;

            string categoryId = Model.CategoryId.ToString();            
            //var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.CollectionYearId == Model.CollectionYearId).OrderBy(x=>x.ProductName).Skip(skipValue).Take(takeValue).ToList();
            var productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId) && x.CollectionYearId == Model.CollectionYearId).OrderBy(x=>x.ProductName).ToList();
            if(Model.CollectionYearId==0)
                productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(categoryId)).OrderBy(x => x.ProductName).ToList();
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
                            foreach (var x in sizemodel)
                            {
                                if (sizemodel.Count > 0)
                                {
                                    //total = sizemodel.Sum(x => x.InStockQty - x.Qty + x.POQty);
                                    total =  x.InStockQty;
                                    if (total > 0)
                                    {
                                        var alravail = finalSizeModel.Where(k => k.StyleNumber == x.StyleNumber && k.ColourName == x.ColourName).FirstOrDefault();
                                        if (alravail == null)
                                            finalSizeModel.AddRange(sizemodel);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            Model.ProductListModel.StockSizeModel = LinqHelper.ChunkBy(finalSizeModel,17);

            int productCount = _context.Products.Where(x => x.StatusId != 4).Count();
            List<SelectListItem> productsSize = new List<SelectListItem>();
            if (productCount > 0)
                productsSize.Add(new SelectListItem { Text = "1-100", Value = "1" });
            if (productCount > 100)
                productsSize.Add(new SelectListItem { Text = "101-200", Value = "2" });
            if (productCount > 200)
                productsSize.Add(new SelectListItem { Text = "201-300", Value = "3" });
            if (productCount > 300)
                productsSize.Add(new SelectListItem { Text = "301-400", Value = "4" });
            if (productCount > 400)
                productsSize.Add(new SelectListItem { Text = "401-500", Value = "5" });
            if (productCount > 500)
                productsSize.Add(new SelectListItem { Text = "501-600", Value = "6" });
            if (productCount > 600)
                productsSize.Add(new SelectListItem { Text = "601-700", Value = "7" });
            if (productCount > 700)
                productsSize.Add(new SelectListItem { Text = "701-800", Value = "8" });

            Model.StockPaging = productsSize;
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel() { Id = 0, WareHouseName = "--Select Ware House--" });
            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select--" });
            Model.CollectionYearList = _productProvider.GetAllCollectionYearList().OrderBy(x => x.CollectionYear).ToList();
            Model.CollectionYearList.Insert(0, new CollectionYearViewModel { Id = 0, CollectionYear = "--Select--" });

            return View(Model);
        }

        #endregion

        #region : Inventory Shortfall Report

        public ActionResult ShortfallInventoryReport()
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Shortfall Inventory Report",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            try
            {

                var Current = _session.CurrentUser.Id;
            PurchaseOrderViewModel Model = new PurchaseOrderViewModel();
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });
            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select Category--" });
            return View(Model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult ShortfallInventoryReport(PurchaseOrderViewModel Model)
        {
            var BreadCrumbModel = new BreadCrumbModel
            {
                Url = "/Home/",
                Title = "Shortfall Inventory Report",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = BreadCrumbModel;
            List<SizeViewModel> finalSizeModel = new List<SizeViewModel>();

            var productsList = _context.Products.Where(x => x.StatusId != 4).OrderBy(x => x.ProductName).ToList();
            if (Model.CategoryId > 0)
            {
                string catgid = Model.CategoryId.ToString() + ",";
                //productsList = _context.Products.Where(x => x.StatusId != 4 && x.CategoryId == Model.CategoryId).OrderBy(x => x.ProductName).ToList();
                productsList = _context.Products.Where(x => x.StatusId != 4 && x.SeletedCategoryIds.Contains(catgid)).OrderBy(x => x.ProductName).ToList();

            }
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
                            List<SizeViewModel> sizemodel = _orderProvider.GetSizeModelForShortfallInventory(product.Id, product.ProductName, colorid.Value, Model.WareHouseId);

                            if (sizemodel.Count > 0)
                            {
                                
                                foreach (var x in sizemodel)
                                {
                                    total = x.InStockQty - x.Qty + x.POQty;
                                    if (total < 0)
                                    {
                                        var alravail = finalSizeModel.Where(k => k.StyleNumber == x.StyleNumber && k.ColourName == x.ColourName).FirstOrDefault();
                                        if (alravail == null)
                                            finalSizeModel.AddRange(sizemodel);
                                    }
                                }

                            }
                        }
                    }
                }
            }

            //List<SizeViewModel> sizemodel = _orderProvider.GetSizeModelForInventory(Model);
            Model.ProductListModel.StockSizeModel = LinqHelper.ChunkBy(finalSizeModel, 17);
            if (finalSizeModel.ToList().Count > 0)
                Model.IsDataAvailable = true;
            Model.WareHouseList = _orderProvider.GetAllWareHouse();
            Model.WareHouseList.Insert(0, new WareHouseViewModel()
            {
                Id = 0,
                WareHouseName = "--Select Ware House--"
            });
            Model.CategoryList = _productProvider.GetAllCategoryList().OrderBy(x => x.Category).ToList();
            Model.CategoryList.Insert(0, new CategoryViewModel { Id = 0, Category = "--Select Category--" });
            return View(Model);
        }

        #endregion


    }

    public static class LinqHelper
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}