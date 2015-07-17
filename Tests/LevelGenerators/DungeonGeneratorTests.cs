using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.LevelGenerators;
using Turnable.Components;
using System.Collections.Generic;
using Turnable.Utilities;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class DungeonGeneratorTests
    {
        private DungeonGenerator _dungeonGenerator;

        [TestInitialize]
        public void Initialize()
        {
            _dungeonGenerator = new DungeonGenerator();
        }

        [TestMethod]
        public void Chunkify_GivenAnInitialChunk_RandomlyBreaksUpTheChunk()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);

            List<Chunk> randomChunks = _dungeonGenerator.Chunkify(initialChunk);

            // TODO: How can I check if the chunk has been broken into multiple random sized chunks?
            Assert.IsTrue(randomChunks.Count > 10);
        }

        [TestMethod]
        public void PlaceRooms_GivenASetOfChunks_PlacesRandomSizedRoomsWithinEachChunk()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> randomChunks = _dungeonGenerator.Chunkify(initialChunk);

            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            Assert.AreEqual(randomRooms.Count, randomChunks.Count);
            foreach (Room room in randomRooms)
            {
                Assert.IsTrue(room.ParentChunk.Bounds.Contains(room.Bounds));
            }
        }

        // -------------
        // Joining Rooms
        // -------------
        [TestMethod]
        public void GetCorridor_GivenTwoRoomsThatTouch_ReturnsANullCorridor()
        { 
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk chunk = new Chunk(bounds);
            Room firstRoom = new Room(chunk, new Rectangle(new Position(0, 0), 10, 10));
            Room secondRoom = new Room(chunk, new Rectangle(new Position(10, 0), 10, 10));

            Corridor corridor = _dungeonGenerator.GetCorridor(firstRoom, secondRoom);

            Assert.IsNull(corridor);
        }
    }
}
