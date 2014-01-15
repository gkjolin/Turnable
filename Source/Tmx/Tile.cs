using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class Tile
    {
        public uint Gid { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Tile(uint gid, int x, int y)
        {
            Gid = gid;
            X = x;
            Y = y;
        }
    }
}
