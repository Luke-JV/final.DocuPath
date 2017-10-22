using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.Custom_Classes;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    //[LogAction]
    public class SchedulingController : Controller
    {
        string controllerName = "Scheduling";
        DocuPathEntities db = new DocuPathEntities();

        [AuthorizeByAccessArea(AccessArea = "Access Personal Schedule")]
        public ActionResult Index()
        {
            try
            {
                return RedirectToAction("Calendar");
            }
            catch (Exception x)
            {
                return RedirectToAction("Error", "Home", new HandleErrorInfo(x, "Scheduling", "DailyAutopsySchedule"));
            }
        }
//----------------------------------------------------------------------------------------------//
        [AuthorizeByAccessArea(AccessArea = "Access Personal Schedule")]
        public ActionResult Calendar()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullInit, "Scheduling");
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullSuccess, "Scheduling");
                #endregion
                int uId = VERTEBRAE.getCurrentUser().UserID;
                string json = _GetCalendarEntries(uId);
                json = json.Replace('_', 'c');
                ViewBag.json = json;
                return View(db.SESSION_USER.Where(su => su.UserID == uId).ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullFail, "Scheduling");
                #endregion
                return RedirectToAction("Error", "Home", new HandleErrorInfo(x, "Scheduling", "DailyAutopsySchedule"));
            }
        }

        public JsonResult GetCalendarEntries()
        {
            string actionName = "GetCalendarEntries";

            int uId = VERTEBRAE.getCurrentUser().UserID;

            try
            {
                //string jsonraw = ;
                //string jsonfurnished = "{'success': 1,'result':" + jsonraw + "}";

                 return Json(_GetCalendarEntries(uId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                //#region AUDIT_WRITE
                //AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullFail, "Scheduling - Personal Google Calendar");
                //#endregion
                //VERTEBRAE.DumpErrorToTxt(x);
                //return View("Error", new HandleErrorInfo(x, controllerName, actionName));
                return null;
            }
        }

        private string _GetCalendarEntries(int uId)
        {
            string actionName = "_GetCalendarEntries";
            List<CALENDAR_APPOINTMENT> aptList = new List<CALENDAR_APPOINTMENT>();
            try
            {
                foreach (var entry in db.SESSION_USER.Where(s => s.UserID == uId && s.AllocationStatusID != 5))
                {
                    #region PREPARE VARIABLES
                    DateTime startUTC = new DateTime(entry.SESSION.DateID.Year, entry.SESSION.DateID.Month, entry.SESSION.DateID.Day, entry.SESSION.SLOT.StartTime.Hour, entry.SESSION.SLOT.StartTime.Minute, entry.SESSION.SLOT.StartTime.Second);

                    DateTime endUTC = new DateTime(entry.SESSION.DateID.Year, entry.SESSION.DateID.Month, entry.SESSION.DateID.Day, entry.SESSION.SLOT.EndTime.Hour, entry.SESSION.SLOT.EndTime.Minute, entry.SESSION.SLOT.EndTime.Second).ToUniversalTime();
                    string cssClass = "";
                    string eventTitle = "";
                    switch (entry.ALLOCATION_STATUS.StatusValue.ToLower())
                    {
                        case "pending":
                            cssClass = "event-pending";
                            eventTitle = "PENDING ALLOCATION (SLOT " + entry.SESSION.SLOT.Description.ToUpper() + " @ " + startUTC.ToString("HH:mm") + " ON "+startUTC.ToString("dd MMM yyyy").ToUpper()+")~" + entry.DateCreated.ToString("dd MMM yyyy HH:mm") + "|" + entry.DateStatusChanged.ToString("dd MMM yyyy HH:mm") + "^" + entry.ALLOCATION_STATUS.StatusValue;
                            break;
                        case "accepted":
                            cssClass = "event-accepted";
                            eventTitle = "ACCEPTED ALLOCATION (SLOT " + entry.SESSION.SLOT.Description.ToUpper() + " @ " + startUTC.ToString("HH:mm") + " ON " + startUTC.ToString("dd MMM yyyy").ToUpper() + ")~" + entry.DateCreated.ToString("dd MMM yyyy HH:mm") + "|" + entry.DateStatusChanged.ToString("dd MMM yyyy HH:mm") + "^" + entry.ALLOCATION_STATUS.StatusValue;
                            break;
                        case "rejected":
                            cssClass = "event-rejected";
                            eventTitle = "REJECTED ALLOCATION (SLOT " + entry.SESSION.SLOT.Description.ToUpper() + " @ " + startUTC.ToString("HH:mm") + " ON " + startUTC.ToString("dd MMM yyyy").ToUpper() + ")~" + entry.DateCreated.ToString("dd MMM yyyy HH:mm") + "|" + entry.DateStatusChanged.ToString("dd MMM yyyy HH:mm") + "^" + entry.ALLOCATION_STATUS.StatusValue;
                            break;
                        case "finalized":
                            cssClass = "event-finalized";
                            eventTitle = "ON DUTY (SLOT " + entry.SESSION.SLOT.Description.ToUpper() + " @ " + startUTC.ToString("HH:mm") + " ON " + startUTC.ToString("dd MMM yyyy").ToUpper() + ")~" +entry.DateCreated.ToString("dd MMM yyyy HH:mm")+"|"+ entry.DateStatusChanged.ToString("dd MMM yyyy HH:mm") + "^" + entry.ALLOCATION_STATUS.StatusValue;
                            break;
                        default:
                            cssClass = "event-general";
                            break;
                    }; 
                    #endregion

                    CALENDAR_APPOINTMENT apt = new CALENDAR_APPOINTMENT();
                    apt.id = entry.SessionID;
                    apt.title = eventTitle;
                    //apt.url = entry.SessionID.ToString();
                    apt.url = "#sesh-" + entry.SessionID;
                    apt._lass = cssClass;
                    apt.start = Convert.ToInt64(startUTC.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
                    apt.end = Convert.ToInt64(endUTC.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);

                    aptList.Add(apt);
                }
            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    RedirectToAction("Error", new HandleErrorInfo(eceex, controllerName, actionName));
                }
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                RedirectToAction("Error", "Home", x.Message);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(aptList);
        }

        [AuthorizeByAccessArea(AccessArea = "Access Personal Schedule")]
        public ActionResult DisplayEvent(int id)
        {
            #region AUDIT_WRITE
            AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullInit, "Scheduling - View Event Details");
            #endregion
            
            #region PREPARE VIEWMODEL
            SESSION_USER model = new SESSION_USER();
            model = db.SESSION_USER.Where(su => su.SessionID == id).FirstOrDefault();
            #endregion

            #region AUDIT_WRITE
            AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullSuccess, "Scheduling - View Event Details");
            #endregion
            return PartialView(model);
        }
//----------------------------------------------------------------------------------------------//
        [AuthorizeByAccessArea(AccessArea = "Compile Monthly Duty Roster")]
        [AuthorizeByAccessArea(AccessArea = "Finalise Monthly Duty Roster")]
        public ActionResult MonthlyDutyRoster()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushInit, "Scheduling");
                #endregion

                // General Info:
                List<SelectListItem> selectUsers = new List<SelectListItem>();
                List<SelectListItem> selectSlots = new List<SelectListItem>();

                selectUsers.Add(new SelectListItem { Value = "0", Text = "..." });
                //selectUsers.Add(new SelectListItem { Value = "1", Text = "Override" });
                foreach (var item in db.USER)
                {
                    selectUsers.Add(new SelectListItem { Value = (item.UserID + 1).ToString(), Text = item.DisplayInitials });
                }
                ViewBag.Users = selectUsers;

                foreach (var item in db.SLOT)
                {
                    selectSlots.Add(new SelectListItem { Value = item.SlotID.ToString(), Text = item.Description });
                }
                ViewBag.Slots = selectSlots;

                // This month's info:
                List<SelectListItem> selectDatesCurrentMonth = new List<SelectListItem>();
                List<SelectListItem> selectDatesNextMonth = new List<SelectListItem>();

                ViewBag.CurrentMonthName = "October " + DateTime.Today.Date.Year;

                foreach (var date in db.SESSION.Where(x => x.DateID.Month == DateTime.Today.Date.Month && x.SlotID == 1))
                {
                    // Make a new allocation:
                    DayAllocationsComments currentMonthAllocation = new DayAllocationsComments();

                    // Set its date equal to the allocation's date:
                    currentMonthAllocation.Date = allocation.SESSION.DateID;

                    // Set its date string equal to the string conversion of the allocation's date:
                    currentMonthAllocation.DateString = allocation.SESSION.DateID.ToString("ddd dd MMM");

                    if (allocation.SESSION.DateID.DayOfWeek != DayOfWeek.Saturday && allocation.SESSION.DateID.DayOfWeek != DayOfWeek.Sunday)
                    {
                        // Allocate its 1A UserID:
                        currentMonthAllocation.Slot1AUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 1).FirstOrDefault().UserID;
                        // Allocate its 1B UserID:
                        currentMonthAllocation.Slot1BUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 2).FirstOrDefault().UserID;
                        // Allocate its 1C UserID:
                        currentMonthAllocation.Slot1CUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 3).FirstOrDefault().UserID;
                        // Allocate its 2A UserID:
                        currentMonthAllocation.Slot2AUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 4).FirstOrDefault().UserID;
                        // Allocate its 2B UserID:
                        currentMonthAllocation.Slot2BUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 5).FirstOrDefault().UserID;
                        // Allocate its CALL UserID:
                        currentMonthAllocation.SlotCallUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 6).FirstOrDefault().UserID;
                    }

                    // Fetch its comments:
                    currentMonthAllocation.DayComments = db.MDR_DAY_COMMENT.Where(c => c.DateID == allocation.SESSION.DateID).FirstOrDefault().CommentsValue;


                // Next month's info:
                ViewBag.NextMonthName = "November " + DateTime.Today.AddMonths(1).Date.Year;

                foreach (var date in db.SESSION.Where(x => x.DateID.Month == 11 && x.SlotID == 1))
                {
                    selectDatesNextMonth.Add(new SelectListItem { Value = date.DateID.ToString(), Text = date.DateID.ToString("ddd dd MMM") });
                }
                ViewBag.DatesNextMonth = selectDatesNextMonth;

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushSuccess, "Scheduling");
                #endregion
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushFail, "Scheduling");
                #endregion
                return RedirectToAction("Error", "Home", new HandleErrorInfo(x, "Scheduling", "DailyAutopsySchedule"));
            }
        }
//----------------------------------------------------------------------------------------------//
        [AuthorizeByAccessArea(AccessArea = "Capture Daily Autopsy Schedule")]
        [AuthorizeByAccessArea(AccessArea = "Retrieve Daily Autopsy Schedule")]
        public ActionResult DailyAutopsySchedule()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushInit, "Scheduling");
                #endregion


                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushSuccess, "Scheduling");
                #endregion
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushFail, "Scheduling");
                #endregion
                return RedirectToAction("Error", "Home", new HandleErrorInfo(x, "Scheduling", "DailyAutopsySchedule"));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Retrieve Daily Autopsy Schedule")]
        public ActionResult RetrievedDailyAutopsySchedule(DateTime id)
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushInit, "Scheduling");
                #endregion

                //DateTime target = new DateTime(2017, 2, 20, 0, 0, 0);
                ViewBag.RetrieveDate = id;

                ViewBag.PreviewURL = "";


                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushSuccess, "Scheduling");
                #endregion
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushFail, "Scheduling");
                #endregion
                return RedirectToAction("Error", "Home", new HandleErrorInfo(x, "Scheduling", "DailyAutopsySchedule"));
            }
        }


    }
}
