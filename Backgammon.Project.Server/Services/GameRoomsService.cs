using Backgammon.Project.Server.Models;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backgammon.Project.Server.Services
{
    public class GameRoomsService
    {
        public Dictionary<Guid, GameRoom> Games { get; set; }

        public GameRoomsService()
        {
            Games = new Dictionary<Guid, GameRoom>();
        }

        public GameRoom CreateRoom(string user1, string user2)
        {
            var game = new GameRoom(user1, user2, Guid.NewGuid());
            Games.Add(game.GameId, game);
            return game;
        }

    }
}
