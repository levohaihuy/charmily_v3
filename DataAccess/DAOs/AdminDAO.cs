using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class AdminDAO : SingletonBase<AdminDAO>
{
    public async Task<List<Admin>> GetListAdminAsync()
    {
        try
        {
            var admins = await _context.Admins.ToListAsync();

            return admins.Count > 0 ? admins : new List<Admin>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error list admin! {ex.Message}");
            return new List<Admin>();
        }
    }

    public async Task ChangePassAsync(int id, string pass)
    {
        try
        {
            var admin = await GetAdminByIdAsync(id);

            if (admin == null) return;

            admin.Password = pass;

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error change pass! {e.Message}");
        }
    }

    public async Task<Admin> GetAdminByIdAsync(int id)
    {
        try
        {
            return await _context.Admins.SingleOrDefaultAsync(a => a.AdminId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get admin by id! {ex.Message}");
            return null;
        }
    }

    public async Task<Admin> FindAdminByEmailAsync(string email)
    {
        try
        {
            return await _context.Admins.SingleOrDefaultAsync(a => a.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error find admin by email! {e.Message}");
            return null;
        }
    }


    public async Task<Admin> CreateAdminAsync(Admin admin)
    {
        try
        {
            if (admin == null) return null;

            var existingAdmin = await GetAdminByIdAsync(Convert.ToInt32(admin.AdminId));

            if (existingAdmin != null) return null;

            await _context.Admins.AddAsync(admin);

            await _context.SaveChangesAsync();

            return admin;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error create admin! {ex.Message}");
            return null;
        }
    }

    public async Task<Admin> UpdateAdminAsync(Admin admin)
    {
        if (admin == null) return null;

        try
        {
            var existingAdmin = await _context.Admins.FindAsync(admin.AdminId);

            if (existingAdmin == null) return null;

            existingAdmin.FirstName = admin.FirstName;
            existingAdmin.LastName = admin.LastName;
            existingAdmin.Username = admin.Username;
            existingAdmin.Password = admin.Password;
            existingAdmin.Email = admin.Email;
            existingAdmin.Picture = admin.Picture;
            existingAdmin.Role = admin.Role;
            existingAdmin.Status = admin.Status;
            existingAdmin.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingAdmin;
        }
        catch (Exception e)
        {
            Console.WriteLine($"error update admin! {e.Message}");
            return null;
        }
    }

    public async Task DeleteAdminAsync(int adminId)
    {
        try
        {
            var admin = await GetAdminByIdAsync(adminId);

            if (admin == null) return;

            _context.Admins.Remove(admin);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error delete admin! {ex.Message}");
        }
    }

    public async Task<Admin> GetAdminByUsernameAsync(string username)
    {
        try
        {
            return await _context.Admins.SingleOrDefaultAsync(a => a.Username.Equals(username));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get admin name! {ex.Message}");
            return null;
        }
    }
}