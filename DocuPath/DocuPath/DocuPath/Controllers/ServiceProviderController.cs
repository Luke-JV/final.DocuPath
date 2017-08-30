using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DocuPath.Models.DPViewModels;

namespace DocuPath.Controllers
{
    [Authorize]
    public class ServiceProviderController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        // GET: ServiceProvider
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Index()
        {
            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        // GET: ServiceProvider/Create
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Add()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion

            ServiceProviderViewModel model = new ServiceProviderViewModel();
            model.titles = db.TITLE.ToList();

            return View(model);
        }

        // POST: ServiceProvider/Create
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Add(SERVICE_PROVIDER SP)
        {
            try
            {
                db.SERVICE_PROVIDER.Add(SP);
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
        // GET: ServiceProvider/All
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.SERVICE_PROVIDER.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        // GET: ServiceProvider/Details/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            SERVICE_PROVIDER model = new SERVICE_PROVIDER();
            model = db.SERVICE_PROVIDER.Where(x => x.ServiceProviderID == id).FirstOrDefault();
            model.TITLE = db.TITLE.Where(x=>x.TitleID == model.TitleID).FirstOrDefault();
            #endregion

            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        // GET: ServiceProvider/Edit/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ServiceProvider/Edit/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Edit(int id, SERVICE_PROVIDER updatedSP)
        {
            try
            {
                #region DB UPDATE
                db.SERVICE_PROVIDER.Attach(updatedSP);
                db.Entry(updatedSP).State = EntityState.Modified;
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
        // GET: ServiceProvider/Delete/5
        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Delete(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        // POST: ServiceProvider/Delete/5
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
