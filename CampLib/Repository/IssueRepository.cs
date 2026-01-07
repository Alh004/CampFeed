using Microsoft.Data.SqlClient;
using System.Data;
using KlasseLib;

namespace CampLib.Repository
{
    public class IssueRepository
    {
        private readonly string _conn = 
            "Server=mssql7.unoeuro.com,1433;" +
            "Database=kunforhustlers_dk_db_campfeed;" +
            "User Id=kunforhustlers_dk;" +
            "Password=RmcAfptngeBaxkw6zr5E;" +
            "Encrypt=False;";

        public async Task<List<Issue>> GetAllAsync()
        {
            var list = new List<Issue>();
            string sql = @"SELECT * FROM Issues";

            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var issue = new Issue
                {
                    Idissue = reader.GetInt32(reader.GetOrdinal("Idissue")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),

                    LastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt")),

                    ClosedAt = reader.IsDBNull(reader.GetOrdinal("ClosedAt"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("ClosedAt")),

                    // Status & Severity er STRINGS i din DB og model
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    Severity = reader.GetString(reader.GetOrdinal("Severity")),

                    ReporterUserId = reader.GetInt32(reader.GetOrdinal("ReporterUserId")),
                    RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),

                    AssignedToUserId = reader.IsDBNull(reader.GetOrdinal("AssignedToUserId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("AssignedToUserId")),

                    AssignedDepartmentId = reader.IsDBNull(reader.GetOrdinal("AssignedDepartmentId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("AssignedDepartmentId"))
                };

                list.Add(issue);
            }

            return list;
        }
    }
}
