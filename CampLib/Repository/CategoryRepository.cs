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
d
    // =========================
    // GET ALL
    // =========================
    public async Task<List<Category>> GetAllAsync()
    {
        var categories = new List<Category>();

        string sql = @"
            SELECT 
                CategoryId,
                Name,
                ParentCategoryId,
                
            FROM dbo.Categories
            ORDER BY Name
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ParentCategoryId = reader.IsDBNull(reader.GetOrdinal("ParentCategoryId"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("ParentCategoryId")),
            });
        }

        return categories;
    }

    // =========================
    // GET BY ID
    // =========================
    public async Task<Category?> GetByIdAsync(int categoryId)
    {
        Category? category = null;

        string sql = @"
            SELECT 
                CategoryId,
                Name,
                ParentCategoryId,
                IsActive
            FROM dbo.Categories
            WHERE CategoryId = @CategoryId
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            category = new Category
            {
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ParentCategoryId = reader.IsDBNull(reader.GetOrdinal("ParentCategoryId"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("ParentCategoryId")),
            };
        }

        return category;
    }

    // =========================
    // CREATE
    // =========================
    public async Task<Category> AddAsync(Category category)
    {
        string sql = @"
            INSERT INTO dbo.Categories (Name, ParentCategoryId,)
            OUTPUT INSERTED.CategoryId
            VALUES (@Name, @ParentCategoryId, @IsActive)
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Name", category.Name);
        cmd.Parameters.AddWithValue(
            "@ParentCategoryId",
            (object?)category.ParentCategoryId ?? DBNull.Value
        );

        var insertedId = (int)await cmd.ExecuteScalarAsync();
        category.CategoryId = insertedId;

        return category;
    }

    // =========================
    // UPDATE
    // =========================
    public async Task<Category?> UpdateAsync(int categoryId, Category category)
    {
        string sql = @"
            UPDATE dbo.Categories
            SET 
                Name = @Name,
                ParentCategoryId = @ParentCategoryId,
                IsActive = @IsActive
            WHERE CategoryId = @CategoryId
        ";

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
        cmd.Parameters.AddWithValue("@Name", category.Name);
        cmd.Parameters.AddWithValue(
            "@ParentCategoryId",
            (object?)category.ParentCategoryId ?? DBNull.Value
        );

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0 ? category : null;
    }
}
