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
            Assert.IsNotNull(map.SpecialLayers);
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

            // Are the special layers correctly initialized?
            Assert.IsNotNull(map.SpecialLayers[Map.SpecialLayer.Collision]);
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
        public void SpecialLayers_AllowsSettingASpecialLayerUsingTheSpecialLayerEnumAsKey()
        {
            var values = Enum.GetValues(typeof(Map.SpecialLayer)).Cast<Map.SpecialLayer>();

            foreach (Map.SpecialLayer specialLayer in Enum.GetValues(typeof(Map.SpecialLayer)).Cast<Map.SpecialLayer>())
            {
                _fullMap.SpecialLayers[specialLayer] = _fullMap.Layers[0];
                Assert.AreEqual("true", _fullMap.Layers[0].Properties["Is" + specialLayer.ToString() + "Layer"]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SpecialLayers_SettingASpecialLayerWhenTheSpecificSpecialLayerAlreadyExists_ThrowsException()
        {
            // Usually special layers requires quite a bit of processing by the framework. For example, processing the character layer sets up teams, NPCs, PCs etc. Once the processing is done for a special layer, there is no easy way currently to undo and redo processing for a new layer. We therefore throw an exception to prevent a special layer being reassigned to another layer.
            _fullMap.SpecialLayers[Map.SpecialLayer.Background] = _fullMap.Layers[0];
            _fullMap.SpecialLayers[Map.SpecialLayer.Background] = _fullMap.Layers[1];
        }

        [TestMethod]
        public void SpecialLayers_WhenASpecialLayerExists_ReturnsTheLayer()
        {
            _fullMap.SpecialLayers[Map.SpecialLayer.Background] = _fullMap.Layers[0];

            Assert.AreEqual(_fullMap.Layers[0], _fullMap.SpecialLayers[Map.SpecialLayer.Background]);
        }

        [TestMethod]
        public void SpecialLayers_WhenASpecialLayerDoesNotExist_ReturnsNull()
        {
            _fullMap.SpecialLayers[Map.SpecialLayer.Character] = _fullMap.Layers[0];

            Assert.IsNull(_fullMap.SpecialLayers[Map.SpecialLayer.Background]);
        }
    }
}
