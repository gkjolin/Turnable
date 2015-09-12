using System;
using NUnit.Framework;
using Turnable.LevelGenerators;
using Turnable.Components;
using System.Collections.Generic;
using Turnable.Utilities;
using Turnable.Api;
using Turnable.Locations;
using Moq;
using Turnable.Tiled;
using Turnable.Vision;

namespace Tests.LevelGenerators
{
    [TestFixture]
    public class DungeonGeneratorTests
    {
        private IDungeonGenerator _dungeonGenerator;
        private Chunk _initialChunk;

        [SetUp]
        public void Initialize()
        {
            _dungeonGenerator = new DungeonGenerator();
            Rectangle bounds = new Rectangle(new Position(0, 0), 100, 100);
            _initialChunk = new Chunk(bounds);
        }

        [Test]
        public void Chunkify_GivenAnInitialChunk_RandomlyBreaksUpTheChunk()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);

            // TODO: How can I check if the chunk has been broken into multiple random sized chunks?
            Assert.That(randomChunks.Count, Is.GreaterThan(1));
        }

        [Test]
        public void PlaceRooms_GivenASetOfChunks_PlacesRandomSizedRoomsWithinEachChunk()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);

            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            Assert.That(randomRooms.Count, Is.EqualTo(randomChunks.Count));
            foreach (Room room in randomRooms)
            {
                Assert.That(room.ParentChunk.Bounds.Contains(room.Bounds), Is.True);
            }
        }

        [Test]
        public void JoinRooms_GivenASetOfRoomsWithinChunks_JoinsTheRoomsSoThatAllRoomsAreReachable()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Corridor> corridors = _dungeonGenerator.JoinRooms(tree);

            // TODO: Test that all rooms are reachable
            // There should atleast be corridors between each set of 2 rooms, so there should atleast be (number of rooms / 2) corridors.
            Assert.That(corridors.Count, Is.GreaterThan(randomChunks.Count / 2));
        }

        [Test]
        public void CollectRooms_GivenTheRootOfABinaryTree_ReturnsAllRooms()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Room> rooms = _dungeonGenerator.CollectRooms(tree, tree.Root);

            Assert.That(rooms.Count, Is.EqualTo(randomChunks.Count));
        }

        [Test]
        public void GetRooms_GivenANodeOtherThanTheRootOfABinaryTree_ReturnsAllRoomsOfTheSubtreeWithTheNodeAsItsRoot()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);

            List<Room> rooms = _dungeonGenerator.CollectRooms(tree, tree.Root.Right);
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes(tree.Root.Right);

            Assert.That(rooms.Count, Is.EqualTo(leafNodes.Count));
        }

        [Test]
        public void DrawLevel_GivenATreeWithASetOfRandomRoomsJoinedTogether_WritesTheRoomsAndCorridorsInTheTreeToALevel()
        {
            BinaryTree<Chunk> tree = _dungeonGenerator.Chunkify(_initialChunk);
            List<Chunk> randomChunks = _dungeonGenerator.CollectLeafChunks(tree);
            List<Room> randomRooms = _dungeonGenerator.PlaceRooms(randomChunks);
            _dungeonGenerator.JoinRooms(tree);
            Level level = new Level();
            List<Position> emptyPositions = new List<Position>();

            _dungeonGenerator.DrawLevel(tree, level);

            List<Room> rooms = _dungeonGenerator.CollectRooms(tree);
            Assert.That(level.SpecialLayers[SpecialLayer.Collision], Is.Not.Null);
            Layer drawnLayer = level.SpecialLayers[SpecialLayer.Collision];
            foreach (Room room in rooms)
            {
                // Check if the room is written to level
                int col, row;

                for (col = room.Bounds.BottomLeft.X; col <= room.Bounds.TopRight.X; col++)
                {
                    for (row = room.Bounds.BottomLeft.Y; row <= room.Bounds.TopRight.Y; row++)
                    {
                        Tile tile = drawnLayer.GetTile(new Position(col, row));
                        Assert.That(tile, Is.Not.Null);
                        Assert.That(tile.GlobalId, Is.EqualTo((uint)0));
                        emptyPositions.Add(new Position(col, row));
                    }
                }

                foreach (Corridor corridor in room.Corridors)
                {
                    // Check if each corridor is written to the level as a 0
                    foreach (LineSegment lineSegment in corridor.LineSegments)
                    {
                        foreach (Position point in lineSegment.Points)
                        {
                            Tile tile = drawnLayer.GetTile(point);
                            Assert.That(tile, Is.Not.Null);
                            Assert.That(tile.GlobalId, Is.EqualTo((uint)0));
                            emptyPositions.Add(point);
                        }
                    }
                }

                // All the positions that have been added to emptyPositions have a tile with a 0 global id.
                // The rest of the layer should all have tiles with a value of 1, since the level is in essence being carved out of a solid block.
                for (col = tree.Root.Value.Bounds.BottomLeft.X; col <= tree.Root.Value.Bounds.TopRight.X; col++)
                {
                    for (row = tree.Root.Value.Bounds.BottomLeft.Y; row <= tree.Root.Value.Bounds.TopRight.Y; row++)
                    {
                        if (!emptyPositions.Contains(new Position(col, row)))
                        {
                            Tile tile = drawnLayer.GetTile(new Position(col, row));
                            Assert.That(tile, Is.Not.Null);
                            Assert.That(tile.GlobalId, Is.EqualTo((uint)1));
                        }
                    }
                }

            }
        }

        [Test]
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
        [Test]
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

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(1, 0)));

            // E.S
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(1, 0)));

            // S
            // .
            // E
            start = new Position(0, 2);
            end = new Position(0, 0);
            
            corridor = Corridor.Build(start, end);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(0, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(0, 1)));

            // E
            // .
            // S
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(0, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(0, 1)));
        }

        [Test]
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

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(3, 0)));

            // E...S
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(3, 0)));

            // S
            // .
            // .
            // .
            // E
            start = new Position(0, 4);
            end = new Position(0, 0);
            
            corridor = Corridor.Build(start, end);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(0, 3)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(0, 1)));

            // E
            // .
            // .
            // .
            // S
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(0, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(0, 3)));
        }

        [Test]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyAndVerticallyByMoreThanOneSpace_ReturnsACorridorThat()
        {
            // TODO
            // S.
            //  .E
        }

        [Test]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyAndVerticallyByTwoHorizontalAndVerticalSpaces_ReturnsALShapedCorridor()
        {
            // The algorithm actually returns a z type corridor with the segment going to the right being just one space long.
            // S. 
            //  ..E
            Position start = new Position(0, 1);
            Position end = new Position(3, 0);

            Corridor corridor = Corridor.Build(start, end);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(2, 0)));

            // E. 
            //  ..S
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(2, 0)));

            //  ..E
            // S.
            start = new Position(0, 0);
            end = new Position(3, 1);
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(2, 1)));

            //  ..S
            // E.
            corridor = Corridor.Build(start, end);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(2, 1)));
        }

        [Test]
        public void BuildCorridor_GivenTwoPositionsThatAreSeparatedHorizontallyAndVerticallyByMoreThanTwoHorizontalAndVerticalSpaces_ReturnsAZShapedCorridor()
        {
            // S.. 
            //   ..E
            Position start = new Position(0, 1);
            Position end = new Position(4, 0);

            Corridor corridor = Corridor.Build(start, end);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(3, 0)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(3, 0)));

            // E.. 
            //   ..S
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 1)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(3, 0)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(3, 0)));

            //   ..E
            // S..
            start = new Position(0, 0);
            end = new Position(4, 1);
            corridor = Corridor.Build(end, start);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(3, 1)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(3, 1)));

            //   ..S
            // E..
            corridor = Corridor.Build(start, end);

            Assert.That(corridor.LineSegments.Count, Is.EqualTo(3));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(1, 0)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(2, 0)));
            Assert.That(corridor.LineSegments[1].Start, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[1].End, Is.EqualTo(new Position(2, 1)));
            Assert.That(corridor.LineSegments[2].Start, Is.EqualTo(new Position(3, 1)));
            Assert.That(corridor.LineSegments[2].End, Is.EqualTo(new Position(3, 1)));
        }

        [Test]
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

            Assert.(corridor, Is.Null);
        }

        [Test]
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
            Assert.That(corridor.LineSegments.Count, Is.EqualTo(1));
            Assert.That(corridor.LineSegments[0].Start, Is.EqualTo(new Position(5, 2)));
            Assert.That(corridor.LineSegments[0].End, Is.EqualTo(new Position(6, 2)));
            Assert.That(corridor.ConnectedRooms[0], Is.SameAs(firstRoom));
            Assert.That(corridor.ConnectedRooms[1], Is.SameAs(secondRoom));
            Assert.That(firstRoom.Corridors[0], Is.SameAs(corridor));
            Assert.That(secondRoom.Corridors[0], Is.SameAs(corridor));
        }

        [Test]
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

            Assert.That(roomsToJoin.Count, Is.EqualTo(2));
            Assert.That(firstListOfRooms[0], Is.EqualTo(roomsToJoin[0]));
            Assert.That(secondListOfRooms[0], Is.EqualTo(roomsToJoin[1]));
        }

        [Test]
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

            Assert.That(roomsToJoin.Count, Is.EqualTo(2));
            Assert.That(firstListOfRooms[0], Is.EqualTo(roomsToJoin[0]));
            Assert.That(secondListOfRooms[1], Is.EqualTo(roomsToJoin[1]));
        }

        [Test]
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

            Assert.That(roomsToJoin.Count, Is.EqualTo(2));
            Assert.That(firstListOfRooms[1], Is.EqualTo(roomsToJoin[0]));
            Assert.That(secondListOfRooms[1], Is.EqualTo(roomsToJoin[1]));
        }
    }
}
