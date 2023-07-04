using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Exceptions;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Loregroup.Provider
{
    public class OrderProvider : IOrderProvider
    {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        private readonly ISession _session;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ICommonProvider _commonProvider;
        private IContentProvider _contentProvider;
        public static Int64? roleIdvalue = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        int timeout = 3600;
        public string Classifiedpath;        

        public OrderProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ICommonProvider commonProvider, ISession session)
        {
            _context = context;
            _security = security;
            _session = session;
            _utilities = utilities;
            _configProvider = configProvider;
            _commonProvider = commonProvider;
        }

        #region : Order

        public string SaveOrder(OrderViewModel Model)
        {
            try
            {
                var check = _context.OrderMasters.Where(x => x.OrderNo == Model.OrderNo).FirstOrDefault();
                if (check != null)
                {
                    return "Order No. already exist";
                }
                else
                {
                    try
                    {
                        Model.CustomerModel.Tax = _context.MasterUsers.FirstOrDefault(x => x.Id == Model.CustomerModel.Id).TaxEnabled;

                    }
                    catch (Exception)
                    {
                    }

                    Model.Tax = 0;
                    //Save Master
                    decimal TotalAmount = 0;
                    if (Model.CustomerModel.CountryId == 3)// && Model.CustomerModel.Tax == true)
                    {
                        Model.Tax = _context.Taxes.FirstOrDefault(x => x.Id == 1).TaxPercentage;
                        TotalAmount = Model.Amount + ((Model.Amount + Model.ShippingCharge + Model.Rushfee + Model.Extracharges) * Model.Tax / 100) + Model.ShippingCharge +Model.Rushfee + Model.Extracharges;
                    }
                    else
                    {
                        TotalAmount = Model.Amount + Model.ShippingCharge + Model.Rushfee + Model.Extracharges;
                    }

                    OrderMaster Master = new OrderMaster()
                    {
                        CustomerId = Model.CustomerModel.Id,
                        CustomerFullName = Model.CustomerModel.CustomerFullName,
                        ZipCode = Model.CustomerModel.ZipCode,
                        CurrencyName = Model.CustomerModel.CurrencyName.Trim(),
                        MobileNo = Model.CustomerModel.MobileNo,
                        ShippingCharge = Model.ShippingCharge,
                        //TaxEnable = Model.TaxEnable,
                        Tax = Model.Tax,
                        EmailId = Model.CustomerModel.EmailId,
                        OrderNote = Model.OrderNote,
                        SystemNotes = Model.SystemNotes,
                        OrderNo = Model.OrderNo,
                        //OrderDate =Convert.ToDateTime(Model.OrderDate),
                        OrderDate = (!String.IsNullOrEmpty(Model.OrderDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : Convert.ToDateTime(DateTime.ParseExact(DateTime.Now.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        WearDate = (!String.IsNullOrEmpty(Model.WearDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.WearDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                        UserSelectDeliveryDate = (!String.IsNullOrEmpty(Model.UserSelectDeliveryDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.UserSelectDeliveryDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                        BridesName = Model.BridesName,
                        PONumber = Model.PurchaseOrderNumber,
                        WareHouseId = Convert.ToInt64(Model.CustomerModel.DistributionPointId),
                        TotalProduct = Model.ProductList.Count,
                        DueDate = (!String.IsNullOrEmpty(Model.DeliveryDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.DeliveryDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                        ShippingState = Model.CustomerModel.ShippingState,
                        Amount = Model.Amount,
                        TotalAmount = TotalAmount,
                        OrderStatusId = 1,
                        StatusId = 1,
                        CreationDate = DateTime.UtcNow,
                        CreatedById = Model.CreatedById,
                        Rushfee=Model.Rushfee,
                        OrderlocatorId=Model.OrderlocatorId,
                        Extracharges=Model.Extracharges,
                        Isdeposit=Model.IsPayment,

                    };
                    _context.OrderMasters.Add(Master);
                    _context.SaveChanges();


                    Model.Id = Master.Id;

                    //Update Stock Inventory
                    UpdateOrderInventory(Model);


                    //Save Details
                    foreach (var data in Model.ProductList)
                    {
                        foreach (var list in data.SizeModel)
                        {
                            OrderDetail Details = new OrderDetail()
                            {
                                OrderMasterId = Master.Id,
                                ProductId = data.Product.Id,
                                //SizeId
                                OrderPrice = data.Product.OrderPrice,
                                SizeUK = list.SizeUK,
                                ColourId = data.Product.ColourId,
                                Qty = list.Qty,
                                StatusId = 1,
                                CreatedById = Model.CreatedById,
                                CreationDate = Model.CreationDate
                            };
                            _context.OrderDetails.Add(Details);
                            _context.SaveChanges();
                        }
                    }

                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error";
            }

        }

        public string UpdateOrder(OrderViewModel Model)
        {
            try
            {
                var chk = _context.OrderMasters.Where(x => x.OrderNo == Model.OrderNo && x.Id != Model.Id).FirstOrDefault();
                if (chk != null)
                {
                    return "Order No. already exist";
                }
                else
                {
                    try
                    {
                        Model.CustomerModel.Tax = _context.MasterUsers.FirstOrDefault(x => x.Id == Model.CustomerModel.Id).TaxEnabled;
                    }
                    catch (Exception)
                    {
                    }

                    //Save Master
                    Model.Tax = 0;

                    decimal TotalAmount = 0;
                    if (Model.CustomerModel.CountryId == 3 && Model.CustomerModel.Tax == true)
                    {
                        Model.Tax = _context.Taxes.FirstOrDefault(x => x.Id == 1).TaxPercentage;
                        TotalAmount = Model.Amount + ((Model.Amount + Model.ShippingCharge + Model.Rushfee + Model.Extracharges) * Model.Tax / 100) + Model.ShippingCharge + Model.Rushfee + Model.Extracharges;
                    }
                    else
                    {
                        TotalAmount = Model.Amount + Model.ShippingCharge + Model.Rushfee+Model.Extracharges;
                    }

                    OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                    if (Master != null)
                    {
                       
                        Master.ZipCode = Model.CustomerModel.ZipCode;
                        Master.CurrencyName = Model.CustomerModel.CurrencyName.Trim();
                        Master.MobileNo = Model.CustomerModel.MobileNo;
                        Master.ShippingCharge = Model.ShippingCharge;
                        //Master.TaxEnable = Model.TaxEnable;
                        Master.Tax = Model.Tax;
                        Master.EmailId = Model.CustomerModel.EmailId;
                        Master.OrderNo = Model.OrderNo;
                        Master.OrderDate = Convert.ToDateTime(DateTime.ParseExact(Model.OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        if (Model.WearDate != null && Model.WearDate != "")
                        {
                            Master.WearDate = Convert.ToDateTime(DateTime.ParseExact(Model.WearDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        }
                
                            if (Model.UserSelectDeliveryDate != null && Model.UserSelectDeliveryDate != "")
                            {

                                Master.UserSelectDeliveryDate = Convert.ToDateTime(DateTime.ParseExact(Model.UserSelectDeliveryDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            }


                        
                        Master.BridesName = Model.BridesName;
                            Master.PONumber = Model.PurchaseOrderNumber;
                            Master.TotalProduct = Model.ProductList.Count;
                        // Master.DeliveryDate = Model.DeliveryDate;

                        if (Model.DeliveryDate != null && Model.DeliveryDate != "")
                        {
                            Master.DueDate = Convert.ToDateTime(DateTime.ParseExact(Model.DeliveryDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        }

                        Master.ShippingState = Model.CustomerModel.ShippingState;
                            Master.Amount = Model.Amount;
                            Master.TotalAmount = TotalAmount;
                            Master.OrderNote = Model.OrderNote;
                            Master.SystemNotes = Model.SystemNotes;
                            Master.OrderStatusId = (int)Model.OrderStatusId;
                         
                            Master.ModificationDate = DateTime.UtcNow;
                            Master.ModifiedById = Model.CreatedById;
                            Master.Rushfee = Model.Rushfee;
                            Master.OrderlocatorId = Model.OrderlocatorId;
                            Master.Extracharges = Model.Extracharges;
                            Master.Isdeposit = Model.IsPayment;
                   
                     
                        }
                    _context.SaveChanges();
                    
                    if (Model.OrderStatusId != OrderStatus.Cancelled)
                    {
                        //Update Stock Inventory
                        UpdateOrderInventory(Model);
                    }

                    if (Model.OrderStatusId == OrderStatus.Cancelled)
                    {
                        List<OrderDetail> DetailsList = _context.OrderDetails.Where(x => x.OrderMasterId == Model.Id && x.StatusId != 4).ToList();
                        foreach (var List in DetailsList)
                        {
                            StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "Order" && x.SizeUK == List.SizeUK && x.ColourId == List.ColourId && x.ProductId == List.ProductId && x.WareHouseId == Master.WareHouseId && x.StatusId != 4).FirstOrDefault();
                            if (StockDetail != null)
                            {
                                Int64 localQty = List.Qty;
                                //StockDetail.ReceivedQty = StockDetail.ReceivedQty + localQty;
                                StockDetail.Qty = StockDetail.Qty - localQty;
                                StockDetail.ModificationDate = DateTime.UtcNow;
                            }
                        }
                        _context.SaveChanges();
                    }

                    //Save Details
                    List<OrderDetail> Details = _context.OrderDetails.Where(x => x.OrderMasterId == Model.Id && x.StatusId != 4).ToList();
                    foreach (var data in Details)
                    {
                        data.StatusId = 4;
                    }
                    _context.SaveChanges();

                    foreach (var data in Model.ProductList)
                    {
                        foreach (var list in data.SizeModel)
                        {
                            OrderDetail Detail = _context.OrderDetails.Where(x => x.OrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id).FirstOrDefault();
                            if (Detail == null)
                            {
                                OrderDetail DetailsModel = new OrderDetail()
                                {
                                    OrderMasterId = Model.Id,
                                    ProductId = data.Product.Id,
                                    //SizeId
                                    OrderPrice = data.Product.OrderPrice,
                                    SizeUK = list.SizeUK,
                                    ColourId = data.Product.ColourId,
                                    Qty = list.Qty,
                                    StatusId = 1,
                                    CreatedById = Model.CreatedById,
                                    CreationDate = DateTime.UtcNow
                                };
                                _context.OrderDetails.Add(DetailsModel);
                            }
                            else
                            {
                                                           
                                Detail.OrderPrice = data.Product.OrderPrice;
                               
                                Detail.Qty = list.Qty;
                                Detail.StatusId = 1;
                             
                                Detail.ModifiedById = Model.CreatedById;
                                Detail.ModificationDate = DateTime.UtcNow;
                            }
                            _context.SaveChanges();
                        }
                    }

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }


      public  List<OrderListDisplayViewModel> GetAllCustmerOrder(int start, int length, string search, int filtercount, Int64 current, Int64 OrderStatusId, Int64 orderlocId)

        { 
                        List<OrderListDisplayViewModel> newData = new List<OrderListDisplayViewModel>();
            try
            {
                        List<OrderListDisplayViewModel> result = new List<OrderListDisplayViewModel>();
         
                var query1 = _context.MasterUsers.Where(x => x.territoryId == current && x.StatusId != 4).ToList();

                IQueryable<OrderMaster> query = _context.OrderMasters.OrderByDescending(x => x.OrderDate).Where(y => y.StatusId != 4);
                //query = query.Where(x => x.UserId == userid);

                if (OrderStatusId == 0)
                {
                    query = query.Where(x => x.OrderStatusId != (int)OrderStatus.Cancelled);
                }
                else
                {
                    query = query.Where(x => x.OrderStatusId == OrderStatusId);
                }

              
                if (orderlocId != 0)
                {
                    if (orderlocId == 5)
                        query = query.Where(x => (x.OrderlocatorId == orderlocId || x.OrderlocatorId == 10));
                    else
                        query = query.Where(x => x.OrderlocatorId == orderlocId);
                }
                //Search Condition
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    query = query.Where(p => (p.CustomerFullName.Contains(value) || p.OrderNo.Contains(value) || (p.CustomerId == _context.MasterUsers.FirstOrDefault(z => z.ShopName.ToLower().Contains(value.ToLower())).Id)));
                }

                var ab = (from ccc in query1
                          join od in query on ccc.Id equals od.CustomerId
                          orderby
                              od.CustomerId
                          select new
                          {
                              od.Id,
                              ccc.ShopName,
                              od.OrderNo,
                              od.OrderDate,
                              od.Isdeposit,
                              od.TotalAmount,
                              od.OrderNote,
                              od.OrderStatusId,
                              od.DeliveryDate,
                              od.CurrencyName,
                              od.SystemNotes,
                          }).ToList();

                foreach (var bcc in ab)
                {
                    var color = "";
                  //  var colorpo = "";
                    var colorpay = "";
                    var coloronlne = "#333333";
                    if ((int)bcc.OrderStatusId == 1)
                    {
                        color = "#3c8dbc"; //status new, color blue
                    }
                    else if ((int)bcc.OrderStatusId == 2)
                    {
                        color = "#09c977"; //status In Progress, color Green
                    }
                    else if ((int)bcc.OrderStatusId == 3)
                    {
                        color = "#ff0000"; //status In Payment Failed, color red
                    }
                    else if ((int)bcc.OrderStatusId == 4)
                    {
                        color = "#229954"; //status Completed, color Dark-Green
                    }
                    else if ((int)bcc.OrderStatusId == 5)
                    {
                        color = "#C98209"; //status Cancelled, color Orange
                    }
                    else if ((int)bcc.OrderStatusId == 6)
                    {
                        color = "#d4bf29"; //status Dispatched, color yellow
                    }
                    else if ((int)bcc.OrderStatusId == 7)
                    {
                        color = "#795C32"; //status Partially-Dispatched, color Light Brown
                    }
                    else if ((int)bcc.OrderStatusId == 8)
                    {
                        color = "#6da56b"; //status Awaiting Confirmation, color Light Green
                    }
                    else if ((int)bcc.OrderStatusId == 9)
                    {
                        color = "#FF3F3F"; //status In-Stock, color Red Orange
                    }
                    var TotalAmountString = "";
                    if (bcc.CurrencyName == "EURO")
                    {
                        TotalAmountString = "€ " + bcc.TotalAmount.ToString();
                    }
                    else if (bcc.CurrencyName == "GBP")
                    {
                        TotalAmountString = "£ " + bcc.TotalAmount.ToString();
                    }
                    else
                    {
                        TotalAmountString = "$ " + bcc.TotalAmount.ToString();
                    }

                    string Notes = "";
                    if (!String.IsNullOrEmpty(bcc.OrderNote))
                    {
                        Notes += "<span class='tooltip' style='opacity:1;display:inline;float:left;'><i class='fa fa-info-circle fa-lg' style='color:red'></i> <span class='tooltiptext tooltip-left'>" + bcc.OrderNote + "</span> </span>";
                    }

                    if (!String.IsNullOrEmpty(bcc.SystemNotes))
                    {
                        if (!String.IsNullOrEmpty(Notes))
                            Notes += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        Notes += "<span class='tooltip' style='opacity:1;display:inline;float:right;'><i class='fa fa-comments fa-lg' style='color:red'></i> <span class='tooltiptext tooltip-right'>" + bcc.SystemNotes + "</span> </span>";
                    }

                    string ispay = null;
                    if (bcc.Isdeposit == true)
                    {
                        ispay = "YES";
                        colorpay = "#229954";
                    }
                    else
                    {
                        ispay = "NO";
                        colorpay = "#ff0000";
                    }
                    OrderListDisplayViewModel oo = new OrderListDisplayViewModel()
                    {

                        
                        ShopName = bcc.ShopName,
                      
                   
                 
              
                
                        OrderStatusId = (OrderStatus)bcc.OrderStatusId,
                        //OrderNo = orderMaster.OrderNo,
                        OrderNo = "<span style='color: " + coloronlne + ";'>" + bcc.OrderNo + "</span>",
                        OrderDate = bcc.OrderDate.ToShortDateString(),
                     //   UserSelectDeliveryDate = (bcc.UserSelectDeliveryDate.HasValue) ? bcc.UserSelectDeliveryDate.Value.ToShortDateString() : "",
                   
                        DeliveryDate = bcc.DeliveryDate,
                    
                        TotalAmountString = TotalAmountString,
                        //IsPOPlaced = POPlaced,
                    //    IsPOPlaced = "<span style='color:white;background: " + colorpo + ";'>" + POPlaced + "</span>",
                        TotalAmount = bcc.TotalAmount,
                        //OrderNotes = "<span class='tooltip' style='opacity:1;'><i class='fa fa-info-circle fa-lg'></i> <span class='tooltiptext tooltip-left'>" + orderMaster.OrderNote + "</span> </span>",
                        OrderNotes = Notes,
                     //   TotalProducts = orderMaster.TotalProduct,
                        IsPayment = "<span style='color:white;background: " + colorpay + ";'>" + ispay + "</span>", //ispay,
                       // DispatchOrder = "<a href='/Order/DispatchOrder?Id=" + orderMaster.Id + "'title='Dispatch Order'>Dispatch Order</a>",
                        OrderStatus = "<span style='color:white;background: " + color + "; font-weight:bold;border-radius: 4px;padding: 3px;'>" + Regex.Replace(((OrderStatus)bcc.OrderStatusId).ToString(), "([A-Z])([a-z]*)", " $1$2") + "</span>",
                     //   Edit = "<a href='/Order/AddNewOrder?id=" + orderMaster.Id + "' title='Edit'><img src='/Content/img/editicon.png'style='height: 15px;'/></a>",
                    //    Delete = "<a href='/Order/DeleteOrder?id=" + orderMaster.Id + "' title='Delete'  onclick='return Confirmation();'><img src='/Content/img/deleteicon.png'style='height: 15px;'/></a>",
                        PrintPreview = "<a href='/Order/GetPrintPreview?id=" + bcc.Id + "' title='Print Preview' target='_blank'><img src='/Content/img/PrintPreview.png'style='height: 15px;'/></a>",
                       //RoleId = ab.ToString(),
                       // IsActive = ab.IsAdmin,


                    };

                      result.Add(oo); 
                
                }
            
                newData = result;

                return newData;
            }
            catch (Exception e)
            {
                return newData;
            }
            
           
        }


        public List<OrderListDisplayViewModel> GetAllOrders(int start, int length, string search, int filtercount, Int64 OrderStatusId, Int64 warehouseId, Int64 orderlocId)
      {
            List<OrderListDisplayViewModel> newData = new List<OrderListDisplayViewModel>();

            try
            {
                List<OrderListDisplayViewModel> result = new List<OrderListDisplayViewModel>();

                IQueryable<OrderMaster> query = _context.OrderMasters.OrderByDescending(x => x.OrderDate).Where(y => y.StatusId != 4);
                //query = query.Where(x => x.UserId == userid);

                if (OrderStatusId == 0)
                {
                    query = query.Where(x => x.OrderStatusId != (int)OrderStatus.Completed && x.OrderStatusId != (int)OrderStatus.Cancelled);
                }
                else
                {
                    query = query.Where(x => x.OrderStatusId == OrderStatusId);
                }

                if (warehouseId != 0)
                {
                    query = query.Where(x => x.WareHouseId == warehouseId);
                }
                if (orderlocId != 0)
                {
                    if(orderlocId==5)
                    query = query.Where(x => (x.OrderlocatorId == orderlocId || x.OrderlocatorId == 10));
                    else
                        query = query.Where(x => x.OrderlocatorId == orderlocId);
                }
                //Search Condition
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    query = query.Where(p => (p.CustomerFullName.Contains(value) || p.OrderNo.Contains(value) || (p.CustomerId == _context.MasterUsers.FirstOrDefault(z => z.ShopName.ToLower().Contains(value.ToLower())).Id) ));
                }

                result = query.ToList().Select(ToOrderListDisplayViewModel).ToList();
                newData = result;


                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }


        public List<OrderListDisplayViewModel> GetAllOrdersBtCustomerId(int start, int length, string search, int filtercount, Int64 OrderStatusId, Int64 warehouseId, Int64 CustomerId)
        {
            List<OrderListDisplayViewModel> newData = new List<OrderListDisplayViewModel>();

            try
            {
                List<OrderListDisplayViewModel> result = new List<OrderListDisplayViewModel>();

                IQueryable<OrderMaster> query = _context.OrderMasters.OrderByDescending(x => x.OrderDate).Where(y => y.StatusId != 4 && y.CustomerId== CustomerId);
                //query = query.Where(x => x.UserId == userid);

                if (OrderStatusId == 0)
                {
                    query = query.Where(x => x.OrderStatusId != (int)OrderStatus.Completed && x.OrderStatusId != (int)OrderStatus.Cancelled);
                }
                else
                {
                    query = query.Where(x => x.OrderStatusId == OrderStatusId);
                }

                if (warehouseId != 0)
                {
                    query = query.Where(x => x.WareHouseId == warehouseId);
                }

                //Search Condition
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    query = query.Where(p => (p.CustomerFullName.Contains(value) || p.OrderNo.Contains(value) || (p.CustomerId == _context.MasterUsers.FirstOrDefault(z => z.ShopName.ToLower().Contains(value.ToLower())).Id)));
                }

                result = query.ToList().Select(ToOrderListDisplayViewModel).ToList();
                newData = result;

                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }


        public OrderListDisplayViewModel ToOrderListDisplayViewModel(OrderMaster orderMaster)
        {
            try
            {
                var bold = "";
                var colord = "Black";
                if (orderMaster.DueDate != null)
                {
                    orderMaster.DeliveryDate = orderMaster.DueDate.ToString();
                    orderMaster.DeliveryDate = Convert.ToDateTime(orderMaster.DeliveryDate).ToString("dd,MMMM,yyyy");
                    if (Convert.ToDateTime(orderMaster.DueDate) <= DateTime.Now.Date.AddDays(+14))
                    {
                        colord = "Blue";
                        bold = "bold";
                    }
                    if(Convert.ToDateTime(orderMaster.DueDate) <= DateTime.Now.Date.AddDays(+1))
                    {
                        colord = "red";
                        bold = "bold";
                    }
                    if (Convert.ToDateTime(orderMaster.DueDate) < DateTime.Now.Date)
                    {
                        colord = "Black";
                        bold = "Normal";
                    }
                }

                //if(orderMaster.DeliveryDate==DateTime.UtcNow.ToShortDateString())

                var color = "";
                var colorpo = "";
                var colorpay = "";
                var coloronlne = "#333333";
                if ((int)orderMaster.OrderStatusId == 1)
                {
                    color = "#3c8dbc"; //status new, color blue
                }
                else if ((int)orderMaster.OrderStatusId == 2)
                {
                    color = "#09c977"; //status In Progress, color Green
                }
                else if ((int)orderMaster.OrderStatusId == 3)
                {
                    color = "#ff0000"; //status In Payment Failed, color red
                }
                else if ((int)orderMaster.OrderStatusId == 4)
                {
                    color = "#229954"; //status Completed, color Dark-Green
                }
                else if ((int)orderMaster.OrderStatusId == 5)
                {
                    color = "#C98209"; //status Cancelled, color Orange
                }
                else if ((int)orderMaster.OrderStatusId == 6)
                {
                    color = "#d4bf29"; //status Dispatched, color yellow
                }
                else if ((int)orderMaster.OrderStatusId == 7)
                {
                    color = "#795C32"; //status Partially-Dispatched, color Light Brown
                }
                else if ((int)orderMaster.OrderStatusId == 8)
                {
                    color = "#6da56b"; //status Awaiting Confirmation, color Light Green
                }
                else if ((int)orderMaster.OrderStatusId == 9)
                {
                    color = "#FF3F3F"; //status In-Stock, color Red Orange
                }

                var POPlaced = "";

                if (orderMaster.IsPOPlaced == true)
                {
                    POPlaced = "YES";
                    colorpo = "#000000";
                }
                else
                {
                    POPlaced = "NO";
                    colorpo = "#ba3f38";
                }
                string WareHouse = "";
                MasterUser Customer = _context.MasterUsers.Where(x => x.Id == orderMaster.CustomerId).FirstOrDefault();
                try
                {
                    WareHouse = _context.WareHouses.Where(x => x.Id == orderMaster.WareHouseId && x.StatusId != 4).FirstOrDefault().WareHouseName;
                }
                catch
                {
                }

                var TotalAmountString = "";
                if (orderMaster.CurrencyName == "EURO")
                {
                    TotalAmountString = "€ " + orderMaster.TotalAmount.ToString();
                }
                else if (orderMaster.CurrencyName == "GBP")
                {
                    TotalAmountString = "£ " + orderMaster.TotalAmount.ToString();
                }
                else
                {
                    TotalAmountString = "$ " + orderMaster.TotalAmount.ToString();
                }

                string Notes = "";
                if (!String.IsNullOrEmpty(orderMaster.OrderNote))
                {
                    Notes += "<span class='tooltip' style='opacity:1;display:inline;float:left;'><i class='fa fa-info-circle fa-lg' style='color:red'></i> <span class='tooltiptext tooltip-left'>" + orderMaster.OrderNote + "</span> </span>";
                }
          

                if (!String.IsNullOrEmpty(orderMaster.SystemNotes))
                {
                    if (!String.IsNullOrEmpty(Notes))
                        Notes += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    Notes += "<span class='tooltip' style='opacity:1;display:inline;float:right;'><i class='fa fa-comments fa-lg' style='color:red'></i> <span class='tooltiptext tooltip-right'>" + orderMaster.SystemNotes + "</span> </span>";
                }
                string ispay = null;
                if (orderMaster.Isdeposit == true)
                {
                    ispay = "YES";
                    colorpay = "#229954";
                }
                else
                {
                    ispay = "NO";
                    colorpay = "#ff0000";
                }
                if(orderMaster.OrderlocatorId==10)
                {
                    coloronlne = "#0000FF"; //status new, color blue

                }
                OrderViewModel ab = new OrderViewModel();
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
                return new OrderListDisplayViewModel()
                {
                    CustomerFullName = orderMaster.CustomerFullName,
                    ShopName = Customer.ShopName,
                    CurrencyName = orderMaster.CurrencyName,
                    MobileNo = orderMaster.MobileNo,
                    ZipCode = orderMaster.ZipCode,
                    EmailId = orderMaster.EmailId,
                    WareHouseName = WareHouse,
                    OrderStatusId = (OrderStatus)orderMaster.OrderStatusId,
                    //OrderNo = orderMaster.OrderNo,
                    OrderNo = "<span style='color: " + coloronlne + ";'>" + orderMaster.OrderNo + "</span>",
                    OrderDate = orderMaster.OrderDate.ToShortDateString(),
                    WearDate = (orderMaster.WearDate.HasValue) ? orderMaster.WearDate.Value.ToShortDateString() : "",
                    UserSelectDeliveryDate = (orderMaster.UserSelectDeliveryDate.HasValue) ? orderMaster.UserSelectDeliveryDate.Value.ToShortDateString() : "",
                    BridesName = orderMaster.BridesName,
                    DeliveryDate = "<span style='color: " + colord + ";font-weight:"+ bold + ";'>" + orderMaster.DeliveryDate + "</span>",
                    Amount = orderMaster.Amount,
                    TotalAmountString = TotalAmountString,
                    //IsPOPlaced = POPlaced,
                    IsPOPlaced = "<span style='color:white;background: " + colorpo + ";'>" + POPlaced + "</span>",
                    TotalAmount = orderMaster.TotalAmount,
                    //OrderNotes = "<span class='tooltip' style='opacity:1;'><i class='fa fa-info-circle fa-lg'></i> <span class='tooltiptext tooltip-left'>" + orderMaster.OrderNote + "</span> </span>",
                    OrderNotes = Notes,
                    TotalProducts = orderMaster.TotalProduct,
                    IsPayment = "<span style='color:white;background: " + colorpay + ";'>" + ispay + "</span>", //ispay,
                    DispatchOrder = "<a href='/Order/DispatchOrder?Id=" + orderMaster.Id + "'title='Dispatch Order'>Dispatch Order</a>",
                    OrderStatus = "<span style='color:white;background: " + color + "; font-weight:bold;border-radius: 4px;padding: 3px;'>" + Regex.Replace(((OrderStatus)orderMaster.OrderStatusId).ToString(), "([A-Z])([a-z]*)", " $1$2") + "</span>",
                    Edit = "<a href='/Order/AddNewOrder?id=" + orderMaster.Id + "' title='Edit'><img src='/Content/img/editicon.png'style='height: 15px;'/></a>",
                    Delete = "<a href='/Order/DeleteOrder?id=" + orderMaster.Id + "' title='Delete'  onclick='return Confirmation();'><img src='/Content/img/deleteicon.png'style='height: 15px;'/></a>",
                    PrintPreview = "<a href='/Order/GetPrintPreview?id=" + orderMaster.Id + "' title='Print Preview' target='_blank'><img src='/Content/img/PrintPreview.png'style='height: 15px;'/></a>",
                    //RoleId = ab.ToString(),
                    IsActive = ab.IsAdmin,
                  
                };
            }
            catch (Exception ex)
            {
                return new OrderListDisplayViewModel();
            }
        }

        public void DeleteOrder(Int64 id, Int64 ModifiedById)
        {
            //Order Master 
            OrderMaster Master = _context.OrderMasters.Where(x => x.Id == id).FirstOrDefault();
            Master.StatusId = 4;
            Master.ModificationDate = DateTime.UtcNow;
            Master.ModifiedById = ModifiedById;

            //Order Details
            List<OrderDetail> DetailsList = _context.OrderDetails.Where(x => x.OrderMasterId == id && x.StatusId != 4).ToList();
            foreach (var List in DetailsList)
            {
                StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "Order" && x.SizeUK == List.SizeUK && x.ColourId == List.ColourId && x.ProductId == List.ProductId && x.WareHouseId == Master.WareHouseId && x.StatusId != 4).FirstOrDefault();
                if (StockDetail != null)
                {
                    Int64 localQty = List.Qty;
                    //StockDetail.ReceivedQty = StockDetail.ReceivedQty + localQty;
                    StockDetail.Qty = StockDetail.Qty - localQty;
                    StockDetail.ModificationDate = DateTime.UtcNow;
                }

                List.StatusId = 4;
                List.ModificationDate = DateTime.UtcNow;
                List.ModifiedById = ModifiedById;
            }

           
            _context.SaveChanges();
        }

        public OrderViewModel GetOrderById(Int64 Id)
        {
            OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Id && x.StatusId != 4).FirstOrDefault();

            if (Master != null)
            {
                return ToOrderViewModel(Master);
            }
            else
            {
                return new OrderViewModel();
            }
        }

        public OrderViewModel GetOrderByOrderNo(string OrderNo)
        {
            OrderMaster Master = _context.OrderMasters.Where(x => x.OrderNo == OrderNo && x.StatusId != 4).FirstOrDefault();

            if (Master != null)
            {
                return ToOrderViewModel(Master);
            }
            else
            {
                return new OrderViewModel();
            }
        }

        public OrderViewModel GetPrintPreview(Int64 Id)
        {
            OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Id).FirstOrDefault();
            if (Master != null)
            {
                return ToOrderViewModel(Master);
            }
            else
            {
                return new OrderViewModel();
            }
        }

        public OrderViewModel ToOrderViewModel(OrderMaster Master)
        {
            try
            {
                string sts = "";
                var color = "";
                if ((int)Master.OrderStatusId == 1)
                {
                    color = "#3c8dbc"; //status new, color blue
                }
                else if ((int)Master.OrderStatusId == 2)
                {
                    color = "#09c977"; //status In Progress, color Green
                }
                else if ((int)Master.OrderStatusId == 3)
                {
                    color = "#ff0000"; //status In Payment Failed, color red
                }
                else if ((int)Master.OrderStatusId == 4)
                {
                    color = "#229954"; //status Completed, color Dark-Green
                }
                else if ((int)Master.OrderStatusId == 5)
                {
                    color = "#C98209"; //status Cancelled, color Orange
                }
                else if ((int)Master.OrderStatusId == 6)
                {
                    color = "#d4bf29"; //status Dispatched, color yellow
                }
                else if ((int)Master.OrderStatusId == 7)
                {
                    color = "#795C32"; //status Partially-Dispatched, color Light Brown
                }
                else if ((int)Master.OrderStatusId == 8)
                {
                    color = "#6da56b"; //status Awaiting Confirmation, color Light Green
                }
                else if ((int)Master.OrderStatusId == 9)
                {
                    color = "#FF3F3F"; //status In-Stock, color Red Orange
                }

                //For customer Details from order table
                CustomerViewModel Customer = new CustomerViewModel();
                Customer.CustomerFullName = Master.CustomerFullName;
                Customer.CurrencyName = Master.CurrencyName;
                Customer.MobileNo = Master.MobileNo;
                Customer.EmailId = Master.EmailId;
                Customer.Id = Master.CustomerId;
                Customer.ShippingState = Master.ShippingState;

                //For customer Basic Details from customer table

                MasterUser customerDetails = _context.MasterUsers.Where(x => x.Id == Master.CustomerId).FirstOrDefault();
                Customer.CompanyName = customerDetails.CompanyName;
                Customer.ShopName = customerDetails.ShopName;
                Customer.Town = _context.Cities.Where(x => x.Id == customerDetails.CityId && x.StatusId != 4).Select(x => x.CityName).FirstOrDefault();
                Customer.CountryId = customerDetails.CountryId;
                Customer.Country = _context.Countries.Where(x => x.Id == customerDetails.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                Customer.State = _context.States.Where(x => x.Id == customerDetails.StateId && x.StatusId != 4).Select(x => x.Statename).FirstOrDefault();
                Customer.TelephoneNo = customerDetails.TelephoneNo;
                Customer.CompanyTaxId = customerDetails.CompanyTaxId;
                Customer.AddressLine1 = customerDetails.AddressLine1;
                Customer.AddressLine2 = customerDetails.AddressLine2;
                Customer.City = customerDetails.City;
                Customer.ZipCode = customerDetails.ZipCode;
                Customer.DistributionPointId = customerDetails.DistributionPointId;
                Customer.DistributionPoint = (DistributionPoint)customerDetails.DistributionPointId;
                Customer.StateName = customerDetails.StateName;
                Customer.Tax = customerDetails.TaxEnabled;


                sts = Regex.Replace(((OrderStatus)Master.OrderStatusId).ToString(), "([A-Z])([a-z]*)", " $1$2");

                List<ProductListModel> ProductDetailsList = GetProductDetailList(Master.Id);


                OrderViewModel Model = new OrderViewModel()
                {
                    Id = Master.Id,
                    OrderNo = Master.OrderNo,
                    OrderDate = (Master.OrderDate).ToString("MM/dd/yyyy"),
                    OrderDateString = Master.OrderDate.ToString("dd-MM-yyyy"),
                    WearDate = (Master.WearDate.HasValue) ? (Master.WearDate.Value).ToString("MM/dd/yyyy") : "",
                    UserSelectDeliveryDate = (Master.UserSelectDeliveryDate.HasValue) ? (Master.UserSelectDeliveryDate.Value).ToString("MM/dd/yyy") : "",
                     UserSelectDeliveryDateString= (Master.UserSelectDeliveryDate.HasValue) ? (Master.UserSelectDeliveryDate.Value).ToString("dd-MM-yyyy") : "",
                    BridesName = Master.BridesName,
                    PurchaseOrderNumber = Master.PONumber,
                    DeliveryDate = Master.DeliveryDate,
                    Amount = Master.Amount,
                    TotalAmount = Master.TotalAmount,
                    TotalProducts = Master.TotalProduct,
                    ProductList = ProductDetailsList,
                    OrderStatusId = (OrderStatus)Master.OrderStatusId,
                    IsPOPlaced = Master.IsPOPlaced,
                    Currency = Master.CurrencyName,
                    CustomerModel = Customer,
                    OrderNote = Master.OrderNote,
                    SystemNotes = Master.SystemNotes,
                    ShippingCharge = Master.ShippingCharge,
                    Tax = Master.Tax,
                    OrderStatus = sts,
                    StatusColor = color,
                    Rushfee=Master.Rushfee,
                    OrderlocatorId=Master.OrderlocatorId,
                    Extracharges=Master.Extracharges,
                    IsPayment=Master.Isdeposit,
                    DistributionPoint= Master.WareHouseId
                };

                return Model;
            }
            catch (Exception ex)
            {
                return new OrderViewModel();
            }
        }

        public List<ProductListModel> GetProductDetailList(Int64 MasterId)
        {
            try
            {
                //List<OrderDetail> List = _context.OrderDetails.Where(x => x.OrderMasterId == MasterId).GroupBy(x => new OrderDetail() {ColourId = x.ColourId, Id = x.ProductId }).ToList();
                List<OrderDetailViewModel> List = _context.OrderDetails.Where(x => x.OrderMasterId == MasterId && x.StatusId != 4)
                                    .GroupBy(x => new { x.ColourId, x.ProductId })
                                    .Select(y => new OrderDetailViewModel()
                                    {
                                        ColourId = y.Key.ColourId,
                                        ProductId = y.Key.ProductId
                                    }).ToList();

                List<ProductListModel> ProductDetailList = new List<ProductListModel>();

                foreach (var data in List)
                {
                    ProductViewModel Product = _context.Products.Where(x => x.Id == data.ProductId && x.StatusId != 4)
                                                .Select(x => new ProductViewModel()
                                                {
                                                    Id = x.Id,
                                                    ProductName = x.ProductName,
                                                    PriceEURO = x.PriceEURO,
                                                    PriceGBP = x.PriceGBP,
                                                    PriceUSD = x.PriceUSD,
                                                }).FirstOrDefault();
                    List<SizeViewModel> sizemodel = _context.OrderDetails.Where(x => x.ProductId == data.ProductId && x.ColourId == data.ColourId && x.OrderMasterId == MasterId && x.StatusId != 4)
                                                    .Select(y => new SizeViewModel()
                                                    {
                                                        SizeUK = y.SizeUK,
                                                        Qty = y.Qty,
                                                        OrderPrice = y.OrderPrice,
                                                    }).ToList();

                    Product.ColourName = _context.Colours.Where(x => x.Id == data.ColourId && x.StatusId != 4).Select(x => x.ColourName).FirstOrDefault();
                    Product.ColourId = data.ColourId;

                    ProductListModel model = new ProductListModel();
                    model.Product = Product;
                    model.SizeModel = sizemodel;

                    ProductDetailList.Add(model);
                }

                return ProductDetailList;
            }
            catch (Exception ex)
            {
                return new List<ProductListModel>();
            }
        }

        public List<ColourViewModel> GetColoursForProduct(Int64 ProductId)
        {
            List<ColourViewModel> colorlist = new List<ColourViewModel>();

            try
            {
                var ColorIds = _context.Products.Where(x => x.Id == ProductId && x.StatusId != 4).Select(x => x.SeletedColorIds).FirstOrDefault();
                Int64[] colourIdList = StringToIntArray(ColorIds);
                colorlist = _context.Colours.Where(x => colourIdList.Contains(x.Id)).ToList().Select(ToColourViewModel).ToList();
                return colorlist;
            }
            catch
            {
                return null;
            }
        }

      

        private static Int64[] StringToIntArray(string myNumbers)
        {
            List<Int64> myIntegers = new List<Int64>();
            Array.ForEach(myNumbers.Split(",".ToCharArray()), s => { Int64 currentInt; if (Int64.TryParse(s, out currentInt)) myIntegers.Add(currentInt); });
            return myIntegers.ToArray();
        }

        public ColourViewModel ToColourViewModel(Colour colour)
        {
            // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();

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
                Delete = "<a href='/Product/DeleteColour?id=" + colour.Id + "' title='Delete'  onclick='return Confirmation();' >Delete</a>"
            };
        }

        public bool DispatchOrder(OrderViewModel Model)
        {
            try
            {
                foreach (var data in Model.ProductList)
                {
                    foreach (var list in data.SizeModel)
                    {
                        OrderDetail Detail = _context.OrderDetails.Where(x => x.OrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.StatusId != 4).FirstOrDefault();
                        Detail.DispatchQty = list.DispatchQty;
                        Detail.ModifiedById = Model.CreatedById;
                        Detail.ModificationDate = DateTime.UtcNow;
                        _context.SaveChanges();
                    }
                }
                OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                Master.OrderStatusId = (int)Model.OrderStatusId;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<OrderDetailViewModel> GetDetailsListForDispatch(Int64 id)
        {
            List<OrderDetailViewModel> sizemodel = _context.OrderDetails.Where(x => x.OrderMasterId == id && x.StatusId != 4).ToList()
                                                            .Select(ToOrderDetailViewModel).ToList();
            return sizemodel;
        }

        public OrderDetailViewModel ToOrderDetailViewModel(OrderDetail data)
        {
            try
            {
                return new OrderDetailViewModel()
                {
                    Id = data.Id,
                    SizeUK = data.SizeUK,
                    Qty = data.Qty,
                    DispatchedQty = data.DispatchQty,
                    AvailableQty = GetAvailableQuantity(data),
                    ProductId = data.ProductId,
                    ProductName = GetProductnameById(data.ProductId),
                    ColourId = data.ColourId,
                    ColourName = GetColorNameById(data.ColourId),
                    SizeUS = (Convert.ToInt32(data.SizeUK) - 4).ToString(),
                    SizeEU = (Convert.ToInt32(data.SizeUK) + 28).ToString()
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailViewModel();
            }
        }

        public Int64 GetAvailableQuantity(OrderDetail data)
        {
            try
            {
                Int64 warehouseid = _context.OrderMasters.FirstOrDefault(x => x.Id == data.OrderMasterId).WareHouseId;

                string size = data.SizeUK.ToString();
                var FromProduct = _context.StockQuantities.Where(x => x.ProductId == data.ProductId && x.InventoryType=="Stock"  && x.ColourId == data.ColourId && x.SizeUK == size && x.WareHouseId == warehouseid && x.StatusId != 4 )
                                .GroupBy(y => y.ProductId).Select(y => new
                                {
                                    ReceivedQty = y.Sum(z => z.ReceivedQty),
                                    //DispatchedQty = y.Sum(z => z.DispatchedQty)
                                }).ToList();

                Int64 AvailableQty = FromProduct[0].ReceivedQty;    // -FromProduct[0].DispatchedQty;
                return AvailableQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DispatchOrderDetails(OrderViewModel Model)
        {
            try
            {
                if (Model.IsDispatchedAll == true)
                {
                    bool chk = true;

                    #region: Dispatch All
                    foreach (var list in Model.DetailsList)
                    {
                        OrderDetail Detail = _context.OrderDetails.Where(x => x.Id == list.Id && x.StatusId != 4).FirstOrDefault();

                        //Update Stock Quantity
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "Order" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();
                        StockQuantity StockDetailPo = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();

                        StockQuantity StockQty = _context.StockQuantities.Where(x => x.InventoryType == "Stock" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();


                        if (StockDetail != null)
                        {
                            Int64 localQty = (Detail.DispatchQty == Detail.Qty) ? 0 : (Detail.Qty - Detail.DispatchQty);

                            //If Stock Detail is null
                        
                                Int64 remainingQtyForStock = localQty; //- StockDetailPo.ReceivedQty;
                            if (StockQty != null)
                            {
                                //Now in Update In StockQuantity Table
                                if (StockQty.ReceivedQty >= remainingQtyForStock)
                                {
                                    StockQty.ReceivedQty = StockQty.ReceivedQty - remainingQtyForStock;

                                    StockDetail.Qty = StockDetail.Qty - remainingQtyForStock;
                                    StockDetail.ModifiedById = Model.CreatedById;
                                    StockDetail.ModificationDate = DateTime.UtcNow;

                                    if (Detail != null)
                                    {
                                        Detail.DispatchQty = Detail.Qty;
                                        Detail.ModifiedById = Model.CreatedById;
                                        Detail.ModificationDate = DateTime.UtcNow;
                                    }
                                }


                                else
                                {
                                    Int64 remainingQtyForStock1 = localQty - StockQty.ReceivedQty;

                                    StockQty.ReceivedQty = StockQty.ReceivedQty - (localQty - remainingQtyForStock1);

                                    StockDetail.Qty = StockDetail.Qty - (localQty - remainingQtyForStock1);
                                    StockDetail.ModifiedById = Model.CreatedById;
                                    StockDetail.ModificationDate = DateTime.UtcNow;

                                    if (Detail != null)
                                    {
                                        Detail.DispatchQty = (list.DispatchedQty - remainingQtyForStock1);
                                        Detail.ModifiedById = Model.CreatedById;
                                        Detail.ModificationDate = DateTime.UtcNow;
                                    }
                                }

                            }
                            else
                            {
                                chk = false;
                            }

                            
                   
                        }
                       

                        _context.SaveChanges();
                    }

                    if (chk == true)
                    {
                        OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                        Master.OrderStatusId = (int)OrderStatus.Completed;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                        Master.OrderStatusId = (int)OrderStatus.PartiallyDispatched;
                        _context.SaveChanges();
                        return true;
                    }

                    #endregion
                }
                else
                {
                    bool chk2 = true;

                    #region : Dispatch By Given Quantity

                    foreach (var list in Model.DetailsList)
                    {
                        OrderDetail Detail = _context.OrderDetails.Where(x => x.Id == list.Id && x.StatusId != 4).FirstOrDefault();

                        //Update Stock Quantity
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "Order" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4 ).FirstOrDefault();
                        StockQuantity StockDetailPo = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();

                        StockQuantity StockQty = _context.StockQuantities.Where(x => x.InventoryType == "Stock" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();

                        if (StockDetail != null)
                        {
                          

                            Int64 localQty = (Detail.DispatchQty == Detail.Qty) ? 0 : (list.DispatchedQty - Detail.DispatchQty);

                            if (StockDetailPo == null)
                            {
                                Int64 remainingQtyForStock = localQty;  // -StockDetailPo.ReceivedQty;

                                //Now in Update In StockQuantity Table
                                if (StockQty != null)
                                {
                                    if (StockQty.ReceivedQty >= remainingQtyForStock)
                                    {
                                        StockQty.ReceivedQty = StockQty.ReceivedQty - remainingQtyForStock;

                                        StockDetail.Qty = StockDetail.Qty - remainingQtyForStock;
                                        StockDetail.ModifiedById = Model.CreatedById;
                                        StockDetail.ModificationDate = DateTime.UtcNow;

                                        if (Detail != null)
                                        {
                                            Detail.DispatchQty = list.DispatchedQty;
                                            Detail.ModifiedById = Model.CreatedById;
                                            Detail.ModificationDate = DateTime.UtcNow;
                                        }
                                    }
                                    else
                                    {
                                        chk2 = false;
                                    }
                                }
                                else
                                {
                                    chk2 = false;
                                }
                            }
                            else
                            {
                                if (StockDetailPo.ReceivedQty >= localQty)
                                {
                                    StockDetailPo.ReceivedQty = StockDetailPo.ReceivedQty - localQty;
                                    if (StockQty != null)
                                    {
                                        try
                                        {
                                            if (StockQty.ReceivedQty >= localQty)
                                            {
                                                StockQty.ReceivedQty = StockQty.ReceivedQty - localQty;
                                            }
                                            else
                                            {
                                                chk2 = false;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            MessageBox.Show(e.Message);
                                        }
                                    }
                                    else
                                    {
                                        chk2 = false;
                                    }
                                    StockDetail.Qty = StockDetail.Qty - localQty;
                                    StockDetail.ModifiedById = Model.CreatedById;
                                    StockDetail.ModificationDate = DateTime.UtcNow;

                                    if (Detail != null)
                                    {
                                        Detail.DispatchQty = list.DispatchedQty;
                                        Detail.ModifiedById = Model.CreatedById;
                                        Detail.ModificationDate = DateTime.UtcNow;
                                    }
                                }
                                else
                                {
                                    Int64 remainingQtyForStock = localQty - StockDetailPo.ReceivedQty;

                                    StockDetailPo.ReceivedQty = StockDetailPo.ReceivedQty - (localQty - remainingQtyForStock);

                                    StockDetail.Qty = StockDetail.Qty - (localQty - remainingQtyForStock);
                                    StockDetail.ModifiedById = Model.CreatedById;
                                    StockDetail.ModificationDate = DateTime.UtcNow;

                                    if (Detail != null)
                                    {
                                        Detail.DispatchQty = (list.DispatchedQty - remainingQtyForStock);
                                        Detail.ModifiedById = Model.CreatedById;
                                        Detail.ModificationDate = DateTime.UtcNow;
                                    }

                                    //Now in Update In StockQuantity Table
                                    if (StockQty != null)
                                    {
                                        if (StockQty.ReceivedQty >= localQty)
                                        {
                                            StockQty.ReceivedQty = StockQty.ReceivedQty - remainingQtyForStock;

                                            StockDetail.Qty = StockDetail.Qty - remainingQtyForStock;
                                            StockDetail.ModifiedById = Model.CreatedById;
                                            StockDetail.ModificationDate = DateTime.UtcNow;

                                            if (Detail != null)
                                            {
                                                Detail.DispatchQty = list.DispatchedQty;
                                                Detail.ModifiedById = Model.CreatedById;
                                                Detail.ModificationDate = DateTime.UtcNow;
                                            }

                                        }
                                        else
                                        {
                                            Int64 remainingQtyForStock1 = localQty - StockQty.ReceivedQty;

                                            StockQty.ReceivedQty = StockQty.ReceivedQty - (localQty - remainingQtyForStock1);

                                            StockDetail.Qty = StockDetail.Qty - (localQty - remainingQtyForStock1);
                                            StockDetail.ModifiedById = Model.CreatedById;
                                            StockDetail.ModificationDate = DateTime.UtcNow;

                                            if (Detail != null)
                                            {
                                                Detail.DispatchQty = (list.DispatchedQty - remainingQtyForStock1);
                                                Detail.ModifiedById = Model.CreatedById;
                                                Detail.ModificationDate = DateTime.UtcNow;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        chk2 = false;
                                    }

                                }
                            }

                          
                        }

                        _context.SaveChanges();
                    }

                    if (chk2 == true)
                    {
                        OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                        Master.OrderStatusId = (int)Model.OrderStatusId;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        OrderMaster Master = _context.OrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                        Master.OrderStatusId = (int)OrderStatus.PartiallyDispatched;
                        _context.SaveChanges();
                        return true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region : Purchase Order

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

        public bool SavePurchaseOrder(PurchaseOrderViewModel Model)
        {
            try
            {
              
                var check = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id).FirstOrDefault();
                if (check != null)
                {
                    return false;
                }
                else
                {
                   

                    var chk = _context.OrderMasters.Where(x => x.OrderNo == Model.OrderRefrence && x.StatusId != 4).FirstOrDefault();
                    if (chk != null)
                    {
                        chk.IsPOPlaced = true;
                    }

                    var date = DateTime.UtcNow.ToString("MM/dd/yyyy");

                    if (Model.OrderDate == null)
                    {
                        Model.OrderDate = date;
                    }

                    PurchaseOrderMaster Master = new PurchaseOrderMaster()
                    {
                        SupplierId = Model.CustomerModel.Id,
                        WareHouseId = Model.WareHouseId,
                        SupplierName = Model.CustomerModel.CustomerFullName,
                        //ZipCode = Model.CustomerModel.ZipCode,
                        //CurrencyName = Model.CustomerModel.CurrencyName.Trim(),
                        MobileNo = Model.CustomerModel.MobileNo,
                        //ShippingCharge = Model.ShippingCharge,
                        //TaxEnable = Model.TaxEnable,
                        //Tax = Model.Tax,
                        OrderNote = Model.OrderNote,
                        OrderRefrence = Model.OrderRefrence,
                        EmailId = Model.CustomerModel.EmailId,
                        POStatusId = 1,
                        //OrderNo = Model.OrderNo,
                        OrderDate = Convert.ToDateTime(DateTime.ParseExact(Model.OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)),
                        TotalProduct = Model.ProductList.Count,
                        Address = Model.CustomerModel.AddressLine1,
                        DueDate = (!String.IsNullOrEmpty(Model.DeliveryDate)) ? Convert.ToDateTime(DateTime.ParseExact(Model.DeliveryDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                        //ShippingState = Model.CustomerModel.ShippingState,
                        // Amount = Model.Amount,
                        // TotalAmount = TotalAmount,
                        StatusId = 1,
                        CreationDate = DateTime.UtcNow,
                        CreatedById = Model.CreatedById,

                    };
                    _context.PurchaseOrderMasters.Add(Master);
                    _context.SaveChanges();


                    //Save Details
                    foreach (var data in Model.ProductList)
                    {
                        foreach (var list in data.SizeModel)
                        {
                            if (data.Product.ColourId > 0)
                                data.Product.ColourName = _context.Colours.Where(x => x.Id == data.Product.ColourId).Select(x => x.ColourName).FirstOrDefault();

                            PurchaseOrderDetail Details = new PurchaseOrderDetail()
                            {
                                PurchaseOrderMasterId = Master.Id,
                                ProductId = data.Product.Id,
                                //SizeId
                                // OrderPrice = data.Product.OrderPrice,
                                SizeUK = list.SizeUK,
                                ColourId = data.Product.ColourId,
                                Qty = list.Qty,
                                StatusId = 1,
                                ReceivedQty = 0,
                                CreatedById = Model.CreatedById,
                                CreationDate = DateTime.UtcNow
                            };
                            _context.PurchaseOrderDetails.Add(Details);
                            _context.SaveChanges();
                        }
                    }
                 
                    foreach (var data in Model.ProductList)
                    {
                        foreach (var list in data.SizeModel)
                        {
                            //StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "PO" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == Model.WareHouseId && x.StatusId != 4).FirstOrDefault();

                            if (data.Product.ColourId > 0)
                                data.Product.ColourName = _context.Colours.Where(x => x.Id == data.Product.ColourId).Select(x => x.ColourName).FirstOrDefault();



                            //  if (StockDetail == null)

                            StockQuantity SQ = new StockQuantity()
                            {
                                ReferenceId = Master.Id,
                                InventoryType = "PO",
                                SizeUK = list.SizeUK,
                                ProductId = data.Product.Id,
                                ColourId = Convert.ToInt64(data.Product.ColourId),
                                WareHouseId = Model.WareHouseId,
                                Qty = list.Qty,
                                CreatedById = Model.CreatedById,
                                CreationDate = DateTime.UtcNow,
                            };
                                    _context.StockQuantities.Add(SQ);
                            
                            
                                _context.SaveChanges();
                            
                        }
                          }
                    foreach (var data in Model.ProductList)
                    {
                        foreach (var list in data.SizeModel)
                        {
                            StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "Stock" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == Model.WareHouseId && x.StatusId != 4).FirstOrDefault();

                            if (data.Product.ColourId > 0)
                                data.Product.ColourName = _context.Colours.Where(x => x.Id == data.Product.ColourId).Select(x => x.ColourName).FirstOrDefault();



                            if (StockDetail == null)
                            {

                                StockQuantity SQ = new StockQuantity() {
                                  InventoryType = "Stock",
                                SizeUK = list.SizeUK,
                                ProductId = data.Product.Id,
                              ColourId = Convert.ToInt64(data.Product.ColourId),
                                WareHouseId = Model.WareHouseId,
                                CreatedById = Model.CreatedById,
                               CreationDate = DateTime.UtcNow,
                            };
                                _context.StockQuantities.Add(SQ);


                                _context.SaveChanges();
                                    }
                        }
                    }
                            Model.Id = Master.Id;
                    Model.DistributionPoint = Model.WareHouseId;

               
                }

                MasterUser user = _context.MasterUsers.Where(x => x.Id == Model.CustomerModel.Id).FirstOrDefault();
                Model.CustomerModel.AddressLine1 = user.AddressLine1;
                Model.CustomerModel.CurrencyName = ((Currency)user.CurrencyId).ToString();
                Model.CustomerModel.Country = _context.Countries.Where(x => x.Id == user.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                Model.SupplierName = user.FirstName + " " + user.LastName;
                Model.WareHouseName = _context.WareHouses.FirstOrDefault(x => x.Id == Model.WareHouseId).WareHouseName;

                if (Model.SendMail == true)
                {
                    SendMail(Model);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private string GetUniqueKey()
        {
            int maxSize = 10;
            int minSize = 10;
            char[] chars = new char[62];
            string a;
            //a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            a = "abcdefghijklmnpqrstuvwxyz123456789";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }

        public bool UpdatePurchaseOrder(PurchaseOrderViewModel Model)
        {
            try
            {
                

                var date = DateTime.UtcNow.ToString("MM/dd/yyyy");

                if (Model.OrderDate == null)
                {
                    Model.OrderDate = date;
                }

                PurchaseOrderMaster Master = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                if (Master != null)
                {
                    if ((int)Model.PurchaseOrderStatusId == 0)
                    {
                        Model.PurchaseOrderStatusId = (PurchaseOrderStatus)Master.POStatusId;
                    }

                 
                    Master.MobileNo = Model.CustomerModel.MobileNo;
                    
                    Master.EmailId = Model.CustomerModel.EmailId;
                    Master.OrderRefrence = Model.OrderRefrence;
                 
                    Master.OrderDate = Convert.ToDateTime(DateTime.ParseExact(Model.OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    Master.DeliveryDate = Model.DeliveryDate;
                    Master.TotalProduct = Model.ProductList.Count;
                    Master.OrderNote = Model.OrderNote;
                    Master.POStatusId = (int)Model.PurchaseOrderStatusId;
                    Master.WareHouseId = Model.WareHouseId;
                 
                    Master.ModificationDate = DateTime.UtcNow;
                    Master.ModifiedById = Model.CreatedById;
                }
                _context.SaveChanges();
                
                Model.DistributionPoint = Master.WareHouseId;

                //Update Stock Inventory
                UpdatePOInventory(Model);

                //Save Details
                List<PurchaseOrderDetail> Details = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == Model.Id && x.StatusId != 4).ToList();
                foreach (var data in Details)
                {
                    data.StatusId = 4;
                }
                _context.SaveChanges();

                foreach (var data in Model.ProductList)
                {
                    foreach (var list in data.SizeModel)
                    {
                        PurchaseOrderDetail Detail = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id).FirstOrDefault();
                        if (Detail == null)
                        {
                            PurchaseOrderDetail DetailsModel = new PurchaseOrderDetail()
                            {
                                PurchaseOrderMasterId = Model.Id,
                                ProductId = data.Product.Id,
                                //SizeId
                                //OrderPrice = data.Product.OrderPrice,
                                SizeUK = list.SizeUK,
                                ColourId = data.Product.ColourId,
                                Qty = list.Qty,
                                StatusId = 1,
                                CreatedById = Model.CreatedById,
                                CreationDate = DateTime.UtcNow
                            };
                            _context.PurchaseOrderDetails.Add(DetailsModel);
                        }
                        else
                        {
                            Detail.Qty = list.Qty;
                            Detail.StatusId = 1;
                            Detail.ModifiedById = Model.CreatedById;
                            Detail.ModificationDate = DateTime.UtcNow;
                        }
                        _context.SaveChanges();
                    }
                }




                MasterUser user = _context.MasterUsers.Where(x => x.Id == Model.CustomerModel.Id).FirstOrDefault();
                Model.CustomerModel.AddressLine1 = user.AddressLine1;
                Model.CustomerModel.Country = _context.Countries.Where(x => x.Id == user.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                Model.CustomerModel.CurrencyName = ((Currency)user.CurrencyId).ToString();
                Model.SupplierName = user.FirstName + " " + user.LastName;
                if (Model.SendMail == true)
                {
                    SendMail(Model);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ReceivePO(PurchaseOrderViewModel Model)
        {
            try
            {
                foreach (var data in Model.ProductList)
                {

                    foreach (var list in data.SizeModel)
                    {
                        PurchaseOrderDetail Detail = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.StatusId != 4).FirstOrDefault();

                        Detail.ReceivedQty = list.ReceivedQty;
                        Detail.ModifiedById = Model.CreatedById;
                        Detail.ModificationDate = DateTime.UtcNow;

                        _context.SaveChanges();
                    }

                }
                PurchaseOrderMaster Master = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                Master.POStatusId = (Int64)Model.PurchaseOrderStatusId;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<OrderListDisplayViewModel> GetAllPurchaseOrders(int start, int length, string search, int filtercount, Int64 POStatusId, Int64 cusid, Int64 warehouseId)
        {
            List<OrderListDisplayViewModel> newData = new List<OrderListDisplayViewModel>();
            #region Old COde


            

            #endregion
            try
            {
                List<OrderListDisplayViewModel> result = new List<OrderListDisplayViewModel>();

                IQueryable<PurchaseOrderMaster> query = _context.PurchaseOrderMasters.OrderByDescending(x => x.OrderDate).Where(y => y.StatusId != 4);
                //query = query.Where(x => x.UserId == userid);

                if (POStatusId == 0)
                {
                    query = query.Where(x => x.POStatusId != (int)PurchaseOrderStatus.Completed && x.POStatusId != (int)OrderStatus.Cancelled);
                }
                else
                {
                    query = query.Where(x => x.POStatusId == POStatusId);
                }

                if (warehouseId != 0)
                {
                    query = query.Where(x => x.WareHouseId == warehouseId);
                }
                if (cusid != 0)
                {
                    query = query.Where(x => x.SupplierId == cusid);
                }
                //Search Condition
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    query = query.Where(p => (p.SupplierName.Contains(value) || p.OrderRefrence.Contains(value)));
                }

                result = query.ToList().Select(ToPurchaseOrderListDisplayViewModel).ToList();
                newData = result;
            }
            catch (Exception ex)
            {

            }

            return newData;
        }

        public OrderListDisplayViewModel ToPurchaseOrderListDisplayViewModel(PurchaseOrderMaster POMaster)
        {
          var  colord = "black";
            var bold = "";
            if (POMaster.DueDate != null)
            {
                POMaster.DeliveryDate = POMaster.DueDate.ToString();
                POMaster.DeliveryDate = Convert.ToDateTime(POMaster.DeliveryDate).ToString("dd,MMMM,yyyy");
                if (Convert.ToDateTime(POMaster.DueDate) <= DateTime.Now.Date.AddDays(+14))
                {
                    colord = "Blue";
                    bold = "bold";
                }
                if (Convert.ToDateTime(POMaster.DueDate) <= DateTime.Now.Date.AddDays(+1))
                {
                    colord = "red";
                    bold = "bold";
                }
                if (Convert.ToDateTime(POMaster.DueDate) < DateTime.Now.Date)
                {
                    colord = "Black";
                    bold = "normal";
                }
            }
            var color = "";
            string WareHouse = "", shopName = "";

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

            if (POMaster.WareHouseId > 0)
            {
                WareHouse = _context.WareHouses.FirstOrDefault(x => x.Id == POMaster.WareHouseId).WareHouseName;
            }

            try
            {
                var orderDetails = _context.OrderMasters.Join(_context.MasterUsers, s=>s.CustomerId, d=>d.Id, (s,d)=>new { s, d })
                                   .Where(x => x.s.OrderNo.ToLower() == POMaster.OrderRefrence.ToLower()).FirstOrDefault();
                if (orderDetails != null)
                {
                    shopName = orderDetails.d.ShopName;
                }
            }
            catch (Exception ex)
            {  }
            PurchaseOrderViewModel ab = new PurchaseOrderViewModel();
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
                DeliveryDate = "<span style='color: " + colord + "; font-weight: "+bold+";'>" + POMaster.DeliveryDate + "</span>",
                //Amount = POMaster.Amount,
                TotalAmount = POMaster.TotalAmount,
                TotalProducts = POMaster.TotalProduct,
                Receive = "<a href='/Order/ReceivePOCounts?Id=" + POMaster.Id + "'title='Receive'>Receive</a>",
                OrderStatus = "<span style='color:white;background: " + color + "; font-weight:bold;border-radius: 4px;padding: 3px;'>" + (PurchaseOrderStatus)POMaster.POStatusId + "</span>",
                Edit = "<a href='/Order/AddNewPurchaseOrder?id=" + POMaster.Id + "' title='Edit'><img src='/Content/img/editicon.png'style='height: 15px;'/></a>",
                Delete = "<a href='/Order/DeletePurchaseOrder?id=" + POMaster.Id + "' title='Delete' onclick='return Confirmation();'><img src='/Content/img/deleteicon.png'style='height: 15px;'/></a>",
                PrintPreview = "<a href='/Order/GetPurchaseOrderPrintPreview?id=" + POMaster.Id + "' title='Print Preview' ><img src='/Content/img/PrintPreview.png'style='height: 15px;'/></a>",
                IsActive = ab.IsAdmin,
            };
        }

        public void DeletePurchaseOrder(Int64 id, Int64 ModifiedById)
        {
            //PO Master
            PurchaseOrderMaster POMaster = _context.PurchaseOrderMasters.Where(x => x.Id == id).FirstOrDefault();
            POMaster.StatusId = 4;
            POMaster.ModificationDate = DateTime.UtcNow;
            POMaster.ModifiedById = ModifiedById;

            //PO Details
            List<PurchaseOrderDetail> PODetailsList = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == id && x.StatusId != 4).ToList();
            foreach (var List in PODetailsList)
            {
                //Update Stock Quantities
                if (POMaster.POStatusId == 6)
                {
                    StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == List.SizeUK && x.ColourId == List.ColourId && x.ProductId == List.ProductId && x.WareHouseId == POMaster.WareHouseId && x.StatusId != 4).FirstOrDefault();
                    if (StockDetail != null)
                    {
                        Int64 localQty = List.Qty;
                        //StockDetail.ReceivedQty = StockDetail.ReceivedQty + localQty;
                        StockDetail.Qty = StockDetail.Qty - localQty;
                        //StockDetail.ModifiedById = Model.CreatedById;
                        StockDetail.ModificationDate = DateTime.UtcNow;
                        //StockDetail.WareHouseId = Model.WareHouseId;
                    }
                }

                List.StatusId = 4;
                List.ModificationDate = DateTime.UtcNow;
                List.ModifiedById = ModifiedById;
            }

            List<StockQuantity> Stock = _context.StockQuantities.Where(x => x.ReferenceId == id && x.StatusId != 4).ToList();
            foreach (var List1 in Stock)
            {
                //Update Stock Quantities
                if (POMaster.POStatusId == 6)
                {
                    StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == List1.SizeUK && x.ColourId == List1.ColourId && x.ProductId == List1.ProductId && x.WareHouseId == POMaster.WareHouseId && x.StatusId != 4).FirstOrDefault();
                    if (StockDetail != null)
                    {
                        Int64 localQty = List1.Qty;
                        //StockDetail.ReceivedQty = StockDetail.ReceivedQty + localQty;
                        StockDetail.Qty = StockDetail.Qty - localQty;
                        //StockDetail.ModifiedById = Model.CreatedById;
                        StockDetail.ModificationDate = DateTime.UtcNow;
                        //StockDetail.WareHouseId = Model.WareHouseId;
                    }
                }

                List1.StatusId = 4;
                List1.ModificationDate = DateTime.UtcNow;
                List1.ModifiedById = ModifiedById;
            }
            //Order Master - Set PO placed to false
            OrderMaster O_Master = _context.OrderMasters.FirstOrDefault(x => x.OrderNo == POMaster.OrderRefrence && x.StatusId != 4);
            if (O_Master != null)
            {
                O_Master.IsPOPlaced = false;
            }

            _context.SaveChanges();
        }

        public PurchaseOrderViewModel GetPurchaseOrderById(Int64 Id)
        {
            PurchaseOrderMaster POMaster = _context.PurchaseOrderMasters.Where(x => x.Id == Id && x.StatusId != 4).FirstOrDefault();

            if (POMaster != null)
            {
                return ToPurchaseOrderViewModel(POMaster);
            }
            else
            {
                return new PurchaseOrderViewModel();
            }
        }

        public PurchaseOrderViewModel ToPurchaseOrderViewModel(PurchaseOrderMaster POMaster)
        {
            try
            {

                //For customer Details from order table
                CustomerViewModel Customer = new CustomerViewModel();
                Customer.CustomerFullName = POMaster.SupplierName;
                Customer.CurrencyName = POMaster.CurrencyName;
                Customer.MobileNo = POMaster.MobileNo;
                //Customer.ZipCode = POMaster.ZipCode;
                Customer.EmailId = POMaster.EmailId;
                Customer.Id = POMaster.SupplierId;
                //Customer.ShippingState = POMaster.ShippingState;

                //For customer Basic Details from customer table

                MasterUser customerDetails = _context.MasterUsers.Where(x => x.Id == POMaster.SupplierId && x.StatusId != 4).FirstOrDefault();
                Customer.CompanyName = customerDetails.CompanyName;
                Customer.Town = _context.Cities.Where(x => x.Id == customerDetails.CityId && x.StatusId != 4).Select(x => x.CityName).FirstOrDefault();
                Customer.CountryId = Customer.CountryId;
                Customer.Country = _context.Countries.Where(x => x.Id == customerDetails.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
                Customer.TelephoneNo = customerDetails.TelephoneNo;
                Customer.CompanyTaxId = customerDetails.CompanyTaxId;
                Customer.AddressLine1 = customerDetails.AddressLine1;
                Customer.AddressLine2 = customerDetails.AddressLine2;
                Customer.City = customerDetails.City;
                //Customer.Town = customerDetails.ci;


                //For Order Details
                List<ProductListModel> ProductDetailsList = GetPOProductDetailList(POMaster.Id);

                //ProductViewModel Productmodel = new ProductViewModel();
                bool mailsent = false;

                if (POMaster.POStatusId == 2 || POMaster.POStatusId == 6)
                {
                    mailsent = true;
                }
                PurchaseOrderViewModel Model = new PurchaseOrderViewModel()
                {
                    Id = POMaster.Id,
                    //OrderNo = POMaster.OrderNo,
                    OrderDate = POMaster.OrderDate.ToString("MM/dd/yyyy"),
                    DeliveryDate = POMaster.DeliveryDate,
                    OrderDateString = POMaster.OrderDate.ToShortDateString(),
                    OrderRefrence = POMaster.OrderRefrence,
                    WareHouseId = POMaster.WareHouseId,
                    PurchaseOrderStatusId = (PurchaseOrderStatus)POMaster.POStatusId,
                  
                    OrderNote = POMaster.OrderNote,
                    ProductList = ProductDetailsList,
                    CurrencyName = POMaster.CurrencyName,
                    CustomerModel = Customer,
                    SendMail = mailsent,
                    //ShippingCharge = POMaster.ShippingCharge,
                    //Tax = POMaster.Tax,
                    SupplierName = POMaster.SupplierName,
                    WareHouseName = _context.WareHouses.FirstOrDefault(x => x.Id == POMaster.WareHouseId).WareHouseName
                };

                return Model;
            }
            catch (Exception ex)
            {
                return new PurchaseOrderViewModel();
            }
        }

        public List<ProductListModel> GetPOProductDetailList(Int64 POMasterId)
        {
            try
            {
                //List<OrderDetail> List = _context.OrderDetails.Where(x => x.OrderMasterId == MasterId).GroupBy(x => new OrderDetail() {ColourId = x.ColourId, Id = x.ProductId }).ToList();
                List<PurchaseOrderDetailViewModel> List = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == POMasterId && x.StatusId != 4)
                                    .GroupBy(x => new { x.ColourId, x.ProductId })
                                    .Select(y => new PurchaseOrderDetailViewModel()
                                    {
                                        ColourId = y.Key.ColourId,
                                        ProductId = y.Key.ProductId
                                    }).ToList();

                List<ProductListModel> ProductDetailList = new List<ProductListModel>();

                foreach (var data in List)
                {
                    ProductViewModel Product = _context.Products.Where(x => x.Id == data.ProductId && x.StatusId != 4)
                                                .Select(x => new ProductViewModel()
                                                {
                                                    Id = x.Id,
                                                    ProductName = x.ProductName,
                                                    Picture1 = x.Picture1,
                                                    PriceEURO = x.PriceEURO,
                                                    PriceGBP = x.PriceGBP,
                                                    PriceUSD = x.PriceUSD,
                                                }).FirstOrDefault();
                    List<SizeViewModel> sizemodel = _context.PurchaseOrderDetails.Where(x => x.ProductId == data.ProductId && x.ColourId == data.ColourId && x.PurchaseOrderMasterId == POMasterId && x.StatusId != 4)
                                                    .Select(y => new SizeViewModel()
                                                    {
                                                        SizeUK = y.SizeUK,
                                                        Qty = y.Qty,
                                                        ReceivedQty = y.ReceivedQty,
                                                        //OrderPrice = y.OrderPrice,
                                                    }).ToList();

                    Product.ColourName = _context.Colours.Where(x => x.Id == data.ColourId && x.StatusId != 4).Select(x => x.ColourName).FirstOrDefault();
                    Product.ColourId = data.ColourId;

                    ProductListModel model = new ProductListModel();
                    model.Product = Product;
                    model.SizeModel = sizemodel;

                    ProductDetailList.Add(model);
                }
                return ProductDetailList;
            }
            catch (Exception ex)
            {
                return new List<ProductListModel>();
            }
        }

        public PurchaseOrderViewModel GetPOPrintPreview(Int64 Id)
        {
            PurchaseOrderMaster POMaster = _context.PurchaseOrderMasters.Where(x => x.Id == Id && x.StatusId != 4).FirstOrDefault();
            if (POMaster != null)
            {
                return ToPurchaseOrderViewModel(POMaster);
            }
            else
            {
                return new PurchaseOrderViewModel();
            }

        }

        public List<SizeViewModel> GetSizeModelForReceivePO(PurchaseOrderViewModel model)
        {
            List<SizeViewModel> sizemodel = _context.PurchaseOrderDetails.Where(x => x.ProductId == model.ProductModel.Id && x.ColourId == model.ColourId && x.PurchaseOrderMasterId == model.Id && x.StatusId != 4)
                                                    .Select(y => new SizeViewModel()
                                                    {
                                                        SizeUK = y.SizeUK,
                                                        Qty = y.Qty,
                                                        ReceivedQty = y.ReceivedQty,
                                                        //OrderPrice = y.OrderPrice,
                                                    }).ToList();
            return sizemodel;
        }

        public bool ReceivePODetails(PurchaseOrderViewModel Model)
        {
            try
            {
                if (Model.IsReceivedAll == true)
                {
                    foreach (var list in Model.DetailsList)
                    {
                        PurchaseOrderDetail Detail = _context.PurchaseOrderDetails.Where(x => x.Id == list.Id && x.StatusId != 4).FirstOrDefault();

                        //Update Stock Quantity
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4 && x.ReferenceId==Detail.PurchaseOrderMasterId).FirstOrDefault();
                        if (StockDetail != null)
                        {
                            Int64 localQty = (Detail.ReceivedQty == Detail.Qty) ? 0 : (Detail.Qty - Detail.ReceivedQty);

                            StockDetail.ReceivedQty = StockDetail.ReceivedQty + localQty;
                            StockDetail.Qty = StockDetail.Qty - localQty;
                            StockDetail.ModifiedById = Model.CreatedById;
                            StockDetail.ModificationDate = DateTime.UtcNow;
                            //StockDetail.WareHouseId = Model.WareHouseId;
                        }
                        StockQuantity StockDetail1 = _context.StockQuantities.Where(x => x.InventoryType == "Stock" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();
                        if (StockDetail1 != null)
                        {
                            Int64 localQty = (Detail.ReceivedQty == Detail.Qty) ? 0 : (Detail.Qty - Detail.ReceivedQty);

                            StockDetail1.ReceivedQty = StockDetail1.ReceivedQty + localQty;
                            //     StockDetail.Qty = StockDetail.Qty - localQty;
                            StockDetail1.ModifiedById = Model.CreatedById;
                            StockDetail1.ModificationDate = DateTime.UtcNow;
                            //StockDetail.WareHouseId = Model.WareHouseId;
                        }
                        if (Detail != null)
                        {
                            Detail.ReceivedQty = Detail.Qty;
                            Detail.ModifiedById = Model.CreatedById;
                            Detail.ModificationDate = DateTime.UtcNow;
                        }

                        _context.SaveChanges();
                    }

                    PurchaseOrderMaster Master = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                    Master.POStatusId = (int)PurchaseOrderStatus.Completed;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    foreach (var list in Model.DetailsList)
                    {
                        PurchaseOrderDetail Detail = _context.PurchaseOrderDetails.Where(x => x.Id == list.Id && x.StatusId != 4).FirstOrDefault();

                        //Update Stock Quantity
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "PO" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4 && x.ReferenceId==Detail.PurchaseOrderMasterId).FirstOrDefault();
                        if (StockDetail != null)
                        {
                            Int64 localQty = (Detail.ReceivedQty == Detail.Qty) ? 0 : (list.ReceiveQty - Detail.ReceivedQty);

                            StockDetail.ReceivedQty = StockDetail.ReceivedQty + localQty;
                            StockDetail.Qty = StockDetail.Qty - localQty;
                            StockDetail.ModifiedById = Model.CreatedById;
                            StockDetail.ModificationDate = DateTime.UtcNow;
                            //StockDetail.WareHouseId = Model.WareHouseId;
                        }
                        StockQuantity StockDetail1 = _context.StockQuantities.Where(x => x.InventoryType == "Stock" && x.SizeUK == list.SizeUK && x.ColourId == list.ColourId && x.ProductId == list.ProductId && x.WareHouseId == Model.DistributionPoint && x.StatusId != 4).FirstOrDefault();
                        if (StockDetail1 != null)
                        {
                            Int64 localQty = (Detail.ReceivedQty == Detail.Qty) ? 0 : (list.ReceiveQty - Detail.ReceivedQty);

                            StockDetail1.ReceivedQty = StockDetail1.ReceivedQty + localQty;
                       //     StockDetail.Qty = StockDetail.Qty - localQty;
                            StockDetail1.ModifiedById = Model.CreatedById;
                            StockDetail1.ModificationDate = DateTime.UtcNow;
                            //StockDetail.WareHouseId = Model.WareHouseId;
                        }

                        if (Detail != null)
                        {
                            Detail.ReceivedQty = list.ReceiveQty;
                            Detail.ModifiedById = Model.CreatedById;
                            Detail.ModificationDate = DateTime.UtcNow;
                        }

                        _context.SaveChanges();
                    }

                    PurchaseOrderMaster Master = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id && x.StatusId != 4).FirstOrDefault();
                    Master.POStatusId = (Int64)Model.PurchaseOrderStatusId;
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }                                 
        public List<PurchaseOrderDetailViewModel> GetDetailsListForReceivePO(Int64 PoMasterid)
        {
            List<PurchaseOrderDetailViewModel> sizemodel = _context.PurchaseOrderDetails.Where(x => x.PurchaseOrderMasterId == PoMasterid && x.StatusId != 4).ToList()
                                                            .Select(ToPurchaseOrderDetailViewModel).ToList();
            return sizemodel;
        }

        public PurchaseOrderDetailViewModel ToPurchaseOrderDetailViewModel(PurchaseOrderDetail data)
        {
            try
            {
                return new PurchaseOrderDetailViewModel()
                {
                    Id = data.Id,
                    SizeUK = data.SizeUK,
                    Qty = data.Qty,
                    ReceiveQty = data.ReceivedQty,
                    ProductId = data.ProductId,
                    ProductName = GetProductnameById(data.ProductId),
                    ColourId = data.ColourId,
                    ColourName = GetColorNameById(data.ColourId),
                    SizeUS = (Convert.ToInt32(data.SizeUK) - 4).ToString(),
                    SizeEU = (Convert.ToInt32(data.SizeUK) + 28).ToString()
                };
            }
            catch (Exception ex)
            {
                return new PurchaseOrderDetailViewModel();
            }
        }

        public string GetProductnameById(Int64 pId)
        {
            return _context.Products.FirstOrDefault(x => x.Id == pId).ProductName;
        }

        public string GetColorNameById(Int64? cId)
        {
            return _context.Colours.FirstOrDefault(x => x.Id == cId).ColourName;
        }

        public List<SizeViewModel> GetSizeModelForDispatchOrder(OrderViewModel model)
        {
            List<SizeViewModel> sizemodel = _context.OrderDetails.Where(x => x.ProductId == model.ProductModel.Id && x.ColourId == model.ColourId && x.OrderMasterId == model.Id && x.StatusId != 4)
                                                    .Select(y => new SizeViewModel()
                                                    {
                                                        SizeUK = y.SizeUK,
                                                        Qty = y.Qty,
                                                        DispatchQty = y.DispatchQty,
                                                        //OrderPrice = y.OrderPrice,
                                                    }).ToList();
            return sizemodel;
        }

        #endregion

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
                        ReceivedQty = 0,
                        POQty = 0,
                        InStockQty = 0,
                        POQtyStr="0"
                    });
                }

                var con = new SqlConnection(connectionString);

                SqlCommand cmd2 = new SqlCommand();
                SqlDataAdapter da2 = new SqlDataAdapter();
                DataTable dt2 = new DataTable("Table1");


                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable("Table1");

                SqlCommand cmd7 = new SqlCommand();
                SqlDataAdapter da7 = new SqlDataAdapter();
                DataTable dt7 = new DataTable("Table1");

                #region OLD Code


                //Get Order Quantity
                con.Open();
                cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + model.WareHouseId + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        string COQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId;
                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);

                        #region New code 23-03
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {

                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == model.WareHouseId).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {


                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.COQtyStr = COQtystr; }


                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }

                    }
                }
                //Get PO Quantity & PO Stock
                con.Open();
                cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + model.WareHouseId + " and ProductId= " + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd.CommandTimeout = timeout;
                da = new SqlDataAdapter(cmd);

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
                        string POQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId ;
                     

                        #region New code 02-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id ).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();

                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != (int)OrderStatus.Cancelled && k.POStatusId != 4 && k.WareHouseId == model.WareHouseId).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                                  

                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQtyStr = POQtystr; }

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }


                    }
                }


             


                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + model.WareHouseId + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd7.CommandTimeout = timeout;
                da7 = new SqlDataAdapter(cmd7);

                da7.Fill(dt7);
                con.Close();
                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++)
                    {
                        string SizeUK = dt7.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
                        // foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }

                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
                    }
                }
              
                  #endregion

                //    }
            

                #region FOR CN


                //Get Order Quantity
                con.Open();
                cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + 4 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + "  group by SizeUK order by SizeUK", con); //10003
                cmd2.CommandTimeout = timeout;
                 da2 = new SqlDataAdapter(cmd2);
                dt2 = new DataTable();
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        string COQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId;
                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);

                        #region New code 23-03
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {
                          
                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4  && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == 4).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {
                                    
                               
                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion

                       
                        {
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.CNQty = Qty; }
                        }
                    }
                }


                //Get PO Quantity & PO Stock
                con.Open();
                 cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + 4 + " and ProductId= " + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd.CommandTimeout = timeout;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
                      
                        #region New code 02-04  
                       
                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();
                               var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != (int)OrderStatus.Cancelled &&  k.POStatusId != 4 && k.WareHouseId == 4).FirstOrDefault();
                                if (ordermastr != null)
                                {
                                if (item.Qty > item.ReceivedQty)
                                {


                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                             }
                         }
                        POQty = mainqunty;
                        #endregion
                       
                        {
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.CNInStockQty = ReceivedQty; }
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.CNPOQty = POQty; }

                        }

                    }
                }


                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + 4 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd7.CommandTimeout = timeout;
                da7 = new SqlDataAdapter(cmd7);
                dt7 = new DataTable();
                da7.Fill(dt7);
                con.Close();
                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++)
                    {
                        string SizeUK = dt7.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
                       
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.CNInStockQty =  ReceivedQty; }
                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
                    }
                }
      

#endregion

                #region FOR US


                //Get Order Quantity
                con.Open();
                cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + 1 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                da2 = new SqlDataAdapter(cmd2);
                dt2 = new DataTable("Table1");
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        string COQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId;
                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);

                        #region New code 23-03
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {

                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == 1).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {


                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion

                      
                        {
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.USQty = Qty; }
                        }
                        // foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.COQtyStr = COQtystr; }
                    }
                }


                //get po quantity & po stock
                con.Open();
                cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + 1 + " and ProductId= " + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd.CommandTimeout = timeout;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
                      
                        #region New code 02-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();
                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != 4 && k.WareHouseId == 1).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                                  

                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion
                 
                        {
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.USInStockQty = ReceivedQty; }
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.USPOQty = POQty; }

                        }

                    }
                }


                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + 1 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd7.CommandTimeout = timeout;
                da7 = new SqlDataAdapter(cmd7);
                dt7 = new DataTable();
                da7.Fill(dt7);
                con.Close();
                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++)
                    {
                        string SizeUK = dt7.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
               
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.USInStockQty =  ReceivedQty; }
                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
                    }
                }
          
                #endregion

                #region FOR EU


                //Get Order Quantity
                con.Open();
                cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + 3 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                da2 = new SqlDataAdapter(cmd2);
                dt2 = new DataTable("Table1");
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        string COQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId;
                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);

                        #region New code 23-03
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {

                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == 3).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {


                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion

                      
                        {
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.EUQty = Qty; }
                        }
                        // foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.COQtyStr = COQtystr; }
                    }
                }


                //get po quantity & po stock
                con.Open();
                cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + 3 + " and ProductId= " + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd.CommandTimeout = timeout;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
                       

                        #region New code 02-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();
                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != (int)OrderStatus.Cancelled && k.POStatusId != 4 && k.WareHouseId == 3).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                           

                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion
       
                        {
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.EUInStockQty = ReceivedQty; }
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.EUPOQty = POQty; }

                        }

                    }
                }


                       
                con.Open();
                cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + 3 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd7.CommandTimeout = timeout;
                da7 = new SqlDataAdapter(cmd7);
                dt7 = new DataTable();
                da7.Fill(dt7);
                con.Close();
                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++)
                    {
                        string SizeUK = dt7.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
            
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.EUInStockQty =  ReceivedQty; }
                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
                    }
                }
        
                #endregion
                #region UK Code


                //Get Order Quantity
                con.Open();
                cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + 2 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        string COQtystr = SizeUK + "-" + model.WareHouseId + "-" + model.ProductModel.Id + "-" + model.ColourId;
                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);

                        #region New code 23-03
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {

                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4  && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == 2).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {


                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.COQtyStr = COQtystr; }


                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.UKQty = Qty; }

                    }
                }


                //Get PO Quantity & PO Stock
                con.Open();
                cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + 2 + " and ProductId= " + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd.CommandTimeout = timeout;
                da = new SqlDataAdapter(cmd);

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
                       

                        #region New code 02-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == model.ColourId && k.SizeUK == SizeUK && k.ProductId == model.ProductModel.Id).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();
                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != (int)OrderStatus.Cancelled && k.POStatusId != 4 && k.WareHouseId ==2).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                       

                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQtyStr = POQtystr; }

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.UKInStockQty = ReceivedQty; }
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.UKPOQty = POQty; }


                    }
                }


                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + 2 + " and ProductId=" + model.ProductModel.Id + " and ColourId=" + model.ColourId + " group by SizeUK order by SizeUK", con);
                cmd7.CommandTimeout = timeout;
                da7 = new SqlDataAdapter(cmd7);

                da7.Fill(dt7);
                con.Close();
                if (dt7 != null && dt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt7.Rows.Count; i++)
                    {
                        string SizeUK = dt7.Rows[i][0].ToString();
                        Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
                        // foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }

                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.UKInStockQty =  ReceivedQty; }

                        //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
                    }
                }
            
                #endregion
                model.IsDataAvailable = true;

                return SizeModelList;
            }
            catch (Exception ex)
            {
                return new List<SizeViewModel>();
            }
        }

        public List<SizeViewModel> GetSizeModelForShortfallInventory(long productId , string productName, long colorId, long warehouseId)
        {
            try
            {
                List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
                string colorName = _context.Colours.FirstOrDefault(x => x.Id == colorId).ColourName;

                for (int i = 2; i <= 34; i += 2)
                {
                    SizeModelList.Add(new SizeViewModel()
                    {
                        SizeUK = i.ToString(),
                        Qty = 0,
                        ReceivedQty = 0,
                        POQty = 0,
                        InStockQty = 0,
                        ColourId = colorId,
                        ColourName = colorName,
                        ProductId = productId,
                        StyleNumber = productName
                    });
                }

                var con = new SqlConnection(connectionString);

                //Get Order Quantity
                con.Open();
                SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable("Table1");
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        #region New code 18-04
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.SizeUK == SizeUK && k.ProductId == productId).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {
                            
                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4  && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == warehouseId).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {


                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion

                       
                        
                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
                    }
                }


                //Get PO Quantity & PO Stock
                con.Open();
                SqlCommand cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId= " + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
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

                        #region New code 19-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.SizeUK == SizeUK && k.ProductId == productId).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();
                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != 4 && k.WareHouseId == warehouseId).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                          

                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion



                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }
                    }
                }


                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                SqlCommand cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType = 'Stock' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
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
                        List<StockQuantity> ProductStock = _context.StockQuantities.Where(x => x.ProductId == productId  && x.ColourId == colorId && x.WareHouseId == warehouseId && x.StatusId != 4).ToList();
                        if (ProductStock != null)
                        {
                            foreach (var data in ProductStock)
                            {
                                foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                            }
                        }
                      
                    }
                }
                
                return SizeModelList;
            }
            catch (Exception ex)
            {
                return new List<SizeViewModel>();
            }
        }


        //Product Stock Count           GetSizeModelForInventory
        public List<List<SizeViewModel>> GetProductInventoryCount(Int64 productId, int warehouseId)
        {
            try
            {
          
                List<List<SizeViewModel>> FinalList = new List<List<SizeViewModel>>();

                List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
                List<ColourViewModel> ColourList = new List<ColourViewModel>();
                try
                {
                    string SelectedColorIds = _context.Products.FirstOrDefault(x => x.Id == productId).SeletedColorIds;
                    string[] values = SelectedColorIds.Split(',');
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
                            ColourList.Add(cm);
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                foreach (var colour in ColourList)
                {
                    SizeModelList = new List<SizeViewModel>();
                    for (int i = 2; i <= 34; i += 2)
                    {
                        SizeModelList.Add(new SizeViewModel()
                        {
                            SizeUK = i.ToString(),
                            Qty = 0,
                            ReceivedQty = 0,
                            POQty = 0,
                            InStockQty = 0,
                            FinalQty = 0,
                            ColourId = colour.Id,
                            ColourName = colour.Colour
                        });
                    }

                    var con = new SqlConnection(connectionString);

                    //Get Order Quantity
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colour.Id + "  group by SizeUK order by SizeUK", con);
                    cmd2.CommandTimeout = timeout;
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable("Table1");
                    da2.Fill(dt2);
                    con.Close();
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            string SizeUK = dt2.Rows[i][0].ToString();
                            Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
                        }
                    }

                    //Get PO Quantity & PO Stock
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId= " + productId + " and ColourId=" + colour.Id + "  group by SizeUK order by SizeUK", con);
                    cmd.CommandTimeout = timeout;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Table1");
                    da.Fill(dt);
                    con.Close();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string SizeUK = dt.Rows[i][0].ToString();
                            Int64 ReceivedQty = Convert.ToInt64(dt.Rows[i][1]);
                            Int64 POQty = Convert.ToInt64(dt.Rows[i][2]);
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }
                        }
                    }

                    //Get Stock Quantity & Add it into the Stock                
                    con.Open();
                    SqlCommand cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colour.Id + "  group by SizeUK order by SizeUK", con);
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
                            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = tom.InStockQty + ReceivedQty; }
                        }
                    }

                    if (SizeModelList != null)
                    {
                        foreach (var item in SizeModelList)
                        {
                            item.FinalQty = (item.InStockQty - item.Qty + item.POQty);
                        }
                    }

                    FinalList.Add(SizeModelList);
                }

                return FinalList;
            }
            catch (Exception ex)
            {
                return new List<List<SizeViewModel>>();
            }
        }
        public List<SizeViewModel> GetSalebyShowReportInventory(Int64 OrderLocaterId, Int64 warehouseId, string OrderDate, string OrderDateString)
        {
            try
            {
                string productName = "";
                string colorName = "";
                
                List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
             
                OrderMaster Model = new OrderMaster();
                Model.UserSelectDeliveryDate = (!String.IsNullOrEmpty(OrderDateString)) ? Convert.ToDateTime(DateTime.ParseExact(OrderDateString, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;

                Model.WearDate = (!String.IsNullOrEmpty(OrderDate)) ? Convert.ToDateTime(DateTime.ParseExact(OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                List<OrderMaster> orderMasters = new List<OrderMaster>();
                if (Model.UserSelectDeliveryDate != null && Model.WearDate != null)
                {
                    orderMasters = _context.OrderMasters.Where(k => k.OrderlocatorId == OrderLocaterId && k.OrderStatusId != 5 && k.OrderDate >= Model.WearDate && k.OrderDate <= Model.UserSelectDeliveryDate && k.WareHouseId == warehouseId && k.StatusId != 4).ToList();
                }
                if (Model.UserSelectDeliveryDate == null && Model.WearDate == null)
                {
                    orderMasters = _context.OrderMasters.Where(k => k.OrderlocatorId == OrderLocaterId && k.OrderStatusId != 5  && k.WareHouseId == warehouseId && k.StatusId != 4).ToList();
                }
                foreach (var ab in orderMasters)
                {
                    var OrderDetail = _context.OrderDetails.Where(k => k.OrderMasterId == ab.Id && k.StatusId != 4).ToList();
                    foreach (var bb in OrderDetail)
                    {
                         colorName = _context.Colours.FirstOrDefault(x => x.Id == bb.ColourId).ColourName;
                         productName = _context.Products.FirstOrDefault(x => x.Id == bb.ProductId).ProductName;
                       for (int i = 2; i <= 34; i += 2)
                        {
                            SizeModelList.Add(new SizeViewModel()
                            {
                                SerialNo=bb.Id,
                                SizeUK = i.ToString(),
                                Qty = 0,
                                ReceivedQty = 0,
                                POQty = 0,
                                InStockQty = 0,
                                ColourId = Convert.ToInt64(bb.ColourId),
                                ColourName = colorName,
                                ProductId = bb.ProductId,
                                StyleNumber = productName,
                                OrderPrice = bb.OrderPrice,
                            }); 
                        }
                        var con = new SqlConnection(connectionString);

                        ////Get Order Quantity
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [OrderDetails] where  Id='"+bb.Id+"' and StatusId!=4 group by SizeUK order by SizeUK", con);
                        cmd2.CommandTimeout = timeout;
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dt2 = new DataTable("Table1");
                        da2.Fill(dt2);
                        con.Close();
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                long mainqunty = 0;
                                string SizeUK = dt2.Rows[i][0].ToString();
                                Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                                //   long mainqunty = 0;
                                OrderMaster Model1 = new OrderMaster();

                        var od=_context.OrderDetails.Where(k=>k.SizeUK==SizeUK && k.Id==bb.Id && k.ProductId== bb.ProductId && k.ColourId==bb.ColourId).FirstOrDefault();
                                if (od.Qty > 0)
                                {
                                    long newqty = bb.Qty;
                                     mainqunty = mainqunty + newqty;

                                   foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK && w.StyleNumber==productName && w.ColourName==colorName && w.SerialNo== bb.Id)) { tom.Qty = newqty; }

                                }


                            }
                        }
                    }
                }

                List<SizeViewModel> SizeModelList1 =  SizeModelList.OrderBy(k => k.StyleNumber).ToList();
                List<SizeViewModel> finalSizeModel = new List<SizeViewModel>();
                foreach (var sz in SizeModelList1)
                {
                    long mainqty = 0;
                   var avail = finalSizeModel.Where(k => k.StyleNumber == sz.StyleNumber && k.ColourName == sz.ColourName && k.SizeUK == sz.SizeUK).FirstOrDefault();
                    if (avail != null)
                    {
                        mainqty = avail.Qty + sz.Qty;
                        //avail.Qty = mainqty;
                        foreach (var tom in finalSizeModel.Where(w => w.SizeUK == avail.SizeUK && w.StyleNumber == avail.StyleNumber && w.ColourName == avail.ColourName  )) { tom.Qty = mainqty; }
                    }
                    // long newQty = avail.Qty;
                    

                    else
                    {
                        finalSizeModel.Add(sz);
                    }
                }
                return finalSizeModel;
                
            }
            catch (Exception ex)
            {
                return new List<SizeViewModel>();
            }
        }
        public List<SizeViewModel> GetSalebyProductReportInventory(Int64 productId, string productName, Int64 colorId, Int64 warehouseId, string OrderDate,string OrderDateString)
        {
            try
            {
                List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
                string colorName = _context.Colours.FirstOrDefault(x => x.Id == colorId).ColourName;

                for (int i = 2; i <= 34; i += 2)
                {
                    SizeModelList.Add(new SizeViewModel()
                    {
                        SizeUK = i.ToString(),
                        Qty = 0,
                        ReceivedQty = 0,
                        POQty = 0,
                        InStockQty = 0,
                        ColourId = colorId,
                        ColourName = colorName,
                        ProductId = productId,
                        StyleNumber = productName
                    });
                }
                var con = new SqlConnection(connectionString);

                ////Get Order Quantity
                con.Open();
                SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable("Table1");
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        //   long mainqunty = 0;
                        OrderMaster Model = new OrderMaster();


                        Model.UserSelectDeliveryDate = (!String.IsNullOrEmpty(OrderDateString)) ? Convert.ToDateTime(DateTime.ParseExact(OrderDateString, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;

                        Model.WearDate = (!String.IsNullOrEmpty(OrderDate)) ? Convert.ToDateTime(DateTime.ParseExact(OrderDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;

                        var OrderDetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.ProductId == productId && k.SizeUK == SizeUK).ToList();
                        //   var ordermastr = _context.OrderMasters.Where(k => k.StatusId != 4 && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.OrderDate >= Model.OrderDate && k.OrderDate <= Model.UserSelectDeliveryDate && k.WareHouseId == warehouseId).ToList();
                        foreach (var bb in OrderDetail)
                        {
                            if (Model.UserSelectDeliveryDate != null && Model.WearDate != null)
                            {
                                var ordermastr = _context.OrderMasters.Where(k => k.StatusId != 4 && k.OrderStatusId != 5 && k.OrderDate >= Model.WearDate && k.OrderDate <= Model.UserSelectDeliveryDate && k.WareHouseId == warehouseId && k.Id == bb.OrderMasterId).FirstOrDefault();


                              
                                if (ordermastr != null)
                                {
                                  

                                        long newqty = bb.Qty ;
                                        mainqunty = mainqunty + newqty;
                                    


                                }
                                Qty = mainqunty;
                                foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
                            }
                            else
                            {
                                if (Model.UserSelectDeliveryDate == null && Model.WearDate == null)
                                {
                                    var ordermastr = _context.OrderMasters.Where(k => k.StatusId != 4  && k.OrderStatusId != 5 && k.WareHouseId == warehouseId && k.Id == bb.OrderMasterId).FirstOrDefault();


                                    //   List<OrderDetail> OrderDetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.ProductId == productId && k.OrderMasterId == item.Id).ToList();
                                    // foreach (var item in ordermastr)
                                    /// {
                                    if (ordermastr != null)
                                    {
                                       


                                            long newqty = bb.Qty;
                                            mainqunty = mainqunty + newqty;
                                        
                                    }
                                }
                                Qty = mainqunty;

                                foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
                            }
                        }


                    }
                }
               return SizeModelList;
            }
            catch (Exception ex)
            {
                return new List<SizeViewModel>();
            }
        }
        public List<SizeViewModel> GetStockReportForInventory(Int64 productId, string styleNumber, Int64 colorId, Int64 warehouseId)
        {
            #region Old code 25-07-22
            //try
            //{
            //    List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
            //    string colorName = _context.Colours.FirstOrDefault(x => x.Id == colorId).ColourName;

            //    for (int i = 2; i <= 34; i += 2)
            //    {
            //        SizeModelList.Add(new SizeViewModel()
            //        {
            //            SizeUK = i.ToString(),
            //            Qty = 0,
            //            ReceivedQty = 0,
            //            POQty = 0,
            //            InStockQty = 0,
            //            ColourId = colorId,
            //            ColourName = colorName,
            //            ProductId = productId,
            //            StyleNumber = styleNumber
            //        });
            //    }

            //    var con = new SqlConnection(connectionString);

            //    //Get Order Quantity
            //    con.Open();
            //    SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + "  group by SizeUK order by SizeUK", con);
            //    cmd2.CommandTimeout = timeout;
            //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //    DataTable dt2 = new DataTable("Table1");
            //    da2.Fill(dt2);
            //    con.Close();
            //    if (dt2 != null && dt2.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt2.Rows.Count; i++)
            //        {
            //            string SizeUK = dt2.Rows[i][0].ToString();
            //            Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
            //            //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
            //        }
            //    }


            //    //Get PO Quantity & PO Stock
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId= " + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
            //    cmd.CommandTimeout = timeout;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable("Table1");
            //    da.Fill(dt);
            //    con.Close();
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            string SizeUK = dt.Rows[i][0].ToString();
            //            Int64 ReceivedQty = Convert.ToInt64(dt.Rows[i][1]);
            //            Int64 POQty = Convert.ToInt64(dt.Rows[i][2]);
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }
            //        }
            //    }


            //    //Get Stock Quantity & Add it into the Stock                
            //    con.Open();
            //    SqlCommand cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
            //    cmd7.CommandTimeout = timeout;
            //    SqlDataAdapter da7 = new SqlDataAdapter(cmd7);
            //    DataTable dt7 = new DataTable("Table1");
            //    da7.Fill(dt7);
            //    con.Close();
            //    if (dt7 != null && dt7.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt7.Rows.Count; i++)
            //        {
            //            string SizeUK = dt7.Rows[i][0].ToString();
            //            Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = tom.InStockQty + ReceivedQty; }
            //            //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
            //        }
            //    }

            //    return SizeModelList;
            //}
            //catch (Exception ex)
            //{
            //    return new List<SizeViewModel>();
            //}
            #endregion

            #region new code 25-07-22
            //try
            //{
            //    List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
            //    string colorName = _context.Colours.FirstOrDefault(x => x.Id == colorId).ColourName;

            //    for (int i = 2; i <= 34; i += 2)
            //    {
            //        SizeModelList.Add(new SizeViewModel()
            //        {
            //            SizeUK = i.ToString(),
            //            Qty = 0,
            //            ReceivedQty = 0,
            //            POQty = 0,
            //            InStockQty = 0,
            //            ColourId = colorId,
            //            ColourName = colorName,
            //            ProductId = productId,
            //            StyleNumber = styleNumber
            //        });
            //    }

            //    var con = new SqlConnection(connectionString);

            //    //Get Order Quantity
            //    con.Open();
            //    SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + "  group by SizeUK order by SizeUK", con);
            //    cmd2.CommandTimeout = timeout;
            //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //    DataTable dt2 = new DataTable("Table1");
            //    da2.Fill(dt2);
            //    con.Close();
            //    if (dt2 != null && dt2.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt2.Rows.Count; i++)
            //        {
            //            long mainqunty = 0;
            //            string SizeUK = dt2.Rows[i][0].ToString();
            //            Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
            //            #region New code 18-04
            //            var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.SizeUK == SizeUK && k.ProductId == productId).ToList();
            //            //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
            //            foreach (var item in orderdetail)
            //            {

            //                var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == warehouseId).FirstOrDefault();
            //                // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
            //                if (ordermastr != null)
            //                {
            //                    if (item.Qty > item.DispatchQty)
            //                    {


            //                        long newqty = item.Qty - item.DispatchQty;
            //                        mainqunty = mainqunty + newqty;
            //                    }
            //                }
            //            }
            //            Qty = mainqunty;
            //            #endregion



            //            //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
            //        }
            //    }


            //    //Get PO Quantity & PO Stock
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId= " + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
            //    cmd.CommandTimeout = timeout;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable("Table1");
            //    da.Fill(dt);
            //    con.Close();
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            long mainqunty = 0;
            //            string SizeUK = dt.Rows[i][0].ToString();
            //            Int64 ReceivedQty = Convert.ToInt64(dt.Rows[i][1]);
            //            Int64 POQty = Convert.ToInt64(dt.Rows[i][2]);

            //            #region New code 19-04  

            //            var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.SizeUK == SizeUK && k.ProductId == productId).ToList();
            //            foreach (var item in orderdetail)
            //            {
            //                //    modl = new CustomerorderQuantityDetail();
            //                var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != 4 && k.WareHouseId == warehouseId).FirstOrDefault();
            //                if (ordermastr != null)
            //                {
            //                    if (item.Qty > item.ReceivedQty)
            //                    {
            //                        //        // var cusdetail = _context.MasterUsers.Where(k => k.Id == ordermastr.SupplierId).FirstOrDefault();
            //                        //        modl.Customername = ordermastr.SupplierName;
            //                        //        modl.OrderNo = ordermastr.OrderRefrence;
            //                        //        modl.Quantity = item.Qty;
            //                        //        modellist.Add(modl);
            //                        //        model.IsDataAvailable = true;

            //                        long newqty = item.Qty - item.ReceivedQty;
            //                        mainqunty = mainqunty + newqty;
            //                    }
            //                }
            //            }
            //            POQty = mainqunty;
            //            #endregion



            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }
            //        }
            //    }


            //    //Get Stock Quantity & Add it into the Stock                
            //    con.Open();
            //    SqlCommand cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where InventoryType='Stock' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
            //    cmd7.CommandTimeout = timeout;
            //    SqlDataAdapter da7 = new SqlDataAdapter(cmd7);
            //    DataTable dt7 = new DataTable("Table1");
            //    da7.Fill(dt7);
            //    con.Close();
            //    if (dt7 != null && dt7.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt7.Rows.Count; i++)
            //        {
            //            string SizeUK = dt7.Rows[i][0].ToString();
            //            Int64 ReceivedQty = Convert.ToInt64(dt7.Rows[i][1]);
            //            foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = tom.InStockQty + ReceivedQty; }
            //            //foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = tom.POQty + (POQty - ReceivedQty); }
            //        }
            //    }

            //    return SizeModelList;
            //}
            //catch (Exception ex)
            //{
            //    return new List<SizeViewModel>();
            //}
            #endregion
            try
            {
                List<SizeViewModel> SizeModelList = new List<SizeViewModel>();
                string colorName = _context.Colours.FirstOrDefault(x => x.Id == colorId).ColourName;

                for (int i = 2; i <= 34; i += 2)
                {
                    SizeModelList.Add(new SizeViewModel()
                    {
                        SizeUK = i.ToString(),
                        Qty = 0,
                        ReceivedQty = 0,
                        POQty = 0,
                        InStockQty = 0,
                        ColourId = colorId,
                        ColourName = colorName,
                        ProductId = productId,
                        StyleNumber = styleNumber
                    });
                }

                var con = new SqlConnection(connectionString);

                //Get Order Quantity
                con.Open();
                SqlCommand cmd2 = new SqlCommand("select SizeUK,Sum(Qty) as Sum from [StockQuantities] where InventoryType='Order' and StatusId!=4 and WareHouseId =" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + "  group by SizeUK order by SizeUK", con);
                cmd2.CommandTimeout = timeout;
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable("Table1");
                da2.Fill(dt2);
                con.Close();
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        long mainqunty = 0;
                        string SizeUK = dt2.Rows[i][0].ToString();
                        Int64 Qty = Convert.ToInt64(dt2.Rows[i][1]);
                        #region New code 18-04
                        var orderdetail = _context.OrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.SizeUK == SizeUK && k.ProductId == productId).ToList();
                        //  var orderdetail = _context.OrderDetails.Where(k => k.ColourId == colorid && k.SizeUK == sze && k.ProductId == productid).ToList();
                        foreach (var item in orderdetail)
                        {

                            var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4  && k.OrderStatusId != 4 && k.OrderStatusId != 5 && k.WareHouseId == warehouseId).FirstOrDefault();
                            // var ordermastr = _context.OrderMasters.Where(k => k.Id == item.OrderMasterId && k.StatusId != 4 && k.OrderStatusId != 6 && k.WareHouseId == wherehouseid).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.DispatchQty)
                                {


                                    long newqty = item.Qty - item.DispatchQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        Qty = mainqunty;
                        #endregion



                        //Int64 DispatchedQty = Convert.ToInt64(dt2.Rows[i][2]);
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.Qty = Qty; }
                    }
                }


                //Get PO Quantity & PO Stock
                con.Open();
                SqlCommand cmd = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received,Sum(Qty) as Qty from [StockQuantities] where InventoryType='PO' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId= " + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
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

                        #region New code 19-04  

                        var orderdetail = _context.PurchaseOrderDetails.Where(k => k.StatusId != 4 && k.ColourId == colorId && k.SizeUK == SizeUK && k.ProductId == productId).ToList();
                        foreach (var item in orderdetail)
                        {
                            //    modl = new CustomerorderQuantityDetail();
                            var ordermastr = _context.PurchaseOrderMasters.Where(k => k.Id == item.PurchaseOrderMasterId && k.StatusId != 4 && k.POStatusId != 4 && k.WareHouseId == warehouseId).FirstOrDefault();
                            if (ordermastr != null)
                            {
                                if (item.Qty > item.ReceivedQty)
                                {
                         
                                    long newqty = item.Qty - item.ReceivedQty;
                                    mainqunty = mainqunty + newqty;
                                }
                            }
                        }
                        POQty = mainqunty;
                        #endregion



                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                        foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.POQty = POQty; }
                    }
                }


                //Get Stock Quantity & Add it into the Stock                
                con.Open();
                SqlCommand cmd7 = new SqlCommand("select SizeUK,Sum(ReceivedQty) as Received from [StockQuantities] where  InventoryType='Stock' and StatusId!=4 and WareHouseId=" + warehouseId + " and ProductId=" + productId + " and ColourId=" + colorId + " group by SizeUK order by SizeUK", con);
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
                        List<StockQuantity> ProductStock = _context.StockQuantities.Where(x => x.ProductId ==productId  && x.ColourId == colorId && x.WareHouseId == warehouseId && x.StatusId != 4).ToList();
                        if (ProductStock != null)
                        {
                            foreach (var data in ProductStock)
                            {
                                foreach (var tom in SizeModelList.Where(w => w.SizeUK == SizeUK)) { tom.InStockQty = ReceivedQty; }
                            }
                        }
                    
                    }
                }

                return SizeModelList;
            }
            catch (Exception ex)
            {
                return new List<SizeViewModel>();
            }
        }


        #region: GetSizeModelForInventory Old Code

        

        #endregion

        #region : Mail

        //Mail to Admin Seller
        public void SendMail(PurchaseOrderViewModel Model)
        {
            bool Success = false;
            try
            {
                //Model.WareHouseName = _context.WareHouses.FirstOrDefault(x => x.Id == Model.WareHouseId).WareHouseName;

                var unqkey = GetUniqueKey();
                unqkey = Model.Id + "#" + unqkey;
                var SizeHeader = "";

                for (int i = 2; i <= 34; i = i + 2)
                {
                    SizeHeader = SizeHeader + "<th style='width: 30px;border: 1px solid #000;border-collapse: collapse;'>" + i + "</th>";
                }
                var SizeData = "";
                foreach (var data in Model.ProductList)
                {
                    var res = "";
                    for (int i = 2; i <= 34; i = i + 2)
                    {
                        try
                        {
                            var k = i.ToString();
                            try
                            {
                                if (data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty > 0)
                                {
                                    res = res + "<td style='width: 30px;border: 1px solid #000;border-collapse: collapse;color:red;font-weight:bolder;'> " + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty + "</td>";
                                }
                                else
                                {
                                    res = res + "<td style='width: 30px;border: 1px solid #000;border-collapse: collapse;'> " + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty + "</td>";
                                }
                            }
                            catch (Exception ex)
                            {
                                res = res + "<td style='width: 30px;border: 1px solid #000;border-collapse: collapse;'> 0 </td>";
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    };

                    Model.Quantity = 0;

                    string ImagePath = _context.Products.FirstOrDefault(x => x.Id == data.Product.Id).Picture1;

                    try
                    {
                        if(data.Product.ColourName==null || data.Product.ColourName=="")
                        {
                            data.Product.ColourName = _context.Colours.Where(x => x.Id == data.Product.ColourId && x.StatusId != 4).Select(x => x.ColourName).FirstOrDefault();
                        }
                    }
                    catch (Exception)
                    {

                    }
                            

                    SizeData = SizeData + "<tr style='border: 1px solid #000;border-collapse: collapse;'><td style='width: 70px;border: 1px solid #000;border-collapse: collapse;'> "
                                       + data.Product.ProductName + "</td><td style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>" + data.Product.ColourName
                                       + "</td><td><img src='http://lorefashions.com/" + ImagePath + "' style='width:120px;height:140px'></td> " + res + "</tr>";
                }

                string from = ConfigurationManager.AppSettings["confirmationEmail"];
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, "Lore Group");
                mail.To.Add(Model.CustomerModel.EmailId);
               // mail.CC.Add("harshit.bhawsar@brainnetic.com");
               mail.CC.Add("PURCHASING@LORE-GROUP.COM");

                // mail.Body
                mail.Subject = "Purchase Order Summary : " + Model.OrderRefrence;
                //mail.Body = "<b>Hello, User ! Welcome</b>" + "<br/>" +
                //    "<br/>" +
                //    "You have purchased Mera-Fayda Product from Rajhans Digital. Your Registration has been completed. Go to the Miss-Call Application Web Module to select Misscall Numbers and manage your profile.<br/>" +
                //    "http://app.rajhans.digital " + "<br/>" +
                //    "Username : <b>" + "UserNAme" + "</b>" + "<br/>" +
                //    "Password : <b>" + "PassWord" + "</b>" + "<br/>" +
                //    "<br/>" +
                //    "<br/>" +
                //    "<br/>" +
                //    "<b>With best regards,</b>" + "<br/>" +
                //    "<b>Mera-Fayda Team</b>" + "<br/>" +
                //    "Rajhans Digital<br/>" +
                //    "<br/>";


                var rowcount = Model.ProductList.Count + 1;
                mail.Body = "<div  style='margin-top: 20px;width: 80%;'>"

                           + "<table style='width: 90%;'><tbody>"
                           + "<tr><td colspan='2' align='left' style='width:100%;'><p>Hello, </p></td></tr>"
                           + "<tr><td colspan='2' align='left' style='width:100%;'></td></tr>"
                           + "<tr><td colspan='2' align='left'><p>A new purchase order comes. See details below : </p></td></tr><br>"
                           + "<tr><td colspan='2' align='left' style='width:100%;'></td></tr>"
                           + "<tr><td>ORDER REF :</td><td>" + Model.OrderRefrence + "</td>"
                           + "<td rowspan='6'style='width: 130px;text-align:  right;'><img src='http://lorefashions.com/Content/lorelogo.jpg' style='width:180px;' ></td></tr>"
                           + "<tr><td>Date of order:</td><td>" + Model.OrderDate + "</td></tr>"
                           + "<tr><td>Supplier Name:</td><td>" + Model.SupplierName + "</td></tr>"
                           + "<tr><td>Address:</td><td>" + Model.CustomerModel.AddressLine1 + "</td></tr>"
                           + "<tr><td>Country:</td><td>" + Model.CustomerModel.Country + "</td></tr>"
                           + "<tr><td>Distribution Point:</td><td>" + Model.WareHouseName + "</td></tr>"
                           + "<tr><td>Delivery Date:</td><td>" + Model.DeliveryDate + "</td></tr></tbody></table>"

                           + "<span style='margin-left: 40%;padding: 10px;border-radius: 5px;background-color:  green;'>"
                           + "<a href='http://lorefashions.com/Order/ConfirmPO?id=" + unqkey + "' title='ConfirmPO' target='_blank' style='text-decoration:  none;color: white;'>Click here to confirm PO</a></span>" +

                           "<div style='overflow-x:auto; margin-top: 15px;'> " +
        "		 <table id='GetSizeModelForInventory' style='width:100%; padding-bottom:0px; padding-top:0px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +

        "                <thead style='display: table-header-group; vertical-align: middle; border-color: inherit;'>" +

        "				<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
        "                        <th rowspan='3'>Product</th>" +
        "                        <th rowspan='3'>Color</th>" +
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
        "                    </tr>" +
        "                </thead>" +
        "                <tbody style='border:solid;border-color:black;border-width:inherit;'>" +
        SizeData +
        "                </tbody>" +
        "               <tr><td colspan='20' rowspan='4' style='width:10%'><div style='padding: 15px;height: 60px;border: 1px solid black;'><b>Order Note: </b><br>" + Model.OrderNote + "</div></td></tr>" +
        "            </table>" +
        "		</div>" +
        "    <table><tr><td></td></tr>" +
        "    <tr><td align='left'><p>Thanks And Regards,</p></td></tr>" +
        "    <tr><td align='left'><p>Lore-Group.</p></td></tr></table>" +
        "</div>";


                //+ "<table style='border: 1px solid #dddddd;padding: 8px; border-collapse: collapse;margin-top: 15px;'><thead><tr style='border: 1px solid #000;border-collapse: collapse;'>"
                //+ "<th style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>Product</th><th style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>Color</th>"
                //+ "<th rowspan='" + rowcount + "' style='width: 70px;border: 1px solid #000;border-collapse: collapse;vertical-align: top;'>Size(UK)</th>" + SizeHeader + "</tr>" + SizeData + "</thead>"
                //+ "<tr><td colspan='20' rowspan='4' style='width:10%'><div style='padding: 15px;height: 60px;border: 1px solid black;'><b>Order Note: </b><br>" + Model.OrderNote + "</div></td></tr>"
                //+ "</table></div>";

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));
                // set the network credentials
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);
                //smtp.EnableSsl = true;                
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = networkCredential;                
             
                smtp.Send(mail);

                Success = true;
            }
            catch (Exception ex)
            {
                Success = false;
            }

            if (Success == true)
            {
                //Update PO Status
                PurchaseOrderMaster POMaster = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id).FirstOrDefault();
                POMaster.POStatusId = 2; //Mail Sent
                _context.SaveChanges();
            }
        }




        public void SendMailToReadyShip(PurchaseOrderViewModel Model)
        {
        // bool Success = false;
            try
            {
                var SizeHeader = "";

                for (int i = 2; i <= 34; i = i + 2)
                {
                    SizeHeader = SizeHeader + "<th style='width: 30px;border: 1px solid #000;border-collapse: collapse;'>" + i + "</th>";
                }
                var SizeData = "";
                foreach (var data in Model.ProductList)
                {
                    var res = "";
                    for (int i = 2; i <= 34; i = i + 2)
                    {
                        try
                        {
                            var k = i.ToString();
                            try
                            {
                                if (data.SizeModel.FirstOrDefault(x=>x.SizeUK==k).Qty > 0)
                                {
                                    res = res + "<td style='width: 30px;border: 1px solid #000;border-collapse: collapse;color:red;font-weight:bolder;'> " + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty + "</td>";
                                }
                                else
                                {
                                    res = res + "<td style='width: 30px;border: 1px solid #000;border-collapse: collapse;'> " + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty + "</td>";
                                }
                            }
                            catch (Exception ex)
                            {
                                res = res + "<td style='width: 30px;border: 1px solid #000;border-collapse: collapse;'> 0 </td>";
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    };

                    Model.Quantity = 0;

                    string ImagePath = _context.Products.FirstOrDefault(x => x.Id == data.Product.Id).Picture1;

                    try
                    {
                        if (data.Product.ColourName == null || data.Product.ColourName == "")
                        {
                            data.Product.ColourName = _context.Colours.Where(x => x.Id == data.Product.ColourId && x.StatusId != 4).Select(x => x.ColourName).FirstOrDefault();
                        }
                    }
                    catch (Exception)
                    {

                    }


                    SizeData = SizeData + "<tr style='border: 1px solid #000;border-collapse: collapse;'><td style='width: 70px;border: 1px solid #000;border-collapse: collapse;'> "
                                       + data.Product.ProductName + "</td><td style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>" + data.Product.ColourName
                                       + "</td><td><img src='http://lorefashions.com/" + ImagePath + "' style='width:120px;height:140px'></td> " + res + "</tr>";
                }
                //Model.WareHouseName = _context.WareHouses.FirstOrDefault(x => x.Id == Model.WareHouseId).WareHouseName;


                string from = ConfigurationManager.AppSettings["confirmationEmail"];
                MailMessage mail = new MailMessage();
                   mail.From = new MailAddress(from, "Lore Group");
                    mail.To.Add("PURCHASING@LORE-GROUP.COM");
                // mail.From = new MailAddress("Harshitbhawsar22@gmail.com");
               //   mail.To.Add("harshit.bhawsar@brainnetic.com");
                //  mail.CC.Add("payal.jha@smtgroup.in");
             //  mail.To.Add(Model.CustomerModel.EmailId);
                //mail.To.Add("SALES@LORE-GROUP.COM");
                //    mail.To.Add(" harshit.bhawsar@brainnetic.com");

                // mail.Body
                mail.Subject = "Order No.: " + Model.OrderRefrence + " is now ready and awaiting shipping instructions";
                //mail.Body = "<b>Hello, User ! Welcome</b>" + "<br/>" +
                //    "<br/>" +
                //    "You have purchased Mera-Fayda Product from Rajhans Digital. Your Registration has been completed. Go to the Miss-Call Application Web Module to select Misscall Numbers and manage your profile.<br/>" +
                //    "http://app.rajhans.digital " + "<br/>" +
                //    "Username : <b>" + "UserNAme" + "</b>" + "<br/>" +
                //    "Password : <b>" + "PassWord" + "</b>" + "<br/>" +
                //    "<br/>" +
                //    "<br/>" +
                //    "<br/>" +
                //    "<b>With best regards,</b>" + "<br/>" +
                //    "<b>Mera-Fayda Team</b>" + "<br/>" +
                //    "Rajhans Digital<br/>" +
                //    "<br/>";


               var rowcount = Model.ProductList.Count + 1;
                mail.Body = "<div  style='margin-top: 20px;width: 80%;'>"

                           + "<table style='width: 90%;'><tbody>"
                           + "<tr><td colspan='2' align='left' style='width:100%;'><p>Hello, </p></td></tr>"
                           + "<tr><td colspan='2' align='left' style='width:100%;'></td></tr>"
                           + "<tr><td colspan='2' align='left'><p>A new purchase Order Ready to ship. See details below : </p></td></tr><br>"
                           + "<tr><td colspan='2' align='left' style='width:100%;'></td></tr>"
                           + "<tr><td>ORDER REF :</td><td>" + Model.OrderRefrence + "</td>"
                           + "<td rowspan='6'style='width: 130px;text-align:  right;'><img src='http://lorefashions.com/Content/lorelogo.jpg' style='width:180px;' ></td></tr>"
                           + "<tr><td>Date of order:</td><td>" + Model.OrderDate + "</td></tr>"
                           + "<tr><td>Supplier Name:</td><td>" + Model.SupplierName + "</td></tr>"
                           + "<tr><td>Address:</td><td>" + Model.CustomerModel.AddressLine1 + "</td></tr>"
                           + "<tr><td>Country:</td><td>" + Model.CustomerModel.Country + "</td></tr>"
                           + "<tr><td>Distribution Point:</td><td>" + Model.WareHouseName + "</td></tr></tbody></table>" +
      
        "		 <table id='GetSizeModelForInventory' style='width:100%; padding-bottom:0px; padding-top:0px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +

        "                <thead style='display: table-header-group; vertical-align: middle; border-color: inherit;'>" +

        "				<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
        "                        <th rowspan='3'>Product</th>" +
        "                        <th rowspan='3'>Color</th>" +
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
        "                    </tr>" +
        "                </thead>" +
        "                <tbody style='border:solid;border-color:black;border-width:inherit;'>" +
        SizeData +
        "                </tbody>" +
     //   "               <tr><td colspan='20' rowspan='4' style='width:10%'><div style='padding: 15px;height: 60px;border: 1px solid black;'><b>Order Note: </b><br>" + Model.OrderNote + "</div></td></tr>" +
        "            </table>" +

        "</div>";


                //+ "<table style='border: 1px solid #dddddd;padding: 8px; border-collapse: collapse;margin-top: 15px;'><thead><tr style='border: 1px solid #000;border-collapse: collapse;'>"
                //+ "<th style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>Product</th><th style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>Color</th>"
                //+ "<th rowspan='" + rowcount + "' style='width: 70px;border: 1px solid #000;border-collapse: collapse;vertical-align: top;'>Size(UK)</th>" + SizeHeader + "</tr>" + SizeData + "</thead>"
                //+ "<tr><td colspan='20' rowspan='4' style='width:10%'><div style='padding: 15px;height: 60px;border: 1px solid black;'><b>Order Note: </b><br>" + Model.OrderNote + "</div></td></tr>"
                //+ "</table></div>";

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));
                // set the network credentials
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);
                //smtp.EnableSsl = true;                
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = networkCredential;                

                smtp.Send(mail);

          //      Success = true;
            }
            catch (Exception ex)
            {
            //    Success = false;
            }

            //if (Success == true)
            //{
            //    //Update PO Status
            //    PurchaseOrderMaster POMaster = _context.PurchaseOrderMasters.Where(x => x.Id == Model.Id).FirstOrDefault();
            //    POMaster.POStatusId = 2; //Mail Sent
            //    _context.SaveChanges();
            //}
        }


        //In Progress Mail to Customer
        public void SendMailToCustomer(OrderViewModel Model)
        {
            try
            {
                var TotalAmt = "";
                var amt = "";
                var shippingcharge = "";
                var taxamount = "";

                if (Model.CustomerModel.CurrencyName == "EURO")
                {
                    TotalAmt = "€ " + Model.TotalAmount.ToString();
                    amt = "€ " + Model.Amount.ToString();
                    shippingcharge = "€ " + Model.ShippingCharge.ToString();
                    taxamount = "€ " + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString();
                }
                else if (Model.CustomerModel.CurrencyName == "GBP")
                {
                    TotalAmt = "£ " + Model.TotalAmount.ToString();
                    amt = "£ " + Model.Amount.ToString();
                    shippingcharge = "£ " + Model.ShippingCharge.ToString();
                    taxamount = "£ " + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString();
                }
                else
                {
                    TotalAmt = "$ " + Model.TotalAmount.ToString();
                    amt = "$ " + Model.Amount.ToString();
                    shippingcharge = "$ " + Model.ShippingCharge.ToString();
                    taxamount = "$ " + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString();
                }

                var SizeHeader = "";

                for (int i = 2; i <= 34; i = i + 2)
                {
                    SizeHeader = SizeHeader + "<th style='width:18%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>" + i + "</th>";
                }
                var SizeData = "";
                foreach (var data in Model.ProductList)
                {
                    var res = "";
                    for (int i = 2; i <= 34; i = i + 2)
                    {
                        try
                        {
                            var k = i.ToString();
                            try
                            {
                                res = res + "<td style='width:10%'> " + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty + "</td>";
                            }
                            catch (Exception ex)
                            {
                                res = res + "<td style='width:10%'> 0 </td>";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    };

                    Model.Quantity = 0;

                    SizeData = SizeData + "<tr><td style='width:10%'> "
                                       + data.Product.ProductName + "<td style='width:10%'>" + data.Product.ColourName + "<td style='width:10%'>" + data.SizeModel[0].OrderPrice
                                       + "</td><td style='width:10%'></td></td> " + res + "</tr>";

                }

                string from = "kapil.rajwani@connekt.in"; //any valid GMail ID
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, "Loré Fashions");
               // mail.To.Add("harshit.bhawsar@brainnetic.com");
                   mail.To.Add(Model.CustomerModel.EmailId);
                //mail.To.Add("kapil.rajwani005@gmail.com");
                mail.Subject = "Confirmation from supplier";

                var rowcount = Model.ProductList.Count + 1;
                //mail.Body = "<div  style='margin-top: 20px;width: 80%;'>"

                //    + "<table style='width: 90%;'><tbody><tr><td>ORDER NO :</td><td>" + Model.OrderNo + "</td>"
                //    + "<td rowspan='4'style='width: 70%;text-align:  right;'><img src='http://backoffice.amore-group.com/img/Lore_LightBackground1.png'></td></tr>"
                //    + "<tr><td>Date of order:</td><td>" + Model.OrderDate + "</td></tr>"
                //    + "<tr><td>Customer Name:</td><td>" + Model.CustomerModel.CustomerFullName + "</td></tr>"
                //    + "<tr><td>Total Amount:</td><td>" + Model.TotalAmount + "</td></tr></table>"

                //    + "<span style='margin-left: 40%;padding: 10px;border-radius: 5px;background-color: green; color: white;'>"
                //    + "Your Order is in Progress"
                //    + "</span>"
                //    + "<table style='border: 1px solid #dddddd;padding: 8px; border-collapse: collapse;margin-top: 15px;'><thead><tr style='border: 1px solid #000;border-collapse: collapse;'>"
                //    + "<th style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>Product</th><th style='width: 70px;border: 1px solid #000;border-collapse: collapse;'>Color</th>"
                //    + "<th rowspan='" + rowcount + "' style='width: 70px;border: 1px solid #000;border-collapse: collapse;vertical-align: top;'>Size(UK)</th>" + SizeHeader + "</tr>" + SizeData + "</thead></table></div>";

                mail.Body = "<div class='row' style='display: block;'>" +
            "<table style='display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;'>" +
                "<tbody style='display: table-row-group;  vertical-align: middle;  border-color: inherit;'>" +

                    "<tr style='display: table-row;  vertical-align: inherit;  border-color: inherit;'>" +
                        "<td style='width: 900px;  padding: 5px 5px 5px 5px;  margin: 0 0 0 0; border: solid 1px #000000; vertical-align: top;'>" +
                            "<table cellpadding='0' cellspacing='0' class='fullwidth' style='width: 100%;'>" +
                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                        "<td style='vertical-align: top;'>" +
                                            "<img src='http://lorefashions.com/Content/lorelogo.jpg'>" +
                                        "</td>" +
                                        "<td style='vertical-align: top; padding-left: 10px;'>" +
                                            "<table cellpadding='0' cellspacing='0' class='order-header-contact-details' style='width: 500px;'>" +
                                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                                    //"<tr style='display: table-row;  vertical-align: inherit; border-color: inherit;'>" +
                                                    //    "<td colspan='2' class='company-name' style='vertical-align: top; font-family: Times New Roman; font-size: 18pt; font-weight:bold; padding-bottom: 15px;'>" +
                                                    //        "LORE LIMITED" +
                                                    //    "</td>" +
                                                    //     "<td class='header' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>ORDER FORM</td>" +
                                                    //"</tr>" +
                                                    "<tr style='display: table-row;  vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td style='vertical-align: top;'>" +
                                                            "<p class='address-block' style='font-size: 10pt; width: 250px;'>" +
                                                            "<b><u>UNITED KINGDON</u></b><br>" +
                                                                "7 Central Business Centre<br>" +
                                                                "Iron Bridge Close<br>" +
                                                                "London NW10 0UR<br>" +
                                                                "TEL: +44 (0)845 224 9601<br>" +
                                                                "FAX: +44 (0)20 8998 0080" +
                                                            "</p>" +
                                                        "</td>" +
                                                        "<td style='vertical-align: top;'>" +
                                                            "<p class='address-block' style='font-size: 10pt; width: 250px;'>" +
                                                            "<b><u>U.S.A</u></b><br>" +
                                                                "AMERICA'S MART <br> SHOWROOM #10N102<br>" +
                                                                "250 SPRING ST NW<br>" +
                                                                "ATLANTA, GA 30303<br>" +
                                                                "TEL: +1 (770) 238-1738<br>" +
                                                                "FAX: +1 (770) 238-1739" +
                                                            //"FAX: +44 (0)20 8998 0080<br>" +
                                                            //"WEB: WWW.LORE-GROUP.COM<br>" +
                                                            //"E-MAIL: SALES@LORE-GROUP.COM" +
                                                            "</p>" +
                                                        "</td>" +
                                                         "<td style='vertical-align: top;'>" +
                                                            "<p class='address-block' style='font-size: 10pt; width: 250px;'>" +
                                                            "<b style='font-size: 18px;'>ORDER FORM</b><br><br>" +
                                                                "Email: sales@lore-group.com <br>" +
                                                                "Web: www.lore-group.com" +
                                                            "</p>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</tbody>" +
                                            "</table>" +
                                        "</td>" +
                                    //"<td class='order-header-title' style='vertical-align:top; font-size: 18pt; font-weight: bold; text-transform: uppercase;'>" +
                                    //    "ORDER FORM" +
                                    //"</td>" +

                                    "</tr>" +
                                "</tbody>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +

                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                        "<td class='order' style='padding: 10px 0 10px 0; margin: 0; vertical-align:top;'>" +
                            "<table cellpadding='0' cellspacing='0' class='fullwidth' style='display: table; border-collapse: separate; border-spacing: 2px; border-color: grey; width: 100%;'>" +
                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                        "<td class='padded-border' width='60%' style='border: solid 1px #000000; padding: 10px 10px 10px 10px; vertical-align: top;'>" +
                                            "<table cellpadding='0' cellspacing='0' class='client-details' style='width: 500px; display: table;  border-collapse: separate; border-spacing: 2px; border-color: grey;'>" +
                                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='header' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>" +
                                                            "Client Details:" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Company Name: <span id='lblcompname'>" +
                                                                "<label id='lblCompanyName'> " + Model.CustomerModel.CompanyName + " </label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Client Name:" +
                                                            "<span id='lblcontact'>" +
                                                                "<label id='lblContactName'> " + Model.CustomerModel.CustomerFullName + "</label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Address:" +
                                                            "<span id='lbladdress'>" +
                                                                "<label id='lblAddress'> " + Model.CustomerModel.AddressLine1 + "</label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "City:" +
                                                            "<span id='lblcity'>" +
                                                                "<label id='lblCity'>" + Model.CustomerModel.City + " </label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "County/State:" +
                                                            "<span id='lblcounty'>" +
                                                                "<label id='lblCounty'> </label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Post Code:" +
                                                            "<span id='lblpost'>" +
                                                                "<label id='lblZipCode'>" + Model.CustomerModel.ZipCode + " </label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Country:" +
                                                            "<span id='lblcountry'>" +
                                                                "<label id='lblCountry'>" + Model.CustomerModel.Country + "</label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Email:" +
                                                            "<span id='lblemail'>" +
                                                                "<label id='lblEmail'>" + Model.CustomerModel.EmailId + "</label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Tel1:" +
                                                            "<span id='lblph'>" +
                                                                "<label id='lblPhone'> " + Model.CustomerModel.TelephoneNo + " </label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Mobile:<span id='lblmob'>" +
                                                                "<label id='lblMobile'> " + Model.CustomerModel.MobileNo + " </label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "Fax:<span id='lblfax'>" +
                                                                "<label id='lblFax'></label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                            "NIF/VAT:" +
                                                            "<span id='lblVat'>" +
                                                                "<label id='lblTaxID'> " + Model.CustomerModel.CompanyTaxId + "</label>" +
                                                            "</span>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    // "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                    //    "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                    //        "DeliveryDate:" +
                                                    //        "<span id='lblDeliv'>" +
                                                    //            "<label id='lblDelivary'> " + Model.UserSelectDeliveryDateString + "</label>" +
                                                    //        "</span>" +
                                                    //    "</td>" +
                                                    //"</tr>" +
                                                    //  "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                    //    "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'>" +
                                                    //        "BriderName:" +
                                                    //        "<span id='lblBride'>" +
                                                    //            "<label id='lblBrideName'> " + Model.BridesName + "</label>" +
                                                    //        "</span>" +
                                                    //    "</td>" +
                                                    //"</tr>" +

                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='detail' style='font-size: 12pt;border-bottom: solid 1px #000000;line-height:25px; padding: 0 5px 0 5px; vertical-align:top;'></td>" +
                                                    "</tr>" +
                                                "</tbody>" +
                                            "</table>" +
                                        "</td>" +
                                        "<td width='2%' style='vertical-align: top;'></td>" +
                                        "<td width='38%' style='vertical-align: top;'>" +
                                            "<table cellpadding='0' cellspacing='0' class='fullwidth' style='width: 100%; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;'>" +
                                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td class='border' style='border: 1px solid black; vertical-align: top;'>" +
                                                            "<table cellpadding='0' cellspacing='0' class='banking-details' style='display: table; border-collapse: separate; border-spacing: 2px;border-color: grey;'>" +
                                                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                                                    //"<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                    //    "<td class='header' colspan='2' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>Lore Limited</td>" +

                                                                    //"</tr>" +
                                                                    //"<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                    //    "<td class='detail' style='height: 25px; font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>VAT No.:</td>" +
                                                                    //    "<td class='detail' style='height: 25px; font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>GB 689932070</td>" +
                                                                    //"</tr>" +
                                                                    //"<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                    //    "<td class='header' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;' colspan='2'>Sterling Bank Details:</td>" +
                                                                    //"</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='header' colspan='2' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>Sterling Bank Details:</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>UK Bank:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Santander</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>A/C:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>79664587</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Sort Code:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>09-01-27</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>BIC:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>ABBYGB2LXXX</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>IBAN:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>GB54 ABBY 0901 2779 6645 87</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='header' colspan='2' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>Euro Bank Details:</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Spain Bank:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Banco Bilbao Vizcava Argentaria</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>IBAN:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>ES19-0182-3379-73-0291557045</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>BIC:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>BBVAESMM</td>" +
                                                                    "</tr>" +

                                                                     "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='header' colspan='2' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>US Dollar bank details:</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Beneficiary Bank:</td>" +
                                                                        "<td class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Bank of the Ozarks</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td nowrap class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Routing Number:</td>" +
                                                                        "<td nowrap class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>0829 0727 3</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td nowrap class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>Beneficiary A/C no.:</td>" +
                                                                        "<td nowrap class='detail' style='font-size: 10pt; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;'>107067745</td>" +
                                                                    "</tr>" +
                                                                "</tbody>" +
                                                            "</table>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                        "<td align='right' style='vertical-align: top;'>" +
                                                            "<table class='order-details' cellpadding='0' cellspacing='0' style='padding-top: 20px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;'>" +
                                                                "<tbody style='display: table-row-group; vertical-align: middle; border-color: inherit;'>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='header' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>DATE OF ORDER:</td>" +
                                                                        "<td class='detail' style='font-size: 12pt; border-bottom: solid 1px #000000; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;' align='right' width='170px'>" +
                                                                            "<span id='lblOrderDate'>" +
                                                                                "<label id='lblOrderDate'>" + Model.OrderDate + "</label>" +
                                                                            "</span>" +
                                                                        "</td>" +
                                                                    "</tr>" +
                                                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                                                        "<td class='header' style='font-size: 12pt; font-weight: bold; padding: 0 5px 0 5px; line-height: 18px; vertical-align: top;'>ORDER NO.:</td>" +
                                                                        "<td class='detail' style='font-size: 12pt; border-bottom: solid 1px #000000; line-height: 25px; padding: 0 5px 0 5px; vertical-align: top;' align='right'>" +
                                                                            "<span id='lblOrderNo'>" +
                                                                                "<label id='lblOrderReference'> " + Model.OrderNo + " </label>" +
                                                                            "</span>" +
                                                                        "</td>" +
                                                                    "</tr>" +
                                                                "</tbody>" +
                                                            "</table>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</tbody>" +
                                            "</table>" +
                                        "</td>" +
                                    "</tr>" +
                                "</tbody>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +
                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                        "<td class='order-item-details' style='vertical-align: top; display: table-cell;'>" +
                            "<table id='detailsTable' style='width:90%; padding-bottom:0px; padding-top:0px; display: table;  border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +
                                "<thead style='display: table-header-group; vertical-align: middle; border-color: inherit;'>" +
                                    "<tr style='display: table-row; vertical-align: inherit; border-color: inherit;'>" +
                                        "<th style='width:10%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>Product</th>" +
                                        "<th style='width:10%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>Color</th>" +
                                        "<th style='width:18%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>Price</th>" +
                                        "<th style='width:10%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>Size(UK)</th>" +
                                         SizeHeader +
                                    //"<th style='width:10%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>Quantity</th>"+
                                    //"<th style='width:10%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>Total</th>"+
                                    //"<th style='width:0%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd; display:none;'></th>"+
                                    "</tr>" +
                                "</thead>" +
                                "<tbody style='border:solid;border-color:black;border-width:inherit;'>" +
                                SizeData +
                                //                "<tr>" +
                                //                    "<td style='width:10%'> 61108</td>" +
                                //                    "<td style='width:10%'> Red</td>" +
                                //                    "<td style='width:10%'> 199</td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> 1</td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> 1</td>" +
                                //                    "<td style='width:10%'> 1</td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                //                    "<td style='width:10%'> </td>" +
                                ////"<td style='width:10%'> 3</td>"+
                                ////"<td style='width:10%'> 597</td>"+
                                //                "</tr>" +
                                "</tbody>" +
                                "<tfoot>" +
                                    "<tr>" +
                                        "<td colspan='18' rowspan='5' style='width:10%'>" +
                                            "<div style='padding: 15px;height: 52px;border: 1px solid black;'>" +
                                                "<b>Order Note:" + Model.OrderNote + "</b><br>" +
                                            "</div>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td nowrap colspan='2'>Sub Total</td>" +
                                        "<td nowrap>" + amt + "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td nowrap colspan='2'>Delivery</td>" +
                                        "<td nowrap>" + shippingcharge + "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td nowrap colspan='2'> Tax </td>" +
                                        "<td nowrap>" + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString() + "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td nowrap colspan='2'> Total </td>" +
                                        "<td nowrap> " + TotalAmt + " </td>" +
                                    "</tr>" +
                                "</tfoot>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +
                "</tbody>" +
            "</table>" +
        "</div>";
                mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = "kapil.rajwani@connekt.in";
                networkCredential.Password = "Allene143";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
            }

        }

        //In Progress Mail to Customer
        public void SendInprogressMailToCustomer(OrderViewModel Model)
        {
            try
            {
                var TotalAmt = "";
                var amt = "";
                var shippingcharge = "";
                var taxamount = "";

                if (Model.CustomerModel.CurrencyName == "EURO")
                {
                    TotalAmt = "€ " + Model.TotalAmount.ToString();
                    amt = "€ " + Model.Amount.ToString();
                    shippingcharge = "€ " + Model.ShippingCharge.ToString();
                    taxamount = "€ " + Math.Round(decimal.Parse((Model.Tax * (Model.Amount+ Model.ShippingCharge) / 100).ToString()), 2).ToString();
                }
                else if (Model.CustomerModel.CurrencyName == "GBP")
                {
                    TotalAmt = "£ " + Model.TotalAmount.ToString();
                    amt = "£ " + Model.Amount.ToString();
                    shippingcharge = "£ " + Model.ShippingCharge.ToString();
                    taxamount = "£ " + Math.Round(decimal.Parse((Model.Tax * (Model.Amount + Model.ShippingCharge) / 100).ToString()), 2).ToString();
                }
                else
                {
                    TotalAmt = "$ " + Model.TotalAmount.ToString();
                    amt = "$ " + Model.Amount.ToString();
                    shippingcharge = "$ " + Model.ShippingCharge.ToString();
                    taxamount = "$ " + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString();
                }

                //var SizeHeader = "";
                //for (int i = 2; i <= 34; i = i + 2)
                //{
                //    SizeHeader = SizeHeader + "<th style='width:18%; font-weight: bold; text-align: -internal-center; display: table-cell; vertical-align: inherit; background-color: #dddddd;'>" + i + "</th>";
                //}

                var SizeData = "";
                foreach (var data in Model.ProductList)
                {
                    var res = "";
                    Int64 rowQty = 0;
                    for (int i = 2; i <= 34; i = i + 2)
                    {
                        try
                        {
                            var k = i.ToString();
                            try
                            {
                                res = res + "<td style='width:10%'> " + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty + "</td>";
                                rowQty = rowQty + data.SizeModel.FirstOrDefault(x => x.SizeUK == k).Qty;
                            }
                            catch (Exception ex)
                            {
                                res = res + "<td style='width:10%'> 0 </td>";
                            }
                        }
                        catch (Exception ex)
                        { }
                    };

                    Model.Quantity = 0;
                    SizeData = SizeData + "<tr><td style='width:10%'>" + data.Product.ProductName + "</td>" +
                                            "<td style='width:10%'>" + data.Product.ColourName + "</td>" +
                                            "<td style='width:10%'>" + data.SizeModel[0].OrderPrice + "</td>" +
                                            "<td style='width:10%'> </td>" +
                                            res +
                                            "<td style='width:10%'>" + rowQty + "</td>" +
                                            "<td style='width:10%'>" + rowQty * data.SizeModel[0].OrderPrice + "</td> </tr>";
                }

                string from = ConfigurationManager.AppSettings["confirmationEmail"];
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, "Lore Group");
          //mail.To.Add("harshitbhawsar22@gmail.com");
                mail.To.Add(Model.CustomerModel.EmailId);
                // mail.CC.Add("sales@lore-group.com");
                      mail.CC.Add(ConfigurationManager.AppSettings["confirmationEmail"]);
                mail.Subject = "Loré Order Confirmation - " + Model.OrderNo;

                var rowcount = Model.ProductList.Count + 1;
                if (Model.DistributionPoint == 2)
                {
                    #region : Mail Body
                    mail.Body = "<!DOCTYPE html>" +
                                "<html>" +
                                "<head>" +
                                "    <title>Order</title>" +
                                "    <meta http-equiv='content-type' content='text/html; charset=UTF-8' />" +
                                "	<meta name='viewport' content='width=device-width, initial-scale=1'>" +

                                "	<style>	" +

                                "table {" +
                                "    border-collapse: collapse;" +
                                "    border-spacing: 0;        " +
                                //"    width:600px;        " +
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

                                "@media (max-width:1146px) {" +
                                "   table {width:100%;} " +
                                "}" +

                                "@media (max-width:1000px) {" +
                                "	#product_div {overflow-x:auto} " +
                                "}" +

                                "@media (max-width:914px) {" +
                                "	table {width:100%;}" +
                                "	.address-block {width:200px;}" +
                                "}" +

                                "@media (max-width:764px) {" +
                                "	table {width:100%;}" +
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
                                "}" +

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
         "<div>" +

                        " <table cellpadding = '0' cellspacing = '0' class='banking-details' style='display: table; border-collapse: separate; border-spacing: 2px;border-color: grey;'>" +
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
                         "                                                     </tbody>" +
                         "              </table>" +

                           "</div>" +
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
                            "                                                     7 CENTRAL BUSINESS CENTRE<br>" +
                            "                                                     IRON BRIDGE CLOSE<br>" +
                            "                                                     LONDON NW10 0UR<br>" +
                            "                                                     TEL: +44 (0)845 224 9601<br>" +
                            "                                                     FAX: +44 (0)20 8998 0080<br>" +

                            "                                                 </p>" +
                            "                                             </td>" +
                            "                                             <td>" +
                            "                                                 <p class='address-block'>" +
                            "                                                          <b><u>U.S.A</u></b><br>" +
                            "                                                             AMERICA'S MART BLDG 3<br>" +
                            "                                                             SHOWROOM #12N101<br>" +
                            "                                                             75 JOHN PORTMAN BLVD<br>" +
                            "                                                             ATLANTA, GA 30303<br>" +
                            "                                                             TEL: +1 (770) 238-1738<br>" +
                            "                                                             FAX: +1 (770) 238-1739<br>" +
                            "                                                 </p>" +
                            "                                             </td>" +
                            "                                             <td>" +
                            "                                                 <p class='address-block'>" +
                            "                                                          <b><u>SPAIN</u></b><br>" +
                            "                                                             CALLE AMAZONA 2<br>" +
                            "                                                             ROQUETAS DE MAR,04740<br>" +
                            "                                                             ALMERIA<br>" +
                            "                                                             TEL: +34 663 01 66 20<br>" +
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

                             //Order - Customer details
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
                            "                                                     <label id='lblCompanyName'> " + Model.CustomerModel.CompanyName + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Client Name:" +
                            "                                                 <span id='lblcontact'>" +
                            "                                                     <label id='lblContactName'> " + Model.CustomerModel.CustomerFullName + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Address:" +
                            "                                                 <span id='lbladdress'>" +
                            "                                                     <label id='lblAddress'> " + Model.CustomerModel.AddressLine1 + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 City:" +
                            "                                                 <span id='lblcity'>" +
                            "                                                     <label id='lblCity'> " + Model.CustomerModel.City + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 County:" +
                            "                                                 <span id='lblcounty'>" +
                            "                                                     <label id='lblCounty'>" + Model.CustomerModel.StateName + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Post Code:" +
                            "                                                 <span id='lblpost'>" +
                            "                                                     <label id='lblZipCode'>" + Model.CustomerModel.ZipCode + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Country:" +
                            "                                                 <span id='lblcountry'>" +
                            "                                                     <label id='lblCountry'>" + Model.CustomerModel.Country + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Email:" +
                            "                                                 <span id='lblemail'>" +
                            "                                                     <label id='lblEmail'>" + Model.CustomerModel.EmailId + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Tel1:" +
                            "                                                 <span id='lblph'>" +
                            "                                                     <label id='lblPhone'>" + Model.CustomerModel.TelephoneNo + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Mobile:<span id='lblmob'>" +
                            "                                                     <label id='lblMobile'>" + Model.CustomerModel.MobileNo + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Fax:<span id='lblfax'>" +
                            "                                                     <label id='lblFax'>" + Model.CustomerModel.Fax + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 NIF/VAT:" +
                            "                                                 <span id='lblVat'>" +
                            "                                                     <label id='lblTaxID'>" + Model.CustomerModel.CompanyTaxId + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                             "<tr>" +
                                                            "<td class='detail'>" +
                                                                "DeliveryDate:" +
                                                                "<span id='lblDeliv'>" +
                                                                    "<label id='lblDelivary'> " + Model.UserSelectDeliveryDateString + "</label>" +
                                                                "</span>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                          "<tr>" +
                                                            "<td class='detail'>" +
                                                                "BriderName:" +
                                                                "<span id='lblBride'>" +
                                                                    "<label id='lblBrideName'> " + Model.BridesName + "</label>" +
                                                                "</span>" +
                                                            "</td>" +
                                                        "</tr>" +
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
                            "                                                                     <label id='lblOrderDate'>" + Model.OrderDate + "</label>" +
                            "                                                                 </span>" +
                            "                                                             </td>" +
                            "                                                         </tr>" +
                            "                                                         <tr>" +
                            "                                                             <td class='header'>ORDER NO.:</td>" +
                            "                                                             <td class='detail' align='right'>" +
                            "                                                                 <span id='lblOrderNo'>" +
                            "                                                                     <label id='lblOrderReference'>" + Model.OrderNo + "</label>" +
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
                            "                         </tr> " +

                            "    					</tbody>" +
                            "                 </table>" +
                            "             </td>" +
                            "         </tr>" +
                            "     </tbody>" +
                            " </table>" +
                            " </div>" +


                           //Product Details     
                           //"    <div style='overflow-x:auto;'>" +
                           "    <div id='product_div'>" +
            "        <table class='product' style='width: 70%;'>" +
            "        <tbody><tr style='/* width: 100%; */display: table-row;vertical-align: inherit;border-color: inherit;'>" +
            "        <td class='order-item-details' style='vertical-align: top; display: table-cell;'>" +
            "         <div style='overflow-x:auto;'> " +
            "		 <table id='detailsTable' style='width:100%; padding-bottom:0px; padding-top:0px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +

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
            "                        <th rowspan='3' style='min-width: 51px;'>Total</th>" +
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



            SizeData +
            "                </tbody>" +
            "                <tfoot>" +
            "                    <tr>" +
            "                        <td colspan='18' rowspan='5' style='width:10%'>" +
            "                            <div style='padding: 15px;height: 60px;border: 1px solid black;'>" +
            "                                <b>Order Note: </b>" + Model.OrderNote + "<br> " +
            "                            </div>" +
            "                        </td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Sub Total</b></td>" +
            "                        <td colspan='2'>" + amt + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Delivery</b></td>" +
            "                        <td colspan='2'>" + shippingcharge + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Tax</b></td>" +
            "                        <td colspan='2'>" + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString() + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Total</b></td>" +
            "                        <td colspan='2'>" + TotalAmt + "</td>" +
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

                    #endregion
                }
                if (Model.DistributionPoint == 3)
                {
                    #region : Mail Body
                    mail.Body = "<!DOCTYPE html>" +
                                "<html>" +
                                "<head>" +
                                "    <title>Order</title>" +
                                "    <meta http-equiv='content-type' content='text/html; charset=UTF-8' />" +
                                "	<meta name='viewport' content='width=device-width, initial-scale=1'>" +

                                "	<style>	" +

                                "table {" +
                                "    border-collapse: collapse;" +
                                "    border-spacing: 0;        " +
                                //"    width:600px;        " +
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

                                "@media (max-width:1146px) {" +
                                "   table {width:100%;} " +
                                "}" +

                                "@media (max-width:1000px) {" +
                                "	#product_div {overflow-x:auto} " +
                                "}" +

                                "@media (max-width:914px) {" +
                                "	table {width:100%;}" +
                                "	.address-block {width:200px;}" +
                                "}" +

                                "@media (max-width:764px) {" +
                                "	table {width:100%;}" +
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
                                "}" +

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

                            "<div> "+
                                       "<div> " +
                        "<table cellpadding = '0' cellspacing = '0' class='banking-details' style='display: table; border-collapse: separate; border-spacing: 2px;border-color: grey;'>" +
                         "                                                     <tbody>" +

                            "                                        <tr>" +
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
                            "                                                         </tr>      " +                                                    
                            "                                                     </tbody>" +
                            "                                                 </table>" +
                            "</div>"+
                     
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
                            "                                                     7 CENTRAL BUSINESS CENTRE<br>" +
                            "                                                     IRON BRIDGE CLOSE<br>" +
                            "                                                     LONDON NW10 0UR<br>" +
                            "                                                     TEL: +44 (0)845 224 9601<br>" +
                            "                                                     FAX: +44 (0)20 8998 0080<br>" +

                            "                                                 </p>" +
                            "                                             </td>" +
                            "                                             <td>" +
                            "                                                 <p class='address-block'>" +
                            "                                                          <b><u>U.S.A</u></b><br>" +
                            "                                                             AMERICA'S MART BLDG 3<br>" +
                            "                                                             SHOWROOM #12N101<br>" +
                            "                                                             75 JOHN PORTMAN BLVD<br>" +
                            "                                                             ATLANTA, GA 30303<br>" +
                            "                                                             TEL: +1 (770) 238-1738<br>" +
                            "                                                             FAX: +1 (770) 238-1739<br>" +
                            "                                                 </p>" +
                            "                                             </td>" +
                            "                                             <td>" +
                            "                                                 <p class='address-block'>" +
                            "                                                          <b><u>SPAIN</u></b><br>" +
                            "                                                             CALLE AMAZONA 2<br>" +
                            "                                                             ROQUETAS DE MAR,04740<br>" +
                            "                                                             ALMERIA<br>" +
                            "                                                             TEL: +34 663 01 66 20<br>" +
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

                             //Order - Customer details
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
                            "                                                     <label id='lblCompanyName'> " + Model.CustomerModel.CompanyName + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Client Name:" +
                            "                                                 <span id='lblcontact'>" +
                            "                                                     <label id='lblContactName'> " + Model.CustomerModel.CustomerFullName + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Address:" +
                            "                                                 <span id='lbladdress'>" +
                            "                                                     <label id='lblAddress'> " + Model.CustomerModel.AddressLine1 + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 City:" +
                            "                                                 <span id='lblcity'>" +
                            "                                                     <label id='lblCity'> " + Model.CustomerModel.City + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 County:" +
                            "                                                 <span id='lblcounty'>" +
                            "                                                     <label id='lblCounty'>" + Model.CustomerModel.StateName + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Post Code:" +
                            "                                                 <span id='lblpost'>" +
                            "                                                     <label id='lblZipCode'>" + Model.CustomerModel.ZipCode + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Country:" +
                            "                                                 <span id='lblcountry'>" +
                            "                                                     <label id='lblCountry'>" + Model.CustomerModel.Country + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Email:" +
                            "                                                 <span id='lblemail'>" +
                            "                                                     <label id='lblEmail'>" + Model.CustomerModel.EmailId + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Tel1:" +
                            "                                                 <span id='lblph'>" +
                            "                                                     <label id='lblPhone'>" + Model.CustomerModel.TelephoneNo + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Mobile:<span id='lblmob'>" +
                            "                                                     <label id='lblMobile'>" + Model.CustomerModel.MobileNo + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Fax:<span id='lblfax'>" +
                            "                                                     <label id='lblFax'>" + Model.CustomerModel.Fax + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 NIF/VAT:" +
                            "                                                 <span id='lblVat'>" +
                            "                                                     <label id='lblTaxID'>" + Model.CustomerModel.CompanyTaxId + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                             "<tr>" +
                                                            "<td class='detail'>" +
                                                                "DeliveryDate:" +
                                                                "<span id='lblDeliv'>" +
                                                                    "<label id='lblDelivary'> " + Model.UserSelectDeliveryDateString + "</label>" +
                                                                "</span>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                          "<tr>" +
                                                            "<td class='detail'>" +
                                                                "BriderName:" +
                                                                "<span id='lblBride'>" +
                                                                    "<label id='lblBrideName'> " + Model.BridesName + "</label>" +
                                                                "</span>" +
                                                            "</td>" +
                                                        "</tr>" +
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
                            "                                                                     <label id='lblOrderDate'>" + Model.OrderDate + "</label>" +
                            "                                                                 </span>" +
                            "                                                             </td>" +
                            "                                                         </tr>" +
                            "                                                         <tr>" +
                            "                                                             <td class='header'>ORDER NO.:</td>" +
                            "                                                             <td class='detail' align='right'>" +
                            "                                                                 <span id='lblOrderNo'>" +
                            "                                                                     <label id='lblOrderReference'>" + Model.OrderNo + "</label>" +
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
                            "                         </tr> " +

                            "    					</tbody>" +
                            "                 </table>" +
                            "             </td>" +
                            "         </tr>" +
                            "     </tbody>" +
                            " </table>" +
                            " </div>" +


                           //Product Details     
                           //"    <div style='overflow-x:auto;'>" +
                           "    <div id='product_div'>" +
            "        <table class='product' style='width: 70%;'>" +
            "        <tbody><tr style='/* width: 100%; */display: table-row;vertical-align: inherit;border-color: inherit;'>" +
            "        <td class='order-item-details' style='vertical-align: top; display: table-cell;'>" +
            "         <div style='overflow-x:auto;'> " +
            "		 <table id='detailsTable' style='width:100%; padding-bottom:0px; padding-top:0px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +

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
            "                        <th rowspan='3' style='min-width: 51px;'>Total</th>" +
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



            SizeData +
            "                </tbody>" +
            "                <tfoot>" +
            "                    <tr>" +
            "                        <td colspan='18' rowspan='5' style='width:10%'>" +
            "                            <div style='padding: 15px;height: 60px;border: 1px solid black;'>" +
            "                                <b>Order Note: </b>" + Model.OrderNote + "<br> " +
            "                            </div>" +
            "                        </td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Sub Total</b></td>" +
            "                        <td colspan='2'>" + amt + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Delivery</b></td>" +
            "                        <td colspan='2'>" + shippingcharge + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Tax</b></td>" +
            "                        <td colspan='2'>" + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString() + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Total</b></td>" +
            "                        <td colspan='2'>" + TotalAmt + "</td>" +
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

                    #endregion
                }
                else if(Model.DistributionPoint==1||Model.DistributionPoint==4)
                {
                    #region : Mail Body
                    mail.Body = "<!DOCTYPE html>" +
                                "<html>" +
                                "<head>" +
                                "    <title>Order</title>" +
                                "    <meta http-equiv='content-type' content='text/html; charset=UTF-8' />" +
                                "	<meta name='viewport' content='width=device-width, initial-scale=1'>" +

                                "	<style>	" +

                                "table {" +
                                "    border-collapse: collapse;" +
                                "    border-spacing: 0;        " +
                                //"    width:600px;        " +
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

                                "@media (max-width:1146px) {" +
                                "   table {width:100%;} " +
                                "}" +

                                "@media (max-width:1000px) {" +
                                "	#product_div {overflow-x:auto} " +
                                "}" +

                                "@media (max-width:914px) {" +
                                "	table {width:100%;}" +
                                "	.address-block {width:200px;}" +
                                "}" +

                                "@media (max-width:764px) {" +
                                "	table {width:100%;}" +
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
                                "}" +

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

                            "<div> "+
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
                            "                                                     7 CENTRAL BUSINESS CENTRE<br>" +
                            "                                                     IRON BRIDGE CLOSE<br>" +
                            "                                                     LONDON NW10 0UR<br>" +
                            "                                                     TEL: +44 (0)845 224 9601<br>" +
                            "                                                     FAX: +44 (0)20 8998 0080<br>" +

                            "                                                 </p>" +
                            "                                             </td>" +
                            "                                             <td>" +
                            "                                                 <p class='address-block'>" +
                            "                                                          <b><u>U.S.A</u></b><br>" +
                            "                                                             AMERICA'S MART BLDG 3<br>" +
                            "                                                             SHOWROOM #12N101<br>" +
                            "                                                             75 JOHN PORTMAN BLVD<br>" +
                            "                                                             ATLANTA, GA 30303<br>" +
                            "                                                             TEL: +1 (770) 238-1738<br>" +
                            "                                                             FAX: +1 (770) 238-1739<br>" +
                            "                                                 </p>" +
                            "                                             </td>" +
                            "                                             <td>" +
                            "                                                 <p class='address-block'>" +
                            "                                                          <b><u>SPAIN</u></b><br>" +
                            "                                                             CALLE AMAZONA 2<br>" +
                            "                                                             ROQUETAS DE MAR,04740<br>" +
                            "                                                             ALMERIA<br>" +
                            "                                                             TEL: +34 663 01 66 20<br>" +
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

                             //Order - Customer details
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
                            "                                                     <label id='lblCompanyName'> " + Model.CustomerModel.CompanyName + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Client Name:" +
                            "                                                 <span id='lblcontact'>" +
                            "                                                     <label id='lblContactName'> " + Model.CustomerModel.CustomerFullName + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Address:" +
                            "                                                 <span id='lbladdress'>" +
                            "                                                     <label id='lblAddress'> " + Model.CustomerModel.AddressLine1 + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 City:" +
                            "                                                 <span id='lblcity'>" +
                            "                                                     <label id='lblCity'> " + Model.CustomerModel.City + " </label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 County:" +
                            "                                                 <span id='lblcounty'>" +
                            "                                                     <label id='lblCounty'>" + Model.CustomerModel.StateName + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Post Code:" +
                            "                                                 <span id='lblpost'>" +
                            "                                                     <label id='lblZipCode'>" + Model.CustomerModel.ZipCode + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Country:" +
                            "                                                 <span id='lblcountry'>" +
                            "                                                     <label id='lblCountry'>" + Model.CustomerModel.Country + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Email:" +
                            "                                                 <span id='lblemail'>" +
                            "                                                     <label id='lblEmail'>" + Model.CustomerModel.EmailId + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Tel1:" +
                            "                                                 <span id='lblph'>" +
                            "                                                     <label id='lblPhone'>" + Model.CustomerModel.TelephoneNo + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Mobile:<span id='lblmob'>" +
                            "                                                     <label id='lblMobile'>" + Model.CustomerModel.MobileNo + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 Fax:<span id='lblfax'>" +
                            "                                                     <label id='lblFax'>" + Model.CustomerModel.Fax + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                            "                                             <td class='detail'>" +
                            "                                                 NIF/VAT:" +
                            "                                                 <span id='lblVat'>" +
                            "                                                     <label id='lblTaxID'>" + Model.CustomerModel.CompanyTaxId + "</label>" +
                            "                                                 </span>" +
                            "                                             </td>" +
                            "                                         </tr>" +
                            "                                         <tr>" +
                             "<tr>" +
                                                            "<td class='detail'>" +
                                                                "DeliveryDate:" +
                                                                "<span id='lblDeliv'>" +
                                                                    "<label id='lblDelivary'> " + Model.UserSelectDeliveryDateString + "</label>" +
                                                                "</span>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                          "<tr>" +
                                                            "<td class='detail'>" +
                                                                "BriderName:" +
                                                                "<span id='lblBride'>" +
                                                                    "<label id='lblBrideName'> " + Model.BridesName + "</label>" +
                                                                "</span>" +
                                                            "</td>" +
                                                        "</tr>" +
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
                            "                                                                     <label id='lblOrderDate'>" + Model.OrderDate + "</label>" +
                            "                                                                 </span>" +
                            "                                                             </td>" +
                            "                                                         </tr>" +
                            "                                                         <tr>" +
                            "                                                             <td class='header'>ORDER NO.:</td>" +
                            "                                                             <td class='detail' align='right'>" +
                            "                                                                 <span id='lblOrderNo'>" +
                            "                                                                     <label id='lblOrderReference'>" + Model.OrderNo + "</label>" +
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
                            "                         </tr> " +

                            "    					</tbody>" +
                            "                 </table>" +
                            "             </td>" +
                            "         </tr>" +
                            "     </tbody>" +
                            " </table>" +
                            " </div>" +


                           //Product Details     
                           //"    <div style='overflow-x:auto;'>" +
                           "    <div id='product_div'>" +
            "        <table class='product' style='width: 70%;'>" +
            "        <tbody><tr style='/* width: 100%; */display: table-row;vertical-align: inherit;border-color: inherit;'>" +
            "        <td class='order-item-details' style='vertical-align: top; display: table-cell;'>" +
            "         <div style='overflow-x:auto;'> " +
            "		 <table id='detailsTable' style='width:100%; padding-bottom:0px; padding-top:0px; display: table; border-collapse: separate; border-spacing: 2px; border-color: grey;' border='1' class='table'>" +

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
            "                        <th rowspan='3' style='min-width: 51px;'>Total</th>" +
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



            SizeData +
            "                </tbody>" +
            "                <tfoot>" +
            "                    <tr>" +
            "                        <td colspan='18' rowspan='5' style='width:10%'>" +
            "                            <div style='padding: 15px;height: 60px;border: 1px solid black;'>" +
            "                                <b>Order Note: </b>" + Model.OrderNote + "<br> " +
            "                            </div>" +
            "                        </td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Sub Total</b></td>" +
            "                        <td colspan='2'>" + amt + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Delivery</b></td>" +
            "                        <td colspan='2'>" + shippingcharge + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Tax</b></td>" +
            "                        <td colspan='2'>" + Math.Round(decimal.Parse((Model.Tax * Model.Amount / 100).ToString()), 2).ToString() + "</td>" +
            "                    </tr>" +
            "                    <tr>" +
            "                        <td colspan='3'><b>Total</b></td>" +
            "                        <td colspan='2'>" + TotalAmt + "</td>" +
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

                    #endregion
                }
                mail.IsBodyHtml = true;
                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));
                // set the network credentials
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);
                //smtp.EnableSsl = true;               
                //smtp.UseDefaultCredentials = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

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
                            "}" +

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
                            "}" +

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
        "                            <th>00</th>" +
        "                            <th>0</th>" +
        "                            <th>2</th>" +
        "							<th>4</th>" +
        "                            <th>6</th>" +
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
        "					<tr>                                         " +
        "                        <th>UK</th>" +
        "                        <th>2</th>" +
        "							<th>4</th>" +
        "                            <th>6</th>" +
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
        "                        <th>EU</th>" +
        "                         <th>30</th>" +
        "							<th>32</th>" +
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
        "                            <td></td>" +
        "							<td></td> " +
        "							<td></td> " +
        "							<td></td> " +
        "							<td>1</td>" +
        "							<td></td> " +
        "                            <td>1</td>" +
        "                            <td>1</td>" +
        "                            <td></td><td style='width:10%'></td><td style='width:10%'></td><td style='width:10%'></td><td style='width:10%'></td><td style='width:10%'></td><td style='width:10%'></td><td style='width:10%'></td><td style='width:10%'> " +
        "                            </td><td style='width:10%'> 3</td>" +
        "                            <td style='width:10%'> 597</td>" +
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
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from, "Loré Fashions");

            //mail.To.Add(Model.CustomerModel.EmailId);
            mail.To.Add("chandrashekhar.bairagi@gmail.com");
            mail.Subject = "Confirmation from supplier";
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential();
            networkCredential.UserName = "chandrashekhar.bairagi@connekt.in";
            networkCredential.Password = "connekt";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);

        }

        #endregion

        #region : Stock Inventory Update

   

        public bool UpdateOrderInventory(OrderViewModel Model)
        {
            try
            {
                Int64 distributionpoint = Convert.ToInt64(Model.CustomerModel.DistributionPointId);

              
                
                var stoqua = _context.StockQuantities.Where(k => k.ReferenceId == Model.Id).ToList();
                foreach (var item in stoqua)
                {
                    _context.StockQuantities.Remove(item);
                    _context.SaveChanges();
                }

                foreach (var data in Model.ProductList)
                {
                    foreach (var list in data.SizeModel)
                    {
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.InventoryType == "Order" && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == distributionpoint && x.StatusId != 4).FirstOrDefault();

                        OrderDetail orderDetail = _context.OrderDetails.FirstOrDefault(x => x.OrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.StatusId != 4);

                      //  if (StockDetail == null)
                        {
                            StockQuantity SQ = new StockQuantity();
                            SQ.InventoryType = "Order";
                            SQ.SizeUK = list.SizeUK;
                            SQ.ProductId = data.Product.Id;
                            SQ.ColourId = Convert.ToInt64(data.Product.ColourId);
                            SQ.WareHouseId = distributionpoint;
                            SQ.Qty = list.Qty;
                            SQ.CreatedById = Model.CreatedById;
                            SQ.CreationDate = DateTime.UtcNow;
                            SQ.ReferenceId = Model.Id;
                            _context.StockQuantities.Add(SQ);
                            _context.SaveChanges();
                        }
                       
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdatePOInventory(PurchaseOrderViewModel Model)
        {
            try
            {
                Int64 distributionpoint = Model.DistributionPoint;

              

                   #region Old code commented
                //if (Model.InsertFrom == "Order")
                //{
                //    foreach (var data in Model.ProductList)
                //    {
                //        foreach (var list in data.SizeModel)
                //        {
                //            StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "PO" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == distributionpoint && x.StatusId != 4).FirstOrDefault();
                //            if (StockDetail == null)
                //            {
                //                StockQuantity SQ = new StockQuantity();
                //                SQ.ReferenceId = Model.Id;
                //                SQ.InventoryType = "PO";
                //                SQ.SizeUK = list.SizeUK;
                //                SQ.ProductId = data.Product.Id;
                //                SQ.ColourId = Convert.ToInt64(data.Product.ColourId);
                //                SQ.WareHouseId = distributionpoint;
                //                SQ.Qty = 0;
                //                SQ.CreatedById = Model.CreatedById;
                //                SQ.CreationDate = DateTime.UtcNow;
                //                _context.StockQuantities.Add(SQ);
                //            }
                //            else
                //            {
                //                StockDetail.Qty = 0;
                //                StockDetail.ModifiedById = Model.CreatedById;
                //                StockDetail.ModificationDate = DateTime.UtcNow;
                //                //StockDetail.WareHouseId = Model.WareHouseId;
                //            }
                //            _context.SaveChanges();
                //        }
                //    }
                //}
                //else
                //{
                //    foreach (var data in Model.ProductList)
                //    {
                //        foreach (var list in data.SizeModel)
                //        {
                //            StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "PO" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == distributionpoint && x.StatusId != 4).FirstOrDefault();
                //            if (StockDetail == null)
                //            {
                //                StockQuantity SQ = new StockQuantity();
                //                SQ.ReferenceId = Model.Id;
                //                SQ.InventoryType = "PO";
                //                SQ.SizeUK = list.SizeUK;
                //                SQ.ProductId = data.Product.Id;
                //                SQ.ColourId = Convert.ToInt64(data.Product.ColourId);
                //                SQ.WareHouseId = distributionpoint;
                //                SQ.Qty = list.Qty;
                //                SQ.CreatedById = Model.CreatedById;
                //                SQ.CreationDate = DateTime.UtcNow;
                //                _context.StockQuantities.Add(SQ);
                //            }
                //            else
                //            {
                //                StockDetail.Qty = list.Qty;
                //                StockDetail.ModifiedById = Model.CreatedById;
                //                StockDetail.ModificationDate = DateTime.UtcNow;
                //                //StockDetail.WareHouseId = Model.WareHouseId;
                //            }
                //            _context.SaveChanges();
                //        }
                //    }

                //}
                #endregion
                   #region Old code 28-10-21
                //foreach (var data in Model.ProductList)
                //{
                //    foreach (var list in data.SizeModel)
                //    {
                //        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "PO" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == distributionpoint && x.StatusId != 4).FirstOrDefault();

                //        PurchaseOrderDetail PoDetail = _context.PurchaseOrderDetails.FirstOrDefault(x => x.PurchaseOrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.StatusId != 4);

                //        if (StockDetail == null)
                //        {
                //            StockQuantity SQ = new StockQuantity();
                //            SQ.ReferenceId = Model.Id;
                //            SQ.InventoryType = "PO";
                //            SQ.SizeUK = list.SizeUK;
                //            SQ.ProductId = data.Product.Id;
                //            SQ.ColourId = Convert.ToInt64(data.Product.ColourId);
                //            SQ.WareHouseId = distributionpoint;
                //            SQ.Qty = list.Qty;
                //            SQ.CreatedById = Model.CreatedById;
                //            SQ.CreationDate = DateTime.UtcNow;

                //            _context.StockQuantities.Add(SQ);
                //            PoDetail.IsInventoryAdded = true;
                //        }
                //        else
                //        {
                //            Int64 localQty = 0;
                //            if (PoDetail.Qty != list.Qty)
                //            {
                //                localQty = list.Qty - PoDetail.Qty;
                //            }
                //            else
                //            {
                //                if (PoDetail.IsInventoryAdded != true)
                //                {
                //                    localQty = list.Qty;
                //                }
                //            }

                //            StockDetail.Qty = StockDetail.Qty + localQty;
                //            StockDetail.ModifiedById = Model.CreatedById;
                //            StockDetail.ModificationDate = DateTime.UtcNow;
                //            //StockDetail.WareHouseId = Model.WareHouseId;

                //            PoDetail.IsInventoryAdded = true;
                //        }
                //        _context.SaveChanges();
                //    }
                //}
                #endregion

                var stoqua = _context.StockQuantities.Where(k => k.ReferenceId == Model.Id).ToList();
                foreach (var item in stoqua)
                {
                    _context.StockQuantities.Remove(item);
                    _context.SaveChanges();
                }
                foreach (var data in Model.ProductList)
                {
                    foreach (var list in data.SizeModel)
                    {
                        StockQuantity StockDetail = _context.StockQuantities.Where(x => x.SizeUK == list.SizeUK && x.InventoryType == "PO" && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.WareHouseId == distributionpoint && x.StatusId != 4).FirstOrDefault();

                        PurchaseOrderDetail PoDetail = _context.PurchaseOrderDetails.FirstOrDefault(x => x.PurchaseOrderMasterId == Model.Id && x.SizeUK == list.SizeUK && x.ColourId == data.Product.ColourId && x.ProductId == data.Product.Id && x.StatusId != 4);

                      //  if (StockDetail == null)
                        {
                            StockQuantity SQ = new StockQuantity();
                            SQ.ReferenceId = Model.Id;
                            SQ.InventoryType = "PO";
                            SQ.SizeUK = list.SizeUK;
                            SQ.ProductId = data.Product.Id;
                            SQ.ColourId = Convert.ToInt64(data.Product.ColourId);
                            SQ.WareHouseId = distributionpoint;
                            SQ.Qty = list.Qty;
                            SQ.CreatedById = Model.CreatedById;
                            SQ.CreationDate = DateTime.UtcNow;

                            _context.StockQuantities.Add(SQ);
                            PoDetail.IsInventoryAdded = true;
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

        #region: Frontend work

        public List<OrderViewModel> GetAllOrderList()
        {
            try
            {
                var result = _context.OrderMasters.Where(x => x.StatusId != 4)
                    .Select(ToOrderViewModel).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<OrderViewModel>();
            }
        }

        public List<OrderViewModel> GetAllOrderListByUserid(Int64 userid)
        {
            try
            {
                var result = _context.OrderMasters.Where(x => x.StatusId != 4 && x.CustomerId == userid).ToList()
                    .Select(ToOrderViewModel).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<OrderViewModel>();
            }
        }

        public List<ProductImage> GetDownloadImagesbyProduct(Int64 ab)
        {

            try
            {
                List<ProductImage> ProimgList = new List<ProductImage>();
                var productList = ProimgList.Where(k => k.ProductId == ab).FirstOrDefault();
                if (productList == null)
                {
                    var productdet = _context.Products.Where(k => k.Id == ab && k.StatusId != 4).FirstOrDefault();
                    if (productdet.Picture1 != null)
                    {
                        ProductImage mod = new ProductImage();
                        if (productdet.Picture1 != null)
                        {
                            mod.ImagePath2 = productdet.Picture1;
                        }
                        if (productdet.Picture2 != null)
                        {
                            mod.ImagePath3 = productdet.Picture2;
                        }
                        if (productdet.Picture3 != null)
                        {
                            mod.ImagePath4 = productdet.Picture3;
                        }
                        mod.ProductId = productdet.Id;
                        ProimgList.Add(mod);
                    }

                }
                return ProimgList;
            }
            catch (Exception ex)
            {
                return new List<ProductImage>();
            }

        }


        public List<ProductImage> GetAllOrderImageListByUserid(Int64 userid)

        {
            try
            {
                List<ProductImage> ProimgList = new List<ProductImage>();

                var result = _context.OrderMasters.Where(x => x.StatusId != 4 && x.CustomerId == userid).ToList()
                    .Select(ToOrderViewModel).ToList();

                foreach (var item in result)
                {
                    var orderdetail = _context.OrderDetails.Where(k => k.OrderMasterId == item.Id).ToList();

                    foreach (var itemdetail in orderdetail)
                    {
                        var productList = ProimgList.Where(k => k.ProductId == itemdetail.ProductId).FirstOrDefault();
                        if (productList == null)
                        {
                            var productdet = _context.Products.Where(k => k.Id == itemdetail.ProductId).FirstOrDefault();
                            if (productdet.Picture1 != null)
                            {
                                ProductImage mod = new ProductImage();
                                mod.ImagePath = "http://lorefashions.com" + productdet.Picture1;
                                mod.ImagePath11 = "http://lorefashions.com" + productdet.Picture2;
                                mod.ImagePath12 = "http://lorefashions.com" + productdet.Picture3; //http://localhost:54414
                                mod.ImagePath2 = productdet.Picture1;
                                mod.ImagePath3 = productdet.Picture2;
                                mod.ImagePath4 = productdet.Picture3;
                                mod.ProductId = productdet.Id;
                                ProimgList.Add(mod);
                            }

                        }

                    }
                }

                return ProimgList;
            }
            catch (Exception ex)
            {
                return new List<ProductImage>();
            }
        }


        public OrderViewModel GetOrderByStatusId(int id)
        {
            OrderViewModel model = new OrderViewModel();

            OrderMaster master = _context.OrderMasters.FirstOrDefault(x => x.OrderStatusId == id);
            if (master != null)
            {
                return ToOrderViewModel(master);
            }
            else
            {
                return new OrderViewModel();
            }
        }

        #endregion


    }
}
