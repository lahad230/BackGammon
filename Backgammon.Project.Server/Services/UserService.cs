using Backgammon.Project.DAL;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backgammon.Project.Server.Services
{
    public class UserService
    {
        private readonly UnitOfWork _db;
        private readonly SessionDataService _data;
        public UserService(UnitOfWork db, SessionDataService data)
        {
            _db = db;
            _data = data;
        }

        public async Task<string> RegisterAsync(User user, string conncetionId)
        {
            if(user.Password.Length < 4 || string.IsNullOrWhiteSpace(user.Password))          
                return "Password length must be 4 or more";
            if (string.IsNullOrWhiteSpace(user.UserName))
                return "User name field cant be empty";

            var users = await _db.UsersRepository.GetUsersAsync();

            foreach (var u in users)
            {
                if(u.UserName == user.UserName)
                {
                    return "User exists";
                }
            }

            if (await _db.UsersRepository.CreateUserAsync(user))
            {
                _data.AddUser(user.UserName, conncetionId);
                return string.Empty;
            }
            else
                return "There was an error with the registration process";            
        }


        public async Task<User> Login(string userName, string password, string connectionId)
        {
            var user = await _db.UsersRepository.GetUserByUsernameAndPassword(userName, password);
            if(user != null && !_data.UsersOnline.ContainsKey(userName))
            {
                _data.AddUser(user.UserName, connectionId);
                return user;
            }
            return null;
        }
    }
}
