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

    public partial class SAMPLE_INVESTIGATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SAMPLE_INVESTIGATION()
        {
            this.STATS_SAMPLES_INVESTIGATION = new HashSet<STATS_SAMPLES_INVESTIGATION>();
        }
    
        public int SampleInvestigationID { get; set; }
        [DefaultValue(FLAG.Text)]
        public string SampleInvestigationDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_SAMPLES_INVESTIGATION> STATS_SAMPLES_INVESTIGATION { get; set; }
    }
}
