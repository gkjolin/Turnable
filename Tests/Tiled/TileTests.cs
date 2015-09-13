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

            Assert.That(tile.GlobalId, Is.EqualTo((uint)32));
            Assert.That(tile.X, Is.EqualTo(1));
            Assert.That(tile.Y, Is.EqualTo(2));
        }
    }
}
