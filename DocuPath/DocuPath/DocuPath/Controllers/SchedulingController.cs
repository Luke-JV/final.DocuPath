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
        
        [AuthorizeByAccessArea(AccessArea="404")]
        public ActionResult Index()
        {
            //404 - redirect
            return View();
        }

        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult Calendar()
        {
            return null;//404
        }

        [AuthorizeByAccessArea(AccessArea = "404")]
        public ActionResult MonthlyDutyRoster()
        {
            return null;//404
        }



    }
}
