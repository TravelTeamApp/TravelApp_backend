namespace WebApplication2.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TCKimlik { get; set; }
        public int? Score { get; set; } = 10;

        public ICollection<Favorite> Favorites { get; set; }  // Kullanıcının favori mekanları
        public ICollection<Comment> Comments { get; set; }    // Kullanıcının yorumları
        // Kullanıcının seçebileceği Mekan Türleri
        public ICollection<UserPlaceType> UserPlaceTypes { get; set; }
        public ICollection<VisitedPlace> VisitedPlaces { get; set; }
    }
}