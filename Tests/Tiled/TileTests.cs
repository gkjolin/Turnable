using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Turnable.Tiled;
using Turnable.Tiled.Api;
using Turnable.Utilities;

namespace Tests
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        public void Constructor_InitializesAllProperties()
        {
            ITile tile = new Tile(32, new Coordinates(1, 2));

            Assert.That(tile.GlobalId, Is.EqualTo((uint)32));
            Assert.That(tile.Location, Is.EqualTo(new Coordinates(1, 2)));
        }
    }
}
