using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using Tests.Factories;
using Turnable.Components;
using Turnable.Utilities;

namespace Tests.Tiled
{
    [TestFixture]
    public class LayerTests
    {
        [Test]
        public void Constructor_GivenAWidthAndHeight_InitializesLayerCorrectly()
        {
            Layer layer = new Layer("Layer 1", 48, 64);

            Assert.That(layer.Name, Is.EqualTo("Layer 1"));
            Assert.That(layer.Width, Is.EqualTo(48));
            Assert.That(layer.Height, Is.EqualTo(64));
            Assert.That(layer.Opacity, Is.EqualTo(1.0));
            Assert.That(layer.IsVisible, Is.True);
            Assert.That(layer.Properties, Is.Not.Null);
        }

        [Test]
        public void Constructor_GivenALayerXElementWithNoProperties_InitializesAnEmptyPropertiesCollection()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithNoProperties());

            Assert.That(layer.Properties, Is.Not.Null);
            Assert.That(layer.Tiles, Is.Not.Null);
        }
        
        [Test]
        public void Constructor_GivenALayerXElement_InitializesLayerIncludingPropertiesAndTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElement());

            Assert.That(layer.Name, Is.EqualTo("Background"));
            Assert.That(layer.Width, Is.EqualTo(15));
            Assert.That(layer.Height, Is.EqualTo(15));
            Assert.That(layer.Opacity, Is.EqualTo(1.0));
            Assert.That(layer.IsVisible, Is.True);

            // Are the tiles in the layer created? 
            // In the case of the fixture that we use (FullExample.tmx), the background is fully filled in, so this simple assert is enough.
            // Tiles is a sparse dictionary, so the below Assert will not always be true. However in this case, the background layer has a tile for each position.
            Assert.That(layer.Tiles.Count, Is.EqualTo(layer.Width * layer.Height));

            // Are the Properties for this Layer loaded?
            Assert.That(layer.Properties.Count, Is.EqualTo(1));
        }

        // ************************
        // Layer Manipulation Tests
        // ************************
        [Test]
        public void IsTileAt_GivenAPositionWhereATileExists_ReturnsTrue()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());
            layer.SetTile(new Position(7, 2), 2107);

            Assert.That(layer.IsTileAt(new Position(7, 2)), Is.True);
        }

        [Test]
        public void IsTileAt_GivenAPositionWhereATileDoesNotExist_ReturnsFalse()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.That(layer.IsTileAt(new Position(7, 2)), Is.False);
        }

        [Test]
        public void GetTile_GivenAPositionWithNoTile_ReturnsNull()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.That(layer.GetTile(new Position(7, 2)), Is.Null);
        }

        [Test]
        public void GetTile_GivenAPositionWithATile_ReturnsTheTile()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());
            layer.SetTile(new Position(7, 2), 2107);

            Tile tile = layer.GetTile(new Position(7, 2));

            Assert.That(tile.GlobalId, Is.EqualTo((uint)2107));
            Assert.That(tile.X, Is.EqualTo(7));
            Assert.That(tile.Y, Is.EqualTo(2));
        }

        [Test]
        public void MoveTile_MovesATileToAnEmptyPositionInTheSameLayer()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            int tileCount = layer.Tiles.Count;
            uint tileGlobalId = layer.Tiles[new Position(6, 1)].GlobalId;

            layer.MoveTile(new Position(6, 1), new Position(6, 2));

            Assert.That(layer.IsTileAt(new Position(6, 1)), Is.False);
            Assert.That(layer.IsTileAt(new Position(6, 2)), Is.True);

            // Make sure that the tileCount does not change
            Assert.That(layer.Tiles.Count, Is.EqualTo(tileCount));

            // Make sure that the tile data is changed as well
            Tile tile = layer.Tiles[new Position(6, 2)];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)2107));
            Assert.That(tile.X, Is.EqualTo(6));
            Assert.That(tile.Y, Is.EqualTo(2));
        }

        [Test]
        public void MoveTile_WhenNoTileExistsAtThePositionInTheLayer_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.That(() => layer.MoveTile(new Position(7, 2), new Position(7, 3)), Throws.InvalidOperationException);
        }

        [Test]
        public void MoveTile_WhenDestinationIsOccupied_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.That(() => layer.MoveTile(new Position(6, 1), new Position(5, 13)), Throws.InvalidOperationException);
        }

        [Test]
        public void SwapTile_SwapsTwoTilesInTheSameLayer()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            int tileCount = layer.Tiles.Count;
            uint tile1GlobalId = layer.Tiles[new Position(6, 1)].GlobalId;
            uint tile2GlobalId = layer.Tiles[new Position(5, 13)].GlobalId;

            layer.SwapTile(new Position(6, 1), new Position(5, 13));

            Assert.That(layer.IsTileAt(new Position(6, 1)), Is.True);
            Assert.That(layer.IsTileAt(new Position(5, 13)), Is.True);

            // Make sure that the tileCount does not change
            Assert.That(layer.Tiles.Count, Is.EqualTo(tileCount));

            // Make sure that the tile data is changed as well
            Tile tile = layer.Tiles[new Position(5, 13)];
            Assert.That(tile.X, Is.EqualTo(5));
            Assert.That(tile.Y, Is.EqualTo(13));
            Assert.That(tile.GlobalId, Is.EqualTo(tile1GlobalId));
            tile = layer.Tiles[new Position(6, 1)];
            Assert.That(tile.X, Is.EqualTo(6));
            Assert.That(tile.Y, Is.EqualTo(1));
            Assert.That(tile.GlobalId, Is.EqualTo(tile2GlobalId));
        }

        [Test]
        public void SwapTile_WhenNoTileExistsInTheFirstPosition_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.That(() => layer.SwapTile(new Position(7, 2), new Position(5, 13)), Throws.InvalidOperationException);
        }

        [Test]
        public void SwapTile_WhenNoTileExistsInTheSecondPosition_ThrowsAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            Assert.That(() => layer.SwapTile(new Position(6, 1), new Position(7, 2)), Throws.InvalidOperationException);
        }

        [Test]
        public void SetTile_SetsATileAtAnEmptyPosition()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SetTile(new Position(7, 2), 200);

            Tile tile = layer.Tiles[new Position(7, 2)];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)200));
            Assert.That(tile.X, Is.EqualTo(7));
            Assert.That(tile.Y, Is.EqualTo(2));
        }

        [Test]
        public void SetTile_WhenATileAlreadyExistsAtThePosition_Succeeds()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SetTile(new Position(7, 2), 2107);
            layer.SetTile(new Position(7, 2), 2106);

            Tile tile = layer.Tiles[new Position(7, 2)];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)2106));
            Assert.That(tile.X, Is.EqualTo(7));
            Assert.That(tile.Y, Is.EqualTo(2));
        }

        // TODO: Setting a tile outside the bounds of the Layer is illegal, write unit test for this

        [Test]
        public void RemoveTile_GivenAPosition_RemovesTheTileThatExistsAtThatPosition()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.SetTile(new Position(7, 2), 2107);
            int tileCount = layer.Tiles.Count;
            layer.RemoveTile(new Position(7, 2));

            Assert.That(layer.IsTileAt(new Position(7, 2)), Is.False);
            Assert.That(layer.Tiles.Count, Is.EqualTo(tileCount - 1));
        }

        [Test]
        public void RemoveTile_GivenAPositionThatHasNoTileToRemove_DoesNotThrowAnException()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.RemoveTile(new Position(7, 2));
            Assert.That(() => layer.RemoveTile(new Position(7, 2)), Throws.Nothing);
        }

        [Test]
        public void Fill_GivenAGlobalId_FillsTheEntireLayerWithTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.Fill(1);

            for (int col = 0; col < layer.Width; col++)
            {
                for (int row = 0; row < layer.Height; row++)
                {
                    Tile tile = layer.Tiles[new Position(col, row)];
                    Assert.That(tile.GlobalId, Is.EqualTo((uint)1));
                    Assert.That(tile.X, Is.EqualTo(col));
                    Assert.That(tile.Y, Is.EqualTo(row));
                }
            }
        }

        [Test]
        public void Fill_GivenARectangleWithinItsBounds_FillsTheRectangleWithTiles()
        {
            Layer layer = new Layer(TiledFactory.BuildLayerXElementWithProperties());

            layer.Fill(new Rectangle(new Position(1, 1), new Position(2, 2)), (uint)1);

            for (int col = 1; col <= 2; col++)
            {
                for (int row = 1; row <= 2; row++)
                {
                    Tile tile = layer.Tiles[new Position(col, row)];
                    Assert.That(tile.GlobalId, Is.EqualTo((uint)1));
                    Assert.That(tile.X, Is.EqualTo(col));
                    Assert.That(tile.Y, Is.EqualTo(row));
                }
            }
        }
    }
}
