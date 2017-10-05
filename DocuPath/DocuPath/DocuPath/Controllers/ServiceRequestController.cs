using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DocuPath.Models.DPViewModels;
using System.IO;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    //[LogAction]
    public class ServiceRequestController : Controller
    {
        string controllerName = "ServiceRequest";
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Service Request")]
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
        [AuthorizeByAccessArea(AccessArea = "Add Service Request")]
        public ActionResult Add()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Service Request");
                #endregion

                List<SelectListItem> selectRequestType = new List<SelectListItem>();
                List<SelectListItem> selectServiceProvider = new List<SelectListItem>();

                selectRequestType.Add(new SelectListItem { Value = "0", Text = "Select a Request Type..." });
                foreach (var item in db.REQUEST_TYPE)
                {
                    selectRequestType.Add(new SelectListItem { Value = item.RequestTypeID.ToString(), Text = item.RequestTypeValue });
                }
                ViewBag.RequestTypes = selectRequestType;

                selectServiceProvider.Add(new SelectListItem { Value = "0", Text = "Select a Service Provider..." });
                foreach (var item in db.SERVICE_PROVIDER)
                {
                    selectServiceProvider.Add(new SelectListItem { Value = item.ServiceProviderID.ToString(), Text = item.CompanyName/* + " (Representative: " + item.RepFirstName + " " + item.RepLastName + ")"*/ });
                }
                ViewBag.ServiceProviders = selectServiceProvider;
                
                return View();
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Service Request");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Service Request")]
        public ActionResult Add(SERVICE_REQUEST SR)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Service Request");
                #endregion
                return View(SR);
            }

            try
            {
                db.SERVICE_REQUEST.Add(SR);
                db.SaveChanges();
                // TODO: Add insert logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Service Request");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Service Request");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region READS:
        [AuthorizeByAccessArea(AccessArea = "Search Service Request")]
        public ActionResult All()
        {
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Service Request");
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchSuccess, "Service Request");
                #endregion
                return View(db.SERVICE_REQUEST.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Service Request");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View Service Request")]
        public ActionResult Details(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "Service Request");
                #endregion
                #region MODEL POPULATION
                SERVICE_REQUEST model = new SERVICE_REQUEST();
                model = db.SERVICE_REQUEST.Where(x => x.ServiceRequestID == id).FirstOrDefault();
                model.SPECIMEN = db.SPECIMEN.Where(x => x.ServiceRequestID == model.ServiceRequestID).ToList();
                model.FORENSIC_CASE = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.ForensicCaseID).FirstOrDefault();
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "Service Request");
                #endregion
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "Service Request");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Link External Report To Service Request")]
        public ActionResult LinkExternalReport(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Service Request");
                #endregion

                EXTERNAL_REPORT report = new EXTERNAL_REPORT();

                report.DateCaptured = DateTime.Now;
                report.DateReceived = DateTime.Now;
                report.ExternalReportID = db.EXTERNAL_REPORT.Max(er => er.ExternalReportID) + 1;
                report.ExternalReportLocation = "";

                List<ReportKVP> existingReports = new List<ReportKVP>();
                foreach (var existingReport in db.EXTERNAL_REPORT.Take(30))
                {
                    ReportKVP existingReportEntry = new ReportKVP();
                    existingReportEntry.reportID = existingReport.ExternalReportID;
                    existingReportEntry.reportPhrase = "\"" + Path.GetFileName(existingReport.ExternalReportLocation) + "\" added on " + existingReport.DateCaptured.ToString("dd MMM yyyy") + " at "+ existingReport.DateCaptured.ToString("HH:mm:ss");

                    existingReports.Add(existingReportEntry);
                }
                
                LinkERToSRViewModel model = new LinkERToSRViewModel();
                model.targetSR = db.SERVICE_REQUEST.Where(sr => sr.ServiceRequestID == id).FirstOrDefault();
                model.targetER = report;
                model.existingERList = existingReports;

                //List<SelectListItem> selectExistingReports = new List<SelectListItem>();

                //selectExistingReports.Add(new SelectListItem { Value = "0", Text = "Select an Existing External Report..." });
                //selectExistingReports.Add(new SelectListItem { Value = "1", Text = "'NHLSreport.PDF' added by 'John Smith' on 29 Mar 2017 09:39:14" });
                //selectExistingReports.Add(new SelectListItem { Value = "2", Text = "'AmpathReport.PDF' added by 'Jane Smith' on 2 Apr 2017 08:35:54" });
                //selectExistingReports.Add(new SelectListItem { Value = "3", Text = "'LancetReport.PDF' added by 'Jeff Peters' on 10 Apr 2017 10:29:19" });
                //selectExistingReports.Add(new SelectListItem { Value = "4", Text = "'Vermaak&VennoteReport.PDF' added by 'Peter Jefferson' on 11 Apr 2017 12:32:11" });
                //selectExistingReports.Add(new SelectListItem { Value = "5", Text = "'AmpathReport.PDF' added by 'Sam Brown' on 15 Apr 2017 06:52:11" });
                //selectExistingReports.Add(new SelectListItem { Value = "6", Text = "'NHLSReport.PDF' added by 'Bronwyn Samuel' on 18 Apr 2017 09:42:24" });
                //selectExistingReports.Add(new SelectListItem { Value = "7", Text = "'Vermaak&VennoteReport.PDF' added by 'Felicia Bye' on 21 Apr 2017 14:21:16" });

                //ViewBag.ExistingReports = selectExistingReports;
                //ViewBag.DateReceivedDefault = "20 Apr 2017 09:25:36";
                //ViewBag.DateCapturedDefault = System.DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");


                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Service Request");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Link External Report To Service Request")]
        public ActionResult LinkExternalReport(LinkERToSRViewModel model)
        {
            if (model.targetER.ExternalReportID > 0)
            {
                SERVICE_REQUEST req = db.SERVICE_REQUEST.Where(x => x.ServiceRequestID == model.targetSR.ServiceRequestID).FirstOrDefault();
                req.SPECIMEN.FirstOrDefault().EXTERNAL_REPORT = db.EXTERNAL_REPORT.Where(x=>x.ExternalReportID == model.targetER.ExternalReportID).FirstOrDefault();
                db.SERVICE_REQUEST.Attach(req);
                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            
            return RedirectToAction("All");
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Link External Report To Service Request")]
        public ActionResult Edit(int id, SERVICE_REQUEST updatedSR)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Service Request");
                #endregion
                return View(updatedSR);
            }

            try
            {
                #region DB UPDATE
                db.SERVICE_REQUEST.Attach(updatedSR);
                db.Entry(updatedSR).State = EntityState.Modified;
                db.SaveChanges();
                #endregion
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "Service Request");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Service Request");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region DELETES:
        [AuthorizeByAccessArea(AccessArea = "Cancel Service Request")]
        public ActionResult Delete(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "Service Request");
                #endregion
                //404 CONFIRM
                db.SERVICE_REQUEST.Where(x => x.ServiceRequestID == id).FirstOrDefault().IsCancelled = true;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "Service Request");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Service Request");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Cancel Service Request")]
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
        [AuthorizeByAccessArea(AccessArea = "Link External Report To Service Request")]
        public ActionResult UploadFiles()
        {
            string actionName = "UploadFiles";
            
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadInit, "Service Request - External Report");
                #endregion
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        string foldername = Request.Form.Get("SRID");
                        DateTime received = Convert.ToDateTime(Request.Form.Get("received"));
                        string rootpath = VERTEBRAE.EXT_REPORT_REPORootPath;
                        for (int i = 0; i < files.Count; i++)
                        {
                        EXTERNAL_REPORT rep = new EXTERNAL_REPORT();
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
                            rep.ExternalReportLocation = fname;
                            rep.ExternalReportID = db.EXTERNAL_REPORT.Max(x => x.ExternalReportID) + 1;
                            rep.DateCaptured = DateTime.Now;
                            rep.DateReceived = received;
                            db.EXTERNAL_REPORT.Add(rep);
                            db.SaveChanges();

                            int srid = Convert.ToInt32(foldername);
                            SERVICE_REQUEST req = db.SERVICE_REQUEST.Where(x=>x.ServiceRequestID == srid).FirstOrDefault();
                            req.SPECIMEN.FirstOrDefault().ExternalReportID = rep.ExternalReportID;
                            db.SERVICE_REQUEST.Attach(req);
                            db.Entry(req).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        //add logic here
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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Service Request - External Report");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
    }
}
