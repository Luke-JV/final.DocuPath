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
    
    public partial class ACCESS_AREA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACCESS_AREA()
        {
            this.LEVEL_AREA = new HashSet<LEVEL_AREA>();
        }
    
        public int AccessAreaID { get; set; }
        public int FunctionGroupID { get; set; }
        public string AccessAreaDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LEVEL_AREA> LEVEL_AREA { get; set; }
        public virtual FUNCTION_GROUP FUNCTION_GROUP { get; set; }
    }
}
