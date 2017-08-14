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
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    
    public partial class USER : IUser<int>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            this.ACTIVE_LOGIN = new HashSet<ACTIVE_LOGIN>();
            this.AUDIT_LOG = new HashSet<AUDIT_LOG>();
            this.EXTERNAL_REVIEW_CASE = new HashSet<EXTERNAL_REVIEW_CASE>();
            this.FORENSIC_CASE = new HashSet<FORENSIC_CASE>();
            this.FPS_AUTOPSY_SCHEDULE = new HashSet<FPS_AUTOPSY_SCHEDULE>();
            this.LEGACY_CASE = new HashSet<LEGACY_CASE>();
            this.MEDIA = new HashSet<MEDIA>();
            this.NOTIFICATION = new HashSet<NOTIFICATION>();
            this.SESSION_USER = new HashSet<SESSION_USER>();
            this.TOKEN_LOG = new HashSet<TOKEN_LOG>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public int TitleID { get; set; }
        public Nullable<int> UserLoginID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayInitials { get; set; }
        public string QualificationDescription { get; set; }
        public string HPCSARegNumber { get; set; }
        public string NationalID { get; set; }
        public string AcademicID { get; set; }
        public string CellNum { get; set; }
        public string TelNum { get; set; }
        public string WorkNum { get; set; }
        public string PersonalEmail { get; set; }
        public string AcademicEmail { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalAddress { get; set; }
        public decimal IsDeactivated { get; set; }
        public Nullable<decimal> DarkUIPref { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACTIVE_LOGIN> ACTIVE_LOGIN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUDIT_LOG> AUDIT_LOG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EXTERNAL_REVIEW_CASE> EXTERNAL_REVIEW_CASE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORENSIC_CASE> FORENSIC_CASE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FPS_AUTOPSY_SCHEDULE> FPS_AUTOPSY_SCHEDULE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LEGACY_CASE> LEGACY_CASE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDIA> MEDIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION> NOTIFICATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SESSION_USER> SESSION_USER { get; set; }
        public virtual TITLE TITLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TOKEN_LOG> TOKEN_LOG { get; set; }
        public virtual USER_LOGIN USER_LOGIN { get; set; }
    }
}
