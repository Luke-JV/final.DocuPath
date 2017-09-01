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
        public List<FUNCTION_GROUP> fxGroups { get; set; }

        public List<selectAreaKVP> FCAreas { get; set; }
        public List<selectAreaKVP> ECAreas { get; set; }
        public List<selectAreaKVP> LCAreas { get; set; }
        public List<selectAreaKVP> MediaAreas { get; set; }
        public List<selectAreaKVP> InsightAreas { get; set; }
        public List<selectAreaKVP> VisionAreas { get; set; }
        public List<selectAreaKVP> SPAreas { get; set; }
        public List<selectAreaKVP> SRAreas { get; set; }
        public List<selectAreaKVP> SchedulingAreas { get; set; }
        public List<selectAreaKVP> UserAreas { get; set; }
        public List<selectAreaKVP> AuditAreas { get; set; }
        public List<selectAreaKVP> AccessLevelAreas { get; set; }
        public List<selectAreaKVP> ContentTagAreas { get; set; }

    }
    public class selectAreaKVP
    {
        public string areaName { get; set; }
        public bool hasAccess { get; set; }

 
    }
}
   