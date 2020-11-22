using Backgammon.Project.UI.Infra;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Backgammon.Project.UI.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        const int WHITE_END = 25;
        const int BLACK_END = 0;
        private readonly IServerGameService _serverGame;
        private readonly IUserService _userService;

        private int _moveFrom = -2;
        private bool _isBlack;
        private bool _pickedEaten = false;
        private bool _isGameLoaded;
        private bool _gameOver;

        public string Instructions { get; set; } = "Instructions:\n" +
                                                   "You can move pieces by clicking on the place you want\n" +
                                                   "to move from followed by the place you want to go to.\n" +
                                                   "You can see how much you can move in the 'Moves' section\n" +
                                                   "To get pieces out of the board, click on the place you\n" +
                                                   "want to move from followed by clicking on the gold home\n" +
                                                   "plate.\n\n" +
                                                   "IF YOU HAVE NO LEGAL MOVE TO MAKE THE GAME WILL PASS\n" +
                                                   "THE TURN AUTOMATICALLY, SO IT MAY APPEAR AS IF A PLAYER\n" +
                                                   "GETS TWO CONSECUTIVE TURNS!!";

        public Game Game { get; set; }
        public RelayCommand<string> MoveCommand { get; set; }
        public RelayCommand<string> MoveEatenCommand { get; set; }
        public RelayCommand<string> GoHomeCommand { get; set; }
        public RelayCommand ResignCommand { get; set; }

        public GameViewModel(IServerGameService serverGame, IUserService userService)
        {
            _serverGame = serverGame;
            _userService = userService;
            _serverGame.MoveFinished += ServerGame_MoveFinished;
            MoveCommand = new RelayCommand<string>(Move, CanMove);
            MoveEatenCommand = new RelayCommand<string>(MoveEaten, CanMoveEaten);
            GoHomeCommand = new RelayCommand<string>(GoHome, CanMove);
            ResignCommand = new RelayCommand(Resign, CanResign);
            Task t = LoadGame();
        }

        private bool CanResign()
        {
            return !_gameOver;
        }

        private void Resign()
        {
            _serverGame.ResignGame(Game.GameId);
        }

        private bool CanMoveEaten(string arg)
        {
            if (!_gameOver)
            {
                if (_isGameLoaded)
                {
                    if (Game.CurrentPlayer == _userService.CurrentUser.UserName)
                    {
                        if ((_isBlack && Game.Board[BLACK_END].SoldiersQuantity > 0) ||
                            (!_isBlack && Game.Board[WHITE_END].SoldiersQuantity > 0) ||
                            _pickedEaten)
                            return true;
                    }
                }
            }
            return false;
        }

        private void ServerGame_MoveFinished(object sender, MoveMadeEventArgs e)
        {
            if (Game.GameId == e.Game.GameId)
            {
                Game = e.Game;
                if (Game.Winner == Game.Black || Game.Winner == Game.White)
                {
                    _gameOver = true;
                    MessageBox.Show($"{Game.Winner} Won!!!");
                    _serverGame.CloseGame(Game.GameId);
                }
                RaisePropertyChanged("Game");
                UpdateCommands();
            }
        }

        private bool CanMove(string arg)
        {
            if (!_gameOver)
            {
                if (_isGameLoaded)
                {
                    {
                        if (Game.CurrentPlayer == _userService.CurrentUser.UserName)
                        {
                            if ((_isBlack && Game.Board[BLACK_END].SoldiersQuantity < 1) ||
                                (!_isBlack && Game.Board[WHITE_END].SoldiersQuantity < 1) ||
                                _pickedEaten)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        private void GoHome(string arg)
        {
            if ((_moveFrom != -2) && (_moveFrom > BLACK_END || _moveFrom < WHITE_END))
            {
                _serverGame.SendHome(_moveFrom, Game.GameId);
                Mouse.OverrideCursor = Cursors.Arrow;
                _moveFrom = -2;
            }
        }

        private void MoveEaten(string origin)
        {
            if (!_pickedEaten)
            {
                if (_isBlack && Game.Board[BLACK_END].SoldiersQuantity > 0)
                {
                    UpdateEatenClicked(BLACK_END);
                }
                else if (!_isBlack && Game.Board[WHITE_END].SoldiersQuantity > 0)
                {
                    UpdateEatenClicked(WHITE_END);
                }
                UpdateCommands();
            }
            else
            {
                if (_isBlack)
                {
                    Game.Board[WHITE_END].SoldiersQuantity++;
                }
                else
                {
                    Game.Board[BLACK_END].SoldiersQuantity++;
                }
                _moveFrom = -2;
                _pickedEaten = false;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void Move(string location)
        {
            _pickedEaten = false;
            int place = int.Parse(location);
            if (_moveFrom == -2)
            {
                if (_isBlack == Game.Board[place].Color && Game.Board[place].SoldiersQuantity > 0)
                {
                    _moveFrom = place;
                    Game.Board[place].SoldiersQuantity--;
                    Mouse.OverrideCursor = Cursors.Hand;
                    RaisePropertyChanged("Game");
                    return;
                }
            }
            else
            {
                if (_moveFrom == place)
                {
                    _moveFrom = -2;
                    Game.Board[place].SoldiersQuantity++;
                    Mouse.OverrideCursor = Cursors.Arrow;
                    RaisePropertyChanged("Game");
                    return;
                }
                else
                {
                    if (CheckMove(_isBlack, place))
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        _serverGame.SendMove(_moveFrom, place, Game.GameId);
                        _moveFrom = -2;
                    }
                }
            }
        }

        private async Task LoadGame()
        {
            Game = await _serverGame.GetGameData(_userService.CurrentGameId);
            if (_userService.CurrentUser.UserName == Game.White)
            {
                _isBlack = false;
            }
            else
            {
                _isBlack = true;
            }
            RaisePropertyChanged("Game");
            _isGameLoaded = true;
            UpdateCommands();
        }

        private bool CheckMove(bool color, int place)
        {
            if ((color && Game.DieActions.Contains(place - _moveFrom)) ||
                (!color && Game.DieActions.Contains(_moveFrom - place)))
            {
                if (Game.Board[place].Color == color)
                {
                    return true;
                }

                if (Game.Board[place].SoldiersQuantity == 1 ||
                    Game.Board[place].SoldiersQuantity == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateEatenClicked(int location)
        {
            _moveFrom = location;
            _pickedEaten = true;
            Game.Board[location].SoldiersQuantity--;
            Mouse.OverrideCursor = Cursors.Hand;
            RaisePropertyChanged("Game");
        }

        private void UpdateCommands()
        {
            MoveCommand.RaiseCanExecuteChanged();
            MoveEatenCommand.RaiseCanExecuteChanged();
            GoHomeCommand.RaiseCanExecuteChanged();
            ResignCommand.RaiseCanExecuteChanged();
        }
    }
}
