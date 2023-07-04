using Loregroup.Core;
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Exceptions;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Logmodels;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
//using Loregroup.Provider;
using Core.Crypto;
using DotNetOpenAuth.AspNet;
using MailKit.Net.Smtp;
using Microsoft.Web.WebPages.OAuth;
//using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
//using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

//using System.Web.Security;

namespace Loregroup.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProvider _userProvider;
        private readonly ICacheService _dataCache;
        private readonly ISession _session;
        private readonly AppContext _context;


        private readonly IUtilities _utilities;
        private readonly IMasterProvider _Masterprovider;
        private string UserId;

        public AccountController(IMasterProvider masterprovider, AppContext context, IUserProvider userProvider, ICacheService dataCache, ISession session, IUtilities utilities)
        {
            _userProvider = userProvider;
            _dataCache = dataCache;
            _session = session;
            _context = context;
            _utilities = utilities;
            _Masterprovider = masterprovider;
        }

        public ActionResult Misscallb(string MobileNo)
        {
            ViewBag.Number = MobileNo;
            ViewBag.Code = GetUniqueKey();
            return View();
        }

        private string GetUniqueKey()
        {
            int maxSize = 4;
            int minSize = 10;
            char[] chars = new char[62];
            string a;
            a = "ABCDEFGHJKLMNPQRSTUVWXYZ";
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

        public ActionResult Login()
         {
            if (CommonClass.LoginId == true)
            {
                TempData["Error1"] = "Register successful";
            }

            CommonClass.LoginId = false;
            try
            {
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Info, "login open success.");
                return View(new LoginViewModel());
            }
            catch (Exception ex)
            {
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Error, ex.Message);
                return View(new LoginViewModel());
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string provider, string returnUrl)
            
        {
            
            if (provider != null)
            {
                return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            }
            CommonClass.LoginId = false;

            try
            {
                if (ModelState.IsValid)
                {
                    // Start For Check Roles
                    var userId = _userProvider.CheckLoginCredentials(model.UserName, model.Password);
                    Int64 i = userId;

                    Int64 Role = _context.MasterUsers.Where(x => x.Id == i).Select(x => x.RoleId).FirstOrDefault();

                    Int64 roleId = Role;//_userProvider.GetUserRole(userId);
                    if (roleId == 1)
                    {
                        FormsAuthentication.SetAuthCookie(userId.ToString(), model.IsRemember);
                        TempData["Welcome"] = "Welcome ! SuperAdmin";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roleId == 2 || roleId == 7)
                    {
                        FormsAuthentication.SetAuthCookie(userId.ToString(), model.IsRemember);
                        _Masterprovider.SaveUserLog("Account", "Backend Login ", userId);
                        TempData["Welcome"] = "Welcome ! Admin";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roleId == 5)
                    {
                        FormsAuthentication.SetAuthCookie(userId.ToString(), model.IsRemember);
                        _Masterprovider.SaveUserLog("Account", "Backend Login ", userId);
                        TempData["Welcome"] = "Welcome ! Agent";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Error1"] = "You didn't have access to use.";
                        //return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (UserLockedFromAtteptsException ex)
            {
                ModelState.AddModelError(string.Empty, "You are locked due to 10 failed login attemts please contact administrator.");
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Error, ex.Message);
                TempData["Error1"] = ex.Message.ToString();
            }
            catch (InvalidPasswordException ex)
            {
                ModelState.AddModelError(string.Empty, "Username and password doesnot match.");
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Error, ex.Message);
                TempData["Error1"] = ex.Message.ToString();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Some error occured.");
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Error, ex.Message);
                TempData["Error1"] = ex.Message.ToString();
            }
            LogMe.Log("AccountController", LogMeCommonMng.LogType.Info, "login successfully.");
            return View(model);
        }

        public ActionResult SignOut()
        {
            //CommonClass.roleid = 0;
            try
            {
                _Masterprovider.SaveUserLog("Account", "Backend Logout ", _session.CurrentUser.Id);
                _dataCache.DeleteCache(_session.CurrentUser.Id.ToString());
                FormsAuthentication.SignOut();
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Info, "logout success.");
                return RedirectToAction("Index", "Home");
                //return RedirectToAction("LoginPage", "Loregroup");
            }
            catch (Exception ex)
            {
                LogMe.Log("AccountController", LogMeCommonMng.LogType.Error, ex.Message);
                return RedirectToAction("Index", "Home");
                //return RedirectToAction("LoginPage", "Loregroup");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }

       


        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }
        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {

                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

       
        public ActionResult GenerateOTPNEW(Int64 uid)
        {
            string[] otps = new string[NB_OTP];


            Random rndm = new Random();

            if (uid > 255)
            {
                uid = rndm.Next(0, 255);
            }


            byte abc = Convert.ToByte(uid);

            byte[] secretKey = new byte[SECRET_LENGTH]
            {
                abc
            };

            OTP otp = new OTP(secretKey: secretKey);

            otps[0] = otp.GetCurrentOTP();

            for (int n = 1; n < NB_OTP; n++)
            {
                otps[n] = otp.GetNextOTP();
            }
            CommonClass.OTParr = otps;
            Session["VerifyOTPs"] = otps[1];
            return View();
        }


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
        #region ForgotPasswordLink

        [HttpPost]
        public JsonResult ForgotPassword(string ForgotPasswordEmail)
        {
            EmailViewModel Model = new EmailViewModel();
            //UserViewModel Model = new UserViewModel();
            // check if username exists
            var result = "";//_userProvider.GetUserByEmailorUserName(ForgotPasswordEmail, "username");
            Model.To = "";//result.Email;

            if (result == null)
            {
                // username not exists, now check if email exists
                result = "";//_userProvider.GetUserByEmailorUserName(ForgotPasswordEmail, "useremail");
                if (result == null)
                {
                    return Json(new { Success = true, EmailSent = false, ResponseText = "User does not exist" }, JsonRequestBehavior.AllowGet);
                }
            }

            //create url with above token
            var usertoken = Guid.NewGuid();
            String Newpassword = GenerateRandomPassword();
            ForgetPasswordViewModel FModel = new ForgetPasswordViewModel();
            FModel.Id = 0; //result.Id;
            FModel.NewPassword = Newpassword;
            bool r = _userProvider.ChangePassword(FModel);

            //var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { token = usertoken, UserID = result.Id }, "http") + "'>Reset Password</a>"; //, rt = token
            //string msgBody = "<b>Please find the Password Reset Token</b><br/>" + resetLink; //edit it

            string msgBody = "<b>Success! Your password changed.. New password is - </b><br/>" + Newpassword; //edit it
            bool isemailSent = false;
            if (r == true)
            {
                isemailSent = true;// SendForgotPasswordEMail(result.Id, result.Email, usertoken.ToString(), "Password reset Link", msgBody);
            }

            //SendEmail(Model);
            //bool isemailSent = true;
            //return Json(new { Success = true, EmailSent = isemailSent, ResponseText = "Mail service not working fine. Please try again later !!" }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, EmailSent = isemailSent, ResponseText = "Mail service not working fine. Please try again later !!" }, JsonRequestBehavior.AllowGet);
        }

        private bool SendForgotPasswordEMail(Int64 userid, string emailid, string usertoken, string subject, string body)
        {
            try
            {
                //MessageSettingsViewModel model = new MessageSettingsViewModel(); //_AppUsersettingsProvider.GetAppUsersettingsByUserid(_session.CurrentUser.Id);
                //model.Port = 587;
                //model.Host = "smtp.gmail.com";
                //// SMTP_SERVER=184.107.217.244:25 
                //model.UserNameEmail = "lskallcenter@gmail.com";
                //model.PasswordMail = "monit@2014";
                //model.RequireSSL = true;


                //MailMessage mail = new MailMessage();
                //mail.To.Add(emailid);
                //mail.Subject = subject;
                //string Body = body;
                //mail.Body = Body;

                //MailAddress fromAddress = new MailAddress(model.UserNameEmail);
                //mail.From = fromAddress;
                //mail.IsBodyHtml = true;
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                //smtp.Host = model.Host;
                //smtp.Port = (int)model.Port;
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new System.Net.NetworkCredential(model.UserNameEmail, model.PasswordMail);
                //smtp.EnableSsl = model.RequireSSL;

                //smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ActionResult ResetPassword(string token, string UserID)
        {
            if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(UserID))
            {
            }
            return View("Index", "Home");
        }

        [HttpPost]
        public ActionResult ResetPassword(string token, int UserID)
        {
            return View("Index", "Home");
        }

        private String GenerateRandomPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            String finalString = new String(stringChars);
            return finalString;
        }



        #endregion

        const int NB_OTP = 10;
        const int SECRET_LENGTH = 1;


        public ActionResult Newindex()
        {
            return View();
        }
        public ActionResult Indexnew()
        {
            return View();
        }
        public ActionResult LoginCheckingApp()
        {
            return View();
        }
        public ActionResult LoginTest()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult LoginTest(StoreMasterViewModel model)
        //{
        //    return View();
        //}

        public ActionResult Design()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Design(StoreMasterViewModel model)
        //{
        //    return View();
        //}
        public ActionResult MainView()
        {
            return View();
        }


        public ActionResult BulkUploadAppUsers()
        {
            try
            {
                JsonObjectModel<String> model = new JsonObjectModel<String>()
                {
                    Object = _utilities.RenderPartialViewToString(this, "_BulkUploadAppUsers", null),
                    Message = "record fetched successfully",
                    Result = true
                };
                LogMe.Log("UserManagementController", LogMeCommonMng.LogType.Info, "Bulk upload Popup show success.");
                return Json(model);
            }
            catch (Exception ex)
            {
                JsonObjectModel<String> model = new JsonObjectModel<String>();
                LogMe.Log("UserManagementController", LogMeCommonMng.LogType.Error, ex.Message);
                return Json(model);
            }
            //return PartialView("_BulkUploadData");
        }
        [HttpGet]
        public ActionResult EditNodePartialView()
        {


            //PWASiteModel model = SitePartialView(id);
            return PartialView("Partial1");

        }
        [HttpGet]

        public ActionResult Registrationmain()
        {

            return PartialView("Partial_Register");
        }


        public bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            return true;

        }



        //
        // POST: /Account/ExternalLoginConfirmation

    }
}