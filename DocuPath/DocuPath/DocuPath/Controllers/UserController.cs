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
   // [LogAction]
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
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "User Management");
                #endregion
                UserViewModel model = new UserViewModel();
                model.users = db.USER.ToList();
                //ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchSuccess, "User Management");
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "User Management");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        [HttpPost]
        public ActionResult All(UserViewModel model)
        {
            return RedirectToAction("GenerateTokens",new { id = model.tkCount+1 });
        }
        [AuthorizeByAccessArea(AccessArea = "View User - All Profiles")]
        public ActionResult Details(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "User Management");
                #endregion
                #region MODEL POPULATION
                USER model = new USER();
                model = db.USER.Where(x => x.UserID == id).FirstOrDefault();

                #endregion

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewSuccess, "User Management");
                #endregion
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "User Management");
                #endregion
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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "User Management");
                #endregion
                return View();


            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "User Management");
                #endregion
                return RedirectToAction("Error", "Home"); ;
            };
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User - Own Profile")]
        public ActionResult Edit(int id, USER updatedUser)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "User Management");
                #endregion
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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "User Management");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "User Management");
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
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteInit, "User Management");
                #endregion
                //404 CONFIRM
                db.USER.Where(x => x.UserID == id).FirstOrDefault().IsDeactivated = true;
                db.SaveChanges();
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteSuccess, "User Management");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.DeleteFail, "User Management");
                #endregion
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
        public ActionResult GenerateTokens(int id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.GenerateTokensInit, "User Management");
                #endregion
                TokenViewModel model = new TokenViewModel();
                model.tokenCount = id;
                model.tokenList = new List<TOKEN_LOG>();
                
                return View(model);
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.GenerateTokensFail, "User Management");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }
        

        [HttpPost]
        public ActionResult GenerateTokens(TokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.GenerateTokensFail, "User Management");
                #endregion
                return View(model);
            }

            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
               


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
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.GenerateTokensSuccess, "User Management");
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.GenerateTokensFail, "User Management");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }
        
        public ActionResult ViewProfile()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                var userID = VERTEBRAE.getCurrentUser().UserID;
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "User Management");
                #endregion

                return RedirectToAction("Details", "User", new { id = userID });
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "User Management");
                #endregion
                return RedirectToAction("Error", "Home", x); // TODO 404: send the Error page the ENTIRE exception, not just the message! Already modified the action to accept an Exception
            }
        }

        public ActionResult UpdateProfile()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                var userID = VERTEBRAE.getCurrentUser().UserID;
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "User Management");
                #endregion

                return RedirectToAction("Edit", "User", new { id = userID });
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "User Management");
                #endregion
                return RedirectToAction("Error", "Home", x);
            }
        }
        #endregion
    }
}
