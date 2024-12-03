namespace WebApplication2.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? TCKimlik { get; set; }
        public int? Score { get; set; } = 10;

    }
}