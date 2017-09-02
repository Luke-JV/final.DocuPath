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
    [HandleError]
    [LogAction]
    public class ServiceProviderController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea = "Search Service Provider")]
        public ActionResult Index()
        {
            try
            {

                return RedirectToAction("All");

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        //----------------------------------------------------------------------------------------------//

        #region CREATES:
        [AuthorizeByAccessArea(AccessArea = "Add Service Provider")]
        public ActionResult Add()
        {
            try
            {

                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion

                ServiceProviderViewModel model = new ServiceProviderViewModel();
                model.serviceProvider = new SERVICE_PROVIDER();
                model.titles = db.TITLE.ToList();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
       
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add Service Provider")]
        public ActionResult Add(ServiceProviderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                db.SERVICE_PROVIDER.Add(model.serviceProvider);
                try
                {
                    model.serviceProvider.ServiceProviderID = db.SERVICE_PROVIDER.Max(x => x.ServiceProviderID) + 1;
                }
                catch (Exception)
                {
                    model.serviceProvider.ServiceProviderID = 0;
                }
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
        [AuthorizeByAccessArea(AccessArea = "Search Service Provider")]
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

        [AuthorizeByAccessArea(AccessArea = "View Service Provider")]
        public ActionResult Details(int id)
        {
            try
            {

                #region MODEL POPULATION
                SERVICE_PROVIDER model = new SERVICE_PROVIDER();
                model = db.SERVICE_PROVIDER.Where(x => x.ServiceProviderID == id).FirstOrDefault();
                model.TITLE = db.TITLE.Where(x => x.TitleID == model.TitleID).FirstOrDefault();
                #endregion

                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region UPDATES:        
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Service Provider")]
        public ActionResult Edit(int id)
        {
            try
            {

                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit Service Provider")]
        public ActionResult Edit(int id, SERVICE_PROVIDER updatedSP)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedSP);
            }

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
        [AuthorizeByAccessArea(AccessArea = "Delete Service Provider")]
        public ActionResult Delete(int id)
        {
            try
            {

                //404 CONFIRM
                db.SERVICE_PROVIDER.Where(x => x.ServiceProviderID == id).FirstOrDefault().IsDeactivated = true;
                db.SaveChanges();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Delete Service Provider")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(collection);
            }

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
