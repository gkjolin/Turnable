using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using System.Collections.Generic;
using System.Linq;
using Turnable.Api;
using Turnable.Vision;
using Turnable.Components;

namespace Tests.Vision
{
    [TestClass]
    public class VisionCalculatorTests
    {
        private ILevel _level;
        private IVisionCalculator _visionCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _visionCalculator = new VisionCalculator(_level);
        }

        [TestMethod]
        public void Constructor_SuccessfullyInitializesAllProperties()
        {
            IVisionCalculator visionCalculator = new VisionCalculator(_level);

            Assert.AreEqual(visionCalculator.Level, _level);
        }

        [TestMethod]
        public void CalculateSlope_CorrectlyCalculatesASlopeBetweenTwoPoints()
        {
            double slope = _visionCalculator.CalculateSlope(0, 0, 1, 1);
            Assert.AreEqual(1.0, slope);
        }

        [TestMethod]
        public void CalculateSlope_CorrectlyCalculatesAnInverseSlopeBetweenTwoPoints()
        {
            double slope = _visionCalculator.CalculateSlope(4, 2, 3, 4, true);
            Assert.AreEqual(-2.0, slope);
        }

        [TestMethod]
        public void CalculateVisibleDistance_CorrectlyCalculatesTheSquaredDistanceBetweenTwoPoints()
        {
            int visibleDistance = _visionCalculator.CalculateVisibleDistance(0, 0, 1, 1);
            Assert.AreEqual(2, visibleDistance);

            visibleDistance = _visionCalculator.CalculateVisibleDistance(4, 2, 3, 4);
            Assert.AreEqual(5, visibleDistance);
        }

        //[TestMethod]
        //public void VisionCalculator_CanEnableAndDisableItself()
        //{
        //    _visionCalculator.Disable();
        //    Assert.IsFalse(_visionCalculator.IsEnabled);
        //    _visionCalculator.Enable();
        //    Assert.IsTrue(_visionCalculator.IsEnabled);
        //}

        //--------------------------------
        // FOV Calculation Examples
        //--------------------------------

