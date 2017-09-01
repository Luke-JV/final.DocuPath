using System.Web;
using System.Web.Optimization;

namespace DocuPath
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region jQuery Bundles:
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootgrid").Include(
                        "~/Scripts/jquery.bootgrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment*"));

            bundles.Add(new ScriptBundle("~/bundles/fileinput").Include(
                        "~/Scripts/fileinput*",
                        "~/Scripts/plugins/theme.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialdatetime").Include(
                        "~/Scripts/bootstrap-material-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/toggle").Include(
                        "~/Scripts/bootstrap-toggle.js"));

            bundles.Add(new ScriptBundle("~/bundles/tageditor").Include(
                           "~/Scripts/jquery.caret.min.js",
                           "~/Scripts/jquery.tag-editor.js",
                          "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/tagsinput").Include(
                            "~/Scripts/bootstrap-tagsinput.js",
                            "~/Scripts/bootstrap-tagsinput-angular"
                            ));
            #endregion

            #region Stylesheet & Visual Script Bundles:
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js",
                          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery.tag-editor.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css"
                      , "~/Content/bootstrap-tagsinput.css"));
            #endregion

            #region Default MVC Bundles:
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            #endregion


        }
    }
}
