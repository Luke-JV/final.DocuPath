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

    public partial class PROVINCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROVINCE()
        {
            this.STATS_PROVINCE_EVENT = new HashSet<STATS_PROVINCE_EVENT>();
        }
    
        public int ProvinceID { get; set; }
        [DefaultValue(FLAG.Text)]
        public string ProvinceName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_PROVINCE_EVENT> STATS_PROVINCE_EVENT { get; set; }
    }
}
