using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;

using System.IO;

using Loregroup.Core;
namespace Loregroup.Core
{
    public class CommonClass
    {
        public static int Questionno = 0;
        public static int QuestionCount = 0;
        

        public static String packagename = "";
        public static int Days = 0;
        public static float Amount = 0;
        public static Int64 OfferId = 0;

        public static string CustomerCode = "";
        public static Int64 PolicyCode = 0;
        public static Int64 customerid = 0;
        public static Int64 printid = 0;
        public static Int64 TemplateId = 0;

        public static bool LoginId = false;

        public static bool ReportCheck;
        public static Int64 Userid = 0;
        public static string[] OTParr;
        public static string RecentOtp = "";
        public static Int64 roleid = 0;
        public static Int64 Classifiedid = 0; 
        public static Int64 Storeidnew = 0;

        public static bool EditMode = false;
        public static string StoreDetails = "";




    }
}
