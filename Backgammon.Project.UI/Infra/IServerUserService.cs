using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon.Project.UI.Infra
{
    public interface IServerUserService
    {
        event EventHandler<UserActionEventArgs> LogInCompleted;
        event EventHandler<UserActionEventArgs> LogOutCompleted;

        Task<string> Register(User user);
        Task<User> Login(string userName, string password);
        void Logout(string userName);
        Task<IEnumerable<string>> GetAllUsers();
    }
}
