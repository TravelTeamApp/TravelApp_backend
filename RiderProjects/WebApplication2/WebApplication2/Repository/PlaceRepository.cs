using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly ApplicationDbContext _context;


        public PlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all places along with their comments and place types
        public async Task<List<Place>> GetAllAsync()
        {
            return await _context.Places
                .Include(c => c.Comments)
                    .ThenInclude(a => a.User) // Include users related to comments
                .Include(pt => pt.PlaceType) // Include PlaceType related to Place
                .ToListAsync();
        }

        // Get a single place by its ID, including comments and the place type
        public async Task<Place?> GetByIdAsync(int id)
        {
            return await _context.Places
                .Include(c => c.Comments)
                    .ThenInclude(a => a.User) // Include users related to comments
                .Include(pt => pt.PlaceType) // Include the place type
                .FirstOrDefaultAsync(i => i.PlaceId == id);
        }

        // Check if a place exists by its ID
        public Task<bool> PlaceExists(int id)
        {
            return _context.Places.AnyAsync(s => s.PlaceId == id);
        }

        // Add a new place along with its place type (if necessary)
        public async Task<Place> AddPlaceAsync(Place place)
        {
            // Add the place to the database
            await _context.Places.AddAsync(place);
            await _context.SaveChangesAsync();
            return place;
        }

        // Update an existing place's information
        public async Task<Place?> UpdatePlaceAsync(int id, Place place)
        {
            var existingPlace = await _context.Places.FirstOrDefaultAsync(p => p.PlaceId == id);
            if (existingPlace == null)
                return null;

            existingPlace.PlaceName = place.PlaceName;
            existingPlace.PlaceAddress = place.PlaceAddress;
            existingPlace.Description = place.Description;
            existingPlace.Rating = place.Rating;
            existingPlace.PlaceTypeId = place.PlaceTypeId; // Update the PlaceTypeId

            await _context.SaveChangesAsync();
            return existingPlace;
        }

        // Delete a place by its ID
        public async Task<bool> DeletePlaceAsync(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null)
                return false;

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<Place>> GetPlacesByPlaceTypeIdsAsync(List<int> placeTypeIds)
        {
            return await _context.Places
                .Where(p => p.PlaceTypeId.HasValue && placeTypeIds.Contains(p.PlaceTypeId.Value))
                .Include(p => p.PlaceType) // Mekan türü ilişkisini yükle
                .Include(p => p.Comments) // Yorumları yükle
                .ToListAsync();
        }
        public async Task UpdateRatingAsync(int placeId, double newRating)
        {
            var place = await _context.Places.FindAsync(placeId);
            if (place != null)
            {
                place.Rating = newRating;
                _context.Places.Update(place);
                await _context.SaveChangesAsync();
            }
        }

        



    }
}
