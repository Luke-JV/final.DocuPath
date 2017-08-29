using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class AccessLevelViewModel
    {
        public ACCESS_LEVEL accessLevel { get; set; }
        public List<ACCESS_AREA> areas { get; set; }
        public List<ACCESS_AREA> allAreas { get; set; }
    }
}