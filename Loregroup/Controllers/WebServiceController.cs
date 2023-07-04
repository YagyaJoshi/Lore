
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
//using System.Web.Mvc;

namespace Loregroup.Controllers
{
    public class WebServiceController : ApiController
    {
        //private readonly IDeliverySlipProvider _deliveryslipProvider;
        //private readonly IMasterProvider _masterProvider;
        //private readonly ISession _session;

        //public WebServiceController(IDeliverySlipProvider deliveryslipProvider, IMasterProvider masterProvider, ISession session)
        //{
        //    _deliveryslipProvider = deliveryslipProvider;
        //    _masterProvider = masterProvider;
        //    _session = session;
        //}

        IDeliverySlipProvider _deliveryslipProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IDeliverySlipProvider>();
        IMasterProvider _masterProvider = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IMasterProvider>();
        ISession _session = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISession>();

        [HttpGet]
        public string GetHtmlDesign()
        {
            return "<html><body><div id = 'printableArea' style='background-color: skyblue; height: 150px;width: 200px;'><h4>CS Bairagi</h4><h4>Connekt</h4><h4>Indore</h4></div></body></html>";
        }

        //To get data to show in reciept according status of consumer
        [HttpGet]
        public JsonObjectModel<CheckStatusViewModel> CheckStatus(string ConsumerNo)
        {


            JsonObjectModel<CheckStatusViewModel> checkModel = new JsonObjectModel<CheckStatusViewModel>
            {
                Message = "Failed",
                Result = false
            };
            CheckStatusViewModel model = new CheckStatusViewModel();
            try
            {

                var c = _deliveryslipProvider.GetConsumerStatusForAPI(ConsumerNo);

                if (c == null)
                {
                    model.MsgAccToStatus = "बुकिंग प्राप्त नहीं हुई है|";

                }
                if (c != null)
                {
                    if (c.BStatus != "Open")
                    {
                        model.MsgAccToStatus = "अपने हॉकर से संपर्क करें|";

                    }
                    else if (c.BStatus == "Open" && (c.InstallationOnBooking == "-" || c.InstallationOnBooking == ""))
                    {
                        model.ConsumerNo = c.ConsumerNo;
                        Int64 p = _masterProvider.GetPackageIdForAPI(c.CreatedById);
                        model.Amount = _deliveryslipProvider.GetRefillAmount(p);
                        model.BookingDate = c.BookingDate;
                        model.BookingNo = c.BookNo;
                        model.declaration = "उपरोक्त रेट में मुझे सही वज़न का सिल पैक सिलिंडर प्राप्त हुआ|";
                        model.Name = c.Name;
                        model.bstatus = "Success";
                        var User = _masterProvider.GetUser(c.CreatedById);
                        model.UserFullName = User.FirstName + User.LastName;
                        model.UserCity = _masterProvider.GetCityName(User.CityId);
                        model.Username = User.UserName + "          ";
                        model.forgap = "";

                    }

                }

                if (c != null)
                {
                    checkModel.Object = model;
                    checkModel.Message = "records fetched successfully";
                    checkModel.Result = true;
                }
                else
                {
                    checkModel.Message = "Records not found";
                    checkModel.Result = false;
                }

            }
            catch (Exception ex)
            {
                checkModel.Message = "Some error occured.";
            }


            return checkModel;
        }
        

        [HttpGet]
        public void Misscalla(string MobileNumber, string DidNumber, string StartTime, string WaitDuration, Int64 ExtraParameter)
        {
            string res = "Success";

            try
            {
                string durl = "http://rajhans.digital/Account/Misscallb?MobileNo=" + MobileNumber;
                string newurl = urlShorter(durl);

                string message = "Rajhans Digital" + Environment.NewLine + "Click below link to see verification code." + //msg.Replace("<br/>", "\n") + Environment.NewLine +
                                newurl + Environment.NewLine;

                //misscalldata.FooterMessage;         //Mera Fayda
                
                string strUser = "vivekwani2001@hotmail.com";
                string strPassword = "jitendra@1985";
                string strUrl = null;

                strUrl = "http://sms.seven-doors.com/mobicomm/submitsms.jsp?user=RAJHANS&key=76c5cc3bd2XX&mobile=+91" + MobileNumber + "&message=" + message + "&senderid=RAJHNS&accusage=1&unicode=1";
                WebRequest objWebRequest = WebRequest.Create(strUrl);
                WebResponse objWebResponse = objWebRequest.GetResponse();
                Stream objStream = objWebResponse.GetResponseStream();
                StreamReader objStreamReader = new StreamReader(objStream);
                string strHTML = objStreamReader.ReadToEnd();
                //string str = HttpUtility.UrlDecode(strHTML);                                    

            }
            catch (Exception exc)
            {
                Console.WriteLine("server error : {0}", exc);
            }
        }

