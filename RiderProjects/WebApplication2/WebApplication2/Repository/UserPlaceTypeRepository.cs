using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;

namespace WebApplication2.Repository
{
    public class UserPlaceTypeRepository : IUserPlaceTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPlaceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kullanıcı ve mekan türü ilişkilendirmesi ekler
        public async Task AddUserPlaceTypeAsync(UserPlaceType userPlaceType)
        {
            await _context.UserPlaceTypes.AddAsync(userPlaceType);
            await _context.SaveChangesAsync();
        }

        // Kullanıcının sahip olduğu mekan türlerini alır
        public async Task<List<PlaceType>> GetPlaceTypesByUserIdAsync(int userId)
        {
            var userPlaceTypes = await _context.UserPlaceTypes
                .Where(upt => upt.UserId == userId)
                .Include(u => u.PlaceType)  // PlaceType'ı da dahil et
                .ToListAsync();

            return userPlaceTypes.Select(upt => upt.PlaceType).ToList();
        }

        // Belirli bir kullanıcı ve mekan türü ilişkisinin var olup olmadığını kontrol eder
        public async Task<bool> UserPlaceTypeExistsAsync(int userId, int placeTypeId)
        {
            return await _context.UserPlaceTypes
                .AnyAsync(upt => upt.UserId == userId && upt.PlaceTypeId == placeTypeId);
        }

        // Kullanıcı ve mekan türü ilişkisini siler
        public async Task RemoveUserPlaceTypeAsync(int userId, int placeTypeId)
        {
            var userPlaceType = await _context.UserPlaceTypes
                .FirstOrDefaultAsync(upt => upt.UserId == userId && upt.PlaceTypeId == placeTypeId);

            if (userPlaceType != null)
            {
                _context.UserPlaceTypes.Remove(userPlaceType);
                await _context.SaveChangesAsync();
            }
        }
    }
}