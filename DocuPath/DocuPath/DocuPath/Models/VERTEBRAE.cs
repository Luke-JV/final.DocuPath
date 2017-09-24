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
using System.Data.Entity.SqlServer;

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
            #region HAPTIC PAGE INFO:
            const string AllFCPageInfo = "<ul class=\"haptic-ul\"> <li style=\"font-weight: bold\">This page displays an interactive table containing all Forensic Cases in the DocuPath system that you are eligible to access.</li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden/Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Editing</kbd>, <kbd>Deleting</kbd>, <kbd>Locking</kbd>, <kbd>Unlocking</kbd> and <kbd>Migrating</kbd> an item is provided in the last column of the table for each Forensic Case displayed.</li> </ul>";
            const string HapticCentrePageInfo = "<ul class=\"haptic-ol\"> <li>This page displays all available HAPTIC help topics in the DocuPath system.</li> </ul> <br /> <ol class=\"haptic-ol\"><li>Start by selecting the sphere of the system in which you require help - either <kbd>CORTEX</kbd>, <kbd>CORNEA</kbd> or <kbd>PULSE</kbd>.</li> <li>Next, select the function within the selected sphere for which you require help - e.g. <kbd>Forensic Cases</kbd>.</li> <li>Next, expand the panel for the task for which you require help - e.g. <kbd>Add Forensic Case</kbd>.</li> <li>If numerous subtopics exist for the selected task, a <ctrl>Search Bar</ctrl> control similar to that found on the page-specific HAPTIC Help Centre <ctrl>sidenav</ctrl> is shown to assist with drilling down into subtopics.</li> <li>If the subtopics for the selected task have already been included elsewhere, a link to the relevant location will be displayed.</li> </ol>";
            #endregion
            #region HAPTIC HELP CONTENT:
            const string AllPageGenericTopics = "<div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Search or filter</strong> using a specific keyword/phrase.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#SearchFilter\">TRY THE FOLLOWING:</div> <div id=\"SearchFilter\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Ensure the target column is visible using the <ctrl>Column Selector</ctrl> dropdown menu, located to the right of the <ctrl>Search Bar</ctrl>.</li> <li>Click the '<kbd>Enter a keyword/phrase...</kbd>' field on the <ctrl>Search Bar</ctrl>.</li> <li>Type the desired keyword or phrase to search/filter by.</li> <li>The table adapts automatically to only display records where the provided query string is present in any of the currently visible columns.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">A minimum of one (1) character is required to initiate a search.</li> <li class=\"colour-dpred\">Clearing the contents of the <ctrl>Search Bar</ctrl> will reset the filtering and reset the table to display all entries (as determined by the current value of the <ctrl>Row Count Selector</ctrl> dropdown menu).</li> <li class=\"colour-dpred\">Filtering occurs automatically after a 250ms delay - no interaction other than entering a search query is needed.</li> <li class=\"colour-dpred\">Filtering is based only on visible columns - invisible columns will not be included in the search query.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\">Change the <strong class=\"catchphrase-highlight\">columns</strong> that are displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#SelectColumns\">TRY THE FOLLOWING:</div> <div id=\"SelectColumns\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Open the <ctrl>Column Selector</ctrl> dropdown menu.</li> <li>Tick/untick the desired column(s) by using the checkbox next to the relevant column(s)</li> <li>The table adapts automatically to display the ticked column(s) and hide the unticked column(s).</li> </ol> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\">Change the <strong class=\"catchphrase-highlight\">number of entries</strong> displayed per page.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#NumberOfEntries\">TRY THE FOLLOWING:</div> <div id=\"NumberOfEntries\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Open the <ctrl>Row Count Selector</ctrl> dropdown menu, located to the right of the <ctrl>Search Bar</ctrl>.</li> <li>Select either <kbd>10</kbd>, <kbd>25</kbd>, <kbd>50</kbd> or <kbd>All</kbd> to display the corresponding number of items per page.</li> <li>The table adapts automatically to display the selected number of items per page.</li> <li>The <ctrl>results counter</ctrl> control to the right below the table displays the number of currently displayed items on this page out of the total number of items.</li> <li>Use the <ctrl>page navigation</ctrl> control to the left below the table to navigate between pages.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Note that some pages (due to the number of records that must be displayed on the page) do not have an <kbd>All</kbd> option. On these pages, the available item counts per page are <kbd>10</kbd>, <kbd>25</kbd>, <kbd>50</kbd> and <kbd>100</kbd>.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\">Change the table's <strong class=\"catchphrase-highlight\">display density</strong>.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#DisplayDensity\">TRY THE FOLLOWING:</div> <div id=\"DisplayDensity\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Click the <ctrl>Toggle Table Density</ctrl> button on the <ctrl>Page Actions</ctrl> button bar to the right of the Page Title.</li> <li>The table cycles through three (3) levels of display density for the table (namely <kbd>Normal</kbd>, <kbd>Condensed</kbd> and <kbd>Minimal</kbd>), compressing or expanding the information displayed.</li> </ol> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Sort</strong> the information displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#SortAscDesc\">TRY THE FOLLOWING:</div> <div id=\"SortAscDesc\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Decide on the desired column to sort by, and whether <kbd>Ascending</kbd> or <kbd>Descending</kbd> sort order is required.</li> <li>Click the column title of the desired column.</li> <li>A <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-up colour-dpred\"></i></kbdicon> icon appears to the right of the column heading, indicating an <kbd>Ascending</kbd> sort order.</li> <li>The table adapts automatically, sorting the currently displayed items according to specified column, in <kbd>Ascending</kbd> order.</li> <li>Clicking the column title of the desired column again changes the displayed icon to the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-down colour-dpred\"></i></kbdicon> icon, indicating a <kbd>Descending</kbd> sort order.</li> <li>The table adapts automatically, sorting the currently displayed items according to specified column, in <kbd>Descending</kbd> order.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Continuously clicking on a column's title will repeatedly switch between <kbd>Ascending</kbd> and <kbd>Descending</kbd> sort order.</li> <li class=\"colour-dpred\">Note that, on page load, the items displayed in the table are sorted by default according to a pre-defined column using a pre-defined sort order.</li> <li class=\"colour-dpred\">The items displayed in the table cannot be sorted according to the values of the <kbd>Actions</kbd> column.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">View </strong> a single item displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#ViewItem\">TRY THE FOLLOWING:</div> <div id=\"ViewItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon.</li> <li>The system redirects to the relevant item's detail page, where all of the information relating to the item is displayed.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">The <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon displays the <ctrl>VIEW</ctrl> tooltip when hovered over.</li> <li class=\"colour-dpred\">To view the item's detailed information in a new browser tab, right click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon and click <kbd>Open link in new tab</kbd>.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Delete</strong> a single item displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#DeleteItem\">TRY THE FOLLOWING:</div> <div id=\"DeleteItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-delete colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-delete colour-dpred\"></i></kbdicon> icon.</li> <li>The system displays a <ctrl>Modal</ctrl> confirmation notification confirming deletion of the relevant item.</li> <li>To proceed with deleting the item, click the <ctrl>Yes</ctrl> button.</li> <li>To cancel deleting the item, click the <ctrl>No</ctrl> button.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Deletion of an item by clicking <ctrl>Yes</ctrl> on the <ctrl>Modal</ctrl> confirmation notification is irreversible from the system frontend - the item will be archived and is only retrievable by contacting a system administrator.</li> <li class=\"colour-dpred\">In some cases, item deletions require authorisation by a Superuser. In these cases, the <ctrl>Modal</ctrl> confirmation notification is still shown, but the item's status is changed to <kbd>Pending</kbd> and a Superuser has to approve the deletion before the status is changed to <kbd>Archived</kbd>.</li> </ul> </div> </div> </div>";
            const string AllFCSpecificTopics = "<div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Migrate</strong> a single item to a new owner.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#MigrateItem\">TRY THE FOLLOWING:</div> <div id=\"MigrateItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-migrate colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-migrate colour-dpred\"></i></kbdicon> icon.</li> <li>The system navigates to the <kbd>Migrate Forensic Case</kbd> page, from where the migration operation can be completed.</li> </ol> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Lock</strong> a single item to prevent further changes.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#LockItem\">TRY THE FOLLOWING:</div> <div id=\"LockItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-lock colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-lock colour-dpred\"></i></kbdicon> icon.</li> <li>The system displays a <ctrl>Modal</ctrl> confirmation notification confirming locking of the relevant item.</li> <li>To proceed with locking the item, click the <ctrl>Yes</ctrl> button, or simply cancel locking the item by clicking the <ctrl>No</ctrl> button.</li> <li>The system changes the status of the item in question to <kbd>Locked</kbd>, disallowing any further changes.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Locking an item will prevent any further changes to the item. In the event where additional evidence or other circumstances arise (that require changes to a locked item), the item will need to be unlocked via Superuser authorisation prior to changes being allowed.</li> <li class=\"colour-dpred\">Refer to the HAPTIC topic on unlocking items for more information.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Unlock</strong> a single item to allow further changes.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#UnlockItem\">TRY THE FOLLOWING:</div> <div id=\"UnlockItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-unlock colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-unlock colour-dpred\"></i></kbdicon> icon.</li> <li>The system displays a <ctrl>Modal</ctrl> confirmation notification confirming unlocking of the relevant item.</li> <li>To proceed with unlocking the item, click the <ctrl>Yes</ctrl> button, or simply cancel unlocking the item by clicking the <ctrl>No</ctrl> button.</li> <li>If the <ctrl>Yes</ctrl> button was clicked, the <ctrl>Modal</ctrl> confirmation notification changes to a <ctrl>Motivated Modal</ctrl> confirmation notification, requesting a reason for the attempted item unlock.</li> <li>Type a reason into the <kbd>Provide a reason...</kbd> field.</li> <li>Click the <ctrl>Finalise</ctrl> button.</li> <li>The system changes the status of the item in question to <kbd>Pending</kbd>, and awaits Superuser approval on the item unlock.</li> <li>Once the Superuser has approved the item unlock, the system will allow further changes to be made to the item.</li> </ol> </div> </div> </div>";
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
                // ======= FORENSIC CASES (GROUP ID: 1) =======
                METRIC vmFCCount = new METRIC(1, true, "mdl2icon mdl2-fc metric-mdl2icon", "All Cases", db.FORENSIC_CASE.Count().ToString(), "cases", "in total", "The total number of Forensic Cases in the DocuPath database, regardless of case status.", System.DateTime.Now);
                visionMetrics.Add(vmFCCount);

                // --> COUNT OF FC BY STATUS:
                // Active:
                METRIC vmActiveFCCount = new METRIC(1, false, "mdl2icon mdl2-done metric-mdl2icon", "Active Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Active").Count().ToString(), "cases", "active", "The number of active Forensic Cases in the DocuPath database.", System.DateTime.Now);
                visionMetrics.Add(vmActiveFCCount);
                // Pending:
                METRIC vmPendingFCCount = new METRIC(1, false, "mdl2icon mdl2-warning metric-mdl2icon", "Pending Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Pending").Count().ToString(), "cases", "pending", "The number of pending Forensic Cases in the DocuPath database.", System.DateTime.Now);
                visionMetrics.Add(vmPendingFCCount);
                // Archived:
                METRIC vmArchivedFCCount = new METRIC(1, false, "mdl2icon mdl2-delete metric-mdl2icon", "Archived Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Archived").Count().ToString(), "cases", "archived", "The number of archived Forensic Cases in the DocuPath database.", System.DateTime.Now);
                visionMetrics.Add(vmArchivedFCCount);
                // Locked:
                METRIC vmLockedFCCount = new METRIC(1, false, "mdl2icon mdl2-lock metric-mdl2icon", "Locked Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Locked").Count().ToString(), "cases", "locked", "The number of locked Forensic Cases in the DocuPath database.", System.DateTime.Now);
                visionMetrics.Add(vmLockedFCCount);
                // Average Duration
                var avg = db.FORENSIC_CASE.Where(fc => fc.DateClosed.HasValue).Average(fc => SqlFunctions.DateDiff("month", fc.DateClosed, fc.DateAdded));
                METRIC vmAvgFCDuration = new METRIC(1, false, "mdl2icon mdl2-time metric-mdl2icon", "Average Duration", avg.ToString(), "months", "open", "The average duration of a typical Forensic Case in the DocuPath database.", System.DateTime.Now);
                visionMetrics.Add(vmAvgFCDuration);
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
                //// --> COUNT OF LC BY DATE CLOSED:
                //ViewBag.LCClosedToday = db.LEGACY_CASE.Where(fc => fc.DateClosed == dtToday);
                //ViewBag.LCClosedPastSevenDays = db.LEGACY_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-7)).Count();
                //ViewBag.LCClosedPastThirtyDays = db.LEGACY_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-30)).Count();
                //ViewBag.LCClosedPastSixtyDays = db.LEGACY_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-60)).Count();
                //ViewBag.LCClosedPastNinetyDays = db.LEGACY_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > dtToday.AddDays(-90)).Count();

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
        public static string GetHelp(string inPageName)
        {
            switch (inPageName)
            {
                case "AllGeneric":
                    return AllPageGenericTopics;
                case "AllFCSpecific":
                    return AllFCSpecificTopics;              
                default:
                    return null;
            }
        }
        public static string GetOnThisPageInfo(string inPageName)
        {
            switch (inPageName)
            {
                case "HapticHelpCentre":
                    return HapticCentrePageInfo;
                case "AllFC":
                    return AllFCPageInfo;
                default:
                    return null;
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