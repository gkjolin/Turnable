using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using System.Linq;
using Moq;
using Turnable.Api;

namespace Tests.Tiled
{
    [TestClass]
    public class MapTests
    {
        private Map _fullMap;

        [TestInitialize]
        public void Initialize()
        {
            _fullMap = new Map("../../Fixtures/FullExample.tmx");
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Map map = new Map();

            Assert.AreEqual(0, map.TileWidth);
            Assert.AreEqual(0, map.TileHeight);
            Assert.AreEqual(0, map.Width);
            Assert.AreEqual(0, map.Height);
            Assert.AreEqual(RenderOrder.RightDown, map.RenderOrder);
            Assert.AreEqual(Orientation.Orthogonal, map.Orientation);
            Assert.IsNull(map.Version);
            Assert.IsNull(map.Layers);
        }

        [TestMethod]
        public void Constructor_GivenAPathToAFullExample_CorrectlyInitializesAllProperties()
        {
            Map map = new Map("../../Fixtures/FullExample.tmx");

            Assert.AreEqual(24, map.TileWidth);
            Assert.AreEqual(24, map.TileHeight);
            Assert.AreEqual(15, map.Width);
            Assert.AreEqual(15, map.Height);
            Assert.AreEqual(RenderOrder.RightDown, map.RenderOrder);
            Assert.AreEqual(Orientation.Orthogonal, map.Orientation);
            Assert.AreEqual("1.0", map.Version);

            // Make sure that the layers are loaded up. We really only need to test that all the layers have been initialized with the correct constructor. The actual unit tests for the layer construction in LayerTests is where all the heavy lifting is done. However there is no way (that I know of) to use Moq to verify that the constructor was correctly called for the layer.
            Assert.AreEqual(4, map.Layers.Count);
            Assert.IsInstanceOfType(map.Layers, typeof(ElementList<Layer>));
            Assert.AreNotEqual(0, map.Layers[0].Tiles.Count);
            Assert.AreEqual(1, map.Layers[0].Properties.Count);

            // Tilesets loaded?
            Assert.AreEqual(2, map.Tilesets.Count);
        }
    }
}
