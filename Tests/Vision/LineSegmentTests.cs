using System;
using NUnit.Framework;
using Tests.Factories;
using System.Collections.Generic;
using System.Linq;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Vision;
using Turnable.Utilities;

namespace Tests.Vision
{
    [TestFixture]
    public class LineSegmentTests
    {
        [Test]
        public void Constructor_GivenTwoAdjacentPositions_CreatesALineWithTwoPoints()
        {
            Position start = new Position(1, 1);

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Position end = start.NeighboringPosition(direction);

                LineSegment line = new LineSegment(start, end);

                Assert.That(line.Points.Count, Is.EqualTo(2));
                Assert.That(line.Points[0], Is.EqualTo(start));
                Assert.That(line.Points[1], Is.EqualTo(end));
            }
        }

        [Test]
        public void Constructor_GivenTwoDistantPoints_CreatesALine()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);

            LineSegment line = new LineSegment(start, end);

            Assert.That(line.Points.Count, Is.EqualTo(6));
            Assert.That(line.Points[0], Is.EqualTo(start));
            Assert.That(line.Points[1], Is.EqualTo(new Position(1, 2)));
            Assert.That(line.Points[2], Is.EqualTo(new Position(1, 3)));
            Assert.That(line.Points[3], Is.EqualTo(new Position(1, 4)));
            Assert.That(line.Points[4], Is.EqualTo(new Position(1, 5)));
            Assert.That(line.Points[5], Is.EqualTo(end));
        }

        [Test]
        public void Start_ReturnsTheVeryFirstPoint()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Assert.That(line.Start, Is.EqualTo(line.Points[0]));
        }

        [Test]
        public void End_ReturnsTheVeryLastPoint()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Assert.That(line.End, Is.EqualTo(line.Points[line.Points.Count - 1]));
        }

        [Test]
        public void IsVertical_IfLineSegmentIsVertical_ReturnsTrue()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.That(lineSegment.IsVertical(), Is.True);

            lineSegment = new LineSegment(new Position(0, 0), new Position(4, 0));
            
            Assert.That(lineSegment.IsVertical(), Is.False);

            lineSegment = new LineSegment(new Position(0, 0), new Position(4, 4));

            Assert.That(lineSegment.IsVertical(), Is.False);
        }

        [Test]
        public void IsHorizontal_IfLineSegmentIsHorizontal_ReturnsTrue()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(4, 0));

            Assert.That(lineSegment.IsHorizontal(), Is.True);

            lineSegment = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.That(lineSegment.IsHorizontal(), Is.False);

            lineSegment = new LineSegment(new Position(0, 0), new Position(4, 4));

            Assert.That(lineSegment.IsHorizontal(), Is.False);
        }

        [Test]
        public void GetRandomPoint_ReturnsARandomPointAlongTheLine()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Position randomPoint = line.GetRandomPoint();

            Assert.That(line.Points.Contains(randomPoint), Is.True);
        }

        [Test]
        public void GetMidpoint_ReturnsTheMidpointOfTheLine()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Position midpoint = line.GetMidpoint();

            // A line with an even number of points uses the (Number of Points)/2 point
            Assert.That(line.Points.Contains(midpoint), Is.True);
            Assert.That(midpoint, Is.EqualTo(line.Points[2]));

            start = new Position(1, 1);
            end = new Position(2, 5);
            line = new LineSegment(start, end);

            midpoint = line.GetMidpoint();

            // A line with an odd number of points returns the exact midpoint
            Assert.That(line.Points.Contains(midpoint), Is.True);
            Assert.That(midpoint, Is.EqualTo(line.Points[2]));
        }

        [Test]
        public void Intersects_GivenARectangleThatTheLineSegmentDoesNotIntersectWith_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(5, 5), new Position(10, 10));
            LineSegment lineSegment = new LineSegment(new Position(0, 1), new Position(4, 4));

            Assert.That(lineSegment.Intersects(rectangle), Is.False);

            lineSegment = new LineSegment(new Position(4, 5), new Position(4, 10));

            Assert.That(lineSegment.Intersects(rectangle), Is.False);
        }

        [Test]
        public void Intersects_GivenARectangleThatTheLineSegmentIntersectsWith_ReturnsTrue()
        {
            Rectangle rectangle = new Rectangle(new Position(5, 5), new Position(10, 10));
            LineSegment lineSegment = new LineSegment(new Position(0, 1), new Position(5, 5));

            Assert.That(lineSegment.Intersects(rectangle), Is.True);

            lineSegment = new LineSegment(new Position(15, 15), new Position(10, 10));

            Assert.That(lineSegment.Intersects(rectangle), Is.True);

            lineSegment = new LineSegment(new Position(15, 15), new Position(5, 5));

            Assert.That(lineSegment.Intersects(rectangle), Is.True);
        }

        [Test]
        public void Intersects_AskedToExcludeTheStartingAndEndingPoints_ReturnsFalseIfOnlyTheStartOrEndPointsIntersectWithARectangle()
        {
            Rectangle rectangle = new Rectangle(new Position(5, 5), new Position(10, 10));
            LineSegment lineSegment = new LineSegment(new Position(5, 5), new Position(4, 4));

            Assert.That(lineSegment.Intersects(rectangle, true), Is.False);

            lineSegment = new LineSegment(new Position(10, 10), new Position(11, 11));

            Assert.That(lineSegment.Intersects(rectangle, true), Is.False);

            lineSegment = new LineSegment(new Position(5, 5), new Position(7, 7));

            Assert.That(lineSegment.Intersects(rectangle, true), Is.True);
        }

        [Test]
        public void IsParallelTo_GivenTwoSegmentsThatAreParallelToEachOther_ReturnsTrue()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(1, 0), new Position(1, 4));

            Assert.That(first.IsParallelTo(second), Is.True);

            first = new LineSegment(new Position(0, 4), new Position(0, 0));
            second = new LineSegment(new Position(1, 4), new Position(1, 0));

            Assert.That(first.IsParallelTo(second), Is.True);

            first = new LineSegment(new Position(0, 0), new Position(4, 0));
            second = new LineSegment(new Position(0, 1), new Position(4, 1));

            Assert.That(first.IsParallelTo(second), Is.True);

            first = new LineSegment(new Position(4, 0), new Position(0, 0));
            second = new LineSegment(new Position(4, 1), new Position(0, 1));

            Assert.That(first.IsParallelTo(second), Is.True);
        }

        [Test]
        public void IsParallelTo_GivenTwoSegmentsOfTheSameLine_ReturnsTrue()
        {
            // The exact same segment
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.That(first.IsParallelTo(second), Is.True);

            // Two segments on the same line
            first = new LineSegment(new Position(0, 4), new Position(0, 0));
            second = new LineSegment(new Position(0, 4), new Position(0, 8));

            Assert.That(first.IsParallelTo(second), Is.True);
        }

        [Test]
        public void IsParallelTo_GivenTwoSegmentsThatAreNotParallelToEachOther_ReturnsFalse()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(4, 0));

            Assert.That(first.IsParallelTo(second), Is.False);

            first = new LineSegment(new Position(0, 0), new Position(0, 4));
            second = new LineSegment(new Position(0, 4), new Position(4, 4));

            Assert.That(first.IsParallelTo(second), Is.False);

            first = new LineSegment(new Position(5, 5), new Position(3, 3));
            second = new LineSegment(new Position(17, 13), new Position(4, 4));

            Assert.That(first.IsParallelTo(second), Is.False);
        }

        [Test]
        public void DistanceBetween_GivenTwoSegmentsThatAreNotParallel_ThrowsAnException()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(4, 0));

            Assert.That(() => first.DistanceBetween(second), Throws.ArgumentException);
        }

        [Test]
        public void DistanceBetween_GivenTwoParallelSegments_ReturnsTheDistanceBetweenThem()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(1, 0), new Position(1, 4));

            Assert.That(first.DistanceBetween(second), Is.EqualTo(1));

            first = new LineSegment(new Position(0, 0), new Position(0, 4));
            second = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.That(first.DistanceBetween(second), Is.EqualTo(0));

            first = new LineSegment(new Position(0, 2), new Position(4, 2));
            second = new LineSegment(new Position(8, 1), new Position(5, 1));

            Assert.That(first.DistanceBetween(second), Is.EqualTo(1));

            first = new LineSegment(new Position(2, 0), new Position(2, 4));
            second = new LineSegment(new Position(5, 0), new Position(5, 2));

            Assert.That(first.DistanceBetween(second), Is.EqualTo(3));
        }

        [Test]
        public void IsTouching_GivenTwoNonParallelSegments_ReturnsFalse()
        {
            LineSegment first = new LineSegment(new Position(0, 1), new Position(5, 7));
            LineSegment second = new LineSegment(new Position(0, 1), new Position(4, 6));

            Assert.That(first.IsTouching(second), Is.False);
        }

        [Test]
        public void IsTouching_GivenTwoParallelSegmentsThatTouchAlongTheirEntireLength_ReturnsTrue()
        {
            // * First line segment; - Second Line Segment
            // ****
            // ----
            LineSegment first = new LineSegment(new Position(0, 1), new Position(3, 1));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(3, 0));

            Assert.That(first.IsTouching(second), Is.True);

            // ----
            // ****
            Assert.IsTrue(second.IsTouching(first));

            // *-
            // *-
            // *-
            // *-
            first = new LineSegment(new Position(0, 0), new Position(0, 3));
            second = new LineSegment(new Position(1, 0), new Position(1, 3));

            Assert.That(first.IsTouching(second), Is.True);

            // -*
            // -*
            // -*
            // -*
            Assert.That(second.IsTouching(first), Is.True);
        }

        [Test]
        public void IsTouching_GivenTwoParallelSegmentsThatTouchAlongSomePartOfTheirLengtth_ReturnsTrue()
        {
            // * First line segment; - Second Line Segment
            // ****
            //    ----
            LineSegment first = new LineSegment(new Position(0, 1), new Position(3, 1));
            LineSegment second = new LineSegment(new Position(3, 0), new Position(6, 0));

            Assert.That(first.IsTouching(second), Is.True);

            // ----
            //    ****
            Assert.That(second.IsTouching(first), Is.True);

            // *
            // *
            // *
            // *-
            //  -
            //  -
            //  -
            first = new LineSegment(new Position(0, 3), new Position(0, 6));
            second = new LineSegment(new Position(1, 0), new Position(1, 3));

            Assert.That(first.IsTouching(second), Is.True);

            // -
            // -
            // -
            // -*
            //  *
            //  *
            //  *
            Assert.That(second.IsTouching(first), Is.True);
        }

        [Test]
        public void IsTouching_GivenTwoCloseParallelSegmentsThatDontTouch_ReturnsFalse()
        {
            // * First line segment; - Second Line Segment
            // ****
            //     ----
            LineSegment first = new LineSegment(new Position(0, 1), new Position(3, 1));
            LineSegment second = new LineSegment(new Position(4, 0), new Position(7, 0));

            Assert.That(first.IsTouching(second), Is.False);

            // ----
            //     ****
            Assert.That(second.IsTouching(first), Is.False);

            // *
            // *
            // *
            // *
            //  -
            //  -
            //  -
            //  -
            first = new LineSegment(new Position(0, 4), new Position(0, 7));
            second = new LineSegment(new Position(1, 0), new Position(1, 3));

            Assert.That(first.IsTouching(second), Is.False);

            // -
            // -
            // -
            // -
            //  *
            //  *
            //  *
            //  *
            Assert.That(second.IsTouching(first), Is.False);
        }

        [Test]
        public void IsTouching_GivenTwoParallelSegmentsThatDontTouch_ReturnsFalse()
        {
            // * First line segment; - Second Line Segment
            // ****
            //     
            //     
            //     ----
            LineSegment first = new LineSegment(new Position(0, 3), new Position(3, 3));
            LineSegment second = new LineSegment(new Position(4, 0), new Position(7, 0));

            Assert.That(first.IsTouching(second), Is.False);

            // ----
            //     
            //     
            //     ****
            Assert.That(second.IsTouching(first), Is.False);

            // *
            // *
            // *
            // *
            //    -
            //    -
            //    -
            //    -
            first = new LineSegment(new Position(0, 4), new Position(0, 7));
            second = new LineSegment(new Position(3, 0), new Position(3, 3));

            Assert.That(first.IsTouching(second), Is.False);

            // -
            // -
            // -
            // -
            //    *
            //    *
            //    *
            //    *
            Assert.That(second.IsTouching(first), Is.False);
        }

        // Equals Tests
        [Test]
        public void Equals_FromIEquatableTInterface_CanCompareLineSegments()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            LineSegment lineSegment2 = new LineSegment(new Position(0, 0), new Position(1, 1));

            Assert.That(lineSegment.Equals(lineSegment2), Is.True);

            lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            lineSegment2 = new LineSegment(new Position(1, 1), new Position(0, 0));

            Assert.That(lineSegment.Equals(lineSegment2), Is.True);

            lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            lineSegment2 = new LineSegment(new Position(0, 2), new Position(2, 2));

            Assert.That(lineSegment.Equals(lineSegment2), Is.False);
        }

        [Test]
        public void Equals_FromIEquatableInterface_CanCompareLineSegmentToNull()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));

            Assert.That(lineSegment.Equals((Position)null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanCompareLineSegments()
        {
            Object lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            Object lineSegment2 = new LineSegment(new Position(0, 0), new Position(1, 1));

            Assert.That(lineSegment.Equals(lineSegment2), Is.True);

            lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            lineSegment2 = new LineSegment(new Position(1, 1), new Position(0, 0));

            Assert.That(lineSegment.Equals(lineSegment2), Is.True);

            lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            lineSegment2 = new LineSegment(new Position(0, 2), new Position(2, 2));

            Assert.That(lineSegment.Equals(lineSegment2), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanCompareLineSegmentToNull()
        {
            Object lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));

            Assert.That(lineSegment.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_ReturnsFalseIfOtherObjectIsNotALineSegment()
        {
            Object lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));

            Assert.That(lineSegment.Equals(new Object()), Is.False);
        }

        [Test]
        public void EqualityOperator_IsImplemented()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            LineSegment lineSegment2 = new LineSegment(new Position(0, 0), new Position(1, 1));

            Assert.That(lineSegment == lineSegment2, Is.True);
        }

        [Test]
        public void InequalityOperator_IsImplemented()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1));
            LineSegment lineSegment2 = new LineSegment(new Position(0, 0), new Position(2, 2));

            Assert.That(lineSegment != lineSegment2, Is.True);
        }

        [Test]
        public void EqualityOperator_CanCompareLineSegmentToNull()
        {
            LineSegment lineSegment = null;

            Assert.That(lineSegment == null, Is.True);
        }

        [Test]
        public void InequalityOperator_CanCompareLineSegmentToNull()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1)); ;

            Assert.That(lineSegment != null, Is.True);
        }

        [Test]
        public void GetHashCode_IsOverridenToReturnASuitableHashCode()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(1, 1)); ;
            int calculatedHash;

            // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                hash = hash * 16777619 ^ lineSegment.Start.GetHashCode();
                hash = hash * 16777619 ^ lineSegment.End.GetHashCode();
                calculatedHash = hash;
            }

            Assert.That(lineSegment.GetHashCode(), Is.EqualTo(calculatedHash));
        }
    }
}

