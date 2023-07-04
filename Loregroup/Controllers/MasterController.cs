using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Core.ViewModels;
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Logmodels;
using System.IO;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces;
using Loregroup.Core.ViewModels;
using Loregroup.Core;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using OfficeOpenXml;
using DataTables.Mvc;
using Loregroup.Data.Entities;

namespace Loregroup.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterProvider _MasterProvider;
        private readonly IUserProvider _userProvider;
        private readonly ISession _session;
        private readonly IUtilities _utilities;
        public static long packageid = 0;
        private readonly AppContext _context;
        private readonly ICommonProvider _commonProvider;
        private readonly IDeliverySlipProvider _deliveryslipProvider;
        public static Int64? abc = 0;
        public static Int64? def = 0;
        public static string CityName = "";
        public static string AreaName = "";
        public static DateTime? StartDate = null;
        public static DateTime? EndDate = null;
        public static Int64 selectedUserId = 0;


        public MasterController(IDeliverySlipProvider deliveryslipProvider, AppContext context, IConfigSettingProvider configProvider, IContentProvider contentProvider, IUserProvider userProvider, IMasterProvider masterProvider, ISession session, IUtilities utilities, ICommonProvider commonProvider)
        {
            _MasterProvider = masterProvider;
            _userProvider = userProvider;
            _session = session;
            _context = context;
            _utilities = utilities;
            _commonProvider = commonProvider;
            _deliveryslipProvider = deliveryslipProvider;
        }

        #region : Old
        
        #region : Image
        public FileContentResult MyImage()
        {
            // Load an existing image
            using (var img = Image.FromFile(Server.MapPath("\\Content\\Customer\\User.jpg")))
            using (var g = Graphics.FromImage(img))
            {
                using (var stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Png);
                    return File(stream.ToArray(), "image/png");
                }
            }
        }

        public ActionResult ShowAllMall()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Mall", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        public ActionResult ShowAllApprovedMall()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Approved Mall", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        public ActionResult slider()
        {

            return View();
        }

        public ActionResult AllProductForAdmin()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Product", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        public FileContentResult ProfilePic()
        {

            // Load an existing image
            using (var img = Image.FromFile(Server.MapPath("\\Content\\Customer\\MallPicture.jpg")))


            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

        public ActionResult ShowAllPendingMobileApp()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Mobile App", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public JsonResult GetSingleUserDetail(Int64 id)
        {
            //UserViewModel UserViewModel = _MasterProvider.GetUser(id);
            //if (UserViewModel.ProfileImage == null)
            //{
            //    FileContentResult abc = ProfilePic();
            //    byte[] xyz = abc.FileContents;
            //    var base64 = Convert.ToBase64String(xyz);

            //    var imgSrc10 = String.Format("data:image/gif;base64,{0}", base64);

            //    UserViewModel.Profile = imgSrc10;

            //}
            //else
            //{
            //    var base64 = Convert.ToBase64String(UserViewModel.ProfileImage);
            //    var imgSrc10 = String.Format("data:image/gif;base64,{0}", base64);
            //    UserViewModel.Profile = imgSrc10;
            //}
            JsonObjectModel<String> model = new JsonObjectModel<String>()
            {
                Object = "",//_utilities.RenderPartialViewToString(this, "_UserDetailTurnover", UserViewModel),
                Message = "offer fetched successfully",
                Result = true
            };
            return Json(model);
        }

        public ActionResult ShowAllPendingClassified()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Classified", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        //

        //Show notification for admin
        public ActionResult ShowAdminNotification()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Notification", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }


        [HttpPost]
        public JsonResult GetAdminnotification(int size, int page)
        {
            var Classified = _MasterProvider.GetAdminnotification(null, 0, 0);

            JsonListModel<NotificationsViewModel> model = new JsonListModel<NotificationsViewModel>
            {
                List = Classified,
                Message = "records fetched successfully",
                Result = true
            };
            return Json(model);
        }

        [HttpPost]
        public JsonResult GetSingleNotification(Int64 id)
        {

            NotificationsViewModel classifiedViewModel = _MasterProvider.Getnotification(id);

            JsonObjectModel<String> model = new JsonObjectModel<String>()
            {
                Object = _utilities.RenderPartialViewToString(this, "_NotificationDetailTurnover", classifiedViewModel),
                Message = "offer fetched successfully",
                Result = true
            };


            return Json(model);
        }

        [HttpPost]
        public JsonResult DeleteNotification(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);
                        _MasterProvider.DeleteNotification(Id, (int)_session.CurrentUser.Id);


                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    _MasterProvider.DeleteNotification(Id, (int)_session.CurrentUser.Id);



                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");

                return Json(id);

            }
            catch (Exception ex)
            {
                LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
                //return RedirectToAction("ShowAdminNotification", "Master");
            }
        }


        #endregion

        public FileContentResult OfferImage()
        {

            //Add By  
            // Load an existing image
            using (var img = Image.FromFile(Server.MapPath("\\Content\\Customer\\Offer.jpg")))



            // Write the resulting image to the response stream
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

        public Image VariousQuality(object value, int no)
        {

            //Add By  
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

        public byte[] imgToByteArray(Image img)
        {

            //Add By  
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        //public ActionResult AdminBanner()
        //{
        //    var breadCrumbModel = new BreadCrumbModel()
        //    {
        //        Url = "/Home/",
        //        Title = "Add Banner",
        //        SubBreadCrumbModel = null
        //    };
        //    ViewBag.BreadCrumb = breadCrumbModel;
        //    BannerViewModel Model = new BannerViewModel();



        //    Model.CityViewModel.CityList = _MasterProvider.GetAllCity(null, 0, 0);
        //    Model.CityViewModel.CityList.Insert(0, new CityViewModel
        //    {
        //        Id = 0,
        //        Cityname = "--Select City--"
        //    });

        //    return View(Model);
        //}

        public bool IsValidBannerimage(object value)
        {
            bool isimg = true;

            if (value == null)
            {
                return true;
            }
            else
            {
                Int64 Width = 1120;
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
        
        public byte[] httptobyte(HttpPostedFileBase fil)
        {            
            int fileSizeInBytes = fil.ContentLength;
            MemoryStream target = new MemoryStream();
            fil.InputStream.CopyTo(target);
            byte[] data = target.ToArray();
            return data;
        }

        ////30/08/16 Email
        //#region : Send Email
        //public ActionResult Sendmail()
        //{
        //    return View();

        //}

        //[HttpPost]
        //public ActionResult Sendmail(EmailViewModel Model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        //    string from ="sulabh110887@gmail.com"; //any valid GMail ID  
        //        //    using (MailMessage mail = new MailMessage(from, Model.To))
        //        //    {
        //        //        mail.Subject = Model.Subject;
        //        //        mail.Body = Model.Body;
        //        //        //if (fileUploader != null)
        //        //        //{
        //        //        //    string fileName = Path.GetFileName(fileUploader.FileName);
        //        //        //    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
        //        //        //}
        //        //        mail.IsBodyHtml = false;
        //        //        SmtpClient smtp = new SmtpClient();
        //        //        smtp.Host = "smtp.gmail.com";
        //        //        smtp.EnableSsl = true;
        //        //        NetworkCredential networkCredential = new NetworkCredential();
        //        //        networkCredential.UserName = "lskallcenter@gmail.com";

        //        //        networkCredential.Password = "monit@2014";
        //        //        smtp.UseDefaultCredentials = true;
        //        //        smtp.Credentials = networkCredential;
        //        //        smtp.Port = 587;
        //        //        //smtp.Host = "localhost";
        //        //        smtp.Send(mail);
        //        //        ViewBag.Message = "Sent";
        //        SendEmail(Model);
        //        return View("Sendmail", Model);
        //        //}
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        ////public void SendEmail(EmailViewModel Model)
        ////{

        ////    if (ModelState.IsValid)
        ////    {

        ////        string from = "sulabh110887@gmail.com"; //any valid GMail ID  
        ////        using (MailMessage mail = new MailMessage(from, Model.To))
        ////        {
        ////            mail.Subject = Model.Subject;
        ////            mail.Body = Model.Body;
        ////            //if (fileUploader != null)
        ////            //{
        ////            //    string fileName = Path.GetFileName(fileUploader.FileName);
        ////            //    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
        ////            //}
        ////            mail.IsBodyHtml = false;
        ////            SmtpClient smtp = new SmtpClient();
        ////            smtp.Host = "smtp.gmail.com";
        ////            smtp.EnableSsl = true;
        ////            NetworkCredential networkCredential = new NetworkCredential();
        ////            networkCredential.UserName = "lskallcenter@gmail.com";

        ////            networkCredential.Password = "monit@2014";
        ////            smtp.UseDefaultCredentials = true;
        ////            smtp.Credentials = networkCredential;
        ////            smtp.Port = 587;
        ////            //smtp.Host = "localhost";
        ////            smtp.Send(mail);
        ////            ViewBag.Message = "Sent";

        ////        }
        ////    }
        ////    else
        ////    {

        ////    }

        ////}

        //#endregion
        //#region : Send Email
        //[HttpPost]
        //public JsonResult DisapproveOfferWithReason(string id)
        //{
        //    ReasonViewModel reasonModel = new ReasonViewModel();
        //    reasonModel.userId = id;
        //    JsonObjectModel<String> model = new JsonObjectModel<String>()
        //    {
        //        Object = _utilities.RenderPartialViewToString(this, "_DisapproveReasonTurnover", reasonModel),
        //        Message = "Success",
        //        Result = true
        //    };
        //    return Json(model);
        //}

        ////[HttpPost]
        ////public ActionResult DisapproveOfferWithReasonSubmit(ReasonViewModel model)

        ////{
        ////   //Int64 Id = Convert.ToInt32(model.userId);
        ////   // Int32 stid =
        ////   // if (stid == 3 || stid == 5)
        ////   // 
        ////   // //return View(model);

        ////    Int64 Id = 0;
        ////    Int32 stid = 0;
        ////    if (model.userId.Contains(','))
        ////    {
        ////        string[] arr = model.userId.Split(',');
        ////        for (int i = 0; i < arr.Length; i++)
        ////        {
        ////            Id = Convert.ToInt32(arr[i]);
        ////            stid = _MasterProvider.GetstatusID(Id);
        ////            if (stid == 3)
        ////                _MasterProvider.DisapproveOfferWithReason(model);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        Id = Convert.ToInt32(model.userId);
        ////        stid = _MasterProvider.GetstatusID(Id);
        ////        if (stid == 3)
        ////            _MasterProvider.DisapproveOfferWithReason(model);
        ////        //return View(model);
        ////    }


        ////    return RedirectToAction("Showallpendingoffer");
        ////}

        //#endregion

        //#region: Disapprove Classified with Reason
        //[HttpPost]
        //public JsonResult DisapproveClassifiedWithReason(string id)
        //{
        //    ReasonViewModel reasonModel = new ReasonViewModel();
        //    reasonModel.userId = id;
        //    JsonObjectModel<String> model = new JsonObjectModel<String>()
        //    {
        //        Object = _utilities.RenderPartialViewToString(this, "_ClassifiedDisapproveReason", reasonModel),
        //        Message = "Success",
        //        Result = true
        //    };
        //    return Json(model);
        //}




        //#endregion

        //#region: Disapprove Store with Reason
        //[HttpPost]
        //public JsonResult DisapproveStoreWithReason(string id)
        //{
        //    ReasonViewModel reasonModel = new ReasonViewModel();
        //    reasonModel.userId = id;
        //    JsonObjectModel<String> model = new JsonObjectModel<String>()
        //    {
        //        Object = _utilities.RenderPartialViewToString(this, "_StoreDisapproveReason", reasonModel),
        //        Message = "Success",
        //        Result = true
        //    };
        //    return Json(model);
        //}


        //#endregion

        //#region: Disapprove Mall with Reason
        //[HttpPost]
        //public JsonResult DisapproveMallWithReason(string id)
        //{
        //    ReasonViewModel reasonModel = new ReasonViewModel();
        //    reasonModel.userId = id;
        //    JsonObjectModel<String> model = new JsonObjectModel<String>()
        //    {
        //        Object = _utilities.RenderPartialViewToString(this, "_MallDisapproveReason", reasonModel),
        //        Message = "Success",
        //        Result = true
        //    };
        //    return Json(model);
        //}

        //#endregion




        //Add advertisement by sulabh

        public ActionResult AddAdvertisement()
        {
            //  var breadCrumbModel = new BreadCrumbModel()
            //  {
            //      Url = "/Home/",
            //      Title = "Add Advertisement",
            //      SubBreadCrumbModel = null
            //  };
            //  ViewBag.BreadCrumb = breadCrumbModel;

            //  Session["Package"] = "Advertisement";
            //  //AdvertisementViewModel Model = new AdvertisementViewModel();
            //  Model.CityViewModel.CityList = _MasterProvider.GetAllCity(null, 0, 0);
            //  Model.CityViewModel.CityList.Insert(0, new CityViewModel
            //  {
            //      Id = 0,
            //      Cityname = "--Select City--"
            //  });

            //  FileContentResult abc = ProfilePic();
            //  byte[] xyz = abc.FileContents;
            //  var base64 = Convert.ToBase64String(xyz);

            //  var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            ////  Model.Picture = imgSrc;
            //  return View(Model);
            return View();
        }

        public ActionResult AddAdvertisementnew()
        {
            // AdvertisementViewModel Model = new AdvertisementViewModel();
            //Model.CityViewModel.CityList = _MasterProvider.GetAllCity(null, 0, 0);
            //Model.CityViewModel.CityList.Insert(0, new CityViewModel
            //{
            //    Id = 0,
            //    Cityname = "--Select City--"
            //});


            return View();
        }
        
        //public ActionResult PartialEditCompany()
        //{
        //    //stuff you need and then return the partial view 
        //    //I recommend using "" quotes for a partial view
        //    return PartialView("ShowSheduler");
        //}

        public ActionResult ShowallAdminAdvertisement()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Sdvertisement", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        //#region: Disapprove Advertise with Reason
        //[HttpPost]
        //public JsonResult DisapproveAdvertisementWithReason(string id)
        //{
        //    ReasonViewModel reasonModel = new ReasonViewModel();
        //    reasonModel.userId = id;
        //    JsonObjectModel<String> model = new JsonObjectModel<String>()
        //    {
        //        Object = _utilities.RenderPartialViewToString(this, "_AdvertisementDisapproveReason", reasonModel),
        //        Message = "Success",
        //        Result = true
        //    };
        //    return Json(model);
        //}


        //#endregion

        //#region: Send mail to user
        //[HttpPost]
        //public JsonResult Sendmailtouser(string id)
        //{
        //    ReasonViewModel reasonModel = new ReasonViewModel();
        //    reasonModel.userId = id;
        //    JsonObjectModel<String> model = new JsonObjectModel<String>()
        //    {
        //        Object = _utilities.RenderPartialViewToString(this, "_SendMailToUser", reasonModel),
        //        Message = "Success",
        //        Result = true
        //    };
        //    return Json(model);
        //}

        //[HttpPost]
        //public ActionResult SendmailtoAppUsersubmit(ReasonViewModel model)
        //{
        //    Int64 Id = 0;
        //    Int32 stid = 0;
        //    EmailViewModel Model = new EmailViewModel();
        //    if (model.userId.Contains(','))
        //    {
        //        string[] arr = model.userId.Split(',');
        //        for (int i = 0; i < arr.Length; i++)
        //        {
        //            Model = new EmailViewModel();
        //            Id = Convert.ToInt32(arr[i]);
        //            string emailid = _context.AppUsers.Where(x => x.Id == Id).Select(k => k.Email).FirstOrDefault();
        //            Model.To = emailid;
        //            Model.Subject = "City-Dekh Info";
        //            Model.Body = model.DisapproveReason;
        //            SendEmail(Model);

        //        }
        //    }
        //    else
        //    {
        //        Id = Convert.ToInt32(model.userId);
        //        string emailid = _context.AppUsers.Where(x => x.Id == Id).Select(k => k.Email).FirstOrDefault();
        //        Model.To = emailid;
        //        Model.Subject = "City-Dekh Info";
        //        Model.Body = model.DisapproveReason;
        //        SendEmail(Model);

        //    }
        //    return RedirectToAction("ShowAllUser", "Master");
        //}
        //#endregion
        
        public void SendEmail(EmailViewModel Model)
        {
            //if (ModelState.IsValid)
            try
            {
                string from = "sulabh110887@gmail.com"; //any valid GMail ID  
                using (MailMessage mail = new MailMessage(from, Model.To))
                {
                    mail.Subject = Model.Subject;
                    mail.Body = Model.Body;
                    //if (fileUploader != null)
                    //{
                    //    string fileName = Path.GetFileName(fileUploader.FileName);
                    //    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                    //}
                    mail.IsBodyHtml = false;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    //smtp.Host = " smtp.zoho.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential();
                    //networkCredential.UserName = "lskallcenter@gmail.com";
                    networkCredential.UserName = "CheckingAppindore@gmail.com";
                    //networkCredential.Password = "monit@2014";
                    networkCredential.Password = "Shilpa123";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    //smtp.Host = "localhost";
                    smtp.Send(mail);
                    ViewBag.Message = "Sent";

                }
            }
            //else
            catch (Exception ex)
            {

            }

        }
        
        public ActionResult Minal()
        {
            return View();
        }

        //public ActionResult AdminFooterAd()
        //{
        //    AdminAdFooterViewModel Model = new AdminAdFooterViewModel();
        //    Model.CityViewModel.CityList = _MasterProvider.GetAllCity(null, 0, 0);
        //    Model.CityViewModel.CityList.Insert(0, new CityViewModel
        //    {
        //        Id = 0,
        //        Cityname = "--Select City--"
        //    });
        //    FileContentResult abc = ProfilePic();
        //    byte[] xyz = abc.FileContents;
        //    var base64 = Convert.ToBase64String(xyz);

        //    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

        //    Model.Picture = imgSrc;
        //    return View(Model);
        //}
        
        public ActionResult GetallAdminFooterAd()
        {
            return View();
        }
        
        //[HttpPost]
        //public ActionResult EditAdminFooterAd(AdminAdFooterViewModel Model, HttpPostedFileBase file)
        //{
        //    FileContentResult abc;
        //    string y = null;
        //    byte[] xyz = new byte[0];
        //    byte[] quy;
        //    bool isvalidimg;
        //    int permittedSizeInBytes = 5242880;


        //    if (ModelState.IsValid)
        //    {
        //        if (Model.CityId != 0)
        //        {
        //            if (file != null)
        //            {
        //                isvalidimg = IsValidAddimage(file);
        //                if (isvalidimg == false)
        //                {
        //                    TempData["Error1"] = "Advertisement 1 Image size should be (width 270Px and height 180Px)";

        //                }
        //                else if (file.ContentLength > permittedSizeInBytes)
        //                {
        //                    TempData["Error1"] = "Advertisement 1 Image size should be less then 5mb";

        //                }
        //                else
        //                {
        //                    int uid = 360;
        //                    Random rndm = new Random();
        //                    if (uid > 255)
        //                    {
        //                        uid = rndm.Next(0, 355);
        //                    }
        //                    Image img = VariousQuality(file, uid);
        //                    System.Drawing.Bitmap imagebit = new Bitmap(Server.MapPath("~/Content/images1/quality_" + uid + ".jpeg"));
        //                    img = (Image)imagebit;
        //                    xyz = imgToByteArray(img);

        //                    //_MasterProvider.UpdateAdminAdFooter(Model, xyz);
        //                    //Code Start to save image in folder : CS

        //                    string path = ConfigurationManager.AppSettings["AdFooterAdminPath"].ToString();
        //                    path = Server.MapPath(path);
        //                    _commonProvider.SaveBannerImageInFolder(Model.Id, path, file, Model.Id.ToString());

        //                    //Code End to save image in folder


        //                    return RedirectToAction("GetallAdminFooterAd");
        //                }
        //            }
        //            else
        //            {
        //                //_MasterProvider.UpdateAdminAdFooter(Model, xyz);


        //                return RedirectToAction("GetallAdminFooterAd");
        //            }
        //        }
        //        else
        //        {
        //            TempData["Error1"] = "Please select city";

        //        }



        //    }
        //    Model.CityViewModel.CityList = _MasterProvider.GetAllCity(null, 0, 0);
        //    Model.CityViewModel.CityList.Insert(0, new CityViewModel
        //    {
        //        Id = 0,
        //        Cityname = "--Select City--"
        //    });
        //    abc = ProfilePic();
        //    quy = abc.FileContents;
        //    var base64 = Convert.ToBase64String(quy);

        //    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

        //    Model.Picture = imgSrc;
        //    return View(Model);
        //}


        //// Advertisement for admin right side

        //public ActionResult RightBanner()
        //{
        //    var breadCrumbModel = new BreadCrumbModel()
        //    {
        //        Url = "/Home/",
        //        Title = "Right Side Advertisement",
        //        SubBreadCrumbModel = null
        //    };
        //    ViewBag.BreadCrumb = breadCrumbModel;
        //    AdminAdvertisementViewModel Model = new AdminAdvertisementViewModel();

        //    FileContentResult abc = ProfilePic();
        //    byte[] xyz = abc.FileContents;
        //    var base6411 = Convert.ToBase64String(xyz);

        //    var imgdef = String.Format("data:image/gif;base64,{0}", base6411);
        //    try
        //    {
        //        int lenth = Model.Banner1.Length;
        //        if (lenth > 1)
        //        {
        //            var base64 = Convert.ToBase64String(Model.Banner1);

        //            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

        //            Model.Image1 = imgSrc;
        //        }
        //        else
        //            Model.Image1 = imgdef;
        //    }
        //    catch (Exception ex)
        //    {
        //        Model.Image1 = imgdef;
        //    }

        //    try
        //    {
        //        int lenth1 = Model.Banner2.Length;
        //        if (lenth1 > 1)
        //        {
        //            var base641 = Convert.ToBase64String(Model.Banner2);
        //            var imgSrc1 = String.Format("data:image/gif;base64,{0}", base641);
        //            Model.Image2 = imgSrc1;
        //        }
        //        else
        //            Model.Image2 = imgdef;
        //    }
        //    catch (Exception ex)
        //    {
        //        Model.Image2 = imgdef;
        //    }

        //    try
        //    {
        //        int lenth2 = Model.Banner3.Length;
        //        if (lenth2 > 1)
        //        {
        //            var base642 = Convert.ToBase64String(Model.Banner3);
        //            var imgSrc2 = String.Format("data:image/gif;base64,{0}", base642);
        //            Model.Image3 = imgSrc2;
        //        }
        //        else
        //            Model.Image3 = imgdef;
        //    }
        //    catch (Exception ex)
        //    {
        //        Model.Image3 = imgdef;
        //    }
        //    Model.CityViewModel.CityList = _MasterProvider.GetAllCity(null, 0, 0);
        //    Model.CityViewModel.CityList.Insert(0, new CityViewModel
        //    {
        //        Id = 0,
        //        Cityname = "--Select City--"
        //    });

        //    return View(Model);
        //}
        
        public bool IsValidAddimage(object value)
        {
            bool isimg = true;

            if (value == null)
            {
                return true;
            }
            else
            {
                Int64 Width = 270;
                Int64 Height = 180;
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

        public ActionResult Upload(FormCollection fc)
        {

            if (Request != null)
            {

                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    var usersList = new List<Users>();
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var user = new Users();
                            user.FirstName = workSheet.Cells[rowIterator, 1].Value.ToString();
                            user.LastName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            usersList.Add(user);
                        }
                    }
                }
            }
            return View();


        }
        
        #endregion

        #region: Block
        //For Show All Block
        public ActionResult ShowAllBlock()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Block", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }
        //Method For Get All Block
        [HttpPost]
        public JsonResult GetAllBlock(int size, int page)
        {
            var Blocklist = _MasterProvider.GetAllBlock(null, null, 0, 0, 0);//.GetAllDistrict(null, null, 0, 0, 0);
            JsonListModel<BlocksViewModel> model = new JsonListModel<BlocksViewModel>
            {
                List = Blocklist,
                Message = "records fetched successfully",
                Result = true
            };

            return Json(model);
        }

        //For Block Master
        public ActionResult Block()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Add New Block", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                BlocksViewModel model = new BlocksViewModel();
                var categories = _context.Districts.Select(c => new { CategoryID = c.Id, CategoryName = c.DistricName, }).ToList();
                ViewBag.Categories = new MultiSelectList(categories, "CategoryID", "CategoryName");
                model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
                model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel { Id = 0, DistrictName = "--Select District Name--" });
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult Block(BlocksViewModel Model)
        {
            if (Model.DistricId == 0)
            {
                TempData["Error1"] = "Please Select District";
                Model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);//.GetAllStates(null, null, 0, 0, 0);
                Model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel
                {
                    Id = 0,
                    DistrictName = "--Select District Name--"
                });

            }

            if (ModelState.IsValid)
            {

                //Int64 Id = _context.Districts.Where(x => x.DistricName == Model.DistricName).Select(x => x.Id).FirstOrDefault();
                Int64 Id = _context.Blocks.Where(x => x.BlockName == Model.BlockName).Select(x => x.Id).FirstOrDefault();
                if (Id == 0)
                {
                    _MasterProvider.SaveBlock(Model);//.SaveDistric(Model);
                    return RedirectToAction("ShowAllBlock");
                }
                else
                {
                    int? status = _context.Blocks.Where(x => x.BlockName == Model.BlockName).Select(x => x.StatusId).FirstOrDefault();
                    if (status == 1)
                    {
                        TempData["Error1"] = "Duplicate Entry Found";

                    }
                    //TempData["Error1"] = "Duplicate Entry Found";
                    else
                    {

                        _MasterProvider.UpdateBlock_status(Id);
                        return RedirectToAction("ShowAllBlock");

                    }
                    Model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
                    Model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel
                    {
                        Id = 0,
                        DistrictName = "--Select District Name--"
                    });

                    return View(Model);

                }

            }
            return View(Model);


        }

        //For Edit Block
        public ActionResult EditBlock(Int64 id = 0)
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Edit Block", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                BlocksViewModel Model = new BlocksViewModel();
                Model = _MasterProvider.GetBlock(id);
                Model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
                Model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel { Id = 0, DistrictName = "--Select District Name--" });
                return View(Model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        //For save Edited Block
        [HttpPost]
        public ActionResult EditBlock(BlocksViewModel model)
        {
            if (model.DistricId == 0)
            {
                TempData["Error1"] = "Please Select Distric";
                model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
                model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel
                {
                    Id = 0,
                    DistrictName = "--Select District Name--"
                });

                return View(model);

            }
            if (ModelState.IsValid)
            {
                // _MasterProvider.UpdateDistric(model);
                _MasterProvider.UpdateBlock(model);

                return RedirectToAction("ShowAllBlock");
            }
            return View(model);

        }
        //for delete block
        [HttpPost]
        public JsonResult DeleteBlock(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);
                        _MasterProvider.DeleteBlock(Id, (int)_session.CurrentUser.Id);//.DeleteDistric(Id, (int)_session.CurrentUser.Id);


                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    // _MasterProvider.DeleteDistric(Id, (int)_session.CurrentUser.Id);
                    _MasterProvider.DeleteBlock(Id, (int)_session.CurrentUser.Id);



                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
            }
        }


        #endregion

        #region: Switch User
        public ActionResult SwitchUser()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Switch User", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                WidgetViewModels model = new WidgetViewModels();
                model.RoleId = (int)_session.CurrentUser.Role;
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }


        #endregion
                
        #region : Country

        public ActionResult AddNewCountry()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Add New Country", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult AddNewCountry(CountryViewModel model)
        {
            WebServiceController service = new WebServiceController();
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Add New Country",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            Int64 i = _context.Countries.Where(x => x.CountryName == model.countryName).Select(x => x.Id).FirstOrDefault();
            if (i == 0)
            {
                model.CreatedById = _session.CurrentUser.Id;
                //var mpackage = service.AddNewCountry(model.countryName, model.currencyName, model.currencySymbol, _session.CurrentUser.Id);
                var mpackage = _MasterProvider.saveCountry(model);
                //model.currencyName = mpackage.Object.currencyName;
                //model.countryName = mpackage.Object.countryName;
                //model.currencySymbol = mpackage.Object.currencySymbol;
                //model.StatusId = mpackage.Object.StatusId;
                //model.ModificationDate = DateTime.UtcNow;
                //model.CreatedById = _session.CurrentUser.Id;

                return RedirectToAction("ShowAllCountries");
            }
            else
            {
                int? status = _context.Countries.Where(x => x.CountryName == model.countryName).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1)
                {
                    TempData["Error1"] = "Duplicate Data found!";
                    return View(model);
                }
                else
                {
                    //var mpackage = service.AddNewCountry(model.currencyName, model.currencySymbol, model.countryName, _session.CurrentUser.Id);
                    var mpackage = _MasterProvider.saveCountry(model);
                    //model.currencyName = mpackage.Object.currencyName;
                    //model.countryName = mpackage.Object.countryName;
                    //model.currencySymbol = mpackage.Object.currencySymbol;
                    //model.StatusId = mpackage.Object.StatusId;
                    //model.ModificationDate = DateTime.UtcNow;
                    //model.CreatedById = _session.CurrentUser.Id;

                    return RedirectToAction("ShowAllCountries");
                }
            }
        }

        public ActionResult ShowAllCountries()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All Countries", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        public JsonResult getAllCountry([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<CountryViewModel> list = new List<CountryViewModel>();
            int filterCount = 0;
            var totalCount = _context.Countries.Where(x => x.StatusId == 1).Count();


            WebServiceController service = new WebServiceController();
            var mpackage = _MasterProvider.getAllCountries(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);


            list = mpackage;
            filterCount = list.Count;
            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCountry(Int64 id)
        {
            // WebServiceController service = new WebServiceController();
            _MasterProvider.deleteCountry(id);

            return RedirectToAction("ShowAllCountries");

        }

        public ActionResult EditCountry(Int64 id)
        {
            WebServiceController service = new WebServiceController();
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Edit Country",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            CountryViewModel model = new CountryViewModel();

            model = _MasterProvider.getCountry(id);
            //model.currencySymbol = mpackage.currencySymbol;
            //model.currencyName = mpackage.currencyName;
            //model.countryName = mpackage.countryName;
            //model.Id = mpackage.Id;
            //model.StatusId = mpackage.StatusId;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditCountry(CountryViewModel model)
        {
            WebServiceController service = new WebServiceController();
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Edit Country",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            if (ModelState.IsValid)
            {
                int? status = _context.Countries.Where(x => x.CountryName == model.countryName).Where(x => x.Id != model.Id).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1)
                {
                    TempData["Error1"] = "Duplicate Data found!";
                    return View(model);
                }
                else
                {
                    model.ModifiedById = _session.CurrentUser.Id;
                    //var mpackage = service.EditCountry(model.Id, model.countryName, model.currencyName, model.currencySymbol, _session.CurrentUser.Id);
                    var chk = _MasterProvider.updateCountry(model);
                    //model.currencySymbol = mpackage.Object.currencySymbol;
                    //model.currencyName = mpackage.Object.currencyName;
                    //model.countryName = mpackage.Object.countryName;
                    //model.Id = mpackage.Object.Id;
                    //model.StatusId = mpackage.Object.StatusId;
                    //model.ModifiedById = _session.CurrentUser.Id;
                    //model.ModificationDate = DateTime.UtcNow;

                    return RedirectToAction("ShowAllCountries");
                }

            }
            return View(model);
        }
        
        public ActionResult InsertCountryToDb()
        {
            var CountryString = "Afghanistan,Albania,Algeria,American Samoa,Andorra,Angola,Anguilla,Antarctica,Antigua And Barbuda,Argentina,Armenia,Aruba,Australia,"
                                + "Austria,Azerbaijan,Bahamas,Bahrain,Bangladesh,Barbados,Belarus,Belgium,Belize,Benin,Bermuda,Bhutan,Bolivia,Bosnia And Herzegovina,"
                                + "Botswana,Bouvet Island,Brazil,British Indian Ocean Territory,Brunei Darussalam,Bulgaria,Burkina Faso,Burundi,Cambodia,Cameroon,Canada,Cape"
                                + "Verde,Cayman Islands,Central African Republic,Chad,Chile,China,Christmas Island,Cocos (keeling) Islands,Colombia,Comoros,Congo,Congo,The"
                                + "Democratic Republic Of The,Cook Islands,Costa Rica,Cote D'ivoire,Croatia,Cuba,Cyprus,Czech Republic,Denmark,Djibouti,Dominica,Dominican"
                                + "Republic,East Timor,Ecuador,Egypt,El Salvador,Equatorial Guinea,Eritrea,Estonia,Ethiopia,Falkland Islands (malvinas),Faroe Islands,Fiji,"
                                + "Finland,France,French Guiana,French Polynesia,French Southern Territories,Gabon,Gambia,Georgia,Germany,Ghana,Gibraltar,Greece,Greenland,"
                                + "Grenada,Guadeloupe,Guam,Guatemala,Guinea,Guinea-bissau,Guyana,Haiti,Heard Island And Mcdonald Islands,Holy See (vatican City State),"
                                + "Honduras,Hong Kong,Hungary,Iceland,Indonesia,Iran,Islamic Republic Of,Iraq,Ireland,Israel,Italy,Jamaica,Japan,Jordan,Kazakstan,"
                                + "Kenya,Kiribati,Korea,Democratic People's Republic Of,Korea,Republic Of,Kosovo,Kuwait,Kyrgyzstan,Lao People's Democratic Republic,Latvia,"
                                + "Lebanon,Lesotho,Liberia,Libyan Arab Jamahiriya,Liechtenstein,Lithuania,Luxembourg,Macau,Macedonia,The Former Yugoslav Republic Of,"
                                + "Madagascar,Malawi,Malaysia,Maldives,Mali,Malta,Marshall Islands,Martinique,Mauritania,Mauritius,Mayotte,Mexico,Micronesia,Federated"
                                + "States Of,Moldova,Republic Of,Monaco,Mongolia,Montserrat,Montenegro,Morocco,Mozambique,Myanmar,Namibia,Nauru,Nepal,Netherlands,"
                                + "Netherlands Antilles,New Caledonia,New Zealand,Nicaragua,Niger,Nigeria,Niue,Norfolk Island,Northern Mariana Islands,Norway,Oman,Pakistan,"
                                + "Palau,Palestinian Territory,Occupied,Panama,Papua New Guinea,Paraguay,Peru,Philippines,Pitcairn,Poland,Portugal,Puerto Rico,Qatar,"
                                + "Reunion,Romania,Russian Federation,Rwanda,Saint Helena,Saint Kitts And Nevis,Saint Lucia,Saint Pierre And Miquelon,Saint Vincent And The"
                                + "Grenadines,Samoa,San Marino,Sao Tome And Principe,Saudi Arabia,Senegal,Serbia,Seychelles,Sierra Leone,Singapore,Slovakia,Slovenia,Solomon"
                                + "Islands,Somalia,South Africa,South Georgia And The South Sandwich Islands,Spain,Sri Lanka,Sudan,Suriname,Svalbard And Jan Mayen,Swaziland,"
                                + "Sweden,Switzerland,Syrian Arab Republic,Taiwan,Province Of China,Tajikistan,Tanzania,United Republic Of,Thailand,Togo,Tokelau,Tonga,"
                                + "Trinidad And Tobago,Tunisia,Turkey,Turkmenistan,Turks And Caicos Islands,Tuvalu,Uganda,Ukraine,United Arab Emirates,"
                                + "United States Minor Outlying Islands,Uruguay,Uzbekistan,Vanuatu,Venezuela,Viet Nam,Virgin Islands,British,Virgin Islands,"
                                + "Wallis And Futuna,Western Sahara,Yemen,Zambia,Zimbabwe";

            var CountryList = CountryString.Split(',');
            foreach(var data in CountryList)
            {
                Country country = new Country();
                country.CountryName = data;
                country.CurrencyName = "";
                country.CurrencySymbol = "";
                country.CreatedById = _session.CurrentUser.Id;
                country.CreationDate = DateTime.UtcNow;
                _context.Countries.Add(country);
            }
            _context.SaveChanges();

            return RedirectToAction("ShowStates");
        }
        #endregion

        #region: State

        public ActionResult State()
        {
            //Add by 
            try {
                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Add New State",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            StateViewModel model = new StateViewModel();
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
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult State(StateViewModel Model)
        {
            //Add by 
            if (ModelState.IsValid)
            {

                Int64 Id = _context.States.Where(x => x.Statename == Model.Statename).Select(x => x.Id).FirstOrDefault();
                if (Id == 0)
                {
                    _MasterProvider.SaveState(Model);
                    return RedirectToAction("ShowStates");
                }
                else
                {
                    int? status = _context.States.Where(x => x.Statename == Model.Statename).Select(x => x.StatusId).FirstOrDefault();
                    if (status == 1)
                    {
                        TempData["Error1"] = "Duplicate Entry Found";
                        return View(Model);
                    }

                    else
                    {
                        _MasterProvider.Updatestate_status(Id);
                        return RedirectToAction("ShowStates");
                    }

                }

            }
            return View(Model);


        }

        public ActionResult ShowStates()
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                //Add by 
                var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "All State",
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

        public ActionResult ShowAllState()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All State", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public JsonResult GetAllState(int size, int page)
        {
            var States = _MasterProvider.GetAllStates(null, null, null, page, size);
            JsonListModel<StateViewModel> model = new JsonListModel<StateViewModel>
            {
                List = States,
                Message = "records fetched successfully",
                Result = true
            };
            return Json(model);
        }

        [HttpPost]
        public JsonResult GetAllStates([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<StateViewModel> list = new List<StateViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<StateViewModel> Report = new JsonListModel<StateViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                StateViewModel model = new StateViewModel();
                //model.CategoryList.Insert(0, new CategoryViewModel
                //{
                //    Id = 0,
                //    Category = "Demo",
                //    Description = "Demo Category Description"

                //});

                //model.StateList = _masterProvider.GetAllStates(start, length, search, filtercount);
                model.StateList = _MasterProvider.GetAllStates(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                //model.CategoryList[0].Edit = "<button type='button' title='Edit' onclick='EditAdvertise(" + 0 + ");' class='btn btn-block btn-primary'>Edit</button>";
                //model.CategoryList[0].Delete = "<button type='button' title='Delete' onclick='EditAdvertise(" + 0 + ");' class='btn btn-block btn-primary'>Delete</button>";
                list = model.StateList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetState(Int64 CountryId)
        {
            StateViewModel model = new StateViewModel();
            model.StateList = _MasterProvider.GetStates(CountryId);
            return Json(new SelectList(model.StateList, "Id", "Statename", model.Id));

        }

        //[HttpPost]
        //public JsonResult GetAllDistricts(int size, int page)
        //{
        //    var States = _MasterProvider.GetAllDistrict(null, null, null, page, size);
        //    JsonListModel<DistrictsViewModel> model = new JsonListModel<DistrictsViewModel>
        //    {
        //        List = States,
        //        Message = "records fetched successfully",
        //        Result = true
        //    };
        //    return Json(model);
        //}
        public ActionResult EditState(Int64 id = 0)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Edit State",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            StateViewModel Model = new StateViewModel();

            Model = _MasterProvider.GetState(id);

            Model.CountryList = _MasterProvider.getAllCountries();
            Model.CountryList.Insert(0, new CountryViewModel
            {
                Id = 0,
                countryName = "--Select Country--"
            });



            return View(Model);

        }

        [HttpPost]
        public ActionResult EditState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {

                _MasterProvider.UpdateState(model);

                return RedirectToAction("ShowStates");
            }
            return View(model);


        }

        [HttpPost]
        public JsonResult DeleteState(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);
                        _MasterProvider.DeleteState(Id, (int)_session.CurrentUser.Id);


                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    _MasterProvider.DeleteState(Id, (int)_session.CurrentUser.Id);



                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
            }
        }

        public ActionResult DeleteState(Int64 Id = 0)
        {
            _MasterProvider.DeleteState(Id);
            return RedirectToAction("ShowStates");
        }
        #endregion

        #region : District

        public ActionResult District()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Add New District", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                DistrictsViewModel model = new DistrictsViewModel();
                model.StateViewModel.StateList = _MasterProvider.GetAllStates(null, null, 0, 0, 0);
                model.StateViewModel.StateList.Insert(0, new StateViewModel { Id = 0, Statename = "--Select State Name--" });
                return View(model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }
        [HttpPost]
        public ActionResult District(DistrictsViewModel Model)
        {
            if (Model.StateId == 0)
            {
                TempData["Error1"] = "Please Select State";
                Model.StateViewModel.StateList = _MasterProvider.GetAllStates(null, null, 0, 0, 0);
                Model.StateViewModel.StateList.Insert(0, new StateViewModel
                {
                    Id = 0,
                    Statename = "--Select State Name--"
                });
                return View(Model);

            }

            if (ModelState.IsValid)
            {

                Int64 Id = _context.Districts.Where(x => x.DistricName == Model.DistrictName).Select(x => x.Id).FirstOrDefault();
                if (Id == 0)
                {
                    _MasterProvider.SaveDistric(Model);
                    return RedirectToAction("ShowAllDistrict");
                }
                else
                {
                    int? status = _context.Districts.Where(x => x.DistricName == Model.DistrictName).Select(x => x.StatusId).FirstOrDefault();
                    if (status == 1)
                    {
                        TempData["Error1"] = "Duplicate Entry Found";

                    }
                    //TempData["Error1"] = "Duplicate Entry Found";
                    else
                    {
                        _MasterProvider.UpdateDistrict_status(Id);
                        return RedirectToAction("ShowAllDistrict");

                    }
                    Model.StateViewModel.StateList = _MasterProvider.GetAllStates(null, null, 0, 0, 0);
                    Model.StateViewModel.StateList.Insert(0, new StateViewModel
                    {
                        Id = 0,
                        Statename = "--Select State name--"
                    });
                    return View(Model);

                }

            }
            return View(Model);


        }
        [HttpPost]
        public JsonResult GetAllDistrict(int size, int page)
        {
            var AppUsers = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
            JsonListModel<DistrictsViewModel> model = new JsonListModel<DistrictsViewModel>
            {
                List = AppUsers,
                Message = "records fetched successfully",
                Result = true
            };

            return Json(model);
        }
        public ActionResult ShowAllDistrict()
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "All District",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                return View();
            }catch(Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult EditDistrict(Int64 id = 0)
        {
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Edit District",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;


            DistrictsViewModel Model = new DistrictsViewModel();
            Model = _MasterProvider.GetDistric(id);


            Model.StateViewModel.StateList = _MasterProvider.GetAllStates();
            Model.StateViewModel.StateList.Insert(0, new StateViewModel
            {
                Id = 0,
                Statename = "--Select State name--"
            });

            return View(Model);

        }
        [HttpPost]
        public ActionResult EditDistrict(DistrictsViewModel model)
        {
            if (model.StateId == 0)
            {
                TempData["Error1"] = "Please Select District";
                model.StateViewModel.StateList = _MasterProvider.GetAllStates();
                model.StateViewModel.StateList.Insert(0, new StateViewModel
                {
                    Id = 0,
                    Statename = "--Select State name--"
                });
                return View(model);

            }
            if (ModelState.IsValid)
            {
                _MasterProvider.UpdateDistric(model);

                return RedirectToAction("ShowAllDistrict");
            }
            return View(model);

        }
        [HttpPost]
        public JsonResult DeleteDistrict(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);
                        _MasterProvider.DeleteDistric(Id, (int)_session.CurrentUser.Id);


                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    _MasterProvider.DeleteDistric(Id, (int)_session.CurrentUser.Id);



                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
            }
        }
        public JsonResult GetDistricts(Int64 SelectedStateId)
        {

            DistrictsViewModel model = new DistrictsViewModel();
            model.DistrictList = _MasterProvider.GetDistricts(SelectedStateId);
            return Json(new SelectList(model.DistrictList, "Id", "DistrictName", model.Id));

        }

        #endregion

        #region: City
        //For City Master
        public ActionResult City()
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Add New City",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;

                CityViewModel model = new CityViewModel();

                model.CountryList = _MasterProvider.getAllCountries();
                model.CountryList.Insert(0, new CountryViewModel
                {
                    Id = 0,
                    countryName = "--Select Country--"
                });

                model.StateList = _MasterProvider.GetAllStates(null, null, 0, 0, 0);
                model.StateList.Insert(0, new StateViewModel
                {
                    Id = 0,
                    Statename = "--Select State Name--"
                });



                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
                
        }

        //For City Master Save
        [HttpPost]
        public ActionResult City(CityViewModel Model)
        {
            //if (Model.DistricId == 0)
            //{
            //    TempData["Error1"] = "Please Select District";
            //    Model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
            //    Model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel
            //    {
            //        Id = 0,
            //        DistrictName = "--Select Distric Name--"
            //    });
            //    return View(Model);

            //}
            //if(ModelState.IsValid)
            //{
            //    _MasterProvider.SaveCity(Model);
            //    return RedirectToAction("ShowAllCity");
            //}
            //return View(Model);
            //if (ModelState.IsValid)
            //{

            Int64 Id = _context.Cities.Where(x => x.CityName == Model.Cityname).Select(x => x.Id).FirstOrDefault();
            if (Id == 0)
            {
                _MasterProvider.SaveCity(Model);
                return RedirectToAction("ShowAllCity");
            }
            else
            {
                int? status = _context.Cities.Where(x => x.CityName == Model.Cityname).Select(x => x.StatusId).FirstOrDefault();
                if (status == 1)
                {
                    TempData["Error1"] = "Duplicate Entry Found";

                }
                //TempData["Error1"] = "Duplicate Entry Found";
                else
                {
                    _MasterProvider.UpdateCity_status(Id);
                    return RedirectToAction("ShowAllCity");

                }
                Model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
                Model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel
                {
                    Id = 0,
                    DistrictName = "--Select Distric Name--"
                });
                return View(Model);

            }



        }
        //Method For Get All City
        [HttpPost]
        public JsonResult GetAllCity(int size, int page)
        {
            var AppUsers = _MasterProvider.GetAllCity(null, 1, size);
            JsonListModel<CityViewModel> model = new JsonListModel<CityViewModel>
            {
                List = AppUsers,
                Message = "records fetched successfully",
                Result = true
            };

            return Json(model);
        }
        //For Show All City
        public ActionResult ShowAllCity()
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "All Cities",
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

        // For Show All City Raveen
        public ActionResult ShowAllCities()
        {
            try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "All Cities",
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
        public JsonResult GetAllCities([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string rid)
        {
            List<CityViewModel> list = new List<CityViewModel>();
            var totalCount = 1; // _context.Brands.Where(x => x.StatusId == 1).Count();
            int filterCount = 0;

            JsonListModel<CityViewModel> Report = new JsonListModel<CityViewModel>
            {
                Message = "Failed",
                Result = false
            };
            try
            {
                CityViewModel model = new CityViewModel();
                //model.CategoryList.Insert(0, new CategoryViewModel
                //{
                //    Id = 0,
                //    Category = "Demo",
                //    Description = "Demo Category Description"

                //});

                //model.StateList = _masterProvider.GetAllStates(start, length, search, filtercount);
                model.CityList = _MasterProvider.GetAllCities(requestModel.Start, requestModel.Length, requestModel.Search.Value, filterCount);
                //model.CategoryList[0].Edit = "<button type='button' title='Edit' onclick='EditAdvertise(" + 0 + ");' class='btn btn-block btn-primary'>Edit</button>";
                //model.CategoryList[0].Delete = "<button type='button' title='Delete' onclick='EditAdvertise(" + 0 + ");' class='btn btn-block btn-primary'>Delete</button>";
                list = model.CityList;
                filterCount = list.Count;

            }
            catch (Exception ex)
            {

            }


            return Json(new DataTablesResponse(requestModel.Draw, list, filterCount, totalCount), JsonRequestBehavior.AllowGet);
        }

        // For Edit City
        public ActionResult EditCity(Int64 id)
        {
            try {
                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Edit City",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;


            CityViewModel Model = new CityViewModel();
            Model = _MasterProvider.GetCity(id);

            Model.CountryList = _MasterProvider.getAllCountries();
            Model.CountryList.Insert(0, new CountryViewModel
            {
                Id = 0,
                countryName = "--Select Country--"
            });


            Model.StateList = _MasterProvider.GetAllStates(null, null, 0, 0, 0);
            Model.StateList.Insert(0, new StateViewModel
            {
                Id = 0,
                Statename = "--Select State Name--"
            });

            return View(Model);
                 }
            catch(Exception e)
            {
                return RedirectToAction("Login", "Account");
            }


            //var breadCrumbModel = new BreadCrumbModel()
            //{
            //    Url = "/Home/",
            //    Title = "Edit City",
            //    SubBreadCrumbModel = null
            //};
            //ViewBag.BreadCrumb = breadCrumbModel;


            //CityViewModel Model = new CityViewModel();
            //Model = _MasterProvider.GetCity(id);


            //Model.DistrictsViewModel.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
            //Model.DistrictsViewModel.DistrictList.Insert(0, new DistrictsViewModel
            //{
            //    Id = 0,
            //    DistrictName = "--Select District Name--"
            //});

            //return View(Model);

        }
        [HttpPost]
        public ActionResult EditCity(CityViewModel model)
        {

            if (ModelState.IsValid)
            {
                _MasterProvider.UpdateCity(model);

                return RedirectToAction("ShowAllCity");
            }
            return View(model);

        }

        [HttpPost]
        public JsonResult DeleteCity(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);
                        _MasterProvider.DeleteCity(Id, (int)_session.CurrentUser.Id);


                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    _MasterProvider.DeleteCity(Id, (int)_session.CurrentUser.Id);



                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
            }
        }

        public JsonResult GetCities(Int64 StateId)
        {
            CityViewModel model = new CityViewModel();
            model.CityList = _MasterProvider.GetCities(StateId);
            return Json(new SelectList(model.CityList, "Id", "CityName", model.Id));

        }

        public ActionResult DeleteCity(Int64 Id = 0)
        {
            _MasterProvider.DeleteCity(Id);
            return RedirectToAction("ShowAllCity");
        }

        #endregion

        #region: User
        public ActionResult ShowAllUser()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "All User", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                return View();
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }
        [HttpPost]
        public JsonResult GetAllUser(int page, int size)
        {
            var AppUsers = _MasterProvider.GetAllAppUsers();
            JsonListModel<MasterUserViewModel> model = new JsonListModel<MasterUserViewModel>
            {
                List = AppUsers,
                Message = "records fetched successfully",
                Result = true
            };
            return Json(model);

        }
        [HttpPost]
        public JsonResult GetAllUserCheckingApp(int page, int size)
        {
            var AppUsers = _MasterProvider.GetAllAppUsers();
            JsonListModel<MasterUserViewModel> model = new JsonListModel<MasterUserViewModel>
            {
                List = AppUsers,
                Message = "records fetched successfully",
                Result = true
            };
            return Json(model);
        }
        [HttpPost]
        public JsonResult DeleteUser(string id)
        {

            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {

                        Id = Convert.ToInt32(arr[i]);
                        _MasterProvider.DeleteMasterUser(Id);//.DeleteUser(Id);



                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    _MasterProvider.DeleteMasterUser(Id);//DeleteUser(Id);


                }
                //LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                //LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);

            }
        }
        public JsonResult ApproveUser(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);
                        // _MasterProvider.ApproveUser(Id, (int)_session.CurrentUser.Id);


                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);
                    //  _MasterProvider.ApproveUser(Id, (int)_session.CurrentUser.Id);



                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Approve Seller record success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                LogMe.Log("SettingsController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
            }
        }
        [HttpPost]
        public JsonResult DisApproveUser(string id)
        {
            try
            {
                int Id = 0;
                if (id.Contains(','))
                {
                    string[] arr = id.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Id = Convert.ToInt32(arr[i]);


                        //  _MasterProvider.DisApproveUser(Id, (int)_session.CurrentUser.Id);
                    }
                }
                else
                {
                    Id = Convert.ToInt32(id);

                    //  _MasterProvider.DisApproveUser(Id, (int)_session.CurrentUser.Id);

                }
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Info, "Disapprove seller data success.");
                return Json(id);
            }
            catch (Exception ex)
            {
                LogMe.Log("SellerController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(id);
            }
        }

        public ActionResult UserRegistration()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = " Add User", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                MasterUserViewModel User = new MasterUserViewModel();
                User.RoleList = _commonProvider.GetAllRole(null);
                User.RoleList.Insert(0, new RoleViewModel { Id = 0, Name = "--Select Role--" });
                if (_session.CurrentUser.RoleId == 1)
                {
                    User.RoleList.Clear();
                    User.RoleList.Insert(0, new RoleViewModel { Id = 0, Name = "--Select Role--" });
                    User.RoleList.Insert(1, new RoleViewModel { Id = 2, Name = "Admin" });
                }
                if (_session.CurrentUser.RoleId == 2)
                {
                    User.RoleList.RemoveAt(1);
                    User.RoleList.RemoveAt(1);
                }
                if (_session.CurrentUser.RoleId == 3)
                {
                    User.RoleList.RemoveAt(1);
                    User.RoleList.RemoveAt(1);
                    User.RoleList.RemoveAt(1);
                }
                if (_session.CurrentUser.RoleId == 4)
                {
                    User.RoleList.RemoveAt(1);
                    User.RoleList.RemoveAt(1);
                    User.RoleList.RemoveAt(1);
                    User.RoleList.RemoveAt(1);
                }
                User.StateList = _MasterProvider.GetAllStates(true, true, 0, 0, 0);
                User.StateList.Insert(0, new StateViewModel { Id = 0, Statename = "--Select State--" });
                User.DistrictList = _MasterProvider.GetAllDistrict(true, true, 0, 0, 0);
                User.DistrictList.Insert(0, new DistrictsViewModel { Id = 0, DistrictName = "--Select District--" });
                User.CityList = _MasterProvider.GetAllCity(null, 1, 0);
                User.UserViewModel.UserList = _MasterProvider.GetUserList();
                User.UserViewModel.UserList.Insert(0, new UserViewModel { Id = 0, UserName = "--Select User Name--" });
                return View(User);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }
        [HttpPost]
        public ActionResult UserRegistration(MasterUserViewModel User)
        {

            //Add by 
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Add User",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;
            User.Keyword = User.Password;
            _MasterProvider.UserRegistration(User);

            return RedirectToAction("ShowAllUser", "Master");
        }
        public ActionResult EditUser(Int64 Id = 0)
        {
            //Add by try{
            try { 
            if (Id == 0)
            {

              
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Edit Profile",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
            }
            else
            {

                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Home/",
                    Title = "Edit User",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
            }

            MasterUserViewModel user = new MasterUserViewModel();

            user = _MasterProvider.GetMasterUserForEdit(Id);
            user.SuperAdminRoleId = _session.CurrentUser.RoleId;
            user.CurrentUser = _session.CurrentUser.Id;

            user.CityList = _MasterProvider.GetAllCity(null, 1, 0);

            user.CityList.Insert(0, new CityViewModel
            {
                Id = 0,
                Cityname = "--Select City--"
            });
            user.StateList = _MasterProvider.GetAllStates(null, null, 0, 0, 0);
            user.StateList.Insert(0, new StateViewModel
            {
                Id = 0,
                Statename = "--Select State Name--"

            });

            user.DistrictList = _MasterProvider.GetAllDistrict(null, null, 0, 0, 0);
            user.DistrictList.Insert(0, new DistrictsViewModel
            {
                Id = 0,
                DistrictName = "--Select District Name--"
            });

            user.RoleList = _commonProvider.GetAllRole(0, 0, 0);
            user.RoleList.Insert(0, new RoleViewModel
            {
                Id = 0,
                Name = "--Select Role --"

            });
            
            user.UserViewModel.UserList = _MasterProvider.GetUserList();
            user.UserViewModel.UserList.Insert(0, new UserViewModel
            {
                Id = 0,
                UserName = "--Select User Name--"

            });

            return View(user);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpPost]
        public ActionResult EditUser(MasterUserViewModel User)
        {
            // string state = _context.States.Where(x => x.Id == User.ZoneId).Select(x => x.Statename).FirstOrDefault();
            //Add by 
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Edit User",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            if (User.CityId == 0)
            {
                User.M_CityViewModels.CityList.Insert(0, new CityViewModel
                {
                    Id = 0,
                    Cityname = "--Select City--"
                });
                return View(User);
            }

            _MasterProvider.EditUpdateUser(User);

            return RedirectToAction("ShowAllUser", "Master");
        }


        #endregion

        #region : Change refill rate

        public ActionResult ChangeRefillRate(Int64 id = 0)
        {
            try { 
            Int64 pid = 0;
            pid = _MasterProvider.GetPackageId(_session.CurrentUser.Id);

            RefillRateViewModel model = new RefillRateViewModel();
            model = _MasterProvider.GetRefillRate(pid);
            if (model.PackageId == 0)
            {
                model.DateFrom = System.DateTime.UtcNow;
            }
            else
            {
                //model.datefrm = System.DateTime.UtcNow;
                model.PackageId = pid;
                //model.NewRefillRate = _MasterProvider.GetNewRate();
                //model.OldRefillRate = _MasterProvider.GetOldRate();
            }


            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Master/",
                Title = "Change Refill Rate",
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
        public ActionResult ChangeRefillRate(RefillRateViewModel model)
        {
            RefillRateViewModel m = new RefillRateViewModel();
            if (model.PackageId > 0)
            {

                model.PackageId = _MasterProvider.GetPackageId(_session.CurrentUser.Id);
                bool msg = _MasterProvider.UpdateRefillRate(model);
                if (msg == true)
                {

                    m.Msg = "Refill Rate has been updated";
                }
                else
                    m.Msg = "";

            }
            else
            {

                //model.CreatedById = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel()
                {
                    Url = "/Master/",
                    Title = "Refill Rate",
                    SubBreadCrumbModel = null
                };
                ViewBag.BreadCrumb = breadCrumbModel;
                _MasterProvider.SaveRefilleRate(model);
                m.Msg = "Refill Rate has been Saved";


            }
            return View(m);


        }

        #endregion

        public ActionResult UpdateCountries()
        {try
            {
                var Current = _session.CurrentUser.Id;
                var breadCrumbModel = new BreadCrumbModel() { Url = "/Home/", Title = "Upload Countries", SubBreadCrumbModel = null };
                ViewBag.BreadCrumb = breadCrumbModel;
                CountryViewModel Model = new CountryViewModel();
                Model.CountryList = _MasterProvider.getAllCountries();
                Model.CountryList.Insert(0, new CountryViewModel() { countryName = "--Select Country--", Id = 0 });
                return View(Model);
            }
            catch (Exception e) { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public ActionResult UpdateCountries(CountryViewModel Model)
        {

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Upload Countries",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            CountryViewModel NewModel = new CountryViewModel();
            
           
            var CountryList = Model.countryName.Split(',');

            foreach (var countryname in CountryList)
            {
                NewModel.countryName = countryname;
                NewModel.currencyName = "";
                NewModel.currencySymbol = "";
                _MasterProvider.saveCountry(NewModel);
            }


            return View(Model);

        }

        public ActionResult UpdateStates()
        {
            try
            {

                var Current = _session.CurrentUser.Id;
            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Upload States",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            CountryViewModel Model = new CountryViewModel();
            Model.CountryList = _MasterProvider.getAllCountries();
            Model.CountryList.Insert(0, new CountryViewModel()
            {
                countryName = "--Select Country--",
                Id = 0
            });
            return View(Model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        public ActionResult UpdateStates(CountryViewModel Model)
        {

            var breadCrumbModel = new BreadCrumbModel()
            {
                Url = "/Home/",
                Title = "Upload States",
                SubBreadCrumbModel = null
            };
            ViewBag.BreadCrumb = breadCrumbModel;

            Model.CountryList = _MasterProvider.getAllCountries();
            Model.CountryList.Insert(0, new CountryViewModel()
            {
                countryName = "--Select Country--",
                Id = 0
            });

            //CountryViewModel Model = new CountryViewModel();
            StateViewModel State = new StateViewModel();
            State.CountryId = Model.Id;

            var stateList = Model.countryName.Split(',');
            
            foreach(var statename in stateList)
            {
                State.Statename = statename;
                _MasterProvider.SaveState(State);
            }

            Model.Id = 0;
            Model.countryName = "";
            return View(Model);

        }

        public ActionResult emplo()
        {
            return View();
        }

    }
}