using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class SchedulingViewModel
    {
        public List<UserKVP> userInitials { get; set; }
    }

    public class UserKVP
    {
        public int uID { get; set; }
        public string uInitials { get; set; }
    }
}