using System;
using System.Collections.Generic;
using System.Text;

namespace API.Server.Models
{
   
        public class User
        {
            public int UserID { get; set; }
            public string Username { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public int RoleID { get; set; }
            public string? Phone { get; set; }
            public int MaxShiftsPerWeek { get; set; } = 5; // מכסת מקסימום משמרות
            public int MinShiftsPerWeek { get; set; } = 2; // מינימום משמרות
            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }
    
}
