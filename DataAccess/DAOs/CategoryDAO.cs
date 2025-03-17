using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class CategoryDAO : SingletonBase<CategoryDAO>
{
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted.Value)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error getting categories: " + e.Message);
            throw;
        }
    }

    public async Task<Category> GetCategoryByIdAsync(long categoryId)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId && !c.IsDeleted.Value);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        if (category == null) throw new ArgumentNullException(nameof(category));
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
        try
        {
            return await _context.Categories.FirstOrDefaultAsync(
                c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving category by name: " + ex.Message);
            return null;
        }
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
        if (existingCategory != null)
        {
            existingCategory.Name = category.Name; // Cập nhật các thuộc tính khác nếu cần
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        return null;
    }

    public async Task DeleteCategoryAsync(long categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}