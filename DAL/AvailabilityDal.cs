using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShiftMaster.DAL
{
    public class AvailabilityDal
    {
        private readonly string _connectionString;

        public AvailabilityDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. שליפת כל דיווחי הזמינות
        public List<Availability> GetAllAvailabilities()
        {
            List<Availability> list = new List<Availability>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT a.AvailabilityID, a.UserID, a.ShiftID, a.ShiftDate, a.IsAvailable, a.SubmittedAt,
                                        u.FullName AS UserName, s.ShiftName
                                 FROM availability a
                                 LEFT JOIN users u ON a.UserID = u.UserID
                                 LEFT JOIN shifts s ON a.ShiftID = s.ShiftID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Availability
                            {
                                AvailabilityID = Convert.ToInt32(reader["AvailabilityID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                ShiftID = Convert.ToInt32(reader["ShiftID"]),
                                ShiftDate = Convert.ToDateTime(reader["ShiftDate"]),
                                IsAvailable = reader["IsAvailable"] != DBNull.Value && Convert.ToBoolean(reader["IsAvailable"]),
                                SubmittedAt = reader["SubmittedAt"] != DBNull.Value ? Convert.ToDateTime(reader["SubmittedAt"]) : DateTime.Now,
                                UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : "",
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : ""
                            });
                        }
                    }
                }
            }
            return list;
        }

        // 2. הוספת דיווח זמינות חדש
        public bool AddAvailability(Availability availability)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO availability (UserID, ShiftID, ShiftDate, IsAvailable) 
                                 VALUES (@UserID, @ShiftID, @ShiftDate, @IsAvailable)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", availability.UserID);
                    cmd.Parameters.AddWithValue("@ShiftID", availability.ShiftID);
                    cmd.Parameters.AddWithValue("@ShiftDate", availability.ShiftDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@IsAvailable", availability.IsAvailable);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}