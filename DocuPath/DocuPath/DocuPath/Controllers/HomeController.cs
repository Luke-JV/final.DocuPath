using DocuPath.DataLayer;
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
    //[LogAction]
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

        public ViewResult Error(HandleErrorInfo model)
        {
            //HandleErrorInfo model = new HandleErrorInfo(new Exception(errorMessage) ,"Home","Index");
            try
            {
                
                //ModelState.AddModelError(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"), model.Exception); // Add the ModelState error
                //ViewBag.ErrorMessage = errorMessage; // Pass the error message to the view for detailed error handling and flat file dumping using ViewBag
                
                return View(model);
            }
            catch (Exception)
            {
                //TODO: errors on the error page? So you can error while you error? Lel...
                return View();
            }
        }

        
        public ActionResult DumpError(string dumpedFileLocation)
        {
                      
            try
            {
                VERTEBRAE.sendMail("ruco@cldrm.co.za","Please note an error has been logged, view the error here:"+dumpedFileLocation,"Error");
                return RedirectToAction("Index","Home");
            }
            catch (Exception x)
            {
                return null;
            }
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
        [HttpPost]
        public ActionResult SendRSS(string rssData)
        {
            try
            {
                VERTEBRAE.sendMail("docupath.vector@gmail.com", "Broadcast:\n\n"+ rssData + "\n\nSent from the DocuPath NEURON Notifications Centre.","Departmental Broadcast");
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}