﻿using DocuPath.DataLayer;
using DocuPath.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    [Authorize]
    [HandleError]
    [LogAction]
    public class HomeController : Controller
    {
        DocuPathEntities db = new DocuPathEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            try
            {

                ViewBag.Message = "Your contact page.";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ViewResult Error(string errorMessage)
        {
            HandleErrorInfo model = new HandleErrorInfo(new Exception(errorMessage) ,"Home","Index");
            try
            {
                
                ModelState.AddModelError("", "error/failure"); // Add the ModelState error
                ViewBag.ErrorMessage = errorMessage; // Pass the error message to the view for detailed error handling and flat file dumping using ViewBag
                return View(model);
            }
            catch (Exception)
            {
                //TODO: errors on the error page? So you can error while you error? Lel...
                return View();
            }
        }

        [HttpPost]
        public ActionResult Error(System.Web.Mvc.HandleErrorInfo err)
        {
            // 404
            string fname = @"\ErrorDump_User." + VERTEBRAE.getCurrentUser().DisplayInitials.ToUpper() + "_" + DateTime.Now.Date.ToString("dd.MM.YYYY") + "_" + DateTime.Now.TimeOfDay.ToString("HH.mm") + ".txt";
            using (StreamWriter sw = new StreamWriter(Server.MapPath(VERTEBRAE.ErrorDumpRootPath + fname)))
            {

            }

            return null;
        }

        public ActionResult SendMail()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult SendSms()
        {
            try
            {

                VERTEBRAE.sendSMS("++27825592322", "CODE:xxxxxx");
                return View("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult DumpError(System.Web.Mvc.HandleErrorInfo err)
        {
            string fname = @"\ErrorDump_User." + VERTEBRAE.getCurrentUser().DisplayInitials.ToUpper() + "_" + DateTime.Now.Date.ToString("dd.MM.YYYY") + "_" + DateTime.Now.TimeOfDay.ToString("HH.mm") + ".txt";
            using (StreamWriter sw = new StreamWriter(Server.MapPath(VERTEBRAE.ErrorDumpRootPath + fname)))
            {
                
            }

            return null;
        }
    }
}