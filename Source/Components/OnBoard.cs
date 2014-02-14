using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Components
{
    public class OnBoard : IComponent
    {
        public Entity Owner { get; set; }
        public IBoard Board { get; set; }

        public OnBoard() : this(null)
        {
        }

        public OnBoard(IBoard board)
        {
            Board = board;
        }
    }
}