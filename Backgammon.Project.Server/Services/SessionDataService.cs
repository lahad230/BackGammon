using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backgammon.Project.Server.Services
{
    public class SessionDataService
    {
        public Dictionary<string,string> UsersOnline { get; set; }
        public Dictionary<string, string> UsersConnections { get; set; }

        public SessionDataService()
        {
            UsersOnline = new Dictionary<string, string>();
            UsersConnections = new Dictionary<string, string>();
        }

        public void AddUser(string userName, string ConnectionId)
        {
            UsersOnline.Add(userName, ConnectionId);
            UsersConnections.Add(ConnectionId, userName);
        }

        public void RemoveUser(string userName)
        {
            UsersConnections.Remove(UsersOnline[userName]);
            UsersOnline.Remove(userName);
        }
    }
}
