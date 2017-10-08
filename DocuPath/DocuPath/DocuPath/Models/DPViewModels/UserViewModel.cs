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
        public List<UiPrefKVP> uiprefs { get; set; }
    }

    public class UpdateUserViewModel
    {
        public USER user { get; set; }
        public List<TITLE> titles { get; set; }
        public List<UiPrefKVP> uiprefs { get; set; }
    }

    public class UiPrefKVP
    {
        public Nullable<int> prefID { get; set; }
        public string prefPhrase { get; set; }
    }
}