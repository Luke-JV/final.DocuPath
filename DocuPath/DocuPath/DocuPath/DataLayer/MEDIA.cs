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
    
    public partial class MEDIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEDIA()
        {
            this.CONTENT_TAG = new HashSet<CONTENT_TAG>();
        }
    
        public int MediaID { get; set; }
        public int StatusID { get; set; }
        public Nullable<int> ForensicCaseID { get; set; }
        public int MediaPurposeID { get; set; }
        public int UserID { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string MediaCaption { get; set; }
        public string MediaDescription { get; set; }
        public bool IsPubliclyAccessible { get; set; }
        public string MediaLocation { get; set; }
    
        public virtual FORENSIC_CASE FORENSIC_CASE { get; set; }
        public virtual MEDIA_PURPOSE MEDIA_PURPOSE { get; set; }
        public virtual USER USER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTENT_TAG> CONTENT_TAG { get; set; }
    }
}
