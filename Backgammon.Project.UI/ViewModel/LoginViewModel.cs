using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IServerUserService _serverUser;
        private INavigationService _nav;
        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }
        public LoginViewModel(IServerUserService serverUser, IUserService userService, INavigationService nav)
        {
            _nav = nav;
            _userService = userService;
            _serverUser = serverUser;
            RegisterUserCommand = new RelayCommand(RegisterUser, CanRegisterUser);
            LoginCommand = new RelayCommand(Login, CanRegisterUser);            
        }


        private string _error;

        public string Error
        {
            get { return _error; }
            set 
            { 
                _error = value;
                RaisePropertyChanged();
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value;
                RegisterUserCommand.RaiseCanExecuteChanged();
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RegisterUserCommand.RaiseCanExecuteChanged();
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanRegisterUser()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        private async void Login()
        {
            User user = await _serverUser.Login(UserName, Password);
            if (user == null)
                Error = "Login failed";
            else
            {
                LoginSuccess(user);
            }

        }

        private async void RegisterUser()
        {
            if (Password.Length < 4)
            {
                Error = "Password must be 4 characters or more. ";
            }
            else
            {
                User user = new User { UserName = this.UserName, Password = this.Password };
                Error = await _serverUser.Register(user);
                if(Error == string.Empty)
                {
                    LoginSuccess(user);
                }
            }
        }  
        
        private void LoginSuccess(User user)
        {
            Error = string.Empty;
            _userService.CurrentUser = user;
            _nav.Navigate(NavigationEvent.LocalLogin);
        }
    }
}
