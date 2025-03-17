using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class CustomerDAO : SingletonBase<CustomerDAO>
{
    public async Task<List<Customer>> GetListCustomerAsync()
    {
        try
        {
            var customers = await _context.Customers.ToListAsync();

            return customers.Count > 0 ? customers : new List<Customer>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error list customer! {ex.Message}");
            return new List<Customer>();
        }
    }

    public async Task ChangePassAsync(int id, string pass)
    {
        try
        {
            var customer = await GetCustomerByIdAsync(id);

            if (customer == null) return;

            customer.Password = pass;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error change pass! {e.Message}");
        }
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        try
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get customer by id! {ex.Message}");
            return null;
        }
    }

    public async Task<Customer> FindCustomerByEmailAsync(string email)
    {
        try
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error find customer by email! {e.Message}");
            return null;
        }
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        try
        {
            if (customer == null) return null;

            var existingCustomer = await GetCustomerByIdAsync(customer.CustomerId);

            if (existingCustomer != null) return null;

            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();

            return customer;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error create customer! {ex.Message}");
            return null;
        }
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        if (customer == null)
        {
            Console.WriteLine("error update customer");
            return null;
        }

        try
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.CustomerId);

            if (existingCustomer == null) return null;

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Gender = customer.Gender;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Address = customer.Address;
            existingCustomer.Dob = customer.Dob;
            existingCustomer.Username = customer.Username;
            existingCustomer.Password = customer.Password;
            existingCustomer.Email = customer.Email;
            existingCustomer.Picture = customer.Picture;
            existingCustomer.Status = customer.Status;
            existingCustomer.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingCustomer;
        }
        catch (Exception e)
        {
            Console.WriteLine($"error update customer! {e.Message}");
            return null;
        }
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        try
        {
            var customer = await GetCustomerByIdAsync(customerId);

            if (customer == null) return;

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error delete customer! {ex.Message}");
        }
    }

    public async Task<Customer> GetCustomerByUsernameAsync(string username)
    {
        try
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Username.Equals(username));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get customer name! {ex.Message}");
            return null;
        }
    }

    public async Task<Customer> GetCustomerByEmailAsync(string email)
    {
        try
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Email.Equals(email));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get customer email! {ex.Message}");
            return null;
        }
    }
}