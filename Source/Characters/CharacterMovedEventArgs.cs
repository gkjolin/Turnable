using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Characters
{
    // TODO: Test this class
    public class CharacterMovedEventArgs : EntityEventArgs
    {
        public MoveResult MoveResult { get; private set; }

        public CharacterMovedEventArgs(Entity character, MoveResult moveResult) : base(character)
        {
            MoveResult = moveResult;
        }
    }
}
