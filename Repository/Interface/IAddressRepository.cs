using Models;

namespace Repository.Interface;

public interface IAddressRepository
{
    Task<List<Address>> GetAllAddressesAsync();

    Task<Address> GetAddressByIdAsync(long addressId);

    Task<Address> CreateAddressAsync(Address address);

    Task<Address> UpdateAddressAsync(Address address);

    Task DeleteAddressAsync(long addressId);
}