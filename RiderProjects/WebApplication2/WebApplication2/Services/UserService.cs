using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> Authenticate(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    public async Task<bool> Register(User newUser)
    {
        if (await _context.Users.AnyAsync(u => u.Email == newUser.Email))
        {
            return false; // Email zaten var
        }

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return true;
    }
}