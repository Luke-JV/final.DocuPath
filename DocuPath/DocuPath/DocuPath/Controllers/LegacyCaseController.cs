using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class LegacyCaseController : Controller
    {
        // GET: LegacyCase
        public ActionResult Index()
        {
            return View();
        }

        // GET: LegacyCase/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
