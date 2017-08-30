using DocuPath.Models;
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
    public class ServiceRequestController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: ServiceRequest
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Index()
        {
            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: ServiceRequest/Create
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Create()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ServiceRequest/Create
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Create(SERVICE_REQUEST SR)
        {
            try
            {
                db.SERVICE_REQUEST.Add(SR);
                db.SaveChanges();
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
        // GET: ServiceRequest/All
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.SERVICE_REQUEST.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        // GET: ServiceRequest/Details/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            SERVICE_REQUEST model = new SERVICE_REQUEST();
            model = db.SERVICE_REQUEST.Where(x => x.ServiceRequestID == id).FirstOrDefault();
            model.SPECIMEN = db.SPECIMEN.Where(x => x.ServiceRequestID == model.ServiceRequestID).ToList();
            model.FORENSIC_CASE = db.FORENSIC_CASE.Where(x => x.ForensicCaseID == model.ForensicCaseID).FirstOrDefault();
            #endregion
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: ServiceRequest/Edit/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ServiceRequest/Edit/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id, SERVICE_REQUEST updatedSR)
        {
            try
            {
                #region DB UPDATE
                db.SERVICE_REQUEST.Attach(updatedSR);
                db.Entry(updatedSR).State = EntityState.Modified;
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
        // GET: ServiceRequest/Delete/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ServiceRequest/Delete/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
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
