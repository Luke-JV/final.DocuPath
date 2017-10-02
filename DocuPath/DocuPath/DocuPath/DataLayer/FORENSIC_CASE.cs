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

    public partial class FORENSIC_CASE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FORENSIC_CASE()
        {
            this.ABDOMEN_OBSERVATION = new HashSet<ABDOMEN_OBSERVATION>();
            this.ADDITIONAL_EVIDENCE = new HashSet<ADDITIONAL_EVIDENCE>();
            this.CASE_COD_ESTIMATION = new HashSet<CASE_COD_ESTIMATION>();
            this.CASE_STATISTICS = new HashSet<CASE_STATISTICS>();
            this.CHEST_OBSERVATION = new HashSet<CHEST_OBSERVATION>();
            this.GENERAL_OBSERVATION = new HashSet<GENERAL_OBSERVATION>();
            this.HEAD_NECK_OBSERVATION = new HashSet<HEAD_NECK_OBSERVATION>();
            this.MEDIA = new HashSet<MEDIA>();
            this.SERVICE_REQUEST = new HashSet<SERVICE_REQUEST>();
            this.SPINE_OBSERVATION = new HashSet<SPINE_OBSERVATION>();
        }

        [DisplayName("ID")]
        public int ForensicCaseID { get; set; }
        [DisplayName("Status")]
        public int StatusID { get; set; }
        [DisplayName("Autopsy Session")]
        [Required]
        public int SessionID { get; set; }
        [DisplayName("Autopsy Area")]
        [Required]
        public int AutopsyAreaID { get; set; }
        [DisplayName("Added By")]
        public int UserID { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("DR Number")]
        [Required]
        public string ForensicDRNumber { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Brief Description")]
        public string FCBriefDescription { get; set; }
        //todo
        [DisplayName("Date Added")]
        public System.DateTime DateAdded { get; set; }   
        [DisplayName("Notice Of Death")]
        [StringLength(15)]
        [Required]
        public string DHANoticeDeathID { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Acting Officer")]
        [Required]
        public string ActingOfficerNameSurname { get; set; }
        [DefaultValue(FLAG.ContactNumber)]
        [DisplayName("Contact Details")]
        [Required]
        [RegularExpression("0[0-9]{9}", ErrorMessage = "Enter a valid 10-digit number starting with 0.")]
        public string ActingOfficerContactNum { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Cause Of Death Conclusion")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "At least a preliminary cause of death estimation is required.")]
        public string CauseOfDeathConclusion { get; set; }
        //todo [DefaultValue(FLAG.Date)]
        [DisplayName("Date Closed")]
        public Nullable<System.DateTime> DateClosed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ABDOMEN_OBSERVATION> ABDOMEN_OBSERVATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADDITIONAL_EVIDENCE> ADDITIONAL_EVIDENCE { get; set; }
        public virtual AUTOPSY_AREA AUTOPSY_AREA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE_COD_ESTIMATION> CASE_COD_ESTIMATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CASE_STATISTICS> CASE_STATISTICS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHEST_OBSERVATION> CHEST_OBSERVATION { get; set; }
        public virtual STATUS STATUS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GENERAL_OBSERVATION> GENERAL_OBSERVATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HEAD_NECK_OBSERVATION> HEAD_NECK_OBSERVATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDIA> MEDIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SERVICE_REQUEST> SERVICE_REQUEST { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SPINE_OBSERVATION> SPINE_OBSERVATION { get; set; }
        public virtual SESSION SESSION { get; set; }
        public virtual USER USER { get; set; }


        public void assignFlagsAndKey(int key)
        {
            this.ForensicCaseID = key;
            
            if (DateClosed == null)
            {
                DateClosed = DateClosed.GetValueOrDefault();
            }

            if (CauseOfDeathConclusion == null)
            {
                AttributeCollection attributes = TypeDescriptor.GetProperties(this)["CauseOfDeathConclusion"].Attributes;
                DefaultValueAttribute myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
                CauseOfDeathConclusion = myAttribute.Value.ToString();
            }
        }
    }
}
