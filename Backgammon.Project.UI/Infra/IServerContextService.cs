using Microsoft.AspNetCore.SignalR.Client;
using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon.Project.UI.Infra
{
    public interface IServerContextService
    {
        HubConnection HubConnection { get; set; }
     }   
}
