using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using IronPdf;
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
                model.forensicCase.SESSION = new SESSION();
                model.autopsyAreas = db.AUTOPSY_AREA.ToList();
                model.stats = new CASE_STATISTICS();
                model.provinces = db.PROVINCE.ToList();
                model.events = db.EVENT.ToList();
                model.sampleInvestigations = db.SAMPLE_INVESTIGATION.ToList();
                model.medTreatments = db.MEDICAL_TREATMENTS.ToList();
                model.injuryScenes = db.SCENE_OF_INJURY.ToList();
                model.externalCause = db.EXTERNAL_CAUSE.ToList();
                model.specialCategory = db.SPECIAL_CATEGORY.ToList();
                model.serviceProvider = db.SERVICE_PROVIDER.ToList();
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
                inCase.ADDITIONAL_EVIDENCE = new List<ADDITIONAL_EVIDENCE>();
                inCase.SERVICE_REQUEST = new List<SERVICE_REQUEST>();
                inCase.MEDIA = new List<MEDIA>();
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
                DateTime sevenDaysAgo = DateTime.Today.AddDays(-7);
                //DateTime today = DateTime.Today.AddDays(-1).AddHours(22).AddMinutes(59).AddMilliseconds(99999);
                DateTime today = DateTime.Today.AddDays(1).AddMilliseconds(-1);

                model.forensicCase = new FORENSIC_CASE();
                model.autopsyAreas = db.AUTOPSY_AREA.ToList();
                model.forensicCase.SESSION = new SESSION();
                model.forensicCase.SessionID = 0;
                model.sessionSelector = new List<sessionKVP>();
                foreach (var item in db.SESSION.Where(x => x.DateID <= today && x.DateID >= sevenDaysAgo).OrderByDescending(x => x.DateID))
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
        [ValidateInput(false)]
        public ActionResult AddCoreData(CoreDataViewModel model)
        {
            string actionName = "AddCoreData";
            //if (!ModelState.IsValid) //todo: rebrain this
            //{
            //    #region AUDIT_WRITE
            //    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Core Data");
            //    #endregion
            //    //return View(model);
            //}

            try
            {
                FORENSIC_CASE inCase = default(FORENSIC_CASE);
                inCase = model.forensicCase;


                try
                {
                    inCase.ForensicCaseID = db.FORENSIC_CASE.Max(x => x.ForensicCaseID) + 1;
                }
                catch (Exception)
                {
                    inCase.ForensicCaseID = 1;
                }

                inCase.STATUS = db.STATUS.Where(x => x.StatusID == db.STATUS.Where(y => y.StatusValue == "Active").FirstOrDefault().StatusID).FirstOrDefault();
                inCase.UserID = VERTEBRAE.getCurrentUser().UserID;
                inCase.assignFlagsAndKey(inCase.ForensicCaseID);

                //OBSERVATIONS INIT
                inCase.GENERAL_OBSERVATION.Add(new GENERAL_OBSERVATION(db.GENERAL_OBSERVATION.Max(x => x.ObsGeneralID) + 1, inCase.ForensicCaseID));
                inCase.ABDOMEN_OBSERVATION.Add(new ABDOMEN_OBSERVATION(db.ABDOMEN_OBSERVATION.Max(x => x.ObsAbdomenID) + 1, inCase.ForensicCaseID));
                inCase.CHEST_OBSERVATION.Add(new CHEST_OBSERVATION(db.CHEST_OBSERVATION.Max(x => x.ObsChestID) + 1, inCase.ForensicCaseID));
                inCase.HEAD_NECK_OBSERVATION.Add(new HEAD_NECK_OBSERVATION(db.HEAD_NECK_OBSERVATION.Max(x => x.ObsHeadNeckID) + 1, inCase.ForensicCaseID));
                inCase.SPINE_OBSERVATION.Add(new SPINE_OBSERVATION(db.SPINE_OBSERVATION.Max(x => x.ObsSpineID) + 1, inCase.ForensicCaseID));




                //todo: validate DR number for duplicate
                db.FORENSIC_CASE.Add(inCase);
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Core Data");
                #endregion
                return RedirectToAction("AddObservations", new { id = inCase.ForensicCaseID });
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
        public ActionResult AddObservations(int id)
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
                var gen = db.GENERAL_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();
                model.genObservation.ForensicCaseID = gen.ForensicCaseID;
                model.genObservation.ObsGeneralID = gen.ObsGeneralID;

                model.abdObservation = new ABDOMEN_OBSERVATION();
                var abd = db.ABDOMEN_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();
                model.abdObservation.ForensicCaseID = abd.ForensicCaseID;
                model.abdObservation.ObsAbdomenID = abd.ObsAbdomenID;

                model.chestObservation = new CHEST_OBSERVATION();
                var chest = db.CHEST_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();
                model.chestObservation.ForensicCaseID = chest.ForensicCaseID;
                model.chestObservation.ObsChestID = chest.ObsChestID;

                model.headNeckObservation = new HEAD_NECK_OBSERVATION();
                var headneck = db.HEAD_NECK_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();
                model.headNeckObservation.ForensicCaseID = headneck.ForensicCaseID;
                model.headNeckObservation.ObsHeadNeckID = headneck.ObsHeadNeckID;

                model.spineObservation = new SPINE_OBSERVATION();
                var spine = db.SPINE_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();
                model.spineObservation.ForensicCaseID = spine.ForensicCaseID;
                model.spineObservation.ObsSpineID = spine.ObsSpineID;

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
        [ValidateInput(false)]
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
                FORENSIC_CASE upCase = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.abdObservation.ForensicCaseID).FirstOrDefault();

                upCase.GENERAL_OBSERVATION.Clear();
                upCase.GENERAL_OBSERVATION.Add(model.genObservation);
                upCase.GENERAL_OBSERVATION.FirstOrDefault().setFlags();

                upCase.ABDOMEN_OBSERVATION.Clear();
                upCase.ABDOMEN_OBSERVATION.Add(model.abdObservation);
                upCase.ABDOMEN_OBSERVATION.FirstOrDefault().setFlags();

                upCase.HEAD_NECK_OBSERVATION.Clear();
                upCase.HEAD_NECK_OBSERVATION.Add(model.headNeckObservation);
                upCase.HEAD_NECK_OBSERVATION.FirstOrDefault().setFlags();

                upCase.SPINE_OBSERVATION.Clear();
                upCase.SPINE_OBSERVATION.Add(model.spineObservation);
                upCase.SPINE_OBSERVATION.FirstOrDefault().setFlags();

                upCase.CHEST_OBSERVATION.Clear();
                upCase.CHEST_OBSERVATION.Add(model.chestObservation);
                upCase.CHEST_OBSERVATION.FirstOrDefault().setFlags();

                db.FORENSIC_CASE.Attach(upCase);
                db.Entry(upCase).State = EntityState.Modified;
                db.SaveChanges();

                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Observations");
                #endregion
                //return RedirectToAction("AddServiceRequests");
                return RedirectToAction("ProvideSpecimens", new { id = upCase.ForensicCaseID });
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
        public ActionResult ProvideSpecimens(int id)
        {
            string actionName = "ProvideSpecimens";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Service Requests");
                #endregion
                #region PREPARE MODEL
                
                SpecimenViewModel model = new SpecimenViewModel();
                model.fcID = id;
                model.specimens = new List<specimen>();
                model.specimens.Add(new specimen());
                

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
        public ActionResult ProvideSpecimens(SpecimenViewModel model)
        {
            string actionName = "ProvideSpecimens";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Service Requests");
                #endregion
                //return View(model);
            }

            try
            {
                FCServiceRequestViewModel newModel = new FCServiceRequestViewModel();
                newModel.serviceRequests = new List<SERVICE_REQUEST>();
                newModel.specimens = new List<SPECIMEN>();
                newModel.activeSP = db.SERVICE_PROVIDER.Where(x => x.IsDeactivated == false).ToList();
                newModel.requestTypes = db.REQUEST_TYPE.ToList();
                foreach (var item in model.specimens.Where(x => x.sealnumber != null && x.description != null))
                {
                SERVICE_REQUEST req = new SERVICE_REQUEST();
                SPECIMEN spec = new SPECIMEN();
                    req.IsCancelled = false;
                    req.RequestNote = "";
                    req.ServiceProviderID = 0;
                    req.ForensicCaseID = model.fcID;
                    req.DateAdded = DateTime.Now;
                    spec.SpecimenNature = item.description;
                    spec.SpecimenSerialNumber = item.sealnumber;
                    spec.InvestigationRequired = "";
                    spec.DisposalMechanism = "";                    
                    req.SPECIMEN.Add(spec);
                    newModel.serviceRequests.Add(req);
                    newModel.specimens.Add(spec);
                }
                TempData["model"] = newModel;
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Service Requests");
                #endregion
                return RedirectToAction("CaptureServiceRequestDetails",new{id=model.fcID});
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
        //>>>>>>>>>>>>>>>>>>>>>>        
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Service Requests Section")]
        [HttpGet]
        public ActionResult CaptureServiceRequestDetails(int id)
        {
            
            string actionName = "CaptureServiceRequestDetails";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Service Requests");
                #endregion
                #region PREPARE MODEL
                FCServiceRequestViewModel model = new FCServiceRequestViewModel();
                model = (FCServiceRequestViewModel)TempData["model"];
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
        public ActionResult CaptureServiceRequestDetails(FCServiceRequestViewModel model)
        {
            string actionName = "CaptureServiceRequestDetails";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Service Requests");
                #endregion
                //return View(model);
            }

            try
            {
                int SRID = 0;
                int SPID = 0;
                try
                {
                     SRID = db.SERVICE_REQUEST.Max(x => x.ServiceRequestID);
                     SPID = db.SPECIMEN.Max(x => x.SpecimenID);
                }
                catch (Exception)
                {
                    SRID = 1;
                     SPID = 1;
                }
                for (int i = 0; i < model.serviceRequests.Count; i++)
                {
                    SERVICE_REQUEST req = new SERVICE_REQUEST();
                    SPECIMEN spec = new SPECIMEN();
                    SRID++;
                    SPID++;
                    req = model.serviceRequests[i];
                    req.ServiceRequestID = SRID;                    
                    spec = model.specimens[i];
                    spec.SpecimenID = SPID;
                    spec.ServiceRequestID = SRID;
                    //req.SPECIMEN.Add(spec);
                    //spec.SERVICE_REQUEST = req;
                    db.SERVICE_REQUEST.Add(req);
                    db.SaveChanges();
                    req.SPECIMEN.Add(spec);
                    db.SERVICE_REQUEST.Attach(req);
                    db.Entry(req).State = EntityState.Modified;
                    db.SaveChanges();
                }
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Service Requests");
                #endregion
                return RedirectToAction("SelectMediaItems",new { id=model.serviceRequests.FirstOrDefault().ForensicCaseID});
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
        //>>>>>>>>>>>>>>>>>>>>>>        
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Media Items Section")]
        public ActionResult SelectMediaItems(int id)
        {
            string actionName = "SelectMediaItems";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Media Items");
                #endregion
                ViewBag.FCID = id;
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Media Items");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Media Items Section")]
        public ActionResult UploadMediaItems(int id)
        {

            string actionName = "UploadMediaItems";
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
                            M.ForensicCaseID = id;
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
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Forensic Case - Media Items");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>        
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Media Items Section")]
        public ActionResult CaptureMediaItemDetails(int id)
        {
            string actionName = "CaptureMediaItemDetails";
            try
            {
                if (id < 0)
                {
                    Session["UPDATE"] = id;
                    id = Math.Abs(id);
                    ViewBag.Instruction = "UPDATE";
                }
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Media Items");
                #endregion
                #region PREPARE MODEL
                CaseMediaViewModel model = new CaseMediaViewModel();

                int uID = VERTEBRAE.getCurrentUser().UserID;
                model.mediaList = db.MEDIA.Where(x => x.UserID == uID && x.MediaCaption.ToUpper() == "PENDING" && x.ForensicCaseID == id).ToList();
                if (model.mediaList.Count < 1)
                {
                    //Response.
                    return RedirectToAction("All");
                }
                foreach (var item in model.mediaList)
                {
                    item.MediaCaption = "";
                    item.MediaDescription = "";
                    //item.ForensicCaseID = 0;
                    item.ForensicCaseID = db.FORENSIC_CASE.Where(fc => fc.UserID == uID).Max(fc => fc.ForensicCaseID);
                    item.MediaPurposeID = db.MEDIA_PURPOSE.Where(p => p.MediaPurposeValue == "Case Related").FirstOrDefault().MediaPurposeID;
                }
                //var week = DateTime.Today.Date.AddDays(-7);
                //var cases = from fc in db.FORENSIC_CASE.Where(x => x.UserID == id)
                //            where fc.DateAdded >= week
                //            select fc;
                //model.fcList = cases.ToList();
                //model.purposeList = db.MEDIA_PURPOSE.ToList();
                //if (model.fcList.Count < 1)
                //{
                //    model.purposeList.Remove(model.purposeList.Where(x => x.MediaPurposeValue == "Case Related").FirstOrDefault());
                //} 
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Media");
                #endregion

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Media Items");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Media Items Section")]
        public ActionResult CaptureMediaItemDetails(CaseMediaViewModel model)
        {
            string actionName = "CaptureMediaItemDetails";

            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Media");
                #endregion
                return View(model);
            }

            try
            {
                int update = 0;
                try
                {
                    update = (int)Session["UPDATE"];
                    Session["UPDATE"] = "";
                }
                catch (Exception)
                {

                }
                
                foreach (var item in model.mediaList)
                {
                    var input = db.MEDIA.Where(x => x.MediaID == item.MediaID).FirstOrDefault();
                    input.MediaDescription = item.MediaDescription;
                    input.MediaCaption = item.MediaCaption;
                    input.MediaPurposeID = db.MEDIA_PURPOSE.Where(m => m.MediaPurposeValue == "Case Related").FirstOrDefault().MediaPurposeID;
                    input.StatusID = db.STATUS.Where(x => x.StatusValue == "Active").FirstOrDefault().StatusID;
                    if (item.ForensicCaseID != null)
                    {
                        input.ForensicCaseID = item.ForensicCaseID;
                    }
                    db.MEDIA.Attach(input);
                    db.Entry(input).State = EntityState.Modified;
                }
                
                db.SaveChanges();

                // TODO: Add insert logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Media");
                #endregion
                if (update < 0)
                {
                    return RedirectToAction("ForensicCaseMediaItems", new { id = Math.Abs(update) });
                }
                else
                {
                    return RedirectToAction("SelectAdditionalEvidenceItems", new { id = model.mediaList.FirstOrDefault().ForensicCaseID });
                }
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Forensic Case - Media Items");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>        
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Additional Evidence Section")]
        public ActionResult SelectAdditionalEvidenceItems(int id)
        {
            string actionName = "SelectAdditionalEvidenceItems";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case -Additional Evidence");
                #endregion
                ViewBag.FCID = id;
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Additional Evidence");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }

        }
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Additional Evidence Section")]
        public ActionResult SelectNewAdditionalEvidenceItems(int id)
        {
            string actionName = "SelectAdditionalEvidenceItems";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case -Additional Evidence");
                #endregion
                ViewBag.FCID = id;
                ViewBag.Instruction = "UPDATE";
                return View("SelectAdditionalEvidenceItems",new {id = id });
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Additional Evidence");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }

        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Additional Evidence Section")]
        public ActionResult UploadAdditionalEvidenceItems(int id)
        {
            string actionName = "UploadAdditionalEvidenceItems";

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    List<ADDITIONAL_EVIDENCE> AEitems = new List<ADDITIONAL_EVIDENCE>();

                    //string foldername = Request.Form.Get("LCDR");
                    string userInitials = VERTEBRAE.getCurrentUser().DisplayInitials.ToUpper();
                    int uId = VERTEBRAE.getCurrentUser().UserID;
                    string datestamp = DateTime.Now.Date.ToString("ddMMyyyy");

                    string foldername = userInitials;
                    string rootpath = VERTEBRAE.ADD_EV_REPORootPath + '/' + datestamp;
                    for (int i = 0; i < files.Count; i++)
                    {
                        ADDITIONAL_EVIDENCE AE = new ADDITIONAL_EVIDENCE();

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
                        AE.AdditionalEvidenceLocation = fname;
                        AEitems.Add(AE);
                        file.SaveAs(fname);

                        //404 propagate to LC
                        VERTEBRAE.GenerateAndSaveThumb(fname);
                    }

                    int AEID = 0;
                    try
                    {
                        AEID = db.ADDITIONAL_EVIDENCE.Max(x => x.AdditionalEvidenceID) + 1;
                    }
                    catch (Exception)
                    {
                        AEID = 1; //404 Propagate set to 1 after scrub
                    }
                    foreach (var AE in AEitems)
                    {
                        AE.AdditionalEvidenceID = AEID;
                        AE.ContactPersonContactNum = "0999999999";
                        AE.ContactPersonNameSurname = "PENDING";
                        AE.EvidenceDescription = "PENDING";
                        AE.EvidenceSerialNumber = "PENDING";

                        AE.ForensicCaseID = id;

                        AEID++;

                        db.ADDITIONAL_EVIDENCE.Add(AE);
                    }

                    db.SaveChanges();
                    #region AUDIT_WRITE
                    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadSuccess, "Additional Evidence");
                    #endregion
                    // Returns message that successfully uploaded  
                    return Json("File(s) Uploaded Successfully!");
                }
                catch (Exception x)
                {
                    #region AUDIT_WRITE
                    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Forensic Case - Additional Evidence");
                    #endregion
                    VERTEBRAE.DumpErrorToTxt(x);
                    return View("Error", new HandleErrorInfo(x, controllerName, actionName));
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>        
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Additional Evidence Section")]
        public ActionResult CaptureAdditionalEvidenceDetails(int id)
        {
            string actionName = "CaptureAdditionalEvidenceDetails";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Additional Evidence");
                #endregion
                #region PREPARE MODEL
                AdditionalEvidenceViewModel model = new AdditionalEvidenceViewModel();

                int uID = VERTEBRAE.getCurrentUser().UserID;
                model.additionalEvidence = db.ADDITIONAL_EVIDENCE.Where(ae => ae.FORENSIC_CASE.UserID == uID && ae.EvidenceDescription.ToUpper() == "PENDING" && ae.ForensicCaseID == id).ToList();
                if (model.additionalEvidence.Count < 1)
                {
                    //Response.
                    return RedirectToAction("All");
                }
                foreach (var item in model.additionalEvidence)
                {
                    item.EvidenceDescription = "";
                    item.ContactPersonNameSurname = "";
                    item.ContactPersonContactNum = "";
                }
                //var week = DateTime.Today.Date.AddDays(-7);
                //var cases = from fc in db.FORENSIC_CASE.Where(x => x.UserID == id)
                //            where fc.DateAdded >= week
                //            select fc;
                //model.fcList = cases.ToList();
                //model.purposeList = db.MEDIA_PURPOSE.ToList();
                //if (model.fcList.Count < 1)
                //{
                //    model.purposeList.Remove(model.purposeList.Where(x => x.MediaPurposeValue == "Case Related").FirstOrDefault());
                //} 
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Additional Evidence");
                #endregion

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Additional Evidence");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Additional Evidence Section")]
        public ActionResult CaptureAdditionalEvidenceDetails(AdditionalEvidenceViewModel model)
        {
            string actionName = "CaptureAdditionalEvidenceDetails";

            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Additional Evidence");
                #endregion
                return View(model);
            }

            try
            {
                foreach (var item in model.additionalEvidence)
                {
                    var input = db.ADDITIONAL_EVIDENCE.Where(x => x.AdditionalEvidenceID == item.AdditionalEvidenceID).FirstOrDefault();
                    input.ContactPersonContactNum = item.ContactPersonContactNum;
                    input.ContactPersonNameSurname = item.ContactPersonNameSurname;
                    input.EvidenceDescription = item.EvidenceDescription;
                    input.EvidenceSerialNumber = item.EvidenceSerialNumber;
                }
                db.SaveChanges();

                // TODO: Add insert logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Additional Evidence");
                #endregion
                return RedirectToAction("AddCauseOfDeath", new { id = model.additionalEvidence.FirstOrDefault().ForensicCaseID });
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadFail, "Forensic Case - Media Items");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Cause Of Death Section")]
        public ActionResult AddCauseOfDeath(int id)
        {
            string actionName = "AddCauseOfDeath";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Cause Of Death");
                #endregion
                #region PREPARE MODEL
                CODViewModel model = new CODViewModel();
                //var sevenDaysAgo = DateTime.Today.Date.AddDays(-7);

                int uId = VERTEBRAE.getCurrentUser().UserID;

                model.primaryCODEst = new CASE_COD_ESTIMATION();
                model.primaryCODEst.ForensicCaseID = id;
                model.primaryCODEst.ProminenceID = 1;


                model.secondaryCODEst = new CASE_COD_ESTIMATION();
                model.secondaryCODEst.ForensicCaseID = id;
                model.secondaryCODEst.ProminenceID = 2;

                model.tertiaryCODEst = new CASE_COD_ESTIMATION();
                model.tertiaryCODEst.ForensicCaseID = id;
                model.tertiaryCODEst.ProminenceID = 3;

                model.quaternaryCODEst = new CASE_COD_ESTIMATION();
                model.quaternaryCODEst.ForensicCaseID = id;
                model.quaternaryCODEst.ProminenceID = 4;

                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Cause Of Death");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Cause Of Death Section")]
        [ValidateInput(false)]
        public ActionResult AddCauseOfDeath(CODViewModel model)
        {
            if (model.primaryCODEst.CONTENT_TAG.ContentTagText == null || model.primaryCODEst.CODMotivation == null)
            {
                return View(model);
            }
            string actionName = "AddCauseOfDeath";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Cause Of Death");
                #endregion
                //return View(model);
            }

            try
            {
                List<CASE_COD_ESTIMATION> COD = new List<CASE_COD_ESTIMATION>();
                string temp = "";


                if (model.primaryCODEst.CONTENT_TAG.ContentTagText != null)
                {
                    temp = model.primaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.primaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                    model.primaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                    model.primaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Primary").FirstOrDefault().ProminenceID;
                    COD.Add(model.primaryCODEst);

                    if (model.secondaryCODEst.CONTENT_TAG.ContentTagText != null)
                    {
                        temp = model.secondaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.secondaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                        model.secondaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                        model.secondaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Secondary").FirstOrDefault().ProminenceID;
                        COD.Add(model.secondaryCODEst);

                        if (model.tertiaryCODEst.CONTENT_TAG.ContentTagText != null)
                        {
                            temp = model.tertiaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.tertiaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                            model.tertiaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                            model.tertiaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Tertiary").FirstOrDefault().ProminenceID;
                            COD.Add(model.tertiaryCODEst);
                            if (model.quaternaryCODEst.CONTENT_TAG.ContentTagText != null)
                            {
                                temp = model.quaternaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.quaternaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                                model.quaternaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                                model.quaternaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Quaternary").FirstOrDefault().ProminenceID;
                                COD.Add(model.quaternaryCODEst);
                            }
                        }
                    }
                }
                if (!(COD.Count < 1))
                {
                    //   
                    FORENSIC_CASE FC = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.primaryCODEst.ForensicCaseID).FirstOrDefault();
                    FC.CASE_COD_ESTIMATION.Clear();
                    FC.CASE_COD_ESTIMATION = COD;
                    db.FORENSIC_CASE.Attach(FC);
                    db.Entry(FC).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Cause Of Death");
                #endregion
                return RedirectToAction("AddStatistics", new { id = model.primaryCODEst.ForensicCaseID });
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Cause Of Death");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Statistics Section")]
        public ActionResult AddStatistics(int id)
        {
            string actionName = "AddStatistics";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Statistics");
                #endregion
                #region PREPARE MODEL
                StatisticsViewModel model = new StatisticsViewModel();

                int uId = VERTEBRAE.getCurrentUser().UserID;

                model.stats = new CASE_STATISTICS();
                model.stats.ForensicCaseID = id;
                model.provinces = db.PROVINCE.ToList();
                model.events = db.EVENT.ToList();
                model.sampleInvestigations = db.SAMPLE_INVESTIGATION.ToList();
                model.medTreatments = db.MEDICAL_TREATMENTS.ToList();
                model.injuryScenes = db.SCENE_OF_INJURY.ToList();
                model.externalCause = db.EXTERNAL_CAUSE.ToList();
                model.specialCategory = db.SPECIAL_CATEGORY.ToList();
                model.serviceProvider = db.SERVICE_PROVIDER.Where(sp => sp.IsDeactivated == false).ToList();
                model.autopsyTypes = db.AUTOPSY_TYPE.ToList();
                model.races = db.INDIVIDUAL_RACE.ToList();
                model.genders = db.INDIVIDUAL_GENDER.ToList();
                model.hospitalsClinics = db.HOSPITAL_CLINIC.ToList();
                model.primaryCauses = db.PRIMARY_CAUSE_DEATH.ToList();
                model.apparentManners = db.APPARENT_MANNER_DEATH.ToList();

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

                model.otherSamplesInvestigationsDescription = "";
                model.otherRaceDescription = "";
                model.otherTreatmentFacilityDescription = "";
                model.otherMedicalTreatmentsDescription = "";
                model.otherPrimaryCauseDeathDescription = "";
                model.otherApparentMannerDeathDescription = "";
                model.otherInjurySceneDescription = "";
                model.otherExternalCauseDescription = "";
                model.otherSpecialCategoryDescription = "";

                //model.DeathProvinceId = 1;
                model.otherDeathProvinceDesc = "";
                //model.ProcessingProvinceId = 1;
                model.otherProcessingProvinceDesc = "";
                //model.OccurrenceProvinceId = 1;
                model.otherOccurrenceProvinceDesc = "";
                //model.TreatmentProvinceId = 1;
                model.otherTreatmentProvinceDesc = "";
                //model.ReportProvinceId = 1;
                model.otherReportProvinceDesc = "";

                //model.JurisdictionStationID = 0;
                model.JurisdictionStationName = "";
                //model.ProcessingStationID = 0;
                model.ProcessingStationName = "";
                //model.InvestigationStationID = 0;
                model.InvestigationStationName = "";
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Statistics Section")]
        [ValidateInput(false)]
        public ActionResult AddStatistics(StatisticsViewModel model)
        {
            string actionName = "AddStatistics";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
                #endregion
                //return View(model);
            }

            try
            {
                CASE_STATISTICS stats = new CASE_STATISTICS();
                stats = model.stats;



                foreach (var item in model.selectedExternalCauses.Where(m => m.isSelected))
                {
                    STATS_EXTERNAL_CAUSE cause = new STATS_EXTERNAL_CAUSE();
                    cause.CaseStatsID = model.stats.CaseStatsID;
                    cause.ExternalCauseID = db.EXTERNAL_CAUSE.Where(m => m.ExternalCauseDescription == item.valueName).FirstOrDefault().ExternalCauseID;
                    if (item.valueName == "Other")
                    {
                        cause.OtherExternalCauseDescription = model.otherExternalCauseDescription;
                    }
                    else cause.OtherExternalCauseDescription = "N/A";
                    stats.STATS_EXTERNAL_CAUSE.Add(cause);
                }
                foreach (var item in model.selectedInjuryScenes.Where(m => m.isSelected))
                {
                    STATS_INJURY_SCENE scene = new STATS_INJURY_SCENE();
                    scene.CaseStatsID = model.stats.CaseStatsID;
                    scene.InjurySceneID = db.SCENE_OF_INJURY.Where(m => m.InjurySceneDescription == item.valueName).FirstOrDefault().InjurySceneID;
                    if (item.valueName == "Other")
                    {
                        scene.OtherSceneDescription = model.otherInjurySceneDescription;
                    }
                    else scene.OtherSceneDescription = "N/A";
                    stats.STATS_INJURY_SCENE.Add(scene);
                }
                foreach (var item in model.selectedMedicalTreatments.Where(m => m.isSelected))
                {
                    STATS_TREATMENTS med = new STATS_TREATMENTS();
                    med.CaseStatsID = model.stats.CaseStatsID;
                    med.MedicalTreatmentID = db.MEDICAL_TREATMENTS.Where(m => m.MedicalTreatmentDescription == item.valueName).FirstOrDefault().MedicalTreatmentID;
                    if (item.valueName == "Other")
                    {
                        med.OtherTreatmentDescription = model.otherMedicalTreatmentsDescription;
                    }
                    else med.OtherTreatmentDescription = "N/A";
                   stats.STATS_TREATMENTS.Add(med);
                }
                foreach (var item in model.selectedSamplesInvestigations.Where(m => m.isSelected))
                {
                    STATS_SAMPLES_INVESTIGATION sample = new STATS_SAMPLES_INVESTIGATION();
                    sample.CaseStatsID = model.stats.CaseStatsID;
                    sample.SampleInvestigationID = db.SAMPLE_INVESTIGATION.Where(m => m.SampleInvestigationDescription == item.valueName).FirstOrDefault().SampleInvestigationID;
                    if (item.valueName == "Other")
                    {
                        sample.SampleInvestigationOtherDescription = model.otherSamplesInvestigationsDescription;
                    }
                    else sample.SampleInvestigationOtherDescription = "N/A";
                    stats.STATS_SAMPLES_INVESTIGATION.Add(sample);
                }
                foreach (var item in model.selectedSpecialCategories.Where(m => m.isSelected))
                {
                    STATS_SPECIAL_CATEGORY special = new STATS_SPECIAL_CATEGORY();
                    special.CaseStatsID = model.stats.CaseStatsID;
                    special.SpecialCategoryID = db.SPECIAL_CATEGORY.Where(m => m.SpecialCategoryDescription == item.valueName).FirstOrDefault().SpecialCategoryID;
                    if (item.valueName == "Other")
                    {
                        special.OtherSpecialCategoryDescription = model.otherSpecialCategoryDescription;
                    }
                    else special.OtherSpecialCategoryDescription = "N/A";
                    stats.STATS_SPECIAL_CATEGORY.Add(special);
                }

                STATS_PROVINCE_EVENT Death = new STATS_PROVINCE_EVENT();
                Death.CaseStatsID = stats.CaseStatsID;
                Death.ProvinceID = model.DeathProvinceId;
                Death.EventID = db.EVENT.Where(m => m.EventDescription == "Death Occurrence").FirstOrDefault().EventID;
                Death.ProvinceOtherDescription = model.otherDeathProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Death);

                STATS_PROVINCE_EVENT Occurrence = new STATS_PROVINCE_EVENT();
                Occurrence.CaseStatsID = stats.CaseStatsID;
                Occurrence.ProvinceID = model.OccurrenceProvinceId;
                Occurrence.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Occurrence").FirstOrDefault().EventID;
                Occurrence.ProvinceOtherDescription = model.otherOccurrenceProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Occurrence);

                STATS_PROVINCE_EVENT Reporting = new STATS_PROVINCE_EVENT();
                Reporting.CaseStatsID = stats.CaseStatsID;
                Reporting.ProvinceID = model.ReportProvinceId;
                Reporting.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Report").FirstOrDefault().EventID;
                Reporting.ProvinceOtherDescription = model.otherReportProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Reporting);

                STATS_PROVINCE_EVENT Processing = new STATS_PROVINCE_EVENT();
                Processing.CaseStatsID = stats.CaseStatsID;
                Processing.ProvinceID = model.ProcessingProvinceId;
                Processing.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Processing").FirstOrDefault().EventID;
                Processing.ProvinceOtherDescription = model.otherProcessingProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Processing);

                STATS_PROVINCE_EVENT Treatment = new STATS_PROVINCE_EVENT();
                Treatment.CaseStatsID = stats.CaseStatsID;
                Treatment.ProvinceID = model.TreatmentProvinceId;
                Treatment.EventID = db.EVENT.Where(m => m.EventDescription == "Medical Treatment").FirstOrDefault().EventID;
                Treatment.ProvinceOtherDescription = model.otherTreatmentProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Treatment);

                STATS_POLICE_STATION Jurisdiction = new STATS_POLICE_STATION();
                Jurisdiction.CaseStatsID = stats.CaseStatsID;
                Jurisdiction.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.JurisdictionStationName).FirstOrDefault().ServiceProviderID;
                Jurisdiction.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Jurisdiction").FirstOrDefault().StationRoleID;

                stats.STATS_POLICE_STATION.Add(Jurisdiction);

                STATS_POLICE_STATION ProcessingStation = new STATS_POLICE_STATION();
                ProcessingStation.CaseStatsID = stats.CaseStatsID;
                ProcessingStation.ServiceProviderID = model.ProcessingStationID;
                ProcessingStation.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.ProcessingStationName).FirstOrDefault().ServiceProviderID;
                ProcessingStation.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Processing").FirstOrDefault().StationRoleID;
                stats.STATS_POLICE_STATION.Add(ProcessingStation);

                STATS_POLICE_STATION Investigating = new STATS_POLICE_STATION();
                Investigating.CaseStatsID = stats.CaseStatsID;
                Investigating.ServiceProviderID = model.InvestigationStationID;
                Investigating.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.InvestigationStationName).FirstOrDefault().ServiceProviderID;
                Investigating.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Investigation").FirstOrDefault().StationRoleID;
                stats.STATS_POLICE_STATION.Add(Investigating);

                stats.IndividualRaceOtherDescription = model.otherRaceDescription;
                stats.HospitalClinicOtherDescription = model.otherTreatmentFacilityDescription;
                stats.OtherApparentMannerDescription = model.otherApparentMannerDeathDescription;
                stats.OtherPrimaryCauseDescription = model.otherPrimaryCauseDeathDescription;





                db.FORENSIC_CASE.Where(m => m.ForensicCaseID == stats.ForensicCaseID).FirstOrDefault().CASE_STATISTICS.Add(stats);
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Statistics");
                #endregion
                return RedirectToAction("ForensicCaseAddedSuccess");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Statistics Section")]
        public ActionResult ForensicCaseAddedSuccess()
        {
            string actionName = "ForensicCaseAddedSuccess";
            try
            {
                return View();
            }
            catch (Exception x)
            {
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
//----------------------------------------------------------------------------------------------//
        #region READS:

        [AuthorizeByAccessArea(AccessArea = "Search Forensic Case")]
        public ActionResult All()
        {
            string actionName = "All";
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
                VERTEBRAE.DumpErrorToTxt(x);
                //return View("Error", new HandleErrorInfo(x, "ForensicCase", "All"));

                return View("Error", new HandleErrorInfo(x, controllerName, actionName));


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

                if (modelCase.CASE_STATISTICS.Count < 1)
                {
                    modelStats = new CASE_STATISTICS();
                }
                else
                {
                    modelStats = modelCase.CASE_STATISTICS.FirstOrDefault();
                }

                model.forensicCase = modelCase;
                foreach (var item in model.forensicCase.SERVICE_REQUEST)
                {
                    if (item.SPECIMEN.FirstOrDefault().EXTERNAL_REPORT == null)
                    {
                        item.SPECIMEN.FirstOrDefault().EXTERNAL_REPORT = new EXTERNAL_REPORT();
                    }
                }
                model.genObservation = modelCase.GENERAL_OBSERVATION.FirstOrDefault();
                model.headNeckObservation = modelCase.HEAD_NECK_OBSERVATION.FirstOrDefault();
                model.abdObservation = modelCase.ABDOMEN_OBSERVATION.FirstOrDefault();
                model.spineObservation = modelCase.SPINE_OBSERVATION.FirstOrDefault();
                model.chestObservation = modelCase.CHEST_OBSERVATION.FirstOrDefault();
                model.serviceRequests = modelCase.SERVICE_REQUEST.ToList();
                model.media = modelCase.MEDIA.ToList();
                model.additionalEvidence = modelCase.ADDITIONAL_EVIDENCE.ToList();
                model.stats = modelStats;
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

        [AuthorizeByAccessArea(AccessArea = "View Forensic Case - All Sections")]
        public ActionResult GenerateReport(int id)
        {
            try
            {

                #region VALIDATE_ACCESS
                bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "Generate Forensic Case Report");
                #endregion

                //var PDF = HtmlToPdf.StaticRenderUrlAsPdf(new Uri("localhost:56640/ForensicCase/Details/"+id));
                //return File(PDF.BinaryData, "application/pdf", "Wiki.Pdf"); 

                IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();

                // add a header to very page easily
                Renderer.PrintOptions.FirstPageNumber = 1; // use 2 if a cover page will be
                Renderer.PrintOptions.Header.DrawDividerLine = true;
                Renderer.PrintOptions.Header.CenterText = "{url}";
                Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
                Renderer.PrintOptions.Header.FontSize = 12;
                // add a footer too
                Renderer.PrintOptions.Footer.DrawDividerLine = true;
                Renderer.PrintOptions.Footer.FontFamily = "Arial";
                Renderer.PrintOptions.Footer.FontSize = 10;
                Renderer.PrintOptions.Footer.LeftText = "{date} {time}";
                Renderer.PrintOptions.Footer.RightText = "{page} of {total-pages}";

                var file = Renderer.RenderHtmlAsPdf("<h1>Hello World<h1>”).SaveAs(“html-string.pdf");

                return File(file.BinaryData, "application/pdf", "test.pdf");

                //return File(Renderer)

                //return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingFail, "Generate Forensic Case Report");
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
        [AuthorizeByAccessArea(AccessArea = "Migrate Forensic Case")]
        //>>>>>>>>>>>>>>>>>>>>>>
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

                //return View();


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

        // TOTO: Missing Migrate POST    
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        public ActionResult ForensicCaseUpdateHub(int id)
        {
            ViewBag.TargetDR = db.FORENSIC_CASE.Where(fc => fc.ForensicCaseID == id).FirstOrDefault().ForensicDRNumber;            
            ViewBag.TargetID = id;
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        public ActionResult ForensicCaseUpdateHub()
        {
            return View();
        }
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Core Data Section")]
        public ActionResult UpdateCoreData(int id)
        {
            ViewBag.Instruction = "UPDATE";
            #region PREPARE MODEL
            CoreDataViewModel model = new CoreDataViewModel();
            var sevenDaysAgo = DateTime.Today.Date.AddDays(-7);
            model.forensicCase = db.FORENSIC_CASE.Where(m => m.ForensicCaseID == id).FirstOrDefault();
            model.autopsyAreas = db.AUTOPSY_AREA.ToList();
            model.sessionSelector = new List<sessionKVP>();
            foreach (var item in db.SESSION.Where(x => x.DateID > sevenDaysAgo))
            {
                sessionKVP newSesh = new sessionKVP();
                newSesh.sessionID = item.SessionID;
                newSesh.sessionDesc = item.SLOT.Description + " at " + item.SLOT.StartTime.ToString("HH:mm") + " on " + item.DateID.ToString("dd MMM yyyy");
                model.sessionSelector.Add(newSesh);
            }
            #endregion
            return View("AddCoreData", model);
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Core Data Section")]
        [ValidateInput(false)]
        public ActionResult UpdateCoreData(CoreDataViewModel model)
        {
            string actionName = "AddCoreData";
            //if (!ModelState.IsValid) //todo: rebrain this
            //{
            //    #region AUDIT_WRITE
            //    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Core Data");
            //    #endregion
            //    //return View(model);
            //}

            try
            {
                FORENSIC_CASE upCase = default(FORENSIC_CASE);
                upCase = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.forensicCase.ForensicCaseID).FirstOrDefault();

                upCase.ForensicDRNumber = model.forensicCase.ForensicDRNumber;
                upCase.FCBriefDescription = model.forensicCase.FCBriefDescription;
                upCase.ActingOfficerContactNum = model.forensicCase.ActingOfficerContactNum;
                upCase.ActingOfficerNameSurname = model.forensicCase.ActingOfficerNameSurname;
                upCase.DHANoticeDeathID = model.forensicCase.DHANoticeDeathID;
                upCase.CauseOfDeathConclusion = model.forensicCase.CauseOfDeathConclusion;
                upCase.AutopsyAreaID = model.forensicCase.AutopsyAreaID;
                upCase.SessionID = model.forensicCase.SessionID;





                db.FORENSIC_CASE.Attach(upCase);
                db.Entry(upCase).State = EntityState.Modified;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Core Data");
                #endregion
                return RedirectToAction("UpdateObservations", new { id = upCase.ForensicCaseID });
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
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Observations Section")]
        public ActionResult UpdateObservations(int id)
        {
            ObservationsViewModel model = new ObservationsViewModel();
            ViewBag.Instruction = "UPDATE";
            model.genObservation = db.GENERAL_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();

            model.abdObservation = db.ABDOMEN_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();

            model.chestObservation = db.CHEST_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();

            model.headNeckObservation = db.HEAD_NECK_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();

            model.spineObservation = db.SPINE_OBSERVATION.Where(x => x.ForensicCaseID == id).FirstOrDefault();

            return View("AddObservations", model);
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Observations Section")]
        [ValidateInput(false)]
        public ActionResult UpdateObservations(ObservationsViewModel model)
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
                FORENSIC_CASE upCase = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.abdObservation.ForensicCaseID).FirstOrDefault();

                upCase.GENERAL_OBSERVATION.Clear();
                upCase.GENERAL_OBSERVATION.Add(model.genObservation);
                upCase.GENERAL_OBSERVATION.FirstOrDefault().setFlags();

                upCase.ABDOMEN_OBSERVATION.Clear();
                upCase.ABDOMEN_OBSERVATION.Add(model.abdObservation);
                upCase.ABDOMEN_OBSERVATION.FirstOrDefault().setFlags();

                upCase.HEAD_NECK_OBSERVATION.Clear();
                upCase.HEAD_NECK_OBSERVATION.Add(model.headNeckObservation);
                upCase.HEAD_NECK_OBSERVATION.FirstOrDefault().setFlags();

                upCase.SPINE_OBSERVATION.Clear();
                upCase.SPINE_OBSERVATION.Add(model.spineObservation);
                upCase.SPINE_OBSERVATION.FirstOrDefault().setFlags();

                upCase.CHEST_OBSERVATION.Clear();
                upCase.CHEST_OBSERVATION.Add(model.chestObservation);
                upCase.CHEST_OBSERVATION.FirstOrDefault().setFlags();

                db.FORENSIC_CASE.Attach(upCase);
                db.Entry(upCase).State = EntityState.Modified;
                db.SaveChanges();

                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Observations");
                #endregion
                //return RedirectToAction("AddServiceRequests");
                return RedirectToAction("UpdateCauseOfDeath", new { id = upCase.ForensicCaseID });
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
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Statistics Section")]
        public ActionResult UpdateStatistics(int id)
        {
            string actionName = "AddStatistics";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Statistics");
                #endregion
                #region PREPARE MODEL
                StatisticsViewModel model = new StatisticsViewModel();

                int uId = VERTEBRAE.getCurrentUser().UserID;

                model.stats = db.CASE_STATISTICS.Where(x => x.ForensicCaseID == id).FirstOrDefault();
                model.provinces = db.PROVINCE.ToList();
                model.events = db.EVENT.ToList();
                model.sampleInvestigations = db.SAMPLE_INVESTIGATION.ToList();
                model.medTreatments = db.MEDICAL_TREATMENTS.ToList();
                model.injuryScenes = db.SCENE_OF_INJURY.ToList();
                model.externalCause = db.EXTERNAL_CAUSE.ToList();
                model.specialCategory = db.SPECIAL_CATEGORY.ToList();
                model.serviceProvider = db.SERVICE_PROVIDER.Where(sp => sp.IsDeactivated == false).ToList();
                model.autopsyTypes = db.AUTOPSY_TYPE.ToList();
                model.races = db.INDIVIDUAL_RACE.ToList();
                model.genders = db.INDIVIDUAL_GENDER.ToList();
                model.hospitalsClinics = db.HOSPITAL_CLINIC.ToList();
                model.primaryCauses = db.PRIMARY_CAUSE_DEATH.ToList();
                model.apparentManners = db.APPARENT_MANNER_DEATH.ToList();

                model.selectedSamplesInvestigations = new List<multiselectKVP>();
                foreach (var item in model.sampleInvestigations)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.SampleInvestigationDescription;
                    newKVP.valueID = item.SampleInvestigationID;
                    newKVP.isSelected = false;
                    foreach (var sample in model.stats.STATS_SAMPLES_INVESTIGATION)
                    {
                        if (item.SampleInvestigationDescription  == sample.SAMPLE_INVESTIGATION.SampleInvestigationDescription)
                        {
                            newKVP.isSelected = true;
                            if (item.SampleInvestigationDescription == "Other")
                            {
                                model.otherSamplesInvestigationsDescription = sample.SampleInvestigationOtherDescription;
                            }
                        }
                    }
                    model.selectedSamplesInvestigations.Add(newKVP);
                }

                model.selectedMedicalTreatments = new List<multiselectKVP>();
                foreach (var item in model.medTreatments)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.MedicalTreatmentDescription;
                    newKVP.valueID = item.MedicalTreatmentID;
                    newKVP.isSelected = false;
                    foreach (var sample in model.stats.STATS_TREATMENTS)
                    {
                        if (item.MedicalTreatmentDescription == sample.MEDICAL_TREATMENTS.MedicalTreatmentDescription)
                        {
                            newKVP.isSelected = true;
                            if (item.MedicalTreatmentDescription == "Other")
                            {
                                model.otherMedicalTreatmentsDescription = sample.OtherTreatmentDescription;
                            }
                        }
                    }
                    model.selectedMedicalTreatments.Add(newKVP);
                }

                model.selectedInjuryScenes = new List<multiselectKVP>();
                foreach (var item in model.injuryScenes)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.InjurySceneDescription;
                    newKVP.valueID = item.InjurySceneID;
                    newKVP.isSelected = false;
                    foreach (var sample in model.stats.STATS_INJURY_SCENE)
                    {
                        if (item.InjurySceneDescription == sample.SCENE_OF_INJURY.InjurySceneDescription)
                        {
                            newKVP.isSelected = true;
                            if (item.InjurySceneDescription == "Other")
                            {
                                model.otherInjurySceneDescription = sample.OtherSceneDescription;
                            }
                        }
                    }
                    model.selectedInjuryScenes.Add(newKVP);
                }

                model.selectedExternalCauses = new List<multiselectKVP>();
                foreach (var item in model.externalCause)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.ExternalCauseDescription;
                    newKVP.valueID = item.ExternalCauseID;
                    newKVP.isSelected = false;
                    foreach (var sample in model.stats.STATS_EXTERNAL_CAUSE)
                    {
                        if (item.ExternalCauseDescription == sample.EXTERNAL_CAUSE.ExternalCauseDescription)
                        {
                            newKVP.isSelected = true;
                            if (item.ExternalCauseDescription == "Other")
                            {
                                model.otherExternalCauseDescription = sample.OtherExternalCauseDescription;
                            }
                        }
                    }
                    model.selectedExternalCauses.Add(newKVP);
                }

                model.selectedSpecialCategories = new List<multiselectKVP>();
                foreach (var item in model.specialCategory)
                {
                    multiselectKVP newKVP = new multiselectKVP();
                    newKVP.valueName = item.SpecialCategoryDescription;
                    newKVP.valueID = item.SpecialCategoryID;
                    newKVP.isSelected = false;
                    foreach (var sample in model.stats.STATS_SPECIAL_CATEGORY)
                    {
                        if (item.SpecialCategoryDescription == sample.SPECIAL_CATEGORY.SpecialCategoryDescription)
                        {
                            newKVP.isSelected = true;
                            if (item.SpecialCategoryDescription == "Other")
                            {
                                model.otherSpecialCategoryDescription = sample.OtherSpecialCategoryDescription;
                            }
                        }
                    }
                    model.selectedSpecialCategories.Add(newKVP);
                }

                //model.otherSamplesInvestigationsDescription = model.stats.STATS_SAMPLES_INVESTIGATION.FirstOrDefault().SampleInvestigationOtherDescription;
                model.otherRaceDescription = model.stats.IndividualRaceOtherDescription;
                model.otherTreatmentFacilityDescription = model.stats.HospitalClinicOtherDescription;
                //model.otherMedicalTreatmentsDescription  = model.stats.STATS_TREATMENTS.FirstOrDefault().OtherTreatmentDescription;
                model.otherPrimaryCauseDeathDescription = model.stats.OtherPrimaryCauseDescription; //
                model.otherApparentMannerDeathDescription = model.stats.OtherApparentMannerDescription;
                model.stats.AutopsyTypeOtherDescription = model.stats.AutopsyTypeOtherDescription;
               // model.otherInjurySceneDescription = model.stats.STATS_INJURY_SCENE.FirstOrDefault().OtherSceneDescription;
                //model.otherExternalCauseDescription = model.stats.STATS_EXTERNAL_CAUSE.FirstOrDefault().OtherExternalCauseDescription;
                //model.otherSpecialCategoryDescription  = model.stats.STATS_SPECIAL_CATEGORY.FirstOrDefault().OtherSpecialCategoryDescription;

                model.DeathProvinceId = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Death Occurrence").FirstOrDefault().ProvinceID;
                model.otherDeathProvinceDesc = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Death Occurrence").FirstOrDefault().ProvinceOtherDescription;
                model.ProcessingProvinceId = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Incident Processing").FirstOrDefault().ProvinceID;
                model.otherProcessingProvinceDesc = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Incident Processing").FirstOrDefault().ProvinceOtherDescription;
                model.OccurrenceProvinceId = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Incident Occurrence").FirstOrDefault().ProvinceID;
                model.otherOccurrenceProvinceDesc = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Incident Occurrence").FirstOrDefault().ProvinceOtherDescription;
                model.TreatmentProvinceId = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Medical Treatment").FirstOrDefault().ProvinceID;
                model.otherTreatmentProvinceDesc = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Medical Treatment").FirstOrDefault().ProvinceOtherDescription;
                model.ReportProvinceId = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Incident Report").FirstOrDefault().ProvinceID;
                model.otherReportProvinceDesc = model.stats.STATS_PROVINCE_EVENT.Where(x => x.EVENT.EventDescription == "Incident Report").FirstOrDefault().ProvinceOtherDescription;

                //model.JurisdictionStationID = 0;
                model.JurisdictionStationName = model.stats.STATS_POLICE_STATION.Where(x=>x.STATION_ROLE.StationRoleDescription == "Jurisdiction").FirstOrDefault().SERVICE_PROVIDER.CompanyName;
                //model.ProcessingStationID = 0;
                model.ProcessingStationName = model.stats.STATS_POLICE_STATION.Where(x => x.STATION_ROLE.StationRoleDescription == "Processing").FirstOrDefault().SERVICE_PROVIDER.CompanyName;
                //model.InvestigationStationID = 0;
                model.InvestigationStationName = model.stats.STATS_POLICE_STATION.Where(x => x.STATION_ROLE.StationRoleDescription == "Investigation").FirstOrDefault().SERVICE_PROVIDER.CompanyName;

                model.stats.DiscoveryDate =Convert.ToDateTime(Convert.ToDateTime(model.stats.DiscoveryDate).ToString("yyyy-MM-dd"));
                ViewBag.Instruction = "UPDATE";

                #endregion
                return View("AddStatistics",model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Statistics Section")]
        [ValidateInput(false)]
        public ActionResult UpdateStatistics(StatisticsViewModel model)
        {
            //string actionName = "AddStatistics";
            //if (!ModelState.IsValid)
            //{
            //    #region AUDIT_WRITE
            //    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
            //    #endregion
            //    //return View(model);
            //}

            //try
            //{
            //    CASE_STATISTICS stats = new CASE_STATISTICS();
            //    stats = model.stats;

            //    foreach (var item in model.selectedExternalCauses.Where(m => m.isSelected))
            //    {
            //        STATS_EXTERNAL_CAUSE cause = new STATS_EXTERNAL_CAUSE();
            //        cause.CaseStatsID = model.stats.CaseStatsID;
            //        cause.ExternalCauseID = db.EXTERNAL_CAUSE.Where(m => m.ExternalCauseDescription == item.valueName).FirstOrDefault().ExternalCauseID;
            //        cause.OtherExternalCauseDescription = "NULL";
            //        stats.STATS_EXTERNAL_CAUSE.Add(cause);
            //    }
            //    foreach (var item in model.selectedInjuryScenes.Where(m => m.isSelected))
            //    {
            //        STATS_INJURY_SCENE scene = new STATS_INJURY_SCENE();
            //        scene.CaseStatsID = model.stats.CaseStatsID;
            //        scene.InjurySceneID = db.SCENE_OF_INJURY.Where(m => m.InjurySceneDescription == item.valueName).FirstOrDefault().InjurySceneID;
            //        scene.OtherSceneDescription = "NULL";
            //        stats.STATS_INJURY_SCENE.Add(scene);
            //    }
            //    foreach (var item in model.selectedMedicalTreatments.Where(m => m.isSelected))
            //    {
            //        STATS_TREATMENTS med = new STATS_TREATMENTS();
            //        med.CaseStatsID = model.stats.CaseStatsID;
            //        med.MedicalTreatmentID = db.MEDICAL_TREATMENTS.Where(m => m.MedicalTreatmentDescription == item.valueName).FirstOrDefault().MedicalTreatmentID;
            //        med.OtherTreatmentDescription = "NULL";
            //        stats.STATS_TREATMENTS.Add(med);
            //    }
            //    foreach (var item in model.selectedSamplesInvestigations.Where(m => m.isSelected))
            //    {
            //        STATS_SAMPLES_INVESTIGATION sample = new STATS_SAMPLES_INVESTIGATION();
            //        sample.CaseStatsID = model.stats.CaseStatsID;
            //        sample.SampleInvestigationID = db.SAMPLE_INVESTIGATION.Where(m => m.SampleInvestigationDescription == item.valueName).FirstOrDefault().SampleInvestigationID;
            //        sample.SampleInvestigationOtherDescription = "NULL";
            //        stats.STATS_SAMPLES_INVESTIGATION.Add(sample);
            //    }
            //    foreach (var item in model.selectedSpecialCategories.Where(m => m.isSelected))
            //    {
            //        STATS_SPECIAL_CATEGORY special = new STATS_SPECIAL_CATEGORY();
            //        special.CaseStatsID = model.stats.CaseStatsID;
            //        special.SpecialCategoryID = db.SPECIAL_CATEGORY.Where(m => m.SpecialCategoryDescription == item.valueName).FirstOrDefault().SpecialCategoryID;
            //        special.OtherSpecialCategoryDescription = "NULL";
            //        stats.STATS_SPECIAL_CATEGORY.Add(special);
            //    }

            //    STATS_PROVINCE_EVENT Death = new STATS_PROVINCE_EVENT();
            //    Death.CaseStatsID = stats.CaseStatsID;
            //    Death.ProvinceID = model.DeathProvinceId;
            //    Death.EventID = db.EVENT.Where(m => m.EventDescription == "Death Occurrence").FirstOrDefault().EventID;
            //    Death.ProvinceOtherDescription = "NULL";
            //    stats.STATS_PROVINCE_EVENT.Add(Death);

            //    STATS_PROVINCE_EVENT Occurrence = new STATS_PROVINCE_EVENT();
            //    Occurrence.CaseStatsID = stats.CaseStatsID;
            //    Occurrence.ProvinceID = model.OccurrenceProvinceId;
            //    Occurrence.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Occurrence").FirstOrDefault().EventID;
            //    Occurrence.ProvinceOtherDescription = "NULL";
            //    stats.STATS_PROVINCE_EVENT.Add(Occurrence);

            //    STATS_PROVINCE_EVENT Reporting = new STATS_PROVINCE_EVENT();
            //    Reporting.CaseStatsID = stats.CaseStatsID;
            //    Reporting.ProvinceID = model.ReportProvinceId;
            //    Reporting.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Report").FirstOrDefault().EventID;
            //    Reporting.ProvinceOtherDescription = "NULL";
            //    stats.STATS_PROVINCE_EVENT.Add(Reporting);

            //    STATS_PROVINCE_EVENT Processing = new STATS_PROVINCE_EVENT();
            //    Processing.CaseStatsID = stats.CaseStatsID;
            //    Processing.ProvinceID = model.ProcessingProvinceId;
            //    Processing.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Processing").FirstOrDefault().EventID;
            //    Processing.ProvinceOtherDescription = "NULL";
            //    stats.STATS_PROVINCE_EVENT.Add(Processing);

            //    STATS_PROVINCE_EVENT Treatment = new STATS_PROVINCE_EVENT();
            //    Treatment.CaseStatsID = stats.CaseStatsID;
            //    Treatment.ProvinceID = model.TreatmentProvinceId;
            //    Treatment.EventID = db.EVENT.Where(m => m.EventDescription == "Medical Treatment").FirstOrDefault().EventID;
            //    Treatment.ProvinceOtherDescription = "NULL";
            //    stats.STATS_PROVINCE_EVENT.Add(Treatment);

            //    STATS_POLICE_STATION Jurisdiction = new STATS_POLICE_STATION();
            //    Jurisdiction.CaseStatsID = stats.CaseStatsID;
            //    Jurisdiction.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.JurisdictionStationName).FirstOrDefault().ServiceProviderID;
            //    Jurisdiction.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Jurisdiction").FirstOrDefault().StationRoleID;

            //    stats.STATS_POLICE_STATION.Add(Jurisdiction);

            //    STATS_POLICE_STATION ProcessingStation = new STATS_POLICE_STATION();
            //    ProcessingStation.CaseStatsID = stats.CaseStatsID;
            //    ProcessingStation.ServiceProviderID = model.ProcessingStationID;
            //    ProcessingStation.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.ProcessingStationName).FirstOrDefault().ServiceProviderID;
            //    ProcessingStation.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Processing").FirstOrDefault().StationRoleID;
            //    stats.STATS_POLICE_STATION.Add(ProcessingStation);

            //    STATS_POLICE_STATION Investigating = new STATS_POLICE_STATION();
            //    Investigating.CaseStatsID = stats.CaseStatsID;
            //    Investigating.ServiceProviderID = model.InvestigationStationID;
            //    Investigating.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.InvestigationStationName).FirstOrDefault().ServiceProviderID;
            //    Investigating.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Investigation").FirstOrDefault().StationRoleID;
            //    stats.STATS_POLICE_STATION.Add(Investigating);


            //    db.FORENSIC_CASE.Where(m => m.ForensicCaseID == stats.ForensicCaseID).FirstOrDefault().CASE_STATISTICS.Add(stats);
            //    db.SaveChanges();
            //    #region AUDIT_WRITE
            //    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Statistics");
            //    #endregion
            //    return RedirectToAction("ForensicCaseAddedSuccess");
            //}
            //catch (Exception x)
            //{
            //    #region AUDIT_WRITE
            //    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
            //    #endregion
            //    VERTEBRAE.DumpErrorToTxt(x);
            //    return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            //}
            string actionName = "AddStatistics";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
                #endregion
                //return View(model);
            }

            try
            {
                CASE_STATISTICS stats = new CASE_STATISTICS();
                stats = model.stats;



                foreach (var item in model.selectedExternalCauses.Where(m => m.isSelected))
                {
                    STATS_EXTERNAL_CAUSE cause = new STATS_EXTERNAL_CAUSE();
                    cause.CaseStatsID = model.stats.CaseStatsID;
                    cause.ForensicCaseID = model.stats.ForensicCaseID;
                    cause.ExternalCauseID = db.EXTERNAL_CAUSE.Where(m => m.ExternalCauseDescription == item.valueName).FirstOrDefault().ExternalCauseID;
                    if (item.valueName == "Other")
                    {
                        cause.OtherExternalCauseDescription = model.otherExternalCauseDescription;
                    }
                    else cause.OtherExternalCauseDescription = "N/A";
                    stats.STATS_EXTERNAL_CAUSE.Add(cause);
                }
                foreach (var item in model.selectedInjuryScenes.Where(m => m.isSelected))
                {
                    STATS_INJURY_SCENE scene = new STATS_INJURY_SCENE();
                    scene.CaseStatsID = model.stats.CaseStatsID;
                    scene.ForensicCaseID = model.stats.ForensicCaseID;
                    scene.InjurySceneID = db.SCENE_OF_INJURY.Where(m => m.InjurySceneDescription == item.valueName).FirstOrDefault().InjurySceneID;
                    if (item.valueName == "Other")
                    {
                        scene.OtherSceneDescription = model.otherInjurySceneDescription;
                    }
                    else scene.OtherSceneDescription = "N/A";
                    stats.STATS_INJURY_SCENE.Add(scene);
                }
                foreach (var item in model.selectedMedicalTreatments.Where(m => m.isSelected))
                {
                    STATS_TREATMENTS med = new STATS_TREATMENTS();
                    med.CaseStatsID = model.stats.CaseStatsID;
                    med.ForensicCaseID = model.stats.ForensicCaseID;
                    med.MedicalTreatmentID = db.MEDICAL_TREATMENTS.Where(m => m.MedicalTreatmentDescription == item.valueName).FirstOrDefault().MedicalTreatmentID;
                    if (item.valueName == "Other")
                    {
                        med.OtherTreatmentDescription = model.otherMedicalTreatmentsDescription;
                    }
                    else med.OtherTreatmentDescription = "N/A";
                    stats.STATS_TREATMENTS.Add(med);
                }
                foreach (var item in model.selectedSamplesInvestigations.Where(m => m.isSelected))
                {
                    STATS_SAMPLES_INVESTIGATION sample = new STATS_SAMPLES_INVESTIGATION();
                    sample.CaseStatsID = model.stats.CaseStatsID;
                    sample.ForensicCaseID = model.stats.ForensicCaseID;
                    sample.SampleInvestigationID = db.SAMPLE_INVESTIGATION.Where(m => m.SampleInvestigationDescription == item.valueName).FirstOrDefault().SampleInvestigationID;
                    if (item.valueName == "Other")
                    {
                        sample.SampleInvestigationOtherDescription = model.otherSamplesInvestigationsDescription;
                    }
                    else sample.SampleInvestigationOtherDescription = "N/A";
                    stats.STATS_SAMPLES_INVESTIGATION.Add(sample);
                }
                foreach (var item in model.selectedSpecialCategories.Where(m => m.isSelected))
                {
                    STATS_SPECIAL_CATEGORY special = new STATS_SPECIAL_CATEGORY();
                    special.CaseStatsID = model.stats.CaseStatsID;
                    special.ForensicCaseID = model.stats.ForensicCaseID;
                    special.SpecialCategoryID = db.SPECIAL_CATEGORY.Where(m => m.SpecialCategoryDescription == item.valueName).FirstOrDefault().SpecialCategoryID;
                    if (item.valueName == "Other")
                    {
                        special.OtherSpecialCategoryDescription = model.otherSpecialCategoryDescription;
                    }
                    else special.OtherSpecialCategoryDescription = "N/A";
                    stats.STATS_SPECIAL_CATEGORY.Add(special);
                }

                STATS_PROVINCE_EVENT Death = new STATS_PROVINCE_EVENT();
                Death.CaseStatsID = stats.CaseStatsID;
                Death.ForensicCaseID = model.stats.ForensicCaseID;
                Death.ProvinceID = model.DeathProvinceId;
                Death.EventID = db.EVENT.Where(m => m.EventDescription == "Death Occurrence").FirstOrDefault().EventID;
                Death.ProvinceOtherDescription = model.otherDeathProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Death);

                STATS_PROVINCE_EVENT Occurrence = new STATS_PROVINCE_EVENT();
                Occurrence.CaseStatsID = stats.CaseStatsID;
                Occurrence.ForensicCaseID = model.stats.ForensicCaseID;
                Occurrence.ProvinceID = model.OccurrenceProvinceId;
                Occurrence.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Occurrence").FirstOrDefault().EventID;
                Occurrence.ProvinceOtherDescription = model.otherOccurrenceProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Occurrence);

                STATS_PROVINCE_EVENT Reporting = new STATS_PROVINCE_EVENT();
                Reporting.CaseStatsID = stats.CaseStatsID;
                Reporting.ForensicCaseID = model.stats.ForensicCaseID;
                Reporting.ProvinceID = model.ReportProvinceId;
                Reporting.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Report").FirstOrDefault().EventID;
                Reporting.ProvinceOtherDescription = model.otherReportProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Reporting);

                STATS_PROVINCE_EVENT Processing = new STATS_PROVINCE_EVENT();
                Processing.CaseStatsID = stats.CaseStatsID;
                Processing.ForensicCaseID = model.stats.ForensicCaseID;
                Processing.ProvinceID = model.ProcessingProvinceId;
                Processing.EventID = db.EVENT.Where(m => m.EventDescription == "Incident Processing").FirstOrDefault().EventID;
                Processing.ProvinceOtherDescription = model.otherProcessingProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Processing);

                STATS_PROVINCE_EVENT Treatment = new STATS_PROVINCE_EVENT();
                Treatment.CaseStatsID = stats.CaseStatsID;
                Treatment.ForensicCaseID = model.stats.ForensicCaseID;
                Treatment.ProvinceID = model.TreatmentProvinceId;
                Treatment.EventID = db.EVENT.Where(m => m.EventDescription == "Medical Treatment").FirstOrDefault().EventID;
                Treatment.ProvinceOtherDescription = model.otherTreatmentProvinceDesc;
                stats.STATS_PROVINCE_EVENT.Add(Treatment);

                STATS_POLICE_STATION Jurisdiction = new STATS_POLICE_STATION();
                Jurisdiction.CaseStatsID = stats.CaseStatsID;
                Jurisdiction.ForensicCaseID = model.stats.ForensicCaseID;
                Jurisdiction.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.JurisdictionStationName).FirstOrDefault().ServiceProviderID;
                Jurisdiction.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Jurisdiction").FirstOrDefault().StationRoleID;

                stats.STATS_POLICE_STATION.Add(Jurisdiction);

                STATS_POLICE_STATION ProcessingStation = new STATS_POLICE_STATION();
                ProcessingStation.CaseStatsID = stats.CaseStatsID;
                ProcessingStation.ForensicCaseID = model.stats.ForensicCaseID;
                ProcessingStation.ServiceProviderID = model.ProcessingStationID;
                ProcessingStation.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.ProcessingStationName).FirstOrDefault().ServiceProviderID;
                ProcessingStation.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Processing").FirstOrDefault().StationRoleID;
                stats.STATS_POLICE_STATION.Add(ProcessingStation);

                STATS_POLICE_STATION Investigating = new STATS_POLICE_STATION();
                Investigating.CaseStatsID = stats.CaseStatsID;
                Investigating.ForensicCaseID = model.stats.ForensicCaseID;
                Investigating.ServiceProviderID = model.InvestigationStationID;
                Investigating.ServiceProviderID = db.SERVICE_PROVIDER.Where(m => m.CompanyName == model.InvestigationStationName).FirstOrDefault().ServiceProviderID;
                Investigating.StationRoleID = db.STATION_ROLE.Where(m => m.StationRoleDescription == "Investigation").FirstOrDefault().StationRoleID;
                stats.STATS_POLICE_STATION.Add(Investigating);

                stats.IndividualRaceOtherDescription = model.otherRaceDescription;
                stats.HospitalClinicOtherDescription = model.otherTreatmentFacilityDescription;
                stats.OtherApparentMannerDescription = model.otherApparentMannerDeathDescription;
                stats.OtherPrimaryCauseDescription = model.otherPrimaryCauseDeathDescription;





                db.CASE_STATISTICS.Attach(stats);
                db.Entry(stats).State = EntityState.Modified;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Statistics");
                #endregion
                return RedirectToAction("ForensicCaseUpdatedSuccess");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Statistics");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")] //todo
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Statistics Section")] //todo
        public ActionResult UpdateCauseOfDeath(int id)
        {
            CODViewModel model = new CODViewModel();

            model.primaryCODEst = db.CASE_COD_ESTIMATION.Where(x => x.ForensicCaseID == id && x.COD_PROMINENCE.ProminenceValue == "Primary").FirstOrDefault();
            model.secondaryCODEst = db.CASE_COD_ESTIMATION.Where(x => x.ForensicCaseID == id && x.COD_PROMINENCE.ProminenceValue == "Secondary").FirstOrDefault();
            model.tertiaryCODEst = db.CASE_COD_ESTIMATION.Where(x => x.ForensicCaseID == id && x.COD_PROMINENCE.ProminenceValue == "Tertiary").FirstOrDefault();
            model.quaternaryCODEst = db.CASE_COD_ESTIMATION.Where(x => x.ForensicCaseID == id && x.COD_PROMINENCE.ProminenceValue == "Quaternary").FirstOrDefault();
            ViewBag.Instruction = "UPDATE";

            return View("AddCauseOfDeath",model);
        }
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Statistics Section")]
        [ValidateInput(false)]
        public ActionResult UpdateCauseOfDeath(CODViewModel model)
        {

            string actionName = "AddCauseOfDeath";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Cause Of Death");
                #endregion
                //return View(model);
            }

            try
            {
                List<CASE_COD_ESTIMATION> COD = new List<CASE_COD_ESTIMATION>();
                string temp = "";


                if (model.primaryCODEst.CONTENT_TAG.ContentTagText != null)
                {
                    temp = model.primaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.primaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                    model.primaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                    model.primaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Primary").FirstOrDefault().ProminenceID;
                    COD.Add(model.primaryCODEst);

                    if (model.secondaryCODEst.CONTENT_TAG.ContentTagText != null)
                    {
                        temp = model.secondaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.secondaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                        model.secondaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                        model.secondaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Secondary").FirstOrDefault().ProminenceID;
                        COD.Add(model.secondaryCODEst);

                        if (model.tertiaryCODEst.CONTENT_TAG.ContentTagText != null)
                        {
                            temp = model.tertiaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.tertiaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                            model.tertiaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                            model.tertiaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Tertiary").FirstOrDefault().ProminenceID;
                            COD.Add(model.tertiaryCODEst);
                            if (model.quaternaryCODEst.CONTENT_TAG.ContentTagText != null)
                            {
                                temp = model.quaternaryCODEst.CONTENT_TAG.ContentTagText.Substring(model.quaternaryCODEst.CONTENT_TAG.ContentTagText.IndexOf(')') + 2);
                                model.quaternaryCODEst.CONTENT_TAG = db.CONTENT_TAG.Where(x => x.ContentTagText == temp).FirstOrDefault();
                                model.quaternaryCODEst.ProminenceID = db.COD_PROMINENCE.Where(x => x.ProminenceValue == "Quaternary").FirstOrDefault().ProminenceID;
                                COD.Add(model.quaternaryCODEst);
                            }
                        }
                    }
                }
                if (!(COD.Count < 1))
                {
                    //   
                    FORENSIC_CASE FC = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.primaryCODEst.ForensicCaseID).FirstOrDefault();
                    try
                    {

                        FC.CASE_COD_ESTIMATION.Clear();
                        FC.CASE_COD_ESTIMATION = COD;
                        db.FORENSIC_CASE.Attach(FC);
                        db.Entry(FC).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        
                    }
                    
                }

                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Cause Of Death");
                #endregion
                return RedirectToAction("UpdateStatistics", new { id = model.primaryCODEst.ForensicCaseID });
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Forensic Case - Cause Of Death");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Service Requests Section")]
        public ActionResult ForensicCaseServiceRequests(int id)
        {
            ViewBag.TargetDR = db.FORENSIC_CASE.Where(fc => fc.ForensicCaseID == id).FirstOrDefault().ForensicDRNumber;
            ViewBag.TargetID = id;
            ViewBag.Instruction = "UPDATE";
            return View(db.SERVICE_REQUEST.Where(sr => sr.ForensicCaseID == id));
        }
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Service Requests Section")]
        public ActionResult ForensicCaseMediaItems(int id)
        {
            ViewBag.TargetDR = db.FORENSIC_CASE.Where(fc => fc.ForensicCaseID == id).FirstOrDefault().ForensicDRNumber;
            ViewBag.TargetID = id;
            ViewBag.Instruction = "UPDATE";
            return View(db.MEDIA.Where(m => m.ForensicCaseID == id));
        }
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Service Requests Section")]
        public ActionResult SelectNewMediaItems(int id)
        {
            ViewBag.FCID = id;
            ViewBag.Instruction = "UPDATE";
            return View("SelectMediaItems");
        }
        public ActionResult CaptureNewMediaItems(int id)
        {
            id = id * (-1);
            ViewBag.Instruction = "UPDATE";
            return RedirectToAction("CaptureMediaItemDetails",new { id = id});
        }
        //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Service Requests Section")]
        public ActionResult ForensicCaseAdditionalEvidenceItems(int id)
        {
            ViewBag.TargetDR = db.FORENSIC_CASE.Where(fc => fc.ForensicCaseID == id).FirstOrDefault().ForensicDRNumber;
            ViewBag.TargetID = id;
            ViewBag.Instruction = "UPDATE";
            return View(db.ADDITIONAL_EVIDENCE.Where(ae => ae.ForensicCaseID == id));
        }
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Statistics Section")]
        public ActionResult ForensicCaseUpdatedSuccess()
        {
            string actionName = "ForensicCaseUpdatedSuccess";
            try
            {
                return View();
            }
            catch (Exception x)
            {
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
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
