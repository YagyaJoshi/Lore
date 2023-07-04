using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;

namespace Loregroup.Core.Interfaces.Utilities {
    public interface IImageUtilities {
        Boolean IsUploadedFileAnImage(HttpPostedFileBase file);
        Size GetImageSize(HttpPostedFileBase file);
        Size GetImageSize(String absolutePath);
        Int64 GetImageLength(String absolutePath);
        Image ResizeImage(Image imgToResize, Size size);
        void ResizeSaveImage(String sourcePath, String destinationPath, Size imageSize, Boolean deleteSource, Boolean widthBased = false);
        void ResizeSaveImageIfBigger(String sourcePath, String destinationPath, Size imageSize, Boolean deleteSource);
        void CropResizeSaveImage(String sourcePath, String destinationPath, Rectangle imageRect, Size imageSize, Boolean deleteSource);
        void CropResizeSaveImage(String sourcePath, String destinationPath, Size imageSize, Boolean deleteSource);
    }
}
