using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using System.IO;

namespace Tests.Tiled
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        public void Constructor_InitializesAllProperties()
        {
            Tile tile = new Tile(32, 1, 2);

            Assert.That((uint)32, tile.GlobalId);
            Assert.That(tile.X, 1);
            Assert.That(tile.Y, 2);
        }
    }
}
