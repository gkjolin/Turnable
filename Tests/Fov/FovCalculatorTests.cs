using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using TurnItUp.Locations;
using TurnItUp.Fov;
using TurnItUp.Pathfinding;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Linq;

namespace Tests.Fov
{
    [TestClass]
    public class FovCalculatorTests
    {
        private Level _level;
        private FovCalculator _fovCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _fovCalculator = new FovCalculator(_level);
        }

        [TestMethod]
        public void FovCalculator_Construction_IsSuccessful()
        {
            FovCalculator fovCalculator = new FovCalculator(_level);

            Assert.AreEqual(fovCalculator.Level, _level);
        }

        [TestMethod]
        public void FovCalculator_WhenCalculatingASlope_IsCorrect()
        {
            double slope = _fovCalculator.CalculateSlope(0, 0, 1, 1);
            Assert.AreEqual(1.0, slope);

            slope = _fovCalculator.CalculateSlope(4, 2, 3, 4);
            Assert.AreEqual(-0.5, slope);
        }

        [TestMethod]
        public void FovCalculator_WhenCalculatingInverseSlope_IsCorrect()
        {
            double slope = _fovCalculator.CalculateSlope(0, 0, 1, 1, true);
            Assert.AreEqual(1.0, slope);

            slope = _fovCalculator.CalculateSlope(4, 2, 3, 4, true);
            Assert.AreEqual(-2.0, slope);
        }

        [TestMethod]
        public void FovCalculator_WhenCalculatingTheVisibleDistance_CorrectlyCalculatesTheSquaredDistance()
        {
            int visibleDistance = _fovCalculator.CalculateVisibleDistance(0, 0, 1, 1);
            Assert.AreEqual(2, visibleDistance);

            visibleDistance = _fovCalculator.CalculateVisibleDistance(4, 2, 3, 4);
            Assert.AreEqual(5, visibleDistance);
        }

        //--------------------------------
        // FOV Calculation Examples
        //--------------------------------

        // The sample level:
        // XXXXXXXXXXXXXXXX
        // X....EEE.......X
        // X..........X...X
        // X.......E......X
        // X.E.X..........X
        // X.....E....E...X
        // X........X.....X
        // X..........XXXXX
        // X..........X...X
        // X..........X...X
        // X......X.......X
        // X.X........X...X
        // X..........X...X
        // X..........X...X
        // X......P...X...X
        // XXXXXXXXXXXXXXXX
        // X - Obstacles, P - Player, E - Enemies

        [TestMethod]
        public void FovCalculator_ForAVisualRangeOf0_ReturnsOnlyTheStartingPositionAsAVisiblePosition()
        {
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(7, 14, 0);

            Assert.AreEqual(1, visiblePositions.Count);
            Assert.AreEqual(new Position(7, 14), visiblePositions[0]);
        }

        [TestMethod]
        public void FovCalculator_ForAVisualRangeOf1AndNoObstacles_ReturnsAllPositionsAdjacentToTheStartingPosition()
        {
            // The FOV algorithm creates a cross for a VisualRange of 1
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(7, 3, 1);

            IEnumerable<Position> distinctVisiblePositions = visiblePositions.Distinct<Position>();

            Assert.AreEqual(5, distinctVisiblePositions.Count<Position>());
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 3)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 2)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 4)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(6, 3)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(8, 3)));
        }

        [TestMethod]
        public void FovCalculator_ForAVisualRangeOf1_IncludesObstaclesInTheVisiblePositions()
        {
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(7, 4, 1);

            IEnumerable<Position> distinctVisiblePositions = visiblePositions.Distinct<Position>();

            Assert.AreEqual(5, distinctVisiblePositions.Count<Position>());
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 4)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 5)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(7, 3)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(6, 4)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(8, 4)));
        }

        [TestMethod]
        public void FovCalculator_ForAVisualRangeOf1_IncludesCharactersInTheVisiblePositions()
        {
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(2, 10, 1);

            IEnumerable<Position> distinctVisiblePositions = visiblePositions.Distinct<Position>();

            Assert.AreEqual(5, distinctVisiblePositions.Count<Position>());
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(2, 10)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(2, 11)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(2, 9)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(3, 10)));
            Assert.IsTrue(distinctVisiblePositions.Contains(new Position(1, 10)));
        }

        // Testing each octant in the FOV with a VisualRange = 2
        // Obstacle to the N
        [TestMethod]
        public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheNorth_CorrectlyCalculatesTheVisiblePositions()
        {
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(7, 4, 2);

            Assert.AreEqual(25, visiblePositions.Count);

            // There should be only one invisible position
            Assert.IsFalse(visiblePositions.Contains(new Position(7, 6)));
        }

        //// Obstacle to the NE
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheNorthEast_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(6, 11, 2);

        //    Assert.AreEqual(23, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(8, 9)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(7, 9)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(8, 10)));
        //}

        //// Obstacle to the E
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheEast_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(6, 10, 2);

        //    Assert.AreEqual(25, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(8, 10)));
        //}

        //// Obstacle to the SE
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheSouthEast_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(6, 9, 2);

        //    Assert.AreEqual(23, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(8, 10)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(8, 11)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(7, 10)));
        //}

        //// Obstacle to the S
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheSouth_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(7, 9, 2);

        //    Assert.AreEqual(25, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(9, 9)));
        //}

        //// Obstacle to the SW
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheSouthWest_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(8, 9, 2);

        //    Assert.AreEqual(23, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(7, 11)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(6, 11)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(6, 10)));
        //}

        //// Obstacle to the W
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheWest_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(8, 10, 2);

        //    Assert.AreEqual(25, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(6, 10)));
        //}

        //// Obstacle to the NW
        //[TestMethod]
        //public void FovCalculator_ForAVisualRangeOf2AndObstacleToTheNorthWest_CorrectlyCalculatesTheVisiblePositions()
        //{
        //    List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(8, 11, 2);

        //    Assert.AreEqual(23, visiblePositions.Count);

        //    Assert.IsFalse(visiblePositions.Contains(new Position(6, 10)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(6, 9)));
        //    Assert.IsFalse(visiblePositions.Contains(new Position(7, 9)));
        //}
    }
}
