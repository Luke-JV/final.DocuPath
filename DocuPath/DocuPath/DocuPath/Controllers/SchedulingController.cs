using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    public class SchedulingController : Controller
    {
        // GET: Scheduling
        public ActionResult Index()
        {
            return View();
        }

        // GET: Scheduling/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Scheduling/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scheduling/Create
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

        // GET: Scheduling/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Scheduling/Edit/5
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

        // GET: Scheduling/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Scheduling/Delete/5
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
