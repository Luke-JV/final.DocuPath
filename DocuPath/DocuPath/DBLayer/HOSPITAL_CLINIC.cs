//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocuPath.DBLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class HOSPITAL_CLINIC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOSPITAL_CLINIC()
        {
            this.CASE_STATISTICS = new HashSet<CASE_STATISTICS>();
        }
    
        public int HospitalClinicID { get; set; }
        public string HospitalClinicName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE_STATISTICS> CASE_STATISTICS { get; set; }
    }
}
