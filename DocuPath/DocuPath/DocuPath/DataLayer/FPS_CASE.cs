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
    
    public partial class FPS_CASE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FPS_CASE()
        {
            this.FPS_AUTOPSY_SCHEDULE = new HashSet<FPS_AUTOPSY_SCHEDULE>();
        }
    
        public int FPSCaseID { get; set; }
        public int FPSAssistantID { get; set; }
        public string FPSDRNum { get; set; }
        public string FPSDeceasedName { get; set; }
        public string FPSRace { get; set; }
        public string FPSGender { get; set; }
        public int FPSAge { get; set; }
        public string FPSCircumstanceCause { get; set; }
        public string FPSDoctor { get; set; }
        public string FPSComment { get; set; }
    
        public virtual FPS_ASSISTANT FPS_ASSISTANT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FPS_AUTOPSY_SCHEDULE> FPS_AUTOPSY_SCHEDULE { get; set; }
    }
}