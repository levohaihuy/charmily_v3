using Models;

namespace Repository.Interface;

public interface ICustomerRepository
{
    Task<List<Customer>> GetListCustomerAsync();

    Task ChangePassAsync(int id, string pass);

    Task<Customer> GetCustomerByIdAsync(int id);

    Task<Customer> FindCustomerByEmailAsync(string email);

    Task<Customer> CreateCustomerAsync(Customer customer);

    Task<Customer> UpdateCustomerAsync(Customer customer);

    Task DeleteCustomerAsync(int customerId);

    Task<Customer> GetCustomerByUsernameAsync(string username);

    Task<Customer> GetCustomerByEmailAsync(string email);
}