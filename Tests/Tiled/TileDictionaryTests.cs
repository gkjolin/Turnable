using NUnit.Framework;

namespace Tests.Tiled
{
    [TestFixture]
    public class TileDictionaryTests
    {
/*
        [Test]
        public void DefaultConstructor_CreatesAnEmptyTileList()
        {
            var tileDictionary = new TileDictionary();

            Assert.That(tileDictionary.Count, Is.EqualTo(0));
        }
*/

/*
        [Test]
        public void Constructor_GivenNullData_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, null);

            Assert.That(tileList.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenDataWithNoTiles_CreatesAnEmptyTileList()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Assert.That(tileList.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenDataWithTiles_CreatesAllTiles()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            Assert.That(tileList.Count, Is.EqualTo(8));

            // Test to see if one tile is loaded up correctly. 
            // The Tiled(.tmx) format uses an origin that starts at the top left with Y increasing going down
            // However most libraries use an origin that starts at the bottom left with Y increasing going up
            // So we need to test that Y is "flipped" using (height - row - 1)
            Tile tile = tileList[new Position(6, 1)];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)2107));
            Assert.That(tile.X, Is.EqualTo(6));
            Assert.That(tile.Y, Is.EqualTo(1));
        }

        [Test]
        public void Indexer_GivenAPositionWithATileAtThatPosition_ReturnsTheTile()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileList[new Position(6, 1)];

            Assert.That(tile.GlobalId, Is.EqualTo((uint)2107));
            Assert.That(tile.X, Is.EqualTo(6));
            Assert.That(tile.Y, Is.EqualTo(1));
        }

        [Test]
        public void Indexer_GivenAPositionWithNoTileAtThatPosition_ReturnsNull()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithTiles());

            // Test to see if one tile is loaded up correctly. 
            Tile tile = tileList[new Position(7, 1)];

            Assert.That(tile, Is.Null);
        }

        [Test]
        public void Indexer_GivenAPositionAndValue_SetsTileAtThePosition()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Tile tile = new Tile(2107, 6, 1);
            tileList[new Position(6, 1)] = tile;

            tile = tileList[new Position(6, 1)];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)2107));
            Assert.That(tile.X, Is.EqualTo(6));
            Assert.That(tile.Y, Is.EqualTo(1));
        }

        [Test]
        public void Indexer_GivenAPositionThatAlreadyHasATile_OverwritesTilesAtThePosition()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            Tile tile = new Tile(2107, 6, 1);
            tileList[new Position(6, 1)] = tile;
            tile = new Tile(2106, 6, 1);
            tileList[new Position(6, 1)] = tile;

            tile = tileList[new Position(6, 1)];
            Assert.That(tile.GlobalId, Is.EqualTo((uint)2106));
            Assert.That(tile.X, Is.EqualTo(6));
            Assert.That(tile.Y, Is.EqualTo(1));
        }

        [Test]
        public void Remove_GivenAPositionThatHasATile_RemovesTileAtThatPosition()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());
            Tile tile = new Tile(2107, 6, 1);
            tileList[new Position(6, 1)] = tile;

            tileList.Remove(new Position(6, 1));

            Assert.That(tileList[new Position(6, 1)], Is.Null);
        }

        [Test]
        public void Remove_GivenAPositionThatHasNoTile_DoesNotDoAnything()
        {
            TileList tileList = new TileList(15, 15, TiledFactory.BuildDataWithNoTiles());

            tileList.Remove(new Position(6, 1));

            Assert.That(tileList[new Position(6, 1)], Is.Null);
        }
*/
    }
}
