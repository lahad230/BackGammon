using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon.Project.UI.Infra
{
    public interface IServerGameService
    {
        event EventHandler<UserActionEventArgs> GameRequested;
        event EventHandler<GameAcceptedEventArgs> GameAccepted;
        event EventHandler<UserActionEventArgs> GameDeclined;
        event EventHandler<MoveMadeEventArgs> MoveFinished;

        Task<Game> GetGameData(Guid gameId);

        void StartNewGame(string user1, string user2);

        void DeclineGame(string user1, string user2);

        void RequestGame(string userName);

        void SendMove(int origin, int destination, Guid gameId);

        void SendHome(int origin, Guid gameId);

        void CloseGame(Guid gameId);

        void ResignGame(Guid gameId);
    }
}
