using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Utilities.Api;

namespace Turnable.Tiled.Api
{
    public interface ITileCollection
    {
        // Methods

        // Properties
        Tile this[int x, int y] { get; set; }
        Tile this[ICoordinates coordinates] { get; set; }
        int Count { get; }
        void Remove(ICoordinates coordinates);

        // Events
    }
}
