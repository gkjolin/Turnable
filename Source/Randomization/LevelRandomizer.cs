using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;

namespace TurnItUp.Randomization
{
    public class LevelRandomizer
    {
        public Level Level { get; set; }

        public LevelRandomizer(Level level)
        {
            Level = level;
        }
    }
}
