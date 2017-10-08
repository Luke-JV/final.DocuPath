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
    //[LogAction]
    public class MediaController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Media Item(s)")]
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
        [AuthorizeByAccessArea(AccessArea = "Add Media Item(s)")]
        public ActionResult Add()
        {
            try
            {

                AddMediaViewModel model = new AddMediaViewModel();
                int id = VERTEBRAE.getCurrentUser().UserID;
                model.mediaList = db.MEDIA.Where(x => x.UserID == id && x.MediaCaption.ToUpper() == "PENDING").ToList();
                if (model.mediaList.Count <1)
                {
                    //Response.
                    return RedirectToAction("All");
                }
                foreach (var item in model.mediaList)
                {
                    item.MediaCaption = "";
                    item.MediaDescription = "";
                    item.ForensicCaseID = 0;
                }
                var week = DateTime.Today.Date.AddDays(-7);
                var cases = from fc in db.FORENSIC_CASE.Where(x => x.UserID == id)
                            where fc.DateAdded >= week
                            select fc;
                model.fcList = cases.ToList();
                model.purposeList = db.MEDIA_PURPOSE.ToList();
                if (model.fcList.Count <1)
                {
                    model.purposeList.Remove(model.purposeList.Where(x=>x.MediaPurposeValue=="Case Related").FirstOrDefault());
                }
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Media");
                #endregion
                
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Media Item(s)")]
        public ActionResult Add(AddMediaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Media");
                #endregion
                return View(model);
            }

            try
            {
                foreach (var item in model.mediaList)
                {
                    var input = db.MEDIA.Where(x => x.MediaID == item.MediaID).FirstOrDefault();
                    input.MediaDescription = item.MediaDescription;
                    input.MediaCaption = item.MediaCaption;
                    input.MediaPurposeID = item.MediaPurposeID;
                    input.StatusID = db.STATUS.Where(x => x.StatusValue == "Active").FirstOrDefault().StatusID;
                    if(item.ForensicCaseID !=null)
                    {
                        input.ForensicCaseID = item.ForensicCaseID;
                    }
                }
                db.SaveChanges();
                
                // TODO: Add insert logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Media");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Media");
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
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Media");
                #endregion
               
                List<MEDIA> model = new List<MEDIA>();
                USER user = VERTEBRAE.getCurrentUser();
                string AccessLevel = VERTEBRAE.getCurrentUser().USER_LOGIN.ACCESS_LEVEL.LevelName;
                if (AccessLevel == "Superuser" || AccessLevel == "Master Access")
                {
                    model = db.MEDIA.Where(x=>x.STATUS.StatusValue != "Pending").ToList();
                }
                else
                {
                    model = db.MEDIA.Where(x=>x.UserID == user.UserID && x.STATUS.StatusValue!="Pending").ToList();
                }
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(user.UserID, TxTypes.SearchSuccess, "Media");
                #endregion
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    throw new Exception("No Users found.");
                }
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View Media Item(s)")]
        public ActionResult Details(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "Media");
                #endregion
                #region MODEL POPULATION
                MediaViewModel model = new MediaViewModel();
                model.media = db.MEDIA.Where(x => x.MediaID == id).FirstOrDefault();
                model.media.USER = db.USER.Where(x => x.UserID == model.media.UserID).FirstOrDefault();
                model.media.MEDIA_PURPOSE = db.MEDIA_PURPOSE.Where(x => x.MediaPurposeID == model.media.MediaPurposeID).FirstOrDefault();
                foreach (var tag in db.MEDIA_TAG)
                {
                    if (tag.MediaID == model.media.MediaID)
                    {
                        model.tags.Add(db.CONTENT_TAG.Where(x => x.ContentTagID == tag.ContentTagID).FirstOrDefault());
                    }
                }
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "Media");
                #endregion
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Maintain Media Item Tags")]
        public ActionResult Edit(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Media");
                #endregion
                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                MediaViewModel model = new MediaViewModel();
                model.media = db.MEDIA.Where(x => x.MediaID == id).FirstOrDefault();
                model.media.USER = db.USER.Where(x => x.UserID == model.media.UserID).FirstOrDefault();
                model.media.MEDIA_PURPOSE = db.MEDIA_PURPOSE.Where(x => x.MediaPurposeID == model.media.MediaPurposeID).FirstOrDefault();
                foreach (var tag in db.MEDIA_TAG)
                {
                    if (tag.MediaID == model.media.MediaID)
                    {
                        model.tags.Add(db.CONTENT_TAG.Where(x => x.ContentTagID == tag.ContentTagID).FirstOrDefault());
                    }
                }

                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Maintain Media Item Tags")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Media");
                #endregion
                return View(collection);
            }

            try
            {
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "Media");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Media");
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
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "Media");
                #endregion
                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                //404 CONFIRM
                db.MEDIA.Where(x => x.MediaID == id).FirstOrDefault().StatusID = db.STATUS.Where(x => x.StatusValue == "Archived").FirstOrDefault().StatusID;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "Media");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Delete Media Item(s)")]
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
        public ActionResult Upload()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Media Item(s)")]
        public ActionResult UploadFiles()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadInit, "Media");
                #endregion
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
                        #region AUDIT_WRITE
                        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadSuccess, "Media");
                        #endregion
                        // Returns message that successfully uploaded  
                        return Json("File(s) Uploaded Successfully!");
                    }
                    catch (Exception ex)
                    {
                        #region AUDIT_WRITE
                        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Media");
                        #endregion
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json("No files selected.");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult GetTags(string query)
        {
            try
            {

                return Json(_GetTags(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        private List<Autocomplete> _GetTags(string query)
        {
            try
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
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult AutoTag(string prefix)
        {
            try
            {

                DocuPathEntities entities = new DocuPathEntities();
                var tags = (from tag in entities.CONTENT_TAG
                            where tag.ContentTagText.Contains(prefix)
                            select new
                            {
                                label = tag.ContentTagText,
                                val = tag.ContentTagID
                            }).Take(15).ToList();

                return Json(tags);
            }
            catch (Exception)
            {

                return null;
            }
        }
        [HttpPost]
        public ActionResult catchTags(string tags)
        {
            try
            {
                List<tagCatcher> myTags = new List<tagCatcher>();

                var split = tags.Split('|');
                foreach (var text in split)
                {
                    if (text != "" && text != null)
                    {
                        tagCatcher temp = new tagCatcher();
                        temp.item = text.Substring(0, text.IndexOf(':'));
                        string tagline = text.Substring(text.IndexOf(':') + 1);
                        tagline = tagline.Replace('`', '\'');
                        var all = tagline.Split('~');
                        temp.tags = all.ToList();
                        myTags.Add(temp);
                    }
                }
                foreach (var media in myTags)
                {
                    foreach (var tag in media.tags)
                    {
                        MEDIA_TAG mTag = new MEDIA_TAG();
                        mTag.MediaID = Convert.ToInt32(media.item);
                        mTag.ContentTagID = db.CONTENT_TAG.Where(x => x.ContentTagText == tag).FirstOrDefault().ContentTagID;
                        db.MEDIA.Where(x => x.MediaID == mTag.MediaID).FirstOrDefault().MEDIA_TAG.Add(mTag);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception)
            {
                return null;                
            }
            return null;
        }
        struct tagCatcher
        {
            public string item;
            public List<string> tags;

        }
        #endregion
    }
}
