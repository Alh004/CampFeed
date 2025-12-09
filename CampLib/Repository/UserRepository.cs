using CampLib.Model;
using Microsoft.Data.SqlClient;

public class UserRepository
{
    private readonly string _connectionString =
        "Server=mssql7.unoeuro.com,1433;" +
        "Database=kunforhustlers_dk_db_campfeed;" +
        "User Id=kunforhustlers_dk;" +
        "Password=RmcAfptngeBaxkw6zr5E;" +
        "Encrypt=False;";

    // Hent alle edu.zealand.dk-brugere
    public async Task<List<User>> GetAllAsync()
    {
        var users = new List<User>();

        string sql = @"
            SELECT Id, Email
            FROM dbo.[User]
            WHERE Email LIKE '%@edu.zealand.dk'
            ORDER BY Email";

        await using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();

        await using var cmd = new SqlCommand(sql, db);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            string email = reader.GetString(reader.GetOrdinal("Email"));

            try
            {
                users.Add(new User(id, email));
            }
            catch
            {
                // Spring over emails som ikke matcher @edu.zealand.dk
            }
        }

        return users;
    }

    // Hent bruger på Id
    public async Task<User?> GetByIdAsync(int id)
    {
        User? user = null;

        string sql = @"
            SELECT Id, Email
            FROM dbo.[User]
            WHERE Id = @Id";

        await using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();

        await using var cmd = new SqlCommand(sql, db);
        cmd.Parameters.AddWithValue("@Id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            int userId = reader.GetInt32(reader.GetOrdinal("Id"));
            string email = reader.GetString(reader.GetOrdinal("Email"));

            try
            {
                user = new User(userId, email);
            }
            catch
            {
                user = null; // Email matcher ikke @edu.zealand.dk
            }
        }

        return user;
    }

    // AddAsync
    public async Task<User> AddAsync(User user)
    {
        string sql = @"
            INSERT INTO dbo.[User] (Email)
            OUTPUT INSERTED.Id
            VALUES (@Email)";

        await using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();

        await using var cmd = new SqlCommand(sql, db);
        cmd.Parameters.AddWithValue("@Email", user.Email);

        var insertedId = (int)await cmd.ExecuteScalarAsync();
        user.Id = insertedId;

        return user;
    }

    // UpdateAsync
    public async Task<User?> UpdateAsync(int id, User user)
    {
        string sql = @"
            UPDATE dbo.[User]
            SET Email = @Email
            WHERE Id = @Id";

        await using var db = new SqlConnection(_connectionString);
        await db.OpenAsync();

        await using var cmd = new SqlCommand(sql, db);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Email", user.Email);

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0 ? user : null;
    }
    
}