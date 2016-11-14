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
            int initialTileCount = layer.TileCount;
            uint tileGlobalId = layer.GetTile(new Coordinates(7, 2)).GlobalId;

            layer.MoveTile(new Coordinates(7, 2), new Coordinates(6, 2));

            Assert.That(layer.IsTilePresent(new Coordinates(7, 2)), Is.False);
            Assert.That(layer.IsTilePresent(new Coordinates(6, 2)), Is.True);

            // The number of tiles in the layer should not have changed
            Assert.That(layer.TileCount, Is.EqualTo(initialTileCount));

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

        [Test]
        public void SwapTile_SwapsTwoTilesInTheSameLayer()
        {
            int initialTileCount = layer.TileCount;

            uint tile1GlobalId = layer.GetTile(new Coordinates(7, 2)).GlobalId;
            uint tile2GlobalId = layer.GetTile(new Coordinates(1, 2)).GlobalId;

            layer.SwapTile(new Coordinates(7, 2), new Coordinates(1, 2));

            Assert.That(layer.IsTilePresent(new Coordinates(7, 2)), Is.True);
            Assert.That(layer.IsTilePresent(new Coordinates(1, 2)), Is.True);

            // The number of tiles in the layer should not have changed
            Assert.That(layer.TileCount, Is.EqualTo(initialTileCount));

            // The tiles should have been swapped correctly
            Assert.That(layer.GetTile(new Coordinates(7, 2)).GlobalId, Is.EqualTo(tile2GlobalId));
            Assert.That(layer.GetTile(new Coordinates(1, 2)).GlobalId, Is.EqualTo(tile1GlobalId));
        }

        [Test]
        public void SwapTile_WhenNoTileExistsInTheFirstPosition_ThrowsAnException()
        {
            Assert.That(() => layer.SwapTile(new Coordinates(6, 2), new Coordinates(7, 2)), Throws.InvalidOperationException);
        }

        [Test]
        public void SwapTile_WhenNoTileExistsInTheSecondPosition_ThrowsAnException()
        {
            Assert.That(() => layer.SwapTile(new Coordinates(7, 2), new Coordinates(6, 2)), Throws.InvalidOperationException);
        }

        [Test]
        public void SetTile_GivenCoordinatesForAnEmptyPosition_SetsTileAtThatPosition()
        {
            layer.SetTile(new Coordinates(0, 0), 200);

            Assert.That(layer.GetTile(new Coordinates(0, 0)).GlobalId, Is.EqualTo((uint)200));
        }

        [Test]
        public void SetTile_GivenCoordinatesWhereTileAlreadyExistsAtThePosition_Succeeds()
        {
            layer.SetTile(new Coordinates(0, 0), 200);
            layer.SetTile(new Coordinates(0, 0), 201);

            Assert.That(layer.GetTile(new Coordinates(0, 0)).GlobalId, Is.EqualTo((uint)201));
        }

        // TODO: Setting a tile outside the bounds of the Layer is illegal, write a unit test for this
        [Test]
        public void RemoveTile_GivenCoordiatesWhereATileExists_RemovesTileThatExistsAtThatPosition()
        {
            layer.SetTile(new Coordinates(0, 0), 200);
            int tileCount = layer.TileCount;

            layer.RemoveTile(new Coordinates(0, 0));

            Assert.That(layer.IsTilePresent(new Coordinates(0, 0)), Is.False);
            Assert.That(layer.TileCount, Is.EqualTo(tileCount - 1));
        }

        [Test]
        public void RemoveTile_GivenCoordinatesThatHasNoTileAtThatPosition_DoesNotThrowAnException()
        {
            Assert.That(() => layer.RemoveTile(new Coordinates(7, 2)), Throws.Nothing);
        }
    }
}
