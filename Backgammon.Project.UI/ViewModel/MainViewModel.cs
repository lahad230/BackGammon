using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Services;
using Backgammon.Project.UI.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Backgammon.Project.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IUserService _userService;
        private IServerUserService _server;
        private INavigationService _nav;
        private UserControl _currentWindow;

        public UserControl CurrentWindow { get => _currentWindow; set { Set(ref _currentWindow, value); } }

        //This property is here for debuging!!!
        //public UserControl PreLoadedLobby { get; set; }

        public MainViewModel(IServerUserService server, INavigationService nav, IUserService userService)
        {
            _userService = userService;
            _nav = nav;
            _server = server;
            _nav.LocalLogin += _nav_LocalLogin;
            _nav.LocalLogout += _nav_LocalLogout;
            Application.Current.MainWindow.Closing += MainWindow_Closing;
            CurrentWindow = new LoginView();
            //PreLoadedLobby = new LobbyView();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            for (int i = Application.Current.Windows.Count -1; i > 1; i--)
            {
                Application.Current.Windows[i].Close();
            }

            if(_userService.CurrentUser != null)
            {
                Task.Run(() => _server.Logout(_userService.CurrentUser.UserName));                
            }
        }

        private void _nav_LocalLogout(object sender, EventArgs e)
        {
            CurrentWindow = new LoginView();
        }

        private void _nav_LocalLogin(object sender, EventArgs e)
        {
            CurrentWindow = new LobbyView();
        }
    }
}
