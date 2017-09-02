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
    [LogAction]
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

                return null;//404
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeByAccessArea(AccessArea = "Compile Monthly Duty Roster")]
        public ActionResult MonthlyDutyRoster()
        {
            try
            {

                return null;//404
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }



    }
}
