namespace WebApplication2.Models;

public class Comment
{
    public int CommentId { get; set; }
    public int PlaceId { get; set; }
    public Place? Place { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedOn { get; set; }= DateTime.Now;
    public int UserID { get; set; }
    public User? User { get; set; }
}