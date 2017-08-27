using DocuPath.DataLayer;
using DocuPath.Models;
using System;
using System.Collections.Generic;
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
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion

            return View();
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: ForensicCase/Edit/5
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ForensicCase/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
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
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
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
