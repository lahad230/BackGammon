using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAssets.Models
{
    public class Game
    {
        public Guid GameId { get; set; }

        public string White { get; set; }

        public string Black { get; set; }

        public string CurrentPlayer { get; set; }

        public Cell[] Board { get; set; }

        public List<int> DieActions { get; set; }

        public int WhiteOut { get; set; }

        public int BlackOut { get; set; }

        public string Winner { get; set; }

        public Game(string user1, string user2, Guid gameId): base()
        {
            GameId = gameId;
            White = user1;
            Black = user2;
            DieActions = new List<int>();
            Cell[] board = {new Cell(0, true),new Cell(2, true), new Cell(), new Cell(), new Cell(), new Cell(), new Cell(5, false),
                            new Cell(), new Cell(3, false), new Cell(), new Cell(), new Cell(), new Cell(5, true),
                            new Cell(5, false), new Cell(), new Cell(), new Cell(), new Cell(3, true), new Cell(),
                            new Cell(5, true), new Cell(), new Cell(), new Cell(), new Cell(), new Cell(2, false), new Cell()};
            Board = board;
        }

        public Game() { }
    }
}
