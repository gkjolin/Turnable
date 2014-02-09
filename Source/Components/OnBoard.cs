using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;

namespace TurnItUp.Components
{
    public class OnBoard : IComponent
    {
        public Entity Owner { get; set; }
        public Board Board { get; set; }

        public OnBoard() : this(null)
        {
        }

        public OnBoard(Board board)
        {
            Board = board;
        }
    }
}