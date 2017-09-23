using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class SystemParametersViewModel
    {
        public List<ALLOCATION_STATUS> allocationStatuses { get; set; }
        public List<APPARENT_MANNER_DEATH> apparentMOD { get; set; }
        public List<AUTOPSY_AREA> autopsyAreas { get; set; }
        public List<AUTOPSY_TYPE> autopsyTypes { get; set; }
        public List<EXTERNAL_CAUSE> externalCauses { get; set; }
        public List<HOSPITAL_CLINIC> hospitalsClinics { get; set; }
        public List<INDIVIDUAL_GENDER> genders { get; set; }
        public List<INDIVIDUAL_RACE> races { get; set; }
        public List<MEDIA_PURPOSE> mediaPurposes { get; set; }
        public List<MEDICAL_TREATMENTS> medicalTreatments { get; set; }
        public List<PRIMARY_CAUSE_DEATH> primaryCOD { get; set; }
        public List<PROVINCE> provinces { get; set; }
        public List<REQUEST_TYPE> requestTypes { get; set; }
        public List<SAMPLE_INVESTIGATION> samplesInvestigations { get; set; }
        public List<SCENE_OF_INJURY> scenesOfInjury { get; set; }
        public List<SLOT> dutySlots { get; set; }
        public List<SPECIAL_CATEGORY> specialCategories { get; set; }
        public List<STATUS> statuses { get; set; }
        public List<TITLE> titles { get; set; }
        public List<NonDBStoredParameter> nonDbParams { get; set; }
    }
    public class NonDBStoredParameter
    {
        public string NonDBParamName { get; set; }
        public string NonDBParamValue { get; set; }
    }
}