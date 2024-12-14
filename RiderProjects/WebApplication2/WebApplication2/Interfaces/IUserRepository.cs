
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IUserRepository
    {
        Task UpdateAsync(User user);
    }
}