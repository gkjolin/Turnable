using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class CorridorTests
    {
        [TestMethod]
        public void DefaulyConstructor_InitializesAllProperties()
        {
            Corridor corridor = new Corridor();

            Assert.IsNotNull(corridor.Segments);
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            List<Segment> segments = new List<Segment>();
            segments.Add(new Segment(new Position(0, 0), new Position(0, 1)));

            Corridor corridor = new Corridor(segments);

            Assert.AreEqual(corridor.Segments, segments);
        }
    }
}
