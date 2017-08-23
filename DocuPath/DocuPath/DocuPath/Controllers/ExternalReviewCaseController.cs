using DocuPath.DataLayer;
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
            return View();
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: ExternalReviewCase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExternalReviewCase/Create
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
        // GET: ExternalReviewCase/All
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                return View(db.EXTERNAL_REVIEW_CASE.ToList());
            }
            catch (Exception x)
            {
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        // GET: ExternalReviewCase/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: ExternalReviewCase/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExternalReviewCase/Edit/5
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
        // GET: ExternalReviewCase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExternalReviewCase/Delete/5
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
