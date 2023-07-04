using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Loregroup.Core.Utilities
{
    public class Utilities:IUtilities
    {
        //public DateTime EpochTime {
        //    get {
        //        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    }
        //}
        public DateTime GetEpochTime() {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public String Truncate(String text, Int32 count, Boolean addElipse)
        {
            if (String.IsNullOrEmpty(text) || count <= 0)
            {
                return String.Empty;
            }
            else if (text.Length <= count)
            {
                return text;
            }
            else
            {
                return String.Format("{0}{1}", text.Substring(0, count), (addElipse ? "..." : String.Empty));
            }
        }

        /// <summary>
        /// Takes the object as parameter that needs to serialize and Returns Serialized string
        /// </summary>	
        ///  <param name="serializeObject">Object to srialize</param>
        /// <returns>serialized string</returns>
        public String Serialize(object serializeObject)
        {
            string serializeString = JsonConvert.SerializeObject(serializeObject);
            return serializeString;
        }

        public T Deserialize<T>(String serializedString) where T:class
        {
            return (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<T>(serializedString);
        }

        public Int32 ParseInt(String number)
        {
            if (number == null)
                return 0;

            Int32 temp;

            if (Int32.TryParse(number, out temp))
                return temp;
            else
                return 0;
        }

        public Boolean IsLowerLetter(String character)
        {
            if (character.Length != 1)
                return false;

            char c = character.Substring(1).ToCharArray()[0];

            if (System.Convert.ToInt32(c) >= System.Convert.ToInt32('a') && System.Convert.ToInt32(c) <= System.Convert.ToInt32('z'))
                return true;
            else
                return false;
        }

        public Boolean IsUpperLetter(String character)
        {
            if (character.Length != 1)
                return false;

            char c = character.Substring(1).ToCharArray()[0];

            if (System.Convert.ToInt32(c) >= System.Convert.ToInt32('A') && System.Convert.ToInt32(c) <= System.Convert.ToInt32('Z'))
                return true;
            else
                return false;
        }

        public String CreateAbsoluteUrl(String relativeUrl)
        {
            String absoluteUrl = String.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port != 80 ? ":" + HttpContext.Current.Request.Url.Port : String.Empty, relativeUrl.StartsWith("/") ? relativeUrl : "/" + relativeUrl);

            return absoluteUrl;
        }

        /// <summary>
        /// Encoding a string to Base64
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public String EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        /// <summary>
        /// Decoding a string from Base64
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public String DecodeFrom64(string encodedData)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);

                string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

                return returnValue;
            }
            catch
            {
                return null;
            }
        }

        public void CreateUserFolders(Guid userId)
        {
            //String userProfilePath = HttpContext.Current.Server.MapPath(String.Format(Helpers.Environment.GetSetting(Configuration.Enumerations.AppSettings.UserProfileImagePath), userId.ToString()));

            //if (!System.IO.Directory.Exists(userProfilePath))
            //    System.IO.Directory.CreateDirectory(userProfilePath);
        }

        public String RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public Double GetTimeStamp(DateTime date)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = date.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return ts.TotalMilliseconds;
        }

        public String ToPhoneFormat(String phone)
        {
            phone = phone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "");

            if (phone.Length == 10)
                phone = String.Format("({0}) {1} - {2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6, 4));
            else if (phone.Length == 11)
                phone = String.Format("+{0} ({1}) {2} - {3}", phone.Substring(0, 1), phone.Substring(1, 3), phone.Substring(4, 3), phone.Substring(7, 4));

            return phone;
        }

        public Boolean IsDate(String dateString)
        {
            try
            {
                DateTime date = Convert.ToDateTime(dateString);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public String GetFormatedDateTime(DateTime dateTime)
        {
            return String.Format("{0:00}/{1:00}/{2} - {3:00}:{4:00}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute);
        }

        public DateTime ConvertToDate(String dateString)
        {
            if (String.IsNullOrEmpty(dateString))
                return new DateTime(0);

            return Convert.ToDateTime(dateString);
        }

        public String ConvertToJSonDateString(DateTime date)
        {
            return String.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public String ConvertDateToFavoriteFormat1(DateTime? date)
        {
            if (date == null)
                return String.Empty;
            else
                return String.Format("{0:ddd MMM d yyyy}", date);
        }

        public String ConvertDateToFavoriteFormat2(DateTime? dateTime)
        {
            if (dateTime == null)
                return String.Empty;
            else
                return String.Format("{0:ddd MMM d yyyy - HH:mm}", dateTime);
        }

        public String ConvertDateToFavoriteFormat3(DateTime? date)
        {
            if (date == null)
                return String.Empty;
            else
                return String.Format("{0:M/d/yyyy}", date);
        }

        public String ConvertDateToFavoriteFormat4(DateTime? date)
        {
            if (date == null)
                return String.Empty;
            else
                return String.Format("{0:MMM d yyyy}", date);
        }

        public Byte[] Image2Bytes(System.Drawing.Bitmap image)
        {
            System.Drawing.Imaging.ImageCodecInfo Lo_GifCodec;
            int Lnum_Quality = 70;
            System.Drawing.Imaging.EncoderParameters Lo_Parameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.IO.MemoryStream Lo_Stream = new System.IO.MemoryStream();

            Lo_GifCodec = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().FirstOrDefault();

            foreach (System.Drawing.Imaging.ImageCodecInfo Info in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
            {
                if (Info.MimeType == "image/gif")
                {
                    Lo_GifCodec = Info;
                    break;
                }
            }

            if (Lo_GifCodec.MimeType != "image/gif")
            {
                throw new Exception("Gif Codec missing");
            }

            Lo_Parameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Lnum_Quality);
            image.Save(Lo_Stream, Lo_GifCodec, Lo_Parameters);

            return Lo_Stream.ToArray();
        }

        public void Base64ToFile(string file, string filePath) {
            var bytes = Convert.FromBase64String(file);
            using (var fileObj = new FileStream(filePath, FileMode.Create)) {
                fileObj.Write(bytes, 0, bytes.Length);
                fileObj.Flush();
            }
        }

        public void ResizeStream(long compressionValue, Stream fileStream, string outputPath)
        {
            //var image = Image.FromStream(filePath);

            // Get a bitmap.
            Bitmap bmp1 = new Bitmap(fileStream);
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compressionValue);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save(outputPath, jgpEncoder, myEncoderParameters);
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public Int32 GetAge(DateTime birthDate)
        {
            Int32 age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            return age;
        }

        public void createAlbumDirectory(Int64 peopleId, Int64 albumId)
        {
            String personPath = HttpContext.Current.Server.MapPath("");

            String albumPath = personPath + "\\" + albumId.ToString();

            if (!System.IO.Directory.Exists(personPath))
                System.IO.Directory.CreateDirectory(personPath);

            if (!System.IO.Directory.Exists(albumPath))
                System.IO.Directory.CreateDirectory(albumPath);

        }

        public string UploadFile(HttpPostedFileBase file)
        {
            Guid n = Guid.NewGuid();
            // extract only the fielname
            string fileName = n.ToString() + Path.GetExtension(file.FileName);
            // store the file inside ~/App_Data/uploads folder
            string folder = "";//HttpContext.Current.Server.MapPath(Core.Helpers.Environment.GetSingleSetting(Core.Helpers.Configuration.Enumerations.AppSettings.TempUploadPath));
            string path = Path.Combine(folder, fileName);
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            file.SaveAs(path);
            return fileName;
        }

        public string ConvertPlaintextURLsIntoHyperlinks(string s)
        {
            //Finds URLs with no protocol
            var urlregex = new Regex(@"\b\({0,1}(?<url>(www|ftp)\.[^ ,""\s<)]*)\b",
              RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //Finds URLs with a protocol
            var httpurlregex = new Regex(@"\b\({0,1}(?<url>[^>](http://www\.|http://|https://|ftp://)[^,""\s<)]*)\b",
              RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //Finds email addresses
            var emailregex = new Regex(@"\b(?<mail>[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)\b",
              RegexOptions.IgnoreCase | RegexOptions.Compiled);
            s = urlregex.Replace(s, " <a href=\"http://${url}\" target=\"_blank\">${url}</a>");
            s = httpurlregex.Replace(s, " <a href=\"${url}\" target=\"_blank\">${url}</a>");
            s = emailregex.Replace(s, "<a href=\"mailto:${mail}\">${mail}</a>");
            return s;
        }

        public string GetYoutubeVideoId(string s)
        {
            var video_id = "";
            try
            {
                video_id = Regex.Split(s, "v=")[1];
                video_id = Regex.Split(video_id, " ")[0];
                var ampersandPosition = video_id.IndexOf('&');
                if (ampersandPosition != -1)
                {
                    video_id = video_id.Substring(0, ampersandPosition);
                }
            }
            catch (Exception ex)
            {
            }
            return video_id;
        }

        public string GetYouTubeVideoThumbnailUrl(string video_id)
        {
            String YouTubeVideoThumbnailUrl = "";

            try
            {
                YouTubeVideoThumbnailUrl = "";// String.Format(Helpers.Environment.GetSingleSetting(Configuration.Enumerations.AppSettings.YouTubeVideoThumbnailUrl));

                YouTubeVideoThumbnailUrl = YouTubeVideoThumbnailUrl.Replace("video_id", video_id);
            }
            catch (Exception ex)
            {
            }
            return YouTubeVideoThumbnailUrl;
        }

        public T Setting<T>(string name) where T : class
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        public T DeserializeXML<T>(string xml) where T : class
        {
            var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(T));
            var instance = (T)serializer.Deserialize(reader);
            return instance;
        }

        public string GetPhoneNumber(string number)
        {
            number = number.Replace("+", "").Replace("-", "").Replace(" ", "");
            return number;
        }

        private string ConvertUnicodeStringToHexString(string unicodeString)
        {
            string hex = "";
            foreach (char c in unicodeString)
            {
                int tmp = c;
                hex += String.Format("{0:x4}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        /// <summary>
        /// Get response from given sms api Url
        /// </summary>
        /// <param name="sURL"></param>
        /// <returns></returns>
        private string GetResponse(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public DateTime ConvertToIndianDateTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        public string MakeUrl(string controller, string action, string routedValue)
        {
            Uri requestUrl = System.Web.HttpContext.Current.Request.Url;
            System.Web.Mvc.UrlHelper u = new System.Web.Mvc.UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            string url = string.Format("{0}{1}", requestUrl.GetLeftPart(UriPartial.Authority), u.Action(action, controller, new { id = routedValue }));
            return url;
        }

        public DateTime EpochTime {
            get {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        public string GenerateRandomString(int length) {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public void AppendMatrixToString(ref StringBuilder csv, double[,] matrix) {
            for (int i = 0; i < matrix.GetLength(0); i++) {
                for (int j = 0; j < matrix.GetLength(1); j++) {
                    if (j == 0 && i != 0) {
                        csv.Append(Environment.NewLine);
                    }
                    string s = (matrix[i, j]).ToString();
                    csv.Append(s + ",");
                }
            }
        }

        public double[] GetDoubleArray(string filePath) {
            List<double> points = new List<double>();
            
            using (StreamReader reader = new StreamReader(filePath)) {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    if (line != null) {
                        var columns = line.Split(',');
                        double point = new double();
                        point = double.Parse(columns[0]);
                        points.Add(point);
                    }
                }
            }
            return points.ToArray();
        }

        public double[,] DoubleArrayToMatrix(List<double[]> doubles) {
            double[,] toReturn = new double[doubles.Count, doubles[0].Length];
            for (int j = 0; j < doubles.Count; j++) {
                for (int i = 0; i < doubles[0].Length; i++) {
                    toReturn[j, i] = doubles[j][i];
                }
            }
            return toReturn;
        }
    }
}