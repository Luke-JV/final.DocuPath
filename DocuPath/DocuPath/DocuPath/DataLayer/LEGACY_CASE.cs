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
    
    public partial class LEGACY_CASE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LEGACY_CASE()
        {
            this.LEGACY_DOCUMENT = new HashSet<LEGACY_DOCUMENT>();
        }
    
        public int LegacyCaseID { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public string LegacyDRNumber { get; set; }
        public string BriefDescription { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> DateClosed { get; set; }
    
        public virtual STATUS STATUS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LEGACY_DOCUMENT> LEGACY_DOCUMENT { get; set; }
        public virtual USER USER { get; set; }
    }
}