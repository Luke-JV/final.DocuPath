using DocuPath.DataLayer;
using DocuPath.Models.Custom_Classes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;



namespace DocuPath.Models
{
    public static class VERTEBRAE
    {
        #region OUTGOING COMMUNICATION:

        public static void sendMail(string destination, string content)
        {
            //404 - mail logic here
            MailMessage outMail = new MailMessage();
            outMail.To.Add("luke@cldrm.co.za");
            outMail.From = new MailAddress("u13098536@tuks.co.za");
            outMail.Subject = "DocuPath Registration Link";
            outMail.Body = "Bruh, \n\n Hit up localhost:5069/Account/Register?token=Aafv=H#qafewwAdfdawFw/ in order to register your User Profile. \n\nThis limited offer ends like tomorrow though... \n\nLit Regards,\nLuke";

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("u13098536@tuks.co.za","Ktm200xcw");
            smtp.Send(outMail);

        }
        public static void sendSMS(string destination, string content)
        {
            //404 - SMS logic here
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region CONSTANTS:
        public const string LC_REPORootPath = "~/Content/DocuPathRepositories/LC_REPO/";
        public const string ERC_REPORootPath = "~/Content/DocuPathRepositories/ERC_REPO/";
        public const string ADD_EV_REPORootPath = "~/Content/DocuPathRepositories/ADD_EV_REPO/";
        public const string EXT_REPORT_REPORootPath = "~/Content/DocuPathRepositories/EXT_REPORT_REPO/";
        public const string MEDIA_REPORootPath = "~/Content/DocuPathRepositories/MEDIA_REPO/";

        #endregion
        //----------------------------------------------------------------------------------------------//
        #region FETCHES, GETS & QUERIES:
        public static List<NOTIFICATION> GetUnhandledNeurons()
        {
            List<NOTIFICATION> unhandledNeurons = new List<NOTIFICATION>();
            using (DocuPathEntities db = new DocuPathEntities())
            {
                var id = HttpContext.Current.User.Identity.GetUserId<int>();
                foreach (var neuron in db.NOTIFICATION.Where(x => x.UserID == id && x.HandledDateTimeStamp == null))
                {
                    unhandledNeurons.Add(neuron);
                }
            }
            return unhandledNeurons;
        }
        public static USER getCurrentUser()
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
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
        public static string GetUrl(string path)
        {
            var arr = path.Split('\\');
            string url = "";
            bool begin = false;
            foreach (var item in arr)
            {
                if (item == "Content")
                {
                    begin = true;
                }
                if (begin)
                {
                    url += item + "/";
                }
            }
            url = url.Remove(url.Length - 1);
            return url;
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region FORMATTING & MARKUP:
        public static string ExtractMediaFileName(string inLocation)
        {
            string location = inLocation;
            string filename = inLocation.Substring(inLocation.LastIndexOf('/',0));

            if (filename == null)
                return "File Name Null";
            else
                return filename;
        }

        public static string PhoneNumberMarkup(string inNumber)
        {
            return "<strong><span style=\"color: #666\">(" + inNumber.Substring(0, 3) + ")</span> " + inNumber.Substring(3, 3) + " " + inNumber.Substring(6, 4) + "</strong>";
        }

        public static string EmailMarkup(string inEmail)
        {
            var address = inEmail.ToLower();
            var idxAt = address.IndexOf('@');
            return "<strong><span style=\"color: #666\">" + address.Substring(0, idxAt) + "</span><span style=\"color: rgba(174,31,31,1)\">" + address.Substring(idxAt, 1) + "</span>" + address.Substring(idxAt + 1) + "</strong>";
        }

        public static string ChunkString(string inRaw, int inChunkFactor, string inSeparator)
        {
            var stringToChunk = inRaw;
            string separator = inSeparator;

            for (int i = inChunkFactor; i <= stringToChunk.Length; i += inChunkFactor)
            {
                stringToChunk = stringToChunk.Insert(i, separator);
                i++;
            }

            return stringToChunk;
        }

        public static string SplitAddress(string inAddress, char inSplitter, char inTrimmer, string inHTMLtag)
        {
            string[] addressLines = inAddress.Split(inSplitter);

            foreach (var entry in addressLines)
            {
                entry.Trim(inTrimmer);
            }

            string SplitAddress = "";

            foreach (var entry in addressLines)
            {
                SplitAddress += inHTMLtag + entry + inHTMLtag.Insert(1, "/");
            }

            return SplitAddress;
        }

        public static string StatusMarkup(string inStatusText, string inMarkupClassName)
        {
            return "<span class=\"" + inMarkupClassName + "\">" + inStatusText + "</span>";
        }

        public static string RemoveInvalidCharacters(string inRaw)
        {
            char[] invalidChars = new char[] {'\\','/', ':', '*', '?', '\"', '>', '<', '|', '.', '_' } ;

            foreach (char checkChar in inRaw)
            {
                if (invalidChars.Contains(checkChar))
                {
                    inRaw = inRaw.Remove(inRaw.IndexOf(checkChar),1);
                }
            }
            return inRaw;
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
    }
}