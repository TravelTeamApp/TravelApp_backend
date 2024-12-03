using WebApplication2.Models;

namespace WebApplication2.Interfaces;


public interface IFavoriteRepository
{
    Task<List<Favorite>> GetUserFavoritesAsync(int userId);
    Task<Favorite> CreateAsync(Favorite favorite);
    Task<Favorite> DeleteFavorite(User appUser, int placeId);
}