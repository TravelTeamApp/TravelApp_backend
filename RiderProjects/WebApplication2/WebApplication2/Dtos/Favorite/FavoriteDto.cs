using WebApplication2.Dtos.Comment;
using WebApplication2.Dtos.PlaceType;

public class FavoriteDto
{
    public int FavoriteId { get; set; }
    public int PlaceId { get; set; }
    public string? PlaceName { get; set; }
    public string? PlaceAddress { get; set; }
    public string? Description { get; set; }
    public double? Rating { get; set; }
        
    // Yeni eklenen PlaceType bilgisi
    public PlaceTypeDto? PlaceType { get; set; } // Her mekanın sadece bir türü olacak

    public List<CommentDto>? Comments { get; set; } // Yorumlar // Added UserName field
    public  string? UserName { get; set; }
}