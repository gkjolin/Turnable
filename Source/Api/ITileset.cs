using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface ITileset
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        List<SpecialTile> FindSpecialTiles(string propertyName, string propertyValue);

        // -----------------
        // Private interface
        // -----------------
    }
}
