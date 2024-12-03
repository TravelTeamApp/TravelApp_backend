namespace WebApplication2.Interfaces;

public interface IFavoriteService
{
    Task<List<FavoriteDto>> GetFavoritesByCurrentUserAsync();
    Task AddFavoriteAsync(int placeId);
    Task RemoveFavoriteAsync(int favoriteId);
}
