﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocuPath.DBLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DocuPathEntities : DbContext
    {
        public DocuPathEntities()
            : base("name=DocuPathEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ABDOMEN_OBSERVATION> ABDOMEN_OBSERVATION { get; set; }
        public virtual DbSet<ACCESS_AREA> ACCESS_AREA { get; set; }
        public virtual DbSet<ACCESS_LEVEL> ACCESS_LEVEL { get; set; }
        public virtual DbSet<ACTIVE_LOGIN> ACTIVE_LOGIN { get; set; }
        public virtual DbSet<ADDITIONAL_EVIDENCE> ADDITIONAL_EVIDENCE { get; set; }
        public virtual DbSet<ALLOCATION_STATUS> ALLOCATION_STATUS { get; set; }
        public virtual DbSet<APPARENT_MANNER_DEATH> APPARENT_MANNER_DEATH { get; set; }
        public virtual DbSet<AUDIT_LOG> AUDIT_LOG { get; set; }
        public virtual DbSet<AUDIT_TX_TYPE> AUDIT_TX_TYPE { get; set; }
        public virtual DbSet<AUTOPSY_AREA> AUTOPSY_AREA { get; set; }
        public virtual DbSet<AUTOPSY_TYPE> AUTOPSY_TYPE { get; set; }
        public virtual DbSet<CASE_COD_ESTIMATION> CASE_COD_ESTIMATION { get; set; }
        public virtual DbSet<CASE_STATISTICS> CASE_STATISTICS { get; set; }
        public virtual DbSet<CHEST_OBSERVATION> CHEST_OBSERVATION { get; set; }
        public virtual DbSet<COD_PROMINENCE> COD_PROMINENCE { get; set; }
        public virtual DbSet<CONTENT_TAG> CONTENT_TAG { get; set; }
        public virtual DbSet<EVENT> EVENTs { get; set; }
        public virtual DbSet<EXTERNAL_CAUSE> EXTERNAL_CAUSE { get; set; }
        public virtual DbSet<EXTERNAL_REPORT> EXTERNAL_REPORT { get; set; }
        public virtual DbSet<EXTERNAL_REVIEW_CASE> EXTERNAL_REVIEW_CASE { get; set; }
        public virtual DbSet<FORENSIC_CASE> FORENSIC_CASE { get; set; }
        public virtual DbSet<FPS_ASSISTANT> FPS_ASSISTANT { get; set; }
        public virtual DbSet<FPS_AUTOPSY_SCHEDULE> FPS_AUTOPSY_SCHEDULE { get; set; }
        public virtual DbSet<FPS_CASE> FPS_CASE { get; set; }
        public virtual DbSet<FPS_DOCTOR> FPS_DOCTOR { get; set; }
        public virtual DbSet<FPS_SCHEDULE_ASSISTANT> FPS_SCHEDULE_ASSISTANT { get; set; }
        public virtual DbSet<FPS_SCHEDULE_DOCTOR> FPS_SCHEDULE_DOCTOR { get; set; }
        public virtual DbSet<FPS_STAFF_RANK> FPS_STAFF_RANK { get; set; }
        public virtual DbSet<FUNCTION_GROUP> FUNCTION_GROUP { get; set; }
        public virtual DbSet<GENERAL_OBSERVATION> GENERAL_OBSERVATION { get; set; }
        public virtual DbSet<HEAD_NECK_OBSERVATION> HEAD_NECK_OBSERVATION { get; set; }
        public virtual DbSet<HOSPITAL_CLINIC> HOSPITAL_CLINIC { get; set; }
        public virtual DbSet<INDIVIDUAL_GENDER> INDIVIDUAL_GENDER { get; set; }
        public virtual DbSet<INDIVIDUAL_RACE> INDIVIDUAL_RACE { get; set; }
        public virtual DbSet<LEGACY_CASE> LEGACY_CASE { get; set; }
        public virtual DbSet<LEGACY_DOCUMENT> LEGACY_DOCUMENT { get; set; }
        public virtual DbSet<LEVEL_AREA> LEVEL_AREA { get; set; }
        public virtual DbSet<MDR_DAY_COMMENT> MDR_DAY_COMMENT { get; set; }
        public virtual DbSet<Medium> MEDIA { get; set; }
        public virtual DbSet<MEDIA_PURPOSE> MEDIA_PURPOSE { get; set; }
        public virtual DbSet<MEDICAL_TREATMENTS> MEDICAL_TREATMENTS { get; set; }
        public virtual DbSet<NOTIFICATION> NOTIFICATIONs { get; set; }
        public virtual DbSet<NOTIFICATION_TYPE> NOTIFICATION_TYPE { get; set; }
        public virtual DbSet<PRIMARY_CAUSE_DEATH> PRIMARY_CAUSE_DEATH { get; set; }
        public virtual DbSet<PROVINCE> PROVINCEs { get; set; }
        public virtual DbSet<REQUEST_TYPE> REQUEST_TYPE { get; set; }
        public virtual DbSet<SAMPLE_INVESTIGATION> SAMPLE_INVESTIGATION { get; set; }
        public virtual DbSet<SCENE_OF_INJURY> SCENE_OF_INJURY { get; set; }
        public virtual DbSet<SERVICE_PROVIDER> SERVICE_PROVIDER { get; set; }
        public virtual DbSet<SERVICE_REQUEST> SERVICE_REQUEST { get; set; }
        public virtual DbSet<SESSION> SESSIONs { get; set; }
        public virtual DbSet<SESSION_USER> SESSION_USER { get; set; }
        public virtual DbSet<SLOT> SLOTs { get; set; }
        public virtual DbSet<SPECIAL_CATEGORY> SPECIAL_CATEGORY { get; set; }
        public virtual DbSet<SPECIMan> SPECIMEN { get; set; }
        public virtual DbSet<SPINE_OBSERVATION> SPINE_OBSERVATION { get; set; }
        public virtual DbSet<STATION_ROLE> STATION_ROLE { get; set; }
        public virtual DbSet<STATS_EXTERNAL_CAUSE> STATS_EXTERNAL_CAUSE { get; set; }
        public virtual DbSet<STATS_INJURY_SCENE> STATS_INJURY_SCENE { get; set; }
        public virtual DbSet<STATS_POLICE_STATION> STATS_POLICE_STATION { get; set; }
        public virtual DbSet<STATS_PROVINCE_EVENT> STATS_PROVINCE_EVENT { get; set; }
        public virtual DbSet<STATS_SAMPLES_INVESTIGATION> STATS_SAMPLES_INVESTIGATION { get; set; }
        public virtual DbSet<STATS_SPECIAL_CATEGORY> STATS_SPECIAL_CATEGORY { get; set; }
        public virtual DbSet<STATS_TREATMENTS> STATS_TREATMENTS { get; set; }
        public virtual DbSet<STATUS> STATUS { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SYSTEM_FLAG> SYSTEM_FLAG { get; set; }
        public virtual DbSet<TAG_CATEGORY> TAG_CATEGORY { get; set; }
        public virtual DbSet<TAG_CONDITION> TAG_CONDITION { get; set; }
        public virtual DbSet<TAG_SUBCATEGORY> TAG_SUBCATEGORY { get; set; }
        public virtual DbSet<TITLE> TITLEs { get; set; }
        public virtual DbSet<TOKEN_LOG> TOKEN_LOG { get; set; }
        public virtual DbSet<USER> USERs { get; set; }
        public virtual DbSet<USER_LOGIN> USER_LOGIN { get; set; }
    }
}
