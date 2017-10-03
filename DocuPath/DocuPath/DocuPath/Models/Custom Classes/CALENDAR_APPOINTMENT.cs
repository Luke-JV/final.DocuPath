using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.Custom_Classes
{
    public class CALENDAR_APPOINTMENT
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string _lass { get; set; }
        public long start { get; set; }
        public long end { get; set; }
    }
}