using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Infra
{
    public interface IUserService
    {
        User CurrentUser { get; set; }

        string Recipient { get; set; }

        Guid CurrentGameId { get; set; }
    }
}
