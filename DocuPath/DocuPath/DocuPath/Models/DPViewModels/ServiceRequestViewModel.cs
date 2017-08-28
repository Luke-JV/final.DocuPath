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
}