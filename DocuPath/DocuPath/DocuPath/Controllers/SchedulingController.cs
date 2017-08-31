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
    public class SchedulingController : Controller
    {
        
        [AuthorizeByAccessArea(AccessArea= "Access Google Calendar")]
        public ActionResult Index()
        {
 
            return RedirectToAction("Calendar");
        }

        [AuthorizeByAccessArea(AccessArea = "Access Google Calendar")]
        public ActionResult Calendar()
        {
            return null;//404
        }

        [AuthorizeByAccessArea(AccessArea = "Compile Monthly Duty Roster")]
        public ActionResult MonthlyDutyRoster()
        {
            return null;//404
        }



    }
}
