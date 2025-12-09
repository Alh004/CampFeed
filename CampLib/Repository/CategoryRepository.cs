using System.Data;
using Microsoft.Data.SqlClient;
using KlasseLib;

namespace CampLib.Repository;

public class CategoryRepository
{
    private readonly SqlConnection _db;

    public CategoryRepository()
    {
        _db = new SqlConnection(
            "Server=mssql7.unoeuro.com,1433;" +
            "Database=kunforhustlers_dk_db_campfeed;" +
            "User Id=kunforhustlers_dk;" +
            "Password=RmcAfptngeBaxkw6zr5E;" +
            "Encrypt=False;"
        );
    }
    
    public async Task<List<Category>> GetAllAsync()
    {
        List<Category> categories = new();

        string sql = @"
            SELECT Id, Name, ParentCategoryId, IsActive
            FROM dbo.Category
            WHERE IsActive = 1
            ORDER BY Name";

        await _db.OpenAsync();

        using (var cmd = new SqlCommand(sql, _db))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
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
        }

        await _db.CloseAsync();
        return categories;
    }

   
    public async Task<Category?> GetByIdAsync(int id)
    {
        Category? category = null;

        string sql = @"
            SELECT Id, Name, ParentCategoryId, IsActive
            FROM dbo.Category
            WHERE Id = @Id";

        await _db.OpenAsync();

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
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
            }
        }

        await _db.CloseAsync();
        return category;
    }

  
    public async Task<int> CreateAsync(Category category)
    {
        string sql = @"
            INSERT INTO dbo.Category (Id, Name, ParentCategoryId, IsActive)
            VALUES (@Id, @Name, @ParentCategoryId, @IsActive)";

        await _db.OpenAsync();

        using (var cmd = new SqlCommand(sql, _db))
        {
            cmd.Parameters.AddWithValue("@Id", category.CategoryId);
            cmd.Parameters.AddWithValue("@Name", category.Name);
            cmd.Parameters.AddWithValue("@ParentCategoryId", (object?)category.ParentCategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", category.IsActive);

            int rows = await cmd.ExecuteNonQueryAsync();
            await _db.CloseAsync();

            return rows;
        }
    }
}
