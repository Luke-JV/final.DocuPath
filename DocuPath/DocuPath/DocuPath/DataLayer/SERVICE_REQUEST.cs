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

    public partial class SERVICE_REQUEST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SERVICE_REQUEST()
        {
            this.SPECIMEN = new HashSet<SPECIMEN>();
        }

        [DisplayName("ID")]
        public int ServiceRequestID { get; set; }
        [DisplayName("Service Provider")]
        public int ServiceProviderID { get; set; }
        [DisplayName("Request Type")]
        public int RequestTypeID { get; set; }
        [DisplayName("Related Case")]
        public Nullable<int> ForensicCaseID { get; set; }
        //todo [DefaultValue(FLAG.Text)]
        [DisplayName("Date Added")]
        public System.DateTime DateAdded { get; set; }
        [DefaultValue(FLAG.Text)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Request Note")]
        public string RequestNote { get; set; }
        [DisplayName("Status")]
        public bool IsCancelled { get; set; }
    
        public virtual FORENSIC_CASE FORENSIC_CASE { get; set; }
        public virtual REQUEST_TYPE REQUEST_TYPE { get; set; }
        public virtual SERVICE_PROVIDER SERVICE_PROVIDER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SPECIMEN> SPECIMEN { get; set; }
    }
}
