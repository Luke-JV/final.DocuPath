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

    public partial class SPECIMEN
    {
        [DisplayName("ID")]
        public int SpecimenID { get; set; }
        [DisplayName("Service Request")]
        public int ServiceRequestID { get; set; }
        [DisplayName("External Report")]
        public Nullable<int> ExternalReportID { get; set; }
        [DisplayName("Service Provider")]
        public int ServiceProviderID { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("Investigation Required")]
        [DataType(DataType.MultilineText)]
        public string InvestigationRequired { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("Chain Of Custody")]
        public string DisposalMechanism { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("Nature/Description")]
        public string SpecimenNature { get; set; }
        [DefaultValue(FLAG.Alphanumeric)]
        [DisplayName("Serial/Seal Number")]
        public string SpecimenSerialNumber { get; set; }
    
        public virtual EXTERNAL_REPORT EXTERNAL_REPORT { get; set; }
        public virtual SERVICE_REQUEST SERVICE_REQUEST { get; set; }
    }
}
