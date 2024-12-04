using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IUserPlaceTypeRepository
    {
        Task AddUserPlaceTypeAsync(UserPlaceType userPlaceType);
        Task<List<PlaceType>> GetPlaceTypesByUserIdAsync(int userId);
        Task<bool> UserPlaceTypeExistsAsync(int userId, int placeTypeId);
        Task RemoveUserPlaceTypeAsync(int userId, int placeTypeId);
        Task<List<PlaceType>> GetAllPlaceTypesAsync();
    }
}