using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAssets.EventsArgs
{
    public class MoveMadeEventArgs : EventArgs
    {
        public Game Game { get; set; }

        public MoveMadeEventArgs(Game game) : base()
        {
            Game = game;
        }
    }
}
