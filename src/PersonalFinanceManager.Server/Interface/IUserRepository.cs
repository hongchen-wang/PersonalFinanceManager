using PersonalFinanceManager.Server.Models;

namespace PersonalFinanceManager.Server.Interface
{
    public interface IUserRepository
    {
        User? GetUserByUsername(string username);
        User? GetUserByEmail(string email);
        Task AddUser(User user);
        Task UpdateUser(User user);
        User? GetUserByRefreshToken(string refreshToken);
        User? GetUserByResetToken(string resetToken);
    }
}
