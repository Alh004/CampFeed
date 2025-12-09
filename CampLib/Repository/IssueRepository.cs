using System.Data;
using Microsoft.Data.SqlClient;
using KlasseLib;

public class IssueRepository
{
    private readonly SqlConnection _db;

    public IssueRepository()
    {
        _db = new SqlConnection(
            "Server=mssql7.unoeuro.com,1433;Database=kunforhustlers_dk_db_campfeed;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;Encrypt=False;");
    }

   
    // CREATE ISSUE
    public async Task<int> CreateAsync(Issue issue)
    {
        string sql = @"
            INSERT INTO dbo.Issue
            (Id, Title, Description, CreatedAt, LastUpdatedAt, Status, Severity,
             RoomId, CategoryId, AssignedDepartmentId, AssignedToUserId)
            VALUES
            (@Id, @Title, @Description, @CreatedAt, @LastUpdatedAt, @Status, @Severity,
             @RoomId, @CategoryId, @AssignedDepartmentId, @AssignedToUserId)
        ";

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@Id", issue.Id);
            cmd.Parameters.AddWithValue("@Title", issue.Title);
            cmd.Parameters.AddWithValue("@Description", issue.Description);
            cmd.Parameters.AddWithValue("@CreatedAt", issue.CreatedAt);
            cmd.Parameters.AddWithValue("@LastUpdatedAt", (object?)issue.LastUpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", issue.Status.ToString());
            cmd.Parameters.AddWithValue("@Severity", issue.Severity.ToString());
            cmd.Parameters.AddWithValue("@RoomId", issue.RoomId);
            cmd.Parameters.AddWithValue("@CategoryId", issue.CategoryId);
            cmd.Parameters.AddWithValue("@AssignedDepartmentId", (object?)issue.AssignedDepartmentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignedToUserId", (object?)issue.AssignedToUserId ?? DBNull.Value);

            await _db.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            _db.Close();

            return rows;
        }
    }

   
    // GET BY ID
    public async Task<Issue?> GetByIdAsync(int id)
    {
        Issue? issue = null;

        string sql = @"
            SELECT 
                Id, Title, Description, CreatedAt, LastUpdatedAt, ClosedAt,
                Status, Severity, RoomId, CategoryId, AssignedDepartmentId, AssignedToUserId
            FROM dbo.Issue
            WHERE Id = @Id
        ";

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            await _db.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    issue = new Issue
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        LastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt")),
                        ClosedAt = reader.IsDBNull(reader.GetOrdinal("ClosedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ClosedAt")),
                        Status = Enum.Parse<IssueStatus>(reader.GetString(reader.GetOrdinal("Status"))),
                        Severity = Enum.Parse<IssueSeverity>(reader.GetString(reader.GetOrdinal("Severity"))),
                        RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        AssignedDepartmentId = reader.IsDBNull(reader.GetOrdinal("AssignedDepartmentId")) ? null : reader.GetInt32(reader.GetOrdinal("AssignedDepartmentId")),
                        AssignedToUserId = reader.IsDBNull(reader.GetOrdinal("AssignedToUserId")) ? null : reader.GetInt32(reader.GetOrdinal("AssignedToUserId"))
                    };
                }
            }

            _db.Close();
        }

        return issue;
    }

    
    // GET ALL
    public async Task<List<Issue>> GetAllAsync()
    {
        List<Issue> issues = new();

        string sql = @"
            SELECT 
                Id, Title, Description, CreatedAt, LastUpdatedAt, ClosedAt,
                Status, Severity, RoomId, CategoryId, AssignedDepartmentId, AssignedToUserId
            FROM dbo.Issue
            ORDER BY CreatedAt DESC
        ";

        using (var cmd = new SqlCommand(sql, _db))
        {
            await _db.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    issues.Add(new Issue
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        LastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt")),
                        ClosedAt = reader.IsDBNull(reader.GetOrdinal("ClosedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ClosedAt")),
                        Status = Enum.Parse<IssueStatus>(reader.GetString(reader.GetOrdinal("Status"))),
                        Severity = Enum.Parse<IssueSeverity>(reader.GetString(reader.GetOrdinal("Severity"))),
                        RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        AssignedDepartmentId = reader.IsDBNull(reader.GetOrdinal("AssignedDepartmentId")) ? null : reader.GetInt32(reader.GetOrdinal("AssignedDepartmentId")),
                        AssignedToUserId = reader.IsDBNull(reader.GetOrdinal("AssignedToUserId")) ? null : reader.GetInt32(reader.GetOrdinal("AssignedToUserId"))
                    });
                }
            }

            _db.Close();
        }

        return issues;
    }
    
    // UPDATE
    public async Task<bool> UpdateAsync(Issue issue)
    {
        string sql = @"
            UPDATE dbo.Issue
            SET 
                Title = @Title,
                Description = @Description,
                LastUpdatedAt = @LastUpdatedAt,
                Status = @Status,
                Severity = @Severity,
                RoomId = @RoomId,
                CategoryId = @CategoryId,
                AssignedDepartmentId = @AssignedDepartmentId,
                AssignedToUserId = @AssignedToUserId,
                ClosedAt = @ClosedAt
            WHERE Id = @Id
        ";

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@Id", issue.Id);
            cmd.Parameters.AddWithValue("@Title", issue.Title);
            cmd.Parameters.AddWithValue("@Description", issue.Description);
            cmd.Parameters.AddWithValue("@LastUpdatedAt", (object?)issue.LastUpdatedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", issue.Status.ToString());
            cmd.Parameters.AddWithValue("@Severity", issue.Severity.ToString());
            cmd.Parameters.AddWithValue("@RoomId", issue.RoomId);
            cmd.Parameters.AddWithValue("@CategoryId", issue.CategoryId);
            cmd.Parameters.AddWithValue("@AssignedDepartmentId", (object?)issue.AssignedDepartmentId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignedToUserId", (object?)issue.AssignedToUserId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClosedAt", (object?)issue.ClosedAt ?? DBNull.Value);

            await _db.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            _db.Close();

            return rows > 0;
        }
    }
}
