using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using Turnable.Tiled.Api;
using Turnable.Utilities;
using Turnable.Utilities.Api;

namespace Tests.Tiled
{
    [TestFixture]
    public class LayerTests
    {
        private XElement layerXElement;
        private ILayer layer;

        [SetUp]
        public void Init()
        {
            var tmxDocument = XDocument.Load("c:/git/Turnable/Tests/SampleData/TmxMap.tmx");
            layerXElement = (from l in tmxDocument.Elements("map").Elements("layer")
                                select l).First();
            layer = new Layer(layerXElement);
        }

        [Test]
        public void Constructor_GivenAName_InitializesLayerCorrectly()
        {
            ILayer newLayer = new Layer("Layer 1");

            Assert.That(newLayer.Name, Is.EqualTo("Layer 1"));
            Assert.That(newLayer.Opacity, Is.EqualTo(1.0));
            Assert.That(newLayer.IsVisible, Is.True);
            Assert.That(newLayer.Width, Is.Null);
            Assert.That(newLayer.Height, Is.Null);
        }

        [Test]
        public void Constructor_GivenALayerXElement_InitializesLayerCorrectly()
        {
            Assert.That(layer.Name, Is.EqualTo("floor"));
            Assert.That(layer.Opacity, Is.EqualTo(1.0));
            Assert.That(layer.IsVisible, Is.True);
            Assert.That(layer.Width, Is.EqualTo(14));
            Assert.That(layer.Height, Is.EqualTo(11));

            // Are the tiles in the layer created? 
            // TODO: Need a better assertion than the one used below. Asserting that atleast one tile has been loaded isn't the best way to test that the tiles of a lyare are correct. 
            Assert.That(layer.TileCount, Is.GreaterThan(1));

            /*
                        // Are the Properties for this Layer loaded?
                        Assert.That(layer.Properties.Count, Is.EqualTo(1));
            */
        }

        // ************************
        // Layer Manipulation Tests
        // ************************
        [Test]
        public void IsTilePresent_GivenCoordinatesWhereTileExists_ReturnsTrue()
        {
            ICoordinates coordinates = new Coordinates(7, 2);
            layer.SetTile(coordinates, 2107);

            Assert.That(layer.IsTilePresent(coordinates), Is.True);
        }

        [Test]
        public void IsTilePresent_GivenCoordinatesWhereTileDoesNotExist_ReturnsFalse()
        {
            ICoordinates coordinates = new Coordinates(0, 0);

            Assert.That(layer.IsTilePresent(coordinates), Is.False);
        }

        [Test]
        public void GetTile_GivenAPositionWithATile_ReturnsTheTile()
        {
            ICoordinates coordinates = new Coordinates(7, 2);
            layer.SetTile(coordinates, 2107);

            Tile tile = layer.GetTile(coordinates);

            Assert.That(tile.GlobalId, Is.EqualTo((uint)2107));
        }

        [Test]
        public void GetTile_GivenCoordinatesWhereNoTileExists_ReturnsNull()
        {
            ICoordinates coordinates = new Coordinates(0, 0);

            Assert.That(layer.IsTilePresent(coordinates), Is.False);

            Assert.That(layer.GetTile(coordinates), Is.Null);
        }

        [Test]
        public void MoveTile_MovesATileToAnEmptyDestinationInTheSameLayer()
        {
            int tileCount = layer.TileCount;
            uint tileGlobalId = layer.GetTile(new Coordinates(7, 2)).GlobalId;

            layer.MoveTile(new Coordinates(7, 2), new Coordinates(6, 2));

            Assert.That(layer.IsTilePresent(new Coordinates(7, 2)), Is.False);
            Assert.That(layer.IsTilePresent(new Coordinates(6, 2)), Is.True);

            // The number of tiles in the layer should not change
            Assert.That(layer.TileCount, Is.EqualTo(tileCount));

            // The tile should have moved correctly
            Tile tile = layer.GetTile(new Coordinates(6, 2));
            Assert.That(tile.GlobalId, Is.EqualTo(tileGlobalId));
        }

        [Test]
        public void MoveTile_WhenNoTileExistsAtThePositionInTheLayer_ThrowsAnException()
        {
            Assert.That(() => layer.MoveTile(new Coordinates(0, 0), new Coordinates(6, 2)), Throws.InvalidOperationException);
        }

        [Test]
        public void MoveTile_WhenDestinationIsOccupied_ThrowsAnException()
        {
            Assert.That(() => layer.MoveTile(new Coordinates(7, 2), new Coordinates(1, 2)), Throws.InvalidOperationException);
        }
/*
        [Test]
        public void SwapTile_SwapsTwoTilesInTheSameLayer()
        {
            int tileCount = layer.Tiles.Count;

            uint tile1GlobalId = layer.Tiles[new Coordinates(7, 2)].GlobalId;
            uint tile2GlobalId = layer.Tiles[new Coordinates(1, 2)].GlobalId;

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
        }*/

        /*
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
*/

    }
}
