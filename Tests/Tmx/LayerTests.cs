using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;
using System.Tuples;
using TurnItUp.Components;

namespace Tests.Tmx
{
    [TestClass]
    public class LayerTests
    {
        private Layer _layer;

        [TestInitialize]
        public void Initialize()
        {
            _layer = TmxFactory.BuildMap().Layers[1];
        }

        [TestMethod]
        public void Layer_ConstructionUsingMinimalTmxFile_IsSuccessful()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElement());

            Assert.AreEqual("Background", layer.Name);
            Assert.AreEqual(16, layer.Width);
            Assert.AreEqual(16, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are Tiles in the layer created?
            Assert.AreEqual(16 * 16, layer.Tiles.Count);
        }

        [TestMethod]
        public void Layer_ConstructionUsingLayerDataWithProperties_IsSuccessful()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

            Assert.AreEqual("Characters", layer.Name);
            Assert.AreEqual(16, layer.Width);
            Assert.AreEqual(16, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are Tiles in the layer created?
            Assert.AreEqual(9, layer.Tiles.Count);

            // Are the Properties for this Layer loaded?
            Assert.AreEqual(1, layer.Properties.Count);
        }

        [TestMethod]
        public void Layer_MovingATile_IsSuccessful()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

            layer.MoveTile(new Position(7, 1), new Position(7, 2));

            Assert.IsFalse(layer.Tiles.ContainsKey(new Tuple<int, int>(7, 1)));
            Assert.IsTrue(layer.Tiles.ContainsKey(new Tuple<int, int>(7, 2)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Layer_MovingANonExistantTile_Fails()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

            layer.MoveTile(new Position(7, 2), new Position(7, 3));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Layer_MovingATileToAnOccupiedPosition_Fails()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

            layer.MoveTile(new Position(7, 1), new Position(7, 14));
        }
    }
}
