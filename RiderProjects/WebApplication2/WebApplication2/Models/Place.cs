namespace WebApplication2.Models
{
    public class Place
    {
        public int PlaceId { get; set; }              // Mekan ID
        public string? PlaceName { get; set; }         // Mekan Adı
        public string? PlaceAddress { get; set; }      // Mekan Adresi
        public string? Description { get; set; }       // Mekan Açıklaması
        public int? Rating { get; set; }               // Mekan Puanı

        // Yorumlar (Comment) ile olan ilişki
        public List<Comment>? Comments { get; set; } = new List<Comment>();

        // Favoriler (Favorite) ile olan ilişki
        public List<Favorite>? Favorites { get; set; } = new List<Favorite>();

        // Mekan Türü (PlaceType) ile olan ilişki
        public int? PlaceTypeId { get; set; }            // Yabancı anahtar
        public PlaceType? PlaceType { get; set; }
        public ICollection<VisitedPlace> VisitedPlaces { get; set; }
    }
}