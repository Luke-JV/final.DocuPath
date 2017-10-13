using DocuPath.DataLayer;
using DocuPath.Models;
using DocuPath.Models.Custom_Classes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
   // [Authorize]
    //[HandleError]
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

        [HttpPost]
        public ActionResult Contact(CONTACT_US model)
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

        public ViewResult Error(HandleErrorInfo model)
        {
            //HandleErrorInfo model = new HandleErrorInfo(new Exception(errorMessage) ,"Home","Index");
            try
            {

                //ModelState.AddModelError(DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"), model.Exception); // Add the ModelState error
                //ViewBag.ErrorMessage = errorMessage; // Pass the error message to the view for detailed error handling and flat file dumping using ViewBag
                Session["Error"] = model;
                return View(model);
            }
            catch (Exception)
            {
                //TODO: errors on the error page? So you can error while you error? Lel...
                return View();
            }
        }

        
        public ActionResult DumpError(string message)
        {
                      
            try
            {

                string filepath = VERTEBRAE.ErrorDumpRootPath;  //Text File Path
                string fname = @"\ErrorDump_User." + VERTEBRAE.getCurrentUser().DisplayInitials.ToUpper() +".txt";

                bool exists = System.IO.Directory.Exists(Server.MapPath(filepath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(filepath));
                StreamWriter sw = new StreamWriter(Server.MapPath(filepath)+fname);
                
                    string error = "Error Dump DateTimeStamp:" + " " + DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
                    sw.WriteLine("[[------- ERROR LOG FILE FOR " + DateTime.Now.ToString("dd MMM yyyy at HH:mm:ss") + " --------------]]");
                    sw.WriteLine(">---------------------------<<< START >>>---------------------------<");                    
                    sw.WriteLine(error);
                sw.WriteLine(message);
                sw.WriteLine(">----------------------------<<< END >>>----------------------------<");
                    
                   // sw.Flush();
                    sw.Close();
                
                VERTEBRAE.sendMail("ruco@cldrm.co.za","Please note an error has been logged, view the error here:"+filepath,"Error");
                return RedirectToAction("Index","Home");
                    
                
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
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

                VERTEBRAE.sendSMS("+27825592322", "CODE:xxxxxx");
                return View("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        // localhost:56640/account/register/1AF7rjEMiNJvOhR2b1%2bIDozA3ueoShCcGleCxB%2fjy7jXYON521XoVBG9sf4FMXq1HCw%3d%3d
        // localhost:56640/account/register/1AF7rjEMiNJvOhR2b1%252bIDozA3ueoShCcGleCxB%252fjy7jXYON521XoVBG9sf4FMXq1HCw%253d%253d
        // localhost:56640/account/register/1AF7rjEMiNJvOhR2b1%25252bIDozA3ueoShCcGleCxB%25252fjy7jXYON521XoVBG9sf4FMXq1HCw%25253d%25253d
        public ActionResult testEncode()
        {
            string str  = "1AF7rjEMiNJvOhR2b1+IDozA3ueoShCcGleCxB/jy7jXYON521XoVBG9sf4FMXq1HCw==";
            var x = Url.Encode(str);
            var y = Server.UrlEncode(str);
            var z = Url.Encode(x);
            var a = Url.Encode(z);
            return null;
        }

        public ActionResult ModalNotificationTest()
        {
            SPINE_OBSERVATION model = new SPINE_OBSERVATION();
            return View(model);
        }

        public JsonResult SendRSS(string rssData)
        {
            try
            {
                VERTEBRAE.sendMail("docupath.vector@gmail.com", "Official Communication from the University of Pretoria Department of Forensic Medicine\nSent at " + DateTime.Now.ToString("HH:mm") + " on " + DateTime.Now.ToString("dd MMM yyyy"), rssData);
                return Json("RSS Broadcast sent successfully!");
            }
            catch (Exception x)
            {

                return Json("An error occurred. Here's what happened: " + x.Message);
            }
        }

        public ActionResult HapticHelp()
        {
            return View();
        }


        public ActionResult HandleNeuron(int id)
        {
            try
            {
                NOTIFICATION neuron = db.NOTIFICATION.Where(x => x.NotificationID == id).FirstOrDefault();
                string instruction = Request.Form.Get("INSTRUCTION");
                switch (neuron.NOTIFICATION_TYPE.NotificationTypeValue)
                {
                    case "AcceptReject":
                        if (instruction == "ACCEPT")
                        {
                            AcceptNeuron(neuron);
                        }
                        else
                        {
                            RejectNeuron(neuron);
                        }
                        break;

                    case "ApproveDeny":
                        if (instruction == "APPROVE")
                        {
                            ApproveNeuron(neuron);
                        }
                        else
                        {
                            DenyNeuron(neuron);
                        }
                        break;

                    case "Information":
                        break;                    

                    default:
                        break;
                }

                neuron.HandledDateTimeStamp = DateTime.Now;
                db.NOTIFICATION.Attach(neuron);
                db.Entry(neuron).State = EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public void ApproveNeuron(NOTIFICATION inbound)
        {
            if (inbound.NotificationTitle == "LOCK")
            {
                db.FORENSIC_CASE.Where(x => x.ForensicDRNumber == inbound.NotificationSummary).FirstOrDefault().StatusID = db.STATUS.Where(x => x.StatusValue == "Locked").FirstOrDefault().StatusID;
            }
        }

        
        public void DenyNeuron(NOTIFICATION inbound)
        {
           
        }

        
        public void AcceptNeuron(NOTIFICATION inbound)
        {
          
        }

        
        public void RejectNeuron(NOTIFICATION inbound)
        {
            
        }

        


    }
}