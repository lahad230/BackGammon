using Backgammon.Project.UI.Infra;
using Backgammon.Project.UI.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.UI.Services
{
    public class GameService : IGameService
    {
        private readonly IServerGameService _server;
        private List<GameWindow> _games;

        public GameService(IServerGameService server)
        {
            _server = server;
            _games = new List<GameWindow>();
        }
        public void CreateGame()
        {
            GameWindow window = new GameWindow();
            _games.Add(window);
            window.Closing += Window_Closing;
            window.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(sender is GameWindow game)
            {
                Guid gameId = new Guid(game.GameId.Text);
                _server.ResignGame(gameId);
                _games.Remove(game);
            }
        }

        public bool IsGameExists(string userName)
        {
            foreach (var win in _games)
            {
                if(win.user1.Text == userName || win.user2.Text == userName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
