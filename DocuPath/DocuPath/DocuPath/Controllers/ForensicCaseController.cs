using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocuPath.DataLayer;
using DocuPath.Models;

namespace DocuPath.Controllers
{
    public class ForensicCaseController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: ForensicCase
        public ActionResult Index()
        {
            return View();
        }

        #region CREATES:
        // GET: ForensicCase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForensicCase/Create
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

        #region READS:
        // GET: ForensicCase/All
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                return View(db.FORENSIC_CASE.ToList());
            }
            catch (Exception x)
            {
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        // GET: ForensicCase/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region UPDATES:
        // GET: ForensicCase/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ForensicCase/Edit/5
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

        #region DELETES:
        // GET: ForensicCase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForensicCase/Delete/5
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

        #region NON-CRUD ACTIONS:

        #endregion






    }
}
