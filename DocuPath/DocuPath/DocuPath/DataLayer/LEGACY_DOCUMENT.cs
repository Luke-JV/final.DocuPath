//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocuPath.DataLayer
{
    using DocuPath.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class LEGACY_DOCUMENT
    {
        public int LegacyDocumentID { get; set; }
        public int LegacyCaseID { get; set; }
        [DefaultValue(FLAG.Text)]
        public string LegacyDocumentTitle { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        public string LegacyDocumentLocation { get; set; }
    
        public virtual LEGACY_CASE LEGACY_CASE { get; set; }
    }
}
