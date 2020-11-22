using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Infra
{
    public interface IGameService
    {
        void CreateGame();

        bool IsGameExists(string userName);
    }
}
