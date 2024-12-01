using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Dtos.Comment;
using WebApplication2.Interfaces;
using WebApplication2.Mappers;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        private readonly ICommentRepository _commentRepo;
        private readonly IPlaceRepository _placeRepo;
       
        public CommentController(UserService userService,ApplicationDbContext context,ICommentRepository commentRepo,
        IPlaceRepository placeRepo)
        {
            _commentRepo = commentRepo;
            _placeRepo = placeRepo;
            _context = context;
            _userService = userService;

        }

        [HttpGet]
        
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync();

            var commentDto = comments.Select(s => s.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        [Route("{placeId:int}")]
        public async Task<IActionResult> Create([FromRoute] int placeId, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullanıcıyı Session'dan alalım
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("User not authenticated.");
            }

            // Fetch the place from the repository
            var place = await _placeRepo.GetByIdAsync(placeId);

            if (place == null)
            {
                return BadRequest("Place does not exist");
            }

            // Fetch the user based on the userId from session
            var appUser = await _userService.GetUserById(userId);
            if (appUser == null)
            {
                return Unauthorized("User not found.");
            }

            // Create the comment model and associate it with the user
            var commentModel = commentDto.ToCommentFromCreate(placeId);
            commentModel.UserID = appUser.UserID;

            // Save the comment to the repository
            await _commentRepo.CreateAsync(commentModel);

            // Return the created comment with a CreatedAtAction response
            return CreatedAtAction(nameof(GetById), new { id = commentModel.CommentId }, commentModel.ToCommentDto());
        }




        

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullanıcı bilgilerini session'dan alıyoruz
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("User not authenticated.");
            }

            // Yorum modelini güncelliyoruz
            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            // Yorumun oturum açmış kullanıcıya ait olup olmadığını kontrol edelim
            if (comment.UserID != userId)
            {
                return Unauthorized("You are not authorized to update this comment.");
            }

            // Yorum güncelleme işlemi
            var updatedcomment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate(id));

            return Ok(updatedcomment.ToCommentDto());
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Yorumun veritabanında var olup olmadığını kontrol et
            var commentModel = await _commentRepo.GetByIdAsync(id);

            if (commentModel == null)
            {
                return NotFound("Comment does not exist");
            }

            // Yorumun silinmesini gerçekleştir
            await _commentRepo.DeleteAsync(id);

            // Silinen yorumun DTO'sunu oluştur
            var commentDto = commentModel.ToCommentDto();

            // Silinen yorumu döndür
            return Ok(commentDto);
        }

    }