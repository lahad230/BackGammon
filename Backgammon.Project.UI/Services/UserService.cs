using Backgammon.Project.UI.Infra;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Services
{
    public class UserService : IUserService
    {
        public User CurrentUser { get; set; }

        public string Recipient { get; set; }

        public Guid CurrentGameId { get; set; }
    }
}
