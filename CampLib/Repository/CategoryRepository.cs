using System.Data;
using Microsoft.Data.SqlClient;
using KlasseLib;

namespace CampLib.Repository;

public class CategoryRepository
{
    private readonly string _connectionString = 
        "Server=mssql7.unoeuro.com,1433;" +
        "Database=kunforhustlers_dk_db_campfeed;" +
        "User Id=kunforhustlers_dk;" +
        "Password=RmcAfptngeBaxkw6zr5E;" +
        "Encrypt=False;";

    // Read all
    public async Task<List<Category>> GetAllAsync()
    {
        var categories = new List<Category>();
        string sql = "SELECT Id, Name, ParentCategoryId, IsActive FROM dbo.Category WHERE IsActive = 1 ORDER BY Name";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                CategoryId = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ParentCategoryId = reader.IsDBNull(reader.GetOrdinal("ParentCategoryId"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("ParentCategoryId")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }

        return categories;
    }

    // Get by Id
    public async Task<Category?> GetByIdAsync(int id)
    {
        Category? category = null;
        string sql = "SELECT Id, Name, ParentCategoryId, IsActive FROM dbo.Category WHERE Id = @Id";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            category = new Category
            {
                CategoryId = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ParentCategoryId = reader.IsDBNull(reader.GetOrdinal("ParentCategoryId"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("ParentCategoryId")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        return category;
    }

    // Create / AddAsync
    public async Task<Category> AddAsync(Category category)
    {
        string sql = @"
            INSERT INTO dbo.Category (Name, ParentCategoryId, IsActive)
            OUTPUT INSERTED.Id
            VALUES (@Name, @ParentCategoryId, @IsActive)
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Name", category.Name);
        cmd.Parameters.AddWithValue("@ParentCategoryId", (object?)category.ParentCategoryId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsActive", category.IsActive);

        // Hent det auto-genererede Id
        var insertedId = (int)await cmd.ExecuteScalarAsync();
        category.CategoryId = insertedId;

        return category;
    }

    // UpdateAsync
    public async Task<Category?> UpdateAsync(int id, Category category)
    {
        string sql = @"
            UPDATE dbo.Category
            SET Name = @Name,
                ParentCategoryId = @ParentCategoryId,
                IsActive = @IsActive
            WHERE Id = @Id
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", category.Name);
        cmd.Parameters.AddWithValue("@ParentCategoryId", (object?)category.ParentCategoryId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsActive", category.IsActive);

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0 ? category : null;
    }
    
}
