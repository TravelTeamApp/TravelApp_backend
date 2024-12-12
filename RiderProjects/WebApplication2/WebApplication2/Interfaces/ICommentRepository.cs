using WebApplication2.Models;

namespace WebApplication2.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment commentModel);
    Task<Comment?> UpdateAsync(int id, Comment commentModel);
    Task<Comment?> DeleteAsync(int id);
    Task<List<Comment>> GetUserCommentsAsync(int userId);
    Task<List<Comment>> GetByPlaceIdAsync(int placeId);

}