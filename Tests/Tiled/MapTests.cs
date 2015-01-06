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
        public void Constructor_GivenAPathToAMinimalBase64GzippedTmxFile_CorrectlyInitializesAllProperties()
        {
            Map map = new Map("../../Fixtures/MinimalBase64GzipCompressed.tmx");

            Assert.AreEqual(24, map.TileWidth);
            Assert.AreEqual(24, map.TileHeight);
            Assert.AreEqual(15, map.Width);
            Assert.AreEqual(15, map.Height);
            Assert.AreEqual(RenderOrder.RightDown, map.RenderOrder);
            Assert.AreEqual(Orientation.Orthogonal, map.Orientation);
            Assert.AreEqual("1.0", map.Version);

            // Make sure that the layers are loaded up
            Assert.AreEqual(1, map.Layers.Count);
            Assert.IsInstanceOfType(map.Layers, typeof(ElementList<Layer>));
            Assert.AreEqual(0, map.Layers[0].Tiles.Count);
        }

        // Special layer tests
        [TestMethod]
        public void Map_SpecialLayerEnum_Defines4DifferentSpecialLayers()
        {
            Assert.AreEqual(4, Enum.GetValues(typeof(Map.SpecialLayer)).Length);
            Assert.IsTrue(Enum.IsDefined(typeof(Map.SpecialLayer), "Background"));
            Assert.IsTrue(Enum.IsDefined(typeof(Map.SpecialLayer), "Collision"));
            Assert.IsTrue(Enum.IsDefined(typeof(Map.SpecialLayer), "Object"));
            Assert.IsTrue(Enum.IsDefined(typeof(Map.SpecialLayer), "Character"));
        }

        [TestMethod]
        public void SetSpecialLayer_SetsTheCorrectPropertyForTheLayer()
        {
            var values = Enum.GetValues(typeof(Map.SpecialLayer)).Cast<Map.SpecialLayer>();

            foreach (Map.SpecialLayer value in Enum.GetValues(typeof(Map.SpecialLayer)).Cast<Map.SpecialLayer>())
            {
                _fullMap.SetSpecialLayer(_fullMap.Layers[0], value);
                Assert.AreEqual("true", _fullMap.Layers[0].Properties["Is" + value.ToString() + "Layer"]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSpecialLayer_WhenTheSpecificSpecialLayerAlreadyExists_ThrowsAnException()
        {
            _fullMap.SetSpecialLayer(_fullMap.Layers[0], Map.SpecialLayer.Background);
            _fullMap.SetSpecialLayer(_fullMap.Layers[1], Map.SpecialLayer.Background);
        }

        [TestMethod]
        public void GetSpecialLayer_WhenASpecialLayerExists_ReturnsTheLayer()
        {
            _fullMap.SetSpecialLayer(_fullMap.Layers[0], Map.SpecialLayer.Background);

            Assert.AreEqual(_fullMap.Layers[0], _fullMap.GetSpecialLayer(Map.SpecialLayer.Background));
        }

        [TestMethod]
        public void GetSpecialLayer_WhenASpecialLayerDoesNotExist_ReturnsNull()
        {
            _fullMap.SetSpecialLayer(_fullMap.Layers[0], Map.SpecialLayer.Character);

            Assert.IsNull(_fullMap.GetSpecialLayer(Map.SpecialLayer.Background));
        }
    }
}
