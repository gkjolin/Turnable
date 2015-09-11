using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Tiled;

namespace Tests.Factories
{
    // Use Tiled to view the fixture levels used for testing.
    public static class LocationsFactory
    {
        public static ILevel BuildLevel(string tmxFullFilePath = "Fixtures/FullExample.tmx")
        {
            ILevel level = new Level();

            // Setup a fully populated level.
            level.Map = new Map(tmxFullFilePath);

            //World world = new World();

            //LevelFactory levelFactory = new LevelFactory();

            //LevelSetUpParams setUpParams = new LevelSetUpParams();
            //setUpParams.TmxPath = tmxPath;
            //setUpParams.PlayerModel = "Knight M";
            //setUpParams.PlayerX = 7;
            //setUpParams.PlayerY = 1;

            //return levelFactory.BuildLevel(world, setUpParams);
            return level;
        }
    }
}
