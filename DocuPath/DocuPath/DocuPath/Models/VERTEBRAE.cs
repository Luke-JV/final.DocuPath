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
        public static void sendMail(string destination, string content, string subject)
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
            #region NON-DATABASE SYSTEM PARAMETERS:
                public const int lockoutDelayMinutes = 15;
                public const int maxSRPerFCAddUpdate = 10;
                public const int maxAEPerFCAddUpdate = 10;
            #endregion
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
            const string AddUAL = "<ul class=\"haptic-ul\"> <li><strong>This page displays the details required to add a Access Level to the DocuPath system.</strong></li> <li> <kbd>Level details</kbd>, <kbd>Function group</kbd> setup as well as <kbd>access area setup</kbd> are required to successfully add the Access Level. </li> </ul>";
            const string AllUAL = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all Access Levels in the DocuPath system that the user is eligible to access.  </strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Editing</kbd>, and <kbd>Toggling</kbd> the Access Level accessibility is provided in the last column of the table for each Access level displayed.</li> <li>The option to add a new Access Level is available from the action bar on this page.</li> </ul>";
            const string ViewUAL = "<ul class=\"haptic-ul\"> <li><strong>This page displays the full set of detailed information of the selected Access Level.</strong></li> <li><kbd>Access Level</kbd> details as well as <kbd>Function Group & access Area</kbd> Details are displayed.</li> <li>No changes can be made to the Access Level from this page.</li> </ul>";
            const string UpdateUAL = "";
            const string AllAT = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all Audit Log Entries in the DocuPath system that the user is eligible to access.</strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>No access to functionality is available in the table. </li> </ul>";
            const string ViewAuditTrail = "";
            const string AddCT = "<ul class=\"haptic-ul\"> <li><strong>This page displays the details required to add a Content Tag to the DocuPath system.</strong></li> <li><kbd>Tag Text</kbd>, <kbd>Tag Category</kbd>, <kbd>Tag Subcategory</kbd> and <kbd>Tag Condition</kbd> are required to successfully add the Content Tag.</li> </ul>";
            const string AllCT = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all CDC ICD10 Content Tags in the DocuPath system that the user is eligible to access.</strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Editing</kbd>, <kbd>Deleting</kbd>, an item is provided in the last column of the table for each Content Tag displayed.</li> <li>The option to add a new Content Tag or update the Content Tag Repository is available from the actions bar on this page.</li> </ul>";
            const string ViewCT = "<ul class=\"haptic-ul\"> <li><strong>This page displays the full set of detailed information of the selected Content Tag at the current point in time.</strong></li> <li><kbd>Content Tag details</kbd> as well as the <kbd>Tag Category</kbd>, <kbd>Tag Subcategory</kbd> and <kbd>Tag Condition</kbd> are displayed.</li> <li>No changes can be made to the Content Tag from this page.</li> </ul>";
            const string UpdateCT = "";
            const string UpdateCTRepo = "<ul class=\"haptic-ul\"> <li><strong>This page displays the facility to Update the Content Tag Repository.</strong></li> <li>Content Tag Files are uploaded via the <kbd>Browse</kbd> button.</li> <li>The <ctrl>File Upload</ctrl> control on this page is set up to warn the user of any file incompatibilities, size restrictions, upload quantity restrictions or other applicable limitations.</li> </ul>";
            const string AddERC = "<ul class=\"haptic-ul\"> <li><strong>This page captures the details required to add an External Review Case to the DocuPath system.  </strong></li> <li>Each captured External Review Case requires a <kbd>DR Number</kbd> and an <kbd>assignee</kbd> to be successfully added. </ul>";
            const string AllERC = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all External Review Cases in the DocuPath system that the user is eligible to access. </strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd> <kbd>Locking</kbd>, <kbd>Unlocking</kbd>, <kbd>Editing</kbd> and <kbd>Archiving</kbd> an item is provided in the last column of the table for each External Review Case displayed.</li> <li>The option to add a new External Review case is available from this page.</li> </ul>";
            const string ViewERC = "<ul class=\"haptic-ul\"> <li><strong>This page displays the complete set of detailed information for the selected External Review Case at the current point in time. </strong></li> <li>Each <ctrl>section</ctrl> contains the current set of information relating to that area of the External Review Case. </li> <li>No changes can be made to the External Review Case from this page.</li> </ul>";
            const string UpdateERC = "";
            const string AddFC = "<ul class=\"haptic-ul\"> <li><strong>This page displays the DocuPath Forensic Case Builder.</strong></li> <li>The Forensic Case builder is divided into <ctrl>TABS</ctrl> to guide you through the logical process of compiling a Forensic Case.</li> <li>Each <ctrl>TAB</ctrl> contains fields relevant to different aspects of an autopsy and requires different sets of critical data – i.e. compulsory data that needs to be provided in order for a Forensic Case to be saved successfully. </li> <li>The <ctrl>TABS</ctrl> consist of: </li> <li class=\"indented\"> <Strong> Core Data </Strong>– provides entry for critical data that identifies each Forensic Case.</li> <li class=\"indented\"> <Strong>Observations</Strong> – records all entries necessary for the Forensic Case from the autopsy that has been performed. </li> <li class=\"indented\"><Strong>Service Requests </Strong>– allows for the tracking and monitoring of external Service Requests e.g. Toxicology, Haematology etc.  </li> <li class=\"indented\"><Strong>Media Items</Strong> – provides the facility to upload images, videos, audio files etc. to support findings in the Forensic Case.  </li> <li class=\"indented\"><Strong>Additional Evidence</Strong> – provides the facility to upload any additional evidence (multimedia) to support findings in the Forensic Case.  </li> <li class=\"indented\"><Strong>Cause of Death </Strong>– provides the facility to enter a primary cause of death as well as the option to record a secondary, tertiary etc. causes of death. </li> <li class=\"indented\"><Strong>Statistics</Strong> – provides the facility to enter statistics from the case that will allow for statistical and data analysis on Forensic Cases.</li> </ul>";
            const string AllFC = "<ul class=\"haptic-ul\"> <li style=\"font-weight: bold\">This page displays an interactive table containing all Forensic Cases in the DocuPath system that you are eligible to access.</li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden/Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Editing</kbd>, <kbd>Deleting</kbd>, <kbd>Locking</kbd>, <kbd>Unlocking</kbd> and <kbd>Migrating</kbd> an item is provided in the last column of the table for each Forensic Case displayed.</li> </ul>";
            const string ViewFC = "<ul class=\"haptic-ul\"> <li><strong>This page displays the complete set of detailed information for the selected Forensic Case at the current point in time. </strong> </li> <li>The <kbd>sections</kbd> on this page divide the Forensic Case into the same sections as the Forensic Case Builder.</li> <li>Each <kbd>section</kbd> contains the current set of information relating to that area of the Forensic Case. </li> <li>The current state of the displayed Forensic Case can be compiled into a report using the <kbd>Compile Report</kbd> button on the Page Actions bar next to the page heading.</li> <li>No changes can be made to the Forensic Case from this page.</li> </ul>";
            const string UpdateFC = "<ul class=\"haptic-ul\"> <li> <strong>This page displays all the current information in the selected Forensic Case.  </strong> </li> <li>Similarly, to Adding a Forensic Case, an option to edit items in a Forensic Case is available. </li> <li>Certain edits are restricted for data integrity purposes. </li> </ul>";
            const string MigrateFC = "<ul class=\"haptic-ul\"> <li> <strong>This page displays the current author details for the selected Forensic Case and the details of the intended new author. </strong> </li> <li>The details of the new author are required for successful migration to take place. </li> </ul>";
            const string HapticHelpCentre = "<ul class=\"haptic-ol\"> <li>This page displays all available HAPTIC help topics in the DocuPath system.</li> </ul> <br /> <ol class=\"haptic-ol\"><li>Start by selecting the sphere of the system in which you require help - either <kbd>CORTEX</kbd>, <kbd>CORNEA</kbd> or <kbd>PULSE</kbd>.</li> <li>Next, select the function within the selected sphere for which you require help - e.g. <kbd>Forensic Cases</kbd>.</li> <li>Next, expand the panel for the task for which you require help - e.g. <kbd>Add Forensic Case</kbd>.</li> <li>If numerous subtopics exist for the selected task, a <ctrl>Search Bar</ctrl> control similar to that found on the page-specific HAPTIC Help Centre <ctrl>sidenav</ctrl> is shown to assist with drilling down into subtopics.</li> <li>If the subtopics for the selected task have already been included elsewhere, a link to the relevant location will be displayed.</li> </ol>";
            const string Home = "";
            const string AddLC = "<ul class=\"haptic-ul\"> <li><strong>This page captures the details required to add a External Legacy Case to the DocuPath system.</strong></li> <li>Each captured Legacy Case requires a <kbd>DR Number</kbd> and a Legacy Case file to be successfully added.</li> <li>The <CTRL>File Upload</CTRL> control on this page is set up to warn the user of any file incompatibilities , size restrictions, upload quantity restrictions or other applicable limitations.</li> </ul>";
            const string AllLC = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all the Legacy Cases in the DocuPath system that the user is eligible to access.  </strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Editing</kbd> and <kbd>Deleting</kbd>, an item is provided in the last column of the table for each Legacy Case displayed.</li> <li>The option to add a new Legacy Case is available from  the page actions bar on this page. </li> </ul>";
            const string ViewLC = "<ul class=\"haptic-ul\"> <li><strong>This page displays all the current information in the selected Legacy Case at the current point in time. </strong></li> <li>Legacy Case details as well as all Legacy Case Documents are all accessible from this page. </li> <li>No changes can be made to the Legacy Case from this page.</li> </ul>";
            const string UpdateLC = "";
            const string AddMedia = "<ul class=\"haptic-ul\"> <li><strong>This page displays all the Media items in the system that were recently uploaded into the Media Repository, but for which metadata has not been captured yet.</strong></li> <li>The system displays a metadata capture panel for each of these <kbd>Pending items</kbd> in the Media Repository.</li> <li><kbd>Accessibility</kbd>, <kbd>Captions</kbd>, <kbd>Description</kbd>, <kbd>Purpose</kbd> as well as <kbd>Media Tags</kbd> are allocated on this page.</li> </ul>";
            const string AllMedia = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all Media Items in the DocuPath system that the user is eligible to access. </strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Maintaining Content Tags</kbd> and <kbd>Deleting</kbd> an item is provided in the last column of the table for each Media item displayed.</li> <li>The option to add a new Media item is available from the page actions bar on this page.</li> </ul>";
            const string ViewMedia = "<ul class=\"haptic-ul\"> <li><strong>This page displays all the current information on the selected Media Item at the current point in time.</strong></li> <li>A large thumbnail of the image is made available.</li> <li>In depth details pertaining to the Media item is displayed.</li> <li>All associated Content Tags are visible with details pertaining to the Content Tag code with a hover of the mouse.</li> <li>No changes can be made to the Media item from this page.</li> </ul>";
            const string UpdateMedia = "<ul class=\"haptic-ul\"> <li><strong>This page displays the current set of Content Tags linked to the specific Media item.</strong></li> <li>An enlarged thumbnail of the Media item is displayed to aid precise tagging.</li> <li>At least one Content Tag needs to be linked to a Media item.</li> </ul>";
            const string UploadMedia = "<ul class=\"haptic-ul\"> <li><strong>This page is used to select and upload media files to the Media Repository.</strong></li> <li>Media items are dragged and dropped into the demarcated area or can be uploaded via the <kbd>Browse button</kbd>.</li> <li>Media items are previewed in the demarcated are where the user can <kbd>Delete</kbd> or <kbd>Preview</kbd> the Media item before proceeding with the upload process.</li> <li>The <CTRL>File Upload</CTRL> control on this page is set up to warn the user of any file incompatibilities , size restrictions, upload quantity restrictions or other applicable limitations.</li> </ul>";
            const string INSIGHT = "<ul class=\"haptic-ul\"> <li><strong> This page displays the DocuPath INSIGHT Reporting Builder. </strong></li> <li>Customised reports are designed and generated to the user's specific needs and requirements. </li> <li>A detailed set of timeframe and other criteria controls allows for retrieval of precise and accurate information for useful reports.</li> </ul>";
            const string VISION = "<ul class=\"haptic-ul\"> <li><strong>This page displays the DocuPath VISION System Dashboard – DocuPath's own built-in business intelligence (BI) platform.</strong></li> <li><strong>The dashboard consists of an interactive set of <CTRL>tiles</CTRL> containing information on various aspects of the DocuPath system (for example Forensic Cases, External Review Cases, Legacy Cases, general system statistics, media statistics, to name a few).</strong></li> <li>Hovering over <ctrl>tiles</ctrl> in the dashboard displays more information regarding each statistic, which is referred to as a <kbd>metric<kbd>.</li> <li>The system function from where the <kbd>metric</kbd> originates can be accessed and explored directly by hovering over the icon in the top right corner of each <ctrl>tile</ctrl>.</li> <li> Each subcategory of metrics also allows access to selected visual metrics that allow for a visual representation of (and drill-down into) the system data. </li> </ul>";
            const string Calendar = "";
            const string DAS = "<ul class=\"haptic-ul\"> <li><strong>This page is used to select and upload Daily Autopsy Schedules or to Retrieve a Daily autopsy schedule from a specific historic date.</strong></li> <li>Daily Autopsy Schedules are uploaded via the <kbd>Browse</kbd> button.</li> <li>The <ctrl>File Upload</ctrl> control on this page is set up to warn the user of any file incompatibilities, size restrictions, upload quantity restrictions or other applicable limitations.</li> </ul>";
            const string MDR = "<ul class=\"haptic-ul\"> <li><strong>This page displays the current state of the Monthly Duty Roster.</strong></li> <li>Duty roster allocations are scheduled on a month to month basis and are compiled on this page according to the <kbd>calendar date</kbd> and <kbd>autopsy slot</kbd>.</li> <li>Users are allocated directly to each slot.</li> </ul>";
            const string AddSP = "<ul class=\"haptic-ul\"> <li><strong>This page displays the details required to successfully add a Service Provider to the DocuPath system.</strong></li> <li><kbd>Company Details</kbd> as well as <kbd>Representative details</kbd> are required to successfully add the Service Provider.</li> </ul>";
            const string AllSP = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all service providers in the DocuPath system that the user is eligible to access.</strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for Viewing, Editing and Deleting an item is provided in the last column of the table for each Service Provider displayed.</li> <li>The option to add a new service provider is available from the page actions bar on this page.</li> </ul>";
            const string ViewSP = "<ul class=\"haptic-ul\"> <li><strong>This page displays the full set of detailed information of the selected Service Provider at the current point in time.  </strong></li> <li><kbd>Company details</kbd> as well as <kbd>representative details</kbd> are displayed. </li> <li>No changes can be made to the Service Provider from this page. </li> </ul>";
            const string UpdateSP = "<ul class=\"haptic-ul\"> <li><strong>This page displays the editable details of a Service Provider.</strong></li> <li>Only editable fields may be altered.</li> </ul>";
            const string AddSR = "<ul class=\"haptic-ul\"> <li><strong>This page displays the details required to add a Service Request to the DocuPath system.  </strong></li> <li><kbd>Service Request Details</kbd> and <kbd>Specimen Details</kbd> are required to successfully add a new Service Request to the DocuPath system.</li> </ul>";
            const string AllSR = "<ul class=\"haptic-ul\"> <li> <strong> This page displays an interactive table containing all Service Requests in the DocuPath system that the user is eligible to access. </strong> </li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li> Access to functionality for <kbd>Viewing</kbd>, <kbd>Cancelling</kbd> and <kbd>linking</kbd> a Service Request to a case is provided in the last column of the table for each Service Request displayed. </li> <li> The option to add a new Service Request is available from the page actions bar on this page. </li> </ul>";
            const string ViewSR = "<ul class=\"haptic-ul\"> <li> <strong> This page displays the full set of detailed information of the selected Service Request at the current point in time. </strong> </li> <li> <kbd>Service Request details</kbd> as well as <kbd>Specimen details</kbd> are displayed. </li> <li> No changes can be made to the Service Request from this page. </li> </ul>";
            const string LinkReport = "<ul class=\"haptic-ul\"> <li><strong>This page displays the details required to Link an External Report (received from a third-party Service Provider) to a Service Request on the DocuPath system.</strong></li> <li> Options to link a new report (<kbd>Link New Report</kbd> <ctrl>panel</ctrl>) or an existing report (<kbd>Link Existing Report</kbd> <ctrl>panel</ctrl>) are available. </li> </ul>";
            const string Error = "";
            const string UnauthorizedAccess = "";
            const string SystemParameters = "<ul class=\"haptic-ul\"> <li><strong>This page allows the user to maintain DocuPath's System Parameters.</strong></li> <li><kbd>Scheduling</kbd>, <kbd>Autopsy</kbd> and <kbd>Statistics</kbd> parameters are made available for viewing and editing as well as <kbd>Statuses & Types</kbd>.</li> </ul>";
            const string AllUsers = "<ul class=\"haptic-ul\"> <li><strong>This page displays an interactive table containing all User Profiles in the DocuPath system that the user is eligible to access.</strong></li> <li>Items in the table can be <kbd>Searched/Filtered</kbd>, <kbd>Sorted</kbd> in either ascending or descending order by a specific column, and columns can be <kbd>Hidden / Shown</kbd> as needed.</li> <li>Access to functionality for <kbd>Viewing</kbd>, <kbd>Editing</kbd> and <kbd>Toggling</kbd> the state of a User Profile is provided in the last column of the table for each User Profile displayed.</li> <li>The option to select a quantity and generate that quantity of Registration Tokens is available from the actions bar on this page.</li> </ul>";
            const string ViewUser = "<ul class=\"haptic-ul\"> <li><strong>This page displays the full set of detailed information of the selected User.</strong></li> <li><kbd>Name & Surname</kbd> details as well as all other contact details are displayed.</li> <li>No changes can be made to the User Profile from this page.</li> </ul>";
            const string MyProfile = "<ul class=\"haptic-ul\"> <li><strong>This page displays the full set of detailed information contained in the User Profile of the user that is currently logged into the DocuPath system.</strong></li> <li><kbd>Name & Surname</kbd> details as well as all other contact details are displayed.</li> <li>Non-sensitive security information (e.g. the Username) are displayed to provide hints.</li> <li>No changes can be made to the User Profile from this page.</li> </ul>";
            const string GenerateTokens = "<ul class=\"haptic-ul\"> <li><strong>This page captures the information used during the process of Registration Token generation.</strong></li> <li>An assigned <kbd>access level</kbd> with the intended user's <kbd>email address</kbd> is required for the system to generate the unique token for the intended user.</li> </ul>";
            #endregion
            #region HAPTIC HELP CONTENT:
                #region GENERIC TOPIC CONSTANTS
            const string AddGeneric = "";
                const string AllGeneric = "<div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Search or filter</strong> using a specific keyword/phrase.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#SearchFilter\">TRY THE FOLLOWING:</div> <div id=\"SearchFilter\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Ensure the target column is visible using the <ctrl>Column Selector</ctrl> dropdown menu, located to the right of the <ctrl>Search Bar</ctrl>.</li> <li>Click the '<kbd>Enter a keyword/phrase...</kbd>' field on the <ctrl>Search Bar</ctrl>.</li> <li>Type the desired keyword or phrase to search/filter by.</li> <li>The table adapts automatically to only display records where the provided query string is present in any of the currently visible columns.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">A minimum of one (1) character is required to initiate a search.</li> <li class=\"colour-dpred\">Clearing the contents of the <ctrl>Search Bar</ctrl> will reset the filtering and reset the table to display all entries (as determined by the current value of the <ctrl>Row Count Selector</ctrl> dropdown menu).</li> <li class=\"colour-dpred\">Filtering occurs automatically after a 250ms delay - no interaction other than entering a search query is needed.</li> <li class=\"colour-dpred\">Filtering is based only on visible columns - invisible columns will not be included in the search query.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\">Change the <strong class=\"catchphrase-highlight\">columns</strong> that are displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#SelectColumns\">TRY THE FOLLOWING:</div> <div id=\"SelectColumns\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Open the <ctrl>Column Selector</ctrl> dropdown menu.</li> <li>Tick/untick the desired column(s) by using the checkbox next to the relevant column(s)</li> <li>The table adapts automatically to display the ticked column(s) and hide the unticked column(s).</li> </ol> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\">Change the <strong class=\"catchphrase-highlight\">number of entries</strong> displayed per page.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#NumberOfEntries\">TRY THE FOLLOWING:</div> <div id=\"NumberOfEntries\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Open the <ctrl>Row Count Selector</ctrl> dropdown menu, located to the right of the <ctrl>Search Bar</ctrl>.</li> <li>Select either <kbd>10</kbd>, <kbd>25</kbd>, <kbd>50</kbd> or <kbd>All</kbd> to display the corresponding number of items per page.</li> <li>The table adapts automatically to display the selected number of items per page.</li> <li>The <ctrl>results counter</ctrl> control to the right below the table displays the number of currently displayed items on this page out of the total number of items.</li> <li>Use the <ctrl>page navigation</ctrl> control to the left below the table to navigate between pages.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Note that some pages (due to the number of records that must be displayed on the page) do not have an <kbd>All</kbd> option. On these pages, the available item counts per page are <kbd>10</kbd>, <kbd>25</kbd>, <kbd>50</kbd> and <kbd>100</kbd>.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\">Change the table's <strong class=\"catchphrase-highlight\">display density</strong>.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#DisplayDensity\">TRY THE FOLLOWING:</div> <div id=\"DisplayDensity\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Click the <ctrl>Toggle Table Density</ctrl> button on the <ctrl>Page Actions</ctrl> button bar to the right of the Page Title.</li> <li>The table cycles through three (3) levels of display density for the table (namely <kbd>Normal</kbd>, <kbd>Condensed</kbd> and <kbd>Minimal</kbd>), compressing or expanding the information displayed.</li> </ol> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Sort</strong> the information displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#SortAscDesc\">TRY THE FOLLOWING:</div> <div id=\"SortAscDesc\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Decide on the desired column to sort by, and whether <kbd>Ascending</kbd> or <kbd>Descending</kbd> sort order is required.</li> <li>Click the column title of the desired column.</li> <li>A <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-up colour-dpred\"></i></kbdicon> icon appears to the right of the column heading, indicating an <kbd>Ascending</kbd> sort order.</li> <li>The table adapts automatically, sorting the currently displayed items according to specified column, in <kbd>Ascending</kbd> order.</li> <li>Clicking the column title of the desired column again changes the displayed icon to the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-down colour-dpred\"></i></kbdicon> icon, indicating a <kbd>Descending</kbd> sort order.</li> <li>The table adapts automatically, sorting the currently displayed items according to specified column, in <kbd>Descending</kbd> order.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Continuously clicking on a column's title will repeatedly switch between <kbd>Ascending</kbd> and <kbd>Descending</kbd> sort order.</li> <li class=\"colour-dpred\">Note that, on page load, the items displayed in the table are sorted by default according to a pre-defined column using a pre-defined sort order.</li> <li class=\"colour-dpred\">The items displayed in the table cannot be sorted according to the values of the <kbd>Actions</kbd> column.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">View </strong> a single item displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#ViewItem\">TRY THE FOLLOWING:</div> <div id=\"ViewItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon.</li> <li>The system redirects to the relevant item's detail page, where all of the information relating to the item is displayed.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">The <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon displays the <ctrl>VIEW</ctrl> tooltip when hovered over.</li> <li class=\"colour-dpred\">To view the item's detailed information in a new browser tab, right click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-view colour-dpred\"></i></kbdicon> icon and click <kbd>Open link in new tab</kbd>.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Delete</strong> a single item displayed in the table.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#DeleteItem\">TRY THE FOLLOWING:</div> <div id=\"DeleteItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-delete colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-delete colour-dpred\"></i></kbdicon> icon.</li> <li>The system displays a <ctrl>Modal</ctrl> confirmation notification confirming deletion of the relevant item.</li> <li>To proceed with deleting the item, click the <ctrl>Yes</ctrl> button.</li> <li>To cancel deleting the item, click the <ctrl>No</ctrl> button.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Deletion of an item by clicking <ctrl>Yes</ctrl> on the <ctrl>Modal</ctrl> confirmation notification is irreversible from the system frontend - the item will be archived and is only retrievable by contacting a system administrator.</li> <li class=\"colour-dpred\">In some cases, item deletions require authorisation by a Superuser. In these cases, the <ctrl>Modal</ctrl> confirmation notification is still shown, but the item's status is changed to <kbd>Pending</kbd> and a Superuser has to approve the deletion before the status is changed to <kbd>Archived</kbd>.</li> </ul> </div> </div> </div>";
                const string ViewGeneric = "";
                const string UpdateGeneric = "";
                const string UploadGeneric = "";
                const string MigrateGeneric = "";
                const string HapticGeneric = "";
                const string HomePageGeneric = "";
                const string INSIGHTGeneric = "";
                const string VISIONGeneric = "";
                const string CalendarGeneric = "";
                const string DASGeneric = "";
                const string MDRGeneric = "";
                const string ErrorGeneric = "";
                const string UnauthorizedAccessGeneric = "";
                const string SystemParametersGeneric = "";
                const string GenerateTokensGeneric = "";
            #endregion
                #region SPECIFIC TOPIC CONSTANTS
            const string AddUALSpecific = "";
            const string AllUALpecific = "";
            const string ViewUALSpecific = "";
            const string UpdateUALSpecific = "";
            const string AllATSpecific = "";
            const string ViewAuditTrailSpecific = "";
            const string AddCTSpecific = "";
            const string AllCTSpecific = "";
            const string ViewCTSpecific = "";
            const string UpdateCTSpecific = "";
            const string UpdateCTRepoSpecific = "";
            const string AddERCSpecific = "";
            const string AllERCSpecific = "";
            const string ViewERCSpecific = "";
            const string UpdateERCSpecific = "";
            const string AddFCSpecific = "";
            const string AllFCSpecific = "<div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Migrate</strong> a single item to a new owner.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#MigrateItem\">TRY THE FOLLOWING:</div> <div id=\"MigrateItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-migrate colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-migrate colour-dpred\"></i></kbdicon> icon.</li> <li>The system navigates to the <kbd>Migrate Forensic Case</kbd> page, from where the migration operation can be completed.</li> </ol> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Lock</strong> a single item to prevent further changes.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#LockItem\">TRY THE FOLLOWING:</div> <div id=\"LockItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-lock colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-lock colour-dpred\"></i></kbdicon> icon.</li> <li>The system displays a <ctrl>Modal</ctrl> confirmation notification confirming locking of the relevant item.</li> <li>To proceed with locking the item, click the <ctrl>Yes</ctrl> button, or simply cancel locking the item by clicking the <ctrl>No</ctrl> button.</li> <li>The system changes the status of the item in question to <kbd>Locked</kbd>, disallowing any further changes.</li> </ol> <ul class=\"haptic-ol\"> <li class=\"colour-dpred\">Locking an item will prevent any further changes to the item. In the event where additional evidence or other circumstances arise (that require changes to a locked item), the item will need to be unlocked via Superuser authorisation prior to changes being allowed.</li> <li class=\"colour-dpred\">Refer to the HAPTIC topic on unlocking items for more information.</li> </ul> </div> </div> </div> <div class=\"haptic-topic-container\"> <div class=\"grid haptic-catchphrase-grid\"> <i class=\"mdl2icon mdl2-topic haptic-mdl2icon\" grid-area=\"icon\"></i> <p class=\"haptic-catchphrase\" grid-area=\"catchphrase\"><strong class=\"catchphrase-highlight\">Unlock</strong> a single item to allow further changes.</p> </div> <div class=\"modal-help-panel\" style=\"margin: 0\"> <div class=\"panel-heading modalbox-help-panel-heading\" data-toggle=\"collapse\" data-target=\"#UnlockItem\">TRY THE FOLLOWING:</div> <div id=\"UnlockItem\" class=\"panel-collapse collapse sidenav-notification-detail\"> <ol class=\"haptic-ol\"> <li>Locate the desired item in the table.</li> <li>Locate the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-unlock colour-dpred\"></i></kbdicon> icon in the <kbd>Actions</kbd> column next to the desired item in the table.</li> <li>Click the <kbdicon><i class=\"mdl2icon haptic-mdl2icon mdl2-unlock colour-dpred\"></i></kbdicon> icon.</li> <li>The system displays a <ctrl>Modal</ctrl> confirmation notification confirming unlocking of the relevant item.</li> <li>To proceed with unlocking the item, click the <ctrl>Yes</ctrl> button, or simply cancel unlocking the item by clicking the <ctrl>No</ctrl> button.</li> <li>If the <ctrl>Yes</ctrl> button was clicked, the <ctrl>Modal</ctrl> confirmation notification changes to a <ctrl>Motivated Modal</ctrl> confirmation notification, requesting a reason for the attempted item unlock.</li> <li>Type a reason into the <kbd>Provide a reason...</kbd> field.</li> <li>Click the <ctrl>Finalise</ctrl> button.</li> <li>The system changes the status of the item in question to <kbd>Pending</kbd>, and awaits Superuser approval on the item unlock.</li> <li>Once the Superuser has approved the item unlock, the system will allow further changes to be made to the item.</li> </ol> </div> </div> </div>";
            const string ViewFCSpecific = "";
            const string UpdateFCSpecific = "";
            const string MigrateFCSpecific = "";
            const string HapticSpecific = "";
            const string HomePageSpecific = "";
            const string AddLCSpecific = "";
            const string AllLCSpecific = "";
            const string ViewLCSpecific = "";
            const string UpdateLCSpecific = "";
            const string AddMediaSpecific = "";
            const string AllMediaSpecific = "";
            const string ViewMediaSpecific = "";
            const string UpdateMediaSpecific = "";
            const string UploadMediaSpecific = "";
            const string INSIGHTSpecific = "";
            const string VISIONSpecific = "";
            const string CalendarSpecific = "";
            const string DASSpecific = "";
            const string MDRSpecific = "";
            const string AddSPSpecific = "";
            const string AllSPSpecific = "";
            const string ViewSPSpecific = "";
            const string UpdateSPSpecific = "";
            const string AddSRSpecific = "";
            const string AllSRSpecific = "";
            const string ViewSRSpecific = "";
            const string LinkReportSpecific = "";
            const string ErrorSpecific = "";
            const string UnauthorizedAccessSpecific = "";
            const string SystemParametersSpecific = "";
            const string AllUsersSpecific = "";
            const string ViewUserSpecific = "";
            const string MyProfileSpecific = "";
            const string GenerateTokensSpecific = "";

        #endregion
        #endregion
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region FETCHES, GETS & QUERIES:
            #region NEURON
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
            #endregion
            #region VISION
                public static List<METRIC> GetVisionMetrics()
            {
                using (DocuPathEntities db = new DocuPathEntities())
                {
                    #region Parameters, Extracted Constants & Values:
                    List<METRIC> visionMetrics = new List<METRIC>();
                    DateTime dtToday = DateTime.Today;
                    DateTime dtTodayMidnight = dtToday.AddDays(1).AddMilliseconds(-1);
                    DateTime dtDateToday = DateTime.Today.Date;
                    DateTime dtTimeNow = DateTime.Now;
                    DateTime dtDateNow = DateTime.Now.Date;
                    double targetAge = TimeSpan.FromDays(365 * 11).TotalDays;
                    var pastSeven = dtTodayMidnight.AddDays(-7);
                    var pastThirty = dtTodayMidnight.AddDays(-30);
                    var pastSixty = dtTodayMidnight.AddDays(-60);
                    var pastNinety = dtTodayMidnight.AddDays(-90);
                    #endregion

                    // ======= FORENSIC CASES (GROUP ID: 1) =======
                    #region Forensic Case Metrics
                    METRIC vmFCCount = new METRIC(1, 1, "mdl2icon mdl2-fc metric-mdl2icon", "/ForensicCase/All", "All Cases", db.FORENSIC_CASE.Count().ToString(), "cases", "in total", "The total number of Forensic Cases in the DocuPath database, regardless of case status.", DateTime.Now);
                    visionMetrics.Add(vmFCCount);

                    // --> COUNT OF FC BY STATUS:
                    // Active:
                    METRIC vmActiveFCCount = new METRIC(1, 2, "mdl2icon mdl2-done metric-mdl2icon", "/ForensicCase/All", "Active Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Active").Count().ToString(), "cases", "active", "The number of active Forensic Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmActiveFCCount);
                    // Pending:
                    METRIC vmPendingFCCount = new METRIC(1, 2, "mdl2icon mdl2-warning metric-mdl2icon", "/ForensicCase/All", "Pending Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Pending").Count().ToString(), "cases", "pending", "The number of pending Forensic Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmPendingFCCount);
                    // Archived:
                    METRIC vmArchivedFCCount = new METRIC(1, 2, "mdl2icon mdl2-delete metric-mdl2icon", "/ForensicCase/All", "Archived Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Archived").Count().ToString(), "cases", "archived", "The number of archived Forensic Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmArchivedFCCount);
                    // Locked:
                    METRIC vmLockedFCCount = new METRIC(1, 2, "mdl2icon mdl2-lock metric-mdl2icon", "/ForensicCase/All", "Locked Cases", db.FORENSIC_CASE.Where(x => x.STATUS.StatusValue == "Locked").Count().ToString(), "cases", "locked", "The number of locked Forensic Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmLockedFCCount);
                    // FC Counts By Status:
                    #region Case Counts:
                    string FCByStatusChartData = vmActiveFCCount.MetricValue + ", " + vmPendingFCCount.MetricValue + ", " + vmArchivedFCCount.MetricValue + ", " + vmLockedFCCount.MetricValue;
                    #endregion
                    METRIC vmFCCountsByStatusGraph = new METRIC(1, 3, "mdl2icon mdl2-approvedeny metric-mdl2icon", "/ForensicCase/All", "Forensic Cases By Status", FCByStatusChartData, "cases", "by status", "Doughnut chart of Forensic Case counts in the DocuPath database, by status.", DateTime.Now);
                    visionMetrics.Add(vmFCCountsByStatusGraph);
                    // Average Duration:
                    #region Average Duration Calculation
                    var avgFC = Math.Round((double)db.FORENSIC_CASE.Where(fc => fc.DateClosed.HasValue).Average(fc => SqlFunctions.DateDiff("month", fc.DateClosed, fc.DateAdded)), 2);
                    #endregion
                    METRIC vmAvgFCDuration = new METRIC(1, 2, "mdl2icon mdl2-time metric-mdl2icon", "/ForensicCase/All", "Average Duration", avgFC.ToString(), "months", "open", "The average duration of a typical Forensic Case in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmAvgFCDuration);
                    // Aged:Non-Aged Ratio:
                    #region Aged:Non-Aged Ratio Calculation:
                    double iAgedCount = db.FORENSIC_CASE.Where(fc => fc.DateClosed.HasValue && (SqlFunctions.DateDiff("day", fc.DateClosed.ToString(), DateTime.Now.ToString()) > targetAge)).Count();
                    double iNonAgedCount = db.FORENSIC_CASE.Where(fc => fc.DateClosed.HasValue && (SqlFunctions.DateDiff("day", fc.DateClosed.ToString(), DateTime.Now.ToString()) < targetAge)).Count();
                    string ratio = "1:" + Math.Round(iAgedCount / iNonAgedCount, 2).ToString();
                    #endregion
                    METRIC vmAgedNonAgedFCRatio = new METRIC(1, 2, "mdl2icon mdl2-history metric-mdl2icon", "/ForensicCase/All", "Aged Case Ratio", ratio, "cases", "Non-Aged:Aged", "The ratio of non-aged versus aged Forensic Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmAgedNonAgedFCRatio);
                    // FC Additions:
                    #region FC Additions:
                    var FCAddedToday = db.FORENSIC_CASE.Where(fc => fc.DateAdded >= dtToday).Count();
                    var FCAddedPastSevenDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > pastSeven).Count();
                    var FCAddedPastThirtyDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > pastThirty).Count();
                    var FCAddedPastSixtyDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > pastSixty).Count();
                    var FCAddedPastNinetyDays = db.FORENSIC_CASE.Where(fc => fc.DateAdded <= dtToday && fc.DateAdded > pastNinety).Count();
                    var FCClosedToday = db.FORENSIC_CASE.Where(fc => fc.DateClosed >= dtToday).Count();
                    var FCClosedPastSevenDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > pastSeven).Count();
                    var FCClosedPastThirtyDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > pastThirty).Count();
                    var FCClosedPastSixtyDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > pastSixty).Count();
                    var FCClosedPastNinetyDays = db.FORENSIC_CASE.Where(fc => fc.DateClosed <= dtToday && fc.DateClosed > pastNinety).Count();
                    string FCAddedChartData = FCAddedToday + ", " + FCAddedPastSevenDays + ", " + FCAddedPastThirtyDays + ", " + FCAddedPastSixtyDays + ", " + FCAddedPastNinetyDays;
                    string FCClosedChartData = FCClosedToday + ", " + FCClosedPastSevenDays + ", " + FCClosedPastThirtyDays + ", " + FCClosedPastSixtyDays + ", " + FCClosedPastNinetyDays;
                    #endregion
                    METRIC vmFCAddedByDayGraph = new METRIC(1, 3, "mdl2icon mdl2-history metric-mdl2icon", "/ForensicCase/All", "Forensic Cases Added & Closed", FCAddedChartData + "|" + FCClosedChartData, "cases", "added/closed", "Bar chart of Forensic Cases added and closed in the DocuPath database, by time interval.", DateTime.Now);
                    visionMetrics.Add(vmFCAddedByDayGraph);
                    #endregion

                    // ======= LEGACY CASES (GROUP ID: 2) =======
                    #region Legacy Case Metrics
                    METRIC vmLCCount = new METRIC(2, 1, "mdl2icon mdl2-lc metric-mdl2icon", "/LegacyCase/All", "All Cases", db.LEGACY_CASE.Count().ToString(), "cases", "in total", "The total number of Legacy Cases in the DocuPath database, regardless of case status.", DateTime.Now);
                    visionMetrics.Add(vmLCCount);

                    // --> COUNT OF LC BY STATUS:
                    // Active:
                    METRIC vmActiveLCCount = new METRIC(2, 2, "mdl2icon mdl2-done metric-mdl2icon", "/LegacyCase/All", "Active Cases", db.LEGACY_CASE.Where(lc => lc.STATUS.StatusValue == "Active").Count().ToString(), "cases", "active", "The number of active Legacy Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmActiveLCCount);
                    // Archived:
                    METRIC vmArchivedLCCount = new METRIC(2, 2, "mdl2icon mdl2-delete metric-mdl2icon", "/LegacyCase/All", "Archived Cases", db.LEGACY_CASE.Where(lc => lc.STATUS.StatusValue == "Archived").Count().ToString(), "cases", "archived", "The number of archived Legacy Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmArchivedLCCount);
                    // LC Counts By Status:
                    #region Case Counts:
                    string LCByStatusChartData = vmActiveLCCount.MetricValue + ", " + vmArchivedLCCount.MetricValue;
                    #endregion
                    METRIC vmLCCountsByStatusGraph = new METRIC(2, 3, "mdl2icon mdl2-approvedeny metric-mdl2icon", "/LegacyCase/All", "Legacy Cases By Status", LCByStatusChartData, "cases", "by status", "Doughnut chart of Legacy Case counts in the DocuPath database, by status.", DateTime.Now);
                    visionMetrics.Add(vmLCCountsByStatusGraph);
                    // Average Duration:
                    #region Average Duration Calculation
                    var avgLC = Math.Round((double)db.LEGACY_CASE.Where(lc => lc.DateClosed.HasValue).Average(lc => SqlFunctions.DateDiff("month", lc.DateClosed, lc.DateAdded)), 2);
                    #endregion
                    METRIC vmAvgLCDuration = new METRIC(2, 2, "mdl2icon mdl2-time metric-mdl2icon", "/LegacyCase/All", "Average Duration", avgLC.ToString(), "months", "open", "The average duration of a typical Legacy Case in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmAvgLCDuration);
                    // Aged:Non-Aged Ratio:
                    #region Aged:Non-Aged Ratio Calculation:
                    double iAgedLCCount = db.LEGACY_CASE.Where(lc => lc.DateClosed.HasValue && (SqlFunctions.DateDiff("day", lc.DateClosed.ToString(), DateTime.Now.ToString()) > targetAge)).Count();
                    double iNonAgedLCCount = db.LEGACY_CASE.Where(lc => lc.DateClosed.HasValue && (SqlFunctions.DateDiff("day", lc.DateClosed.ToString(), DateTime.Now.ToString()) < targetAge)).Count();
                    string ratioLC = "1:" + Math.Round(iAgedLCCount / iNonAgedLCCount, 2).ToString();
                    #endregion
                    METRIC vmAgedNonAgedLCRatio = new METRIC(2, 2, "mdl2icon mdl2-history metric-mdl2icon", "/LegacyCase/All", "Aged Case Ratio", ratioLC, "cases", "Non-Aged:Aged", "The ratio of non-aged versus aged Legacy Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmAgedNonAgedLCRatio);
                    // LC Additions:
                    #region LC Additions:
                    var LCAddedToday = db.LEGACY_CASE.Where(lc => lc.DateAdded >= dtToday).Count();
                    var LCAddedPastSevenDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > pastSeven).Count();
                    var LCAddedPastThirtyDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > pastThirty).Count();
                    var LCAddedPastSixtyDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > pastSixty).Count();
                    var LCAddedPastNinetyDays = db.LEGACY_CASE.Where(lc => lc.DateAdded <= dtToday && lc.DateAdded > pastNinety).Count();
                    string LCAddedChartData = LCAddedToday + ", " + LCAddedPastSevenDays + ", " + LCAddedPastThirtyDays + ", " + LCAddedPastSixtyDays + ", " + LCAddedPastNinetyDays;
                    #endregion
                    METRIC vmLCAddedByDayGraph = new METRIC(2, 3, "mdl2icon mdl2-history metric-mdl2icon", "/LegacyCase/All", "Legacy Cases Captured", LCAddedChartData, "cases", "captured", "Bar chart of Legacy Cases added captured into the DocuPath database, by time interval.", DateTime.Now);
                    visionMetrics.Add(vmLCAddedByDayGraph);
                    #endregion

                    // ======= EXTERNAL REVIEW CASES (GROUP ID: 3) =======
                    #region External Review Case Metrics
                    METRIC vmERCCount = new METRIC(3, 1, "mdl2icon mdl2-erc metric-mdl2icon", "/ExternalReviewCase/All", "All Cases", db.EXTERNAL_REVIEW_CASE.Count().ToString(), "cases", "in total", "The total number of External Review Cases in the DocuPath database, regardless of case status.", DateTime.Now);
                    visionMetrics.Add(vmERCCount);

                    // --> COUNT OF ERC BY STATUS:
                    // Active:
                    METRIC vmActiveERCCount = new METRIC(3, 2, "mdl2icon mdl2-done metric-mdl2icon", "/ExternalReviewCase/All", "Active Cases", db.EXTERNAL_REVIEW_CASE.Where(erc => erc.STATUS.StatusValue == "Active").Count().ToString(), "cases", "active", "The number of active External Review Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmActiveERCCount);
                    // Pending:
                    METRIC vmPendingERCCount = new METRIC(3, 2, "mdl2icon mdl2-warning metric-mdl2icon", "/ExternalReviewCase/All", "Pending Cases", db.EXTERNAL_REVIEW_CASE.Where(erc => erc.STATUS.StatusValue == "Pending").Count().ToString(), "cases", "pending", "The number of pending External Review Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmPendingERCCount);
                    // Archived:
                    METRIC vmArchivedERCCount = new METRIC(3, 2, "mdl2icon mdl2-delete metric-mdl2icon", "/ExternalReviewCase/All", "Archived Cases", db.EXTERNAL_REVIEW_CASE.Where(erc => erc.STATUS.StatusValue == "Archived").Count().ToString(), "cases", "archived", "The number of archived External Review Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmArchivedERCCount);
                    // Locked:
                    METRIC vmLockedERCCount = new METRIC(3, 2, "mdl2icon mdl2-lock metric-mdl2icon", "/ExternalReviewCase/All", "Locked Cases", db.EXTERNAL_REVIEW_CASE.Where(erc => erc.STATUS.StatusValue == "Locked").Count().ToString(), "cases", "locked", "The number of locked External Review Cases in the DocuPath database.", DateTime.Now);
                    visionMetrics.Add(vmLockedERCCount);
                    // ERC Counts By Status:
                    #region Case Counts:
                    string ERCByStatusChartData = vmActiveERCCount.MetricValue + ", " + vmPendingERCCount.MetricValue + ", " + vmArchivedERCCount.MetricValue + ", " + vmLockedERCCount.MetricValue;
                    #endregion
                    METRIC vmERCCountsByStatusGraph = new METRIC(3, 3, "mdl2icon mdl2-approvedeny metric-mdl2icon", "/ExternalReviewCase/All", "External Review Cases By Status", ERCByStatusChartData, "cases", "by status", "Doughnut chart of External Review Case counts in the DocuPath database, by status.", DateTime.Now);
                    visionMetrics.Add(vmERCCountsByStatusGraph);
                    #endregion

                    // ======= MEDIA (GROUP ID: 4) =======

                    // ======= SERVICE REQUESTS & SERVICE PROVIDERS (GROUP ID: 5) =======

                    // ======= CONTENT TAGS (GROUP ID: 6) =======

                    // ======= USERS & SYSTEM ACTIVITY (GROUP ID: 7) =======

                    return visionMetrics;
                }
            }
            #endregion
            #region HAPTIC
                public static string GetHelp(string inSelector)
            {
                switch (inSelector)
                {
                    // GENERIC SELECTORS:
                    case "AddGeneric": return AddGeneric;
                    case "AllGeneric": return AllGeneric;
                    case "ViewGeneric": return ViewGeneric;
                    case "UpdateGeneric": return UpdateGeneric;
                    case "UploadGeneric": return UploadGeneric;
                    case "MigrateGeneric": return MigrateGeneric;
                    case "HapticGeneric": return HapticGeneric;
                    case "HomePageGeneric": return HomePageGeneric;
                    case "INSIGHTGeneric": return INSIGHTGeneric;
                    case "VISIONGeneric": return VISIONGeneric;
                    case "CalendarGeneric": return CalendarGeneric;
                    case "DASGeneric": return DASGeneric;
                    case "MDRGeneric": return MDRGeneric;
                    case "ErrorGeneric": return ErrorGeneric;
                    case "UnauthorizedAccessGeneric": return UnauthorizedAccessGeneric;
                    case "SystemParametersGeneric": return SystemParametersGeneric;
                    case "GenerateTokensGeneric": return GenerateTokensGeneric;
                    //SPECIFIC SELECTORS:
                    case "AddUALSpecific": return AddUALSpecific;
                    case "AllUALpecific": return AllUALpecific;
                    case "ViewUALSpecific": return ViewUALSpecific;
                    case "UpdateUALSpecific": return UpdateUALSpecific;
                    case "AllATSpecific": return AllATSpecific;
                    case "ViewAuditTrailSpecific": return ViewAuditTrailSpecific;
                    case "AddCTSpecific": return AddCTSpecific;
                    case "AllCTSpecific": return AllCTSpecific;
                    case "ViewCTSpecific": return ViewCTSpecific;
                    case "UpdateCTSpecific": return UpdateCTSpecific;
                    case "UpdateCTRepoSpecific": return UpdateCTRepoSpecific;
                    case "AddERCSpecific": return AddERCSpecific;
                    case "AllERCSpecific": return AllERCSpecific;
                    case "ViewERCSpecific": return ViewERCSpecific;
                    case "UpdateERCSpecific": return UpdateERCSpecific;
                    case "AddFCSpecific": return AddFCSpecific;
                    case "AllFCSpecific": return AllFCSpecific;
                    case "ViewFCSpecific": return ViewFCSpecific;
                    case "UpdateFCSpecific": return UpdateFCSpecific;
                    case "MigrateFCSpecific": return MigrateFCSpecific;
                    case "HapticSpecific": return HapticSpecific;
                    case "HomePageSpecific": return HomePageSpecific;
                    case "AddLCSpecific": return AddLCSpecific;
                    case "AllLCSpecific": return AllLCSpecific;
                    case "ViewLCSpecific": return ViewLCSpecific;
                    case "UpdateLCSpecific": return UpdateLCSpecific;
                    case "AddMediaSpecific": return AddMediaSpecific;
                    case "AllMediaSpecific": return AllMediaSpecific;
                    case "ViewMediaSpecific": return ViewMediaSpecific;
                    case "UpdateMediaSpecific": return UpdateMediaSpecific;
                    case "UploadMediaSpecific": return UploadMediaSpecific;
                    case "INSIGHTSpecific": return INSIGHTSpecific;
                    case "VISIONSpecific": return VISIONSpecific;
                    case "CalendarSpecific": return CalendarSpecific;
                    case "DASSpecific": return DASSpecific;
                    case "MDRSpecific": return MDRSpecific;
                    case "AddSPSpecific": return AddSPSpecific;
                    case "AllSPSpecific": return AllSPSpecific;
                    case "ViewSPSpecific": return ViewSPSpecific;
                    case "UpdateSPSpecific": return UpdateSPSpecific;
                    case "AddSRSpecific": return AddSRSpecific;
                    case "AllSRSpecific": return AllSRSpecific;
                    case "ViewSRSpecific": return ViewSRSpecific;
                    case "LinkReportSpecific": return LinkReportSpecific;
                    case "ErrorSpecific": return ErrorSpecific;
                    case "UnauthorizedAccessSpecific": return UnauthorizedAccessSpecific;
                    case "SystemParametersSpecific": return SystemParametersSpecific;
                    case "AllUsersSpecific": return AllUsersSpecific;
                    case "ViewUserSpecific": return ViewUserSpecific;
                    case "MyProfileSpecific": return MyProfileSpecific;
                    case "GenerateTokensSpecific": return GenerateTokensSpecific;
                    // DEFAULT RETURN:
                    default:
                        return null;
                }
            }
                public static string GetOnThisPageInfo(string inPageName)
            {
                switch (inPageName)
                {
                    case "AddUAL": return AddUAL;
                    case "AllUAL": return AllUAL;
                    case "ViewUAL": return ViewUAL;
                    case "UpdateUAL": return UpdateUAL;
                    case "AllAT": return AllAT;
                    case "ViewAuditTrail": return ViewAuditTrail;
                    case "AddCT": return AddCT;
                    case "AllCT": return AllCT;
                    case "ViewCT": return ViewCT;
                    case "UpdateCT": return UpdateCT;
                    case "UpdateCTRepo": return UpdateCTRepo;
                    case "AddERC": return AddERC;
                    case "AllERC": return AllERC;
                    case "ViewERC": return ViewERC;
                    case "UpdateERC": return UpdateERC;
                    case "AddFC": return AddFC;
                    case "AllFC": return AllFC;
                    case "ViewFC": return ViewFC;
                    case "UpdateFC": return UpdateFC;
                    case "MigrateFC": return MigrateFC;
                    case "HapticHelpCentre": return HapticHelpCentre;
                    case "Home": return Home;
                    case "AddLC": return AddLC;
                    case "AllLC": return AllLC;
                    case "ViewLC": return ViewLC;
                    case "UpdateLC": return UpdateLC;
                    case "AddMedia": return AddMedia;
                    case "AllMedia": return AllMedia;
                    case "ViewMedia": return ViewMedia;
                    case "UpdateMedia": return UpdateMedia;
                    case "UploadMedia": return UploadMedia;
                    case "INSIGHT": return INSIGHT;
                    case "VISION": return VISION;
                    case "Calendar": return Calendar;
                    case "DAS": return DAS;
                    case "MDR": return MDR;
                    case "AddSP": return AddSP;
                    case "AllSP": return AllSP;
                    case "ViewSP": return ViewSP;
                    case "UpdateSP": return UpdateSP;
                    case "AddSR": return AddSR;
                    case "AllSR": return AllSR;
                    case "ViewSR": return ViewSR;
                    case "LinkReport": return LinkReport;
                    case "Error": return Error;
                    case "UnauthorizedAccess": return UnauthorizedAccess;
                    case "SystemParameters": return SystemParameters;
                    case "AllUsers": return AllUsers;
                    case "ViewUser": return ViewUser;
                    case "MyProfile": return MyProfile;
                    case "GenerateTokens": return GenerateTokens;
                    default:
                        return null;
                }
            }
            #endregion
            #region FILES & FILE TYPES
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
                ///<summary>
                ///Pass a single comma-separated string of any combination of the following to fetch the relevant file type icons (case insensitive): PowerPoint / Excel / Word / PDF / Image / Video / Audio / Text / Markup / Compressed. An additional bool parameter specifies whether icons should be returned with or without their container.
                ///</summary>
                public static string GetAcceptedFileTypeMinis(string inAcceptedFiletypes, bool inContainerNeeded)
                {
                    var split = inAcceptedFiletypes.Split(',');
                    var markup = "";
                    if (inContainerNeeded)
                    {
                        markup += "<div class=\"grid ico-container\">";
                        foreach (var type in split)
                        {
                            switch (type.ToUpper())
                            {
                                case "POWERPOINT": markup += "<div class=\"ico ppt-mini\" data-toggle=\"tooltip\" title=\"PPT, PPTX, POT, POTX\" data-placement=\"bottom\"></div>"; break;
                                case "EXCEL": markup += "<div class=\"ico xls-mini\" data-toggle=\"tooltip\" title=\"XLS, XLSX, XLT, XLTX\" data-placement=\"bottom\"></div>"; break;
                                case "WORD": markup += "<div class=\"ico doc-mini\" data-toggle=\"tooltip\" title=\"DOC, DOCX, DOT, DOTX\" data-placement=\"bottom\"></div>"; break;
                                case "PDF": markup += "<div class=\"ico pdf-mini\" data-toggle=\"tooltip\" title=\"PDF\" data-placement=\"bottom\"></div>"; break;
                                case "IMAGE": markup += "<div class=\"ico imagefile-mini\" data-toggle=\"tooltip\" title=\"BMP, GIF, JPG, PNG, TIFF\" data-placement=\"bottom\"></div>"; break;
                                case "VIDEO": markup += "<div class=\"ico videofile-mini\" data-toggle=\"tooltip\" title=\"3GP, AVI, MP4, MPG, WMV\" data-placement=\"bottom\"></div>"; break;
                                case "AUDIO": markup += "<div class=\"ico audiofile-mini\" data-toggle=\"tooltip\" title=\"MP3, WAV\" data-placement=\"bottom\"></div>"; break;
                                case "TEXT": markup += "<div class=\"ico txtfile-mini\" data-toggle=\"tooltip\" title=\"TXT, RTF\" data-placement=\"bottom\"></div>"; break;
                                case "MARKUP": markup += "<div class=\"ico markupfile-mini\" data-toggle=\"tooltip\" title=\"XML\" data-placement=\"bottom\"></div>"; break;
                                case "COMPRESSED": markup += "<div class=\"ico compressedfile-mini\" data-toggle=\"tooltip\" title=\"ZIP, RAR\" data-placement=\"bottom\"></div>"; break;
                                default: markup += ""; break;
                            }
                        }
                        markup += "</div>";
                    }
                    else
                    {
                        foreach (var type in split)
                        {
                            switch (type.ToUpper())
                            {
                                case "POWERPOINT": markup += "<div class=\"ico ppt-mini\" data-toggle=\"tooltip\" title=\"PPT, PPTX, POT, POTX\" data-placement=\"bottom\"></div>"; break;
                                case "EXCEL": markup += "<div class=\"ico xls-mini\" data-toggle=\"tooltip\" title=\"XLS, XLSX, XLT, XLTX\" data-placement=\"bottom\"></div>"; break;
                                case "WORD": markup += "<div class=\"ico doc-mini\" data-toggle=\"tooltip\" title=\"DOC, DOCX, DOT, DOTX\" data-placement=\"bottom\"></div>"; break;
                                case "PDF": markup += "<div class=\"ico pdf-mini\" data-toggle=\"tooltip\" title=\"PDF\" data-placement=\"bottom\"></div>"; break;
                                case "IMAGE": markup += "<div class=\"ico imagefile-mini\" data-toggle=\"tooltip\" title=\"BMP, GIF, JPG, PNG, TIFF\" data-placement=\"bottom\"></div>"; break;
                                case "VIDEO": markup += "<div class=\"ico videofile-mini\" data-toggle=\"tooltip\" title=\"3GP, AVI, MP4, MPG, WMV\" data-placement=\"bottom\"></div>"; break;
                                case "AUDIO": markup += "<div class=\"ico audiofile-mini\" data-toggle=\"tooltip\" title=\"MP3, WAV\" data-placement=\"bottom\"></div>"; break;
                                case "TEXT": markup += "<div class=\"ico txtfile-mini\" data-toggle=\"tooltip\" title=\"TXT, RTF\" data-placement=\"bottom\"></div>"; break;
                                case "MARKUP": markup += "<div class=\"ico markupfile-mini\" data-toggle=\"tooltip\" title=\"XML\" data-placement=\"bottom\"></div>"; break;
                                case "COMPRESSED": markup += "<div class=\"ico compressedfile-mini\" data-toggle=\"tooltip\" title=\"ZIP, RAR\" data-placement=\"bottom\"></div>"; break;
                                default: markup += ""; break;
                            }
                        }
                    }
                    return markup;
                }
                public static string GetFileTypeThumbnail(string inFiletype, bool inContainerNeeded)
                {
                    var markup = "";
                    if (inContainerNeeded)
                    {
                        markup += "<div class=\"grid ico-container\">";
                        switch (inFiletype.ToUpper())
                        {
                            case "PPT": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>PPT</span></div>"; break;
                            case "POT": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>POT</span></div>"; break;
                            case "PPTX": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>PPTX</span></div>"; break;
                            case "PPOTX": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>POTX</span></div>"; break;
                            case "XLS": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLS</span></div>"; break;
                            case "XLT": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLT</span></div>"; break;
                            case "XLSX": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLSX</span></div>"; break;
                            case "XLTX": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLTX</span></div>"; break;
                            case "DOC": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOC</span></div>"; break;
                            case "DOT": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOT</span></div>"; break;
                            case "DOCX": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOCX</span></div>"; break;
                            case "DOTX": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOTX</span></div>"; break;
                            case "PDF": markup += "<div class=\"ico-lg\"><div class=\"ico pdf-mini\"></div><br /><span>PDF</span></div>"; break;
                            case "MP3": markup += "<div class=\"ico-lg\"><div class=\"ico audiofile-mini\"></div><br /><span>MP3</span></div>"; break;
                            case "WAV": markup += "<div class=\"ico-lg\"><div class=\"ico audiofile-mini\"></div><br /><span>WAV</span></div>"; break;
                            case "BMP": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>BMP</span></div>"; break;
                            case "GIF": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>GIF</span></div>"; break;
                            case "JPG": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>JPG</span></div>"; break;
                            case "PNG": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>PNG</span></div>"; break;
                            case "TIFF": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>TIFF</span></div>"; break;
                            case "3GP": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>3GP</span></div>"; break;
                            case "AVI": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>AVI</span></div>"; break;
                            case "MP4": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>MP4</span></div>"; break;
                            case "MPG": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>MPG</span></div>"; break;
                            case "WMV": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>WMV</span></div>"; break;
                            case "TXT": markup += "<div class=\"ico-lg\"><div class=\"ico txtfile-mini\"></div><br /><span>TXT</span></div>"; break;
                            case "RTF": markup += "<div class=\"ico-lg\"><div class=\"ico txtfile-mini\"></div><br /><span>RTF</span></div>"; break;
                            case "XML": markup += "<div class=\"ico-lg\"><div class=\"ico markupfile-mini\"></div><br /><span>XML</span></div>"; break;
                            case "ZIP": markup += "<div class=\"ico-lg\"><div class=\"ico compressedfile-mini\"></div><br /><span>ZIP</span></div>"; break;
                            case "RAR": markup += "<div class=\"ico-lg\"><div class=\"ico compressedfile-mini\"></div><br /><span>RAR</span></div>"; break;
                            default: markup += "<div class=\"ico-lg\"><div class=\"ico nopreview-mini\" data-toggle=\"tooltip\" title=\"NO PREVIEW AVAILABLE\"></div><br /><span>NONE</span></div>"; break;
                        }
                        markup += "</div>";
                    }
                    else
                    {
                        switch (inFiletype.ToUpper())
                        {
                            case "PPT": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>PPT</span></div>"; break;
                            case "POT": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>POT</span></div>"; break;
                            case "PPTX": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>PPTX</span></div>"; break;
                            case "PPOTX": markup += "<div class=\"ico-lg\"><div class=\"ico ppt-mini\"></div><br /><span>POTX</span></div>"; break;
                            case "XLS": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLS</span></div>"; break;
                            case "XLT": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLT</span></div>"; break;
                            case "XLSX": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLSX</span></div>"; break;
                            case "XLTX": markup += "<div class=\"ico-lg\"><div class=\"ico xls-mini\"></div><br /><span>XLTX</span></div>"; break;
                            case "DOC": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOC</span></div>"; break;
                            case "DOT": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOT</span></div>"; break;
                            case "DOCX": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOCX</span></div>"; break;
                            case "DOTX": markup += "<div class=\"ico-lg\"><div class=\"ico doc-mini\"></div><br /><span>DOTX</span></div>"; break;
                            case "PDF": markup += "<div class=\"ico-lg\"><div class=\"ico pdf-mini\"></div><br /><span>PDF</span></div>"; break;
                            case "MP3": markup += "<div class=\"ico-lg\"><div class=\"ico audiofile-mini\"></div><br /><span>MP3</span></div>"; break;
                            case "WAV": markup += "<div class=\"ico-lg\"><div class=\"ico audiofile-mini\"></div><br /><span>WAV</span></div>"; break;
                            case "BMP": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>BMP</span></div>"; break;
                            case "GIF": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>GIF</span></div>"; break;
                            case "JPG": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>JPG</span></div>"; break;
                            case "PNG": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>PNG</span></div>"; break;
                            case "TIFF": markup += "<div class=\"ico-lg\"><div class=\"ico imagefile-mini\"></div><br /><span>TIFF</span></div>"; break;
                            case "3GP": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>3GP</span></div>"; break;
                            case "AVI": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>AVI</span></div>"; break;
                            case "MP4": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>MP4</span></div>"; break;
                            case "MPG": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>MPG</span></div>"; break;
                            case "WMV": markup += "<div class=\"ico-lg\"><div class=\"ico videofile-mini\"></div><br /><span>WMV</span></div>"; break;
                            case "TXT": markup += "<div class=\"ico-lg\"><div class=\"ico txtfile-mini\"></div><br /><span>TXT</span></div>"; break;
                            case "RTF": markup += "<div class=\"ico-lg\"><div class=\"ico txtfile-mini\"></div><br /><span>RTF</span></div>"; break;
                            case "XML": markup += "<div class=\"ico-lg\"><div class=\"ico markupfile-mini\"></div><br /><span>XML</span></div>"; break;
                            case "ZIP": markup += "<div class=\"ico-lg\"><div class=\"ico compressedfile-mini\"></div><br /><span>ZIP</span></div>"; break;
                            case "RAR": markup += "<div class=\"ico-lg\"><div class=\"ico compressedfile-mini\"></div><br /><span>RAR</span></div>"; break;
                            default: markup += "<div class=\"ico-lg\"><div class=\"ico nopreview-mini\" data-toggle=\"tooltip\" title=\"NO PREVIEW AVAILABLE\"></div><br /><span>NONE</span></div>"; break;
                        }
                    }
                    return markup;
                }
            #endregion
            #region USERS
            public static USER getCurrentUser()
            {
                DocuPathEntities db = new DocuPathEntities();

                int id = HttpContext.Current.User.Identity.GetUserId<int>();
                USER currentUser = db.USER.Where(x => x.UserID == id).FirstOrDefault();
                currentUser.USER_LOGIN = db.USER_LOGIN.Where(x => x.UserLoginID == currentUser.UserLoginID).FirstOrDefault();
                currentUser.TITLE = db.TITLE.Where(x => x.TitleID == currentUser.TitleID).FirstOrDefault();

                return currentUser;

            }
            #endregion
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region FORMATTING & MARKUP:
        public static string ExtractMediaFileName(string inLocation)
        {
            string location = inLocation;
            string filename = inLocation.Substring(inLocation.LastIndexOf('/', 0));

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
            char[] invalidChars = new char[] { '\\', '/', ':', '*', '?', '\"', '>', '<', '|', '.', '_' };

            foreach (char checkChar in inRaw)
            {
                if (invalidChars.Contains(checkChar))
                {
                    inRaw = inRaw.Remove(inRaw.IndexOf(checkChar), 1);
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
        public static string GetDeletePopoverContent()
        {
            return "<div class=\"popoverbox\"> <div class=\"popoverbox-title-warning\"><img class=\"popoverbox-title-mdl2icon\" src=\"~/Content/Resources/icoWarn.png\" />WARNING</div> <div class=\"popoverbox-message-text\"> Are you sure you want to delete this item? This action cannot be reverted. </div> <div class=\"popoverbox-button-bar\"> <div class=\"col-md-12 popover-btn-col\"> <input type=\"submit\" id=\"btnWarningYes\" value=\"Yes\" class=\"btn btn-modal-generic\" /> <input type=\"submit\" id=\"btnWarningNo\" value=\"No\" class=\"btn btn-modal-generic\" /> </div> </div> </div>";
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region x
        #region y
        public static string xTx = "Ktm200xcw";
        #endregion
        #endregion
    }

    #region FLAG DEFAULTS
    public static class FLAG
    {
        public const decimal Decimal_One = 0.0m;
        public const decimal Decimal_Two = 0.00m;
        public const decimal Decimal_Three = 0.000m;
        public const string Time = "00:00:00";
        //public const DateTime Date = Convert.ToDateTime("1/1/9999");
        public const string AcademicID = "99999999";
        public const string Integer = "99999999"; //TODO CHECK
        public const string Numeric = "99999999"; //TODO check
        public const string ContactNumber = "9999999999";
        public const string NationalID = "9999999999999";
        public const string Alphanumeric = "9X9X9X9X";
        public const string Text = "Unavailable";
        public const string Email = "x@y.z";

    }
    #endregion

}