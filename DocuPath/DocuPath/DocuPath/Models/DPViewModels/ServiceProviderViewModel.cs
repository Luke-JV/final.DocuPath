using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ServiceProviderViewModel
    {
        public SERVICE_PROVIDER serviceProvider { get; set; }
        public List<TITLE> titles { get; set; }
    }
}