using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Services;
using Backgammon.Project.UI.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Backgammon.Project.UI.ViewModel
{
    public class LobbyViewModel : ViewModelBase
    {
        private INavigationService _nav;
        private readonly IUserService _userService;
        private readonly IServerUserService _serverUser;
        private readonly IServerChatService _serverChat;
        private readonly IServerGameService _serverGame;
        private readonly IChatService _chatService;
        private readonly IGameService _gameService;

        private List<string> _gameRequests;
        public ObservableCollection<string> Users { get; set; }
        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand<string> StartChatCommand { get; set; }

        public RelayCommand<string> RequestGameCommand { get; set; }

        public LobbyViewModel(IServerGameService serverGame, IServerChatService serverChat, IServerUserService serverUser, IUserService userService, INavigationService nav, IChatService chatService, IGameService gameService)
        {
            _serverUser = serverUser;
            _serverChat = serverChat;
            _serverGame = serverGame;
            _userService = userService;
            _nav = nav;
            _chatService = chatService;
            _gameService = gameService;
            _serverUser.LogInCompleted += Server_LogInCompleted;
            _serverUser.LogOutCompleted += Server_LogOutCompleted;
            _serverChat.MessageReceivedCheck += Server_MessageReceivedCheck;
            _serverGame.GameRequested += Server_GameRequested;
            _serverGame.GameAccepted += Server_GameAccepted;
            _serverGame.GameDeclined += Server_GameDeclined;
            _gameRequests = new List<string>();
            LogOutCommand = new RelayCommand(LogOut);
            StartChatCommand = new RelayCommand<string>(StartChat);
            RequestGameCommand = new RelayCommand<string>(RequestGame);
            Task t = LoadUsers();
        }

        private void Server_GameDeclined(object sender, UserActionEventArgs e)
        {
            MessageBox.Show($"{e.UserName} Declined your game request");
            _gameRequests.Remove(e.UserName);
        }

        private void Server_GameAccepted(object sender, GameAcceptedEventArgs e)
        {
            _gameRequests.Remove(e.FromUser);
            _userService.CurrentGameId = e.GameId;
            _gameService.CreateGame();
        }

        private void Server_GameRequested(object sender, UserActionEventArgs e)
        {
            _gameRequests.Add(e.UserName);
            var result = MessageBox.Show($"{e.UserName} requested a game with you, click yes to start game, click no to decline",
                "Game request", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _gameRequests.Remove(e.UserName);
                _serverGame.StartNewGame(_userService.CurrentUser.UserName, e.UserName);
            }
            else
            {
                _gameRequests.Remove(e.UserName);
                _serverGame.DeclineGame(e.UserName, _userService.CurrentUser.UserName);
            }            
        }

        private void RequestGame(string userName)
        {
            if (!_gameRequests.Contains(userName))
            {
                if (!_gameService.IsGameExists(userName))
                {
                    _serverGame.RequestGame(userName);
                    _gameRequests.Add(userName);
                }
                else
                {
                    MessageBox.Show($"You already have an open game window with {userName}");
                }
            }
        }

        private void Server_MessageReceivedCheck(object sender, UserActionEventArgs e)
        {
            if (!_chatService.IsChatExists(e.UserName))
            {
                _userService.Recipient = e.UserName;
                _chatService.CreateChat();
            }
        }

        private void StartChat(string userName)
        {
            if (!_chatService.IsChatExists(userName))
            {
                _userService.Recipient = userName;
                _chatService.CreateChat();
            }
            else
            {
                MessageBox.Show($"You already have an open chat window with {userName}");
            }
        }

        private void LogOut()
        {
            _serverUser.Logout(_userService.CurrentUser.UserName);
            _nav.Navigate(NavigationEvent.LocalLogout);
        }

        private async Task LoadUsers()
        {
            //Load all existing users before startup
            //dont load current user
            IEnumerable<string> userList = await _serverUser.GetAllUsers();
            Users = new ObservableCollection<string>(userList);
            Users.Remove(_userService.CurrentUser.UserName);
            RaisePropertyChanged("Users");
        }

        private void Server_LogInCompleted(object sender, UserActionEventArgs e)
        {
            if (_userService.CurrentUser.UserName != e.UserName)
            {
                Users.Add(e.UserName);
                RaisePropertyChanged("Users");
            }
        }

        private void Server_LogOutCompleted(object sender, UserActionEventArgs e)
        {
            Users.Remove(e.UserName);
            RaisePropertyChanged("Users");
        }
    }
}
