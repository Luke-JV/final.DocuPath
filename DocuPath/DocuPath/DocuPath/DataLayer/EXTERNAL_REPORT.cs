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
    
    public partial class EXTERNAL_REPORT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EXTERNAL_REPORT()
        {
            this.SPECIMEN = new HashSet<SPECIMEN>();
        }
    
        public int ExternalReportID { get; set; }
        public System.DateTime DateReceived { get; set; }
        public System.DateTime DateCaptured { get; set; }
        public string ExternalReportLocation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SPECIMEN> SPECIMEN { get; set; }
    }
}