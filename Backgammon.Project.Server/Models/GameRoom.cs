using SharedAssets.EventsArgs;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backgammon.Project.Server.Models
{
    public class GameRoom : Game
    {
        private Random _rnd;
        public GameRoom(string user1, string user2, Guid id) : base(user1, user2, id)
        {
            _rnd = new Random();
            ChooseWhoStarts();
        }

        private string ChooseWhoStarts()
        {
            if (_rnd.Next(2) == 0)
                CurrentPlayer = White;
            else
                CurrentPlayer = Black;

            RollDice();
            return CurrentPlayer;
        }

        private List<int> RollDice()
        {
            DieActions.Clear();
            List<int> results = new List<int>();
            int die1 = _rnd.Next(1, 7);
            int die2 = _rnd.Next(1, 7);

            results.Add(die1);
            results.Add(die2);

            DieActions.Add(die1);
            DieActions.Add(die2);

            if (die1 == die2)
            {
                DieActions.Add(die1);
                DieActions.Add(die2);
            }

            return results;
        }

        public void MakeMove(int origin, int destination, string madeBy)
        {
            bool Color;
            if (madeBy != CurrentPlayer)
                return;
            if (origin < 0 || destination > 24)
                return;

            Color = WhoseTurn();

            if (CheckMoveLegality(origin, destination, Color, out int action))
            {
                DieActions.Remove(action);
                Board[origin].SoldiersQuantity--;
                switch (Board[destination].SoldiersQuantity)
                {
                    case 0:
                        Board[destination].Color = Color;
                        Board[destination].SoldiersQuantity++;
                        break;
                    case 1:
                        if (Board[destination].Color != Color)
                        {
                            if (Color)
                                Board[25].SoldiersQuantity++;
                            else
                                Board[0].SoldiersQuantity++;

                            Board[destination].Color = Color;
                        }
                        else
                        {
                            Board[destination].SoldiersQuantity++;
                        }
                        break;
                    default:
                        Board[destination].SoldiersQuantity++;
                        break;
                }
            }

            if (DieActions.Count == 0 || !CanMakeMoves(WhoseTurn()))
                StartNewTurn();
        }

        private void StartNewTurn()
        {
            if (CurrentPlayer == White)
                CurrentPlayer = Black;
            else
                CurrentPlayer = White;

            RollDice();
            if (!CanMakeMoves(WhoseTurn()))
                StartNewTurn();
        }

        private bool CheckMoveLegality(int origin, int destination, bool Color, out int action)
        {
            action = default;
            if (origin < 0 || origin > 25 || destination < 1 || destination > 24)
                return false;
            bool result = false;
            if (Board[destination].Color == Color)
                result = true;

            if (Board[destination].SoldiersQuantity < 2)
                result = true;

            if (result)
            {
                if (Color)
                {
                    result = DieActions.Contains(destination - origin);
                    action = destination - origin;
                }
                else
                {
                    result = DieActions.Contains(origin - destination);
                    action = origin - destination;
                }
            }
            return result;
        }

        public void TakePieceOut(int origin, string madeBy)
        {
            bool regularMove = false;
            if (madeBy != CurrentPlayer)
            {
                return;
            }
            bool enoughAction = false;
            int lowestDie = 7;
            if (origin < 1 || origin > 24)
                return;
            bool color = WhoseTurn();
            if (CheckWinViability(color))//if all the pieces are in home you can start taking the, out.
            {

                if (color)
                {
                    //check if other moves except taking piece out is available.
                    foreach (var action in DieActions)
                    {
                        for (int i = 19; i < 25; i++)
                        {
                            if (CheckMoveLegality(i, i + action, color, out int filler))
                            {
                                regularMove = true;
                            }
                        }
                    }

                    //if you can make other moves on the board, you can tak out only pieces that are exacly
                    //die action from getting out.
                    if (regularMove)
                    {
                        foreach (var action in DieActions)
                        {
                            if (origin + action == 25)
                            {
                                enoughAction = true;
                                lowestDie = action;
                            }
                        }
                    }

                    //if you can only take piece out, you can use higher action.
                    else
                    {
                        for (int i = 19; i < 25; i++)
                        {
                            Cell cell = Board[i];
                            if (cell.Color == color && cell.SoldiersQuantity > 0 && i < origin)
                                return;
                        }
                        foreach (var action in DieActions)
                        {
                            if (origin + action > 24)
                            {
                                enoughAction = true;
                                if (action < lowestDie)
                                    lowestDie = action;
                            }
                        }
                    }
                    if (enoughAction)
                    {
                        removeFromOrigion(origin, lowestDie, color);
                    }
                }

                //same but for white.
                else
                {
                    foreach (var action in DieActions)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            if (CheckMoveLegality(i, i - action, color, out int filler))
                            {
                                regularMove = true;
                            }
                        }
                    }

                    if (regularMove)
                    {
                        foreach (var action in DieActions)
                        {
                            if (origin - action == 0)
                            {
                                enoughAction = true;
                                lowestDie = action;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 6; i > 0; i--)
                        {
                            Cell cell = Board[i];
                            if (cell.Color == color && cell.SoldiersQuantity > 0 && i > origin)
                                return;
                        }

                        foreach (var action in DieActions)
                        {
                            if (origin - action < 1)
                            {
                                enoughAction = true;
                                if (action < lowestDie)
                                    lowestDie = action;
                            }
                        }
                    }
                    if (enoughAction)
                    {
                        removeFromOrigion(origin, lowestDie, color);
                    }
                }
            }
            if (BlackOut == 15 || WhiteOut == 15)
                Winner = CurrentPlayer;
            if (DieActions.Count == 0 || !CanMakeMoves(WhoseTurn()))
                StartNewTurn();
        }

        private bool CheckWinViability(bool black)
        {
            //add here a condtion that only if there are 
            //no move in the board you van take pieces out!!
            if (black)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (Board[i].Color == black && Board[i].SoldiersQuantity > 0)
                        return false;
                }
            }
            else
            {
                for (int i = 25; i > 6; i--)
                {
                    if (Board[i].Color == black && Board[i].SoldiersQuantity > 0)
                        return false;
                }
            }
            return true;
        }

        private bool WhoseTurn()
        {
            if (CurrentPlayer == Black)
                return true;
            else
                return false;
        }

        private bool CanMakeMoves(bool color)
        {
            if (color)
            {
                if (Board[0].SoldiersQuantity > 0)
                {
                    foreach (var action in DieActions)
                    {
                        if (CheckMoveLegality(0, action, color, out int filler))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < Board.Length; i++)
                    {
                        foreach (var action in DieActions)
                        {
                            if (CheckMoveLegality(i, i + action, color, out int filler))
                            {
                                return true;
                            }
                        }
                    }
                    if (CheckWinViability(color))
                    {
                        for (int i = 19; i < 25; i++)
                        {
                            foreach (var action in DieActions)
                            {
                                if (i + action > 24)
                                    return true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (Board[25].SoldiersQuantity > 0)
                {
                    foreach (var action in DieActions)
                    {
                        if (CheckMoveLegality(25, 25 - action, color, out int filler))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    for (int i = Board.Length - 2; i > 0; i--)
                    {
                        foreach (var action in DieActions)
                        {
                            if (CheckMoveLegality(i, i - action, color, out int filler))
                            {
                                return true;
                            }
                        }
                    }
                    if (CheckWinViability(color))
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            foreach (var action in DieActions)
                            {
                                if (i - action > 1)
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void ResignGame(string player)
        {
            if (player == White)
            {
                Winner = Black;
            }
            else
            {
                Winner = White;
            }
        }

        private void removeFromOrigion(int origin, int action, bool color)
        {
            if (color)
                BlackOut++;
            else
                WhiteOut++;
            Board[origin].SoldiersQuantity--;
            DieActions.Remove(action);

        }
    }

}