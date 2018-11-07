using System.Web;
using System.Web.Optimization;

namespace SmartMonitorAdmin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js",
                      "~/Scripts/metro.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jsgrid.min.js",
                      "~/Scripts/jquery.AjaxModal/jquery.ajaxmodal.min.js",
                      "~/Scripts/ui.core.js",
                      "~/Scripts/ui.dropdownchecklist.js"
                      ));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/all.css",
              "~/Content/jquery.AjaxModal/jquery.ajaxmodal.min.css",
              "~/Content/ui.dropdownchecklist.css"
              ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/jsgrid.min.css",
                "~/Content/jsgrid-theme.min.css"
                ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/bootstrap-theme.css",
                      "~/Content/metro.css",
                      "~/Content/metro-icons.css",
                      "~/Content/metro-responsive.css",
                      "~/Content/metro-rtl.css",
                      "~/Content/metro-schemes.css",
                      "~/Content/metro-colors.css",
                      "~/Content/site.css"));
        }
    }
}
