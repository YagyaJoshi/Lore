using DataTables.Mvc;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loregroup.Controllers
{
    public class UploadFileController : Controller
    {
        
        private readonly IWidgetProvider _widgetProvider;
        private readonly IUserProvider _userProvider;
        private readonly ISession _session;
        private readonly IErrorHandler _errorHandler;
        private readonly INotificationProvider _notificationprovider;
        private readonly IMasterProvider _masterProvider;
        private readonly AppContext _context;
        private readonly ICacheService _dataCache;
        private readonly IUploadFileProvider _uploadFileProvider;

        public UploadFileController(IUploadFileProvider uploadFileProvider,AppContext context, IErrorHandler errorHandler, ISession session, IUserProvider userProvider, IWidgetProvider widgetProvider, INotificationProvider notificationprovider, IMasterProvider masterProvider, ICacheService dataCache)
        {
            _errorHandler = errorHandler;
            _session = session;
            _userProvider = userProvider;
            _widgetProvider = widgetProvider;
            _notificationprovider = notificationprovider;
            _masterProvider = masterProvider;
            _context = context;
            _dataCache = dataCache;
            _uploadFileProvider = uploadFileProvider;
        }       
      
        
        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }


        #region : Upload File

        public ActionResult UploadFile() {
            try {
                var Current = _session.CurrentUser.Id;

            ConsumersViewModel model = new ConsumersViewModel();
            var breadCrumbModel = new BreadCrumbModel()
            {
                Title = "Consumers",
                SubBreadCrumbModel = new BreadCrumbModel()
                {
                    SubBreadCrumbModel = null,
                    Title = "Consumers List"
                }
            };
            ViewBag.Title = breadCrumbModel.Title;
            ViewBag.BreadCrumb = breadCrumbModel;

            


            return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file) {

            List<ConsumersViewModel> datalist = new List<ConsumersViewModel>();
            FileUploadViewModel fm = new FileUploadViewModel();
            fm.UploadMsg = "";        
            DataSet ds = new DataSet();         
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/UploadFile/",
                Title = "Upload File",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;


            if (file.ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {

                    //string filename =DateTime.Now.ToString("hh:mm:ss")+FileUpload.FileName;
                    string[] stAFileName = file.FileName.Split('.');

                    string strFileName = DateTime.Now.ToString("hhmmss") + "." + stAFileName[1];
                    string fileLocation = Server.MapPath("~/Content/") + strFileName;
                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    catch
                    {
                        //System.IO.File.Delete(fileLocation);
                        //FileUpload.SaveAs(fileLocation);
                    }

                    file.SaveAs(fileLocation);
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
                        return null;
                    }

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
                    if (ds != null)
                    {
                        dt1 = ds.Tables[0];
                        if (dt1 != null)
                        {
                          

                            foreach (DataRow dr in dt1.Rows)
                            { 

                                ConsumersViewModel item = new ConsumersViewModel();
                                item.Equipment = dr[0].ToString();
                                item.Scheme = dr[1].ToString();
                                item.BookTime = Convert.ToDateTime(dr[2].ToString());
                                item.BookNo = dr[3].ToString();
                                item.BStatus = dr[5].ToString();
                                item.BookingDate = Convert.ToDateTime(dr[2].ToString());
                                item.ConsumerId = Convert.ToInt64(dr[6]);
                                item.ConsumerNo = dr[7].ToString();
                                item.Name = dr[8].ToString();
                                item.NoOfCycles = Convert.ToInt32(dr[9].ToString());
                                try
                                {
                                    item.CashMemoNo = dr[10].ToString();
                                }
                                catch(Exception ex)
                                {
                                    item.CashMemoNo = "";
                                }

                                try
                                {
                                    item.CashMemoDate = Convert.ToDateTime(dr[11].ToString());
                                }
                                catch (Exception ex)
                                {
                                    item.CashMemoDate = null;
                                }
                                try
                                {
                                    item.DeliveryDate = Convert.ToDateTime(dr[12].ToString());
                                }
                                catch (Exception ex)
                                {
                                    item.DeliveryDate = null;
                                }
                                try
                                {
                                    item.DeliveryTime = Convert.ToDateTime(dr[13].ToString());
                                }
                                catch (Exception ex)
                                {
                                    item.DeliveryTime = null;
                                }                               
                                                             
                                item.UserId = dr[14].ToString();
                                item.InstallationOnBooking = dr[15].ToString();
                                item.IvrsReferenceNo = dr[16].ToString();
                                item.DBTLStatus = dr[17].ToString();
                              
                                item.ModifiedById = _session.CurrentUser.Id;
                                item.CreationDate = System.DateTime.UtcNow;
                                item.CreatedById = _session.CurrentUser.Id;

                                datalist.Add(item);
                            }
                        }
                    }
                } 
                catch(Exception ex){}
            }
            var result = _uploadFileProvider.ConsumerRegistration(datalist);
            if (result != "")
            {
                fm.UploadMsg = result;
            }
            else
                fm.UploadMsg = "Excel File Template does not match";
           
            return View(fm);
        }

        [HttpPost]

        
        public JsonResult GetUploadedData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string startDateStr, string endDateStr, string statusvalue, int mpvalue)
        {
          List<ConsumersViewModel> ConsumerList = new List<ConsumersViewModel>();
          var totalCount =   _context.Consumers.ToList().Count ;
          var filteredCount = 0;
          if (statusvalue != "" && startDateStr != "" && endDateStr != "" && statusvalue != null && startDateStr != null && endDateStr != null && statusvalue != "0")
              {

                  int val = Convert.ToInt32(statusvalue);

                  if (val != 0 && val != null)
                      if (val == 1)
                      {
                          statusvalue = "Delivered";
                      }
                  if (val != 0 && val != null)
                      if (val == 2)
                      {
                          statusvalue = "Under Delivery";
                      }

                  if (val != 0 && val != null)
                      if (val == 3)
                      {
                          statusvalue = "Open";
                      }
              ConsumerList = _uploadFileProvider.GetAllCon(statusvalue, startDateStr, endDateStr);
              filteredCount = ConsumerList.Count;
              }

         else if (statusvalue != "" && statusvalue != "0" && statusvalue != null)
          {
              int val = Convert.ToInt32(statusvalue);

              if (val != 0 && val != null)
                  if (val == 1) {
                      statusvalue = "Delivered";
                  }
              if (val != 0 && val != null)
                  if (val == 2)
                  {
                      statusvalue = "Under Delivery";
                  }

              if (val != 0 && val != null)
                  if (val == 3)
                  {
                      statusvalue = "Open";
                  }
                         
              ConsumerList = _uploadFileProvider.GetAllConsumersAccStatus(statusvalue);
              filteredCount = ConsumerList.Count;
          }
       

          else if (startDateStr != "" && endDateStr != "" && startDateStr != null && endDateStr != null)
          {
              ConsumerList = _uploadFileProvider.GetAllConsumersAccBookinDate(startDateStr, endDateStr);
              filteredCount = ConsumerList.Count;
          }

          else
          {
              ConsumerList = _uploadFileProvider.GetAllConsumers(null, 0, 0);
              filteredCount = ConsumerList.Count;
          }        
            IQueryable<Consumer> query = _context.Consumers;
            int userid = Convert.ToInt32(_session.CurrentUser.Id);
            query = query.Where(x => x.Id == userid);
            return Json(new DataTablesResponse(requestModel.Draw, ConsumerList, filteredCount, totalCount), JsonRequestBehavior.AllowGet);

        }

        #endregion 

    }
}