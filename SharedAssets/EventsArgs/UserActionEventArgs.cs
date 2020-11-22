using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAssets.EventsArgs
{
    public class UserActionEventArgs : EventArgs
    {
        public string UserName { get; set; }

        public UserActionEventArgs(string userName) : base()
        {
            UserName = userName;
        }
    }
}
