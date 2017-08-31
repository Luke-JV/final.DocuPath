using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Models.DPViewModels
{
    public class TokenViewModel
    {
        public List<TOKEN_LOG> tokenList { get; set; }
        public List<ACCESS_LEVEL> ualList { get; set; }
        public int tokenCount { get; set; }
                
    }
}