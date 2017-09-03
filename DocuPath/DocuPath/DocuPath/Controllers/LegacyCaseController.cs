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
using System.IO.Compression;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    [LogAction]
    public class LegacyCaseController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Legacy Case")]
        public ActionResult Index()
        {
            try
            {

                return RedirectToAction("All");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        //----------------------------------------------------------------------------------------------//

        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "Add Legacy Case")]
        public ActionResult Add()
        {
            try
            {

                var model = new LEGACY_CASE();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Legacy Case");
                #endregion
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Legacy Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }
        
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Legacy Case")]
        public ActionResult Add(LEGACY_CASE LC)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Legacy Case");
                #endregion
                return View(LC);
            }

            try
            {
                LEGACY_CASE update = new LEGACY_CASE();
                update = db.LEGACY_CASE.Where(x=>x.LegacyDRNumber == LC.LegacyDRNumber).FirstOrDefault();
                update.LCBriefDescription = LC.LCBriefDescription;
                update.DateClosed = LC.DateClosed;


                db.LEGACY_CASE.Attach(update);
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                // TODO: Add insert logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Legacy Case");
                #endregion
                return RedirectToAction("All");
            }
            catch(Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Legacy Case");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region READS:
        
        [AuthorizeByAccessArea(AccessArea = "Search Legacy Case")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Legacy Case");
                #endregion
                return View(db.LEGACY_CASE.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Legacy Case");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        
        [AuthorizeByAccessArea(AccessArea = "View Legacy Case")]
        public ActionResult Details(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "Legacy Case");
                #endregion

                LegacyCaseViewModel model = new LegacyCaseViewModel();
                model.legacyCase = db.LEGACY_CASE.Where(x => x.LegacyCaseID == id).FirstOrDefault();
                model.legacyCase.USER = db.USER.Where(x => x.UserID == model.legacyCase.UserID).FirstOrDefault();


                model.legacyDocs = db.LEGACY_DOCUMENT.Where(x => x.LegacyCaseID == model.legacyCase.LegacyCaseID).ToList();

                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(model.legacyCase.UserID);
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "Legacy Case");
                #endregion
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "Legacy Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View Legacy Case")]
        public ActionResult ViewDoc(int id)
        {
            try
            {
                string location = GetVirtualPath(db.LEGACY_DOCUMENT.Where(x => x.LegacyDocumentID == id).FirstOrDefault().LegacyDocumentLocation);

                var file = File(location, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(location));
                file.FileDownloadName = db.LEGACY_DOCUMENT.Where(x => x.LegacyDocumentID == id).FirstOrDefault().LegacyDocumentTitle;
                return file;
            }
            catch (Exception)
            {

                return null;
            }                    
        }

        [AuthorizeByAccessArea(AccessArea = "View Legacy Case")]
        public ActionResult ZipAll(int id)
        {
            try
            {

                string location = db.LEGACY_DOCUMENT.Where(x => x.LegacyCaseID == id).FirstOrDefault().LegacyDocumentLocation;
                location = location.Replace("\\", "/");
                var split = location.Split('/');
                string source = "";
                string destination = "";
                for (int i = 0; i < split.Length - 1; i++)
                {
                    if (i < split.Length - 2)
                    {
                        destination += split[i] + '/';
                    }
                    source += split[i] + '/';

                }
                System.IO.DirectoryInfo di = new DirectoryInfo(destination + "ZIP/");

                foreach (FileInfo FI in di.GetFiles())
                {
                    FI.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                ZipFile.CreateFromDirectory(source, destination + "/ZIP/test.zip");

                var file = File(destination + "/ZIP/test.zip", System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(destination + "/ZIP/test.zip"));
                file.FileDownloadName = "x.zip"; //404 404



                return file;

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Legacy Case")]
        public ActionResult Edit(int id)
        {
            try
            {

                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Legacy Case");
                #endregion
                return View();
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Legacy Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Legacy Case")]
        public ActionResult Edit(int id, LEGACY_CASE updatedLC)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Legacy Case");
                #endregion
                return View(updatedLC);
            }

            try
            {
                #region DB UPDATE
                db.LEGACY_CASE.Attach(updatedLC);
                db.Entry(updatedLC).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "Legacy Case");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Legacy Case");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region DELETES:
        [AuthorizeByAccessArea(AccessArea = "Archive Legacy Case")]
        public ActionResult Delete(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "Legacy Case");
                #endregion
                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion
                //404 CONFIRM!
                db.LEGACY_CASE.Where(x => x.LegacyCaseID == id).FirstOrDefault().StatusID = db.STATUS.Where(x => x.StatusValue == "Archived").FirstOrDefault().StatusID;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "Legacy Case");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Legacy Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Archive Legacy Case")]
        public ActionResult Delete(int id, FormCollection collection)
        {
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
        [AuthorizeByAccessArea(AccessArea = "Add Legacy Case")]
        public ActionResult UploadFiles()
        {
            #region AUDIT_WRITE
            AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadInit, "Legacy Case");
            #endregion
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    List<LEGACY_DOCUMENT> docs = new List<LEGACY_DOCUMENT>();                    
                    
                    LEGACY_CASE LC = new LEGACY_CASE();
                    try
                    {
                        LC.LegacyCaseID = db.LEGACY_CASE.Max(x => x.LegacyCaseID) + 1;
                    }
                    catch (Exception)
                    {
                        LC.LegacyCaseID = 0;
                    }
                    LC.LCBriefDescription = "N/A";
                    LC.DateAdded = DateTime.Now;
                    LC.DateClosed = default(DateTime);
                    LC.LegacyDRNumber = Request.Form.Get("LCDR");
                    LC.STATUS = db.STATUS.Where(x => x.StatusValue == "Active").First();
                    LC.UserID = VERTEBRAE.getCurrentUser().UserID;
                    db.LEGACY_CASE.Add(LC);
                    db.SaveChanges();



                    string foldername = Request.Form.Get("LCDR");
                    string rootpath = VERTEBRAE.LC_REPORootPath;
                    for (int i = 0; i < files.Count; i++)
                    {
                        LEGACY_DOCUMENT doc = new LEGACY_DOCUMENT();
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {//404!?
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            doc.LegacyDocumentTitle = testfiles[testfiles.Length - 1];
                            fname = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + i.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                        }
                        else
                        {
                            fname = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + i.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                            doc.LegacyDocumentTitle = file.FileName;
                            
                        }
                        
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath(rootpath + foldername), fname);
                        bool exists = System.IO.Directory.Exists(Server.MapPath(rootpath + foldername));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(rootpath + foldername));
                        doc.LegacyDocumentLocation = fname;
                        docs.Add(doc);
                        file.SaveAs(fname);
                    }
                    int docId = db.LEGACY_DOCUMENT.Max(x => x.LegacyDocumentID);
                    
                    foreach (var item in docs)
                    {
                        docId++;
                        item.LegacyDocumentID = docId;
                        item.LegacyCaseID = LC.LegacyCaseID;
                        db.LEGACY_DOCUMENT.Add(item);
                    }
                    db.SaveChanges();
                    // Returns message that successfully uploaded  
                    #region AUDIT_WRITE
                    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadSuccess, "Legacy Case");
                    #endregion
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

        private string GetVirtualPath(string physicalPath)
        {
            try
            {

                string rootpath = Server.MapPath("~/");

                physicalPath = physicalPath.Replace(rootpath, "");
                physicalPath = physicalPath.Replace("\\", "/");

                return "~/" + physicalPath;
            }
            catch (Exception)
            {
                return physicalPath;
            }
        }
        #endregion
    }
}
