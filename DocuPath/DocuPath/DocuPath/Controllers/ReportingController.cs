using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class ReportingController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        public ActionResult Index()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Vision Dashboard - System Health Overview")]
        public ActionResult VISION()
        {
            try
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

        [AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        public ActionResult INSIGHT()
        {
            try
            {

                return null;//404
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
