using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class UserViewModel
    {
        public List<USER> users { get; set; }
        public int tkCount { get; set; }
    }
}