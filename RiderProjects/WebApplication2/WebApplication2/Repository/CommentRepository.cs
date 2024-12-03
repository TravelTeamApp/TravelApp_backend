using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == id);

        if (commentModel == null)
        {
            return null;
        }

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
     
        return await _context.Comments
            .Include(a => a.User) 
            .Include(c => c.Place)  
            .OrderByDescending(c => c.CreatedOn) 
            .ToListAsync();
    }

    public async Task<List<Comment>> GetUserCommentsAsync(int userId)
    {
     
        return await _context.Comments.Where(f => f.UserID == userId)
            .Include(a => a.User) 
            .Include(c => c.Place)  
            .OrderByDescending(c => c.CreatedOn) 
            .ToListAsync();
    }


    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.Include(a => a.User).FirstOrDefaultAsync(c => c.CommentId == id);
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await _context.Comments.FindAsync(id);

        if (existingComment == null)
        {
            return null;
        }

        existingComment.Text = commentModel.Text;

        await _context.SaveChangesAsync();

        return existingComment;
    }
}