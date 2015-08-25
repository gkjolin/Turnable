using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Tiled;
using Tests.Factories;
using System.Tuples;
using Turnable.Components;
using Turnable.Utilities;

namespace Tests.Tiled
{
    [TestClass]
    public class LayerTests
    {
        [TestMethod]
        public void Constructor_GivenAWidthAndHeight_InitializesLayerCorrectly()
        {
            Layer layer = new Layer("Layer 1", 48, 64);

            Assert.AreEqual("Layer 1", layer.Name);
            Assert.AreEqual(48, layer.Width);
            Assert.AreEqual(64, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);
            Assert.IsNotNull(layer.Properties);
        }

        [TestMethod]
        public void Constructor_GivenALayerXElementWithNoProperties_InitializesAnEmptyPropertiesCollection()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithNoProperties());

            Assert.IsNotNull(layer.Properties);
            Assert.IsNotNull(layer.Tiles);
        }
        
        [TestMethod]
        public void Constructor_GivenALayerXElement_InitializesLayerIncludingPropertiesAndTiles()
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
        [TestMethod]
        public void GetTile_GivenAPositionWithNoTile_ReturnsNull()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.IsNull(layer.GetTile(new Position(7, 2)));
        }

        [TestMethod]
        public void GetTile_GivenAPositionWithATile_ReturnsTheTile()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());
            layer.SetTile(new Position(7, 2), 2107);

            Tile tile = layer.GetTile(new Position(7, 2));

            Assert.AreEqual((uint)2107, tile.GlobalId);
            Assert.AreEqual(7, tile.X);
            Assert.AreEqual(2, tile.Y);
        }

        [TestMethod]
        public void MoveTile_MovesATileToAnEmptyPositionInTheSameLayer()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            int tileCount = layer.Tiles.Count;
            uint tileGlobalId = layer.Tiles[new Tuple<int, int>(6, 1)].GlobalId;

            layer.MoveTile(new Position(6, 1), new Position(6, 2));

            Assert.IsFalse(layer.Tiles.ContainsKey(new Tuple<int, int>(6, 1)));
            Assert.IsTrue(layer.Tiles.ContainsKey(new Tuple<int, int>(6, 2)));

            // Make sure that the tileCount does not change
            Assert.AreEqual(tileCount, layer.Tiles.Count);

            // Make sure that the tile data is changed as well
            Tile tile = layer.Tiles[new Tuple<int, int>(6, 2)];
            Assert.AreEqual(6, tile.X);
            Assert.AreEqual(2, tile.Y);
            Assert.AreEqual(tileGlobalId, tile.GlobalId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveTile_WhenNoTileExistsAtThePositionInTheLayer_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.MoveTile(new Position(7, 2), new Position(7, 3));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveTile_WhenDestinationIsOccupied_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.MoveTile(new Position(6, 1), new Position(5, 13));
        }

        [TestMethod]
        public void SwapTile_SwapsTwoTilesInTheSameLayer()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            int tileCount = layer.Tiles.Count;
            uint tile1GlobalId = layer.Tiles[new Tuple<int, int>(6, 1)].GlobalId;
            uint tile2GlobalId = layer.Tiles[new Tuple<int, int>(5, 13)].GlobalId;

            layer.SwapTile(new Position(6, 1), new Position(5, 13));

            Assert.IsTrue(layer.Tiles.ContainsKey(new Tuple<int, int>(6, 1)));
            Assert.IsTrue(layer.Tiles.ContainsKey(new Tuple<int, int>(5, 13)));

            // Make sure that the tileCount does not change
            Assert.AreEqual(tileCount, layer.Tiles.Count);

            // Make sure that the tile data is changed as well
            Tile tile = layer.Tiles[new Tuple<int, int>(5, 13)];
            Assert.AreEqual(5, tile.X);
            Assert.AreEqual(13, tile.Y);
            Assert.AreEqual(tile1GlobalId, tile.GlobalId);
            tile = layer.Tiles[new Tuple<int, int>(6, 1)];
            Assert.AreEqual(6, tile.X);
            Assert.AreEqual(1, tile.Y);
            Assert.AreEqual(tile2GlobalId, tile.GlobalId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SwapTile_WhenNoTileExistsInTheFirstPosition_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SwapTile(new Position(7, 2), new Position(5, 13));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SwapTile_WhenNoTileExistsInTheSecondPosition_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SwapTile(new Position(6, 1), new Position(7, 2));
        }

        [TestMethod]
        public void SetTile_SetsATileAtAnEmptyPosition()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SetTile(new Position(7, 2), 200);

            Assert.AreEqual((uint)200, layer.Tiles[new Tuple<int, int>(7, 2)].GlobalId);
            Assert.AreEqual(7, layer.Tiles[new Tuple<int, int>(7, 2)].X);
            Assert.AreEqual(2, layer.Tiles[new Tuple<int, int>(7, 2)].Y);
        }

        [TestMethod]
        public void SetTile_WhenATileAlreadyExistsAtThePosition_Succeeds()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SetTile(new Position(7, 2), 2107);
            layer.SetTile(new Position(7, 2), 2106);

            Assert.AreEqual((uint)2106, layer.Tiles[new Tuple<int, int>(7, 2)].GlobalId);
            Assert.AreEqual(7, layer.Tiles[new Tuple<int, int>(7, 2)].X);
            Assert.AreEqual(2, layer.Tiles[new Tuple<int, int>(7, 2)].Y);
        }

        // TODO: Setting a tile outside the bounds of the Layer is illegal, write unit test for this

        [TestMethod]
        public void RemoveTile_GivenAPosition_RemovesTheTileThatExistsAtThatPosition()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SetTile(new Position(7, 2), 2107);
            int tileCount = layer.Tiles.Count;
            layer.RemoveTile(new Position(7, 2));

            Assert.IsFalse(layer.Tiles.ContainsKey(new Tuple<int, int>(7, 2)));
            Assert.AreEqual(tileCount - 1, layer.Tiles.Count);
        }

        [TestMethod]
        public void RemoveTile_GivenAPositionThatHasNoTileToRemove_FailsWithoutThrowingAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.RemoveTile(new Position(7, 2));
            layer.RemoveTile(new Position(7, 2));
        }

        [TestMethod]
        public void Fill_GivenAGlobalId_FillsTheEntireLayerWithTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.Fill(1);

            for (int col = 0; col < layer.Width; col++)
            {
                for (int row = 0; row < layer.Height; row++)
                {
                    Assert.AreEqual((uint)1, layer.Tiles[new Tuple<int, int>(col, row)].GlobalId);
                    Assert.AreEqual(col, layer.Tiles[new Tuple<int, int>(col, row)].X);
                    Assert.AreEqual(row, layer.Tiles[new Tuple<int, int>(col, row)].Y);
                }
            }
        }

        [TestMethod]
        public void Fill_GivenARectangleWithinItsBounds_FillsTheRectangleWithTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.Fill(new Rectangle(new Position(1, 1), new Position(2, 2)), (uint)1);

            for (int col = 1; col <= 2; col++)
            {
                for (int row = 1; row <= 2; row++)
                {
                    Assert.AreEqual((uint)1, layer.Tiles[new Tuple<int, int>(col, row)].GlobalId);
                    Assert.AreEqual(col, layer.Tiles[new Tuple<int, int>(col, row)].X);
                    Assert.AreEqual(row, layer.Tiles[new Tuple<int, int>(col, row)].Y);
                }
            }
        }
    }
}
