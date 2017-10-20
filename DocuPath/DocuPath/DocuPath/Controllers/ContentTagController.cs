using DocuPath.DataLayer;
using DocuPath.Models;
using System.Data.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocuPath.Models.DPViewModels;
using System.IO;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
  //  [LogAction]
    public class ContentTagController : Controller
    {
        string controllerName = "ContentTag";
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Content Tag")]
        public ActionResult Index()
        {
            string actionName = "Index";
            try
            {
                return RedirectToAction("All");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "Add Content Tag")]
        public ActionResult Add()
        {
            string actionName = "Add";
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Content Tag");
                #endregion

                ContentTagViewModel model = new ContentTagViewModel();

                model.tag = new CONTENT_TAG();

                var ccTags = db.CONTENT_TAG.Where(x => x.ContentTagCode.Substring(0, 3) == "CC.").ToList();
                List<double> numeric = new List<double>();
                foreach (var cctag in ccTags)
                {
                    numeric.Add(Convert.ToDouble(cctag.ContentTagCode.Substring(3)));
                }
                double max = numeric.Max();
                string newCode = "0000";
                newCode += Convert.ToString(max + 1);
                newCode = new string(newCode.ToCharArray().Reverse().ToArray());
                newCode = newCode.Substring(0, 4);
                newCode = new string(newCode.ToCharArray().Reverse().ToArray());

                model.tag.ContentTagCode = "CC." + newCode;



                //model.categories = db.TAG_CATEGORY.ToList();
                //model.subcategories = db.TAG_SUBCATEGORY.ToList();
                //model.conditions = db.TAG_CONDITION.ToList();

                return View(model);

            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Content Tag")]
        public ActionResult Add(ContentTagViewModel model)
        {
            string actionName = "Add";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Content Tag");
                #endregion
                return View(model);
            }

            try
            {
                CONTENT_TAG tag = new CONTENT_TAG();
                tag.ContentTagCode = model.tag.ContentTagCode;
                tag.ContentTagText = model.tag.ContentTagText;
                tag.TagCategoryID = model.catID;
                tag.TagSubCategoryID = model.subcatID;
                tag.TagConditionID = model.conditionID;

                try
                {
                    tag.ContentTagID = db.CONTENT_TAG.Max(x => x.ContentTagID) + 1;
                }
                catch (Exception)
                {
                    tag.ContentTagID = 0;
                }

                db.CONTENT_TAG.Add(tag);
                db.SaveChanges();
                // TODO: Add insert logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Content Tag");
                #endregion
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:
        [AuthorizeByAccessArea(AccessArea = "Search Content Tag")]
        public ActionResult All()
        {
            string actionName = "All";
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Content Tag");
                #endregion
                return View(db.CONTENT_TAG.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View Content Tag")]
        public ActionResult Details(int id)
        {
            string actionName = "Details";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "Content Tag");
                #endregion

                #region MODEL POPULATION
                CONTENT_TAG model = db.CONTENT_TAG.Where(x => x.ContentTagID == id).FirstOrDefault();
                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "Content Tag");
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Content Tag")]
        public ActionResult UpdateRepository()
        {
            string actionName = "UpdateRepository";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Content Tag");
                #endregion
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Update/Edit Content Tag")]
        public ActionResult Edit(int id)
        {
            string actionName = "Edit";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Content Tag - Edit");
                #endregion

                ContentTagViewModel model = new ContentTagViewModel();

                model.tag = db.CONTENT_TAG.Where(ct => ct.ContentTagID == id).FirstOrDefault();
                model.conditionName = model.tag.TAG_CONDITION.TagConditionName;
                model.conditionID = model.tag.TAG_CONDITION.TagConditionID;
                model.catName = model.tag.TAG_CATEGORY.TagCategoryName;
                model.catID = model.tag.TAG_CATEGORY.TagCategoryID;
                model.subcatName = model.tag.TAG_SUBCATEGORY.TagSubCategoryName;
                model.subcatID = model.tag.TAG_SUBCATEGORY.TagSubCategoryID;

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Content Tag - Edit");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Content Tag")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            string actionName = "Edit";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Content Tag");
                #endregion
                return View(collection);
            }

            try
            {
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "Content Tag");
                #endregion
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:
        //TODO: Confirm Deletion of CT possible?
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id)
        {
            string actionName = "Delete";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "Content Tag");
                #endregion
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }


        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string actionName = "Delete";
            if (!ModelState.IsValid)
            {
                return View(collection);
            }

            try
            {
                // TODO: Add delete logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:
        public ActionResult GetCategories(string query)
        {
            string actionName = "GetCategories";
            try
            {

                return Json(_GetCategories(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SysFlagFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
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
            string actionName = "GetSubcategories";
            try
            {

                return Json(_GetSubcategories(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SysFlagFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
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
            string actionName = "GetConditions";
            try
            {

                return Json(_GetConditions(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SysFlagFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
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

        public ActionResult GetTags(string query)
        {
            string actionName = "GetTags";
            try
            {

                return Json(_GetTags(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SysFlagFail, "Content Tags");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        private List<Autocomplete> _GetTags(string query)
        {
            List<Autocomplete> tags = new List<Autocomplete>();
            try
            {
                var tagResults = (from tag in db.CONTENT_TAG
                                        where (tag.ContentTagText.Contains(query) || tag.ContentTagCode.Contains(query))
                                        orderby tag.ContentTagText
                                        select tag).ToList();

                foreach (var result in tagResults)
                {
                    Autocomplete tag = new Autocomplete();
                    tag.Name = "(" + result.ContentTagCode + ") " + result.ContentTagText;
                    tag.Id = result.ContentTagID;
                    tags.Add(tag);
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
            return tags;
        }
        
        public ActionResult ParseXML()
        {
            string actionName = "ParseXML";
            #region AUDIT_WRITE
            AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadInit, "Legacy Case");
            #endregion
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    ////  Get all files from Request object  
                    //HttpFileCollectionBase files = Request.Files;
                    
                    //string foldername = Request.Form.Get("LCDR");
                    //string rootpath = VERTEBRAE.LC_REPORootPath;
                    //for (int i = 0; i < files.Count; i++)
                    //{
                    //    LEGACY_DOCUMENT doc = new LEGACY_DOCUMENT();
                    //    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    //    HttpPostedFileBase file = files[i];
                    //    string fname;

                    //    // Checking for Internet Explorer  
                    //    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    //    {//404!?
                    //        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    //        doc.LegacyDocumentTitle = testfiles[testfiles.Length - 1];
                    //        fname = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + i.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                    //    }
                    //    else
                    //    {
                    //        fname = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + i.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                    //        doc.LegacyDocumentTitle = file.FileName;

                    //    }

                    //    // Get the complete folder path and store the file inside it.  
                    //    fname = Path.Combine(Server.MapPath(rootpath + foldername), fname);
                    //    bool exists = System.IO.Directory.Exists(Server.MapPath(rootpath + foldername));

                    //    if (!exists)
                    //        System.IO.Directory.CreateDirectory(Server.MapPath(rootpath + foldername));
                    //    doc.LegacyDocumentLocation = fname;
                    //    docs.Add(doc);
                    //    file.SaveAs(fname);
                    //}
                    //int docId = db.LEGACY_DOCUMENT.Max(x => x.LegacyDocumentID);

                    //foreach (var item in docs)
                    //{
                    //    docId++;
                    //    item.LegacyDocumentID = docId;
                    //    item.LegacyCaseID = LC.LegacyCaseID;
                    //    db.LEGACY_DOCUMENT.Add(item);
                    //}
                    //db.SaveChanges();
                    //// Returns message that successfully uploaded  
                    //#region AUDIT_WRITE
                    //AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadSuccess, "Legacy Case");
                    //#endregion
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    #region AUDIT_WRITE
                    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Legacy Case");
                    #endregion
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        #endregion
    }
}
