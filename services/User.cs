using CrudApi.Models;
using CrudApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using CrudApi.Dtos;

namespace CrudApi.Services;

public interface IUserService
{
    Task<UserDetailDto?> GetUserByIdAsync(int id);
    bool VerifyPassword(User user, string password);  
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user, string password);
    Task<User?> UpdateUserAsync(int id, User userUpdate);
    Task<bool> DeleteUserAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Password verification
    public bool VerifyPassword(User user, string password)
    {
        if (user == null || string.IsNullOrEmpty(password))
        {
            return false;
        }

        var hashedInputPassword = HashPassword(password);
        return user.PasswordHash == hashedInputPassword;
    }

    // Create new user
    public async Task<User> CreateUserAsync(User user, string password)
    {
        if (await _context.Users!.AnyAsync(u => u.Email == user.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        // Hash password
        user.PasswordHash = HashPassword(password);
        user.CreatedAt = DateTime.UtcNow;

        _context.Users!.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    // Get user by ID
   public async Task<UserDetailDto?> GetUserByIdAsync(int id)
{
    var userWithKaryawan = await _context.Users!
        .Where(u => u.Id == id)
        .Select(u => new UserDetailDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Karyawan = u.Karyawan.Select(k => new KaryawanDetailDto
            {
                Nik = k.Nik,
                Name = k.Name,
                TanggalLahir = k.TanggalLahir,
                Alamat = k.Alamat,
                JenisKelamin = k.JenisKelamin
            }).ToList()
        })
        .FirstOrDefaultAsync();

    return userWithKaryawan;
}

    // Get user by email
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users!
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    // Get all users
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users!
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    // Update user
    public async Task<User?> UpdateUserAsync(int id, User userUpdate)
    {
        var user = await _context.Users!.FindAsync(id);

        if (user == null)
        {
            return null;
        }

        if (userUpdate.Email != user.Email && 
            await _context.Users.AnyAsync(u => u.Email == userUpdate.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        user.Name = userUpdate.Name;
        user.Email = userUpdate.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return user;
    }

    // Delete user
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users!.FindAsync(id);
        
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    // Helper method to hash password
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}