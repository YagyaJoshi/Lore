using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Loregroup.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
using Loregroup.Core;
using System.Web.Http;
using Loregroup;
using System.Web.Hosting;

namespace Loregroup
{
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            //FaceBook.AuthConfig.RegisterAuth();
            AuthConfig.RegisterAuth();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityServiceLocator locator = new UnityServiceLocator(UnityConfig.RegisterComponents());
            ServiceLocator.SetLocatorProvider(() => locator);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string path = HostingEnvironment.ApplicationPhysicalPath;

            path = path + "Content\\";
                        
            //string path = @"D:\webspace\Content\";
            //string path = @"E:\webspace\qis_staging\Content\";

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            //if (!Directory.Exists(@"E:\webspace\qis_staging\Content\")) {
            //    Directory.CreateDirectory(@"E:\webspace\qis_staging\Content\");
            //}

            string spImage = "/9j/4AAQSkZJRgABAQAAAQABAAD//gA7Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c2luZyBJSkcgSlBFRyB2NjIpLCBxdWFsaXR5ID0gODAK/9sAQwAGBAUGBQQGBgUGBwcGCAoQCgoJCQoUDg8MEBcUGBgXFBYWGh0lHxobIxwWFiAsICMmJykqKRkfLTAtKDAlKCko/9sAQwEHBwcKCAoTCgoTKBoWGigoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgo/8AAEQgAUABJAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A81m6/jVVtyMOepwR+FWZmJ6Gue8TPc+Vb2dqC0t0TznBAHGPxqD6qpUVKDkyHV/EsFoWisx58w439FX/ABrnrjxFqMwjXzljWONY1EaAYAGBXTQfDfVJYf3aq823c4B4QdufpWlB8MNSjtyZ1hjkKnC53H8fyx9TS54LqePUliarvt6Hn6azqCOGF1ISP73P8609O8TPGgivYhJGCSHThgTz06Hmtm5+GetLbtLFDnYDkHjOPSuXu9HltgMujH06Ee1NTi9mY3xFN3uzvNBvILhrhrcebiwlYn0YsDj8FArKjEjSICM+prm9B1KXS71tuPKnQwyg+h4z+FdYgG3IAzk80mrHtYHEKrBu2pBJbkfe2L3z6/5/rVT8R+dWZ+hJGQvJAqb+zJv7n6ig9BStudFIMMC2CQf0zVvwbBaan4uDSRCQ2wK5YZ5zkBR+JJPtVWc5P+NW/hbDI/juSNAdm55GPthf8f1qKjaizz8TG6j6n0BpWjR3AVCqhOp47+v1q5e6RDEp8tj1wcGtDTAPJDg4QDGaoaxJ+8QxSkhmGRXPGMVHVHK5ylOyehzOsWyJEyOOCOQRXiXjG0E07MucA8qea+gfEklqNPQq43FeefwrwvxOFhaVlOVPako2kaynenqePa3a+RdMVGAe3pXW2xJtIWAJLKDwPUZrK8VxKSkq8h+K1YXeOCIMOQgHP0rrT0Iy6Npz+RZjjEdtFMmDKsrAgjIwFUgEH6mo/Nb/AJ4r/wB9t/jTS4FsQoUclsjPJIA/pUG8/wDPQ0z1Er7nUYX7RF5pAh8wb/8Ad716pZaDYaP42SXTwkQurJ5GhTorblUn2BwpryuYL5UjE4C8mvYPCt1/aHhW1vQiC7gijhnyMsuAAOfQqVNYVlpdGNaN0rlXxNb+KL2wvYdEmliNtHjezhBI5P3E56DjLEjvgGj4ZaFfW9zPPqmpS3sAYJhiThyATg9Dg16TbWJuLXEpU5+8MDH60nlQxPDa2u3bGwGF7E1C+BaHAtJOzPn3xI8uq/E3+ybi+mtNKjbDFWIJGeQP1rE1nR5tNlkS72zRmVhEImbcqfwtnAB9+O9bXjWJbP4h37zkpG1wSsh6KCxxn0+tdBqumoumee582bGVJOabnyq1inR55OVzxLV4l82ETuyQq/zHHIzUtowTfsChOCDnJb5ckk9+TV7V7N7pnhYlUZh5hxn5Rz+HI61m4EZCRD5FG1RntW0dTXA0W5t9C00nyceoH60faf8ApnF+VQKcqM9N1R7j6CrPVUUjrrjP2eRcfLnp15q1ZaldaZJA0N1KpIXzgjY3qD91sdRgdPeopGK5IBHzenoP/r1RYd/Wk9TFK+59JSarNFoTz2g8x2RVjAPBZiACfbnJ9qraLf2UmmIljfmW7jkJlkijZ2aTqQygHHUcHnGK5P4Zau17oi2lw2ZICYgf7y4yP0/lXolrJLaSSXFm7xTMAJPLAPmY6Eg9eOPWuWKs7M86pFxbSPAvinb6peXs09xcBLYQ7p3S3dRt9SSM1peGpbu88GvNOWNvFhYndSpYfQ89uK6b4jT3uooqz+aYguMOuB+XeuZsb4LohspDthjczSE9+OP5VUtdELWLu+pwPiCJhqqKrMgFvNIcdwAODWEQSCx5x3rcvZ4r691W8mlSLbakQozAbiZIxgf8BLGsSRSmVYYJAPX9a3jsd2Da5GkKn+rBxn5j/So6sRFUi3uQEHOTUv7r++n51Ru6qi7M66YEQkMBkEsRjplTWWAC2Dg4ORV6OUyQlWDHcDlj646frUnhzSJdb1qz0+ON0E0gDkDJVOCxOOmBn8aN9DOUlBNy6G98NxMt9P8Au5fsTkIZ1U7Uk/hG7pnrxXq9trEljA1veRsdi8SAZDDsfasv4hW01rf6fpumRC00WxhVYoo+FeYkHJHcgbeT/eq/YzreabFI4AYDDZ7HuKwqpRqWR5qq/WI+0tY5Pxdq1vdQFluUiJUA5OTjvwOa8u1jU/NRrWxjdbcH53YcyH/CvVNZ0T7XO4CKAT2rnbzw0om2KmT0Cgc/lU8yWxTg3q2eRazZS3EQRULSk/KB1J9K+iL/AOFNlPodtp0g2XkMKoZ0HzK4Az9RntVj4d/Ci5fWbbWdciMFpauJo7eRfnlYHI3DsucdeTXr8tnvm34O4knNd+Fjo3JHkYqtyVE6T1R8uax8Jdf0sxSWkEeqR4YBY1wydDnaTXOf8Ij4k/6A+of9+q+zRbf6RacEEBzkfQD+tXfs/sK0lSh0LjmlX7STP//Z";

