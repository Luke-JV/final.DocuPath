using DocuPath.DataLayer;
using DocuPath.Models.Custom_Classes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using System.Configuration;
using Twilio.Types;
using context = System.Web.HttpContext;

namespace DocuPath.Models
{
    public static class VERTEBRAE
    {
        #region OUTGOING COMMUNICATION:
        public static void sendMail(string destination, string content,string subject)
        {
            //404 - mail logic here
            MailMessage outMail = new MailMessage();
            outMail.To.Add(destination);
            outMail.From = new MailAddress("u13098536@tuks.co.za");
            outMail.Subject = subject;
            // TODO: 404 proper mail body html markup here
            outMail.Body = content;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("u13098536@tuks.co.za", xTx);
            smtp.Send(outMail);

        }
        public static void sendSMS(string destination, string content)
        {
            //404 - SMS logic here
            string accountSid = ConfigurationManager.AppSettings["SMSAccountIdentification"];

            string authToken = ConfigurationManager.AppSettings["SMSAccountPassword"];

            string fromNumber = ConfigurationManager.AppSettings["SMSAccountFrom"];

            // Initialize the Twilio client

            TwilioClient.Init(accountSid, authToken);

            MessageResource result = MessageResource.Create(

                    from: new PhoneNumber(fromNumber),

                    to: new PhoneNumber(destination),

                    body: content);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region CONSTANTS:
            #region ROOT PATHS:
            // Functions:
            public const string ErrorDumpRootPath = "~/Content/DocuPathRepositories/ERRORDUMPS/";
            // Repositories:
            public const string LC_REPORootPath = "~/Content/DocuPathRepositories/LC_REPO/";
            public const string ERC_REPORootPath = "~/Content/DocuPathRepositories/ERC_REPO/";
            public const string ADD_EV_REPORootPath = "~/Content/DocuPathRepositories/ADD_EV_REPO/";
            public const string EXT_REPORT_REPORootPath = "~/Content/DocuPathRepositories/EXT_REPORT_REPO/";
            public const string MEDIA_REPORootPath = "~/Content/DocuPathRepositories/MEDIA_REPO/";
            #endregion
            #region FILE / MEDIA HANDLING:
            public static List<string> Thumbnail_AcceptedFileTypes()
            {
                List<string> outList = new List<string>();

                outList.Add(".JPG");
                outList.Add(".JPEG");
                outList.Add(".JPE");
                outList.Add(".JIF");
                outList.Add(".JFIF");
                outList.Add(".JFI");
                outList.Add(".BMP");
                outList.Add(".PNG");
                outList.Add(".GIF");
                outList.Add(".TIF");
                outList.Add(".TIFF");

                return outList;
            }
        #endregion
        #region AUDIT TRAIL DESCRIPTIONS:
        const string Uncategorized = "Performed an uncategorised operation. "; // 0
        const string InitiateAdd = "Initiated an Add operation using _."; // 1
        const string SuccessfulAdd = "Successfuly completed an Add operation using _."; // 2
        const string FailedAdd = "Failed to complete an Add operation using _. Failure occurred with the following error message: *"; // 3
        const string InitiateSearch = "Initiated a Search operation using _."; // 4
        const string SuccessfulSearch = "Successfuly completed a Search operation using _."; // 5
        const string FailedSearch = "Failed to complete a Search operation using _. Failure occurred with the following error message: *"; // 6
        const string InitiateView = "Initiated a View operation using _. The ID of the record viewed is *."; // 7
        const string SuccessfulView = "Successfuly completed a View operation using _. The ID of the record viewed is *."; // 8
        const string FailedView = "Failed to complete a View operation using _. Failure occurred with the following error message: *"; // 9
        const string InitiateUpdateEdit = "Initiated a View operation using _. The ID of the record viewed is *."; // 10
        const string SuccessfulUpdateEdit = "Successfuly completed a View operation using _. The ID of the record viewed is *."; // 11
        const string FailedUpdateEdit = "Failed to complete a View operation using _. Failure occurred with the following error message: *"; // 12


        #endregion
        #region NEURON DETAILS:
        // TODO
        #endregion
        #region EMAIL NOTIFICATION CONTENTS & MARKUP:
        // TODO
        #endregion
        #region SMS NOTIFICATION CONTENTS & MARKUP:
        // TODO
        #endregion
        #region HAPTIC HELP CONTENT:
        const string[,] x = new string[];
        string AllFCSearchByKeyword = "<ol class=\"haptic-ol\"><li>Ensure the target column is visible using the column selector menu.</li><li>Click the '<kbd>Enter a keyword/phrase...</kbd>' textbox.</li><li>Type the desired keyword or phrase to search by(<kbd>minimum of 3 characters, filtering occurs automatically after 250ms delay</kbd>).</li></ol>";
        #endregion
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
            unhandledNeurons = unhandledNeurons.OrderByDescending(x => x.DateID).ToList();
            return unhandledNeurons;
        }
        public static USER getCurrentUser()
        {
            DocuPathEntities db = new DocuPathEntities();

            int id = HttpContext.Current.User.Identity.GetUserId<int>();
            USER currentUser = db.USER.Where(x => x.UserID == id).FirstOrDefault();
            currentUser.USER_LOGIN = db.USER_LOGIN.Where(x => x.UserLoginID == currentUser.UserLoginID).FirstOrDefault();
            currentUser.TITLE = db.TITLE.Where(x => x.TitleID == currentUser.TitleID).FirstOrDefault();

            return currentUser;

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
            try
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
            catch (Exception)
            {

                return path;
            }
        }
        public static string GetThumbUrl(string inPath)
        {
            try
            {

                string relPath = GetUrl(inPath);
                return relPath.Insert(relPath.LastIndexOf('.'), "_thumb");

            }
            catch (Exception)
            {

                return inPath;
            }
            //404!

        }
        public static void GenerateAndSaveThumb(string fetchFromPath)
        {
            if (fetchFromPath != null && Thumbnail_AcceptedFileTypes().Contains(Path.GetExtension(fetchFromPath).ToUpper()) && File.Exists(fetchFromPath))
            {
                Image thumb;
                //var fileName = fetchFromPath.Substring(fetchFromPath.LastIndexOf('/'));

                Image img = Image.FromFile(fetchFromPath);

                string destinationPath = fetchFromPath.Insert(fetchFromPath.LastIndexOf('.'), "_thumb"); ;

                int imgHeight = 150;
                int imgWidth = 150;
                if (img.Width < img.Height)
                {
                    //portrait image  
                    imgHeight = 150;
                    var imgRatio = (float)imgHeight / (float)img.Height;
                    imgWidth = Convert.ToInt32(img.Height * imgRatio);
                }
                else if (img.Height < img.Width)
                {
                    //landscape image  
                    imgWidth = 150;
                    var imgRatio = (float)imgWidth / (float)img.Width;
                    imgHeight = Convert.ToInt32(img.Height * imgRatio);
                }

                thumb = img.GetThumbnailImage(imgWidth, imgHeight, () => false, IntPtr.Zero);
                thumb.Save(destinationPath);
            }
        }
        public static List<string> GetHelp(string inPageName)
        {
            switch (inPageName)
            {
                case "All Forensic Cases":
                    BuildHapticHelp(inPageName);
                    break;
                default:
                    break;
            }
        }
        public static List<string> BuildHapticHelp(string inPageName)
        {
            List<string> FAQs = new List<string>();
            return FAQs;
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

        public static double CalcRoundedPercentage(int inNumerator, int inDenominator)
        {
            return Math.Round(Convert.ToDouble(inNumerator) / Convert.ToDouble(inDenominator), 0);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region ERROR HANDLING:
        public static string DumpErrorToTxt(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;
            string lineNum, errorMsg, exceptionType, exceptionURL, hostIp, errorLocation;

            lineNum = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            errorMsg = ex.GetType().Name.ToString();
            exceptionType = ex.GetType().ToString();
            exceptionURL = context.Current.Request.Url.ToString();
            errorLocation = ex.Message.ToString();
            hostIp = context.Current.Request.UserHostAddress.ToString() + " " + context.Current.Request.UserHostName.ToString();

            try
            {
                string filepath = ErrorDumpRootPath;  //Text File Path
                string fname = @"\ErrorDump_User." + VERTEBRAE.getCurrentUser().DisplayInitials.ToUpper() + "_Date." + DateTime.Now.Date.ToString("ddMMMYYYY") + "_Time." + DateTime.Now.TimeOfDay.ToString("HHhmmss") + ".txt";

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + fname;   //Text File Name

                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Error Dump DateTimeStamp:" + " " + DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") + line + "Error Line Number:" + " " + lineNum + line + "Error Message:" + " " + errorMsg + line + "Exception Type:" + " " + exceptionType + line + "Error Location:" + " " + errorLocation + line + " Error Origin URL:" + " " + exceptionURL + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("[[------- ERROR LOG FILE FOR " + DateTime.Now.ToString("dd MMM yyyy at HH:mm:ss") + " --------------]]");
                    sw.WriteLine(">---------------------------<<< START >>>---------------------------<");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine(">----------------------------<<< END >>>----------------------------<");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
                return filepath;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//

        #region x
        #region y
        public static string xTx = "Ktm200xcw";  
        #endregion
        #endregion
    }

}