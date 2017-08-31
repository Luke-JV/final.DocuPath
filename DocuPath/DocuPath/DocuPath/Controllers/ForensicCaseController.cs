using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class ForensicCaseController : Controller
    {
        
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Forensic Case")]
        public ActionResult Index()
        {
            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//

        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        public ActionResult Add()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        public ActionResult Add(FormCollection collection)
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
        
        [AuthorizeByAccessArea(AccessArea = "Search Forensic Case")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();

                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion

                return View(db.FORENSIC_CASE.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View Forensic Case - All Sections")]
        public ActionResult Details(int id)
        {
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion

            #region PREPARE VIEWMODEL
            ForensicCaseViewModel model = new ForensicCaseViewModel();
            FORENSIC_CASE modelCase = new FORENSIC_CASE();
            CASE_STATISTICS modelStats = new CASE_STATISTICS();
            modelCase = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == id).FirstOrDefault();
            modelStats = modelCase.CASE_STATISTICS.FirstOrDefault();

            model.forensicCase = modelCase;
            model.genObservation = modelCase.GENERAL_OBSERVATION.FirstOrDefault();
            model.headNeckObservation = modelCase.HEAD_NECK_OBSERVATION.FirstOrDefault();
            model.abdObservation = modelCase.ABDOMEN_OBSERVATION.FirstOrDefault();
            model.spineObservation = modelCase.SPINE_OBSERVATION.FirstOrDefault();
            model.chestObservation = modelCase.CHEST_OBSERVATION.FirstOrDefault();
            model.serviceRequests = modelCase.SERVICE_REQUEST.ToList();
            model.media = modelCase.MEDIA.ToList();
            model.additionalEvidence = modelCase.ADDITIONAL_EVIDENCE.ToList();
            model.stats = modelCase.CASE_STATISTICS.FirstOrDefault();
            model.caseCODEstimations = modelCase.CASE_COD_ESTIMATION.ToList();
            model.sampleInvestigations = modelStats.STATS_SAMPLES_INVESTIGATION.ToList();
            model.medTreatments = modelStats.STATS_TREATMENTS.ToList();
            model.injuryScenes = modelStats.STATS_INJURY_SCENE.ToList();
            model.externalCause = modelStats.STATS_EXTERNAL_CAUSE.ToList();
            model.sampleInvestigations = modelStats.STATS_SAMPLES_INVESTIGATION.ToList();
            model.specialCategory = modelStats.STATS_SPECIAL_CATEGORY.ToList();
            model.provinces = modelStats.STATS_PROVINCE_EVENT.Where(x => x.ForensicCaseID == modelCase.ForensicCaseID).ToList();
            model.stations = modelStats.STATS_POLICE_STATION.Where(x => x.ForensicCaseID == modelCase.ForensicCaseID).ToList();
            #endregion

            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
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
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        public ActionResult Edit(int id, FORENSIC_CASE updatedFC)
        {
            try
            {
                #region DB UPDATE MASSIVE 404!
                db.FORENSIC_CASE.Attach(updatedFC);
                db.Entry(updatedFC).State = EntityState.Modified;
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
        [AuthorizeByAccessArea(AccessArea = "Delete Forensic Case")]
        public ActionResult Delete(int id)
        {
           
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Delete Forensic Case")]
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

        #endregion
    }
}
