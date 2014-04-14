using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TurnItUp.Tmx;
using TurnItUp.Locations;
using Entropy;

namespace Tests.Factories
{
    public static class LocationsFactory
    {
        public static Level BuildLevel(string tmxPath = "../../Fixtures/FullExample.tmx")
        {
            World world = new World();

            // TODO: Perhaps use a LevelFactory here after one has been built?
            Level level = new Level(world);
            level.SetUpMap(tmxPath);
            level.SetUpCharacters();
            level.SetUpPathfinder();

            return level;
        }
    }
}
