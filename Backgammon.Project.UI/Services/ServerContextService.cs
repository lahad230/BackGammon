using Backgammon.Project.UI.Infra;
using Microsoft.AspNetCore.SignalR.Client;
using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Backgammon.Project.UI.Services
{
    public class ServerContextService : IServerContextService
    {
        private readonly string _url = "http://localhost:49673/Users";
        public HubConnection HubConnection { get; set; }
        public ServerContextService()
        {
            ConnectToServer();
        }
        private void ConnectToServer()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(_url).Build();

            Task.Run(async () => await HubConnection.StartAsync());
        }
    }
}
