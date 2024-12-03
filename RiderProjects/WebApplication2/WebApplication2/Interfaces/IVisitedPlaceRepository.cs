using WebApplication2.Models;

namespace WebApplication2.Interfaces;


public interface IVisitedPlaceRepository
{
    Task<List<VisitedPlace>> GetUserVisitedPlacesAsync(int userId);
    Task<VisitedPlace> CreateAsync(VisitedPlace visitedPlace);
    Task<VisitedPlace> DeleteVisitedPlace(User appUser, int placeId);
    Task<bool> HasVisitedAsync(int userId, int placeId);
}