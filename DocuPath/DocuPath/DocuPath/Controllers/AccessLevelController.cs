using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    public class AccessLevelController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        
        [AuthorizeByAccessArea(AccessArea = "Search User Access Level")]
        public ActionResult Index()
        {

            return RedirectToAction("All");
        }
        //----------------------------------------------------------------------------------------------//
        #region CREATES:
        
        [AuthorizeByAccessArea(AccessArea = "Add User Access Level")]
        public ActionResult Add()
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion

            AccessLevelViewModel model = new AccessLevelViewModel();
            model.accessLevel = new ACCESS_LEVEL();
            model.allAreas = db.ACCESS_AREA.ToList();
            model.fxGroups = db.FUNCTION_GROUP.ToList();

            return View(model);
        }

        
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Add User Access Level")]
        public ActionResult Add(ACCESS_LEVEL AL)
        {
            try
            {
                db.ACCESS_LEVEL.Add(AL);
                db.SaveChanges();
                // TODO: Add insert logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region READS:
        
        [AuthorizeByAccessArea(AccessArea = "Search User Access Level")]
        public ActionResult All()
        {
            try
            {
                ViewBag.Neurons = VERTEBRAE.GetUnhandledNeurons();
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View(db.ACCESS_LEVEL.ToList());
            }
            catch (Exception x)
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Error", "Home", x.Message);
            }
        }

        
        [AuthorizeByAccessArea(AccessArea = "View User Access Level")]
        public ActionResult Details(int id)
        {
            #region MODEL POPULATION
            ACCESS_LEVEL model = db.ACCESS_LEVEL.Where(x => x.AccessLevelID == id).FirstOrDefault();

            #endregion
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View(model);
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region UPDATES:
        
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User Access Level")]
        public ActionResult Edit(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");,
            #endregion
            return View();
        }

        // POST: AccessLevel/Edit/5
        [HttpPost]
        [AuthorizeByAccessArea(AccessArea = "Update/Edit User Access Level")]
        public ActionResult Edit(int id, ACCESS_LEVEL updatedLevel)
        {
            try
            {
                #region DB UPDATE
                db.ACCESS_LEVEL.Attach(updatedLevel);
                db.Entry(updatedLevel).State = EntityState.Modified;
                db.SaveChanges();
                #endregion
                // TODO: Add update logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region DELETES:
        
        [AuthorizeByAccessArea(AccessArea = "Deactivate User Access Level")]
        public ActionResult Delete(int id)
        {
            #region AUDIT_WRITE
            //AuditModel.WriteTransaction(0, "404");
            #endregion
            return View();
        }

        
        [AuthorizeByAccessArea(AccessArea = "Deactivate User Access Level")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return RedirectToAction("Index");
            }
            catch
            {
                #region AUDIT_WRITE
                //AuditModel.WriteTransaction(0, "404");
                #endregion
                return View();
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------//
        #region NON-CRUD ACTIONS:

        public ActionResult test()
        {
            List<SelectListItem> selectFCFunctions = new List<SelectListItem>();
            List<SelectListItem> selectERCFunctions = new List<SelectListItem>();
            List<SelectListItem> selectLCFunctions = new List<SelectListItem>();
            List<SelectListItem> selectMediaFunctions = new List<SelectListItem>();
            List<SelectListItem> selectINSIGHTFunctions = new List<SelectListItem>();
            List<SelectListItem> selectVISIONFunctions = new List<SelectListItem>();
            List<SelectListItem> selectSPFunctions = new List<SelectListItem>();
            List<SelectListItem> selectSRFunctions = new List<SelectListItem>();
            List<SelectListItem> selectSchedulingFunctions = new List<SelectListItem>();
            List<SelectListItem> selectUserFunctions = new List<SelectListItem>();
            List<SelectListItem> selectAuditTrailFunctions = new List<SelectListItem>();
            List<SelectListItem> selectUALFunctions = new List<SelectListItem>();
            List<SelectListItem> selectCTFunctions = new List<SelectListItem>();

            foreach (var item in db.ACCESS_AREA)
            {
                if (item.AccessAreaID >= 0 && item.AccessAreaID <= 29)
                {
                    selectFCFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 30 && item.AccessAreaID <= 36)
                {
                    selectERCFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 37 && item.AccessAreaID <= 41)
                {
                    selectMediaFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 42 && item.AccessAreaID <= 46)
                {
                    selectSPFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 47 && item.AccessAreaID <= 51)
                {
                    selectSRFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 52 && item.AccessAreaID <= 57)
                {
                    selectSchedulingFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 58 && item.AccessAreaID <= 62)
                {
                    selectLCFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 63 && item.AccessAreaID <= 71)
                {
                    selectINSIGHTFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID == 72)
                {
                    selectVISIONFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 73 && item.AccessAreaID <= 79)
                {
                    selectUserFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 80 && item.AccessAreaID <= 81)
                {
                    selectAuditTrailFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 82 && item.AccessAreaID <= 86)
                {
                    selectUALFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID >= 87 && item.AccessAreaID <= 91)
                {
                    selectCTFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
                else if (item.AccessAreaID == 92)
                {
                    selectUserFunctions.Add(new SelectListItem { Value = item.AccessAreaID.ToString(), Text = item.AccessAreaDescription });
                }
            }

            ViewBag.FC = selectFCFunctions;
            ViewBag.ERC = selectERCFunctions;
            ViewBag.LC = selectLCFunctions;
            ViewBag.Media = selectMediaFunctions;
            ViewBag.INSIGHT = selectINSIGHTFunctions;
            ViewBag.VISION = selectVISIONFunctions;
            ViewBag.SP = selectSPFunctions;
            ViewBag.SR = selectSRFunctions;
            ViewBag.Scheduling = selectSchedulingFunctions;
            ViewBag.User = selectUserFunctions;
            ViewBag.AuditTrail = selectAuditTrailFunctions;
            ViewBag.UAL = selectUALFunctions;
            ViewBag.CT = selectCTFunctions;

            return View();
        }

        #endregion
    }
}
