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

    public partial class COD_PROMINENCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COD_PROMINENCE()
        {
            this.CASE_COD_ESTIMATION = new HashSet<CASE_COD_ESTIMATION>();
        }
    
        public int ProminenceID { get; set; }
        [DefaultValue(FLAG.Text)]
        public string ProminenceValue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE_COD_ESTIMATION> CASE_COD_ESTIMATION { get; set; }
    }
}
