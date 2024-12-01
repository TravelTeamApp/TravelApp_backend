namespace WebApplication2.Models;

public class PlaceType
{
    public int PlaceTypeId { get; set; }  // Mekan türünün id'si
    public string PlaceTypeName { get; set; } = string.Empty; // Mekan türü adı (Restoran, Otel, vb.)
        
    public ICollection<UserPlaceType>? UserPlaceTypes { get; set; } = new List<UserPlaceType>();
}