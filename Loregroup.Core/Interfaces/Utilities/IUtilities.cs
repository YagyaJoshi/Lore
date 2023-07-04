using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Loregroup.Core.Interfaces.Utilities
{
    public interface IUtilities
    {
        DateTime GetEpochTime();
        String Truncate(String text, Int32 count, Boolean addElipse);
        String Serialize(object serializeObject);
        Boolean IsLowerLetter(String character);
        Boolean IsUpperLetter(String character);
        Int32 ParseInt(String number);
        String CreateAbsoluteUrl(String relativeUrl);
        String EncodeTo64(string toEncode);
        String DecodeFrom64(string encodedData);
        void CreateUserFolders(Guid userId);
        String RenderPartialViewToString(Controller controller, string viewName, object model);
        Double GetTimeStamp(DateTime date);
        String ToPhoneFormat(String phone);
        Boolean IsDate(String dateString);
        String GetFormatedDateTime(DateTime dateTime);
        DateTime ConvertToDate(String dateString);
        String ConvertToJSonDateString(DateTime date);
        String ConvertDateToFavoriteFormat1(DateTime? date);
        String ConvertDateToFavoriteFormat2(DateTime? dateTime);
        String ConvertDateToFavoriteFormat3(DateTime? date);
        String ConvertDateToFavoriteFormat4(DateTime? date);
        Byte[] Image2Bytes(System.Drawing.Bitmap image);
        void Base64ToFile(string file, string filePath);
        void ResizeStream(long compressionValue, Stream fileStream, string outputPath);
        Int32 GetAge(DateTime birthDate);
        void createAlbumDirectory(Int64 peopleId, Int64 albumId);
        string UploadFile(HttpPostedFileBase file);
        string ConvertPlaintextURLsIntoHyperlinks(string s);
        string GetYoutubeVideoId(string s);
        string GetYouTubeVideoThumbnailUrl(string video_id);
        T Setting<T>(string name) where T : class;
        T Deserialize<T>(string serializedString) where T : class;
        T DeserializeXML<T>(string xml) where T : class;
        string GetPhoneNumber(string number);
        DateTime ConvertToIndianDateTime(DateTime dateTime);
        string MakeUrl(string controller, string action, string routedValue);
        string GenerateRandomString(int length);
        DateTime EpochTime
        {
            get;
        }

        void AppendMatrixToString(ref StringBuilder csv, double[,] matrix);
        double[] GetDoubleArray(string filePath);
        double[,] DoubleArrayToMatrix(List<double[]> doubles);
    }
}
