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
    
    public partial class ACCESS_LEVEL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACCESS_LEVEL()
        {
            this.LEVEL_AREA = new HashSet<LEVEL_AREA>();
            this.TOKEN_LOG = new HashSet<TOKEN_LOG>();
            this.USER_LOGIN = new HashSet<USER_LOGIN>();
        }
    
        public int AccessLevelID { get; set; }
        public string LevelName { get; set; }
        public decimal IsDeactivated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LEVEL_AREA> LEVEL_AREA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TOKEN_LOG> TOKEN_LOG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_LOGIN> USER_LOGIN { get; set; }
    }
}