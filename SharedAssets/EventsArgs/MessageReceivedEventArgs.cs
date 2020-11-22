using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAssets.EventsArgs
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string FromUser { get; set; }

        public string MessageContent { get; set; }

        public MessageReceivedEventArgs(string user, string message) : base()
        {
            FromUser = user;

            MessageContent = message;
        }
    }
}
