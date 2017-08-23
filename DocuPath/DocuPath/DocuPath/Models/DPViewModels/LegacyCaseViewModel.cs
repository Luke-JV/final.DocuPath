using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class LegacyCaseViewModel
    {
        public LEGACY_CASE legacyCase {get;set;}
        public List<LEGACY_DOCUMENT> legacyDocs {get;set;}
}
}