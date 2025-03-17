using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class AddressRepository : IAddressRepository
{
    private readonly AddressDAO _addressDAO;

    public AddressRepository(AddressDAO addressDAO)
    {
        _addressDAO = addressDAO;
    }

    public async Task<List<Address>> GetAllAddressesAsync()
    {
        return await _addressDAO.GetAllAddressesAsync();
    }

    public async Task<Address> GetAddressByIdAsync(long addressId)
    {
        return await _addressDAO.GetAddressByIdAsync(addressId);
    }

    public async Task<Address> CreateAddressAsync(Address address)
    {
        return await _addressDAO.CreateAddressAsync(address);
    }

    public async Task<Address> UpdateAddressAsync(Address address)
    {
        return await _addressDAO.UpdateAddressAsync(address);
    }

    public async Task DeleteAddressAsync(long addressId)
    {
        await _addressDAO.DeleteAddressAsync(addressId);
    }
}