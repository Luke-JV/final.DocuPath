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

                //Model:
                MonthlyDutyRosterViewModel model = new MonthlyDutyRosterViewModel();

                // General Info:
                List<UserKVP> selectUsers = new List<UserKVP>();
                List<SlotKVP> selectSlots = new List<SlotKVP>();

                foreach (var item in db.USER.Where(u => u.IsDeactivated == false))
                {
                    UserKVP userToAdd = new UserKVP();
                    userToAdd.uID = item.UserID;
                    userToAdd.uInitials = item.DisplayInitials;
                    selectUsers.Add(userToAdd);
                }
                model.users = selectUsers;

                foreach (var item in db.SLOT)
                {
                    SlotKVP slotToAdd = new SlotKVP();
                    slotToAdd.SlotID = item.SlotID;
                    slotToAdd.SlotDesc = item.Description;
                    selectSlots.Add(slotToAdd);
                }
                model.slots = selectSlots;

                // Current & next month's info:
                var thisMonth = DateTime.Now.Month;
                var nextMonth = DateTime.Now.AddMonths(1).Month;
                var year = DateTime.Now.Year;

                switch (thisMonth)
                {
                    case 1: // JAN
                        ViewBag.CurrentMonthName = "January " + year;
                        ViewBag.NextMonthName = "February " + year;
                        break;
                    case 2: // FEB
                        ViewBag.CurrentMonthName = "February " + year;
                        ViewBag.NextMonthName = "March " + year;
                        break;
                    case 3: // MAR
                        ViewBag.CurrentMonthName = "March " + year;
                        ViewBag.NextMonthName = "April " + year;
                        break;
                    case 4: // APR
                        ViewBag.CurrentMonthName = "April " + year;
                        ViewBag.NextMonthName = "May " + year;
                        break;
                    case 5: // MAY
                        ViewBag.CurrentMonthName = "May " + year;
                        ViewBag.NextMonthName = "June " + year;
                        break;
                    case 6: // JUN
                        ViewBag.CurrentMonthName = "June " + year;
                        ViewBag.NextMonthName = "July " + year;
                        break;
                    case 7: // JUL
                        ViewBag.CurrentMonthName = "July " + year;
                        ViewBag.NextMonthName = "August " + year;
                        break;
                    case 8: // AUG
                        ViewBag.CurrentMonthName = "August " + year;
                        ViewBag.NextMonthName = "September " + year;
                        break;
                    case 9: // SEP
                        ViewBag.CurrentMonthName = "September " + year;
                        ViewBag.NextMonthName = "October " + year;
                        break;
                    case 10: // OCT
                        ViewBag.CurrentMonthName = "October " + year;
                        ViewBag.NextMonthName = "November " + year;
                        break;
                    case 11: // NOV
                        ViewBag.CurrentMonthName = "November " + year;
                        ViewBag.NextMonthName = "December " + year;
                        break;
                    case 12: // DEC
                        ViewBag.CurrentMonthName = "December " + year;
                        ViewBag.NextMonthName = "January " + year + 1;
                        break;
                    default:
                        break;
                }

                // CURRENT MONTH:
                List<DayAllocationsComments> allocationsCurrent = new List<DayAllocationsComments>();

                foreach (var allocation in db.SESSION_USER.Where(a => a.SESSION.DateID.Month == thisMonth && a.SESSION.SlotID == 1))
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
                        currentMonthAllocation.Slot1AUID = db.SESSION.Where(su => su.DateID == allocation.SESSION.DateID && su.SlotID == 1).FirstOrDefault().SESSION_USER.FirstOrDefault().UserID;
                        // Allocate its 1B UserID:
                        currentMonthAllocation.Slot1BUID = db.SESSION.Where(su => su.DateID == allocation.SESSION.DateID && su.SlotID == 2).FirstOrDefault().SESSION_USER.FirstOrDefault().UserID;
                        // Allocate its 1C UserID:
                        currentMonthAllocation.Slot1CUID = db.SESSION.Where(su => su.DateID == allocation.SESSION.DateID && su.SlotID == 3).FirstOrDefault().SESSION_USER.FirstOrDefault().UserID;
                        // Allocate its 2A UserID:
                        currentMonthAllocation.Slot2AUID = db.SESSION.Where(su => su.DateID == allocation.SESSION.DateID && su.SlotID == 4).FirstOrDefault().SESSION_USER.FirstOrDefault().UserID;
                        // Allocate its 2B UserID:
                        currentMonthAllocation.Slot2BUID = db.SESSION.Where(su => su.DateID == allocation.SESSION.DateID && su.SlotID == 5).FirstOrDefault().SESSION_USER.FirstOrDefault().UserID;
                        // Allocate its CALL UserID:
                        currentMonthAllocation.SlotCallUID = db.SESSION.Where(su => su.DateID == allocation.SESSION.DateID && su.SlotID == 6).FirstOrDefault().SESSION_USER.FirstOrDefault().UserID;
                    }
                    else
                    {
                        var x = allocation;
                    }

                    // Fetch its comments:
                    currentMonthAllocation.DayComments = db.MDR_DAY_COMMENT.Where(c => c.DateID == allocation.SESSION.DateID).FirstOrDefault().CommentsValue;

                    // Add it to the allocations list:
                    allocationsCurrent.Add(currentMonthAllocation);
                }
                model.currentMonthAllocations = allocationsCurrent;

                // NEXT MONTH:
                List<DayAllocationsComments> allocationsNext = new List<DayAllocationsComments>();

                foreach (var allocation in db.SESSION_USER.Where(a => a.SESSION.DateID.Month == nextMonth && a.SESSION.SlotID == 1))
                {
                    // Make a new allocation:
                    DayAllocationsComments nextMonthAllocation = new DayAllocationsComments();

                    // Set its date equal to the allocation's date:
                    nextMonthAllocation.Date = allocation.SESSION.DateID;

                    // Set its date string equal to the string conversion of the allocation's date:
                    nextMonthAllocation.DateString = allocation.SESSION.DateID.ToString("ddd dd MMM");

                    // Allocate its 1A UserID:
                    nextMonthAllocation.Slot1AUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 1).FirstOrDefault().UserID;
                    // Allocate its 1B UserID:
                    nextMonthAllocation.Slot1BUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 2).FirstOrDefault().UserID;
                    // Allocate its 1C UserID:
                    nextMonthAllocation.Slot1CUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 3).FirstOrDefault().UserID;
                    // Allocate its 2A UserID:
                    nextMonthAllocation.Slot2AUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 4).FirstOrDefault().UserID;
                    // Allocate its 2B UserID:
                    nextMonthAllocation.Slot2BUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 5).FirstOrDefault().UserID;
                    // Allocate its CALL UserID:
                    nextMonthAllocation.SlotCallUID = db.SESSION_USER.Where(su => su.SESSION.DateID == allocation.SESSION.DateID && su.SESSION.SlotID == 6).FirstOrDefault().UserID;

                    // Fetch its comments:
                    nextMonthAllocation.DayComments = db.MDR_DAY_COMMENT.Where(c => c.DateID == allocation.SESSION.DateID).FirstOrDefault().CommentsValue;

                    // Add it to the allocations list:
                    allocationsNext.Add(nextMonthAllocation);
                }
                model.nextMonthAllocations = allocationsNext;

                //List<string> selectDatesCurrentMonth = new List<string>();

                //foreach (var date in db.SESSION.Where(x => x.DateID.Month == DateTime.Today.Date.Month && x.SlotID == 1))
                //{
                //    selectDatesCurrentMonth.Add(date.DateID.ToString("ddd dd MMM"));
                //}
                //model.currentMonthDates = selectDatesCurrentMonth;

                //// NEXT MONTH:
                //List<string> selectDatesNextMonth = new List<string>();
                //foreach (var date in db.SESSION.Where(x => x.DateID.Month == 10 && x.SlotID == 1))
                //{
                //    selectDatesNextMonth.Add(date.DateID.ToString("ddd dd MMM"));
                //}
                //model.nextMonthDates = selectDatesNextMonth;

                //foreach (var allocation in db.SESSION_USER.Where(a => a.SESSION.DateID.Month == thisMonth))
                //{
                //    DayAllocationsComments allocationToGet = new DayAllocationsComments();
                //    switch (allocation.SESSION.SlotID)
                //    {
                //        case 1: // SLOT 1A
                //            allocationToGet.
                //            break;
                //        case 2: // SLOT 1B
                //            break;
                //        case 3: // SLOT 1C
                //            break;
                //        case 4: // SLOT 2A
                //            break;
                //        case 5: // SLOT 2B
                //            break;
                //        case 6: // SLOT CALL
                //            break;
                //        default:
                //            break;
                //    }

                //}



                //List<SESSION_USER> allocationsCurrent = new List<SESSION_USER>();
                //foreach (var allocation in db.SESSION_USER.Where(a => a.SESSION.DateID.Month == thisMonth))
                //{

                //}

                ////for (int cur = 0; cur < selectDatesCurrentMonth.Count; cur++)
                ////{
                ////    if (db.SESSION_USER.Where(s => s.UserID == -1).)
                ////    {

                ////    }
                ////}
                //model.currentMonthAllocations = allocationsCurrent;

                //List<SESSION_USER> allocationsNext = new List<SESSION_USER>();
                //model.nextMonthAllocations = allocationsNext;

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushSuccess, "Scheduling");
                #endregion
                return View(model);
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