        [TestMethod]
        public void CalculateVisiblePositions_ForAVisualRangeOf0_ReturnsOnlyTheOrigin()
        {
            IEnumerable<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(7, 14, 0);

            Assert.AreEqual(1, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 14)));
        }

        [TestMethod]
        public void VisionCalculator_ForAVisualRangeOf1AndNoObstacles_ReturnsTheStartingPositionAndAllPositionsAdjacentToTheOrigin()
        {
            // The FOV algorithm creates a cross for a VisualRange of 1
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(7, 3, 1);

            Assert.AreEqual(5, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 3)));
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 2)));
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 4)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 3)));
            Assert.IsTrue(visiblePositions.Contains(new Position(8, 3)));
        }

        [TestMethod]
        public void VisionCalculator_ForAVisualRangeOf1_IncludesObstaclesInTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(6, 4, 1);

            IEnumerable<Position> distinctVisiblePositions = visiblePositions.Distinct<Position>();

            Assert.AreEqual(5, distinctVisiblePositions.Count<Position>());
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(6, 4)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(6, 5)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(6, 3)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(5, 4)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 4)));
        }

        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf1_IncludesCharactersInTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(2, 10, 1);

        //    IEnumerable<Position> distinctVisiblePositions = visiblePositions.Distinct<Position>();

        //    Assert.AreEqual(5, distinctVisiblePositions.Count<Position>());
        //    Assert.IsTrue(distinctVisiblePositions.Contains(new Position(2, 10)));
        //    Assert.IsTrue(distinctVisiblePositions.Contains(new Position(2, 11)));
        //    Assert.IsTrue(distinctVisiblePositions.Contains(new Position(2, 9)));
        //    Assert.IsTrue(distinctVisiblePositions.Contains(new Position(3, 10)));
        //    Assert.IsTrue(distinctVisiblePositions.Contains(new Position(1, 10)));
        //}

        // Testing each octant in the FOV with a VisualRange = 2
        // Obstacle at 6,5
        // With a VisualRange of 2 using the current algorithm, only obstacles directly to the E, N, W or S will block off a visible position
        //   X
        //  XXX
        // XXOXX
        //  XXX
        //   X
        // With a Visual Range of 2, the FOV is every adjacent tile as well as two tiles in the E, N, W and S direction. 

        // Obstacle to the N
        [TestMethod]
        public void CalculateVisiblePositions_ForAVisualRangeOf2AndObstacleToTheNorth_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(6, 4, 2);

            Assert.AreEqual(12, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 4)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
            Assert.IsFalse(visiblePositions.Contains(new Position(6, 6)));
        }

        // Obstacle to the NE
        [TestMethod]
        public void CalculateVisiblePositions_ForAVisualRangeOf2AndObstacleToTheNorthEast_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(5, 4, 2);

            Assert.AreEqual(13, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(5, 4)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
        }

        // Obstacle to the E
        [TestMethod]
        public void CalculateVisiblePositions_ForAVisualRangeOf2AndObstacleToTheEast_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(5, 5, 2);

            Assert.AreEqual(12, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(5, 5)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
            Assert.IsFalse(visiblePositions.Contains(new Position(7, 5)));
        }

        // Obstacle to the SE
        [TestMethod]
        public void CalculateVisiblePositions_ForAVisualRangeOf2AndObstacleToTheSouthEast_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(5, 6, 2);

            Assert.AreEqual(13, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(5, 6)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
        }

        // Obstacle to the S
        [TestMethod]
        public void CalculateVisiblePositions_ForAVisualRangeOf2AndObstacleToTheSouth_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(6, 6, 2);

            Assert.AreEqual(12, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 6)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
            Assert.IsFalse(visiblePositions.Contains(new Position(6, 4)));
        }

        // Obstacle to the SW
        [TestMethod]
        public void VisionCalculator_ForAVisualRangeOf2AndObstacleToTheSouthWest_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(7, 6, 2);

            Assert.AreEqual(13, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 6)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
        }

        // Obstacle to the W
        [TestMethod]
        public void VisionCalculator_ForAVisualRangeOf2AndObstacleToTheWest_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(7, 5, 2);

            Assert.AreEqual(12, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 5)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
            Assert.IsFalse(visiblePositions.Contains(new Position(5, 5)));
        }

        // Obstacle to the NW
        [TestMethod]
        public void VisionCalculator_ForAVisualRangeOf2AndObstacleToTheNorthWest_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _visionCalculator.CalculateVisiblePositions(7, 4, 2);

            Assert.AreEqual(13, visiblePositions.Count<Position>());
            Assert.IsTrue(visiblePositions.Contains(new Position(7, 4)));
            Assert.IsTrue(visiblePositions.Contains(new Position(6, 5)));
        }

        ////--------------------------------
        //// LoS (Line of Sight) Calculation Examples
        ////--------------------------------

        //// The sample level:
        //// XXXXXXXXXXXXXXXX
        //// X....EEE.......X
        //// X..........X...X
        //// X.......E......X
        //// X.E.X..........X
        //// X.....E....E...X
        //// X........X.....X
        //// X..........XXXXX
        //// X..........X...X
        //// X..........X...X
        //// X......X.......X
        //// X.X........X...X
        //// X..........X...X
        //// X..........X...X
        //// X......P...X...X
        //// XXXXXXXXXXXXXXXX
        //// X - Obstacles, P - Player, E - Enemies

        //// No Obstacles in between starting and ending position
        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf1AndAnEndingPositionInTheVisualRange_ReturnsTrue()
        //{
        //    Position startingPosition = new Position(2, 2);

        //    Node startingNode = new Node(_level, 2, 2);

        //    // Only orthogonal nodes are in a visual range of 1 from the starting position
        //    foreach (Node node in startingNode.GetAdjacentNodes(false))
        //    {
        //        Assert.IsTrue(_visionCalculator.IsInLineOfSight(startingPosition, node.Position, 1));
        //    }
        //}

        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf1AndAnEndingPositionNotInTheVisualRange_ReturnsFalse()
        //{
        //    Position startingPosition = new Position(2, 2);

        //    Node startingNode = new Node(_level, 2, 2);

        //    // All diagonal nodes are not in a visual range of 1 from the starting position
        //    foreach (Node node in startingNode.GetAdjacentNodes(true))
        //    {
        //        if (!node.IsOrthogonalTo(new Node(_level, startingPosition.X, startingPosition.Y)))
        //        {
        //            Assert.IsFalse(_visionCalculator.IsInLineOfSight(startingPosition, node.Position, 1));
        //        }
        //    }
        //}

        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf2AndAnEndingPositionInTheVisualRange_ReturnsTrue()
        //{
        //    Position startingPosition = new Position(2, 2);

        //    Node startingNode = new Node(_level, 2, 2);

        //    // All adjacent nodes including diagonal nodes are in a visual range of 2 from the starting position
        //    foreach (Node node in startingNode.GetAdjacentNodes(false))
        //    {
        //        Assert.IsTrue(_visionCalculator.IsInLineOfSight(startingPosition, node.Position, 2));
        //    }
        //}

        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf3AndAnEndingPositionInTheVisualRange_ReturnsTrue()
        //{
        //    Position startingPosition = new Position(1, 1);

        //    Position endingPosition = new Position(3, 3);

        //    Assert.IsTrue(_visionCalculator.IsInLineOfSight(startingPosition, endingPosition, 3));
        //}

        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf3AndAnEndingPositionOutsideTheVisualRange_ReturnsFalse()
        //{
        //    Position startingPosition = new Position(1, 1);

        //    Position endingPosition = new Position(4, 4);

        //    Assert.IsFalse(_visionCalculator.IsInLineOfSight(startingPosition, endingPosition, 3));
        //}

        //// Obstacles in between starting and ending position
        //[TestMethod]
        //public void VisionCalculator_ForAVisualRangeOf3WithAnObstacleBetweenStartingAndEndingPosition_ReturnsFalse()
        //{
        //    Position startingPosition = new Position(1, 4);

        //    Position endingPosition = new Position(3, 5);

        //    Assert.IsFalse(_visionCalculator.IsInLineOfSight(startingPosition, endingPosition, 3));
        //}
    }
}
