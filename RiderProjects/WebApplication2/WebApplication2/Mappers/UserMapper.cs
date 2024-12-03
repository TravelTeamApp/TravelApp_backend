using WebApplication2.Models;

namespace WebApplication2.Mappers;

public static class UserMapper
{
    public static User ToUser(this RegisterRequestDto dto)
    {
        return new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Password = dto.Password, // Şifre hashlenmeli (örneğin BCrypt kullanabilirsiniz)
            TCKimlik = dto.TCKimlik,

        };
    }
}
