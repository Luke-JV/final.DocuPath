﻿using System.Web;
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
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/autogrow.min.js",
                        "~/Scripts/shortcut.js",
                        "~/Scripts/chart.js",
                        "~/Scripts/mark.js",
                        "~/Scripts/jquery.maskedinput.js",
                        "~/Scripts/fort.js",
                        "~/Scripts/knockout-3.4.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                        "~/Scripts/ckeditor/ckeditor.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                      "~/Scripts/calendar.js",
                      "~/Scripts/underscore-min.js"));
            #endregion

            #region Stylesheet & Visual Script Bundles:
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js",
                          "~/Scripts/printThis.js",
                          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery.tag-editor.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/bootstrap-tagsinput.css",
                      "~/Content/calendar/calendar.css"));

            bundles.Add(new StyleBundle("~/Content/cssdark").Include(
                      "~/Content/bootstrap-dark.css",
                      "~/Content/site.css",
                      "~/Content/jquery.tag-editor.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/bootstrap-tagsinput.css",
                      "~/Content/calendar/calendar.css"));
            #endregion

            #region Default MVC Bundles:
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            #endregion

            #region AUTOTAGGER ISOLATED
            bundles.Add(new StyleBundle("~/tags/css").Include(
                     "~/Content/autotagger/jquery-ui.min.css",
                     "~/Content/autotagger/jquery-ui.structure.min.css",
                     "~/Content/autotagger/jquery-ui.theme.min.css",
                     "~/Content/autotagger/jquery.tag-editor.css"
                     ));
            bundles.Add(new ScriptBundle("~/bundles/tags").Include(
                      "~/Scripts/autotagger/jquery-ui.min.js",
                      "~/Scripts/autotagger/jquery.caret.min.js",
                      "~/Scripts/autotagger/jquery.tag-editor.min.js"
                      ));
            #endregion
        }
    }
}
