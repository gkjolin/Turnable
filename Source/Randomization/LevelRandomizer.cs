using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Randomization
{
    public class LevelRandomizer
    {
        public Level Level { get; set; }

        public LevelRandomizer(Level level)
        {
            Level = level;
        }

        public TileList BuildRandomTileList(Layer targetLayer, int countOfRandomTiles)
        {
            TileList returnValue = new TileList(
        }
    }
}
