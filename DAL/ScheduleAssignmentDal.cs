using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShiftMaster.DAL
{
    public class ScheduleAssignmentDal
    {
        private readonly string _connectionString;

        public ScheduleAssignmentDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. שליפת כל השיבוצים
        public List<ScheduleAssignment> GetAllAssignments()
        {
            List<ScheduleAssignment> list = new List<ScheduleAssignment>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT sa.AssignmentID, sa.UserID, sa.ShiftID, sa.ShiftDate, sa.Status,
                                        u.FullName AS UserName, s.ShiftName
                                 FROM scheduleassignments sa
                                 LEFT JOIN users u ON sa.UserID = u.UserID
                                 LEFT JOIN shifts s ON sa.ShiftID = s.ShiftID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ScheduleAssignment
                            {
                                AssignmentID = Convert.ToInt32(reader["AssignmentID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                ShiftID = Convert.ToInt32(reader["ShiftID"]),
                                AssignmentDate = reader["ShiftDate"] != DBNull.Value ? Convert.ToDateTime(reader["ShiftDate"]) : DateTime.Now,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString()! : "Approved",
                                UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : "",
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : ""
                            });
                        }
                    }
                }
            }
            return list;
        }

        // 2. הוספת שיבוץ חדש
        public bool AddAssignment(ScheduleAssignment assignment)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO scheduleassignments (UserID, ShiftID, ShiftDate, Status) 
                                 VALUES (@UserID, @ShiftID, @ShiftDate, @Status)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", assignment.UserID);
                    cmd.Parameters.AddWithValue("@ShiftID", assignment.ShiftID);
                    cmd.Parameters.AddWithValue("@ShiftDate", assignment.AssignmentDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Status", assignment.Status ?? "Approved");

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}