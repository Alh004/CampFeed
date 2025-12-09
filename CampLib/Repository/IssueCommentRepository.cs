using System.Data;
using Microsoft.Data.SqlClient;
using KlasseLib;

public class IssueCommentRepository
{
    private readonly SqlConnection _db;

    public IssueCommentRepository()
    {
        _db = new SqlConnection(
            "Server=mssql7.unoeuro.com,1433;Database=kunforhustlers_dk_db_campfeed;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;Encrypt=False;");
    }
    
    // CREATE COMMENT
    public async Task<int> AddCommentAsync(Issue_Comment comment)
    {
        string sql = @"
            INSERT INTO dbo.Issue_Comment (Id, Text, CreatedAt, IssueId, CreatedByUserId)
            VALUES (@Id, @Text, @CreatedAt, @IssueId, @CreatedByUserId)
        ";

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@Id", comment.id);
            cmd.Parameters.AddWithValue("@Text", comment.Text);
            cmd.Parameters.AddWithValue("@CreatedAt", comment.createdAt);
            cmd.Parameters.AddWithValue("@IssueId", comment.Issueid);
            cmd.Parameters.AddWithValue("@CreatedByUserId", comment.CreatedByUserid);

            await _db.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            _db.Close();

            return rows;
        }
    }

   
    // GET ALL COMMENTS FOR ISSUE
    public async Task<List<Issue_Comment>> GetCommentsForIssueAsync(int issueId)
    {
        List<Issue_Comment> comments = new();

        string sql = @"
            SELECT Id, Text, CreatedAt, IssueId, CreatedByUserId
            FROM dbo.Issue_Comment
            WHERE IssueId = @IssueId
            ORDER BY CreatedAt
        ";

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@IssueId", issueId);

            await _db.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    comments.Add(new Issue_Comment(
                        createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        text: reader.GetString(reader.GetOrdinal("Text")),
                        issueId: reader.GetInt32(reader.GetOrdinal("IssueId")),
                        createdByUserId: reader.GetInt32(reader.GetOrdinal("CreatedByUserId"))
                    ));
                }
            }

            _db.Close();
        }

        return comments;
    }
}
