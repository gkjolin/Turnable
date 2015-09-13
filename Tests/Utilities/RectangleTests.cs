using System;
using NUnit.Framework;
using Turnable.Components;
using Turnable.Utilities;
using Turnable.LevelGenerators;
using System.Collections.Generic;
using Turnable.Vision;

namespace Tests.Utilities
{
    [TestFixture]
    public class RectangleTests
    {
        [Test]
        public void Constructor_GivenTwoCornersOfTheRectangle_InitializesAllProperties()
        {
            Rectangle rectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(0, 0)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(4, 4)));
            Assert.That(rectangle.Width, Is.EqualTo(5));
            Assert.That(rectangle.Height, Is.EqualTo(5));

            // Check if four edges (one for each edge of the rectangle) are initialized.
            // Also check to see if the edges are ordered counter-clockwise.
            Assert.That(rectangle.Edges.Count, Is.EqualTo(4));
            Assert.That(rectangle.Edges[0].Start, Is.EqualTo(new Position(0, 0)));
            Assert.That(rectangle.Edges[0].End, Is.EqualTo(new Position(4, 0)));
            Assert.That(rectangle.Edges[1].Start, Is.EqualTo(new Position(4, 0)));
            Assert.That(rectangle.Edges[1].End, Is.EqualTo(new Position(4, 4)));
            Assert.That(rectangle.Edges[2].Start, Is.EqualTo(new Position(4, 4)));
            Assert.That(rectangle.Edges[2].End, Is.EqualTo(new Position(0, 4)));
            Assert.That(rectangle.Edges[3].Start, Is.EqualTo(new Position(0, 4)));
            Assert.That(rectangle.Edges[3].End, Is.EqualTo(new Position(0, 0)));
        }

        [Test]
        public void Constructor_GivenTheBottomLeftCornerAndTheWidthAndTheHeight_InitializesAllProperties()
        {
            Rectangle rectangle = new Rectangle(new Position(0, 0), 5, 5);

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(0, 0)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(4, 4)));
            Assert.That(rectangle.Width, Is.EqualTo(5));
            Assert.That(rectangle.Height, Is.EqualTo(5));
        }

        [Test]
        public void BuildRandomRectangle_GivenSomeBounds_ReturnsARandomlySizedRectangleWithinThoseBounds()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 5, 4);
            Rectangle rectangle = Rectangle.BuildRandomRectangle(bounds);

            Assert.That(rectangle.Width, Is.InRange(0, 5));
            Assert.That(rectangle.Height, Is.InRange(0, 4));
            Assert.That(bounds.Contains(rectangle), Is.True);
        }

        [Test]
        public void Constructor_GivenAnyTwoCorners_CorrectlyCalculatesBottomLeftAndTopRightCorners()
        {
            // BottomLeft, TopRight corners. Handled by default.

            // BottomRight, TopLeft
            Rectangle rectangle = new Rectangle(new Position(3, 1), new Position(1, 4));

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(1, 1)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(3, 4)));

            // TopRight, BottomLeft
            rectangle = new Rectangle(new Position(4, 4), new Position(1, 1));

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(1, 1)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(4, 4)));

            // TopLeft, BottomRight
            rectangle = new Rectangle(new Position(1, 4), new Position(3, 1));

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(1, 1)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(3, 4)));

            // Rectangles with a width of 1
            rectangle = new Rectangle(new Position(4, 4), new Position(4, 1));

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(4, 1)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(4, 4)));

            // Rectangles with a height of 1
            rectangle = new Rectangle(new Position(4, 3), new Position(1, 3));

            Assert.That(rectangle.BottomLeft, Is.EqualTo(new Position(1, 3)));
            Assert.That(rectangle.TopRight, Is.EqualTo(new Position(4, 3)));
        }

        [Test]
        public void Contains_GivenAPositionWithinTheRectangleIncludingTheEdgesOfTheRectangle_ReturnsTrue()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Assert.That(rectangle.Contains(new Position(2, 3)), Is.True);
            Assert.That(rectangle.Contains(new Position(1, 1)), Is.True);
            Assert.That(rectangle.Contains(new Position(5, 1)), Is.True);
            Assert.That(rectangle.Contains(new Position(1, 4)), Is.True);
            Assert.That(rectangle.Contains(new Position(5, 4)), Is.True);
        }

        [Test]
        public void Contains_GivenAPositionOutsideTheRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Assert.That(rectangle.Contains(new Position(0, 0)), Is.False);
            Assert.That(rectangle.Contains(new Position(0, 1)), Is.False);
            Assert.That(rectangle.Contains(new Position(6, 1)), Is.False);
            Assert.That(rectangle.Contains(new Position(1, 5)), Is.False);
            Assert.That(rectangle.Contains(new Position(5, 6)), Is.False);
        }

        [Test]
        public void Contains_GivenARectangleThatFitsIntoTheOtherRectangleEvenIfSomeEdgesOfBothRectangleTouch_ReturnsTrue()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            // A rectangle totally within the first rectangle
            Rectangle otherRectangle = new Rectangle(new Position(2, 2), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.True);

            // Rectangles that share some of the edges of the first rectangle
            otherRectangle = new Rectangle(new Position(1, 1), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.True);
            otherRectangle = new Rectangle(new Position(3, 1), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.True);
            otherRectangle = new Rectangle(new Position(3, 4), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.True);
            otherRectangle = new Rectangle(new Position(1, 4), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.True);

            // Rectangle that is the exact size of the first rectangle
            otherRectangle = new Rectangle(new Position(1, 1), 5, 4);
            Assert.That(rectangle.Contains(otherRectangle), Is.True);
        }

        [Test]
        public void Contains_GivenARectangleThatIsPartiallyInsideAndPartiallyOutsideTheOtherRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Rectangle otherRectangle = new Rectangle(new Position(0, 1), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.False);

            otherRectangle = new Rectangle(new Position(5, 1), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.False);

            otherRectangle = new Rectangle(new Position(1, 5), 2, 2);
            Assert.That(rectangle.Contains(otherRectangle), Is.False);

            otherRectangle = new Rectangle(new Position(5, 5), 2, 2);
            Assert.That(rectangle.Contains(otherRectangle), Is.False);
        }

        [Test]
        public void Contains_GivenARectangleThatIsCompletelyOutsideTheOtherRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Rectangle otherRectangle = new Rectangle(new Position(7, 7), 2, 1);
            Assert.That(rectangle.Contains(otherRectangle), Is.False);
        }

        [Test]
        public void Contains_GivenARectangleThatIsBiggerAndPlacedOverTheOtherRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Rectangle otherRectangle = new Rectangle(new Position(0, 0), 7, 7);
            Assert.That(rectangle.Contains(otherRectangle), Is.False);
        }

        [Test]
        public void IsTouching_GivenTwoRectanglesThatAreTouchingEachOtherAlongOneWholeEdge_ReturnsTrue()
        {
            // * First rectangle 
            // : Second rectangle
            // First rectangle to left of second rectangle
            // *****:::::
            // *****:::::
            // *****::::: 
            // *****:::::
            // *****:::::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(9, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);

            // First rectangle to right of second rectangle
            // :::::*****
            // :::::*****
            // :::::*****
            // :::::*****
            // :::::*****
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);

            // First rectangle above second rectangle
            // *****
            // *****
            // *****
            // *****
            // *****
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(0, 5), new Position(4, 9));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);

            // First rectangle below second rectangle
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            // *****
            // *****
            // *****
            // *****
            // *****
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);
        }

        [Test]
        public void IsTouching_GivenTwoRectanglesThatAreTouchingEachOtherAtLeastAtOnePointAlongOneEdge_ReturnsTrue()
        {
            // *****
            // *****
            // *****
            // *****
            // *****:::::
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // First rectangle to left and above second rectangle
            Rectangle firstRectangle = new Rectangle(new Position(0, 8), new Position(4, 4));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(5, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);

            // :::::
            // :::::
            // :::::
            // :::::
            // :::::*****
            //      *****
            //      *****
            //      *****
            //      *****
            // First rectangle to right and below second rectangle
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);

            // First rectangle to right and above second rectangle
            //      *****
            //      *****
            //      *****
            //      *****
            // :::::*****
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(5, 8), new Position(9, 4));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);

            // First rectangle to left and below second rectangle
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // *****:::::
            // *****
            // *****
            // *****
            // *****
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.True);
        }

        [Test]
        public void IsTouching_GivenTwoRectanglesThatAreDiagonallyPlacedToEachOtherWithNotEvenOnePointTouchingAlongAEdge_ReturnsFalse()
        {
            // *****
            // *****
            // *****
            // *****
            // *****
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // First rectangle to left and above second rectangle
            Rectangle firstRectangle = new Rectangle(new Position(0, 9), new Position(4, 5));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(9, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            //      *****
            //      *****
            //      *****
            //      *****
            //      *****
            // First rectangle to right and below second rectangle
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // First rectangle to right and above second rectangle
            //      *****
            //      *****
            //      *****
            //      *****
            //      *****
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(5, 9), new Position(9, 5));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // First rectangle to left and below second rectangle
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // *****
            // *****
            // *****
            // *****
            // *****
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);
        }

        [Test]
        public void IsTouching_GivenTwoRectanglesThatAreSeparatedByAtleastOnePointFromEachOther_ReturnsFalse()
        {
            // * First rectangle 
            // : Second rectangle
            // First rectangle to left of second rectangle
            // ***** :::::
            // ***** :::::
            // ***** ::::: 
            // ***** :::::
            // ***** :::::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));
            Rectangle secondRectangle = new Rectangle(new Position(6, 0), new Position(10, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // First rectangle to right of second rectangle
            // ::::: *****
            // ::::: *****
            // ::::: *****
            // ::::: *****
            // ::::: *****
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // First rectangle above second rectangle
            // *****
            // *****
            // *****
            // *****
            // *****
            // 
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(0, 6), new Position(4, 10));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // First rectangle below second rectangle
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            // 
            // *****
            // *****
            // *****
            // *****
            // *****
            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);

            // ***
            // ***
            // ***  
            // ***  
            // ***  
            //      :::
            //      :::
            //      :::
            firstRectangle = new Rectangle(new Position(0, 3), new Position(2, 7));
            secondRectangle = new Rectangle(new Position(5, 0), new Position(7, 2));

            Assert.That(firstRectangle.IsTouching(secondRectangle), Is.False);
        }

        [Test]
        public void GetClosestEdges_GivenTwoRectanglesWithSuitableParallelEdges_ReturnsTwoEdgesThatAreClosestAndParallelToEachOther()
        {
            // * First rectangle, : Second Rectangle, C Closest edge
            // **C
            // **C
            // **C  C::
            // **C  C::
            // **C  C::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(2, 4));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(7, 2));

            List<LineSegment> closestEdges = firstRectangle.GetClosestEdges(secondRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(2, 0)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(2, 4)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(5, 2)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(5, 0)));

            // * First rectangle, : Second Rectangle, F Facing edge
            // ::C
            // ::C
            // ::C  C**
            // ::C  C**
            // ::C  C**
            closestEdges = secondRectangle.GetClosestEdges(firstRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(5, 2)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(5, 0)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(2, 0)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(2, 4)));

            // *****
            // *****
            // CCCCC
            //
            //
            // CCCC
            // ::::
            firstRectangle = new Rectangle(new Position(0, 4), new Position(4, 6));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(3, 1));

            closestEdges = firstRectangle.GetClosestEdges(secondRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(0, 4)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(4, 4)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(3, 1)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(0, 1)));

            // :::::
            // :::::
            // CCCCC
            //
            //
            // CCCC
            // ****
            closestEdges = secondRectangle.GetClosestEdges(firstRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(3, 1)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(0, 1)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(0, 4)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(4, 4)));
        }

        [Test]
        public void GetClosestEdges_GivenTwoRectanglesWithParallelEdgesThatAreTouching_ReturnsThoseParallelEdges()
        {
            // * First rectangle, : Second Rectangle, C Closest edge
            // **C
            // **C
            // **CC::
            // **CC::
            // **CC::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(2, 4));
            Rectangle secondRectangle = new Rectangle(new Position(3, 0), new Position(5, 2));

            List<LineSegment> closestEdges = firstRectangle.GetClosestEdges(secondRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(2, 0)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(2, 4)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(3, 2)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(3, 0)));

            // * First rectangle, : Second Rectangle, C Closest edge
            // ***
            // ***
            // ***
            // ***
            // CCC
            //   CCC
            //   :::
            //   :::
            firstRectangle = new Rectangle(new Position(0, 3), new Position(2, 7));
            secondRectangle = new Rectangle(new Position(2, 0), new Position(4, 2));

            closestEdges = firstRectangle.GetClosestEdges(secondRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(0, 3)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(2, 3)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(4, 2)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(2, 2)));
        }

        [Test]
        public void GetClosestEdges_GivenTwoRectanglesWithSuitableParallelAndNonParallelEdges_PrefersToReturnTheParallelEdges()
        {
            // * First rectangle, : Second Rectangle, F Facing edge
            // **C
            // **C
            // **C  
            // **C  
            // **C  
            //      C::
            //      C::
            //      C::
            Rectangle firstRectangle = new Rectangle(new Position(0, 3), new Position(2, 7));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(7, 2));

            List<LineSegment> closestEdges = firstRectangle.GetClosestEdges(secondRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(2, 3)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(2, 7)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(5, 2)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(5, 0)));
        }

        [Test]
        public void GetClosestEdges_GivenTwoRectanglesWithNoSuitableParallelEdges_ReturnsTheClosesNonParallelEdges()
        {
            // ***
            // ***
            // ***  
            // ***  
            // CCC  
            //    C::
            //    C::
            //    C::
            Rectangle firstRectangle = new Rectangle(new Position(0, 3), new Position(2, 7));
            Rectangle secondRectangle = new Rectangle(new Position(3, 0), new Position(5, 2));

            List<LineSegment> closestEdges = firstRectangle.GetClosestEdges(secondRectangle);

            Assert.That(closestEdges.Count, Is.EqualTo(2));
            Assert.That(closestEdges[0].Start, Is.EqualTo(new Position(0, 3)));
            Assert.That(closestEdges[0].End, Is.EqualTo(new Position(2, 3)));
            Assert.That(closestEdges[1].Start, Is.EqualTo(new Position(3, 2)));
            Assert.That(closestEdges[1].End, Is.EqualTo(new Position(3, 0)));
        }
    }
}
