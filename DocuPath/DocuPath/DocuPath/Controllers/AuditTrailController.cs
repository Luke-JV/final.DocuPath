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
        DocuPathEntities db = new DocuPathEntities();

        public ActionResult Index()
        {
            try
            {

                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Audit Trail - View")]
        public ActionResult All()
        {
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
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Full Access Master")]
        public ActionResult Details(int id)
        {
            try
            {

                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchSuccess, "Audit Log");
                #endregion
                return View();
            }
            catch (Exception)
            {
                #region AUDIT_WRITE
                AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.SearchFail, "Audit Log");
                #endregion
                return RedirectToAction("Error", "Home");
            }
        }

        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:

        #endregion
    }
}
