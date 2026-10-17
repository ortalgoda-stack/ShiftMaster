using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShiftMaster.DAL
{
    public class ConstraintDal
    {
        private readonly string _connectionString;

        public ConstraintDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. שליפת כל האילוצים
        public List<ConstraintModel> GetAllConstraints()
        {
            List<ConstraintModel> list = new List<ConstraintModel>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT c.ConstraintID, c.UserID, c.CreatedByRole, c.BlockedShiftID, c.Reason, c.CreatedAt,
                                        u.FullName AS UserName, s.ShiftName AS BlockedShiftName
                                 FROM constraints c
                                 LEFT JOIN users u ON c.UserID = u.UserID
                                 LEFT JOIN shifts s ON c.BlockedShiftID = s.ShiftID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ConstraintModel
                            {
                                ConstraintID = Convert.ToInt32(reader["ConstraintID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                CreatedByRole = reader["CreatedByRole"] != DBNull.Value ? reader["CreatedByRole"].ToString()! : "Employee",
                                BlockedShiftID = Convert.ToInt32(reader["BlockedShiftID"]),
                                Reason = reader["Reason"] != DBNull.Value ? reader["Reason"].ToString() : "",
                                CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.Now,
                                UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : "",
                                BlockedShiftName = reader["BlockedShiftName"] != DBNull.Value ? reader["BlockedShiftName"].ToString() : ""
                            });
                        }
                    }
                }
            }
            return list;
        }

        // 2. הוספת אילוץ חדש
        public bool AddConstraint(ConstraintModel constraint)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO constraints (UserID, CreatedByRole, BlockedShiftID, Reason) 
                                 VALUES (@UserID, @CreatedByRole, @BlockedShiftID, @Reason)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", constraint.UserID);
                    cmd.Parameters.AddWithValue("@CreatedByRole", constraint.CreatedByRole ?? "Employee");
                    cmd.Parameters.AddWithValue("@BlockedShiftID", constraint.BlockedShiftID);
                    cmd.Parameters.AddWithValue("@Reason", constraint.Reason ?? "");

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}