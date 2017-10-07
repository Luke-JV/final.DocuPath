using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ReportingViewModel
    {
        public InsightQuery iq { get; set; }
        public List<UserFullKVP> users { get; set; }
        public List<UserFullKVP> superusers { get; set; }
        public List<ActivityKVP> activityTypes { get; set; }
        public List<ReportKVP> reports { get; set; }
        public List<TimeframeKVP> timeframes { get; set; }
    }
    public class InsightQuery
    {
        [Required]
        public int reportToGenerate { get; set; }
        [Required]
        public int reportTimeframe { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime reportDateFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime reportDateTo { get; set; }
        public int reportUsersSelector { get; set; }
        public int reportActivitiesSelector { get; set; }
        public int reportSuperuserSelector { get; set; }
    }
    public class ReportKVP
    {
        public int reportID { get; set; }
        public string reportPhrase { get; set; }
    }
    public class TimeframeKVP
    {
        public int tfId { get; set; }
        public string tfPhrase { get; set; }
        public DateTime tfStartValue { get; set; }
        public DateTime tfEndValue { get; set; }
    }
    public class UserFullKVP
    {
        public int uId { get; set; }
        public string uNameSurname { get; set; }
    }
    public class ActivityKVP
    {
        public int aId { get; set; }
        public string aDesc { get; set; }
    }
    //public class ReportTimeframeRange
    //{
    //    private DateTime dateFrom { get; set; }
    //    private DateTime dateTo { get; set; }
    //}
    //public class ReportParameter
    //{
    //    private string ParamName { get; set; }
    //    private string ParamID { get; set; }
    //}
}