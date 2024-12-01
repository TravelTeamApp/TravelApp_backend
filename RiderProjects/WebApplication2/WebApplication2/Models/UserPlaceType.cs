namespace WebApplication2.Models
{
    public class UserPlaceType
    {
        public int UserId { get; set; }       // Kullanıcı ID
        public User User { get; set; }         // Kullanıcı ile ilişki

        public int PlaceTypeId { get; set; }  // Mekan Türü ID
        public PlaceType PlaceType { get; set; } // Mekan Türü ile ilişki
    }
}