using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TurnItUp.Tmx;
using TurnItUp.Locations;

namespace Tests.Factories
{
    public static class LocationsFactory
    {
        public static Board BuildBoard()
        {
            Board board = new Board();
            board.Initialize("../../Fixtures/FullExample.tmx");

            return board;
        }
    }
}
