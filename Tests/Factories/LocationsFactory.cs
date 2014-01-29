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
        public static Board BuildBoard()
        {
            World world = new World();
            Board board = new Board();
            board.Initialize(world, "../../Fixtures/FullExample.tmx");

            return board;
        }
    }
}
