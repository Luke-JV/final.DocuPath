﻿using DocuPath.DataLayer;
using DocuPath.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: User
        [AuthorizeByAccessArea(AccessArea ="404")]
        public ActionResult Index()
        {
            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: User/Create
        public ActionResult Create()
        {
            VERTEBRAE.sendMail("404","404");
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: User/Create
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
        // GET: User/All
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.USER.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        // GET: User/Details/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            USER model = new USER();
            model = db.USER.Where(x => x.UserID == id).FirstOrDefault();
            
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: User/Edit/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id, USER updatedUser)
        {
            try
            {
                #region DB UPDATE
                db.USER.Attach(updatedUser);
                db.Entry(updatedUser).State = EntityState.Modified;
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
        // GET: User/Delete/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: User/Delete/5
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
