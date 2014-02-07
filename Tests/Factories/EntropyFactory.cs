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
    public static class EntropyFactory
    {
        public static Entity BuildEntity()
        {
            World world = new World();

            return world.CreateEntity();
        }
    }
}
