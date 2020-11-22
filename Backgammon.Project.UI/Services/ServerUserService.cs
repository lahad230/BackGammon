using Backgammon.Project.UI.Infra;
using Microsoft.AspNetCore.SignalR.Client;
using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon.Project.UI.Services
{
    public class ServerUserService : IServerUserService
    {
        private readonly IServerContextService _server;

        public event EventHandler<UserActionEventArgs> LogInCompleted;
        public event EventHandler<UserActionEventArgs> LogOutCompleted;

        public ServerUserService(IServerContextService server)
        {
            _server = server;
            RegisterToService();
        }

        private void RegisterToService()
        {
            _server.HubConnection.On("LogInNotificated", async (string userName) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        LogInCompleted?.Invoke(this, new UserActionEventArgs(userName));
                    });
            });

            _server.HubConnection.On("LogOutNotificated", async (string userName) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        LogOutCompleted?.Invoke(this, new UserActionEventArgs(userName));
                    });
            });
        }

        public async Task<string> Register(User user)
        {
            string error = await _server.HubConnection.InvokeAsync<string>("Register", user);
            if (error == string.Empty)
            {
                UpdateLogIn(user.UserName);
            }
            return error;
        }

        public async Task<User> Login(string userName, string password)
        {
            User currentUser = await _server.HubConnection.InvokeAsync<User>("Login", userName, password);
            if (currentUser != null)
            {
                UpdateLogIn(currentUser.UserName);
            }
            return currentUser;
        }

        public async void Logout(string userName)
        {
            await _server.HubConnection.InvokeAsync("Logout", userName);
            UpdateLogOut(userName);
        }

        public async Task<IEnumerable<string>> GetAllUsers()
        {
            IEnumerable<string> users = await _server.HubConnection.InvokeAsync<IEnumerable<string>>("GetAllUserNames");
            return users;
        }

        private void UpdateLogIn(string userName)
        {
            _server.HubConnection.SendAsync("LogInCompletedNotifcation", userName);
        }

        private void UpdateLogOut(string userName)
        {
            _server.HubConnection.SendAsync("LogOutCompletedNotifcation", userName);
        }
    }
}
