using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharedAssets.EventsArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Backgammon.Project.UI.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly IServerChatService _serverChat;
        private readonly IUserService _userService;
        public string Recipient { get; set; }

        public ObservableCollection<string> Messages { get; set; }

        public string YourMessage { get; set; }

        public RelayCommand SendMessageCommand { get; set; }
     
        public ChatViewModel(IServerChatService serverChat, IUserService userService)
        {
            _serverChat = serverChat;
            _userService = userService;
            Recipient = _userService.Recipient;
            _serverChat.MessageReceived += Server_MessageReceived;
            SendMessageCommand = new RelayCommand(SendMessage);
            Messages = new ObservableCollection<string>();
        }

        private void Server_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if(e.FromUser == Recipient)
            {
                Messages.Add($"{e.FromUser}: {e.MessageContent}");
            }
        }

        private void SendMessage()
        {
            _serverChat.SendMessage(YourMessage, Recipient);
            Messages.Add($"You: {YourMessage}");
            YourMessage = string.Empty;
            RaisePropertyChanged("YourMessage");
        }
    }
}
