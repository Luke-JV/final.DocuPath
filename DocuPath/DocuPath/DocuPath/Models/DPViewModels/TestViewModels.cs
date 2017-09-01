using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class TagCloudViewModel
    {
        public string tagName { get; set; }
        public int tagID { get; set; }
    }

    public class ToggleViewModel
    {
        public bool tog1 { get; set; }
        public bool tog2 { get; set; }
        public int togID { get; set; }

       

        public List<areaKVP> areas { get; set; }
    }
    public class areaKVP
    {
        public string areaName { get; set; }
        public bool hasAccess { get; set; }

        //public string areaName;
        //public bool hasAccess;
    }
}
