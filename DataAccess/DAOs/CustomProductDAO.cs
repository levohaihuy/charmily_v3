using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class CustomProductDAO : SingletonBase<CustomProductDAO>
{
    public async Task<List<CustomProduct>> GetAllCustomProductsAsync()
    {
        try
        {
            return await _context.CustomProducts
                .Include(cp => cp.CustomProductImages)
                .Include(cp => cp.BaseProduct)
                .Where(cp => !cp.IsDeleted.Value)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("loi list custom products: " + e.Message);
            throw;
        }
    }

    public async Task<CustomProduct> GetCustomProductByIdAsync(int id)
    {
        try
        {
            return await _context.CustomProducts
                .Include(cp => cp.CustomProductImages)
                .Include(cp => cp.BaseProduct)
                .FirstOrDefaultAsync(cp => cp.CustomProductId == id && !cp.IsDeleted.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine("loi get custom product: " + e.Message);
            throw;
        }
    }

    public async Task<List<CustomProduct>> GetPendingCustomProductsAsync()
    {
        try
        {
            return await _context.CustomProducts
                .Include(cp => cp.Customer)
                .Include(cp => cp.BaseProduct)
                .Where(cp => cp.Status == "pending" && !cp.IsDeleted.Value)
                .OrderByDescending(cp => cp.CreatedAt)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("loi get pending custom products: " + e.Message);
            throw;
        }
    }

    public async Task<CustomProduct> CreateCustomProductAsync(CustomProduct product)
    {
        try
        {
            _context.CustomProducts.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine("loi create custom product: " + e.Message);
            throw;
        }
    }

    public async Task AddCustomProductImageAsync(CustomProductImage image)
    {
        try
        {
            _context.CustomProductImages.Add(image);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("loi add custom product image: " + e.Message);
            throw;
        }
    }
    

    public async Task UpdateCustomProductStatusAsync(int id, string status, string comments, int adminId)
    {
        try
        {
            var product = await _context.CustomProducts.FindAsync(id);
            if (product != null)
            {
                product.Status = status;
                product.AdminComments = comments;
                product.ApprovedBy = adminId;
                product.ApprovedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("loi update custom product status: " + e.Message);
            throw;
        }
    }
}