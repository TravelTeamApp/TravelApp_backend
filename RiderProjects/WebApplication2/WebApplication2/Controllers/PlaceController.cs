using WebApplication2.Interfaces;
using WebApplication2.Mappers;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Dtos.Comment;
using WebApplication2.Dtos.Place;
using WebApplication2.Dtos.PlaceType;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlaceRepository _placeRepo;
        private readonly IUserPlaceTypeRepository _userPlaceTypeRepo;

        public PlaceController(ApplicationDbContext context, IPlaceRepository placeRepo, IUserPlaceTypeRepository userPlaceTypeRepo)
        {
            _placeRepo = placeRepo;
            _context = context;
            _userPlaceTypeRepo = userPlaceTypeRepo;
        }

        // Get all places along with their comments and place types
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var places = await _placeRepo.GetAllAsync();

            var placeDto = places.Select(s => s.ToPlaceDto()).ToList();

            return Ok(placeDto);
        }

        // Get a place by its ID, including comments and place type
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var place = await _placeRepo.GetByIdAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            return Ok(place.ToPlaceDto());
        }

        [HttpGet("userplace")]
        public async Task<IActionResult> GetPlacesByUserPlaceTypes()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized("User not authenticated.");

            var userPlaceTypes = await _userPlaceTypeRepo.GetPlaceTypesByUserIdAsync(userId);
            if (userPlaceTypes == null || !userPlaceTypes.Any())
                return NotFound("Kullanıcının seçtiği mekan türü bulunamadı.");

            var placeTypeIds = userPlaceTypes.Select(pt => pt.PlaceTypeId).ToList();
            var places = await _placeRepo.GetPlacesByPlaceTypeIdsAsync(placeTypeIds);

            if (places == null || !places.Any())
                return NotFound("Seçilen mekan türlerine ait mekan bulunamadı.");

            // DTO'ya map etme işlemi
            var placeDtos = places.Select(p => p.ToPlaceDto()).ToList();

            return Ok(placeDtos);
        }






        

        
    }
}
