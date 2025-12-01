namespace KlasseLib;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Underkategori – kan være null (ingen parent)
    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; } = true;

    public Category()
    {
    }

    public Category(int categoryId, string name, int? parentCategoryId = null, bool isActive = true)
    {
        CategoryId = categoryId;
        Name = name;
        ParentCategoryId = parentCategoryId;
        IsActive = isActive;
    }

    public override string ToString()
    {
        return $"CategoryId={CategoryId}, Name={Name}, ParentCategoryId={ParentCategoryId}, IsActive={IsActive}";
    }
}