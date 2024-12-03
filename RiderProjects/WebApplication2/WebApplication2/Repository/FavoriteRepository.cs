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


public class FavoriteRepository : IFavoriteRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;

    public FavoriteRepository(IHttpContextAccessor httpContextAccessor,UserService userService,ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _userService = userService;
    }

    public async Task<Favorite> CreateAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync();
        return favorite;
    }

    public async Task<Favorite> DeleteFavorite(User AppUser, int placeId)
    {
        var favoriteModel = await _context.Favorites.FirstOrDefaultAsync(x => x.UserID == AppUser.UserID && x.Place.PlaceId == placeId);

        if (favoriteModel == null)
        {
            return null;
        }

        _context.Favorites.Remove(favoriteModel);
        await _context.SaveChangesAsync();
        return favoriteModel;
    }

    public async Task<List<Favorite>> GetUserFavoritesAsync(int userId)
    {
        return await _context.Favorites
            .Where(f => f.UserID == userId)
            .Include(f => f.Place)                      // Place'ı dahil et
            .ThenInclude(p => p.PlaceType)           // PlaceType'ı dahil et
            .Include(f => f.Place)                      // Place'ı tekrar dahil etmek yerine zaten yukarıda dahil ettik
            .ThenInclude(p => p.Comments)           // Comments'ı dahil et
            .ThenInclude(c => c.User)           // Comment'ların User'larını da dahil e
            .ToListAsync();
    }




}