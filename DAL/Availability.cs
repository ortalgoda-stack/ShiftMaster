using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace ShiftMaster.DAL
{
    public class Availability
    {
        public int AvailabilityID { get; set; }
        public int UserID { get; set; }
        public int ShiftID { get; set; }
        public DateTime ShiftDate { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // שדות עזר להצגת שם העובד ושם המשמרת
        public string? UserName { get; set; }
        public string? ShiftName { get; set; }
    }
}