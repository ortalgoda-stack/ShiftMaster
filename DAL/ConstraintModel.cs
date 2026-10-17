using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace ShiftMaster.DAL
{
    public class ConstraintModel
    {
        public int ConstraintID { get; set; }
        public int UserID { get; set; }
        public string CreatedByRole { get; set; } = "Employee";
        public int BlockedShiftID { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // שדות עזר להצגת שם העובד ושם המשמרת
        public string? UserName { get; set; }
        public string? BlockedShiftName { get; set; }
    }
}