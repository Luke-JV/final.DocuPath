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

    public partial class ADDITIONAL_EVIDENCE
    {
        [DisplayName("ID")]
        public int AdditionalEvidenceID { get; set; }
        [DisplayName("Forensic Case ID")]
        public int ForensicCaseID { get; set; }
        [DefaultValue(FLAG.Text)]
        [DataType(DataType.MultilineText)]
        [StringLength(235)]
        [DisplayName("Item Description")]
        [Required]
        public string EvidenceDescription { get; set; }
        [Required]
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("Serial/Seal Number")]
        public string EvidenceSerialNumber { get; set; }
        [Required]
        [DefaultValue(FLAG.Text)]
        [DisplayName("Contact Person")]
        public string ContactPersonNameSurname { get; set; }
        [Required]
        [DefaultValue(FLAG.ContactNumber)]
        [RegularExpression("0[0-9]{9}", ErrorMessage = "Enter a valid 10-digit number starting with 0")]
        [DisplayName("Contact Number")]
        public string ContactPersonContactNum { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("Item Location")]
        public string AdditionalEvidenceLocation { get; set; }
    
        public virtual FORENSIC_CASE FORENSIC_CASE { get; set; }
    }
}
