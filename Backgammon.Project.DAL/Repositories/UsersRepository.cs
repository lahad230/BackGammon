using Backgammon.Project.DAL.DataContext;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon.Project.DAL.Repositories
{
    public class UsersRepository
    {
        private readonly BgDataContext _context;

        public UsersRepository(BgDataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<User> users;
            users = await Task.Run(() => { return _context.Users; });
            return users;
        }

        public async Task<User> GetUserByUsernameAndPassword(string UserName, string Password)
        {
            return await Task.Run(() => _context.Users.FirstOrDefault((user) => user.UserName == UserName && user.Password == Password));
        }

    }
}
