using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class RoomTests
    {
        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Chunk chunk = new Chunk(new Position(0, 0), 7, 5);
            Room room = new Room(chunk, new Position(1, 1), new Position(2, 2));

            Assert.AreEqual(chunk, room.ParentChunk);
            Assert.AreEqual(new Position(1, 1), room.TopLeft);
            Assert.AreEqual(new Position(2, 2), room.BottomRight);
        }

        [TestMethod]
        public void Constructor_GivenAnyTwoCorners_CorrectlyCalculatesTopLeftAndBottomRightCorner()
        {
            Chunk chunk = new Chunk(new Position(0, 0), 7, 4);

            // TopLeft, BottomRight corners. Handled by default.

            // TopRight, BottomLeft
            Room room = new Room(chunk, new Position(3, 1), new Position(1, 4));

            Assert.AreEqual(new Position(1, 1), room.TopLeft);
            Assert.AreEqual(new Position(3, 4), room.BottomRight);

            // BottomRight, TopLeft corners
            room = new Room(chunk, new Position(2, 2), new Position(1, 1));

            Assert.AreEqual(new Position(1, 1), room.TopLeft);
            Assert.AreEqual(new Position(2, 2), room.BottomRight);

            // BottomLeft, TopRight
            room = new Room(chunk, new Position(1, 4), new Position(4, 1));

            Assert.AreEqual(new Position(1, 1), room.TopLeft);
            Assert.AreEqual(new Position(4, 4), room.BottomRight);

            // Rooms with a width of 1
            room = new Room(chunk, new Position(4, 4), new Position(4, 1));

            Assert.AreEqual(new Position(4, 1), room.TopLeft);
            Assert.AreEqual(new Position(4, 4), room.BottomRight);

            // Rooms with a height of 1
            room = new Room(chunk, new Position(4, 3), new Position(1, 3));

            Assert.AreEqual(new Position(1, 3), room.TopLeft);
            Assert.AreEqual(new Position(4, 3), room.BottomRight);
        }

        // TODO: Test that any corners are out of the chunk, but use a BoundsChecker. 

        [TestMethod]
        public void Constructor_GivenOnlyAParentChunk_ConstructsARandomlySizedRoom()
        {
            Chunk chunk = new Chunk(new Position(0, 0), 7, 4);

            Room room = new Room(chunk);

            Assert.IsNotNull(room.TopLeft);
            Assert.IsTrue(room.TopLeft.X >= 0 && room.TopLeft.X <= 6);
            Assert.IsTrue(room.TopLeft.Y >= 0 && room.TopLeft.Y <= 4);
            Assert.IsNotNull(room.BottomRight);
            Assert.IsTrue(room.BottomRight.X >= 0 && room.BottomRight.X <= 6);
            Assert.IsTrue(room.BottomRight.Y >= 0 && room.BottomRight.Y <= 4);
        }
    }
}
