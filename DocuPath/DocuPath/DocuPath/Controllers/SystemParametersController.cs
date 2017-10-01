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
                //model.allocationstatus
                //foreach (var item in model.allocationStatuses)
                //{
                //    if (!db.ALLOCATION_STATUS.Contains(item))
                //    {
                //        db.ALLOCATION_STATUS.Add(item);
                //    }
                
                //}

                //model.apparentMOD
                List<APPARENT_MANNER_DEATH> temp = db.APPARENT_MANNER_DEATH.ToList();
                foreach (var item in model.apparentMOD)
                {
                    if (!temp.Contains(item))
                    {
                        db.APPARENT_MANNER_DEATH.Add(item);
                    }
                }
                //model.autopsyAreas
                foreach (var item in model.autopsyAreas)
                {
                    if (!db.AUTOPSY_AREA.Contains(item))
                    {
                        db.AUTOPSY_AREA.Add(item);
                    }

                }
                //model.autopsyTypes
                foreach (var item in model.autopsyTypes)
                {
                    if (!db.AUTOPSY_TYPE.Contains(item))
                    {
                        db.AUTOPSY_TYPE.Add(item);
                    }

                }
                //model.dutySlots
                foreach (var item in model.dutySlots)
                {
                    if (!db.SLOT.Contains(item))
                    {
                        db.SLOT.Add(item);
                    }

                }
                //model.externalCauses
                foreach (var item in model.externalCauses)
                {
                    if (!db.EXTERNAL_CAUSE.Contains(item))
                    {
                        db.EXTERNAL_CAUSE.Add(item);
                    }

                }
                //model.genders
                foreach (var item in model.genders)
                {
                    if (!db.INDIVIDUAL_GENDER.Contains(item))
                    {
                        db.INDIVIDUAL_GENDER.Add(item);
                    }

                }
                //                model.hospitalsClinics
                foreach (var item in model.hospitalsClinics)
                {
                    if (!db.HOSPITAL_CLINIC.Contains(item))
                    {
                        db.HOSPITAL_CLINIC.Add(item);
                    }

                }
                //model.mediaPurposes
                foreach (var item in model.mediaPurposes)
                {
                    if (!db.MEDIA_PURPOSE.Contains(item))
                    {
                        db.MEDIA_PURPOSE.Add(item);
                    }

                }
                //model.medicalTreatments
                foreach (var item in model.medicalTreatments)
                {
                    if (!db.MEDICAL_TREATMENTS.Contains(item))
                    {
                        db.MEDICAL_TREATMENTS.Add(item);
                    }

                }
                //model.primaryCOD
                foreach (var item in model.primaryCOD)
                {
                    if (!db.PRIMARY_CAUSE_DEATH.Contains(item))
                    {
                        db.PRIMARY_CAUSE_DEATH.Add(item);
                    }

                }
                //model.provinces
                foreach (var item in model.provinces)
                {
                    if (!db.PROVINCE.Contains(item))
                    {
                        db.PROVINCE.Add(item);
                    }

                }
                //model.races
                foreach (var item in model.races)
                {
                    if (!db.INDIVIDUAL_RACE.Contains(item))
                    {
                        db.INDIVIDUAL_RACE.Add(item);
                    }

                }
                //model.requestTypes
                foreach (var item in model.requestTypes)
                {
                    if (!db.REQUEST_TYPE.Contains(item))
                    {
                        db.REQUEST_TYPE.Add(item);
                    }

                }
                //model.samplesInvestigations
                foreach (var item in model.samplesInvestigations)
                {
                    if (!db.SAMPLE_INVESTIGATION.Contains(item))
                    {
                        db.SAMPLE_INVESTIGATION.Add(item);
                    }

                }
                //model.scenesOfInjury
                foreach (var item in model.scenesOfInjury)
                {
                    if (!db.SCENE_OF_INJURY.Contains(item))
                    {
                        db.SCENE_OF_INJURY.Add(item);
                    }

                }
                //model.specialCategories
                foreach (var item in model.specialCategories)
                {
                    if (!db.SPECIAL_CATEGORY.Contains(item))
                    {
                        db.SPECIAL_CATEGORY.Add(item);
                    }

                }
                ////model.statuses
                //foreach (var item in model.statuses)
                //{
                //    if (!db.STATUS.Contains(item))
                //    {
                //        db.STATUS.Add(item);
                //    }

                //}
                //model.titles
                foreach (var item in model.titles)
                {
                    if (!db.TITLE.Contains(item))
                    {
                        db.TITLE.Add(item);
                    }

                }


                //db.Entry(updatedUser).State = EntityState.Modified;
                db.SaveChanges();
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
