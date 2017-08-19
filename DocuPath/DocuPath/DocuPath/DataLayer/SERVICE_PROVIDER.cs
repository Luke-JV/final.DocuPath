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
    
    public partial class SERVICE_PROVIDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SERVICE_PROVIDER()
        {
            this.SERVICE_REQUEST = new HashSet<SERVICE_REQUEST>();
            this.STATS_POLICE_STATION = new HashSet<STATS_POLICE_STATION>();
        }
    
        public int ServiceProviderID { get; set; }
        public int TitleID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTelNum { get; set; }
        public string CompanyFaxNum { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhysicalAddress { get; set; }
        public string CompanyPostalAddress { get; set; }
        public string RepFirstName { get; set; }
        public string RepLastName { get; set; }
        public string RepJobDescription { get; set; }
        public string RepCellNum { get; set; }
        public string RepWorkNum { get; set; }
        public string RepEmail { get; set; }
        public decimal IsDeactivated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SERVICE_REQUEST> SERVICE_REQUEST { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_POLICE_STATION> STATS_POLICE_STATION { get; set; }
        public virtual TITLE TITLE { get; set; }
    }
}