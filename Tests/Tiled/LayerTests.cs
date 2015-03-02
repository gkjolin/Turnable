using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using Tests.Factories;
using System.Tuples;

namespace Tests.Tiled
{
    [TestClass]
    public class LayerTests
    {
        [TestMethod]
        public void Constructor_GivenALayerXElementWithNoProperties_InitializesAnEmptyPropertiesCollection()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithNoProperties());

            Assert.IsNotNull(layer.Properties);
        }
        
        [TestMethod]
        public void Constructor_GivenALayerXElement_SuccessfullyInitializesLayerIncludingPropertiesAndTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElement());

            Assert.AreEqual("Background", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are the tiles in the layer created? 
            // In the case of the fixture that we use (FullExample.tmx), the background is fully filled in, so this simple assert is enough.
            // Tiles is a sparse dictionary, so the below Assert will not always be true. However in this case, the background layer has a tile for each position.
            Assert.AreEqual(layer.Width * layer.Height, layer.Tiles.Count);

            // Are the Properties for this Layer loaded?
            Assert.AreEqual(1, layer.Properties.Count);
        }

        // ************************
        // Layer Manipulation Tests
        // ************************
        //[TestMethod]
        //public void MoveTile_MovesATileToAnEmptyPositionInTheSameLayer()
        //{
        //    Layer layer = new Layer(TiledFactory.BuildLayerXElement());

        //    int tileCount = layer.Tiles.Count;
        //    uint tileGlobalId = layer.Tiles[new Tuple<int, int>(8, 7)].Gid;

        //    layer.MoveTile(new Position(8, 7), new Position(8, 8));

        //    Assert.IsFalse(layer.Tiles.ContainsKey(new Tuple<int, int>(8, 7)));
        //    Assert.IsTrue(layer.Tiles.ContainsKey(new Tuple<int, int>(8, 8)));

        //    // Make sure that the tileCount does not change
        //    Assert.AreEqual(tileCount, layer.Tiles.Count);

        //    // Make sure that the tile data is changed as well
        //    Tile tile = layer.Tiles[new Tuple<int, int>(8, 8)];
        //    Assert.AreEqual(8, tile.X);
        //    Assert.AreEqual(8, tile.Y);
        //    Assert.AreEqual(tileGid, tile.Gid);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void MoveATile_WhenNoTileExistsAtTheLayer_MovingANonExistantTile_Fails()
        //{
        //    Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

        //    layer.MoveTile(new Position(7, 2), new Position(7, 3));
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void Layer_MovingATileToAnOccupiedPosition_Fails()
        //{
        //    Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

        //    layer.MoveTile(new Position(8, 7), new Position(7, 14));
        //}

        //[TestMethod]
        //public void Layer_SettingATile_IsSuccessful()
        //{
        //    Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

        //    layer.SetTile(new Position(7, 2), 200);
        //    layer.SetTile(new Position(7, 3), 2108);

        //    Assert.AreEqual((uint)200, layer.Tiles[new Tuple<int, int>(7, 2)].Gid);
        //    Assert.AreEqual(7, layer.Tiles[new Tuple<int, int>(7, 2)].X);
        //    Assert.AreEqual(2, layer.Tiles[new Tuple<int, int>(7, 2)].Y);
        //    Assert.AreEqual((uint)2108, layer.Tiles[new Tuple<int, int>(7, 3)].Gid);
        //    Assert.AreEqual(7, layer.Tiles[new Tuple<int, int>(7, 3)].X);
        //    Assert.AreEqual(3, layer.Tiles[new Tuple<int, int>(7, 3)].Y);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void Layer_SettingATileThatIsAlreadySet_Fails()
        //{
        //    Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties());

        //    layer.SetTile(new Position(7, 2), 2107);
        //    layer.SetTile(new Position(7, 2), 2107);
        //}
    }
}
