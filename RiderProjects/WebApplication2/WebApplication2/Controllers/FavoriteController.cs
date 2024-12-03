using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Services;
using System.Linq;
using WebApplication2.Mappers;

namespace WebApplication2.Controllers;


[Route("api/favorite")]
[ApiController]
public class FavoriteController : ControllerBase
{
    private readonly IPlaceRepository _placeRepo;
    private readonly IFavoriteRepository _favoriteRepo;
    private readonly UserService _userService;

   
    public FavoriteController(UserService userService,IPlaceRepository placeRepo, IFavoriteRepository favoriteRepo)
    {
        _placeRepo = placeRepo;
        _favoriteRepo = favoriteRepo;
        _userService = userService;

        
    }


    [HttpGet]
    public async Task<IActionResult> GetUserFavorites()
    {
        // Retrieve userId from session
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("User not authenticated.");
        }

        // Fetch user details
        var appUser = await _userService.GetUserById(userId);
        if (appUser == null)
        {
            return Unauthorized("User not found.");
        }

        // Fetch user's favorites using the repository function
        var favorites = await _favoriteRepo.GetUserFavoritesAsync(userId);

        // Map entities to DTOs using the extension method

        var favoriteDtos = favorites.Select(f => f.ToFavoriteDto()).ToList();

        return Ok(favoriteDtos);
    }

    [HttpPost]
    public async Task<IActionResult> AddFavorite(int placeId)
    {
        
        var place = await _placeRepo.GetByIdAsync(placeId);

        if (place == null)
        {
          
           
                return BadRequest("Place does not exists");
          
        }

        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("User not authenticated.");
        }
        // Fetch the user based on the userId from session
        var appUser = await _userService.GetUserById(userId);
        if (appUser == null)
        {
            return Unauthorized("User not found.");
        }

        var userFavorite = await _favoriteRepo.GetUserFavoritesAsync(userId);

        if (userFavorite.Any(e => e.PlaceId == placeId)) return BadRequest("Cannot add same place to favorite");

        var favoriteModel = new Favorite
        {
            PlaceId = place.PlaceId,
            UserID = appUser.UserID
        };

        await _favoriteRepo.CreateAsync(favoriteModel);

        if (favoriteModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else
        {
            return Created();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFavorite(int placeId)
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("User not authenticated.");
        }
        // Fetch the user based on the userId from session
        var appUser = await _userService.GetUserById(userId);
        if (appUser == null)
        {
            return Unauthorized("User not found.");
        }

        var userFavorite = await _favoriteRepo.GetUserFavoritesAsync(userId);

        var filteredPlace = userFavorite.Where(s => s.PlaceId == placeId ).ToList();

        if (filteredPlace.Count() == 1)
        {
            await _favoriteRepo.DeleteFavorite(appUser, placeId);
        }
        else
        {
            return BadRequest("Place not in your favorite");
        }

        return Ok();
    }

}