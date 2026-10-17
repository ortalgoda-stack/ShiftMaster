using MySql.Data.MySqlClient;

namespace DAL
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
        public int MaxShiftsPerWeek { get; set; }
        public int MinShiftsPerWeek { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }

    public class userDal
    {
        private readonly string _connectionString;

        public userDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        // שליפת כל המשתמשים
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT UserID, Username, FullName, Email, PasswordHash, RoleID, Phone, MaxShiftsPerWeek, MinShiftsPerWeek, IsActive, CreatedAt FROM Users";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString()!,
                                FullName = reader["FullName"].ToString()!,
                                Email = reader["Email"].ToString()!,
                                PasswordHash = reader["PasswordHash"].ToString()!,
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                MaxShiftsPerWeek = Convert.ToInt32(reader["MaxShiftsPerWeek"]),
                                MinShiftsPerWeek = Convert.ToInt32(reader["MinShiftsPerWeek"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                    }
                }
            }
            return users;
        }

        // שליפת משתמש לפי ID
        public User? GetUserById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT UserID, Username, FullName, Email, PasswordHash, RoleID, Phone, MaxShiftsPerWeek, MinShiftsPerWeek, IsActive, CreatedAt FROM Users WHERE UserID = @UserID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString()!,
                                FullName = reader["FullName"].ToString()!,
                                Email = reader["Email"].ToString()!,
                                PasswordHash = reader["PasswordHash"].ToString()!,
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                MaxShiftsPerWeek = Convert.ToInt32(reader["MaxShiftsPerWeek"]),
                                MinShiftsPerWeek = Convert.ToInt32(reader["MinShiftsPerWeek"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        // הוספת משתמש חדש
        public bool AddUser(User user)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Users 
                                (Username, FullName, Email, PasswordHash, RoleID, Phone, MaxShiftsPerWeek, MinShiftsPerWeek, IsActive) 
                                VALUES 
                                (@Username, @FullName, @Email, @PasswordHash, @RoleID, @Phone, @MaxShiftsPerWeek, @MinShiftsPerWeek, @IsActive)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaxShiftsPerWeek", user.MaxShiftsPerWeek);
                    cmd.Parameters.AddWithValue("@MinShiftsPerWeek", user.MinShiftsPerWeek);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // עדכון משתמש (כולל מכסות משמרות Min/Max)
        public bool UpdateUser(User user)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"UPDATE Users 
                                SET Username = @Username, FullName = @FullName, Email = @Email, 
                                    RoleID = @RoleID, Phone = @Phone, MaxShiftsPerWeek = @MaxShiftsPerWeek, 
                                    MinShiftsPerWeek = @MinShiftsPerWeek, IsActive = @IsActive 
                                WHERE UserID = @UserID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaxShiftsPerWeek", user.MaxShiftsPerWeek);
                    cmd.Parameters.AddWithValue("@MinShiftsPerWeek", user.MinShiftsPerWeek);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // הפיכת משתמש ללא פעיל / מחיקה
        public bool DeleteUser(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Users SET IsActive = 0 WHERE UserID = @UserID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}