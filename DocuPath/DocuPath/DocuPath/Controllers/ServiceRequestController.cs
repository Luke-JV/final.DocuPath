using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class ServiceRequestController : Controller
    {
        // GET: ServiceRequest
        public ActionResult Index()
        {
            return View();
        }

        // GET: ServiceRequest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServiceRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequest/Create
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

        // GET: ServiceRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServiceRequest/Edit/5
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

        // GET: ServiceRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServiceRequest/Delete/5
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
