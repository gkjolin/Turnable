using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Turnable.Api;
using Turnable.Locations;

namespace Tests.Factories
{
    public static class LocationsFactory
    {
        public static ILevel BuildLevel(string tmxPath = "../../Fixtures/FullExample.tmx")
        {
            ILevel level = new Level();

            //World world = new World();

            //LevelFactory levelFactory = new LevelFactory();

            //LevelSetUpParams setUpParams = new LevelSetUpParams();
            //setUpParams.TmxPath = tmxPath;
            //setUpParams.PlayerModel = "Knight M";
            //setUpParams.PlayerX = 7;
            //setUpParams.PlayerY = 1;

            //return levelFactory.BuildLevel(world, setUpParams);
        }
    }
}
