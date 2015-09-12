using System;
using NUnit.Framework;
using Turnable.Tiled;
using System.Linq;
using Moq;
using Turnable.Api;

namespace Tests.Tiled
{
    [TestFixture]
    public class MapTests
    {
        private Map _fullMap;

        [SetUp]
        public void Initialize()
        {
            _fullMap = new Map("../../Fixtures/FullExample.tmx");
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            Map map = new Map();

            Assert.That(0, map.TileWidth);
            Assert.That(0, map.TileHeight);
            Assert.That(0, map.Width);
            Assert.That(0, map.Height);
            Assert.That(RenderOrder.RightDown, map.RenderOrder);
            Assert.That(Orientation.Orthogonal, map.Orientation);
            Assert.IsNotNull(map.Layers);
            Assert.IsNull(map.Version);
        }

        [Test]
        public void Constructor_GivenAPathToAFullExample_CorrectlyInitializesAllProperties()
        {
            Map map = new Map("../../Fixtures/FullExample.tmx");

            Assert.That(24, map.TileWidth);
            Assert.That(24, map.TileHeight);
            Assert.That(15, map.Width);
            Assert.That(15, map.Height);
            Assert.That(RenderOrder.RightDown, map.RenderOrder);
            Assert.That(Orientation.Orthogonal, map.Orientation);
            Assert.That("1.0", map.Version);

            // Make sure that the layers are loaded up. We really only need to test that all the layers have been initialized with the correct constructor. The actual unit tests for the layer construction in LayerTests is where all the heavy lifting is done. However there is no way (that I know of) to use Moq to verify that the constructor was correctly called for the layer.
            Assert.That(4, map.Layers.Count);
            Assert.IsInstanceOfType(map.Layers, typeof(ElementList<Layer>));
            Assert.AreNotEqual(0, map.Layers[0].Tiles.Count);
            Assert.That(1, map.Layers[0].Properties.Count);

            // Tilesets loaded?
            Assert.That(2, map.Tilesets.Count);
        }
    }
}