        private const string key = "AIzaSyAt0hLneSw7xafAe9gJlDuf0ElzPhsBm_0";
        public string urlShorter(string url)
        {
            string finalURL = "";
            string post = "{\"longUrl\": \"" + url + "\"}";
            string shortUrl = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + key);
            try
            {
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");
                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            string json = responseReader.ReadToEnd();
                            finalURL = Regex.Match(json, @"""id"": ?""(?<id>.+)""").Groups["id"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // if Google's URL Shortener is down...
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return finalURL;
        }

        [HttpGet]
        public string TestSendEmail(string Name)       //string Name, string EmailId, string Subject, string Message)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello,</p></td></tr>"
                                 //+ "<tr><td align='left'><p>A New Customer has been registered in your system. Details :</p></td></tr><br>"
                                + "<tr><td align='left'><p>Name : <b> " + Name + "</b></p></td></tr>"                                                                
                                 //+ "<tr><td align='left'><p>Email-Id : <b>" + EmailId + "</b></p></td></tr>"
                    //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                                 //+ "<tr><td align='left'><p>Backend Link : <b> http://dev1.connekt.in </b></p></td></tr>"
                                 + "<tr><td align='left'><p>&nbsp;</p></td></tr>"
                                 + "<tr><td align='right'><p>Thanks And Regards,</p></td></tr>"
                                 + "<tr><td align='right'><p>CS</p></td></tr>";

                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));

                // set the network credentials
                mailClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);

                // enable ssl
                mailClient.EnableSsl = true;

                // Create the Mail Message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["confirmationEmail"], "Loré Fashions");
                mailMessage.To.Add(ConfigurationManager.AppSettings["confirmationEmail"]);
                //mailMessage.To.Add("sales@lore-group.com");
                // mailMessage.CC.Add(usermailid);
                mailMessage.Subject = "Testing email";
                mailMessage.Body = mailFormat;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                //Send the mail
                mailClient.Send(mailMessage);
                return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg==";
            }
            catch (Exception ex)
            {
                return "Fail - " + ex.Message;
            }
        }



        [HttpGet]
        public string SendEmailForCheck(string Name)       //string Name, string EmailId, string Subject, string Message)
        {
            try
            {
                string mailFormat = "<table cellpadding='0' cellspacing='0' border='0' style='width:100%; font-family:verdana;'>"
                                 + "<tr><td align='left' style='width:100%;'><p>Hello,</p></td></tr>"
                    //+ "<tr><td align='left'><p>A New Customer has been registered in your system. Details :</p></td></tr><br>"
                     + "<tr><td align='left'><p>Name : <b>" + Name + "</b></p></td></tr>"
                                + "<tr><td align='left'> <img src=http://lorefashions.com/api/WebService/TestSendEmail?Name=" + Name + " alt='test' style='width:1px;height:1px;display:none;'> </td></tr>"

                    //+ "<tr><td align='left'><p>Email-Id : <b>" + EmailId + "</b></p></td></tr>"
                    //+ "<tr><td align='left'><p>Subject : <b>" + Subject + "</b></p></td></tr>"
                    //+ "<tr><td align='left'><p>Backend Link : <b> http://dev1.connekt.in </b></p></td></tr>"
                                 + "<tr><td align='left'><p>&nbsp;</p></td></tr>"
                                 + "<tr><td align='right'><p>Thanks And Regards,</p></td></tr>"
                                 + "<tr><td align='right'><p>CS</p></td></tr>";

                // Configure mail client (may need additional code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["confirmationHostName"], int.Parse(ConfigurationManager.AppSettings["confirmationPort"]));

                // set the network credentials
                mailClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["confirmationEmail"], ConfigurationManager.AppSettings["confirmationPassword"]);

                // enable ssl
                mailClient.EnableSsl = true;

                // Create the Mail Message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["confirmationEmail"], "Loré Fashions");
                mailMessage.To.Add(ConfigurationManager.AppSettings["confirmationEmail"]);
                mailMessage.To.Add(Name);
                // mailMessage.CC.Add(usermailid);
                mailMessage.Subject = "mail Send 1";
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



    }
}
