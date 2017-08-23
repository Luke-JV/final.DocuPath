using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class ReportingController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();

        // GET: Reporting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VISION()
        {
            ViewBag.VISIONMetrics = VERTEBRAE.GetVisionMetrics();

            //List<SelectListItem> selectSuperusers = new List<SelectListItem>();
            //List<SelectListItem> selectUsers = new List<SelectListItem>();
            //List<SelectListItem> selectActivityTypes = new List<SelectListItem>();

            //selectSuperusers.Add(new SelectListItem { Value = "0", Text = "Select a Superuser..." });
            //foreach (var item in db.USERs)
            //{
            //    if (item.USER_LOGIN.ACCESS_LEVEL.AccessLevelID == 1)
            //    {
            //        selectSuperusers.Add(new SelectListItem { Value = item.UserID.ToString(), Text = item.FirstName + " " + item.LastName });
            //    }
            //}
            //ViewBag.Superusers = selectSuperusers;

            //selectUsers.Add(new SelectListItem { Value = "0", Text = "Select a User..." });
            //foreach (var item in db.USERs)
            //{
            //    selectUsers.Add(new SelectListItem { Value = item.UserID.ToString(), Text = item.FirstName + " " + item.LastName });
            //}
            //ViewBag.Users = selectUsers;

            //selectActivityTypes.Add(new SelectListItem { Value = "0", Text = "Select an Activity Type..." });
            //selectActivityTypes.Add(new SelectListItem { Value = "1", Text = "All Activity Types" });
            //foreach (var item in db.AUDIT_TX_TYPE)
            //{
            //    selectActivityTypes.Add(new SelectListItem { Value = (item.AuditLogTxTypeID + 1).ToString(), Text = item.TypeValue });
            //}
            //ViewBag.ActivityTypes = selectActivityTypes;

            return View();
        }

        // GET: Reporting/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reporting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reporting/Create
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

        // GET: Reporting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reporting/Edit/5
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

        // GET: Reporting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reporting/Delete/5
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
