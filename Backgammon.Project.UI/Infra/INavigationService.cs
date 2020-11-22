using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Infra
{
    public enum NavigationEvent
    {
        LocalLogin,
        LocalLogout
    }
    public interface INavigationService
    {
        void Navigate(NavigationEvent NavEvent);

        public event EventHandler<EventArgs> LocalLogin;
        public event EventHandler<EventArgs> LocalLogout;
    }
}
