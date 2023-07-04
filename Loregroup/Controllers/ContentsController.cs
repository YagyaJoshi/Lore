using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Interfaces.Providers;
using System.IO;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Core.Logmodels;

namespace Loregroup.Controllers
{
    public class ContentsController : Controller
    {
        private readonly IContentProvider _contentProvider;
        private readonly IConfigSettingProvider _configSettingProvider;
        private readonly ISession _session;
        private readonly IUtilities _utilities;

        public ContentsController(IContentProvider contentProvider, IConfigSettingProvider configSettingProvider,
            ISession session, IUtilities utilities)
        {
            _contentProvider = contentProvider;
            _configSettingProvider = configSettingProvider;
            _session = session;
            _utilities = utilities;
        }

        //manisha
        //public JsonResult Index(Int64 id)
        //{
        //    //try
        //    //{
        //    //    var fileDetails = _contentProvider.GetFile(id);
        //    //    LogMe.Log("ContentsController", LogMeCommonMng.LogType.Info, "file details show success.");                
        //    //    return Json(fileDetails, JsonRequestBehavior.AllowGet);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    var fileDetails = _contentProvider.GetFile(id);
        //    //    LogMe.Log("ContentsController", LogMeCommonMng.LogType.Error, ex.Message);
        //    //    return Json(fileDetails, JsonRequestBehavior.AllowGet);
        //    //}
            
        //}

        //public FileStreamResult File(Int64 id)
        //{
        //    //try
        //    //{
        //    //    var fileDetails = _contentProvider.GetFile(id);
        //    //    Stream image =
        //    //        new FileStream(
        //    //            _configSettingProvider.GetSetting<String>(Core.Enumerations.ConfigSetting.Storage) +
        //    //            fileDetails.FilePath, FileMode.Open);
        //    //    LogMe.Log("ContentController", LogMeCommonMng.LogType.Info, "get file name details and file path storage success.");

        //    //    return File(image, "image/jpg", fileDetails.FileName);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    var fileDetails = _contentProvider.GetFile(id);
        //    //    Stream image = new FileStream(_configSettingProvider.GetSetting<String>(Core.Enumerations.ConfigSetting.Storage) + fileDetails.FilePath, FileMode.Open);
        //    //    LogMe.Log("ContentsController", LogMeCommonMng.LogType.Error, ex.Message);
        //    //    return File(image, "image/jpg", fileDetails.FileName);
        //    //}
        //}

        //public FileStreamResult Thumb(Int64 id)
        //{
        //    //try
        //    //{
        //    //    var fileDetails = _contentProvider.GetFile(id);
        //    //    Stream image =
        //    //        new FileStream(
        //    //            _configSettingProvider.GetSetting<String>(Core.Enumerations.ConfigSetting.Storage) +
        //    //            fileDetails.ThumbnailPath, FileMode.Open);
        //    //    //LogMe.Log("ContentController", LogMeCommonMng.LogType.Info, "profiles image save using thumbnail success.");
        //    //    return File(image, "image/jpg", fileDetails.ThumbnailName);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    var fileDetails = _contentProvider.GetFile(id);
        //    //    Stream image = new FileStream(_configSettingProvider.GetSetting<String>(Core.Enumerations.ConfigSetting.Storage) + fileDetails.ThumbnailPath, FileMode.Open);
        //    //    LogMe.Log("ContentsController", LogMeCommonMng.LogType.Error, ex.Message);
        //    //    return File(image, "image/jpg", fileDetails.ThumbnailName);
        //    //}
        //}

        //[HttpPost]
        //public JsonResult Upload(FileUploadDetailModel model) {
        //    JsonUploadModel retModel = new JsonUploadModel() {
        //        Message = "Cannot upload file.",
        //        Result = false
        //    };

        //    for (int i = 0; i < Request.Files.Count; i++) {
        //        Int64 folderName = _contentProvider.GetCurrentId();
        //        string folderPath = _configSettingProvider.GetSetting<string>(ConfigSetting.Storage) + folderName + @"\";
        //        string fileExtention = "";
        //        string fileName = "";

        //        if (!Directory.Exists(folderPath)) {
        //            Directory.CreateDirectory(folderPath);
        //        }
        //        if (!Directory.Exists(folderPath + @"File\")) {
        //            Directory.CreateDirectory(folderPath + @"File\");
        //        }
        //        if (!Directory.Exists(folderPath + @"Thumb\")) {
        //            Directory.CreateDirectory(folderPath + @"Thumb\");
        //        }

        //        HttpPostedFileBase httpPostedFileBase = Request.Files[i];
        //        if (httpPostedFileBase != null) {
        //            fileExtention = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
        //            fileName = folderName + "." + fileExtention;

        //            var databaseId = _contentProvider.SaveContent(new FileDetailViewModel() {
        //                CreatedById = _session.CurrentUser.Id,
        //                FileRelation = model.Relation,
        //                FileName = httpPostedFileBase.FileName,
        //                FilePath =  folderName + @"/File/"+ fileName,
        //                ThumbnailName= httpPostedFileBase.FileName,
        //                ThumbnailPath = folderName + @"/Thumb/" + fileName,
        //                FileType = FileType.Anonymous,
        //                ThumbnailFileType = FileType.Anonymous,
        //                ModifiedById = _session.CurrentUser.Id,
        //                Status = Status.Active,
        //                IsInFileSystem = true
        //            });

        //            var savedFile = _contentProvider.GetFile(databaseId);

        //            httpPostedFileBase.SaveAs(folderPath + @"File\" + fileName);
        //            httpPostedFileBase.SaveAs(folderPath + @"Thumb\" + fileName);

        //            retModel.Result = true;
        //            retModel.File = httpPostedFileBase.FileName;
        //            retModel.Weight = httpPostedFileBase.ContentLength.ToString() + "bytes.";
        //            retModel.ImageId = databaseId.ToString();
        //            retModel.Message = _utilities.RenderPartialViewToString(this, "_UploadSuccess", retModel);
                    
        //        }
        //    }
        //    return Json(retModel);
        //}
    }
}