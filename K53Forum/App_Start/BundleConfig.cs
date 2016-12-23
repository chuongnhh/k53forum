using System.Web;
using System.Web.Optimization;

namespace K53Forum
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/respond.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/website").Include(
                       "~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/showup").Include(
                       "~/Scripts/backtotop/showup.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                       "~/Theme/formstone/js/core.js",
                       "~/Theme/formstone/js/mediaquery.js",
                       "~/Theme/formstone/js/score.js",
                       "~/Theme/formstone/js/swap.js"));

            bundles.Add(new ScriptBundle("~/bundles/notify").Include(
                     "~/Scripts/bootstrap-notify.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/controller").Include(
                      "~/Scripts/controller/*.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.lumen.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/bootstrap-datetimepicker-build.less",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/showup").Include(
                      "~/Scripts/backtotop/showup.css"));

            bundles.Add(new StyleBundle("~/Content/animate").Include(
                      "~/Content/animate.min.css"));
        }
    }
}
