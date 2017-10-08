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
    [LogAction]
    public class AccessLevelController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        string controllerName = "AccessLevel";

        [AuthorizeByAccessArea(AccessArea = "Search User Access Level")]
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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:

        [AuthorizeByAccessArea(AccessArea = "Add User Access Level")]
        public ActionResult Add()
        {
            string actionName = "Add";
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit,"Access Level");
                #endregion

                AccessLevelViewModel model = new AccessLevelViewModel();
                model.accessLevel = new ACCESS_LEVEL();
                model.allAreas = db.ACCESS_AREA.ToList();
                model.fxGroups = db.FUNCTION_GROUP.ToList();

                model.FCAreas = new List<selectAreaKVP>();
                model.ECAreas = new List<selectAreaKVP>();
                model.LCAreas = new List<selectAreaKVP>();
                model.MediaAreas = new List<selectAreaKVP>();
                model.InsightAreas = new List<selectAreaKVP>();
                model.VisionAreas = new List<selectAreaKVP>();
                model.SPAreas = new List<selectAreaKVP>();
                model.SRAreas = new List<selectAreaKVP>();
                model.SchedulingAreas = new List<selectAreaKVP>();
                model.UserAreas = new List<selectAreaKVP>();
                model.AuditAreas = new List<selectAreaKVP>();
                model.AccessLevelAreas = new List<selectAreaKVP>();
                model.ContentTagAreas = new List<selectAreaKVP>();

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > Forensic Case"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.FCAreas.Add(area);
                }


                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > External Review Case"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.ECAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > Legacy Case"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.LCAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORNEA > Media Repository"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.MediaAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORNEA > Insight Reporting"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.InsightAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORNEA > Vision Dashboard"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.VisionAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Service Providers"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.SPAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Service Requests"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.SRAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Scheduling"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.SchedulingAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Users"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.UserAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Audit Trail"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.AuditAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Access Levels"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.AccessLevelAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Content Tags"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.ContentTagAreas.Add(area);
                }



                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add User Access Level")]
        public ActionResult Add(AccessLevelViewModel model)
        {
            string actionName = "Add";
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Access Level");
                #endregion
                return View(model);
            }

            try
            {
                ACCESS_LEVEL level = new ACCESS_LEVEL();
                level.LevelName = model.accessLevel.LevelName;
                try
                {
                    level.AccessLevelID = db.ACCESS_LEVEL.Max(x => x.AccessLevelID) + 1;
                }
                catch (Exception)
                {
                    level.AccessLevelID = 1;
                }

                List<LEVEL_AREA> areas = new List<LEVEL_AREA>();

                //FC
                foreach (var item in model.FCAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //EC
                foreach (var item in model.ECAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //LC
                foreach (var item in model.LCAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //Media
                foreach (var item in model.MediaAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //Insight
                foreach (var item in model.InsightAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //Vision
                foreach (var item in model.VisionAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //SP
                foreach (var item in model.SPAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //SR
                foreach (var item in model.SRAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //Scheduling
                foreach (var item in model.SchedulingAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //User
                foreach (var item in model.UserAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //Audit
                foreach (var item in model.AuditAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //AL
                foreach (var item in model.AccessLevelAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                //CT
                foreach (var item in model.ContentTagAreas)
                {
                    if (item.hasAccess)
                    {
                        LEVEL_AREA area = new LEVEL_AREA();
                        area.AccessLevelID = level.AccessLevelID;
                        area.AccessAreaID = db.ACCESS_AREA.Where(x => x.AccessAreaDescription == item.areaName).FirstOrDefault().AccessAreaID;
                        areas.Add(area);
                    }
                }

                level.LEVEL_AREA = areas;

                db.ACCESS_LEVEL.Add(level);
                db.SaveChanges();
                
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddSuccess, "Access Level");
                #endregion
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:

        [AuthorizeByAccessArea(AccessArea = "Search User Access Level")]
        public ActionResult All()
        {
            string actionName = "All";

            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Access Level");
                #endregion
                return View(db.ACCESS_LEVEL.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }


        [AuthorizeByAccessArea(AccessArea = "View User Access Level")]
        public ActionResult Details(int id)
        {
            string actionName = "Details";

            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "Access Level");
                #endregion

                #region MODEL POPULATION
                ACCESS_LEVEL model = db.ACCESS_LEVEL.Where(x => x.AccessLevelID == id).FirstOrDefault();
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "Access Level");
                #endregion

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:

        [AuthorizeByAccessArea(AccessArea = "Update/Edit User Access Level")]
        public ActionResult Edit(int id)
        {
            string actionName = "Edit";

            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Access Level");
                #endregion
                AccessLevelViewModel model = new AccessLevelViewModel();
                model.accessLevel = db.ACCESS_LEVEL.Where(ual => ual.AccessLevelID == id).FirstOrDefault();
                model.allAreas = db.ACCESS_AREA.ToList();
                model.fxGroups = db.FUNCTION_GROUP.ToList();

                model.FCAreas = new List<selectAreaKVP>();
                model.ECAreas = new List<selectAreaKVP>();
                model.LCAreas = new List<selectAreaKVP>();
                model.MediaAreas = new List<selectAreaKVP>();
                model.InsightAreas = new List<selectAreaKVP>();
                model.VisionAreas = new List<selectAreaKVP>();
                model.SPAreas = new List<selectAreaKVP>();
                model.SRAreas = new List<selectAreaKVP>();
                model.SchedulingAreas = new List<selectAreaKVP>();
                model.UserAreas = new List<selectAreaKVP>();
                model.AuditAreas = new List<selectAreaKVP>();
                model.AccessLevelAreas = new List<selectAreaKVP>();
                model.ContentTagAreas = new List<selectAreaKVP>();

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > Forensic Case"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.FCAreas.Add(area);
                }


                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > External Review Case"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.ECAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORTEX > Legacy Case"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.LCAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORNEA > Media Repository"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.MediaAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORNEA > Insight Reporting"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.InsightAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "CORNEA > Vision Dashboard"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.VisionAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Service Providers"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.SPAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Service Requests"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.SRAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Scheduling"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.SchedulingAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Users"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.UserAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Audit Trail"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.AuditAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Access Levels"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.AccessLevelAreas.Add(area);
                }

                foreach (var item in db.ACCESS_AREA.Where(x => x.FUNCTION_GROUP.FunctionGroupDescription == "PULSE > Content Tags"))
                {
                    selectAreaKVP area = new selectAreaKVP();
                    area.areaName = item.AccessAreaDescription;
                    area.hasAccess = false;
                    model.ContentTagAreas.Add(area);
                }

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        // POST: AccessLevel/Edit/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User Access Level")]
        public ActionResult Edit(int id, ACCESS_LEVEL updatedLevel)
        {
            string actionName = "Edit";

            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Access Level");
                #endregion
                return View(updatedLevel);
            }

            try
            {
                #region DB UPDATE
                db.ACCESS_LEVEL.Attach(updatedLevel);
                db.Entry(updatedLevel).State = EntityState.Modified;
                db.SaveChanges();
                #endregion
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "Access Level");
                #endregion
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:

        [AuthorizeByAccessArea(AccessArea = "Deactivate User Access Level")]
        public ActionResult Delete(int id)
        {
            string actionName = "Delete";

            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "Access Level");
                #endregion
                //404 CONFIRM
                db.ACCESS_LEVEL.Where(x => x.AccessLevelID == id).FirstOrDefault().IsDeactivated = true;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "Access Level");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }


        [AuthorizeByAccessArea(AccessArea = "Deactivate User Access Level")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string actionName = "Delete";

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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "Access Level");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:

        public ActionResult test()
        {
            string actionName = "test";

            try
            {

                List<SelectListItem> selectFCFunctions = new List<SelectListItem>();
                List<SelectListItem> selectERCFunctions = new List<SelectListItem>();
                List<SelectListItem> selectLCFunctions = new List<SelectListItem>();
                List<SelectListItem> selectMediaFunctions = new List<SelectListItem>();
                List<SelectListItem> selectINSIGHTFunctions = new List<SelectListItem>();
                List<SelectListItem> selectVISIONFunctions = new List<SelectListItem>();
                List<SelectListItem> selectSPFunctions = new List<SelectListItem>();
                List<SelectListItem> selectSRFunctions = new List<SelectListItem>();
                List<SelectListItem> selectSchedulingFunctions = new List<SelectListItem>();
                List<SelectListItem> selectUserFunctions = new List<SelectListItem>();
                List<SelectListItem> selectAuditTrailFunctions = new List<SelectListItem>();
                List<SelectListItem> selectUALFunctions = new List<SelectListItem>();
                List<SelectListItem> selectCTFunctions = new List<SelectListItem>();

                foreach (var item in db.ACCESS_AREA)
                {
                    if (item.AccessAreaID >= 0 && item.AccessAreaID <= 29)
                    {
                        selectFCFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 30 && item.AccessAreaID <= 36)
                    {
                        selectERCFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 37 && item.AccessAreaID <= 41)
                    {
                        selectMediaFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 42 && item.AccessAreaID <= 46)
                    {
                        selectSPFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 47 && item.AccessAreaID <= 51)
                    {
                        selectSRFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 52 && item.AccessAreaID <= 57)
                    {
                        selectSchedulingFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 58 && item.AccessAreaID <= 62)
                    {
                        selectLCFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 63 && item.AccessAreaID <= 71)
                    {
                        selectINSIGHTFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID == 72)
                    {
                        selectVISIONFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 73 && item.AccessAreaID <= 79)
                    {
                        selectUserFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 80 && item.AccessAreaID <= 81)
                    {
                        selectAuditTrailFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 82 && item.AccessAreaID <= 86)
                    {
                        selectUALFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID >= 87 && item.AccessAreaID <= 91)
                    {
                        selectCTFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                    else if (item.AccessAreaID == 92)
                    {
                        selectUserFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                    }
                }

                ViewBag.FC = selectFCFunctions;
                ViewBag.ERC = selectERCFunctions;
                ViewBag.LC = selectLCFunctions;
                ViewBag.Media = selectMediaFunctions;
                ViewBag.INSIGHT = selectINSIGHTFunctions;
                ViewBag.VISION = selectVISIONFunctions;
                ViewBag.SP = selectSPFunctions;
                ViewBag.SR = selectSRFunctions;
                ViewBag.Scheduling = selectSchedulingFunctions;
                ViewBag.User = selectUserFunctions;
                ViewBag.AuditTrail = selectAuditTrailFunctions;
                ViewBag.UAL = selectUALFunctions;
                ViewBag.CT = selectCTFunctions;

                return View();
            }
            catch (Exception x)
            {
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        #endregion
    }
}
