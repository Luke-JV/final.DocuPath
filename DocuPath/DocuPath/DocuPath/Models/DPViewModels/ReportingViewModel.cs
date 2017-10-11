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
        public Nullable<int> reportTimeframe { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]        
        public Nullable<DateTime> reportDateFrom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> reportDateTo { get; set; }
        public Nullable<int> reportUsersSelector { get; set; }
        public Nullable<int> reportActivitiesSelector { get; set; }
        public Nullable<int> reportSuperuserSelector { get; set; }
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

    public class InsightCODDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<FreqKVP> dsCOD { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class FreqKVP
    {
        public int FreqID { get; set; }
        public string FreqDesc { get; set; }
        public int FreqCount { get; set; }
        public string FreqLastClosedDate { get; set; }
    }

    public class InsightMODDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<FreqKVP> dsMOD { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class InsightSOIDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<FreqKVP> dsSOI { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class InsightOSRDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<OSRKVP> dsOSR { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class OSRKVP
    {
        public int SRid { get; set; }
        public string SPCompanyName { get; set; }
        public string SRAddedBy { get; set; }
        public string DRNum { get; set; }
        public string DateAdded { get; set; }
        public int DaysOpen { get; set; }
    }

    public class InsightNewMediaDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<MEDIA> dsNewMedia { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class InsightSUODataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<AUDIT_LOG> dsSUO { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class InsightCaseDurationDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<FORENSIC_CASE> dsCaseDuration { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
    }

    public class InsightUARDataset
    {
        public string dsTimeframeToString { get; set; }
        public string dsGeneratedBy { get; set; }
        public int dsNumEntries { get; set; }
        public string dsSortedBy { get; set; }
        public string dsGroupedBy { get; set; }
        public List<AUDIT_LOG> dsUAR { get; set; }
        public string dsChartData { get; set; }
        public string dsChartLabels { get; set; }
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