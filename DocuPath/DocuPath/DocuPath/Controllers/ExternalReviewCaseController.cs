﻿using DocuPath.DataLayer;
using DocuPath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class ExternalReviewCaseController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: ExternalReviewCase
        public ActionResult Index()
        {
            //404 - redirect to /All
            return View();
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: ExternalReviewCase/Create
        public ActionResult Create()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ExternalReviewCase/Create
        [HttpPost]
        public ActionResult Create(EXTERNAL_REVIEW_CASE ERC)
        {
            try
            {
                db.EXTERNAL_REVIEW_CASE.Add(ERC);
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
        // GET: ExternalReviewCase/All
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.EXTERNAL_REVIEW_CASE.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        // GET: ExternalReviewCase/Details/5
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            EXTERNAL_REVIEW_CASE model = new EXTERNAL_REVIEW_CASE();
            model = db.EXTERNAL_REVIEW_CASE.Where(x => x.ExternalReviewCaseID == id).FirstOrDefault();
            #endregion
            
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion

            return View(model);
        }

        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: ExternalReviewCase/Edit/5
        public ActionResult Edit(int id)
        {
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ExternalReviewCase/Edit/5
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
        // GET: ExternalReviewCase/Delete/5
        public ActionResult Delete(int id)
        {
           
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ExternalReviewCase/Delete/5
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
