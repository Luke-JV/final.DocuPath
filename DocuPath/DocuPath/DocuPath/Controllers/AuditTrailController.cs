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
   // [LogAction]
    public class AuditTrailController : Controller
    {
        string controllerName = "AuditTrail";
        DocuPathEntities db = new DocuPathEntities();

        public ActionResult Index()
        {
            string actionName = "Index";
            try
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Audit Log");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Audit Trail - View")]
        public ActionResult All()
        {
            string actionName = "All";
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchInit, "Audit Log");
                #endregion
                return View(db.AUDIT_LOG.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Audit Log");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Full Access Master")]
        public ActionResult Details(int id)
        {
            string actionName = "Details";
            try
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchSuccess, "Audit Log");
                #endregion
                AUDIT_LOG model = new AUDIT_LOG();
                model = db.AUDIT_LOG.Where(x => x.AuditLogTxID == id).FirstOrDefault();
                model.TxOldRecord = model.TxOldRecord.Replace("\",\"", "\" | \"").Replace(",\"", " | \"").Replace("{", "").Replace("}", "");
                model.TxNewRecord = model.TxNewRecord.Replace("\",\"", "\" | \"").Replace(",\"", " | \"").Replace("{", "").Replace("}", "");
                return View(model);
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SysFlagFail, "Audit Log");
                #endregion
                VERTEBRAE.DumpErrorToTxt(x);
                return View("Error", new HandleErrorInfo(x, controllerName, actionName));
            }
        }

        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:

        #endregion
    }
}
