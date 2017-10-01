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
                return RedirectToAction("AddObservations",new {id=inCase.ForensicCaseID });
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
                return RedirectToAction("SelectMediaItems", new { id=upCase.ForensicCaseID });
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
                return RedirectToAction("SelectMediaItems");
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
                foreach (var item in model.mediaList)
                {
                    var input = db.MEDIA.Where(x => x.MediaID == item.MediaID).FirstOrDefault();
                    input.MediaDescription = item.MediaDescription;
                    input.MediaCaption = item.MediaCaption;
                    input.MediaPurposeID = item.MediaPurposeID;
                    input.StatusID = db.STATUS.Where(x => x.StatusValue == "Active").FirstOrDefault().StatusID;
                    if (item.ForensicCaseID != null)
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
        public ActionResult SelectAdditionalEvidenceItems()
        {
            string actionName = "SelectAdditionalEvidenceItems";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case -Additional Evidence");
                #endregion

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

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Add Forensic Case - Additional Evidence Section")]
        public ActionResult UploadAdditionalEvidenceItems()
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
                        AE.ContactPersonContactNum = "";
                        AE.ContactPersonNameSurname = "PENDING";
                        AE.EvidenceDescription = "PENDING";
                        AE.EvidenceSerialNumber = "";
                        // CHECK THIS FCID PLEASE, NOT SURE I BRAINED IT CORRECTLY - NOT SIMPLY THE LAST FCID IN FORENSIC_CASE, BUT THE LAST FCID ADDED BY THE CURRENT USER!
                        AE.ForensicCaseID = db.FORENSIC_CASE.Where(fc => fc.UserID == uId).Max(fc => fc.ForensicCaseID);

                        AEID++;

                        db.ADDITIONAL_EVIDENCE.Add(AE);
                    }

                    db.SaveChanges();
                    #region AUDIT_WRITE
                    AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UploadSuccess, "Additional Evidence");
                    #endregion
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
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
        public ActionResult CaptureAdditionalEvidenceDetails()
        {
            string actionName = "CaptureAdditionalEvidenceDetails";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Forensic Case - Additional Evidence");
                #endregion
                #region PREPARE MODEL
                AdditionalEvidenceViewModel model = new AdditionalEvidenceViewModel();

                int id = VERTEBRAE.getCurrentUser().UserID;
                model.additionalEvidence = db.ADDITIONAL_EVIDENCE.Where(ae => ae.FORENSIC_CASE.UserID == id && ae.EvidenceDescription.ToUpper() == "PENDING").ToList();
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
                return RedirectToAction("Index");
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
        public ActionResult AddCauseOfDeath()
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
                model.primaryCODEst.ForensicCaseID = db.FORENSIC_CASE.Where(fc => fc.UserID == uId).Max(fc => fc.ForensicCaseID);
                model.primaryCODEst.ProminenceID = 1;

                model.secondaryCODEst = new CASE_COD_ESTIMATION();
                model.secondaryCODEst.ForensicCaseID = db.FORENSIC_CASE.Where(fc => fc.UserID == uId).Max(fc => fc.ForensicCaseID);
                model.secondaryCODEst.ProminenceID = 2;

                model.tertiaryCODEst = new CASE_COD_ESTIMATION();
                model.tertiaryCODEst.ForensicCaseID = db.FORENSIC_CASE.Where(fc => fc.UserID == uId).Max(fc => fc.ForensicCaseID);
                model.tertiaryCODEst.ProminenceID = 3;

                model.quaternaryCODEst = new CASE_COD_ESTIMATION();
                model.quaternaryCODEst.ForensicCaseID = db.FORENSIC_CASE.Where(fc => fc.UserID == uId).Max(fc => fc.ForensicCaseID);
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
        public ActionResult AddCauseOfDeath(CODViewModel model)
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
                //TODO: DB Logic
                //db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Forensic Case - Cause Of Death");
                #endregion
                return RedirectToAction("AddStatistics");
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
        public ActionResult AddStatistics()
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
                //TODO: DB Logic
                //db.SaveChanges();
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
                return View("Error",new HandleErrorInfo(x, controllerName, actionName));
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

                if (modelCase.CASE_STATISTICS.Count <1)
                {
                    modelStats = new CASE_STATISTICS();
                }
                else
                {
                    modelStats = modelCase.CASE_STATISTICS.FirstOrDefault();
                }

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

        // TOTO: Missing Migrate POST    
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        public ActionResult ForensicCaseUpdateHub(int id)
        {
            ViewBag.TargetDR = db.FORENSIC_CASE.Where(fc => fc.ForensicCaseID == id).FirstOrDefault().ForensicDRNumber;
            ViewBag.TargetBriefDesc = db.FORENSIC_CASE.Where(fc => fc.ForensicCaseID == id).FirstOrDefault().FCBriefDescription;
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
        public ActionResult UpdateCoreData(int FCID)
        {
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Core Data Section")]
        public ActionResult UpdateCoreData(CoreDataViewModel model)
        {
            return View();
        }
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Observations Section")]
        public ActionResult UpdateObservations(int FCID)
        {
            return View();
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Observations Section")]
        public ActionResult UpdateObservations(ObservationsViewModel model)
        {
            return View();
        }
    //>>>>>>>>>>>>>>>>>>>>>>
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - All Sections")]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Forensic Case - Service Requests Section")]
        public ActionResult ForensicCaseServiceRequests(int FCID)
        {
            return View(db.SERVICE_REQUEST.Where(sr => sr.ForensicCaseID == FCID));
        }
    //>>>>>>>>>>>>>>>>>>>>>>
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
