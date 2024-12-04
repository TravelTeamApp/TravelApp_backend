using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTOs;
using WebApplication2.Dtos.PlaceType;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPlaceTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IUserPlaceTypeRepository _userPlaceTypeRepository;

        public UserPlaceTypeController(ApplicationDbContext context,IUserPlaceTypeRepository userPlaceTypeRepository)
        {
            _userPlaceTypeRepository = userPlaceTypeRepository;
            _context = context;
        }

        // Kullanıcı ve mekan türü ilişkisi ekler
        [HttpPost]
        [Route("add-by-names")]
        public async Task<IActionResult> AddUserPlaceTypeByNames([FromBody] UserPlaceTypeDto dto)
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized("User not authenticated.");

            if (dto.PlaceTypeNames == null || !dto.PlaceTypeNames.Any())
                return BadRequest("PlaceTypeNames cannot be null or empty.");

            // Verilen PlaceTypeName'lere göre PlaceTypeId'leri al
            var placeTypes = await _context.PlaceTypes
                .Where(pt => dto.PlaceTypeNames.Contains(pt.PlaceTypeName))
                .ToListAsync();

            if (!placeTypes.Any())
                return NotFound("No matching place types found.");

            var addedPlaceTypes = new List<string>();
            var existingPlaceTypes = new List<string>();

            foreach (var placeType in placeTypes)
            {
                // Zaten ilişki var mı kontrol et
                var exists = await _userPlaceTypeRepository.UserPlaceTypeExistsAsync(userId, placeType.PlaceTypeId);
                if (exists)
                {
                    existingPlaceTypes.Add(placeType.PlaceTypeName);
                    continue;
                }

                var userPlaceType = new UserPlaceType
                {
                    UserId = userId,
                    PlaceTypeId = placeType.PlaceTypeId
                };

                await _userPlaceTypeRepository.AddUserPlaceTypeAsync(userPlaceType);
                addedPlaceTypes.Add(placeType.PlaceTypeName);
            }

            return Ok(new
            {
                Message = "Operation completed.",
                AddedPlaceTypes = addedPlaceTypes,
                ExistingPlaceTypes = existingPlaceTypes
            });
        }


        // Kullanıcının sahip olduğu mekan türlerini getirir
        [HttpGet]
        public async Task<IActionResult> GetPlaceTypesByUserId()
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized("User not authenticated.");

            var placeTypes = await _userPlaceTypeRepository.GetPlaceTypesByUserIdAsync(userId);

            if (placeTypes == null || !placeTypes.Any())
                return NotFound("No place types found for this user.");

            return Ok(placeTypes);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPlaceTypes()
        {
            // Tüm mekan türlerini al
            var placeTypes = await _context.PlaceTypes
                .Select(pt => new PlaceTypeDto
                {
                    PlaceTypeId = pt.PlaceTypeId,
                    PlaceTypeName = pt.PlaceTypeName
                })
                .ToListAsync();

            if (placeTypes == null || !placeTypes.Any())
                return NotFound("No place types found.");

            return Ok(placeTypes);
        }


        // Kullanıcı ve mekan türü ilişkisinin varlığını kontrol eder
        [HttpGet("exists")]
        public async Task<IActionResult> CheckUserPlaceTypeExists([FromQuery] int placeTypeId)
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized("User not authenticated.");

            var exists = await _userPlaceTypeRepository.UserPlaceTypeExistsAsync(userId, placeTypeId);
            return Ok(new { exists });
        }

        // Kullanıcı ve mekan türü ilişkisini siler
        [HttpDelete]
        public async Task<IActionResult> RemoveUserPlaceType([FromQuery] int placeTypeId)
        {
            // Kullanıcı ID'sini session'dan al
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized("User not authenticated.");

            var exists = await _userPlaceTypeRepository.UserPlaceTypeExistsAsync(userId, placeTypeId);
            if (!exists)
                return NotFound("UserPlaceType does not exist.");

            await _userPlaceTypeRepository.RemoveUserPlaceTypeAsync(userId, placeTypeId);
            return Ok("UserPlaceType successfully removed.");
        }
    }
}
