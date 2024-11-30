namespace WebApplication2.Models;

public class Comment
{
    public int CommentID { get; set; } // Yorumun benzersiz kimliği
    public int PlaceID { get; set; } // Yoruma ait yerin kimliği
    public int UserID { get; set; } // Yorumu yapan kullanıcının kimliği
    public double? Rating { get; set; } // Yorumdaki değerlendirme puanı (örneğin 0-5 arası)
    public string? Review { get; set; } // Yorum başlığı veya kısa özeti
    public DateTime? Date { get; set; } = DateTime.Now; // Yorumun yapıldığı tarih
    public string? CommentDescription { get; set; } // Yorumun tam açıklaması

    // Yer ile ilişki
    public Place Place { get; set; } // Yorumun ait olduğu yer
    // Kullanıcı ile ilişki (opsiyonel)
    public User User { get; set; } // Yorumu yapan kullanıcı
}
