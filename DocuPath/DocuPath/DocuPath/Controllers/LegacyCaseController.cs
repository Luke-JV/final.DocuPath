using DocuPath.DataLayer;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class LegacyCaseController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: LegacyCase
        public ActionResult Index()
        {
            return View();
        }

        // GET: LegacyCase/Details/5
        public ActionResult Details(int id)
        {
            LegacyCaseViewModel model = new LegacyCaseViewModel();
            model.legacyCase = db.LEGACY_CASE.Where(x=>x.LegacyCaseID == id).FirstOrDefault();
            model.legacyCase.USER = db.USER.Where(x=>x.UserID == model.legacyCase.UserID).FirstOrDefault();
            
            model.legacyDocs = db.LEGACY_DOCUMENT.Where(x => x.LegacyCaseID == model.legacyCase.LegacyCaseID).ToList();
            return View(model);
        }

        // GET: LegacyCase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LegacyCase/Create
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

        // GET: LegacyCase/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LegacyCase/Edit/5
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

        // GET: LegacyCase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LegacyCase/Delete/5
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
    }
}
