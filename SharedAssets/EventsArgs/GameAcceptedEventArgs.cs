using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAssets.EventsArgs
{
    public class GameAcceptedEventArgs : EventArgs
    {
        public Guid GameId { get; set; }

        public string FromUser { get; set; }

        public GameAcceptedEventArgs(Guid gameId, string user) : base()
        {
            GameId = gameId;
            FromUser = user;
        }
    }
}
