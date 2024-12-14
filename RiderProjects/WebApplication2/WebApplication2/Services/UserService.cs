using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepo;



    public UserService(ApplicationDbContext context,IUserRepository userRepo)
    {
        _context = context;
        _userRepo = userRepo;
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

    // Find user by email
    public async Task<User?> FindUserByEmail(string email)
    {
        // E-posta geçerliliğini kontrol et
        if (string.IsNullOrEmpty(email))
        {
            return null; // Geçersiz e-posta
        }

        // E-posta adresiyle kullanıcıyı veritabanından sorgula
        var user = await _context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        return user;
    }

    // E-posta formatının geçerli olup olmadığını kontrol eden basit bir fonksiyon
    /*private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }*/
    public async Task<bool> UpdateUser(User user)
    {
        try
        {
            _context.Users.Update(user); // Kullanıcıyı günceller
            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına uygular
            return true;
        }
        catch (Exception ex)
        {
            // Güncelleme sırasında hata yönetimi
            Console.WriteLine($"Error updating user: {ex.Message}");
            return false;
        }
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
    }

    public async Task IncreaseUserScoreAsync(User user, int increment = 50)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        user.Score = (user.Score ?? 0) + increment;
    
        if (_userRepo == null)
        {
            throw new InvalidOperationException("UserRepository is not initialized.");
        }

        await _userRepo.UpdateAsync(user);
    }

}