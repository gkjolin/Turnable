using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Turnable.Tiled;
using Turnable.Tiled.Api;
using Turnable.Tiled.Internal;
using Turnable.Utilities;

namespace Tests.Tiled
{
    [TestFixture]
    public class TileCollectionTests
    {
        private Data data;

        [SetUp]
        public void Init()
        {
            var tmxDocument = XDocument.Load("c:/git/Turnable/Tests/SampleData/TmxMap.tmx");
            var tmxLayerData = from l in tmxDocument.Elements("map").Elements("layer")
                               where (l.Attribute("name").Value == "walls")
                               select l;
            data = new Data(tmxLayerData.Elements("data").First());
        }

        [Test]
        public void DefaultConstructor_CreatesAnEmptyTileCollection()
        {
            ITileCollection tileCollection = new TileCollection();

            Assert.That(tileCollection.Count, Is.EqualTo(0));

        }
        [Test]
        public void Constructor_GivenNullData_CreatesAnEmptyTileCollection()
        {
            ITileCollection tileCollection = new TileCollection(15, 15, null);

            Assert.That(tileCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenDataWithTiles_CreatesAllTiles()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, data);

            Assert.That(tileCollection.Count, Is.EqualTo(105));
        }

        [Test]
        public void Indexer_GivenALocationWithATileAtThatLocation_ReturnsTheTile()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, data);

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileCollection[new Coordinates(6, 1)];

            Assert.That(tile.GlobalId, Is.EqualTo((uint)263));
        }

        [Test]
        public void Indexer_GivenALocationWithNoTileAtThatLocation_ReturnsNull()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, data);

            // Test to see if a missing tile is returned as null. 
            Tile tile = tileCollection[new Coordinates(1, 2)];

            Assert.That(tile, Is.Null);
        }
        
        [Test]
        public void Indexer_GivenALocationAndATile_SetsTileAtTheLocation()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, null);

            Coordinates location = new Coordinates(6, 1);
            Tile tile = new Tile(105);
            tileCollection[location] = tile;

            tile = tileCollection[location];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)105));
        }

        [Test]
        public void Indexer_GivenALocationThatAlreadyHasATile_OverwritesTheTileAtThatPosition()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, null);

            Coordinates location = new Coordinates(6, 1);
            Tile tile = new Tile(2107);
            tileCollection[location] = tile;
            tile = new Tile(106);
            tileCollection[location] = tile;

            tile = tileCollection[location];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)106));
        }

        [Test]
        public void Indexer_GivenXAndYWithNoTileAtThatLocation_ReturnsNull()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, data);

            // Test to see if a missing tile is returned as null. 
            Tile tile = tileCollection[1, 2];

            Assert.That(tile, Is.Null);
        }

        [Test]
        public void Indexer_GivenXAndYAndATile_SetsTileAtTheLocation()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, null);

            Tile tile = new Tile(105);
            tileCollection[6, 1] = tile;

            tile = tileCollection[6, 1];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)105));
        }

        [Test]
        public void Indexer_GivenXAndYAndALocationThatAlreadyHasATile_OverwritesTileAtThatPosition()
        {
            ITileCollection tileCollection = new TileCollection(14, 11, null);

            Coordinates location = new Coordinates(6, 1);
            Tile tile = new Tile(2107);
            tileCollection[6, 1] = tile;
            tile = new Tile(106);
            tileCollection[6, 1] = tile;

            tile = tileCollection[location];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)106));
        }

        [Test]
        public void Remove_GivenALocationThatHasATile_RemovesTileAtThatPosition()
        {
            Coordinates location = new Coordinates(6, 1);
            var tileCollection = new TileCollection(14, 11, null);
            Tile tile = new Tile(2107);
            tileCollection[location] = tile;

            tileCollection.Remove(location);

            Assert.That(tileCollection[location], Is.Null);
        }

        [Test]
        public void Remove_GivenALocationThatHasNoTile_DoesNotDoAnything()
        {
            Coordinates location = new Coordinates(6, 1);
            var tileCollection = new TileCollection(14, 11, null);

            tileCollection.Remove(location);

            Assert.That(tileCollection[location], Is.Null);
        }
    }
}
