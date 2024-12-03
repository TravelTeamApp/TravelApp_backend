using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Services;
using System.Linq;
using WebApplication2.Mappers;

namespace WebApplication2.Controllers;


[Route("api/visitedPlace")]
[ApiController]
public class VisitedPlaceController : ControllerBase
{
    private readonly IPlaceRepository _placeRepo;
    private readonly IVisitedPlaceRepository _visitedPlaceRepo;
    private readonly UserService _userService;

   
    public VisitedPlaceController(UserService userService,IPlaceRepository placeRepo, IVisitedPlaceRepository visitedPlaceRepo)
    {
        _placeRepo = placeRepo;
        _visitedPlaceRepo = visitedPlaceRepo;
        _userService = userService;

        
    }


    [HttpGet]
    public async Task<IActionResult> GetUserVisitedPlaces()
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

        // Fetch user's visitedPlaces using the repository function
        var visitedPlaces = await _visitedPlaceRepo.GetUserVisitedPlacesAsync(userId);

        // Map entities to DTOs using the extension method

        var visitedPlaceDtos = visitedPlaces.Select(f => f.ToVisitedPlaceDto()).ToList();

        return Ok(visitedPlaceDtos);
    }

    [HttpPost]
    public async Task<IActionResult> AddVisitedPlace(int placeId)
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

        var userVisitedPlace = await _visitedPlaceRepo.GetUserVisitedPlacesAsync(userId);

        if (userVisitedPlace.Any(e => e.PlaceId == placeId)) return BadRequest("Cannot add same place to visitedPlace");

        var visitedPlaceModel = new VisitedPlace
        {
            PlaceId = place.PlaceId,
            UserID = appUser.UserID
        };

        await _visitedPlaceRepo.CreateAsync(visitedPlaceModel);

        if (visitedPlaceModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else
        {
            return Created();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteVisitedPlace(int placeId)
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

        var userVisitedPlace = await _visitedPlaceRepo.GetUserVisitedPlacesAsync(userId);

        var filteredPlace = userVisitedPlace.Where(s => s.PlaceId == placeId ).ToList();

        if (filteredPlace.Count() == 1)
        {
            await _visitedPlaceRepo.DeleteVisitedPlace(appUser, placeId);
        }
        else
        {
            return BadRequest("Place not in your visitedPlace");
        }

        return Ok();
    }

}