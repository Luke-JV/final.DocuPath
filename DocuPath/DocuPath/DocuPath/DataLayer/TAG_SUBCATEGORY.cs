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
    
    public partial class TAG_SUBCATEGORY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAG_SUBCATEGORY()
        {
            this.CONTENT_TAG = new HashSet<CONTENT_TAG>();
        }
    
        [DisplayName("ID")]
        public int TagSubCategoryID { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Tag Subcategory")]
        public string TagSubCategoryName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTENT_TAG> CONTENT_TAG { get; set; }
    }
}
