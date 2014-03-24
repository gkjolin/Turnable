using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using TurnItUp.Locations;
using TurnItUp.Fov;
using TurnItUp.Pathfinding;
using System.Collections.Generic;
using TurnItUp.Components;

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
        public void FovCalculator_ForAVisualRangeOf0_ReturnsOnlyTheOriginPositionAsAVisiblePosition()
        {
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(0, 7, 14);

            Assert.AreEqual(1, visiblePositions.Count);
            Assert.AreEqual(new Position(7, 14), visiblePositions[0]);
        }

        [TestMethod]
        public void FovCalculator_ForAVisualRangeOf1AndNoObstacles_ReturnsAllPositionsAdjacentToTheOriginPosition()
        {
            List<Position> visiblePositions = _fovCalculator.CalculateVisiblePositions(1, 7, 12);

            Assert.AreEqual(9, visiblePositions.Count);
        }
    }
}
