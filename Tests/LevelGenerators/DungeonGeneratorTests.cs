using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.LevelGenerators;
using Turnable.Components;
using System.Collections.Generic;

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
            Chunk initialChunk = new Chunk(new Position(0, 0), 100, 100);

            List<Chunk> randomChunks = _dungeonGenerator.Chunkify(initialChunk);

            // TODO: How can I check if the chunk has been broken into multiple random sized chunks?
            Assert.IsTrue(randomChunks.Count > 10);
        }

        [TestMethod]
        public void PlaceRooms_GivenASetOfChunks_PlacesRandomSizedRoomsWithinEachChunk()
        {
            Chunk initialChunk = new Chunk(new Position(0, 0), 100, 100);
            List<Chunk> randomChunks = _dungeonGenerator.Chunkify(initialChunk);

            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            Assert.AreEqual(randomRooms.Count, randomChunks.Count);

            foreach (Room room in randomRooms)
            {

            }
        }

        // -------------
        // Joining Rooms
        // -------------


        [TestMethod]
        public void JoinRooms_JoiningTwoRoomsThatTouch_ReturnsANullCorridor()
        { }
    }
}
