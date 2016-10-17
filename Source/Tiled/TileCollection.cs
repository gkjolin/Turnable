using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Tiled.Api;
using Turnable.Utilities;
using Turnable.Utilities.Api;

namespace Turnable.Tiled
{
    public class TileCollection : Dictionary<ICoordinates, Tile>, ITileCollection
    {
        public TileCollection()
        {
        }

/*
        public TileCollection(int layerWidth, int layerHeight, Data data)
        {
            
        }
*/
    }
}
