using DocuPath.Models;
using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocuPath.Models.DPViewModels;
using IronPdf;

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
        public ViewResult Insight(ReportingViewModel model)
        {
            string actionName = "Insight";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "INSIGHT Reporting");
                #endregion

                model.iq.reportDateTo = model.iq.reportDateTo.Value.AddDays(1).AddSeconds(-1);
                var user = VERTEBRAE.getCurrentUser();
                DateTime TODAY = DateTime.Today;
                DateTime TODAY_MIDN = TODAY.AddDays(1).AddMilliseconds(-1);
                var YESTERDAY = TODAY.AddDays(-1).AddMilliseconds(1);
                var LAST_WEEK = TODAY.AddDays(-7).AddMilliseconds(1);
                var PREV_WEEK = TODAY.AddDays(-14).AddMilliseconds(1);
                var targetFrom = new DateTime();
                var targetTo = new DateTime();

                switch (model.iq.reportToGenerate)
                {
                    case 1: // COD
                        #region COD REPORT BUILDER
                        InsightCODDataset codDataSet = new InsightCODDataset();
                        // Generated by:
                        codDataSet.dsGeneratedBy = user.FirstName + " " + user.LastName;
                        // Grouped by:
                        codDataSet.dsGroupedBy = "None";
                        // Sorted by:
                        codDataSet.dsSortedBy = "Frequency Descending";
                        // Timeframe:
                        switch (model.iq.reportTimeframe)
                        {
                            case 1: // TODAY
                                codDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                targetFrom = TODAY;
                                targetTo = TODAY_MIDN;
                                break;
                            case 2: // YESTERDAY
                                codDataSet.dsTimeframeToString = YESTERDAY.ToString("dd MMM yyyy");
                                targetFrom = YESTERDAY;
                                targetTo = TODAY;
                                break;
                            case 3: // THIS WEEK
                                codDataSet.dsTimeframeToString = LAST_WEEK.ToString("dd MMM yyyy") + " - " + TODAY.ToString("dd MMM yyyy");
                                targetFrom = LAST_WEEK;
                                targetTo = TODAY;
                                break;
                            case 4: // LAST WEEK
                                codDataSet.dsTimeframeToString = PREV_WEEK.ToString("dd MMM yyyy") + " - " + LAST_WEEK.ToString("dd MMM yyyy");
                                targetFrom = PREV_WEEK;
                                targetTo = LAST_WEEK;
                                break;
                            case 5: // CUSTOM
                                targetFrom = (DateTime)model.iq.reportDateFrom;
                                targetTo = (DateTime)model.iq.reportDateTo;

                                codDataSet.dsTimeframeToString = targetFrom.ToString("dd MMM yyyy") + " - " + targetTo.ToString("dd MMM yyyy");
                                break;
                            default:
                                codDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                break;
                        }

                        // Table Data:
                        List<FreqKVP> codFreqDeterminator = new List<FreqKVP>();
                        foreach (var cod in db.PRIMARY_CAUSE_DEATH)
                        {
                            FreqKVP newCODFreq = new FreqKVP();
                            int freq = 0;
                            DateTime maxClosedDate = new DateTime();
                            //DateTime maxClosedDate = Convert.ToDateTime("1/1/0000 00:00:00");
                            newCODFreq.FreqID = cod.PrimaryCauseID;
                            newCODFreq.FreqDesc = cod.PrimaryCauseDescription;

                            foreach (var fc in db.FORENSIC_CASE.Where(fc => fc.DateClosed != null && fc.DateClosed.Value.Year != 9999 && fc.DateClosed.Value.Year != 0001 && fc.DateClosed >= targetFrom && fc.DateClosed <= targetTo && fc.CASE_STATISTICS.Count > 0))
                            {
                                if (fc.CASE_STATISTICS.FirstOrDefault().PRIMARY_CAUSE_DEATH.PrimaryCauseDescription == cod.PrimaryCauseDescription)
                                {
                                    freq++;
                                    if (fc.DateClosed > maxClosedDate)
                                    {
                                        maxClosedDate = (DateTime)fc.DateClosed;
                                    }
                                }
                            }
                            newCODFreq.FreqCount = freq;
                            newCODFreq.FreqLastClosedDate = maxClosedDate.ToString("dd MMM yyyy");
                            codFreqDeterminator.Add(newCODFreq);
                        }
                        codDataSet.dsCOD = codFreqDeterminator;
                        codDataSet.dsCOD = codDataSet.dsCOD.Where(item => item.FreqCount > 0).ToList();
                            codDataSet.dsCOD = codDataSet.dsCOD.OrderByDescending(cod => cod.FreqCount).ToList();
                        
                        // Number of Entries:
                        codDataSet.dsNumEntries = codDataSet.dsCOD.Count;

                        // Chart Data & Labels:
                        string CODChartData = "";
                        string CODChartLabels = "";

                        if (codDataSet.dsCOD.Count < 5)
                        {
                            // FIRST n WHERE n < 5:
                            for (int i = 0; i < codDataSet.dsCOD.Count; i++)
                            {
                                CODChartData += codDataSet.dsCOD[i].FreqCount + ", ";
                                CODChartLabels += "\'"+codDataSet.dsCOD[i].FreqDesc + "\', ";
                            }
                        }
                        else
                        {
                            // FIRST 5:
                            for (int i = 0; i < 5; i++)
                            {
                                CODChartData += codDataSet.dsCOD[i].FreqCount + ", ";
                                CODChartLabels += "\'" + codDataSet.dsCOD[i].FreqDesc + "\', ";
                            }
                        }
                            // REST:
                            var temp = codDataSet.dsCOD.Skip(5).ToList();

                            CODChartLabels += "Remainder";
                            int remainderCount = 0;
                            foreach (var taken in temp)
                            {
                                remainderCount += taken.FreqCount;
                            }
                            CODChartData += remainderCount;

                        codDataSet.dsChartData = CODChartData;
                        codDataSet.dsChartLabels = CODChartLabels;
                                                
                        return View("GenerateCauseOfDeathReport", codDataSet); 
                    #endregion
                    case 2: // MOD
                        #region MOD REPORT BUILDER
                        InsightMODDataset modDataSet = new InsightMODDataset();
                        // Generated by:
                        modDataSet.dsGeneratedBy = user.FirstName + " " + user.LastName;
                        // Grouped by:
                        modDataSet.dsGroupedBy = "None";
                        // Sorted by:
                        modDataSet.dsSortedBy = "Frequency Descending";
                        // Timeframe:
                        switch (model.iq.reportTimeframe)
                        {
                            case 1: // TODAY
                                modDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                targetFrom = TODAY;
                                targetTo = TODAY_MIDN;
                                break;
                            case 2: // YESTERDAY
                                modDataSet.dsTimeframeToString = YESTERDAY.ToString("dd MMM yyyy");
                                targetFrom = YESTERDAY;
                                targetTo = TODAY;
                                break;
                            case 3: // THIS WEEK
                                modDataSet.dsTimeframeToString = LAST_WEEK.ToString("dd MMM yyyy") + " - " + TODAY.ToString("dd MMM yyyy");
                                targetFrom = LAST_WEEK;
                                targetTo = TODAY;
                                break;
                            case 4: // LAST WEEK
                                modDataSet.dsTimeframeToString = PREV_WEEK.ToString("dd MMM yyyy") + " - " + LAST_WEEK.ToString("dd MMM yyyy");
                                targetFrom = PREV_WEEK;
                                targetTo = LAST_WEEK;
                                break;
                            case 5: // CUSTOM
                                targetFrom = (DateTime)model.iq.reportDateFrom;
                                targetTo = (DateTime)model.iq.reportDateTo;

                                modDataSet.dsTimeframeToString = targetFrom.ToString("dd MMM yyyy") + " - " + targetTo.ToString("dd MMM yyyy");
                                break;
                            default:
                                modDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                break;
                        }

                        // Table Data:
                        List<FreqKVP> modFreqDeterminator = new List<FreqKVP>();
                        foreach (var mod in db.APPARENT_MANNER_DEATH)
                        {
                            FreqKVP newMODFreq = new FreqKVP();
                            int freq = 0;
                            DateTime maxClosedDate = new DateTime();
                            //DateTime maxClosedDate = Convert.ToDateTime("1/1/0000 00:00:00");
                            newMODFreq.FreqID = mod.ApparentMannerID;
                            newMODFreq.FreqDesc = mod.ApparentMannerDescription;

                            foreach (var fc in db.FORENSIC_CASE.Where(fc => fc.DateClosed != null && fc.DateClosed.Value.Year != 9999 && fc.DateClosed.Value.Year != 0001 && fc.DateClosed >= targetFrom && fc.DateClosed <= targetTo && fc.CASE_STATISTICS.Count > 0))
                            {
                                if (fc.CASE_STATISTICS.FirstOrDefault().APPARENT_MANNER_DEATH.ApparentMannerDescription == mod.ApparentMannerDescription)
                                {
                                    freq++;
                                    if (fc.DateClosed > maxClosedDate)
                                    {
                                        maxClosedDate = (DateTime)fc.DateClosed;
                                    }
                                }
                            }
                            newMODFreq.FreqCount = freq;
                            newMODFreq.FreqLastClosedDate = maxClosedDate.ToString("dd MMM yyyy");
                            modFreqDeterminator.Add(newMODFreq);
                        }
                        modDataSet.dsMOD = modFreqDeterminator;
                        modDataSet.dsMOD = modDataSet.dsMOD.Where(item => item.FreqCount > 0).ToList();
                        modDataSet.dsMOD = modDataSet.dsMOD.OrderByDescending(mod => mod.FreqCount).ToList();

                        // Number of Entries:
                        modDataSet.dsNumEntries = modDataSet.dsMOD.Count;

                        // Chart Data & Labels:
                        string MODChartData = "";
                        string MODChartLabels = "";

                        if (modDataSet.dsMOD.Count < 5)
                        {
                            // FIRST n WHERE n < 5:
                            for (int i = 0; i < modDataSet.dsMOD.Count; i++)
                            {
                                MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                                MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                            }
                        }
                        else
                        {
                            // FIRST 5:
                            for (int i = 0; i < 5; i++)
                            {
                                MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                                MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                            }
                        }
                        // REST:
                        var MODtemp = modDataSet.dsMOD.Skip(5).ToList();

                        MODChartLabels += "Remainder";
                        int MODremainderCount = 0;
                        foreach (var taken in MODtemp)
                        {
                            MODremainderCount += taken.FreqCount;
                        }
                        MODChartData += MODremainderCount;

                        modDataSet.dsChartData = MODChartData;
                        modDataSet.dsChartLabels = MODChartLabels;

                        return View("GenerateMannerOfDeathReport", modDataSet);
                    #endregion
                    case 3: // SOI
                        #region SOI REPORT BUILDER
                        InsightSOIDataset soiDataSet = new InsightSOIDataset();
                        // Generated by:
                        soiDataSet.dsGeneratedBy = user.FirstName + " " + user.LastName;
                        // Grouped by:
                        soiDataSet.dsGroupedBy = "None";
                        // Sorted by:
                        soiDataSet.dsSortedBy = "Frequency Descending";
                        // Timeframe:
                        switch (model.iq.reportTimeframe)
                        {
                            case 1: // TODAY
                                soiDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                targetFrom = TODAY;
                                targetTo = TODAY_MIDN;
                                break;
                            case 2: // YESTERDAY
                                soiDataSet.dsTimeframeToString = YESTERDAY.ToString("dd MMM yyyy");
                                targetFrom = YESTERDAY;
                                targetTo = TODAY;
                                break;
                            case 3: // THIS WEEK
                                soiDataSet.dsTimeframeToString = LAST_WEEK.ToString("dd MMM yyyy") + " - " + TODAY.ToString("dd MMM yyyy");
                                targetFrom = LAST_WEEK;
                                targetTo = TODAY;
                                break;
                            case 4: // LAST WEEK
                                soiDataSet.dsTimeframeToString = PREV_WEEK.ToString("dd MMM yyyy") + " - " + LAST_WEEK.ToString("dd MMM yyyy");
                                targetFrom = PREV_WEEK;
                                targetTo = LAST_WEEK;
                                break;
                            case 5: // CUSTOM
                                targetFrom = (DateTime)model.iq.reportDateFrom;
                                targetTo = (DateTime)model.iq.reportDateTo;

                                soiDataSet.dsTimeframeToString = targetFrom.ToString("dd MMM yyyy") + " - " + targetTo.ToString("dd MMM yyyy");
                                break;
                            default:
                                soiDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                break;
                        }

                        // Table Data:
                        //List<FreqKVP> soiFreqDeterminator = new List<FreqKVP>();
                        //foreach (var fc in db.FORENSIC_CASE.Where(fc => fc.DateClosed != null && fc.DateClosed.Value.Year != 9999 && fc.DateClosed.Value.Year != 0001 && fc.DateClosed >= targetFrom && fc.DateClosed <= targetTo && fc.CASE_STATISTICS.Count > 0))
                        //{
                        //    foreach (var sis in db.STATS_INJURY_SCENE.Where(x => x.ForensicCaseID == fc.ForensicCaseID))
                        //    {

                        //    }
                        //}


                        //foreach (var sis in db.STATS_INJURY_SCENE.Where(si => si.ForensicCaseID == ))
                        //{

                        //}


                        //foreach (var soi in db.SCENE_OF_INJURY)
                        //{
                        //    FreqKVP newSOIFreq = new FreqKVP();
                        //    int freq = 0;
                        //    DateTime maxClosedDate = new DateTime();
                        //    //DateTime maxClosedDate = Convert.ToDateTime("1/1/0000 00:00:00");
                        //    newSOIFreq.FreqID = soi.InjurySceneID;
                        //    newSOIFreq.FreqDesc = soi.InjurySceneDescription;

                        //    foreach (var fc in db.FORENSIC_CASE.Where(fc => fc.DateClosed != null && fc.DateClosed.Value.Year != 9999 && fc.DateClosed.Value.Year != 0001 && fc.DateClosed >= targetFrom && fc.DateClosed <= targetTo && fc.CASE_STATISTICS.Count > 0))
                        //    {
                        //        if (fc.CASE_STATISTICS.FirstOrDefault().STATS_INJURY_SCENE.Where(sis => sis.ForensicCaseID == fc.ForensicCaseID).Any(x => x.SCENE_OF_INJURY.InjurySceneDescription) )
                        //        {
                        //            freq++;
                        //            if (fc.DateClosed > maxClosedDate)
                        //            {
                        //                maxClosedDate = (DateTime)fc.DateClosed;
                        //            }
                        //        }
                        //    }
                        //    newSOIFreq.FreqCount = freq;
                        //    newSOIFreq.FreqLastClosedDate = maxClosedDate.ToString("dd MMM yyyy");
                        //    soiFreqDeterminator.Add(newSOIFreq);
                        //}
                        //modDataSet.dsMOD = modFreqDeterminator;
                        //modDataSet.dsMOD = modDataSet.dsMOD.Where(item => item.FreqCount > 0).ToList();
                        //modDataSet.dsMOD = modDataSet.dsMOD.OrderByDescending(mod => mod.FreqCount).ToList();

                        //// Number of Entries:
                        //modDataSet.dsNumEntries = modDataSet.dsMOD.Count;

                        //// Chart Data & Labels:
                        //string MODChartData = "";
                        //string MODChartLabels = "";

                        //if (modDataSet.dsMOD.Count < 5)
                        //{
                        //    // FIRST n WHERE n < 5:
                        //    for (int i = 0; i < modDataSet.dsMOD.Count; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //else
                        //{
                        //    // FIRST 5:
                        //    for (int i = 0; i < 5; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //// REST:
                        //var SOItemp = modDataSet.dsMOD.Skip(5).ToList();

                        //MODChartLabels += "Remainder";
                        //int SOIremainderCount = 0;
                        //foreach (var taken in SOItemp)
                        //{
                        //    SOIremainderCount += taken.FreqCount;
                        //}
                        //SOIChartData += SOIremainderCount;

                        //modDataSet.dsChartData = MODChartData;
                        //modDataSet.dsChartLabels = MODChartLabels;

                        return View("GenerateSceneOfInjuryReport", soiDataSet);
                    #endregion
                    case 4: // OSR
                        #region OSR REPORT BUILDER
                        InsightOSRDataset osrDataSet = new InsightOSRDataset();
                        // Generated by:
                        osrDataSet.dsGeneratedBy = user.FirstName + " " + user.LastName;
                        // Grouped by:
                        osrDataSet.dsGroupedBy = "None";
                        // Sorted by:
                        osrDataSet.dsSortedBy = "Date Added Descending";
                        // Timeframe:
                        osrDataSet.dsTimeframeToString = "All Time";
                        // Table Data:
                        List<OSRKVP> OSRDeterminator = new List<OSRKVP>();
                        foreach (var sr in db.SERVICE_REQUEST.Where(sr => sr.SPECIMEN.FirstOrDefault().EXTERNAL_REPORT != null))
                        {
                            OSRKVP newOSR = new OSRKVP();
                            newOSR.SRid = sr.ServiceRequestID;
                            newOSR.SPCompanyName = sr.SERVICE_PROVIDER.CompanyName;
                            newOSR.SRAddedBy = sr.FORENSIC_CASE.USER.FirstName+" "+sr.FORENSIC_CASE.USER.LastName;
                            newOSR.DRNum = sr.FORENSIC_CASE.ForensicDRNumber;
                            newOSR.DateAdded = sr.DateAdded.ToString("dd MMM yyyy");
                            newOSR.DaysOpen = (DateTime.Now - (DateTime)sr.DateAdded).Days;

                            OSRDeterminator.Add(newOSR);
                        }
                        osrDataSet.dsOSR = OSRDeterminator;
                        osrDataSet.dsOSR = osrDataSet.dsOSR.OrderByDescending(osr => osr.DateAdded).ToList();

                        // Number of Entries:
                        osrDataSet.dsNumEntries = osrDataSet.dsOSR.Count;

                        // Chart Data & Labels:
                        //string MODChartData = "";
                        //string MODChartLabels = "";

                        //if (modDataSet.dsMOD.Count < 5)
                        //{
                        //    // FIRST n WHERE n < 5:
                        //    for (int i = 0; i < modDataSet.dsMOD.Count; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //else
                        //{
                        //    // FIRST 5:
                        //    for (int i = 0; i < 5; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //// REST:
                        //var MODtemp = modDataSet.dsMOD.Skip(5).ToList();

                        //MODChartLabels += "Remainder";
                        //int MODremainderCount = 0;
                        //foreach (var taken in MODtemp)
                        //{
                        //    MODremainderCount += taken.FreqCount;
                        //}
                        //MODChartData += MODremainderCount;

                        //modDataSet.dsChartData = MODChartData;
                        //modDataSet.dsChartLabels = MODChartLabels;

                        return View("GenerateOutstandingServiceRequestReport", osrDataSet);
                    #endregion
                    case 5: // NEWMED
                        #region NEWMED REPORT BUILDER
                        InsightNewMediaDataset newmedDataSet = new InsightNewMediaDataset();
                        // Generated by:
                        newmedDataSet.dsGeneratedBy = user.FirstName + " " + user.LastName;
                        // Grouped by:
                        newmedDataSet.dsGroupedBy = "None";
                        // Sorted by:
                        newmedDataSet.dsSortedBy = "Date Added Ascending";
                        // Timeframe:
                        switch (model.iq.reportTimeframe)
                        {
                            case 1: // TODAY
                                newmedDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                targetFrom = TODAY;
                                targetTo = TODAY_MIDN;
                                break;
                            case 2: // YESTERDAY
                                newmedDataSet.dsTimeframeToString = YESTERDAY.ToString("dd MMM yyyy");
                                targetFrom = YESTERDAY;
                                targetTo = TODAY;
                                break;
                            case 3: // THIS WEEK
                                newmedDataSet.dsTimeframeToString = LAST_WEEK.ToString("dd MMM yyyy") + " - " + TODAY.ToString("dd MMM yyyy");
                                targetFrom = LAST_WEEK;
                                targetTo = TODAY;
                                break;
                            case 4: // LAST WEEK
                                newmedDataSet.dsTimeframeToString = PREV_WEEK.ToString("dd MMM yyyy") + " - " + LAST_WEEK.ToString("dd MMM yyyy");
                                targetFrom = PREV_WEEK;
                                targetTo = LAST_WEEK;
                                break;
                            case 5: // CUSTOM
                                targetFrom = (DateTime)model.iq.reportDateFrom;
                                targetTo = (DateTime)model.iq.reportDateTo;

                                newmedDataSet.dsTimeframeToString = targetFrom.ToString("dd MMM yyyy") + " - " + targetTo.ToString("dd MMM yyyy");
                                break;
                            default:
                                newmedDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                break;
                        }

                        // Table Data:
                        List<MEDIA> medialist = new List<MEDIA>();
                        foreach (var media in db.MEDIA.Where(med => med.DateAdded >= targetFrom && med.DateAdded <= targetTo))
                        {
                            medialist.Add(media);
                        }
                        newmedDataSet.dsNewMedia = medialist;

                        // Number of Entries:
                        newmedDataSet.dsNumEntries = newmedDataSet.dsNewMedia.Count;

                        // Chart Data & Labels:
                        //string MODChartData = "";
                        //string MODChartLabels = "";

                        //if (modDataSet.dsMOD.Count < 5)
                        //{
                        //    // FIRST n WHERE n < 5:
                        //    for (int i = 0; i < modDataSet.dsMOD.Count; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //else
                        //{
                        //    // FIRST 5:
                        //    for (int i = 0; i < 5; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //// REST:
                        //var MODtemp = modDataSet.dsMOD.Skip(5).ToList();

                        //MODChartLabels += "Remainder";
                        //int MODremainderCount = 0;
                        //foreach (var taken in MODtemp)
                        //{
                        //    MODremainderCount += taken.FreqCount;
                        //}
                        //MODChartData += MODremainderCount;

                        //modDataSet.dsChartData = MODChartData;
                        //modDataSet.dsChartLabels = MODChartLabels;

                        return View("GenerateNewMediaReport", newmedDataSet);
                    #endregion
                    case 6: // SUO
                        //return RedirectToAction("GenerateCauseOfDeathReport", model);
                    case 7: // CASE DURATION
                        //return RedirectToAction("GenerateCauseOfDeathReport", model);
                    case 8: // USER ACTIVITY
                        #region USER ACTIVITY REPORT BUILDER
                        InsightUARDataset newUARDataSet = new InsightUARDataset();
                        // Generated by:
                        newUARDataSet.dsGeneratedBy = user.FirstName + " " + user.LastName;
                        // Grouped by:
                        newUARDataSet.dsGroupedBy = "Date, Activity Type";
                        // Sorted by:
                        newUARDataSet.dsSortedBy = "Timestamp Ascending";
                        // Timeframe:
                        switch (model.iq.reportTimeframe)
                        {
                            case 1: // TODAY
                                newUARDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                targetFrom = TODAY;
                                targetTo = TODAY_MIDN;
                                break;
                            case 2: // YESTERDAY
                                newUARDataSet.dsTimeframeToString = YESTERDAY.ToString("dd MMM yyyy");
                                targetFrom = YESTERDAY;
                                targetTo = TODAY;
                                break;
                            case 3: // THIS WEEK
                                newUARDataSet.dsTimeframeToString = LAST_WEEK.ToString("dd MMM yyyy") + " - " + TODAY.ToString("dd MMM yyyy");
                                targetFrom = LAST_WEEK;
                                targetTo = TODAY;
                                break;
                            case 4: // LAST WEEK
                                newUARDataSet.dsTimeframeToString = PREV_WEEK.ToString("dd MMM yyyy") + " - " + LAST_WEEK.ToString("dd MMM yyyy");
                                targetFrom = PREV_WEEK;
                                targetTo = LAST_WEEK;
                                break;
                            case 5: // CUSTOM
                                targetFrom = (DateTime)model.iq.reportDateFrom;
                                targetTo = (DateTime)model.iq.reportDateTo;

                                newUARDataSet.dsTimeframeToString = targetFrom.ToString("dd MMM yyyy") + " - " + targetTo.ToString("dd MMM yyyy");
                                break;
                            default:
                                newUARDataSet.dsTimeframeToString = TODAY.ToString("dd MMM yyyy");
                                break;
                        }

                        // Additional Parameters:
                        int uId = db.USER.Where(u => u.UserID == model.iq.reportUsersSelector).FirstOrDefault().UserID;

                        ViewBag.TargetUser = user.FirstName + " " + user.LastName;

                        // MAKE A NEW TOP-LEVEL LIST FOR ALL ENTRIES:
                        List<cbrkDate> entries = new List<cbrkDate>();
                        int numberCounter = 0;

                        if (model.iq.reportActivitiesSelector == 999) // ALL TYPES
                        {
                            #region ALL TYPES
                            // THEN, ITERATE OVER DAYS:
                            foreach (DateTime day in EachDay(targetFrom, targetTo))
                            {
                                // MAKE DATE ENTRY FOR THE DAY:
                                cbrkDate dateEntry = new cbrkDate();
                                // CALL IT THE DATE TO STRING:
                                dateEntry.cbrkDateTitle = day.ToString("dddd, dd MMMM yyyy");

                                // NEW LIST TO CAPTURE ACTIVITIES:
                                List<cbrkActivityTypes> activities = new List<cbrkActivityTypes>();

                                foreach (var type in db.AUDIT_TX_TYPE)
                                {
                                    cbrkActivityTypes activityEntry = new cbrkActivityTypes();
                                    // CALL IT THE NAME OF THE TX TYPE:
                                    activityEntry.cbrkActivityTypeTitle = type.TypeValue;

                                    // CHECK ALL TRANSACTIONS IN LOG. IF USERID = CURRENT, TYPE = CURRENT AND DATE = CURRENT:
                                    List<AUDIT_LOG> txList = new List<AUDIT_LOG>();
                                    foreach (var transaction in db.AUDIT_LOG.Where(tx => tx.UserID == uId && tx.AuditLogTxTypeID == type.AuditLogTxTypeID))
                                    {
                                        if (transaction.TxDateStamp.Date == day.Date)
                                        {
                                            // THEN ADD TO LOG ENTRY LIST:
                                            txList.Add(transaction);
                                            numberCounter++;
                                        }
                                    }

                                    // SET TYPE LIST EQUAL TO LOG ENTRY LIST:
                                    activityEntry.cbrkActivityTypeEntries = txList; 
                                    // ADD TYPE LIST TO MASTER LIST:
                                    activities.Add(activityEntry);
                                }

                                // SET DATE ENTRY ACTIVITIES EQUAL TO TYPE LIST:
                                dateEntry.cbrkDaysActivities = activities;

                                // ADD TO TOP-LEVEL LIST:
                                entries.Add(dateEntry);
                            }

                            #endregion
                        }
                        else // SPECIFIC TYPES
                        {
                            #region SPECIFIC TYPES
                            // THEN, ITERATE OVER DAYS:
                            foreach (DateTime day in EachDay(targetFrom, targetTo))
                            {
                                // MAKE DATE ENTRY FOR THE DAY:
                                cbrkDate dateEntry = new cbrkDate();
                                // CALL IT THE DATE TO STRING:
                                dateEntry.cbrkDateTitle = day.ToString("dddd, dd MMMM yyyy");

                                    // NEW LIST TO CAPTURE ACTIVITIES:
                                    List<cbrkActivityTypes> activities = new List<cbrkActivityTypes>();
                                    cbrkActivityTypes activityEntry = new cbrkActivityTypes();

                                    // CALL IT THE NAME OF THE TX TYPE:
                                    activityEntry.cbrkActivityTypeTitle = db.AUDIT_TX_TYPE.Where(type => type.AuditLogTxTypeID == model.iq.reportActivitiesSelector).FirstOrDefault().TypeValue;
                                    List<AUDIT_LOG> txList = new List<AUDIT_LOG>();

                                        // CHECK ALL TRANSACTIONS IN LOG. IF USERID = CURRENT, TYPE = CURRENT AND DATE = CURRENT:
                                        foreach (var transaction in db.AUDIT_LOG.Where(tx => tx.UserID == uId && tx.AuditLogTxTypeID == model.iq.reportActivitiesSelector))
                                        {
                                            if (transaction.TxDateStamp.Date == day.Date)
                                            {
                                                // THEN ADD TO LOG ENTRY LIST:
                                                txList.Add(transaction);
                                                numberCounter++;
                                            }
                                        }

                                    // SET TYPE LIST EQUAL TO LOG ENTRY LIST:
                                    activityEntry.cbrkActivityTypeEntries = txList;                                    
                                    // ADD TYPE LIST TO MASTER LIST:
                                    activities.Add(activityEntry);

                                // SET DATE ENTRY ACTIVITIES EQUAL TO TYPE LIST:
                                dateEntry.cbrkDaysActivities = activities;

                                // ADD TO TOP-LEVEL LIST:
                                entries.Add(dateEntry);
                            } 
                            #endregion

                        }

                        newUARDataSet.dsUAR = entries;

                        //List<AUDIT_LOG> activityList = new List<AUDIT_LOG>();
                        //if (model.iq.reportActivitiesSelector != 999)
                        //{
                        //    // Table Data:
                        //    foreach (var tx in db.AUDIT_LOG.Where(tx => tx.UserID == uId && tx.AuditLogTxTypeID == model.iq.reportActivitiesSelector && tx.TxDateStamp >= targetFrom && tx.TxDateStamp <= targetTo))
                        //    {
                        //        AUDIT_LOG newtx = new AUDIT_LOG();
                        //        newtx = tx;
                        //        activityList.Add(newtx);
                        //    }
                        //}
                        //else
                        //{
                        //    // Table Data:
                        //    foreach (var tx in db.AUDIT_LOG.Where(tx => tx.UserID == uId && tx.TxDateStamp >= targetFrom && tx.TxDateStamp <= targetTo))
                        //    {
                        //        AUDIT_LOG newtx = new AUDIT_LOG();
                        //        newtx = tx;
                        //        activityList.Add(newtx);
                        //    }
                        //}
                        //newUARDataSet.dsUAR = activityList;
                        //newUARDataSet.dsUAR = newUARDataSet.dsUAR.GroupBy(tx => new { tx.TxDateStamp, tx.AUDIT_TX_TYPE.TypeValue }).OrderByDescending(tx => tx.TxDateStamp).ThenByDescending(tx => tx.TxTimeStamp).ToList();

                        // Number of Entries:
                        newUARDataSet.dsNumEntries = numberCounter;

                        // Chart Data & Labels:
                        //string MODChartData = "";
                        //string MODChartLabels = "";

                        //if (modDataSet.dsMOD.Count < 5)
                        //{
                        //    // FIRST n WHERE n < 5:
                        //    for (int i = 0; i < modDataSet.dsMOD.Count; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //else
                        //{
                        //    // FIRST 5:
                        //    for (int i = 0; i < 5; i++)
                        //    {
                        //        MODChartData += modDataSet.dsMOD[i].FreqCount + ", ";
                        //        MODChartLabels += "\'" + modDataSet.dsMOD[i].FreqDesc + "\', ";
                        //    }
                        //}
                        //// REST:
                        //var MODtemp = modDataSet.dsMOD.Skip(5).ToList();

                        //MODChartLabels += "Remainder";
                        //int MODremainderCount = 0;
                        //foreach (var taken in MODtemp)
                        //{
                        //    MODremainderCount += taken.FreqCount;
                        //}
                        //MODChartData += MODremainderCount;

                        //modDataSet.dsChartData = MODChartData;
                        //modDataSet.dsChartLabels = MODChartLabels;

                        return View("GenerateUserActivityReport", newUARDataSet);
                    #endregion
                    default:
                        break;
                }
                return View();
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

        private IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        {
            for (var day = start.Date; day.Date <= end.Date; day = day.AddDays(1))
                yield return day;
        }

        //[AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        //public ActionResult GenerateCauseOfDeathReport(InsightCODDataset model)
        //{
        //    string actionName = "GenerateCauseOfDeathReport";
        //    try
        //    {
        //        #region AUDIT_WRITE
        //        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "INSIGHT Reporting");
        //        #endregion

        //        InsightCODDataset newModel = new InsightCODDataset();

        //        newModel = model;
        //        return View(newModel);
        //    }
        //    catch (Exception x)
        //    {
        //        #region AUDIT_WRITE
        //        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingFail, "INSIGHT Reporting");
        //        #endregion
        //        VERTEBRAE.DumpErrorToTxt(x);
        //        return View("Error", new HandleErrorInfo(x, controllerName, actionName));
        //    }
        //}

        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        public PdfDocument PrintReportToPDF(string inHTML)
        {
            HtmlToPdf myHtmlToPdf = new IronPdf.HtmlToPdf();

            PdfDocument pdf = myHtmlToPdf.RenderHtmlAsPdf(inHTML);
            
            //string fileName = DateTime.Now.ToString("dd_mm_yyyy_HH_mm.pdf");

            return pdf;
        }

        public JsonResult SendRSS(string rssData)
        {
            try
            {
                VERTEBRAE.sendMail("docupath.vector@gmail.com", "Official Communication from the University of Pretoria Department of Forensic Medicine\nSent at " + DateTime.Now.ToString("HH:mm") + " on " + DateTime.Now.ToString("dd MMM yyyy"), rssData);
                return Json("RSS Broadcast sent successfully!");
            }
            catch (Exception x)
            {

                return Json("An error occurred. Here's what happened: " + x.Message);
            }
        }

        //[AuthorizeByAccessArea(AccessArea = "Insight Reporting - All Reports")]
        //public ActionResult GenerateUserActivityReport()
        //{
        //    string actionName = "GenerateUserActivityReport";
        //    try
        //    {
        //        #region AUDIT_WRITE
        //        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingInit, "INSIGHT Reporting");
        //        #endregion

        //        List<AUDIT_LOG> model = new List<AUDIT_LOG>();
        //        int uId = VERTEBRAE.getCurrentUser().UserID;
        //        AUDIT_LOG txToAdd = new AUDIT_LOG();

        //        foreach (var tx in db.AUDIT_LOG.Where(tx => tx.UserID == uId))
        //        {
        //            txToAdd = tx;
        //            model.Add(txToAdd);
        //        }

        //        ViewBag.EntriesQty = model.Count;

        //        model = model.OrderByDescending(tx => tx.TxDateStamp).ToList();
        //        return View(model);
        //    }
        //    catch (Exception x)
        //    {
        //        #region AUDIT_WRITE
        //        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.ReportingFail, "INSIGHT Reporting");
        //        #endregion
        //        VERTEBRAE.DumpErrorToTxt(x);
        //        return View("Error", new HandleErrorInfo(x, controllerName, actionName));
        //    }
        //}
    }
}
