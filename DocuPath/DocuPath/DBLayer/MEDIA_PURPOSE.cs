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
    
    public partial class MEDIA_PURPOSE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEDIA_PURPOSE()
        {
            this.MEDIA = new HashSet<Medium>();
        }
    
        public int MediaPurposeID { get; set; }
        public string MediaPurposeValue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medium> MEDIA { get; set; }
    }
}
