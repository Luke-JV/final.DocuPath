using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class SchedulingViewModel
    {
        public List<UserKVP> userInitials { get; set; }
    }

    public class UserKVP
    {
        public int uID { get; set; }
        public string uInitials { get; set; }
    }

    public class SlotKVP
    {
        public int SlotID { get; set; }
        public string SlotDesc { get; set; }
    }

    public class MonthlyDutyRosterViewModel
    {
        public List<UserKVP> users { get; set; }
        public List<SlotKVP> slots { get; set; }
        //public List<string> currentMonthDates { get; set; }
        //public List<string> nextMonthDates { get; set; }
        public List<DayAllocationsComments> currentMonthAllocations { get; set; }
        public List<DayAllocationsComments> nextMonthAllocations { get; set; }
        public int currentMonthDayCount { get; set; }
        public int nextMonthDayCount { get; set; }
    }

    public class DayAllocationsComments
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public int Slot1AUID { get; set; }
        public int Slot1BUID { get; set; }
        public int Slot1CUID { get; set; }
        public int Slot2AUID { get; set; }
        public int Slot2BUID { get; set; }
        public int SlotCallUID { get; set; }
        public string DayComments { get; set; }

    }        
}