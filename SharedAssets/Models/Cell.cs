using System;
using System.Collections.Generic;
using System.Text;

namespace SharedAssets.Models
{
    public class Cell
    {
        public int SoldiersQuantity { get; set; }

        public bool Color { get; set; }

        public Cell(int soldiersAmount, bool isBlack) : base()
        {
            SoldiersQuantity = soldiersAmount;
            Color = isBlack;
        }
        public Cell() { }

    }
}
