using DocuPath.DataLayer;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ContentTagViewModel
    {
        public CONTENT_TAG tag { get; set; }
        public string catName { get; set; }
        public int catID { get; set; }
        public string subcatName { get; set; }
        public int subcatID { get; set; }
        public string conditionName { get; set; }
        public int conditionID { get; set; }

        //public List<TAG_CATEGORY> categories { get; set; }
        //public List<TAG_SUBCATEGORY> subcategories { get; set; }
        //public List<TAG_CONDITION> conditions { get; set; }
    }
}