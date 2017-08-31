using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.Custom_Classes;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class MediaController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Media Item(s)")]
        public ActionResult Index()
        {
            return RedirectToAction("All");
        }

        //----------------------------------------------------------------------------------------------//

        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "Add Media Item(s)")]
        public ActionResult Add()
        {
            AddMediaViewModel model = new AddMediaViewModel();
            int id = VERTEBRAE.getCurrentUser().UserID;
            model.mediaList = db.MEDIA.Where(x => x.UserID == id  && x.MediaCaption.ToUpper() == "PENDING").ToList();
            foreach (var item in model.mediaList)
            {
                item.MediaCaption = "";
                item.MediaDescription = "";
            }
            var week = DateTime.Today.Date.AddDays(-7);
            var cases = from fc in db.FORENSIC_CASE.Where(x => x.UserID == id)
                    where fc.DateAdded >= week
                    select fc;
            model.fcList = cases.ToList();
            model.purposeList = db.MEDIA_PURPOSE.ToList();
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }

        
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Media Item(s)")]
        public ActionResult Add(AddMediaViewModel model)
        {
            try
            {
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
        [AuthorizeByAccessArea(AccessArea = "Search Media Item(s)")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.MEDIA.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View Media Item(s)")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            MediaViewModel model = new MediaViewModel();
            model.media = db.MEDIA.Where(x => x.MediaID == id).FirstOrDefault();
            model.media.USER = db.USER.Where(x => x.UserID == model.media.UserID).FirstOrDefault();
            model.media.MEDIA_PURPOSE = db.MEDIA_PURPOSE.Where(x => x.MediaPurposeID == model.media.MediaPurposeID).FirstOrDefault();
            foreach (var tag in db.MEDIA_TAG)
            {
                if (tag.MediaID == model.media.MediaID)
                {
                    model.tags.Add(db.CONTENT_TAG.Where(x=>x.ContentTagID == tag.ContentTagID).FirstOrDefault());
                }
            }
            #endregion
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Maintain Media Item Tags")]
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

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Maintain Media Item Tags")]
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
        [AuthorizeByAccessArea(AccessArea = "Delete Media Item(s)")]
        public ActionResult Delete(int id)
        {
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            //404 CONFIRM
            db.MEDIA.Where(x => x.MediaID == id).FirstOrDefault().StatusID = db.STATUS.Where(x=>x.StatusValue=="Archived").FirstOrDefault().StatusID;
            db.SaveChanges();
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return RedirectToAction("All");
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Delete Media Item(s)")]
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
        public ActionResult Upload()
        {
            return View();
        }
        #region NON-CRUD ACTIONS:
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Media Item(s)")]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    List<MEDIA> mediaItems = new List<MEDIA>();

                    //string foldername = Request.Form.Get("LCDR");
                    string userInitials = VERTEBRAE.getCurrentUser().DisplayInitials.ToUpper();
                    string datestamp = DateTime.Now.Date.ToString("ddMMyyyy");

                    string foldername = userInitials;
                    string rootpath = VERTEBRAE.MEDIA_REPORootPath + '/' + datestamp;
                    for (int i = 0; i < files.Count; i++)
                    {
                        MEDIA M = new MEDIA();

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {//404!?
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath(rootpath + foldername), fname);
                        bool exists = System.IO.Directory.Exists(Server.MapPath(rootpath + foldername));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(rootpath + foldername));
                        M.MediaLocation = fname;
                        mediaItems.Add(M);
                        file.SaveAs(fname);

                        //404 propagate to LC
                        VERTEBRAE.GenerateAndSaveThumb(fname);
                    }

                    int MediaID = 0;
                    try
                    {
                        MediaID = db.MEDIA.Max(x => x.MediaID) + 1;
                    }
                    catch (Exception)
                    {
                        MediaID = 1; //404 Propagate set to 1 after scrub
                    }
                    foreach (var M in mediaItems)
                    {
                        M.MediaID = MediaID;
                        M.IsPubliclyAccessible = false;
                        M.DateAdded = DateTime.Now;
                        M.MediaCaption = "PENDING";
                        M.MediaDescription = "PENDING";
                        M.UserID = VERTEBRAE.getCurrentUser().UserID;
                        M.StatusID = db.STATUS.Where(x => x.StatusValue == "Pending").FirstOrDefault().StatusID;
                        M.MediaPurposeID = 1;

                        MediaID++;

                        db.MEDIA.Add(M);
                    }

                    db.SaveChanges();
                    
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

        public ActionResult GetTags(string query)
        {
            return Json(_GetTags(query), JsonRequestBehavior.AllowGet);
        }

        private List<Autocomplete> _GetTags(string query)
        {
            List<Autocomplete> tags = new List<Autocomplete>();
            try
            {
                var tagResults = (from tag in db.CONTENT_TAG
                                       where tag.ContentTagText.Contains(query) || tag.ContentTagCode.Contains(query)
                                  orderby tag.ContentTagText
                                       select tag).ToList();

                foreach (var result in tagResults)
                {
                    Autocomplete tag = new Autocomplete();
                    tag.Name = "(" + result.ContentTagCode + " )" + result.ContentTagText;
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
        #endregion
    }
}
