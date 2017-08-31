using DocuPath.Models.DPViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Controllers
{
    public class TestController : Controller
    {
        

        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TagCloud()
        {
            TagCloudViewModel model = new TagCloudViewModel();
            return View(model);
        }
    }
}