using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.LevelGenerators;
using Turnable.Components;
using System.Collections.Generic;
using Turnable.Utilities;
using Turnable.Api;
using Turnable.Locations;
using Moq;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class DungeonGeneratorTests
    {
        private IDungeonGenerator _dungeonGenerator;
        private Chunk _initialChunk;

        [TestInitialize]
        public void Initialize()
        {
            _dungeonGenerator = new DungeonGenerator();
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            _initialChunk = new Chunk(bounds);
        }

        [TestMethod]
        public void Chunkify_GivenAnInitialChunk_RandomlyBreaksUpTheChunk()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);

            // TODO: How can I check if the chunk has been broken into multiple random sized chunks?
            Assert.IsTrue(randomChunks.Count > 1);
        }

        [TestMethod]
        public void PlaceRooms_GivenASetOfChunks_PlacesRandomSizedRoomsWithinEachChunk()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);

            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            Assert.AreEqual(randomRooms.Count, randomChunks.Count);
            foreach (Room room in randomRooms)
            {
                Assert.IsTrue(room.ParentChunk.Bounds.Contains(room.Bounds));
            }
        }

        [TestMethod]
        public void JoinRooms_GivenASetOfRoomsWithinChunks_JoinsTheRoomsSoThatAllRoomsAreReachable()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Corridor> corridors = _dungeonGenerator.JoinRooms(tree);

            // TODO: Test that all rooms are reachable
            // There should atleast be corridors between each set of 2 rooms, so there should atleast be (number of rooms / 2) corridors.
            Assert.IsTrue(corridors.Count > (randomChunks.Count / 2));
        }

        [TestMethod]
        public void CollectRooms_GivenTheRootOfABinaryTree_ReturnsAllRooms()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Room> rooms = _dungeonGenerator.CollectRooms(tree, tree.Root);

            Assert.AreEqual(randomChunks.Count, rooms.Count);
        }

        [TestMethod]
        public void GetRooms_GivenANodeOtherThanTheRootOfABinaryTree_ReturnsAllRoomsOfTheSubtreeWithTheNodeAsItsRoot()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Room> rooms = _dungeonGenerator.CollectRooms(tree, tree.Root.Right);
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes(tree.Root.Right);

            Assert.AreEqual(leafNodes.Count, rooms.Count);
        }

        [TestMethod]
        public void DrawLevel_GivenATreeWithASetOfRandomRoomsJoinedTogether_WritesTheTreeToANewLevel()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);
            _dungeonGenerator.JoinRooms(tree);
            Level level = new Level();

            _dungeonGenerator.DrawLevel(tree, out level);

            Assert.IsNotNull(level.SpecialLayers[SpecialLayer.Collision]);
        }

        [TestMethod]
        public void Generate_GivenAStartingChunk_GeneratesARandomDungeon()
        {
            BinaryTree<Chunk> tree = new BinaryTree<Chunk>(); ;
            List<Chunk> randomChunks = new List<Chunk>();
            Mock<DungeonGenerator> mockDungeonGenerator = new Mock<DungeonGenerator>();
            mockDungeonGenerator.CallBase = true;
            mockDungeonGenerator.Setup(dg => dg.Chunkify(_initialChunk)).Returns(tree);
            mockDungeonGenerator.Setup(dg => dg.CollectLeafChunks(tree, null)).Returns(randomChunks);
            mockDungeonGenerator.Setup(dg => dg.PlaceRooms(randomChunks));
            mockDungeonGenerator.Setup(dg => dg.JoinRooms(tree));

            Level randomLevel = mockDungeonGenerator.Object.Generate(_initialChunk, out tree);

            mockDungeonGenerator.Verify(dg => dg.Chunkify(_initialChunk));
            mockDungeonGenerator.Verify(dg => dg.CollectLeafChunks(tree, null));
            mockDungeonGenerator.Verify(dg => dg.PlaceRooms(randomChunks));
            mockDungeonGenerator.Verify(dg => dg.JoinRooms(tree));
        }

        // -------------
        // Joining Rooms
        // -------------
        [TestMethod]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyOrVerticallyByOneSpace_ReturnsACorridorWithOneLineSegment()
        {
            // A corridor is always built excluding the start and end positions.
            // The reason for this is because the start and end positions of most corridors are chosen from the edges of two rooms.
            // Therefore the start and end positions are NOT part of the corridor.
            // S - Start position, E - End position, . - corridor

            // S.E
            Position start = new Position(0, 0);
            Position end = new Position(2, 0);

            Corridor corridor = Corridor.Build(start, end);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].End);

            // E.S
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].End);

            // S
            // .
            // E
            start = new Position(0, 2);
            end = new Position(0, 0);
            
            corridor = Corridor.Build(start, end);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(0, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(0, 1), corridor.LineSegments[0].End);

            // E
            // .
            // S
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(0, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(0, 1), corridor.LineSegments[0].End);
        }

        [TestMethod]
        public void BuildCorridor_GivenTwoPositionsSeparatedHorizontallyOrVerticallyByMoreThanOneSpace_ReturnsACorridorWithOneLineSegment()
        {
            // A corridor is always built excluding the start and end positions.
            // The reason for this is because the start and end positions of most corridors are chosen from the edges of two rooms.
            // Therefore the start and end positions are NOT part of the corridor.
            // S - Start position, E - End position, . - corridor

            // S...E
            Position start = new Position(0, 0);
            Position end = new Position(4, 0);
            
            Corridor corridor = Corridor.Build(start, end);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(3, 0), corridor.LineSegments[0].End);

            // E...S
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(3, 0), corridor.LineSegments[0].End);

            // S
            // .
            // .
            // .
            // E
            start = new Position(0, 4);
            end = new Position(0, 0);
            
            corridor = Corridor.Build(start, end);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(0, 3), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(0, 1), corridor.LineSegments[0].End);

            // E
            // .
            // .
            // .
            // S
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(0, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(0, 3), corridor.LineSegments[0].End);
        }

        [TestMethod]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyAndVerticallyByMoreThanOneSpace_ReturnsACorridorTODO()
        {
            // S.
            //  .E
        }

        [TestMethod]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyAndVerticallyByTwoHorizontalAndVerticalSpaces_ReturnsALShapedCorridor()
        {
            // The algorithm actually returns a z type corridor with the segment going to the right being just one space long.
            // S. 
            //  ..E
            Position start = new Position(0, 1);
            Position end = new Position(3, 0);

            Corridor corridor = Corridor.Build(start, end);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[2].End);

            // E. 
            //  ..S
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[2].End);

            //  ..E
            // S.
            start = new Position(0, 0);
            end = new Position(3, 1);
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[2].End);

            //  ..S
            // E.
            corridor = Corridor.Build(start, end);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[2].End);
        }

        [TestMethod]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyAndVerticallyByMoreThanTwoHorizontalAndVerticalSpaces_ReturnsAZShapedCorridor()
        {
            // S.. 
            //   ..E
            Position start = new Position(0, 1);
            Position end = new Position(4, 0);

            Corridor corridor = Corridor.Build(start, end);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(3, 0), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(3, 0), corridor.LineSegments[2].End);

            // E.. 
            //   ..S
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 1), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(3, 0), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(3, 0), corridor.LineSegments[2].End);

            //   ..E
            // S..
            start = new Position(0, 0);
            end = new Position(4, 1);
            corridor = Corridor.Build(end, start);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(3, 1), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(3, 1), corridor.LineSegments[2].End);

            //   ..S
            // E..
            corridor = Corridor.Build(start, end);

            Assert.AreEqual(3, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(1, 0), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(2, 0), corridor.LineSegments[0].End);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[1].Start);
            Assert.AreEqual(new Position(2, 1), corridor.LineSegments[1].End);
            Assert.AreEqual(new Position(3, 1), corridor.LineSegments[2].Start);
            Assert.AreEqual(new Position(3, 1), corridor.LineSegments[2].End);
        }

        [TestMethod]
        public void Join_GivenTwoRoomsThatAreTouchingEachOther_DoesNotCreateACorridorBetweenThem()
        {
            // * First Room; : Second Room
            // *****:::::
            // *****:::::
            // *****:::::
            // *****:::::
            // *****:::::
            Room firstRoom = new Room(_initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4)));
            Room secondRoom = new Room(_initialChunk, new Rectangle(new Position(5, 0), new Position(9, 4)));

            Corridor corridor = firstRoom.Join(secondRoom);

            Assert.IsNull(corridor);
        }

        [TestMethod]
        public void Join_GivenTwoRoomsThatAreNotTouchingEachOther_CreatesACorridorBetweenTheMidpointsOfTheirClosestEdges()
        {
            // * First Room; : Second Room; . Corridor
            // *****  :::::
            // *****  :::::
            // *****..:::::
            // *****  :::::
            // *****  :::::
            Room firstRoom = new Room(_initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4)));
            Room secondRoom = new Room(_initialChunk, new Rectangle(new Position(7, 0), new Position(11, 4)));

            Corridor corridor = firstRoom.Join(secondRoom);

            Assert.IsNotNull(corridor);
            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(5, 2), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(6, 2), corridor.LineSegments[0].End);
            Assert.AreEqual(firstRoom, corridor.ConnectedRooms[0]);
            Assert.AreEqual(secondRoom, corridor.ConnectedRooms[1]);
            Assert.AreEqual(corridor, firstRoom.Corridors[0]);
            Assert.AreEqual(corridor, secondRoom.Corridors[0]);
        }

        [TestMethod]
        public void ChooseRoomsToJoin_GivenTwoListsOfRoomsWithOneRoomEach_ReturnsThoseTwoRooms()
        {
            // * First Room; : Second Room 
            // *****  :::::
            // *****  :::::
            // *****  :::::
            // *****  :::::
            // *****  :::::
            List<Room> firstListOfRooms = new List<Room>() { new Room(_initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4))) };
            List<Room> secondListOfRooms = new List<Room>() { new Room(_initialChunk, new Rectangle(new Position(7, 0), new Position(11, 4))) };

            List<Room> roomsToJoin = _dungeonGenerator.ChooseRoomsToJoin(firstListOfRooms, secondListOfRooms);

            Assert.AreEqual(2, roomsToJoin.Count);
            Assert.AreEqual(firstListOfRooms[0], roomsToJoin[0]);
            Assert.AreEqual(secondListOfRooms[0], roomsToJoin[1]);
        }

        [TestMethod]
        public void ChooseRoomsToJoin_GivenTwoListsOfRoomsWithTwoRoomsInFirstListAndOneRoomInSecondList_ReturnsTheTwoRoomsWithTheClosestEdges()
        {
            // * First Room; # Second Room; : Third Room
            // *****  :::::
            // *****  :::::
            // *****  :::::   ####
            // *****  :::::   ####
            // *****  :::::
            List<Room> firstListOfRooms = new List<Room>() { 
                new Room(_initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4))) 
            };
            List<Room> secondListOfRooms = new List<Room>() { 
                new Room(_initialChunk, new Rectangle(new Position(15, 1), new Position(18, 2))),
                new Room(_initialChunk, new Rectangle(new Position(7, 0), new Position(11, 4)))
            };

            List<Room> roomsToJoin = _dungeonGenerator.ChooseRoomsToJoin(firstListOfRooms, secondListOfRooms);

            Assert.AreEqual(2, roomsToJoin.Count);
            Assert.AreEqual(firstListOfRooms[0], roomsToJoin[0]);
            Assert.AreEqual(secondListOfRooms[1], roomsToJoin[1]);
        }

        [TestMethod]
        public void ChooseRoomsToJoin_GivenTwoListsOfRoomsWithTwoRoomsInEachList_ReturnsTheTwoRoomsWithTheClosestEdges()
        {
            // * First Room; : Second Room; + Third Room; # Fourth Room
            // *****  :::::
            // *****  :::::
            // *****  :::::   ####  ++++
            // *****  :::::   ####  ++++
            // *****  :::::         ++++
            List<Room> firstListOfRooms = new List<Room>() { 
                new Room(_initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4))),
                new Room(_initialChunk, new Rectangle(new Position(7, 0), new Position(11, 4)))
            };
            List<Room> secondListOfRooms = new List<Room>() { 
                new Room(_initialChunk, new Rectangle(new Position(21, 0), new Position(24, 2))),
                new Room(_initialChunk, new Rectangle(new Position(15, 1), new Position(18, 2)))
            };

            List<Room> roomsToJoin = _dungeonGenerator.ChooseRoomsToJoin(firstListOfRooms, secondListOfRooms);

            Assert.AreEqual(2, roomsToJoin.Count);
            Assert.AreEqual(firstListOfRooms[1], roomsToJoin[0]);
            Assert.AreEqual(secondListOfRooms[1], roomsToJoin[1]);
        }
    }
}
