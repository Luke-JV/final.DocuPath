using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DocuPath.Models.DPViewModels.ToggleViewModel;

namespace DocuPath.Controllers
{
    public class TestController : Controller
    {

        DocuPathEntities db = new DocuPathEntities();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TagTest()
        {
            ToggleViewModel model = new ToggleViewModel();
            model.tog1 = false;
            model.tog2 = false;
            model.areas = new List<areaKVP>();

            foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > Forensic Case"))
            {
                areaKVP area = new areaKVP();
                area.areaName = item.AccessAreaDescription;
                area.hasAccess = false;
                model.areas.Add(area);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult TagTest(ToggleViewModel model)
        {
            // var x = model;

            return View();
        }

        public ActionResult TagCloud()
        {
            TagCloudViewModel model = new TagCloudViewModel();
            return View(model);
        }

        public ActionResult GetTags(string query)
        {
            return Json(_GetTags(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetTags(string query)
        {
            List<Autocomplete> categories = new List<Autocomplete>();
            try
            {
                var categoryResults = (from cat in db.TAG_CATEGORY
                                       where cat.TagCategoryName.Contains(query)
                                       orderby cat.TagCategoryName
                                       select cat).ToList();

                foreach (var result in categoryResults)
                {
                    Autocomplete category = new Autocomplete();
                    category.Name = result.TagCategoryName;
                    category.Id = result.TagCategoryID;
                    categories.Add(category);
                }
            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    RedirectToAction("Error", "Home", eceex.Message);
                }
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                RedirectToAction("Error", "Home", x.Message);
            }
            return categories;
        }

        public ActionResult tagEditor()
        {
            return View();
        }
        
        public ActionResult fetch()
        {
            return View();
        }
    }
}