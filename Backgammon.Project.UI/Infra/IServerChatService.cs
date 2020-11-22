using SharedAssets.EventsArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Infra
{
    public interface IServerChatService
    {
        event EventHandler<UserActionEventArgs> MessageReceivedCheck;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        void SendMessage(string message, string toUser);
    }
}
