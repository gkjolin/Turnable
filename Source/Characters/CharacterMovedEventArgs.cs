using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Locations;

namespace Turnable.Characters
{
    public class CharacterMovedEventArgs : EventArgs
    {
        // TODO: Unit test
        public Entity Character { get; set; }
        public Movement Movement { get; set; }

        public CharacterMovedEventArgs(Entity character, Movement movement)
        {
            Character = character;
            Movement = movement;
        }
    }
}
