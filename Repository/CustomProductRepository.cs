using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class CustomProductRepository : ICustomProductRepository
{
    private readonly CustomProductDAO _customProductDAO;

    public CustomProductRepository(CustomProductDAO customProductDAO)
    {
        _customProductDAO = customProductDAO;
    }

    public async Task<List<CustomProduct>> GetAllCustomProductsAsync()
    {
        return await _customProductDAO.GetAllCustomProductsAsync();
    }

    public async Task<CustomProduct> GetCustomProductByIdAsync(int id)
    {
        return await _customProductDAO.GetCustomProductByIdAsync(id);
    }

    public async Task<List<CustomProduct>> GetPendingCustomProductsAsync()
    {
        return await _customProductDAO.GetPendingCustomProductsAsync();
    }

    public async Task<CustomProduct> CreateCustomProductAsync(CustomProduct product)
    {
        return await _customProductDAO.CreateCustomProductAsync(product);
    }

    public async Task AddCustomProductImageAsync(CustomProductImage image)
    {
        await _customProductDAO.AddCustomProductImageAsync(image);
    }
    

    public async Task UpdateCustomProductStatusAsync(int id, string status, string comments, int adminId)
    {
        await _customProductDAO.UpdateCustomProductStatusAsync(id, status, comments, adminId);
    }
}