using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.Custom_Classes
{
    public class METRIC
    {
        // Members:
        private int mMetricGroupID;
        private bool mIsMain;
        private string mIconClass;
        private string mMetricName;
        private string mMetricValue;
        private string mMetricUnit;
        private string mMetricSummary;
        private string mMetricDescription;
        private System.DateTime mMetricLastRefreshed;

        // Properties:
        public int MetricGroupID { get { return mMetricGroupID; } set { mMetricGroupID = value; } }
        public bool IsMain { get { return mIsMain; } set { mIsMain = value; } }
        public string IconClass { get { return mIconClass; } set { mIconClass = value; } }
        public string MetricName { get { return mMetricName; } set { mMetricName = value; } }
        public string MetricValue { get { return mMetricValue; } set { mMetricValue = value; } }
        public string MetricUnit { get { return mMetricUnit; } set { mMetricUnit = value; } }
        public string MetricSummary { get { return mMetricSummary; } set { mMetricSummary = value; } }
        public string MetricDescription { get { return mMetricDescription; } set { mMetricDescription = value; } }
        public Nullable<System.DateTime> MetricLastRefreshed { get { return mMetricLastRefreshed; } set { mMetricLastRefreshed = Convert.ToDateTime(value); } }

        // Default Constructor:
        public METRIC()
        {
            MetricGroupID = 0;
            IsMain = true;
            IconClass = "mdl2icon mdl2-info metric-mdl2icon";
            MetricName = "";
            MetricValue = "";
            MetricUnit = "";
            MetricSummary = "";
            MetricDescription = "";
            mMetricLastRefreshed = System.DateTime.Now;
        }

        // Parameterised Constructor:
        public METRIC(int inGroupID, bool inIsMain, string inIconClass, string inName, string inValue, string inUnit, string inSummary, string inDescription, DateTime inRefreshStamp)
        {
            MetricGroupID = inGroupID;
            IsMain = inIsMain;
            IconClass = inIconClass;
            MetricName = inName;
            MetricValue = inValue;
            MetricUnit = inUnit;
            MetricSummary = inSummary;
            MetricDescription = inDescription;
            mMetricLastRefreshed = inRefreshStamp;
        }
    }
}