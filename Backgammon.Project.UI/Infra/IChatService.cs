using Backgammon.Project.UI.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Infra
{
    public interface IChatService
    {
        void CreateChat();

        bool IsChatExists(string userName);
    }
}
