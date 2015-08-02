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

        [TestMethod]
        public void GetRandomPoint_GivenASegment_ReturnsARandomPointAlongTheSegment()
        {
            Segment segment = new Segment(new Position(0, 0), new Position(0, 4));
            
            Position randomPoint = segment.GetRandomPoint();

            Assert.AreEqual(0, randomPoint.X);
            Assert.IsTrue(randomPoint.Y >= 0 && randomPoint.Y <= 4);

            segment = new Segment(new Position(0, 0), new Position(4, 0));

            randomPoint = segment.GetRandomPoint();

            Assert.AreEqual(0, randomPoint.Y);
            Assert.IsTrue(randomPoint.X >= 0 && randomPoint.X <= 4);
        }

        [TestMethod]
        public void GetRandomPoint_GivenASegmentWhoseStartAndEndPointsAreNotOrderedCorrectly_ReturnsARandomPointAlongTheSegment()
        {
            Segment segment = new Segment(new Position(0, 4), new Position(0, 0));

            Position randomPoint = segment.GetRandomPoint();

            Assert.AreEqual(0, randomPoint.X);
            Assert.IsTrue(randomPoint.Y >= 0 && randomPoint.Y <= 4);

            segment = new Segment(new Position(4, 0), new Position(0, 0));

            randomPoint = segment.GetRandomPoint();

            Assert.AreEqual(0, randomPoint.Y);
            Assert.IsTrue(randomPoint.X >= 0 && randomPoint.X <= 4);
        }

        [TestMethod]
        public void IsParallelTo_GivenTwoSegments_ReturnsTrueIfTheSegmentsAreParallelToEachOther()
        {
            Segment first = new Segment(new Position(0, 0), new Position(0, 4));
            Segment second = new Segment(new Position(1, 0), new Position(1, 4));

            Assert.IsTrue(first.IsParallelTo(second));

            first = new Segment(new Position(0, 4), new Position(0, 0));
            second = new Segment(new Position(1, 4), new Position(1, 0));

            Assert.IsTrue(first.IsParallelTo(second));

            first = new Segment(new Position(0, 0), new Position(4, 0));
            second = new Segment(new Position(0, 1), new Position(4, 1));

            Assert.IsTrue(first.IsParallelTo(second));

            first = new Segment(new Position(4, 0), new Position(0, 0));
            second = new Segment(new Position(4, 1), new Position(0, 1));

            Assert.IsTrue(first.IsParallelTo(second));
        }

        [TestMethod]
        public void IsParallelTo_GivenTwoSegmentsOfTheSameLine_ReturnsTrue()
        {
            // The exact same segment
            Segment first = new Segment(new Position(0, 0), new Position(0, 4));
            Segment second = new Segment(new Position(0, 0), new Position(0, 4));

            Assert.IsTrue(first.IsParallelTo(second));

            // Two segments on the same line
            first = new Segment(new Position(0, 4), new Position(0, 0));
            second = new Segment(new Position(0, 4), new Position(0, 8));

            Assert.IsTrue(first.IsParallelTo(second));
        }

        [TestMethod]
        public void IsParallelTo_GivenTwoSegments_ReturnsFalseIfTheSegmentsAreNotParallelToEachOther()
        {
            Segment first = new Segment(new Position(0, 0), new Position(0, 4));
            Segment second = new Segment(new Position(0, 0), new Position(4, 0));

            Assert.IsFalse(first.IsParallelTo(second));

            first = new Segment(new Position(0, 0), new Position(0, 4));
            second = new Segment(new Position(0, 4), new Position(4, 4));

            Assert.IsFalse(first.IsParallelTo(second));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DistanceBetween_GivenTwoSegmentsThatAreNotParallel_ThrowsAnException()
        {
            Segment first = new Segment(new Position(0, 0), new Position(0, 4));
            Segment second = new Segment(new Position(0, 0), new Position(4, 0));

            first.DistanceBetween(second);
        }

        [TestMethod]
        public void DistanceBetween_GivenTwoParallelSegments_ReturnsTheDistanceBetweenThem()
        {
            Segment first = new Segment(new Position(0, 0), new Position(0, 4));
            Segment second = new Segment(new Position(1, 0), new Position(1, 4));

            Assert.AreEqual(1, first.DistanceBetween(second));

            first = new Segment(new Position(0, 0), new Position(0, 4));
            second = new Segment(new Position(0, 0), new Position(0, 4));

            Assert.AreEqual(0, first.DistanceBetween(second));
        }
    }
}
