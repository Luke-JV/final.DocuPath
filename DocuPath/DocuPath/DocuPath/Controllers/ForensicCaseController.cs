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
    public class ForensicCaseController : Controller
    {
        
        DocuPathEntities db = new DocuPathEntities();
        // GET: ForensicCase
        public ActionResult Index()
        {
            //404 - redirect to /All
            return View();
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: ForensicCase/Create
        public ActionResult Create()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ForensicCase/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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
        // GET: ForensicCase/All
        //[AuthorizeByAccessArea(AccessArea = "Search Forensic Case")]
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

        // GET: ForensicCase/Details/5
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
        // GET: ForensicCase/Edit/5
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

        // POST: ForensicCase/Edit/5
        [HttpPost]
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
        // GET: ForensicCase/Delete/5
        public ActionResult Delete(int id)
        {
           
            return View();
        }

        // POST: ForensicCase/Delete/5
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

        #endregion






    }
}
