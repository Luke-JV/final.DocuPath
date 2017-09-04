using DocuPath.DataLayer;
using DocuPath.Models;
using System;
using System.Collections.Generic;
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
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea= "Access Google Calendar")]
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

        [AuthorizeByAccessArea(AccessArea = "Access Google Calendar")]
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
                return View();
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullFail, "Scheduling");
                #endregion
                return RedirectToAction("Error", "Home", new HandleErrorInfo(x, "Scheduling", "DailyAutopsySchedule"));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Compile Monthly Duty Roster")]
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
                selectUsers.Add(new SelectListItem { Value = "1", Text = "Override" });
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

                ViewBag.CurrentMonthName = "September " + DateTime.Today.Date.Year;

                foreach (var date in db.SESSION.Where(x => x.DateID.Month == DateTime.Today.Date.Month && x.SlotID == 1))
                {
                    selectDatesCurrentMonth.Add(new SelectListItem { Value = date.DateID.ToString(), Text = date.DateID.ToString("ddd dd MMM") });
                }
                ViewBag.DatesCurrentMonth = selectDatesCurrentMonth;


                // Next month's info:
                ViewBag.NextMonthName = "October " + DateTime.Today.AddMonths(1).Date.Year;

                foreach (var date in db.SESSION.Where(x => x.DateID.Month == 10 && x.SlotID == 1))
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

    }
}
