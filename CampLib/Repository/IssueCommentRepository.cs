using System.Data;
using Microsoft.Data.SqlClient;
using KlasseLib;

public class IssueCommentRepository
{
    private readonly string _connectionString =
        "Server=mssql7.unoeuro.com,1433;Database=kunforhustlers_dk_db_campfeed;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;Encrypt=False;";

    // ---------------------------------------------------------
    // ADD COMMENT
    // ---------------------------------------------------------
    public async Task<int> AddCommentAsync(Issue_Comment comment)
    {
        const string sql = @"
            INSERT INTO Issue_Comment (Text, CreatedAt, IsInternal, IssueId, CreatedByUserId)
            OUTPUT INSERTED.Idcomment
            VALUES (@Text, @CreatedAt, @IsInternal, @IssueId, @CreatedByUserId);
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Text", comment.Text);
        cmd.Parameters.AddWithValue("@CreatedAt", comment.CreatedAt);
        cmd.Parameters.AddWithValue("@IsInternal", comment.IsInternal);
        cmd.Parameters.AddWithValue("@IssueId", comment.IssueId);
        cmd.Parameters.AddWithValue("@CreatedByUserId", comment.CreatedByUserId);

        // Returner det nye ID
        int newId = (int)await cmd.ExecuteScalarAsync();
        return newId;
    }

    // ---------------------------------------------------------
    // GET ALL COMMENTS FOR ONE ISSUE
    // ---------------------------------------------------------
    public async Task<List<Issue_Comment>> GetCommentsForIssueAsync(int issueId)
    {
        var comments = new List<Issue_Comment>();

        const string sql = @"
            SELECT Idcomment, Text, CreatedAt, IsInternal, IssueId, CreatedByUserId
            FROM Issue_Comment
            WHERE IssueId = @IssueId
            ORDER BY CreatedAt ASC;
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@IssueId", issueId);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            comments.Add(new Issue_Comment
            {
                Idcomment = reader.GetInt32(reader.GetOrdinal("Idcomment")),
                Text = reader.GetString(reader.GetOrdinal("Text")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                IsInternal = reader.GetBoolean(reader.GetOrdinal("IsInternal")),
                IssueId = reader.GetInt32(reader.GetOrdinal("IssueId")),
                CreatedByUserId = reader.GetInt32(reader.GetOrdinal("CreatedByUserId"))
            });
        }

        return comments;
    }
}
