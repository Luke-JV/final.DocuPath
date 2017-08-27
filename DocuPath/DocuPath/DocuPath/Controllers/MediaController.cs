using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.Custom_Classes;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class MediaController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: Media
        public ActionResult Index()
        {
            //404 - redirect to /all
            return View();
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: Media/Create
        public ActionResult Create()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: Media/Create
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
        // GET: Media/All
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.MEDIA.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        // GET: Media/Details/5
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            MediaViewModel model = new MediaViewModel();
            model.media = db.MEDIA.Where(x => x.MediaID == id).FirstOrDefault();
            model.media.USER = db.USER.Where(x => x.UserID == model.media.UserID).FirstOrDefault();
            model.media.MEDIA_PURPOSE = db.MEDIA_PURPOSE.Where(x => x.MediaPurposeID == model.media.MediaPurposeID).FirstOrDefault();
            foreach (var tag in db.MEDIA_TAG)
            {
                if (tag.MediaID == model.media.MediaID)
                {
                    model.tags.Add(db.CONTENT_TAG.Where(x=>x.ContentTagID == tag.ContentTagID).FirstOrDefault());
                }
            }
            #endregion
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: Media/Edit/5
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

        // POST: Media/Edit/5
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
        // GET: Media/Delete/5
        public ActionResult Delete(int id)
        {
            #region VALIDATE_ACCESS
            bool access = VECTOR.ValidateAccess(/*model.userID - 404*/0);
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: Media/Delete/5
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
