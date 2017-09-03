﻿using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    //[LogAction]
    public class ServiceRequestController : Controller
    {
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
        public ActionResult Create()
        {
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Service Request");
                #endregion
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
        public ActionResult Create(SERVICE_REQUEST SR)
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
        public ActionResult Edit(int id)
        {
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "Service Request");
                #endregion
                return View();
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

        #endregion
    }
}
