using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class SegmentTests
    {
        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Segment segment = new Segment(new Position(0, 0), new Position(0, 1));

            Assert.AreEqual(new Position(0, 0), segment.Start);
            Assert.AreEqual(new Position(0, 1), segment.End);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_AskedToConstructASegmentThatIsntFullyHorizontalOrVertical_RaisesAnException()
        {
            Segment segment = new Segment(new Position(0, 0), new Position(1, 1));
        }
    }
}
