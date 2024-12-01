using WebApplication2.Models;

namespace WebApplication2.Dtos.Comment;

public class CommentDto
{
    public int CommentId { get; set; }
    public int PlaceId { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public string? CreatedBy { get; set; }
    public int UserID { get; set; }
    
}