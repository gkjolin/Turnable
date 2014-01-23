using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class MapTests
    {
        private Map _map;

        [TestInitialize]
        public void Initialize()
        {
            _map = TmxFactory.BuildMap();
        }

        [TestMethod]
        public void Map_ConstructionUsingMinimalTmxFile_IsSuccessful()
        {
            Map map = new Map("../../Fixtures/FullExample.tmx");

            // Map attributes correctly loaded?
            Assert.AreEqual("1.0", map.Version);
            Assert.AreEqual(Orientation.Orthogonal, map.Orientation);
            Assert.AreEqual(16, map.Width);
            Assert.AreEqual(16, map.Height);
            Assert.AreEqual(24, map.TileWidth);
            Assert.AreEqual(24, map.TileHeight);

            // Layers loaded with tiles and layer properties?
            Assert.AreEqual(4, map.Layers.Count);
            Assert.IsNotNull(map.Layers["Background"]);
            Assert.IsNotNull(map.Layers["Obstacles"]);
            Assert.IsNotNull(map.Layers["Obstacles"].Properties);
            Assert.AreEqual(1, map.Layers["Obstacles"].Properties.Count);

            // Tilesets loaded?
            Assert.AreEqual(2, map.Tilesets.Count);
        }

        [TestMethod]
        public void Map_GivenAPropertyNameAndValue_CanFindALayer()
        {
            Layer layer = _map.FindLayerByProperty("IsCollision", "true");

            Assert.IsNotNull(layer);
            Assert.AreEqual("true", layer.Properties["IsCollision"]);
        }

        [TestMethod]
        public void Map_GivenAPropertyNameAndValue_ReturnsNullIfLayerWithPropertyCannotBeFound()
        {
            Layer layer = _map.FindLayerByProperty("IsCollision", "false");

            Assert.IsNull(layer);
        }
    }
}
