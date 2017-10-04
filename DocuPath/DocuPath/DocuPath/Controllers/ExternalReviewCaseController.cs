﻿using DocuPath.DataLayer;
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
    [Authorize]
    [HandleError]
    //[LogAction]
    public class ExternalReviewCaseController : Controller
    {
        string controllerName = "ExternalReviewCase";
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search External Review Case")]
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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "Add External Review Case")]
        public ActionResult Add()
        {
            string actionName = "Add";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "External Review Case");
                #endregion

                ExternalReviewCaseViewModel model = new ExternalReviewCaseViewModel();
                model.users = db.USER.ToList();

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add External Review Case")]
        public ActionResult Add(ExternalReviewCaseViewModel ERC)
        {
            string actionName = "Add";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "External Review Case");
                #endregion
                return View(ERC);
            }

            try
            {
                //405 test
                ERC.extCase.DateAdded = DateTime.Now;
                ERC.extCase.StatusID = db.STATUS.Where(x => x.StatusValue == "Active").FirstOrDefault().StatusID;
                try
                {
                    ERC.extCase.ExternalReviewCaseID = db.EXTERNAL_REVIEW_CASE.Max(x => x.ExternalReviewCaseID) + 1;
                }
                catch (Exception)
                {
                    ERC.extCase.ExternalReviewCaseID = 0;
                }

                db.EXTERNAL_REVIEW_CASE.Add(ERC.extCase);
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "External Review Case");
                #endregion
               
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:
        [AuthorizeByAccessArea(AccessArea = "Search External Review Case")]
        public ActionResult All()
        {
            string actionName = "All";
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "External Review Case");
                #endregion
                return View(db.EXTERNAL_REVIEW_CASE.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        [AuthorizeByAccessArea(AccessArea = "View External Review Case")]
        public ActionResult Details(int id)
        {
            string actionName = "Details";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "External Review Case");
                #endregion
                #region MODEL POPULATION
                EXTERNAL_REVIEW_CASE model = new EXTERNAL_REVIEW_CASE();
                model = db.EXTERNAL_REVIEW_CASE.Where(x => x.ExternalReviewCaseID == id).FirstOrDefault();
                #endregion

                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "External Review Case");
                #endregion

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Update/Edit External Review Case")]
        public ActionResult Edit(int id)
        {
            string actionName = "Edit";
            try
            {
                EXTERNAL_REVIEW_CASE model = new EXTERNAL_REVIEW_CASE();
                model = db.EXTERNAL_REVIEW_CASE.Where(x => x.ExternalReviewCaseID == id).FirstOrDefault();
                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "External Review Case");
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit External Review Case")]
        public ActionResult Edit(int id, EXTERNAL_REVIEW_CASE updatedERC)
        {
            string actionName = "Edit";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "External Review Case");
                #endregion
                return View(updatedERC);
            }

            try
            {
                #region DB UPDATE
                db.EXTERNAL_REVIEW_CASE.Attach(updatedERC);
                db.Entry(updatedERC).State = EntityState.Modified;
                db.SaveChanges();
                #endregion
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "External Review Case");
                #endregion
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:
        [AuthorizeByAccessArea(AccessArea = "Delete External Review Case")]
        public ActionResult Delete(int id)
        {
            string actionName = "Delete";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "External Review Case");
                #endregion
                //404 confirm!!
                db.EXTERNAL_REVIEW_CASE.Where(x => x.ExternalReviewCaseID == id).FirstOrDefault().StatusID = db.STATUS.Where(x => x.StatusValue == "Archived").FirstOrDefault().StatusID;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "External Review Case");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Delete External Review Case")]
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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add External Review Case")]
        public ActionResult UploadFiles()
        {
            string actionName = "UploadFiles";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadInit, "External Review Case");
                #endregion
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        string foldername = Request.Form.Get("ERCDR");
                        string rootpath = VERTEBRAE.ERC_REPORootPath;
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
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "External Review Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        #endregion
    }
}
