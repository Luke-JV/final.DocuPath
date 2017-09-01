using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class UserController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Search User - All Profiles")]
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

        #region READS:
        [AuthorizeByAccessArea(AccessArea = "Search User - All Profiles")]
        public ActionResult All()
        {
            try
            {

                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();

                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.USER.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "View User - All Profiles")]
        public ActionResult Details(int id)
        {
            try
            {
                #region MODEL POPULATION
                USER model = new USER();
                model = db.USER.Where(x => x.UserID == id).FirstOrDefault();

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
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User - Own Profile")]
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

                return RedirectToAction("Error", "Home"); ;
            };
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User - Own Profile")]
        public ActionResult Edit(int id, USER updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            try
            {
                #region DB UPDATE
                db.USER.Attach(updatedUser);
                db.Entry(updatedUser).State = EntityState.Modified;
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
        [AuthorizeByAccessArea(AccessArea = "Deactivate User - Any Profile")]
        public ActionResult Delete(int id)
        {
            try
            {
                //404 CONFIRM
                db.USER.Where(x => x.UserID == id).FirstOrDefault().IsDeactivated = true;
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
        [AuthorizeByAccessArea(AccessArea = "Deactivate User - Any Profile")]
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
        public ActionResult GenerateTokens()
        {
            try
            {

                TokenViewModel model = new TokenViewModel();

                return View(model);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public ActionResult GenerateTokens(FormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }


                return null;
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult GenerateTokens(TokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion


                model.tokenList = new List<TOKEN_LOG>();
                for (int i = 0; i < model.tokenCount; i++)
                {
                    //gen token
                    //model.tokenList.Add(tk);
                    TOKEN_LOG x = new TOKEN_LOG();

                    x.IssueTimestamp = DateTime.Now;
                    x.TokenValue = "xxxxx";
                    model.tokenList.Add(x);
                }
                model.ualList = db.ACCESS_LEVEL.ToList();
                model.tokenCount = model.tokenCount;

                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        #endregion
    }
}
