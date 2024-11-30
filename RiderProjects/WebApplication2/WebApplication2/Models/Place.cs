namespace WebApplication2.Models;

public class Place
{
    public int PlaceID { get; set; } // Benzersiz bir kimlik
    public string PlaceName { get; set; } // Yer adı
    public string Address { get; set; } // Yer adresi
    public string Description { get; set; } // Yer açıklaması
    public double Rating { get; set; } // Yer değerlendirme puanı (örneğin 0-5 arasında)

    // Yorumlarla ilişki
    public List<Comment> Comments { get; set; } = new List<Comment>(); // Bir yerin birden fazla yorumu olabilir
}
