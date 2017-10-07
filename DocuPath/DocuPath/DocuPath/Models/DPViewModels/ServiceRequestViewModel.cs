using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ServiceRequestViewModel
    {
        public SERVICE_REQUEST serviceRequest { get; set; }
        public SPECIMEN specimen { get; set; }
    }

    public class FCFilteredSRModel
    {
        public List<SERVICE_REQUEST> FCSRList { get; set; }
        public List<SPECIMEN> FCSRSpecimenList { get; set; }
    }

    public class LinkERToSRViewModel
    {
        public SERVICE_REQUEST targetSR { get; set; }
        public EXTERNAL_REPORT targetER { get; set; }
        public List<ReportKVP> existingERList { get; set; }
    }
}