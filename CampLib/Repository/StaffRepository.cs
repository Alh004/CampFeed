using CampLib.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampLib.Repositorya
{
    public class StaffRepository
    {
        private readonly string _connectionString =
            "Server=mssql7.unoeuro.com,1433;" +
            "Database=kunforhustlers_dk_db_campfeed;" +
            "User Id=kunforhustlers_dk;" +
            "Password=RmcAfptngeBaxkw6zr5E;" +
            "Encrypt=False;";

        // =========================
        // GET ALL STAFF
        // =========================
        public async Task<List<Staff>> GetAllAsync()
        {
            var staffList = new List<Staff>();
            string sql = "SELECT IdStaff, Username, Password FROM dbo.Staff ORDER BY Username";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                staffList.Add(new Staff(
                    reader.GetInt32(reader.GetOrdinal("IdStaff")),
                    reader.GetString(reader.GetOrdinal("Username")),
                    reader.GetString(reader.GetOrdinal("Password"))
                ));
            }

            return staffList;
        }

        // =========================
        // ADD STAFF
        // =========================
        public async Task<Staff> AddAsync(Staff staff)
        {
            if (!staff.Username.EndsWith("@edu.zealand.dk", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Staff skal have en @edu.zealand.dk email");

            string sql = @"
                INSERT INTO dbo.Staff (Username, Password)
                OUTPUT INSERTED.IdStaff
                VALUES (@Username, @Password)
            ";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", staff.Username);
            cmd.Parameters.AddWithValue("@Password", staff.Password);

            var insertedId = (int)await cmd.ExecuteScalarAsync();
            staff.Id = insertedId;

            return staff;
        }

        // =========================
        // DELETE STAFF
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "DELETE FROM dbo.Staff WHERE IdStaff = @Id";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        // =========================
        // GET BY USERNAME (til login)
        // =========================
        public async Task<Staff?> GetByUsernameAsync(string username)
        {
            Staff? staff = null;
            string sql = "SELECT IdStaff, Username, Password FROM dbo.Staff WHERE Username = @Username";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                staff = new Staff(
                    reader.GetInt32(reader.GetOrdinal("IdStaff")),
                    reader.GetString(reader.GetOrdinal("Username")),
                    reader.GetString(reader.GetOrdinal("Password"))
                );
            }d

            return staff;
        }
    }
}