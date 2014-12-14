using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Turnable.Tiled
{
    public class Tile
    {
        public uint GlobalId { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Tile(uint globalId, int x, int y)
        {
            GlobalId = globalId;
            X = x;
            Y = y;
        }
    }
}
