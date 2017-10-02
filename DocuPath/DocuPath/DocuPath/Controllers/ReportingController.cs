using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocuPath.Models.DPViewModels;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    //[LogAction]
    public class ReportingController : Controller
    {
        string controllerName = "Reporting";
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        public ActionResult Index()
        {
            string actionName = "Index";
            try
            {
                return RedirectToAction("Insight");
            }
            catch (Exception x)
            {
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Vision Dashboard - System Health Overview")]
        public ActionResult Vision()
        {
            string actionName = "Vision";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewInit, "VISION");
                #endregion
                var metrics = VERTEBRAE.GetVisionMetrics();
                ViewBag.VISIONMetrics = metrics;

                ViewBag.FCByStatusChartData = metrics.Where(m => m.MetricName == "Forensic Cases By Status").FirstOrDefault().MetricValue;

                var fcaddedmetricvalue = metrics.Where(m => m.MetricName == "Forensic Cases Added & Closed").FirstOrDefault().MetricValue;
                ViewBag.FCAddedChartData = fcaddedmetricvalue.Substring(0, fcaddedmetricvalue.IndexOf('|'));
                ViewBag.FCClosedChartData = fcaddedmetricvalue.Substring(fcaddedmetricvalue.IndexOf('|') + 1);

                ViewBag.LCByStatusChartData = metrics.Where(m => m.MetricName == "Legacy Cases By Status").FirstOrDefault().MetricValue;

                ViewBag.LCCapturedChartData = metrics.Where(m => m.MetricName == "Legacy Cases Captured").FirstOrDefault().MetricValue;

                ViewBag.ERCByStatusChartData = metrics.Where(m => m.MetricName == "External Review Cases By Status").FirstOrDefault().MetricValue;
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ViewFail, "VISION");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        public ActionResult Insight()
        {
            string actionName = "Insight";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "Media");
                #endregion

                //model = new ReportingViewModel();
                //model.query = new InsightQuery();
                //model.query.allUsers = new List<InsightUser>();
                //model.query.allActivityTypes = new List<ActivityType>();

                //foreach (var item in db.USER)
                //{
                //    //if (item.USER_LOGIN.ACCESS_LEVEL.LevelName == "Superuser")
                //    //{
                //    InsightUser userToAdd = new InsightUser();
                //    InsightUser.
                //    model.query.allUsers.Add(new User { Value = item.UserID.ToString(), Text = item.FirstName + " " + item.LastName });
                //    //}
                //}

                //model.query.reportToGenerate = 0;
                //model.query.timeframeToTarget = 0;
                //model.query.dateFrom = DateTime.Today;
                //model.query.dateTo = DateTime.Today;
                //model.query.suID = 0;
                //model.query.uID = 0;
                //model.query.activityTypeID = 0;

                List<SelectListItem> selectSuperusers = new List<SelectListItem>();
                List<SelectListItem> selectUsers = new List<SelectListItem>();
                List<SelectListItem> selectActivityTypes = new List<SelectListItem>();

                selectSuperusers.Add(new SelectListItem { Value = "0", Text = "Select a Superuser..." });
                foreach (var item in db.USER)
                {
                    if (item.USER_LOGIN.ACCESS_LEVEL.LevelName == "Superuser")
                    {
                        selectSuperusers.Add(new SelectListItem { Value = item.UserID.ToString(), Text = item.FirstName + " " + item.LastName });
                    }
                }
                ViewBag.Superusers = selectSuperusers;

                selectUsers.Add(new SelectListItem { Value = "0", Text = "Select a User..." });
                foreach (var item in db.USER)
                {
                    selectUsers.Add(new SelectListItem { Value = item.UserID.ToString(), Text = item.FirstName + " " + item.LastName });
                }
                ViewBag.Users = selectUsers;

                selectActivityTypes.Add(new SelectListItem { Value = "0", Text = "Select an Activity Type..." });
                selectActivityTypes.Add(new SelectListItem { Value = "1", Text = "All Activity Types" });
                foreach (var item in db.AUDIT_TX_TYPE)
                {
                    selectActivityTypes.Add(new SelectListItem { Value = (item.AuditLogTxTypeID + 1).ToString(), Text = item.TypeValue });
                }
                ViewBag.ActivityTypes = selectActivityTypes;
                ReportingViewModel model = new ReportingViewModel();
                int userID = VERTEBRAE.getCurrentUser().UserID;
                DateTime dateFrom = DateTime.Today.Date.AddDays(-7);
                DateTime dateTo = DateTime.Today.Date;
                model.activityTransactions = db.AUDIT_LOG.Where(x => x.UserID == userID && x.TxDateStamp > dateFrom && x.TxDateStamp < dateTo ).ToList();
                model.activityTransactions = model.activityTransactions.OrderByDescending(x => x.TxTimeStamp).ToList();
                                
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingSuccess, "Media");
                #endregion
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingFail, "Media");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }
    }
}
