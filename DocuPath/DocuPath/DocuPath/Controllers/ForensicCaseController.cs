﻿using DocuPath.DataLayer;
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
        string controllerName = "ForensicCase";
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
        public ActionResult Add()
        {
            string actionName = "Add";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case");
                #endregion
                #region PREPARE MODEL
                AddForensicCaseViewModel model = new AddForensicCaseViewModel();
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
                model.forensicCase.SESSION = new SESSION();
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
                model.forensicCase.SessionID = 0;
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
                model.forensicCase.DateAdded = DateTime.Now;
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        public ActionResult Add(AddForensicCaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case");
                #endregion
                //return View(model);
            }

            try
            {
                FORENSIC_CASE inCase = new FORENSIC_CASE();

                try
                {
                    inCase.ForensicCaseID = db.FORENSIC_CASE.Max(x => x.ForensicCaseID) + 1;
                }
                catch (Exception)
                {
                    inCase.ForensicCaseID = 1;                    
                }

                inCase.StatusID = model.forensicCase.StatusID;
                inCase.SessionID = model.forensicCase.SessionID;
                inCase.AutopsyAreaID = model.forensicCase.AutopsyAreaID;                
                inCase.ForensicDRNumber = model.forensicCase.ForensicDRNumber;
                inCase.FCBriefDescription = model.forensicCase.FCBriefDescription;
                inCase.DateAdded = model.forensicCase.DateAdded;
                inCase.DHANoticeDeathID = model.forensicCase.DHANoticeDeathID;
                inCase.ActingOfficerNameSurname = model.forensicCase.ActingOfficerNameSurname;
                inCase.ActingOfficerContactNum = model.forensicCase.ActingOfficerContactNum;
                inCase.CauseOfDeathConclusion = model.forensicCase.CauseOfDeathConclusion;
                inCase.DateClosed = null;

                inCase.ABDOMEN_OBSERVATION.Add(model.abdObservation);

                inCase.ADDITIONAL_EVIDENCE = model.additionalEvidence;
                try
                {
                    inCase.CASE_COD_ESTIMATION.Add(model.primaryCODEst);
                }
                catch { }
                try
                {
                    inCase.CASE_COD_ESTIMATION.Add(model.secondaryCODEst);
                }
                catch { }
                try
                {
                    inCase.CASE_COD_ESTIMATION.Add(model.tertiaryCODEst);
                }
                catch { }
                try
                {
                    inCase.CASE_COD_ESTIMATION.Add(model.quaternaryCODEst);
                }
                catch { }
                inCase.CASE_STATISTICS.Add(model.stats);

                inCase.CHEST_OBSERVATION.Add(model.chestObservation);
                inCase.GENERAL_OBSERVATION.Add(model.genObservation);
                inCase.HEAD_NECK_OBSERVATION.Add(model.headNeckObservation);
                inCase.SPINE_OBSERVATION.Add(model.spineObservation);
                inCase.MEDIA = model.media;
                inCase.SERVICE_REQUEST = model.serviceRequests;
                //404inCase.SESSION.DateID = model.sessionSelector.FirstOrDefault().
                inCase.USER = VERTEBRAE.getCurrentUser();
                inCase.ADDITIONAL_EVIDENCE= new List<ADDITIONAL_EVIDENCE>();
                inCase.SERVICE_REQUEST= new List<SERVICE_REQUEST>();
                inCase.MEDIA= new List<MEDIA>();
                inCase.STATUS = db.STATUS.Where(x => x.StatusID == inCase.StatusID).FirstOrDefault();
                inCase.AUTOPSY_AREA = new AUTOPSY_AREA();
                
                //AE
                //AA
                //MEDIA
                //SR//Session
                db.FORENSIC_CASE.Add(inCase);
                db.SaveChanges();
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
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Core Data Section")]
        public ActionResult AddCoreData()
        {
            string actionName = "AddCoreData";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Core Data");
                #endregion
                #region PREPARE MODEL
                CoreDataViewModel model = new CoreDataViewModel();
                var sevenDaysAgo = DateTime.Today.Date.AddDays(-7);

                model.forensicCase = new FORENSIC_CASE();
                model.autopsyAreas = db.AUTOPSY_AREA.ToList();
                model.forensicCase.SESSION = new SESSION();
                model.forensicCase.SessionID = 0;
                model.sessionSelector = new List<sessionKVP>();
                foreach (var item in db.SESSION.Where(x => x.DateID > sevenDaysAgo))
                {
                    sessionKVP newSesh = new sessionKVP();
                    newSesh.sessionID = item.SessionID;
                    newSesh.sessionDesc = item.SLOT.Description + " at " + item.SLOT.StartTime.ToString("HH:mm") + " on " + item.DateID.ToString("dd MMM yyyy");
                    model.sessionSelector.Add(newSesh);
                }
                
                model.forensicCase.DateAdded = DateTime.Now;
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Core Data");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Core Data Section")]
        public ActionResult AddCoreData(CoreDataViewModel model)
        {
            string actionName = "AddCoreData";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Core Data");
                #endregion
                //return View(model);
            }

            try
            {
                //TODO: DB Logic
                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Core Data");
                #endregion
                return RedirectToAction("AddObservations");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Core Data");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Observations Section")]
        public ActionResult AddObservations()
        {
            string actionName = "AddObservations";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Observations");
                #endregion
                #region PREPARE MODEL
                ObservationsViewModel model = new ObservationsViewModel();

                model.genObservation = new GENERAL_OBSERVATION();
                model.abdObservation = new ABDOMEN_OBSERVATION();
                model.chestObservation = new CHEST_OBSERVATION();
                model.headNeckObservation = new HEAD_NECK_OBSERVATION();
                model.spineObservation = new SPINE_OBSERVATION();
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Observations");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Observations Section")]
        public ActionResult AddObservations(ObservationsViewModel model)
        {
            string actionName = "AddObservations";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Observations");
                #endregion
                //return View(model);
            }

            try
            {
                //TODO: DB Logic
                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Observations");
                #endregion
                return RedirectToAction("AddServiceRequests");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Observations");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
    //>>>>>>>>>>>>>>>>>>>>>>        
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Service Requests Section")]
        public ActionResult AddServiceRequests()
        {
            string actionName = "AddServiceRequests";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Service Requests");
                #endregion
                #region PREPARE MODEL
                FCServiceRequestViewModel model = new FCServiceRequestViewModel();
                List<SERVICE_REQUEST> srList = new List<SERVICE_REQUEST>();
                List<SPECIMEN> specimenList = new List<SPECIMEN>();

                for (int i = 0; i < VERTEBRAE.maxSRPerFCAddUpdate; i++)
                {
                    SERVICE_REQUEST newSR = new SERVICE_REQUEST();
                    srList.Add(newSR);
                    SPECIMEN newSpecimen = new SPECIMEN();
                    specimenList.Add(newSpecimen);
                }


                model.serviceRequests = srList;
                model.specimens = specimenList;
                model.activeSP = db.SERVICE_PROVIDER.Where(sp => sp.IsDeactivated == false).ToList();
                model.requestTypes = db.REQUEST_TYPE.ToList();

                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Service Requests");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Service Requests Section")]
        public ActionResult AddServiceRequests(FCServiceRequestViewModel model)
        {
            string actionName = "AddServiceRequests";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Service Requests");
                #endregion
                //return View(model);
            }

            try
            {
                //TODO: DB Logic
                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Service Requests");
                #endregion
                return RedirectToAction("ForensicCaseServiceRequests");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Observations");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
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
