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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class APPARENT_MANNER_DEATH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APPARENT_MANNER_DEATH()
        {
            this.CASE_STATISTICS = new HashSet<CASE_STATISTICS>();
        }

        [DisplayName("ID")]
        public int ApparentMannerID { get; set; }
        [DisplayName("Apparent Manner of Death Description")]
        public string ApparentMannerDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE_STATISTICS> CASE_STATISTICS { get; set; }
    }
}
