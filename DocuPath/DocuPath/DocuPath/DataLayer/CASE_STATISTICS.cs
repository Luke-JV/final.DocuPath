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

    public partial class CASE_STATISTICS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CASE_STATISTICS()
        {
            this.STATS_EXTERNAL_CAUSE = new HashSet<STATS_EXTERNAL_CAUSE>();
            this.STATS_INJURY_SCENE = new HashSet<STATS_INJURY_SCENE>();
            this.STATS_POLICE_STATION = new HashSet<STATS_POLICE_STATION>();
            this.STATS_PROVINCE_EVENT = new HashSet<STATS_PROVINCE_EVENT>();
            this.STATS_SAMPLES_INVESTIGATION = new HashSet<STATS_SAMPLES_INVESTIGATION>();
            this.STATS_SPECIAL_CATEGORY = new HashSet<STATS_SPECIAL_CATEGORY>();
            this.STATS_TREATMENTS = new HashSet<STATS_TREATMENTS>();
        }

        [DisplayName("ID")]
        public int CaseStatsID { get; set; }
        [DisplayName("Forensic Case ID")]
        public int ForensicCaseID { get; set; }
        [DisplayName("Apparent Manner of Death")]
        public int ApparentMannerID { get; set; }
        [DisplayName("Autopsy Type")]
        public int AutopsyTypeID { get; set; }
        [DisplayName("Hospital/Clinic")]
        public Nullable<int> HospitalClinicID { get; set; }
        [DisplayName("Individual Gender")]
        public int IndividualGenderID { get; set; }
        [DisplayName("Individual Race")]
        public int IndividualRaceID { get; set; }
        [DisplayName("Primary Cause Of Death")]
        public Nullable<int> PrimaryCauseID { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Surname of Body Collector")]
        public string BodyCollectorSurname { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Surname of Eviscerator")]
        public string EvisceratorSurname { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Surname of Organ Dissector")]
        public string OrganDissectorSurname { get; set; }
        //TODO: DATE DEFAULT
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Estimated Date of Injury")]
        public Nullable<System.DateTime> EstimatedInjuryDate { get; set; }
        //TODO: DATE DEFAULT
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Date of Discovery")]
        public Nullable<System.DateTime> DiscoveryDate { get; set; }
        //TODO: DATE DEFAULT
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Estimated Date of Death")]
        public Nullable<System.DateTime> EstimatedDeathDate { get; set; }
        [DefaultValue(FLAG.Integer)]
        [DisplayName("Individual Estimated Age")]
        public string IndividualEstimatedAge { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Medical History")]
        [DataType(DataType.MultilineText)]
        public string MedicalHistory { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Medical Treatment Duration")]
        public string TreatmentDuration { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string OtherPrimaryCauseDescription { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string OtherApparentMannerDescription { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string AutopsyTypeOtherDescription { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string HospitalClinicOtherDescription { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string IndividualGenderOtherDescription { get; set; }
        [DefaultValue(FLAG.Text)]
        [DisplayName("Description (If 'Other')")]
        public string IndividualRaceOtherDescription { get; set; }
    
        public virtual APPARENT_MANNER_DEATH APPARENT_MANNER_DEATH { get; set; }
        public virtual AUTOPSY_TYPE AUTOPSY_TYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_EXTERNAL_CAUSE> STATS_EXTERNAL_CAUSE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_INJURY_SCENE> STATS_INJURY_SCENE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_POLICE_STATION> STATS_POLICE_STATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_PROVINCE_EVENT> STATS_PROVINCE_EVENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_SAMPLES_INVESTIGATION> STATS_SAMPLES_INVESTIGATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_SPECIAL_CATEGORY> STATS_SPECIAL_CATEGORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STATS_TREATMENTS> STATS_TREATMENTS { get; set; }
        public virtual FORENSIC_CASE FORENSIC_CASE { get; set; }
        public virtual HOSPITAL_CLINIC HOSPITAL_CLINIC { get; set; }
        public virtual INDIVIDUAL_GENDER INDIVIDUAL_GENDER { get; set; }
        public virtual INDIVIDUAL_RACE INDIVIDUAL_RACE { get; set; }
        public virtual PRIMARY_CAUSE_DEATH PRIMARY_CAUSE_DEATH { get; set; }
    }
}
