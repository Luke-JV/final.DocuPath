using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ForensicCaseViewModel
    {
        public FORENSIC_CASE forensicCase { get; set; }
        public GENERAL_OBSERVATION genObservation { get; set; }
        public ABDOMEN_OBSERVATION abdObservation { get; set; }
        public CHEST_OBSERVATION chestObservation { get; set; }
        public HEAD_NECK_OBSERVATION headNeckObservation { get; set; }
        public SPINE_OBSERVATION spineObservation { get; set; }
        public List<ADDITIONAL_EVIDENCE> additionalEvidence { get; set; }
        public List<SERVICE_REQUEST> serviceRequests { get; set; }
        public List<MEDIA> media { get; set; }
        public CASE_STATISTICS stats { get; set; }
        public List<STATS_PROVINCE_EVENT> provinces { get; set; }
        public List<CASE_COD_ESTIMATION> caseCODEstimations { get; set; }
        public List<STATS_SAMPLES_INVESTIGATION> sampleInvestigations { get; set; }
        public List<STATS_TREATMENTS> medTreatments { get; set; }
        public List<STATS_INJURY_SCENE> injuryScenes { get; set; }
        public List<STATS_EXTERNAL_CAUSE> externalCause { get; set; }
        public List<STATS_SPECIAL_CATEGORY> specialCategory { get; set; }
        public List<STATS_POLICE_STATION> stations { get; set; }
    }   

    public class AddForensicCaseViewModel
    {
        public FORENSIC_CASE forensicCase { get; set; }
        public GENERAL_OBSERVATION genObservation { get; set; }
        public ABDOMEN_OBSERVATION abdObservation { get; set; }
        public CHEST_OBSERVATION chestObservation { get; set; }
        public HEAD_NECK_OBSERVATION headNeckObservation { get; set; }
        public SPINE_OBSERVATION spineObservation { get; set; }
        public List<ADDITIONAL_EVIDENCE> additionalEvidence { get; set; }
        public List<SERVICE_REQUEST> serviceRequests { get; set; }
        public List<MEDIA> media { get; set; }
        public CASE_STATISTICS stats { get; set; }
        public List<PROVINCE> provinces { get; set; }
        public List<EVENT> events { get; set; }
        public List<SAMPLE_INVESTIGATION> sampleInvestigations { get; set; }
        public List<MEDICAL_TREATMENTS> medTreatments { get; set; }
        public List<SCENE_OF_INJURY> injuryScenes { get; set; }
        public List<EXTERNAL_CAUSE> externalCause { get; set; }
        public List<SPECIAL_CATEGORY> specialCategory { get; set; }
        public List<SERVICE_PROVIDER> serviceProvider { get; set; }
        public List<HOSPITAL_CLINIC> hospitalsClinics { get; set; }
        //public List<SESSION> sessions { get; set; }
        public List<AUTOPSY_AREA> autopsyAreas { get; set; }
        public List<AUTOPSY_TYPE> autopsyTypes { get; set; }
        public List<INDIVIDUAL_RACE> races { get; set; }
        public List<INDIVIDUAL_GENDER> genders { get; set; }
        public List<PRIMARY_CAUSE_DEATH> primaryCauses { get; set; }
        public List<APPARENT_MANNER_DEATH> apparentManners { get; set; }

        public List<sessionKVP> sessionSelector { get; set; }
        public List<multiselectKVP> selectedSamplesInvestigations { get; set; }
        public List<multiselectKVP> selectedMedicalTreatments { get; set; }
        public List<multiselectKVP> selectedInjuryScenes { get; set; }
        public List<multiselectKVP> selectedExternalCauses { get; set; }
        public List<multiselectKVP> selectedSpecialCategories { get; set; }

        //public List<SPE_KVP> provinceEvents { get; set; }
        //public List<stationRolesKVP> stationRoles { get; set; }

        public CASE_COD_ESTIMATION primaryCODEst { get; set; }
        public CASE_COD_ESTIMATION secondaryCODEst { get; set; }
        public CASE_COD_ESTIMATION tertiaryCODEst { get; set; }
        public CASE_COD_ESTIMATION quaternaryCODEst { get; set; }


        [DisplayName("Description (If 'Other')")]
        public string otherSamplesInvestigationsDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherRaceDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherTreatmentFacilityDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherMedicalTreatmentsDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherPrimaryCauseDeathDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherApparentMannerDeathDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherInjurySceneDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherExternalCauseDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherSpecialCategoryDescription { get; set; }

        public int DeathProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherDeathProvinceDesc { get; set; }
        public int OccurrenceProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherOccurrenceProvinceDesc { get; set; }
        public int ReportProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherReportProvinceDesc { get; set; }
        public int ProcessingProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherProcessingProvinceDesc { get; set; }
        public int TreatmentProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherTreatmentProvinceDesc { get; set; }

        public int JurisdictionStationID { get; set; }
        public string JurisdictionStationName { get; set; }
        public int ProcessingStationID { get; set; }
        public string ProcessingStationName { get; set; }
        public int InvestigationStationID { get; set; }
        public string InvestigationStationName { get; set; }


    }

    public class CoreDataViewModel
    {
        public FORENSIC_CASE forensicCase { get; set; }
    }

    public class ObservationsViewModel
    {
        public GENERAL_OBSERVATION genObservation { get; set; }
        public ABDOMEN_OBSERVATION abdObservation { get; set; }
        public CHEST_OBSERVATION chestObservation { get; set; }
        public HEAD_NECK_OBSERVATION headNeckObservation { get; set; }
        public SPINE_OBSERVATION spineObservation { get; set; }
    }

    public class FCServiceRequestViewModel
    {
        public List<SERVICE_REQUEST> serviceRequests { get; set; }
    }

    public class CaseMediaViewModel
    {
        public List<MEDIA> media { get; set; }
    }

    public class AdditionalEvidenceViewModel
    {
        public List<ADDITIONAL_EVIDENCE> additionalEvidence { get; set; }
    }

    public class CODViewModel
    {
        public CASE_COD_ESTIMATION primaryCODEst { get; set; }
        public CASE_COD_ESTIMATION secondaryCODEst { get; set; }
        public CASE_COD_ESTIMATION tertiaryCODEst { get; set; }
        public CASE_COD_ESTIMATION quaternaryCODEst { get; set; }

    }

    public class StatisticsViewModel
    {
        public CASE_STATISTICS stats { get; set; }
        public List<PROVINCE> provinces { get; set; }
        public List<EVENT> events { get; set; }
        public List<SAMPLE_INVESTIGATION> sampleInvestigations { get; set; }
        public List<MEDICAL_TREATMENTS> medTreatments { get; set; }
        public List<SCENE_OF_INJURY> injuryScenes { get; set; }
        public List<EXTERNAL_CAUSE> externalCause { get; set; }
        public List<SPECIAL_CATEGORY> specialCategory { get; set; }
        public List<SERVICE_PROVIDER> serviceProvider { get; set; }
        public List<HOSPITAL_CLINIC> hospitalsClinics { get; set; }

        public List<AUTOPSY_AREA> autopsyAreas { get; set; }
        public List<AUTOPSY_TYPE> autopsyTypes { get; set; }
        public List<INDIVIDUAL_RACE> races { get; set; }
        public List<INDIVIDUAL_GENDER> genders { get; set; }
        public List<PRIMARY_CAUSE_DEATH> primaryCauses { get; set; }
        public List<APPARENT_MANNER_DEATH> apparentManners { get; set; }

        public List<sessionKVP> sessionSelector { get; set; }
        public List<multiselectKVP> selectedSamplesInvestigations { get; set; }
        public List<multiselectKVP> selectedMedicalTreatments { get; set; }
        public List<multiselectKVP> selectedInjuryScenes { get; set; }
        public List<multiselectKVP> selectedExternalCauses { get; set; }
        public List<multiselectKVP> selectedSpecialCategories { get; set; }

        [DisplayName("Description (If 'Other')")]
        public string otherSamplesInvestigationsDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherRaceDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherTreatmentFacilityDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherMedicalTreatmentsDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherPrimaryCauseDeathDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherApparentMannerDeathDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherInjurySceneDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherExternalCauseDescription { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherSpecialCategoryDescription { get; set; }

        public int DeathProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherDeathProvinceDesc { get; set; }
        public int OccurrenceProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherOccurrenceProvinceDesc { get; set; }
        public int ReportProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherReportProvinceDesc { get; set; }
        public int ProcessingProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherProcessingProvinceDesc { get; set; }
        public int TreatmentProvinceId { get; set; }
        [DisplayName("Description (If 'Other')")]
        public string otherTreatmentProvinceDesc { get; set; }

        public int JurisdictionStationID { get; set; }
        public string JurisdictionStationName { get; set; }
        public int ProcessingStationID { get; set; }
        public string ProcessingStationName { get; set; }
        public int InvestigationStationID { get; set; }
        public string InvestigationStationName { get; set; }
    }

    public class sessionKVP
    {
        public int sessionID { get; set; }
        public string sessionDesc { get; set; }
    }

    public class multiselectKVP
    {
        public string valueName { get; set; }
        public int valueID { get; set; }
        public bool isSelected { get; set; }
    }

    //public class SPE_KVP
    //{
    //    public int provinceID { get; set; }
    //    public int eventID { get; set; }
    //}

    //public class stationRolesKVP
    //{
    //    public int providerID { get; set; }
    //    public int roleID { get; set; }
    //}
}