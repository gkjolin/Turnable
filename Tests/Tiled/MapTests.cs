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

            Assert.That(map.TileWidth, Is.EqualTo(0));
            Assert.That(map.TileHeight, Is.EqualTo(0));
            Assert.That(map.Width, Is.EqualTo(0));
            Assert.That(map.Height, Is.EqualTo(0));
            Assert.That(map.RenderOrder, Is.EqualTo(RenderOrder.RightDown));
            Assert.That(map.Orientation, Is.EqualTo(Orientation.Orthogonal));
            Assert.That(map.Layers, Is.Not.Null);
            Assert.That(map.Version, Is.Null);
        }

        [Test]
        public void Constructor_GivenAPathToAFullExample_CorrectlyInitializesAllProperties()
        {
            Map map = new Map("../../Fixtures/FullExample.tmx");

            Assert.That(map.TileWidth, Is.EqualTo(24));
            Assert.That(map.TileHeight, Is.EqualTo(24));
            Assert.That(map.Width, Is.EqualTo(15));
            Assert.That(map.Height, Is.EqualTo(15));
            Assert.That(map.RenderOrder, Is.EqualTo(RenderOrder.RightDown));
            Assert.That(map.Orientation, Is.EqualTo(Orientation.Orthogonal));
            Assert.That(map.Version, Is.EqualTo("1.0"));

            // Make sure that the layers are loaded up. We really only need to test that all the layers have been initialized with the correct constructor. The actual unit tests for the layer construction in LayerTests is where all the heavy lifting is done. However there is no way (that I know of) to use Moq to verify that the constructor was correctly called for the layer.
            Assert.That(map.Layers.Count, Is.EqualTo(4));
            Assert.That(map.Layers, Is.InstanceOf<ElementList<Layer>>());
            Assert.That(map.Layers[0].Tiles.Count, Is.Not.EqualTo(0));
            Assert.That(map.Layers[0].Properties.Count, Is.EqualTo(1));

            // Tilesets loaded?
            Assert.That(map.Tilesets.Count, Is.EqualTo(2));
        }
    }
}
