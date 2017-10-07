using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ExternalReviewCaseViewModel
    {
        public EXTERNAL_REVIEW_CASE extCase { get; set; }
        public List<UserFullKVP> users { get; set; }
        public List<STATUS> statuses { get; set; }
    }
}