using Models;

namespace Xanh_Dau.DTO;

public class CategoryDTO
{
    public Category Category { get; set; }

    public List<Category> ListCategories { get; set; } = new();
}