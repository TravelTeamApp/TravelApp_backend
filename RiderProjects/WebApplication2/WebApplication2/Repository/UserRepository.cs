using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Interfaces;
using WebApplication2.Models;


namespace WebApplication2.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // The asynchronous update method
        public async Task UpdateAsync(User user)
        {
            // Check if the user exists in the database
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == user.UserID);

            if (existingUser != null)
            {
                // Update the properties of the existing user
                existingUser.Score = user.Score;
                existingUser.UserName = user.UserName; // Update other fields as needed

                // Save changes asynchronously
                await _context.SaveChangesAsync();
            }
        }
    }
}