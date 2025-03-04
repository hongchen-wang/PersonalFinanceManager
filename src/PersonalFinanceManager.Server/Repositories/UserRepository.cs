using PersonalFinanceManager.Server.Data;
using PersonalFinanceManager.Server.Models;

namespace PersonalFinanceManager.Server.Repositories
{
    public class UserRepository
    {
        private readonly FinanceManagerDbContext _context;
        public UserRepository(FinanceManagerDbContext context)
        {
            _context = context;
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public async Task AddUser(User user)
        {
            if (GetUserByUsername(user.Username) == null
                && GetUserByEmail(user.Email) == null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(User user)
        {
            if (GetUserByUsername(user.Username) != null
                || GetUserByEmail(user.Email) != null)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public User? GetUserByRefreshToken(string refreshToken)
        {
            return _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
        }
    }
}
