using Backgammon.Project.UI.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Services
{
    public class NavigationService : INavigationService
    {        
        public event EventHandler<EventArgs> LocalLogin;
        public event EventHandler<EventArgs> LocalLogout;

        public void Navigate(NavigationEvent NavEvent)
        {
            switch (NavEvent)
            {
                case NavigationEvent.LocalLogin:
                    LocalLogin?.Invoke(this, new EventArgs());
                    break;
                case NavigationEvent.LocalLogout:
                    LocalLogout?.Invoke(this, new EventArgs());
                    break;
                default:
                    break;
            }
        }
    }
}
