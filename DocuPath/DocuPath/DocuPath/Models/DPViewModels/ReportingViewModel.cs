using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class ReportingViewModel
    {
        public InsightQuery query { get; set; }
    }
    public class InsightQuery
    {
        public int reportToGenerate { get; set; }
        public int timeframeToTarget { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public List<InsightUser> allUsers { get; set; }
        public List<ActivityType> allActivityTypes { get; set; }
        public int selectedSuperuser { get; set; }
        public int selectedUser { get; set; }
        public int selectedActivityType { get; set; }
    }

    public class InsightUser
    {
        public int UserID { get; set; }
        public string UserNameSurname { get; set; }
    }
    public class ActivityType
    {
        public int ActivityTypeID { get; set; }
        public string ActivityTypeDesc { get; set; }
    }
    //    private ReportTarget reportToGenerate { get; set; }
    //    private ReportTimeframe timeframeToTarget { get; set; }
    //    private ReportTimeframeRange timeframeRangeToTarget { get; set; }
    //    private List<ReportParameter> paramList { get; set; }
    //}
    //public class ReportTarget
    //{
    //    private int ReportID { get; set; }
    //    private string ReportName { get; set; }
    //}
    //public class ReportTimeframe
    //{
    //    private int TimeframeID { get; set; }
    //    private string TimeframeDesc { get; set; }
    //}
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

    public class VisionQuery
    {

    }
}