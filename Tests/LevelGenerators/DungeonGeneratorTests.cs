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

            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);

            // TODO: How can I check if the chunk has been broken into multiple random sized chunks?
            Assert.IsTrue(randomChunks.Count > 1);
        }

        [TestMethod]
        public void PlaceRooms_GivenASetOfChunks_PlacesRandomSizedRoomsWithinEachChunk()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(initialChunk);
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
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Corridor> corridors = _dungeonGenerator.JoinRooms(tree);

            Assert.AreEqual(randomChunks.Count / 2, corridors.Count);
        }

        [TestMethod]
        public void GetRooms_GivenTheRootOfABinaryTree_ReturnsAllRooms()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Room> rooms = _dungeonGenerator.GetRooms(tree, tree.Root);

            Assert.AreEqual(randomChunks.Count, rooms.Count);
        }

        [TestMethod]
        public void GetRooms_GivenANodeOtherThanTheRootOfABinaryTree_ReturnsAllRoomsOfTheSubtreeWithTheNodeAsItsRoot()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Room> rooms = _dungeonGenerator.GetRooms(tree, tree.Root.Right);
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes(tree.Root.Right);

            Assert.AreEqual(leafNodes.Count, rooms.Count);
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
        public void JoinRoom_GivenTwoRoomsThatAreTouchingEachOther_DoesNotCreateACorridorBetweenThem()
        {
            // * First Room; : Second Room
            // *****:::::
            // *****:::::
            // *****:::::
            // *****:::::
            // *****:::::
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            Room firstRoom = new Room(initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4)));
            Room secondRoom = new Room(initialChunk, new Rectangle(new Position(5, 0), new Position(9, 4)));

            Corridor corridor = firstRoom.Join(secondRoom);

            Assert.IsNull(corridor);
        }

        [TestMethod]
        public void JoinRoom_GivenTwoRoomsThatAreNotTouchingEachOther_CreatesACorridorBetweenTheMidpointsOfTheirClosestEdges()
        {
            // * First Room; : Second Room; . Corridor
            // *****  :::::
            // *****  :::::
            // *****..:::::
            // *****  :::::
            // *****  :::::
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            Chunk initialChunk = new Chunk(bounds);
            Room firstRoom = new Room(initialChunk, new Rectangle(new Position(0, 0), new Position(4, 4)));
            Room secondRoom = new Room(initialChunk, new Rectangle(new Position(7, 0), new Position(11, 4)));

            Corridor corridor = firstRoom.Join(secondRoom);

            Assert.IsNotNull(corridor);
            Assert.AreEqual(1, corridor.LineSegments.Count);
            Assert.AreEqual(new Position(5, 2), corridor.LineSegments[0].Start);
            Assert.AreEqual(new Position(6, 2), corridor.LineSegments[0].End);
            Assert.AreEqual(firstRoom, corridor.ConnectedRooms[0]);
            Assert.AreEqual(secondRoom, corridor.ConnectedRooms[1]);
        }
    }
}
