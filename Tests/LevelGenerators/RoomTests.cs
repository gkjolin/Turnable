using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;
using Turnable.Utilities;
using Turnable.Api;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class RoomTests
    {
        [TestMethod]
        public void Room_ImplementsTheIBoundedInterface()
        {
            Rectangle chunkBounds = new Rectangle(new Position(0, 0), 7, 5);
            Chunk chunk = new Chunk(chunkBounds);
            Rectangle roomBounds = new Rectangle(new Position(1, 1), new Position(2, 2));
            Room room = new Room(chunk, roomBounds);

            Assert.IsInstanceOfType(room, typeof(IBounded));
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Rectangle chunkBounds = new Rectangle(new Position(0, 0), 7, 5);
            Chunk chunk = new Chunk(chunkBounds);
            Rectangle roomBounds = new Rectangle(new Position(1, 1), new Position(2, 2));
            Room room = new Room(chunk, roomBounds);

            Assert.AreEqual(chunk, room.ParentChunk);
            Assert.AreEqual(roomBounds, room.Bounds);
            Assert.AreEqual(room, chunk.Room);
            Assert.IsNotNull(room.Corridors);
        }

        [TestMethod]
        public void ToString_DisplaysBottomLeftAndTopLeftCorners()
        {
            Chunk parentChunk = new Chunk(new Rectangle(new Position(0, 0), new Position(100, 100)));
            Room room = new Room(parentChunk, new Rectangle(new Position(0, 0), new Position(10, 10)));

            Assert.AreEqual("BottomLeft: (0, 0); TopRight: (10, 10)", room.ToString());
        }

        // TODO: Test that if any corners are out of the chunk, the construction is invalid (use the bounds checking feature of parent chunk)
    }
}
