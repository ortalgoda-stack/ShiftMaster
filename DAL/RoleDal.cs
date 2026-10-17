using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ShiftMaster.DAL
{
    public class RoleDal
    {
        private readonly string _connectionString;

        public RoleDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 1. שליפת כל התפקידים
        public List<Role> GetAllRoles()
        {
            List<Role> list = new List<Role>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT RoleID, RoleName FROM roles";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Role
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString()! : ""
                            });
                        }
                    }
                }
            }
            return list;
        }

        // 2. הוספת תפקיד חדש
        public bool AddRole(Role role)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO roles (RoleName) VALUES (@RoleName)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoleName", role.RoleName);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}