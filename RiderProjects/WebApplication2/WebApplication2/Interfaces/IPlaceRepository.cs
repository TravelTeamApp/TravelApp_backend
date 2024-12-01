using WebApplication2.Models;

namespace WebApplication2.Interfaces;

public interface IPlaceRepository
{
    Task<List<Place>> GetAllAsync();
    Task<Place?> GetByIdAsync(int id);
    Task<bool> PlaceExists(int id);
}