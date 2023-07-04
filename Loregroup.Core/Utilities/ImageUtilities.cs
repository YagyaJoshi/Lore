using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using Loregroup.Core.Interfaces.Utilities;

namespace Loregroup.Core.Utilities
{
    public class ImageUtilities : IImageUtilities
    {
        private readonly IErrorHandler _errorHandler;
        private readonly IUtilities _utilities;
        public ImageUtilities()
        { }
        public ImageUtilities(IErrorHandler errorHandler,
                              IUtilities utilities)
        {
            _errorHandler = errorHandler;
            _utilities = utilities;
        }

        public Boolean IsUploadedFileAnImage(HttpPostedFileBase file)
        {
            try
            {
                Bitmap img = new Bitmap(file.InputStream);

                img.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Size GetImageSize(HttpPostedFileBase file)
        {
            try
            {
                Bitmap img = new Bitmap(file.InputStream);

                Size size = img.Size;

                img.Dispose();
                img = null;

                return size;
            }
            catch (Exception ex)
            {
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, null);
            }
        }

        public Size GetImageSize(String absolutePath)
        {
            try
            {
                System.IO.FileStream bitmapFile = new System.IO.FileStream(absolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                Image img = new Bitmap(bitmapFile);

                Size size = img.Size;

                bitmapFile.Close();
                bitmapFile.Dispose();
                bitmapFile = null;
                img.Dispose();
                img = null;

                return size;
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("imagePath", absolutePath);
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        public Int64 GetImageLength(String absolutePath) {
            try {
                System.IO.FileStream bitmapFile = new System.IO.FileStream(absolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

                Int64 length = bitmapFile.Length;
                bitmapFile.Close();
                bitmapFile.Dispose();
                bitmapFile = null;
                return length;
            } catch (Exception ex) {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("imagePath", absolutePath);
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        private Image CropImage(Image img, Rectangle cropArea)
        {
            try
            {
                Image croppedImg = new Bitmap(cropArea.Width, cropArea.Height);

                Graphics gfx = Graphics.FromImage(croppedImg);
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.SmoothingMode = SmoothingMode.HighQuality;

                gfx.DrawImage(img, new Rectangle(0, 0, cropArea.Width, cropArea.Height), cropArea, GraphicsUnit.Pixel);

                gfx.Dispose();

                return croppedImg;
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("cropArea", _utilities.Serialize(cropArea));
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        public Image ResizeImage(Image imgToResize, Size size)
        {
            try
            {
                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap b = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                return (Image)b;
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("size", _utilities.Serialize(size));
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        private Image ResizeImageWidthBased(Image imgToResize, Size size)
        {
            try
            {
                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                //if (nPercentH < nPercentW)
                //    nPercent = nPercentH;
                //else
                nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                if (destHeight < size.Height)
                {
                    nPercent = nPercentH;
                    destWidth = (int)(sourceWidth * nPercent);
                    destHeight = size.Height;
                }

                Bitmap b = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.CompositingQuality = CompositingQuality.HighQuality;

                if (destHeight > size.Height)
                {
                    g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                }
                else
                {
                    b = new Bitmap(sourceWidth, destHeight);
                    g = Graphics.FromImage((Image)b);
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.CompositingQuality = CompositingQuality.HighQuality;

                    imgToResize= ResizeImage(imgToResize, new Size(destWidth, destHeight));
                    imgToResize = CropImage(imgToResize, new Rectangle((destWidth - sourceWidth) / 2, 0, sourceWidth, destHeight));
                    g.DrawImage(imgToResize, 0, 0, sourceWidth, destHeight);
                }

                g.Dispose();

                return (Image)b;
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("size", _utilities.Serialize(size));
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        public void ResizeSaveImage(String sourcePath, String destinationPath, Size imageSize, Boolean deleteSource, Boolean widthBased = false)
        {
            try
            {
                System.IO.FileStream bitmapFile = new System.IO.FileStream(sourcePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                Image img = new Bitmap(bitmapFile);

                if (widthBased)
                    img = ResizeImageWidthBased(img, imageSize);
                else
                    img = ResizeImage(img, imageSize);

                img.Save(destinationPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                bitmapFile.Close();
                bitmapFile.Dispose();
                bitmapFile = null;
                img.Dispose();
                img = null;

                if (deleteSource)
                {
                    try
                    {
                        System.IO.File.Delete(sourcePath);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("sourcePath", sourcePath);
                methodParam.Add("destinationPath", destinationPath);
                methodParam.Add("imageSize", _utilities.Serialize(imageSize));
                methodParam.Add("deleteSource", deleteSource.ToString());
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        public void ResizeSaveImageIfBigger(String sourcePath, String destinationPath, Size imageSize, Boolean deleteSource)
        {
            try
            {
                System.IO.FileStream bitmapFile = new System.IO.FileStream(sourcePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                Image img = new Bitmap(bitmapFile);

                if (img.Width > imageSize.Width || img.Height > imageSize.Height)
                    img = ResizeImage(img, imageSize);

                img.Save(destinationPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                bitmapFile.Close();
                bitmapFile.Dispose();
                bitmapFile = null;
                img.Dispose();
                img = null;

                if (deleteSource)
                {
                    try
                    {
                        System.IO.File.Delete(sourcePath);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("sourcePath", sourcePath);
                methodParam.Add("destinationPath", destinationPath);
                methodParam.Add("imageSize", _utilities.Serialize(imageSize));
                methodParam.Add("deleteSource", deleteSource.ToString());
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        public void CropResizeSaveImage(String sourcePath, String destinationPath, Rectangle imageRect, Size imageSize, Boolean deleteSource)
        {
            try
            {
                System.IO.FileStream bitmapFile = new System.IO.FileStream(sourcePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                Image img = new Bitmap(bitmapFile);

                //img = ResizeImage(img, new Size(405, 405));
                img = ResizeImage(img, imageSize);
                img = CropImage(img, imageRect);
                img.Save(destinationPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                bitmapFile.Close();
                bitmapFile.Dispose();
                bitmapFile = null;
                img.Dispose();
                img = null;

                if (deleteSource)
                {
                    try
                    {
                        System.IO.File.Delete(sourcePath);
                    }
                    catch (Exception ex)
                    { 
                    }
                }
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("sourcePath", sourcePath);
                methodParam.Add("destinationPath", destinationPath);
                methodParam.Add("imageSize", _utilities.Serialize(imageSize));
                methodParam.Add("imageRect", _utilities.Serialize(imageRect));
                methodParam.Add("deleteSource", deleteSource.ToString());
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }

        public void CropResizeSaveImage(String sourcePath, String destinationPath, Size imageSize, Boolean deleteSource)
        {
            try
            {
                System.IO.FileStream bitmapFile = new System.IO.FileStream(sourcePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                Image img = new Bitmap(bitmapFile);

                //img = ResizeImage(img, new Size(405, 405));
                img = ResizeImage(img, imageSize);
                img = CropImage(img, new Rectangle(0, 0, Math.Min(imageSize.Width, img.Width), Math.Min(imageSize.Height, img.Height)));
                img.Save(destinationPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                bitmapFile.Close();
                bitmapFile.Dispose();
                bitmapFile = null;
                img.Dispose();
                img = null;

                if (deleteSource)
                {
                    try
                    {
                        System.IO.File.Delete(sourcePath);
                    }
                    catch (Exception exe)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                NameValueCollection methodParam = new NameValueCollection();
                methodParam.Add("sourcePath", sourcePath);
                methodParam.Add("destinationPath", destinationPath);
                methodParam.Add("imageSize", _utilities.Serialize(imageSize));
                methodParam.Add("deleteSource", deleteSource.ToString());
                throw this._errorHandler.HandleError(ex, String.Format("Core.{0}", this.GetType().Name), (new System.Diagnostics.StackFrame()).GetMethod().Name, methodParam);
            }
        }
    }
}
