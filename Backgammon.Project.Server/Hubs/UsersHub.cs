using Backgammon.Project.Server.Services;
using Microsoft.AspNetCore.SignalR;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backgammon.Project.Server.Hubs
{
    public class UsersHub : Hub
    {
        private readonly UserService _us;
        private readonly SessionDataService _data;
        private readonly GameRoomsService _gameService;

        public UsersHub(UserService us, SessionDataService data, GameRoomsService gameService)
        {
            _us = us;
            _data = data;
            _gameService = gameService;
        }

        //db funcs
        public async Task<string> Register(User user)
        {
            return await _us.RegisterAsync(user, Context.ConnectionId);
        }
        public async Task<User> Login(string userName, string password)
        {
            return await _us.Login(userName, password, Context.ConnectionId);
        }

        public void Logout(string userName)
        {
            try
            {
                _data.RemoveUser(userName);
            }
            catch { }
        }

        //session funcs
        public IEnumerable<string> GetAllUserNames()
        {
            return _data.UsersOnline.Keys;
        }

        public Game GetGameData(Guid gameId)
        {
            try
            {
                return _gameService.Games[gameId];
            }
            catch { return null; }
        }

        public void GameRequested(string userName)
        {
            try
            {
                Clients.Client(_data.UsersOnline[userName]).SendAsync("GameRequested", _data.UsersConnections[Context.ConnectionId]);
            }
            catch { }
        }

        public void StartNewGame(string user1, string user2)
        {
            try
            {
                var id = _gameService.CreateRoom(user1, user2).GameId;
                Clients.Client(_data.UsersOnline[user1]).SendAsync("GameAccepted", id, user2);
                Clients.Client(_data.UsersOnline[user2]).SendAsync("GameAccepted", id, user1);
            }
            catch { }
        }

        public void GameDeclined(string user1, string user2)
        {
            try
            {
                Clients.Client(_data.UsersOnline[user1]).SendAsync("GameDeclined", user2);
            }
            catch { }
        }

        public void MakeMove(int origin, int destination, Guid gameId)
        {
            try
            {
                var game = _gameService.Games[gameId];
                game.MakeMove(origin, destination, _data.UsersConnections[Context.ConnectionId]);
                Clients.Client(_data.UsersOnline[game.White]).SendAsync("MoveMade", game);
                Clients.Client(_data.UsersOnline[game.Black]).SendAsync("MoveMade", game);
            }
            catch { }
        }

        public void SendHome(int origin, Guid gameId)
        {
            try
            {
                var game = _gameService.Games[gameId];
                game.TakePieceOut(origin, _data.UsersConnections[Context.ConnectionId]);
                Clients.Client(_data.UsersOnline[game.White]).SendAsync("MoveMade", game);
                Clients.Client(_data.UsersOnline[game.Black]).SendAsync("MoveMade", game);
            }
            catch { }
        }

        public void CloseGame(Guid gameId)
        {
            try
            {
                _gameService.Games.Remove(gameId);
            }
            catch { }
        }

        public void ResignGame(Guid gameId)
        {
            try
            {
                var game = _gameService.Games[gameId];
                game.ResignGame(_data.UsersConnections[Context.ConnectionId]);
                Clients.Client(_data.UsersOnline[game.White]).SendAsync("MoveMade", game);
                Clients.Client(_data.UsersOnline[game.Black]).SendAsync("MoveMade", game);
            }
            catch { }
        }

        //clients notification funcs
        public void LogInCompletedNotifcation(string userName)
        {
            Clients.Others.SendAsync("LogInNotificated", userName);
        }

        public void LogOutCompletedNotifcation(string userName)
        {
            Clients.Others.SendAsync("LogOutNotificated", userName);
        }

        public void SendMessageTo(string message, string toUser)
        {
            try
            {
                // Call the broadcastMessage method to update clients.
                Clients.Client(_data.UsersOnline[toUser]).SendAsync("broadcastMessage", _data.UsersConnections[this.Context.ConnectionId], message);
            }
            catch { }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                _data.RemoveUser(_data.UsersConnections[Context.ConnectionId]);
            }
            catch { }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
