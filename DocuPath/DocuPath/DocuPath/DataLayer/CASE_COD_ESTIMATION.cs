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
    using System.ComponentModel.DataAnnotations;

    public partial class CASE_COD_ESTIMATION
    {
        
        public int ProminenceID { get; set; }
        [Required]
        public int ContentTagID { get; set; }
        public int ForensicCaseID { get; set; }
        [DefaultValue(FLAG.Text)]
        public string CODMotivation { get; set; }
    
        public virtual COD_PROMINENCE COD_PROMINENCE { get; set; }
        public virtual FORENSIC_CASE FORENSIC_CASE { get; set; }
        public virtual CONTENT_TAG CONTENT_TAG { get; set; }
    }
}
