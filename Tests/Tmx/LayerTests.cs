using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;
using System.Tuples;

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
            Layer layer = new Layer(TmxFactory.BuildLayerXElement(), 15, 15);

            Assert.AreEqual("Background", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are Tiles in the layer created?
            Assert.AreEqual(15 * 15, layer.Tiles.Count);
        }

        [TestMethod]
        public void Layer_ConstructionUsingLayerDataWithProperties_IsSuccessful()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties(), 15, 15);

            Assert.AreEqual("Characters", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are Tiles in the layer created?
            Assert.AreEqual(8, layer.Tiles.Count);

            // Are the Properties for this Layer loaded?
            Assert.AreEqual(1, layer.Properties.Count);
        }

        //[TestMethod]
        //public void Layer_MovingATileToAnEmptyLocation_ChangesThePositionOfTheTile()
        //{
        //    uint tileGid = _layer.Tiles[new Tuple<int, int>(0, 0)].Gid;

        //    _layer.MoveTile(0, 0, 1, 1);

        //    Assert.IsFalse(_layer.Tiles.ContainsKey(new Tuple<int, int>(0, 0)));
        //    Assert.IsTrue(_layer.Tiles.ContainsKey(new Tuple<int, int>(1, 1)));
        //    Assert.AreEqual(tileGid, _layer.Tiles[new Tuple<int, int>(1, 1)].Gid);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void Layer_MovingATileToANotEmptyLocation_ThrowsAnException()
        //{
        //    uint tileGid = _layer.Tiles[new Tuple<int, int>(0, 0)].Gid;

        //    _layer.MoveTile(0, 0, 0, 1);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void Layer_MovingAnEmptyTile_ThrowsAnException()
        //{
        //    uint tileGid = _layer.Tiles[new Tuple<int, int>(0, 0)].Gid;

        //    _layer.MoveTile(1, 1, 0, 0);
        //}

        //[TestMethod]
        //public void Layer_UnsettingATile_RemovesTheTileFromTheLayersTilesCollection()
        //{
        //    _layer.UnsetTile(0, 0);

        //    Assert.IsFalse(_layer.Tiles.ContainsKey(new Tuple<int, int>(0, 0)));
        //}

        //[TestMethod]
        //public void Layer_UnsettingATileThatNoLongerExists_DoesNotThrowAnException()
        //{
        //    _layer.UnsetTile(0, 0);
        //    _layer.UnsetTile(0, 0);
        //}
    }
}
