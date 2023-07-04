using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Loregroup.Core.Helpers
{
    public class ImageResult : ActionResult
    {
        private readonly System.Drawing.Bitmap _image;

        public ImageResult(System.Drawing.Bitmap image)
        {
            _image = image;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "image/jpg";
            _image.Save(context.HttpContext.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg); 
        }
    }
}
