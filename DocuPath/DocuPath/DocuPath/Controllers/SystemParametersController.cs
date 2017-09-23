using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class SystemParametersController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        string controllerName = "SystemParameters";
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: SystemParameters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemParameters/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:
        [AuthorizeByAccessArea(AccessArea = "Maintain System Parameters")]
        public ActionResult Index()
        {
            string actionName = "Index";
            try
            {
                return RedirectToAction("Maintain");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "System Parameters");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Maintain System Parameters")]
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        [AuthorizeByAccessArea(AccessArea = "Maintain System Parameters")]
        public ActionResult Maintain()
        {
            string actionName = "Maintain";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "System Parameters");
                #endregion

                #region MODEL PREPARATION
                SystemParametersViewModel model = new SystemParametersViewModel();
                // SCHEDULING:
                model.allocationStatuses = db.ALLOCATION_STATUS.ToList();
                model.dutySlots = db.SLOT.ToList();
                // AUTOPSIES:
                model.autopsyAreas = db.AUTOPSY_AREA.ToList();
                model.autopsyTypes = db.AUTOPSY_TYPE.ToList();
                // STATISTICS:
                model.apparentMOD = db.APPARENT_MANNER_DEATH.ToList();
                model.externalCauses = db.EXTERNAL_CAUSE.ToList();
                model.hospitalsClinics = db.HOSPITAL_CLINIC.ToList();
                model.genders = db.INDIVIDUAL_GENDER.ToList();
                model.races = db.INDIVIDUAL_RACE.ToList();
                model.primaryCOD = db.PRIMARY_CAUSE_DEATH.ToList();
                model.medicalTreatments = db.MEDICAL_TREATMENTS.ToList();
                model.provinces = db.PROVINCE.ToList();
                model.samplesInvestigations = db.SAMPLE_INVESTIGATION.ToList();
                model.scenesOfInjury = db.SCENE_OF_INJURY.ToList();
                model.specialCategories = db.SPECIAL_CATEGORY.ToList();
                // STATUSES & TYPES:
                model.mediaPurposes = db.MEDIA_PURPOSE.ToList();
                model.requestTypes = db.REQUEST_TYPE.ToList();
                model.statuses = db.STATUS.ToList();
                model.titles = db.TITLE.ToList();
                #endregion

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "System Parameters");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            };
        }

        [HttpPost]
        public ActionResult Maintain(SystemParametersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "System Parameters");
                #endregion
                return View(model);
            }

            try
            {
                #region DB UPDATE
                // 404 for params update logic
                //db.USER.Attach(updatedUser);
                //db.Entry(updatedUser).State = EntityState.Modified;
                //db.SaveChanges();
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "System Parameters");
                #endregion
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "System Parameters");
                #endregion
                return View();
            }

        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:
        // GET: SystemParameters/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SystemParameters/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:

        #endregion
    }
}
