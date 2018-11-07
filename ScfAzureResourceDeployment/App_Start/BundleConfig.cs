using System.Web;
using System.Web.Optimization;

namespace ScfAzureResourceDeployment
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

          //  bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
          //"~/Scripts/jquery-ui.js"));

          //  bundles.Add(new StyleBundle("~/Content/css").Include(
          //            "~/Content/jquery-ui.css",
          //            "~/Content/jquery-ui.structure.css",
          //            "~/Content/jquery-ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/metro.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/jquery-ui.theme.css",
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
