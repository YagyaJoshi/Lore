using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace Loregroup
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
         
            // To let AppUsers of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "142687032812341",
                appSecret: "e38fbf76d00649aea148f53bd48d991a");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "204202539994123",
            //    appSecret: "29f9715920fd3fcdf7eed44379481e4d");


            //OAuthWebSecurity.RegisterGoogleClient("googleplus");
            OAuthWebSecurity.RegisterClient(new GoogleCustomClient("179465109304-4s6242qa53mi4m0q6s7iri922tphhk1d.apps.googleusercontent.com", "s52_uQGPOSr1V99wkIIkjIck"), "Google", null);

            //OAuthWebSecurity.RegisterLinkedInClient("75j4sfbfni7vnb", "Cu5wq5axw3ydkOlt");

            
        }
    }
}
