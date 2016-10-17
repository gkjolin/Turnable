using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Tiled.Api;
using Turnable.Utilities;

namespace Turnable.Tiled
{
    public class Tile : ITile
    {
        public uint GlobalId { get; set; }
        public Coordinates Location { get; set; }

        public Tile(uint globalId, Coordinates coordinates)
        {
            GlobalId = globalId;
            Location = coordinates;
        }
    }
}
