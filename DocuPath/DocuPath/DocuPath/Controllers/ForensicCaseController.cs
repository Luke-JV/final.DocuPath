using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
   // [LogAction]
    public class ForensicCaseController : Controller
    {
        
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search Forensic Case")]
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
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        public ActionResult Add(AddForensicCaseViewModel model)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case");
                #endregion
                #region PREPARE MODEL
                var sevenDaysAgo = DateTime.Today.Date.AddDays(-7);

                model.forensicCase = new FORENSIC_CASE();
                model.genObservation = new GENERAL_OBSERVATION();
                model.abdObservation = new ABDOMEN_OBSERVATION();
                model.chestObservation = new CHEST_OBSERVATION();
                model.headNeckObservation = new HEAD_NECK_OBSERVATION();
                model.spineObservation = new SPINE_OBSERVATION();
                model.additionalEvidence = new List<ADDITIONAL_EVIDENCE>();
                model.serviceRequests = new List<SERVICE_REQUEST>();
                model.media = new List<MEDIA>();
                model.stats = new CASE_STATISTICS();
                model.provinces = db.PROVINCE.ToList();
                model.events = db.EVENT.ToList();
                model.sampleInvestigations = db.SAMPLE_INVESTIGATION.ToList();
                model.medTreatments = db.MEDICAL_TREATMENTS.ToList();
                model.injuryScenes = db.SCENE_OF_INJURY.ToList();
                model.externalCause = db.EXTERNAL_CAUSE.ToList();
                model.specialCategory = db.SPECIAL_CATEGORY.ToList();
                model.serviceProvider = db.SERVICE_PROVIDER.ToList();
                model.autopsyAreas = db.AUTOPSY_AREA.ToList();
                model.autopsyTypes = db.AUTOPSY_TYPE.ToList();
                model.races = db.INDIVIDUAL_RACE.ToList();
                model.genders = db.INDIVIDUAL_GENDER.ToList();
                model.hospitalsClinics = db.HOSPITAL_CLINIC.ToList();
                model.primaryCauses = db.PRIMARY_CAUSE_DEATH.ToList();
                model.apparentManners = db.APPARENT_MANNER_DEATH.ToList();

                model.sessionSelector = new List<sessionKVP>();
                foreach (var item in db.SESSION.Where(x => x.DateID > sevenDaysAgo))
                {
                    sessionKVP newSesh = new sessionKVP();
                    newSesh.sessionID = item.SessionID;
                    newSesh.sessionDesc = item.SLOT.Description + " on " + item.DateID.ToString("dd MMM yyyy");
                    model.sessionSelector.Add(newSesh);
                }

                model.selectedSamplesInvestigations = new List<multiselectKVP>();
                foreach (var item in model.sampleInvestigations)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.SampleInvestigationDescription;
                    newKVP.valueID = item.SampleInvestigationID;
                    newKVP.isSelected = false;
                    model.selectedSamplesInvestigations.Add(newKVP);
                }
                
                model.selectedMedicalTreatments = new List<multiselectKVP>();
                foreach (var item in model.medTreatments)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.MedicalTreatmentDescription;
                    newKVP.valueID = item.MedicalTreatmentID;
                    newKVP.isSelected = false;
                    model.selectedMedicalTreatments.Add(newKVP);
                }

                model.selectedInjuryScenes = new List<multiselectKVP>();
                foreach (var item in model.injuryScenes)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.InjurySceneDescription;
                    newKVP.valueID = item.InjurySceneID;
                    newKVP.isSelected = false;
                    model.selectedInjuryScenes.Add(newKVP);
                }

                model.selectedExternalCauses = new List<multiselectKVP>();
                foreach (var item in model.externalCause)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.ExternalCauseDescription;
                    newKVP.valueID = item.ExternalCauseID;
                    newKVP.isSelected = false;
                    model.selectedExternalCauses.Add(newKVP);
                }

                model.selectedSpecialCategories = new List<multiselectKVP>();
                foreach (var item in model.specialCategory)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.SpecialCategoryDescription;
                    newKVP.valueID = item.SpecialCategoryID;
                    newKVP.isSelected = false;
                    model.selectedSpecialCategories.Add(newKVP);
                }

                //model.provinceEvents = new List<SPE_KVP>();
                //foreach (var item in model.events)
                //{
                //    SPE_KVP newSPE = new SPE_KVP();
                //    newSPE.eventID = item.EventID;                   
                //    model.provinceEvents.Add(newSPE);
                //}
                //model.stationRoles = new List<stationRolesKVP>();

                model.primaryCODEst = new CASE_COD_ESTIMATION();
                model.primaryCODEst.ProminenceID = 1;
                model.secondaryCODEst = new CASE_COD_ESTIMATION();
                model.secondaryCODEst.ProminenceID = 2;
                model.tertiaryCODEst = new CASE_COD_ESTIMATION();
                model.tertiaryCODEst.ProminenceID = 3;
                model.quaternaryCODEst = new CASE_COD_ESTIMATION();
                model.quaternaryCODEst.ProminenceID = 4;

                model.otherSamplesInvestigationsDescription = "";
                model.otherRaceDescription = "";
                model.otherTreatmentFacilityDescription = "";
                model.otherMedicalTreatmentsDescription = "";
                model.otherPrimaryCauseDeathDescription = "";
                model.otherApparentMannerDeathDescription = "";
                model.otherInjurySceneDescription = "";
                model.otherExternalCauseDescription = "";
                model.otherSpecialCategoryDescription = "";

                model.DeathProvinceId = 1;
                model.otherDeathProvinceDesc = "";
                model.ProcessingProvinceId = 1;
                model.otherProcessingProvinceDesc = "";
                model.OccurrenceProvinceId = 1;
                model.otherOccurrenceProvinceDesc = "";
                model.TreatmentProvinceId = 1;
                model.otherTreatmentProvinceDesc = "";
                model.ReportProvinceId = 1;
                model.otherReportProvinceDesc = "";

                model.JurisdictionStationID = 0;
                model.JurisdictionStationName = "";
                model.ProcessingStationID = 0;
                model.ProcessingStationName = "";
                model.InvestigationStationID = 0;
                model.InvestigationStationName = "";

                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, "ForensicCase", "Add"));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        public ActionResult Add(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case");
                #endregion
                return View(collection);
            }

            try
            {
                // TODO: Add insert logic here

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case");
                #endregion

                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, "ForensicCase", "Add"));
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
                

                //throw new Exception();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Forensic Case");
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchSuccess, "Forensic Case");
                #endregion
                return View(db.FORENSIC_CASE.ToList());

            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Forensic Case");
                #endregion
                // TODO 404 - Propagate error handling logic
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error",new HandleErrorInfo(x, "ForensicCase", "All"));
            }
        }
        
        [AuthorizeByAccessArea(AccessArea = "View Forensic Case - All Sections")]
        public ActionResult Details(int id)
        {
            try
            {

                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "Forensic Case");
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
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "Forensic Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, "ForensicCase", "Details"));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        public ActionResult Edit(int id)
        {
            try
            {


                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Forensic Case");
                #endregion
                return View();
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Forensic Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Migrate Forensic Case")]
        public ActionResult Migrate(int id)
        {
            try
            {
                List<SelectListItem> selectUsers = new List<SelectListItem>();

                selectUsers.Add(new SelectListItem { Value = "0", Text = "Select a User..." });
                foreach (var item in db.USER)
                {
                    selectUsers.Add(new SelectListItem { Value = item.UserID.ToString(), Text = item.DisplayInitials });
                }
                ViewBag.Users = selectUsers;

                ViewBag.CurrentAuthor = "John Doe";
                ViewBag.AuthorSince = "5 Jan 2017 00:00:00";
                ViewBag.MigrationMessage = "Still waiting for XYZ from ABC, contact John Smith.";

                return View();


                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Forensic Case");
                #endregion
                return View();
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Forensic Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        public ActionResult Edit(int id, FORENSIC_CASE updatedFC)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Forensic Case");
                #endregion
                return View(updatedFC);
            }

            try
            {
                #region DB UPDATE MASSIVE 404!
                db.FORENSIC_CASE.Attach(updatedFC);
                db.Entry(updatedFC).State = EntityState.Modified;
                db.SaveChanges();
                #endregion
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "Forensic Case");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Forensic Case");
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
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "Forensic Case");
                #endregion
                //404 CONFIRM
                db.FORENSIC_CASE.Where(x => x.ForensicCaseID == id).FirstOrDefault().StatusID = db.STATUS.Where(x => x.StatusValue == "Archived").FirstOrDefault().StatusID;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "Forensic Case");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Forensic Case");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Delete Forensic Case")]
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
        public ActionResult GetSP(string query)
        {
            try
            {

                return Json(_GetSP(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        private List<Autocomplete> _GetSP(string query)
        {
            List<Autocomplete> serviceProviders = new List<Autocomplete>();
            try
            {
                var spResults = (from sp in db.SERVICE_PROVIDER
                                  where sp.CompanyName.Contains(query)
                                  orderby sp.CompanyName
                                  select sp).ToList();

                foreach (var result in spResults)
                {
                    Autocomplete provider = new Autocomplete();
                    provider.Name = result.CompanyName;
                    provider.Id = result.ServiceProviderID;
                    serviceProviders.Add(provider);
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
            return serviceProviders;
        }
        #endregion
    }
}
