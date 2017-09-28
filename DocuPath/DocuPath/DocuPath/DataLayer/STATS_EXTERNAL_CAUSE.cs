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

    public partial class STATS_EXTERNAL_CAUSE
    {
        public int CaseStatsID { get; set; }
        public int ForensicCaseID { get; set; }
        public int ExternalCauseID { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string OtherExternalCauseDescription { get; set; }
    
        public virtual CASE_STATISTICS CASE_STATISTICS { get; set; }
        public virtual EXTERNAL_CAUSE EXTERNAL_CAUSE { get; set; }
    }
}
