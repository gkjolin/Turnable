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

namespace TurnItUp.Stats
{
    // TODO: Test this class
    public class StatChangedEventArgs : EntityEventArgs
    {
        public Stat Stat { get; private set; }

        public StatChangedEventArgs(Entity character, Stat stat) : base(character)
        {
            Stat = stat;
        }
    }
}
