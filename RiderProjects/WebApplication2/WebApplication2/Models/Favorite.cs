namespace WebApplication2.Models;

public class Favorite
{
    public int FavoriteId { get; set; }
    public int? UserID { get; set; }
    public User? User { get; set; }
    public int? PlaceId { get; set; }
    public Place? Place { get; set; }
}