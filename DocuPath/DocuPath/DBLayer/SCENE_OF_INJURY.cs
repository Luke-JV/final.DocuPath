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
    
    public partial class SCENE_OF_INJURY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SCENE_OF_INJURY()
        {
            this.STATS_INJURY_SCENE = new HashSet<STATS_INJURY_SCENE>();
        }
    
        public int InjurySceneID { get; set; }
        public string InjurySceneDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_INJURY_SCENE> STATS_INJURY_SCENE { get; set; }
    }
}
