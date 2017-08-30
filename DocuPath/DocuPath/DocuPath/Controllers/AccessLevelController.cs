using DocuPath.DataLayer;
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
    [HandleError]
    public class AccessLevelController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea = "Search User Access Level")]
        public ActionResult Index()
        {

            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        
        [AuthorizeByAccessArea(AccessArea = "Add User Access Level")]
        public ActionResult Create()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add User Access Level")]
        public ActionResult Create(ACCESS_LEVEL AL)
        {
            try
            {
                db.ACCESS_LEVEL.Add(AL);
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
        
        [AuthorizeByAccessArea(AccessArea = "Search User Access Level")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.ACCESS_LEVEL.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        
        [AuthorizeByAccessArea(AccessArea = "View User Access Level")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            ACCESS_LEVEL model = db.ACCESS_LEVEL.Where(x => x.AccessLevelID == id).FirstOrDefault();

            #endregion
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User Access Level")]
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");,
            #endregion
            return View();
        }

        // POST: AccessLevel/Edit/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User Access Level")]
        public ActionResult Edit(int id, ACCESS_LEVEL updatedLevel)
        {
            try
            {
                #region DB UPDATE
                db.ACCESS_LEVEL.Attach(updatedLevel);
                db.Entry(updatedLevel).State = EntityState.Modified;
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
        
        [AuthorizeByAccessArea(AccessArea = "Deactivate User Access Level")]
        public ActionResult Delete(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        
        [AuthorizeByAccessArea(AccessArea = "Deactivate User Access Level")]
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
