using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
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
}