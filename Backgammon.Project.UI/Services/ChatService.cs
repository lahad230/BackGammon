using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Backgammon.Project.UI.Services
{
    public class ChatService : IChatService
    {
        private List<ChatWindow> _openChats;

        public ChatService()
        {
            _openChats = new List<ChatWindow>();
        }

        public bool IsChatExists(string userName)
        {
            var tempChat = _openChats.FirstOrDefault((chat) => chat.title.Text == userName);
            if (tempChat == null)
                return false;
            return true;
        }

        public void CreateChat()
        {
            //????get connectionId from server using the username????
            //fix mvvm by using interface
            ChatWindow window = new ChatWindow();
            window.Closing += Window_Closing;
            _openChats.Add(window);
            window.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (sender is ChatWindow chat)
            {
                _openChats.Remove(chat);
            }
        }
    }
}
