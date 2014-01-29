using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Characters
{
    public class Character
    {
        public Position Position { get; private set; }
        public bool IsPlayer { get; set; }

        public Character(int x, int y) 
        {
            Position = new Position(x, y);
            IsPlayer = false;
        }
    }
}
