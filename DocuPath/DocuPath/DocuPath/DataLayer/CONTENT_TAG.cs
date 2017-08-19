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
    
    public partial class CONTENT_TAG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONTENT_TAG()
        {
            this.CASE_COD_ESTIMATION = new HashSet<CASE_COD_ESTIMATION>();
            this.MEDIA = new HashSet<MEDIA>();
        }
    
        public int ContentTagID { get; set; }
        public int TagCategoryID { get; set; }
        public int TagSubCategoryID { get; set; }
        public int TagConditionID { get; set; }
        public string ContentTagCode { get; set; }
        public string ContentTagText { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE_COD_ESTIMATION> CASE_COD_ESTIMATION { get; set; }
        public virtual TAG_CONDITION TAG_CONDITION { get; set; }
        public virtual TAG_SUBCATEGORY TAG_SUBCATEGORY { get; set; }
        public virtual TAG_CATEGORY TAG_CATEGORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDIA> MEDIA { get; set; }
    }
}