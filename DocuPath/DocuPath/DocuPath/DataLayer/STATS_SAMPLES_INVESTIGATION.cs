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
    using System;
    using System.Collections.Generic;
    
    public partial class STATS_SAMPLES_INVESTIGATION
    {
        public int CaseStatsID { get; set; }
        public int ForensicCaseID { get; set; }
        public int SampleInvestigationID { get; set; }
        public string SampleInvestigationOtherDescription { get; set; }
    
        public virtual CASE_STATISTICS CASE_STATISTICS { get; set; }
        public virtual SAMPLE_INVESTIGATION SAMPLE_INVESTIGATION { get; set; }
    }
}