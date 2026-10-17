using MySql.Data.MySqlClient;

using MySql.Data.MySqlClient;

namespace DAL
{
    public class Shift
    {
        public int ShiftID { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RequiredEmployees { get; set; } = 1;
        public bool IsActive { get; set; } = true;
    }

    public class ShiftDal
    {
        private readonly string _connectionString;

        public ShiftDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Shift> GetAllShifts()
        {
            List<Shift> shifts = new List<Shift>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ShiftID, ShiftName, StartTime, EndTime, RequiredEmployees, IsActive FROM Shifts";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // טיפול בטוח בזמנים (TimeSpan מ-MySQL)
                            TimeSpan startTimeSpan = reader["StartTime"] != DBNull.Value ? (TimeSpan)reader["StartTime"] : TimeSpan.Zero;
                            TimeSpan endTimeSpan = reader["EndTime"] != DBNull.Value ? (TimeSpan)reader["EndTime"] : TimeSpan.Zero;

                            shifts.Add(new Shift
                            {
                                ShiftID = Convert.ToInt32(reader["ShiftID"]),
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString()! : "",

                                // אם המודל Shift אצלך משתמש ב-DateTime:
                                StartTime = DateTime.Today.Add(startTimeSpan),
                                EndTime = DateTime.Today.Add(endTimeSpan),

                                RequiredEmployees = reader["RequiredEmployees"] != DBNull.Value ? Convert.ToInt32(reader["RequiredEmployees"]) : 1,
                                IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : true
                            });
                        }
                    }
                }
            }
            return shifts;
        }

        public Shift? GetShiftById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ShiftID, ShiftName, StartTime, EndTime, RequiredEmployees, IsActive FROM Shifts WHERE ShiftID = @ShiftID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ShiftID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Shift
                            {
                                ShiftID = Convert.ToInt32(reader["ShiftID"]),
                                ShiftName = reader["ShiftName"].ToString()!,
                                StartTime = Convert.ToDateTime(reader["StartTime"]),
                                EndTime = Convert.ToDateTime(reader["EndTime"]),
                                RequiredEmployees = Convert.ToInt32(reader["RequiredEmployees"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool AddShift(Shift shift)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Shifts (ShiftName, StartTime, EndTime, RequiredEmployees, IsActive) 
                                VALUES (@ShiftName, @StartTime, @EndTime, @RequiredEmployees, @IsActive)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ShiftName", shift.ShiftName);
                    cmd.Parameters.AddWithValue("@StartTime", shift.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", shift.EndTime);
                    cmd.Parameters.AddWithValue("@RequiredEmployees", shift.RequiredEmployees);
                    cmd.Parameters.AddWithValue("@IsActive", shift.IsActive);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateShift(Shift shift)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"UPDATE Shifts 
                                SET ShiftName = @ShiftName, StartTime = @StartTime, EndTime = @EndTime, 
                                    RequiredEmployees = @RequiredEmployees, IsActive = @IsActive 
                                WHERE ShiftID = @ShiftID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ShiftID", shift.ShiftID);
                    cmd.Parameters.AddWithValue("@ShiftName", shift.ShiftName);
                    cmd.Parameters.AddWithValue("@StartTime", shift.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", shift.EndTime);
                    cmd.Parameters.AddWithValue("@RequiredEmployees", shift.RequiredEmployees);
                    cmd.Parameters.AddWithValue("@IsActive", shift.IsActive);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteShift(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Shifts SET IsActive = 0 WHERE ShiftID = @ShiftID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ShiftID", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}