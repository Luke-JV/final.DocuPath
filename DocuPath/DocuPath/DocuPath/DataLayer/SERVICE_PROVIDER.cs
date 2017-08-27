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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class SERVICE_PROVIDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SERVICE_PROVIDER()
        {
            this.SERVICE_REQUEST = new HashSet<SERVICE_REQUEST>();
            this.STATS_POLICE_STATION = new HashSet<STATS_POLICE_STATION>();
        }

        [DisplayName("ID")]
        public int ServiceProviderID { get; set; }
        [DisplayName("Title")]
        public int TitleID { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Telephone Number")]
        public string CompanyTelNum { get; set; }
        [DisplayName("Fax Number")]
        public string CompanyFaxNum { get; set; }
        [DisplayName("Email Address")]
        public string CompanyEmail { get; set; }
        [DisplayName("Physical Address")]
        public string CompanyPhysicalAddress { get; set; }
        [DisplayName("Postal Address")]
        public string CompanyPostalAddress { get; set; }
        [DisplayName("First Name")]
        public string RepFirstName { get; set; }
        [DisplayName("Last Name")]
        public string RepLastName { get; set; }
        [DisplayName("Job Description")]
        public string RepJobDescription { get; set; }
        [DisplayName("Cellphone Number")]
        public string RepCellNum { get; set; }
        [DisplayName("Work Number")]
        public string RepWorkNum { get; set; }
        [DisplayName("Email Address")]
        public string RepEmail { get; set; }
        [DisplayName("Status")]
        public bool IsDeactivated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SERVICE_REQUEST> SERVICE_REQUEST { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_POLICE_STATION> STATS_POLICE_STATION { get; set; }
        public virtual TITLE TITLE { get; set; }
    }
}
