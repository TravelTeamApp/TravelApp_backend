using WebApplication2.Dtos.Comment;
using WebApplication2.Dtos.PlaceType;

namespace WebApplication2.Dtos.Place
{
    public class PlaceDto
    {
        public int PlaceId { get; set; }
        public string? PlaceName { get; set; }
        public string? PlaceAddress { get; set; }
        public string? Description { get; set; }
        public double? Rating { get; set; }
        public double? Latitude { get; set; }    // Enlem
        public double? Longitude { get; set; }   // Boylam
        // Yeni eklenen PlaceType bilgisi
        public PlaceTypeDto? PlaceType { get; set; } // Her mekanın sadece bir türü olacak

        public List<CommentDto>? Comments { get; set; } // Yorumlar
    }
}