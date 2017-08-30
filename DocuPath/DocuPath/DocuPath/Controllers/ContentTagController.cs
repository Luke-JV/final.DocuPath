using DocuPath.DataLayer;
using DocuPath.Models;
using System.Data.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocuPath.Models.DPViewModels;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class ContentTagController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea = "Search Content Tag")]
        public ActionResult Index()
        {
            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//

        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Add()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion

            ContentTagViewModel model = new ContentTagViewModel();
            // TODO: Max of CUS.### + 1 -->
            model.tag = new CONTENT_TAG();
            model.tag.ContentTagCode = "CC.0001";
            //model.categories = db.TAG_CATEGORY.ToList();
            //model.subcategories = db.TAG_SUBCATEGORY.ToList();
            //model.conditions = db.TAG_CONDITION.ToList();
            
            return View(model);
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Add(CONTENT_TAG tag)
        {
            try
            {
                db.CONTENT_TAG.Add(tag);
                db.SaveChanges();
                // TODO: Add insert logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:
        
        [AuthorizeByAccessArea(AccessArea = "Search Content Tag")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.CONTENT_TAG.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            CONTENT_TAG model =  db.CONTENT_TAG.Where(x => x.ContentTagID == id).FirstOrDefault();
            #endregion
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:
        public ActionResult GetCategories(string query)
        {
            return Json(_GetCategories(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetCategories(string query)
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

        public ActionResult GetSubcategories(string query)
        {
            return Json(_GetSubcategories(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetSubcategories(string query)
        {
            List<Autocomplete> subcategories = new List<Autocomplete>();
            try
            {
                var subcategoryResults = (from subcat in db.TAG_SUBCATEGORY
                                       where subcat.TagSubCategoryName.Contains(query)
                                       orderby subcat.TagSubCategoryName
                                       select subcat).ToList();

                foreach (var result in subcategoryResults)
                {
                    Autocomplete subcategory = new Autocomplete();
                    subcategory.Name = result.TagSubCategoryName;
                    subcategory.Id = result.TagSubCategoryID;
                    subcategories.Add(subcategory);
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
            return subcategories;
        }

        public ActionResult GetConditions(string query)
        {
            return Json(_GetConditions(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetConditions(string query)
        {
            List<Autocomplete> conditions = new List<Autocomplete>();
            try
            {
                var conditionResults = (from condition in db.TAG_CONDITION
                                          where condition.TagConditionName.Contains(query)
                                          orderby condition.TagConditionName
                                          select condition).ToList();

                foreach (var result in conditionResults)
                {
                    Autocomplete condition = new Autocomplete();
                    condition.Name = result.TagConditionName;
                    condition.Id = result.TagConditionID;
                    conditions.Add(condition);
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
            return conditions;
        }

        #endregion
    }
}
