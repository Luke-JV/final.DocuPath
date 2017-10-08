﻿using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using Microsoft.AspNet.Identity;
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
        string controllerName = "User";
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
            return RedirectToAction("GenerateTokens", new { id = model.tkCount + 1 });
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
            string actionName = "Edit";
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateInit, "My Profile");
                #endregion

                UpdateUserViewModel model = new UpdateUserViewModel();
                model.user = db.USER.Where(u => u.UserID == id).FirstOrDefault();
                model.titles = db.TITLE.ToList();


                List<UiPrefKVP> prefslist = new List<UiPrefKVP>();
                UiPrefKVP notset = new UiPrefKVP();
                notset.prefID = null;
                notset.prefPhrase = "Not Set";
                prefslist.Add(notset);
                UiPrefKVP light = new UiPrefKVP();
                light.prefID = 0;
                light.prefPhrase = "Light Theme";
                prefslist.Add(light);
                UiPrefKVP dark = new UiPrefKVP();
                dark.prefID = 1;
                dark.prefPhrase = "Dark Theme";
                prefslist.Add(dark);

                model.uiprefs = prefslist;
                
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "User Management");
                #endregion
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            };
        }

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User - Own Profile")]
        public ActionResult Edit(int id, UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateFail, "My Profile");
                #endregion
                return View(model);
            }

            try
            {
                #region DB UPDATE
                db.USER.Attach(model.user);
                db.Entry(model.user).State = EntityState.Modified;
                db.SaveChanges();
                #endregion
                // TODO: Add update logic here
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.UpdateSuccess, "My Profile");
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
                model.ualList = db.ACCESS_LEVEL.Where(x=>x.IsDeactivated==false).ToList();
                for (int i = 0; i < id; i++)
                {
                    model.tokenList.Add(new TOKEN_LOG());

                }
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
                List<TOKEN_LOG> finalTokens = new List<TOKEN_LOG>();
                foreach (var item in model.tokenList)
                {
                    TOKEN_LOG tk = new TOKEN_LOG();
                    tk = VECTOR.GenerateToken(item.DestinationEmail);
                    tk.IssueTimestamp = DateTime.Now;
                    tk.AccessLevelID = item.AccessLevelID;
                    finalTokens.Add(tk);
                }
                int count = 0;
                foreach (var final in finalTokens)
                {
                    string mailbody = "";
                    try
                    {
                        final.TokenID = db.TOKEN_LOG.Max(x => x.TokenID) + 1;
                    }
                    catch (Exception)
                    {
                        final.TokenID = 1;
                    }

                    mailbody = "Good Day,\n\n" +

                               "Please follow the link below in order to register your User profile on the DocuPath system.\n" +
                               "http://localhost:56640/account/redeemtoken/" + final.TokenValue+"\n\n" +

                               "Regards,\n" +
                               "The DocuPath team";


                    VERTEBRAE.sendMail(final.DestinationEmail,mailbody,"DocuPath Registration Token");
                    PasswordHasher hasher = new PasswordHasher();
                    final.TokenValue = hasher.HashPassword(final.TokenValue);
                    db.TOKEN_LOG.Add(final);
                    db.SaveChanges();
                }
                
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.GenerateTokensSuccess, "User Management");
                #endregion
                return RedirectToAction("All");
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
