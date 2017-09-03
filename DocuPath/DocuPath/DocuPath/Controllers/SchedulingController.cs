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
        
        [AuthorizeByAccessArea(AccessArea= "Access Google Calendar")]
        public ActionResult Index()
        {
            try
            {

                return RedirectToAction("Calendar");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Access Google Calendar")]
        public ActionResult Calendar()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullInit, "Media");
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullSuccess, "Media");
                #endregion
                return null;//404
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPullFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Compile Monthly Duty Roster")]
        public ActionResult MonthlyDutyRoster()
        {
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushInit, "Media");
                #endregion
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushSuccess, "Media");
                #endregion
                return null;//404
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.CalendarPushFail, "Media");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }



    }
}
