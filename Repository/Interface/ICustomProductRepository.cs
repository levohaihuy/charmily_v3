using Models;

namespace Repository.Interface;

public interface ICustomProductRepository
{
    Task<List<CustomProduct>> GetAllCustomProductsAsync();
    Task<CustomProduct> GetCustomProductByIdAsync(int id);
    Task<List<CustomProduct>> GetPendingCustomProductsAsync();
    Task<CustomProduct> CreateCustomProductAsync(CustomProduct product);
    Task AddCustomProductImageAsync(CustomProductImage image);
    Task UpdateCustomProductStatusAsync(int id, string status, string comments, int adminId);
}
