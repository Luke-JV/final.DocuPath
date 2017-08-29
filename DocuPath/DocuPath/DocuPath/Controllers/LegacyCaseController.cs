using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class LegacyCaseController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: LegacyCase
        public ActionResult Index()
        {
            //404 - redirect to /All
            return View();
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: LegacyCase/Create
        public ActionResult Add()
        {
            var model = new LEGACY_CASE();
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }

        // POST: LegacyCase/Create
        [HttpPost]
        public ActionResult Add(LEGACY_CASE LC)
        {
            try
            {
                db.LEGACY_CASE.Add(LC);
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
        // GET: LegacyCase/All
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.LEGACY_CASE.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        // GET: LegacyCase/Details/5
        public ActionResult Details(int id)
        {
            LegacyCaseViewModel model = new LegacyCaseViewModel();
            model.legacyCase = db.LEGACY_CASE.Where(x=>x.LegacyCaseID == id).FirstOrDefault();
            model.legacyCase.USER = db.USER.Where(x=>x.UserID == model.legacyCase.UserID).FirstOrDefault();
            //model.legacyCase.USER.FirstOrDefault();

            model.legacyDocs = db.LEGACY_DOCUMENT.Where(x => x.LegacyCaseID == model.legacyCase.LegacyCaseID).ToList();

            #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(model.legacyCase.UserID);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: LegacyCase/Edit/5
        public ActionResult Edit(int id)
        {
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: LegacyCase/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, LEGACY_CASE updatedLC)
        {
            try
            {
                #region DB UPDATE
                db.LEGACY_CASE.Attach(updatedLC);
                db.Entry(updatedLC).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

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
        // GET: LegacyCase/Delete/5
        public ActionResult Delete(int id)
        {
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: LegacyCase/Delete/5
        [HttpPost]
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
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    string foldername = Request.Form.Get("LCDR");
                    string rootpath = VERTEBRAE.LC_REPORootPath;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = DateTime.Now.ToString("ddmmyyyy_HHmmss") + "_" + i.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                            //fname = file.FileName;
                            //fname = VERTEBRAE.RenameFileForStorage() 404;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath(rootpath + foldername), fname);
                        bool exists = System.IO.Directory.Exists(Server.MapPath(rootpath + foldername));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(rootpath + foldername));

                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
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
