using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;
using Turnable.Vision;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class CorridorTests
    {
        [TestMethod]
        public void DefaultConstructor_InitializesAllProperties()
        {
            Corridor corridor = new Corridor();

            Assert.IsNotNull(corridor.Segments);
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            List<LineSegment> segments = new List<LineSegment>();
            segments.Add(new LineSegment(new Position(0, 0), new Position(0, 1)));

            Corridor corridor = new Corridor(segments);

            Assert.AreEqual(corridor.Segments, segments);
        }
    }
}
