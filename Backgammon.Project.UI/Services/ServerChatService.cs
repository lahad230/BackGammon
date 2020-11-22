using Backgammon.Project.UI.Infra;
using Microsoft.AspNetCore.SignalR.Client;
using SharedAssets.EventsArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Services
{
    public class ServerChatService : IServerChatService
    {
        private readonly IServerContextService _server;

        public event EventHandler<UserActionEventArgs> MessageReceivedCheck;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public ServerChatService(IServerContextService server)
        {
            _server = server;
            RegisterToService();
        }

        private void RegisterToService()
        {
            _server.HubConnection.On("broadcastMessage", async (string nameReceived, string messageReceived) =>
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        MessageReceivedCheck?.Invoke(this, new UserActionEventArgs(nameReceived));
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(nameReceived, messageReceived));
                    });
            });
        }

        public void SendMessage(string message, string toUser)
        {
            _server.HubConnection.SendAsync("SendMessageTo", message, toUser);
        }
    }
}