            if (!Directory.Exists(path+ @"1\File\")) {
                Directory.CreateDirectory(path + @"1\File\");
            }

            //if (!Directory.Exists(@"E:\webspace\qis_staging\Content\1\File\")) {
            //    Directory.CreateDirectory(@"E:\webspace\qis_staging\Content\1\File\");
            //}

            if (!Directory.Exists(path + @"1\Thumb\")) {
                Directory.CreateDirectory(path + @"1\Thumb\");
            }

            //if (!Directory.Exists(@"E:\webspace\qis_staging\Content\1\Thumb\")) {
            //    Directory.CreateDirectory(@"E:\webspace\qis_staging\Content\1\Thumb\");
            //}

            var utilities = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<Loregroup.Core.Interfaces.Utilities.IUtilities>();
            if (!File.Exists(path + @"1\File\1.jpg"))
                utilities.Base64ToFile(spImage, path + @"1\File\1.jpg");
            if (!File.Exists(path + @"1\Thumb\1.jpg"))
                utilities.Base64ToFile(spImage, path + @"1\Thumb\1.jpg");

            //if (!File.Exists(@"E:\webspace\qis_staging\Content\1\File\1.jpg"))
            //    utilities.Base64ToFile(spImage, @"E:\webspace\qis_staging\Content\1\File\1.jpg");
            //if (!File.Exists(@"E:\webspace\qis_staging\Content\1\Thumb\1.jpg"))
            //    utilities.Base64ToFile(spImage, @"E:\webspace\qis_staging\Content\1\Thumb\1.jpg");

            //System.Data.Entity.Database.SetInitializer(new Loregroup.Data.Migrations.DatabaseInitializer());
        }

        protected void Application_Error() {
            Response.TrySkipIisCustomErrors = true; 
        }
    }
}