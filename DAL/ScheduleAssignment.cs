using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace ShiftMaster.DAL
{
    public class ScheduleAssignment
    {
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int ShiftID { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Status { get; set; } = "Approved";

        // שדות עזר להצגת שם העובד ושם המשמרת
        public string? UserName { get; set; }
        public string? ShiftName { get; set; }
    }
}