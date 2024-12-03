using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.Mappers;
using WebApplication2.Services;
namespace WebApplication2.Repository;


public class VisitedPlaceRepository : IVisitedPlaceRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;

    public VisitedPlaceRepository(IHttpContextAccessor httpContextAccessor,UserService userService,ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _userService = userService;
    }

    public async Task<VisitedPlace> CreateAsync(VisitedPlace visitedPlace)
    {
        await _context.VisitedPlaces.AddAsync(visitedPlace);
        await _context.SaveChangesAsync();
        return visitedPlace;
    }

    public async Task<VisitedPlace> DeleteVisitedPlace(User AppUser, int placeId)
    {
        var visitedPlaceModel = await _context.VisitedPlaces.FirstOrDefaultAsync(x => x.UserID == AppUser.UserID && x.Place.PlaceId == placeId);

        if (visitedPlaceModel == null)
        {
            return null;
        }

        _context.VisitedPlaces.Remove(visitedPlaceModel);
        await _context.SaveChangesAsync();
        return visitedPlaceModel;
    }

    public async Task<List<VisitedPlace>> GetUserVisitedPlacesAsync(int userId)
    {
        return await _context.VisitedPlaces
            .Where(f => f.UserID == userId)
            .Include(f => f.Place)                      // Place'ı dahil et
            .ThenInclude(p => p.PlaceType)           // PlaceType'ı dahil et
            .Include(f => f.Place)                      // Place'ı tekrar dahil etmek yerine zaten yukarıda dahil ettik
            .ThenInclude(p => p.Comments)           // Comments'ı dahil et
            .ThenInclude(c => c.User)           // Comment'ların User'larını da dahil e
            .ToListAsync();
    }
    
    public async Task<bool> HasVisitedAsync(int userId, int placeId)
    {
        return await _context.VisitedPlaces
            .AnyAsync(vp => vp.UserID == userId && vp.PlaceId == placeId);
    }





}