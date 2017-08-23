using DocuPath.DataLayer;
using DocuPath.Models.Custom_Classes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace DocuPath.Models
{
    public static class VERTEBRAE
    {
        public static List<NOTIFICATION> GetUnhandledNeurons()
        {
            List<NOTIFICATION> unhandledNeurons = new List<NOTIFICATION>();
            using (DocuPathEntities db = new DocuPathEntities())
            {

                foreach (var neuron in db.NOTIFICATION)
                {
                    if (neuron.HandledDateTimeStamp == null)
                    {
                        unhandledNeurons.Add(neuron);
                    }
                }
            }
            return unhandledNeurons;
        }
        public static USER getCurrentUser()
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
                //int id = User.Identity.GetUserId<int>();
                int id = HttpContext.Current.User.Identity.GetUserId<int>();
                USER currentUser = db.USER.Where(x => x.UserID == id).FirstOrDefault();
                currentUser.USER_LOGIN = db.USER_LOGIN.Where(x => x.UserLoginID == currentUser.UserLoginID).FirstOrDefault();
                currentUser.TITLE = db.TITLE.Where(x => x.TitleID == currentUser.TitleID).FirstOrDefault();

                return currentUser;
            }
        }
        public static List<METRIC> GetVisionMetrics()
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
                List<METRIC> visionMetrics = new List<METRIC>();

                DateTime dtToday = System.DateTime.Today;
                DateTime dtDateToday = System.DateTime.Today.Date;
                DateTime dtTimeNow = System.DateTime.Now;
                DateTime dtDateNow = System.DateTime.Now.Date;

                // CASE MANAGEMENT OVERVIEW:
                // ======= FORENSIC CASES =======
                METRIC vmFCCount = new METRIC("Forensic Cases", db.FORENSIC_CASE.Count().ToString(), "cases", "", "", System.DateTime.Now);
                visionMetrics.Add(vmFCCount);

                //// --> COUNT OF FC BY STATUS:
                //ViewBag.ActiveFCCount = db.FORENSIC_CASE.Where(fc => fc.STATUS.StatusValue == "Active").Count();
                //ViewBag.PendingFCCount = db.FORENSIC_CASE.Where(fc => fc.STATUS.StatusValue == "Pending").Count();
                //ViewBag.ArchivedFCCount = db.FORENSIC_CASE.Where(fc => fc.STATUS.StatusValue == "Archived").Count();
                //ViewBag.LockedFCCount = db.FORENSIC_CASE.Where(fc => fc.STATUS.StatusValue == "Locked").Count();
                //// --> COUNT OF FC BY DATE ADDED:
                //ViewBag.FCAddedToday = db.FORENSIC_CASE.Where(fc => fc.DateAdded == dtToday).Count();
                //ViewBag.FCAddedPastSevenDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > dtToday.AddDays(-7)).Count();
                //ViewBag.FCAddedPastThirtyDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > dtToday.AddDays(-30)).Count();
                //ViewBag.FCAddedPastSixtyDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > dtToday.AddDays(-60)).Count();
                //ViewBag.FCAddedPastNinetyDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > dtToday.AddDays(-90)).Count();
                //// --> COUNT OF FC BY DATE CLOSED:
                //ViewBag.FCClosedToday = db.FORENSIC_CASE.Where(fc => fc.DateClosed == dtToday);
                //ViewBag.FCClosedPastSevenDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-7)).Count();
                //ViewBag.FCClosedPastThirtyDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-30)).Count();
                //ViewBag.FCClosedPastSixtyDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-60)).Count();
                //ViewBag.FCClosedPastNinetyDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-90)).Count();
                //// --> MISC FC STATS:
                //// Average Forensic Case Duration:
                //double dAverageTicks = db.FORENSIC_CASE.Average(fc => (fc.DateClosed - fc.DateAdded).Value.Ticks);
                //long lAverageTicks = Convert.ToInt64(dAverageTicks);
                //ViewBag.AverageFCDuration = new TimeSpan(lAverageTicks);
                //// Aged:Non-Aged Forensic Case Ratio:
                //double dAgedFCPercentage = db.FORENSIC_CASE.Where(fc => (dtDateNow - fc.DateClosed).Value.Days > 365 * 11).Count() / db.FORENSIC_CASE.Count() * 100;
                //double dRoundedAgedFCPercentage = Math.Round(dAgedFCPercentage, 2);
                //double dRoundedNonAgedFCPercentage = Math.Round(100 - dRoundedAgedFCPercentage, 2);
                //ViewBag.AgedFCPercentage = dRoundedAgedFCPercentage;
                //ViewBag.NonAgedFCPercentage = dRoundedNonAgedFCPercentage;

                //// ======= LEGACY CASES =======
                //ViewBag.TotalLCCount = db.LEGACY_CASE.Count();
                //// --> COUNT OF LC BY STATUS:
                //ViewBag.ActiveLCCount = db.LEGACY_CASE.Where(lc => lc.STATUS.StatusValue == "Active").Count();
                //ViewBag.ArchivedLCCount = db.LEGACY_CASE.Where(lc => lc.STATUS.StatusValue == "Archived").Count();
                //// --> COUNT OF LC BY DATE ADDED:
                //ViewBag.LCAddedToday = db.LEGACY_CASE.Where(lc => lc.DateAdded == dtToday).Count();
                //ViewBag.LCAddedPastSevenDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > dtToday.AddDays(-7)).Count();
                //ViewBag.LCAddedPastThirtyDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > dtToday.AddDays(-30)).Count();
                //ViewBag.LCAddedPastSixtyDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > dtToday.AddDays(-60)).Count();
                //ViewBag.LCAddedPastNinetyDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > dtToday.AddDays(-90)).Count();

                //// ======= EXTERNAL REVIEW CASES =======
                //ViewBag.TotalERCCount = db.EXTERNAL_REVIEW_CASE.Count();

                // MEDIA OVERVIEW:

                // CONTENT OVERVIEW:

                // USER OVERVIEW:

                // SYSTEM OVERVIEW:

                //TODO: Compile a list of metrics for use in VISION dashboard.
                //Unsure what to do to have METRIC show up as an option when defining method return type...did this too long ago.

                return visionMetrics;
            }
        }
    }
}