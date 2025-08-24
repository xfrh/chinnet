using System.Web;
using System.Web.Optimization;

namespace ManageSystem.Admin.App_Start
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            //Admin
            bundles.Add(new ScriptBundle("~/admin/script").Include(
            "~/Content/Scripts/jquery-1.10.2.min.js",
            "~/Content/Scripts/modernizr-2.6.2.js",
            "~/Content/Scripts/Admin/jquery.validate.min.js",
            "~/Content/Scripts/Admin/jquery.validate.unobtrusive.min.js",
            "~/Content/Scripts/kendo/2014.1.318/kendo.web.min.js",
            "~/Content/Scripts/kendo/2014.1.318/kendo.aspnetmvc.min.js",
             "~/Content/Scripts/Admin/jquery-migrate-1.2.1.min.js",
            "~/Content/Scripts/Admin/admin.common.js",
            "~/Content/Scripts/jquery.easydropdown.js"

            ));

            bundles.Add(new StyleBundle("~/admin/css").Include(
            "~/Content/Css/bootstrap.min.css",
            "~/Content/kendo/2014.1.318/kendo.common.min.css",
             "~/Content/kendo/2014.1.318/kendo.default.min.css",
              "~/Content/Css/easydropdown.css",
                "~/Content/Css/Admin/Style.css",
                    "~/Content/Css/Site.css"
            ));

            //前台 Web
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Content/Scripts/jquery-1.10.2.min.js",
                  "~/Content/Scripts/modernizr-2.6.2.js",
                  "~/Content/Scripts/Admin/jquery.validate.min.js",
                  "~/Content/Scripts/Admin/jquery.validate.unobtrusive.min.js",
                  "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Scripts/bootstrap.js",
                      "~/Content/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Css/bootstrap.css",
                      "~/Content/Css/site.css"));

        }
    }
}