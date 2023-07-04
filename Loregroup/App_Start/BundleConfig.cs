using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Loregroup.App_Start
{
    public class BundleConfig {
         // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            //
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            //

            bundles.Add(new StyleBundle("~/cssBundle")
                //.Include("~/Content/css/jquery-fileupload-ui.css", new CssRewriteUrlTransform())
                .Include(
                    // bootstrap 3.0.2
                    "~/Content/css/bootstrap.css", new CssRewriteUrlTransform())
                // font Awesome
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransform())
                // Ionicons
                .Include("~/Content/css/ionicons.min.css", new CssRewriteUrlTransform())
                // CSS Spinners
                .Include("~/Content/css/spinner.css", new CssRewriteUrlTransform())
                // Theme style
                .Include("~/Content/css/AdminLTE.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/Desertfire.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/Desertfire-uploader.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/dropdownliststyle.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/typeaheadjs.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/tagmanager.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/dropdownliststyle.css", new CssRewriteUrlTransform())
                 .Include("~/Content/css/Multiselect.css", new CssRewriteUrlTransform())
                );

            bundles.Add(new StyleBundle("~/datatablesBundle").Include(
                // datatable
                        "~/Content/css/datatables/dataTables.bootstrap.css", new CssRewriteUrlTransform()
                ));

            bundles.Add(new StyleBundle("~/fullcalendarBundle").Include(
                // fullCalendar
                "~/Content/css/fullcalendar/fullcalendar.css", new CssRewriteUrlTransform()
                ));

            bundles.Add(new StyleBundle("~/morrisBundle").Include(
                // jvectormap
                "~/Content/css/morris/morris.css", new CssRewriteUrlTransform()
                ));

            bundles.Add(new StyleBundle("~/jvectormapBundle").Include(
                // Morris chart
                "~/Content/css/jvectormap/jquery-jvectormap-1.2.2.css", new CssRewriteUrlTransform()
                ));

            bundles.Add(new StyleBundle("~/daterangepickerBundle").Include(
                 // Daterange picker
                "~/Content/css/daterangepicker/daterangepicker-bs3.css", new CssRewriteUrlTransform()
                ));

            bundles.Add(new StyleBundle("~/bootstrap-wysihtml5Bundle").Include(
                 // Daterange picker
                "~/Content/css/bootstrap-wysihtml5/bootstrap3-wysihtml5.css", new CssRewriteUrlTransform()
                ));



            bundles.Add(new ScriptBundle("~/Scripts/LibrariesBundle").Include(
                "~/Scripts/Libraries/jquery.cookie.js",
                //"~/Scripts/Libraries/jquery-ui-1.10.3.js",
                //"~/Scripts/Libraries/jquery.fileupload.js",
                "~/Scripts/Libraries/jquery.iframe-transport.js",
                "~/Scripts/Libraries/bootstrap.js",
                "~/Scripts/Libraries/raphael-min.js",
                "~/Scripts/Libraries/jquery.unobtrusive-ajax.js",
                "~/Scripts/Libraries/jquery.validate.js",
                "~/Scripts/Libraries/jquery.validate.unobtrusive.js",
                "~/Scripts/Libraries/typeahead.bundle.js",
                "~/Scripts/jquery.sumoselect.js"
                //"~/Scripts/Libraries/bootstrap-tagsinput.js",
               
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/MorrisBundle").Include(
                        "~/Scripts/Plugins/morris/morris.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/datatablesBundle").Include(
                        "~/Scripts/Plugins/datatables/jquery.dataTables.js",
                         "~/Scripts/Plugins/datatables/dataTables.bootstrap.js"
                        ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/sparklineBundle").Include(
                        "~/Scripts/Plugins/sparkline/jquery.sparkline.js"
                        ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/jvectormapBundle").Include(
                        "~/Scripts/Plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        "~/Scripts/Plugins/jvectormap/jquery-jvectormap-world-mill-en.js"
                        ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/fullcalendarBundle").Include(
                       "~/Scripts/Plugins/fullcalendar/fullcalendar.js"
                       ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/jqueryKnobBundle").Include(
                       "~/Scripts/Plugins/jqueryKnob/jquery.knob.js"
                       ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/daterangepickerBundle").Include(
                       "~/Scripts/Plugins/daterangepicker/daterangepicker.js"
                       ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/bootstrap-wysihtml5Bundle").Include(
                       "~/Scripts/Plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/Scripts/Plugins/icheckBundle").Include(
                       "~/Scripts/Plugins/icheck/icheck.js"
                       ));

            bundles.Add(new ScriptBundle("~/ScriptsBundle").Include(
                        "~/Scripts/app.js",
                       "~/Scripts/Comm.js",
                       "~/Scripts/datatable.js",
                       "~/Scripts/Navigation.js",
                       "~/Scripts/Users.js",
                       "~/Scripts/Desertfire.js",
                       //"~/Scripts/CityDekh.js",
                        //"~/Scripts/uploads.js",
                       "~/Scripts/AppUsers.js",
                       "~/Scripts/min.js"
                       ));

        }

        public class CssRewriteUrlTransformWrapper : IItemTransform {
            public string Process(string includedVirtualPath, string input) {
                return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
            }
        }
    }
}