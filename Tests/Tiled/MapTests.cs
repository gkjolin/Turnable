using NUnit.Framework;
using Turnable.Tiled;
using Turnable.Tiled.Api;

namespace Tests.Tiled
{
    [TestFixture]
    public class MapTests
    {
        public void Constructor_SuccessfullyCreatesANewMap()
        {
            IMap map = new Map();

            Assert.That(map.Version, Is.Null);
            Assert.That(map.Orientation, Is.EqualTo(Orientation.Orthonogal));

/*
        orientation: Map orientation. Tiled supports "orthogonal", "isometric", "staggered"(since 0.9) and "hexagonal"(since 0.11).
renderorder: The order in which tiles on tile layers are rendered.Valid values are right-down(the default), right - up, left - down and left - up.In all cases, the map is drawn row - by - row. (since 0.10, but only supported for orthogonal maps at the moment)
            width: The map width in tiles.
            height: The map height in tiles.
            tilewidth: The width of a tile.
            tileheight: The height of a tile.
            hexsidelength: Only for hexagonal maps. Determines the width or height(depending on the staggered axis) of the tile's edge, in pixels.
            staggeraxis: For staggered and hexagonal maps, determines which axis("x" or "y") is staggered. (since 0.11)
            staggerindex: For staggered and hexagonal maps, determines whether the "even" or "odd" indexes along the staggered axis are shifted. (since 0.11)
backgroundcolor: The background color of the map. (since 0.9, optional, may include alpha value since 0.15 in the form #AARRGGBB)
nextobjectid: Stores the next available ID for new objects.This number is stored to prevent reuse of the same ID after objects have been removed. (since 0.11)
*/
        }
    }
}
