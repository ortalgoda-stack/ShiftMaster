using System;
using System.Collections.Generic;
using System.Text;

namespace API.Server.Models
{
    public class Constraint
    {
        public int ConstraintID { get; set; }
        public int UserID { get; set; }
        public string CreatedByRole { get; set; } = string.Empty; // Employee / Manager
        public int BlockedShiftID { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
