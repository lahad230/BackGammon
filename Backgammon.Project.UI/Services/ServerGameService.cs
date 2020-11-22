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
    public class ServerGameService : IServerGameService
    {
        private readonly IServerContextService _server;
        public event EventHandler<UserActionEventArgs> GameRequested;
        public event EventHandler<GameAcceptedEventArgs> GameAccepted;
        public event EventHandler<UserActionEventArgs> GameDeclined;
        public event EventHandler<MoveMadeEventArgs> MoveFinished;

        public ServerGameService(IServerContextService server)
        {
            _server = server;
            RegisterToService();
        }

        private void RegisterToService()
        {
            _server.HubConnection.On("GameRequested", async (string userName) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        GameRequested?.Invoke(this, new UserActionEventArgs(userName));
                    });
            });

            _server.HubConnection.On("GameAccepted", async (Guid gameId, string user) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        GameAccepted?.Invoke(this, new GameAcceptedEventArgs(gameId, user));
                    });
            });

            _server.HubConnection.On("GameDeclined", async (string userName) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        GameDeclined?.Invoke(this, new UserActionEventArgs(userName));
                    });
            });

            _server.HubConnection.On("MoveMade", async (Game game) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        MoveFinished?.Invoke(this, new MoveMadeEventArgs(game));
                    });
            });
        }
        
        public async Task<Game> GetGameData(Guid gameId)
        {
            return await _server.HubConnection.InvokeAsync<Game>("GetGameData", gameId);
        }

        public void StartNewGame(string user1, string user2)
        {
            _server.HubConnection.InvokeAsync("StartNewGame", user1, user2);
        }

        public void DeclineGame(string user1, string user2)
        {
            _server.HubConnection.SendAsync("GameDeclined", user1, user2);
        }

        public void RequestGame(string userName)
        {
            _server.HubConnection.SendAsync("GameRequested", userName);
        }

        public void SendMove(int origin, int destination, Guid gameId)
        {
            _server.HubConnection.SendAsync("MakeMove", origin, destination, gameId);
        }

        public void SendHome(int origin, Guid gameId)
        {
            _server.HubConnection.SendAsync("SendHome", origin, gameId);
        }

        public void CloseGame(Guid gameId)
        {
            _server.HubConnection.SendAsync("CloseGame", gameId);
        }

        public void ResignGame(Guid gameId)
        {
            _server.HubConnection.SendAsync("ResignGame", gameId);
        }
    }
}
