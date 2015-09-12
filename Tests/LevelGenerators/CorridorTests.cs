using System;
using NUnit.Framework;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;
using Turnable.Vision;
using Turnable.Utilities;

namespace Tests.LevelGenerators
{
    [TestFixture]
    public class CorridorTests
    {
        [Test]
        public void DefaultConstructor_InitializesAllProperties()
        {
            Corridor corridor = new Corridor();

            Assert.That(corridor.LineSegments, Is.Not.Null);
            Assert.That(corridor.ConnectedRooms, Is.Not.Null);
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            Chunk chunk = new Chunk(new Rectangle(new Position(0, 0), new Position(100, 100)));
            Room firstRoom = new Room(chunk, new Rectangle(new Position(0, 0), new Position(1, 1)));
            Room secondRoom = new Room(chunk, new Rectangle(new Position(0, 0), new Position(1, 1)));
            List<LineSegment> segments = new List<LineSegment>();
            segments.Add(new LineSegment(new Position(0, 0), new Position(0, 1)));

            Corridor corridor = new Corridor(firstRoom, secondRoom, segments);

            Assert.That(corridor.LineSegments, Is.SameAs(segments));
            Assert.That(corridor.ConnectedRooms[0], Is.SameAs(firstRoom));
            Assert.That(corridor.ConnectedRooms[1], Is.SameAs(secondRoom));
        }
    }
}
