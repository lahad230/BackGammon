using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Services;
using Backgammon.Project.UI.Views;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<IChatService, ChatService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IUserService, UserService>();
            SimpleIoc.Default.Register<IServerContextService, ServerContextService>();
            SimpleIoc.Default.Register<IServerUserService, ServerUserService>();
            SimpleIoc.Default.Register<IServerChatService, ServerChatService>();
            SimpleIoc.Default.Register<IServerGameService, ServerGameService>();
            SimpleIoc.Default.Register<IGameService, GameService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LobbyViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ChatViewModel>();
            SimpleIoc.Default.Register<GameViewModel>();
        }

        public MainViewModel Main { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }

        public LobbyViewModel Lobby { get { return SimpleIoc.Default.GetInstance<LobbyViewModel>(); } }

        public LoginViewModel Login { get { return SimpleIoc.Default.GetInstance<LoginViewModel>(); } }

        public ChatViewModel Chat { get { return SimpleIoc.Default.GetInstance<ChatViewModel>(Guid.NewGuid().ToString()); } }

        public GameViewModel Game { get { return SimpleIoc.Default.GetInstance<GameViewModel>(Guid.NewGuid().ToString()); } }
    }
}
