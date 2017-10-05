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
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "INSIGHT Reporting");
                #endregion

                DateTime TODAY = DateTime.Today;
                DateTime TODAY_MIDN = TODAY.AddDays(1).AddMilliseconds(-1);
                var YESTERDAY = TODAY_MIDN.AddDays(-1);
                var LAST_WEEK = TODAY_MIDN.AddDays(-7);
                var LAST_MONTH = TODAY_MIDN.AddDays(-30);

                ReportingViewModel model = new ReportingViewModel();
                
                //QUERY
                InsightQuery query = new InsightQuery();
                query.reportToGenerate = 0;
                query.reportTimeframe = 0;
                query.reportDateFrom = DateTime.Now;
                query.reportDateTo = DateTime.Now;
                query.reportUsersSelector = -1;
                query.reportActivitiesSelector = -1;
                query.reportSuperuserSelector = -1;
                model.iq = query;
                //USERS
                List<UserFullKVP> userKVPList = new List<UserFullKVP>();
                foreach (var user in db.USER)
                {
                    UserFullKVP userToAdd = new UserFullKVP();
                    userToAdd.uId = user.UserID;
                    userToAdd.uNameSurname = user.FirstName + " " + user.LastName;
                    userKVPList.Add(userToAdd);
                }
                model.users = userKVPList;
                //SUPERUSERS
                List<UserFullKVP> superuserKVPList = new List<UserFullKVP>();
                foreach (var user in db.USER.Where(u => u.USER_LOGIN.ACCESS_LEVEL.LevelName == "Superuser"))
                {
                    UserFullKVP superuserToAdd = new UserFullKVP();
                    superuserToAdd.uId = user.UserID;
                    superuserToAdd.uNameSurname = user.FirstName + " " + user.LastName;
                    superuserKVPList.Add(superuserToAdd);
                }
                model.superusers = superuserKVPList;
                //ACTIVITY TYPES
                List<ActivityKVP> activityKVPList = new List<ActivityKVP>();
                ActivityKVP allTypes = new ActivityKVP();
                allTypes.aId = 999;
                allTypes.aDesc = "All Activity Types";
                activityKVPList.Add(allTypes);
                foreach (var activityType in db.AUDIT_TX_TYPE)
                {
                    ActivityKVP typeToAdd = new ActivityKVP();
                    typeToAdd.aId = activityType.AuditLogTxTypeID;
                    typeToAdd.aDesc = activityType.TypeValue;
                    activityKVPList.Add(typeToAdd);
                }
                model.activityTypes = activityKVPList;
                //REPORTS
                List<ReportKVP> reportsKVPList = new List<ReportKVP>();
                ReportKVP cod = new ReportKVP();
                cod.reportID = 1;
                cod.reportPhrase = "Generate Cause Of Death Report";
                reportsKVPList.Add(cod);

                ReportKVP mod = new ReportKVP();
                mod.reportID = 2;
                mod.reportPhrase = "Generate Manner Of Death Report";
                reportsKVPList.Add(mod);

                ReportKVP soi = new ReportKVP();
                soi.reportID = 3;
                soi.reportPhrase = "Generate Scene Of Injury Report";
                reportsKVPList.Add(soi);

                ReportKVP outSR = new ReportKVP();
                outSR.reportID = 4;
                outSR.reportPhrase = "Generate Outstanding Service Request Report";
                reportsKVPList.Add(outSR);

                ReportKVP newMedia = new ReportKVP();
                newMedia.reportID = 5;
                newMedia.reportPhrase = "Generate New Media Report";
                reportsKVPList.Add(newMedia);

                ReportKVP SUoverride = new ReportKVP();
                SUoverride.reportID = 6;
                SUoverride.reportPhrase = "Generate Superuser Override Report";
                reportsKVPList.Add(SUoverride);

                ReportKVP caseDuration = new ReportKVP();
                caseDuration.reportID = 7;
                caseDuration.reportPhrase = "Generate Case Duration Report";
                reportsKVPList.Add(caseDuration);

                ReportKVP userActivity = new ReportKVP();
                userActivity.reportID = 8;
                userActivity.reportPhrase = "Generate User Activity Report";
                reportsKVPList.Add(userActivity);

                model.reports = reportsKVPList;

                //TIMEFRAMES
                List<TimeframeKVP> timeframesKVPList = new List<TimeframeKVP>();
                TimeframeKVP today = new TimeframeKVP();
                today.tfId = 1;
                today.tfPhrase = "Today";
                today.tfStartValue = TODAY;
                today.tfEndValue = TODAY_MIDN;
                timeframesKVPList.Add(today);

                TimeframeKVP yesterday = new TimeframeKVP();
                yesterday.tfId = 2;
                yesterday.tfPhrase = "Yesterday";
                yesterday.tfStartValue = TODAY;
                yesterday.tfEndValue = YESTERDAY;
                timeframesKVPList.Add(yesterday);

                TimeframeKVP thisweek = new TimeframeKVP();
                thisweek.tfId = 3;
                thisweek.tfPhrase = "This Week";
                thisweek.tfStartValue = TODAY_MIDN;
                thisweek.tfEndValue = TODAY_MIDN.AddDays(-7);
                timeframesKVPList.Add(thisweek);

                TimeframeKVP lastweek = new TimeframeKVP();
                lastweek.tfId = 4;
                lastweek.tfPhrase = "Last Week";
                lastweek.tfStartValue = TODAY_MIDN.AddDays(-7);
                lastweek.tfEndValue = TODAY_MIDN.AddDays(-14);
                timeframesKVPList.Add(lastweek);

                TimeframeKVP custom = new TimeframeKVP();
                custom.tfId = 5;
                custom.tfPhrase = "Custom Timeframe";
                custom.tfStartValue = TODAY;
                custom.tfEndValue = TODAY_MIDN;
                timeframesKVPList.Add(custom);

                model.timeframes = timeframesKVPList;

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

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        public ActionResult Insight(ReportingViewModel model)
        {
            string actionName = "Insight";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "INSIGHT Reporting");
                #endregion

                return null;
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

        //[AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        //public ActionResult GenCODReport()
        //{

        //}
    }
}
