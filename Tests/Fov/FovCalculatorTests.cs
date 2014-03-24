using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using TurnItUp.Locations;
using TurnItUp.Fov;
using TurnItUp.Pathfinding;
using System.Collections.Generic;

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
        public void FovCalculator_WhenCalculatingASlopeBetweenTwoNodes_IsCorrect()
        {
            Node node1 = new Node(_level, 0, 0);
            Node node2 = new Node(_level, 1, 1);
            double slope = _fovCalculator.CalculateSlope(node1, node2);
            Assert.AreEqual(1.0, slope);

            node1 = new Node(_level, 4, 2);
            node2 = new Node(_level, 3, 4);
            slope = _fovCalculator.CalculateSlope(node1, node2);
            Assert.AreEqual(-0.5, slope);
        }

        [TestMethod]
        public void FovCalculator_WhenCalculatingInverseSlope_IsCorrect()
        {
            Node node1 = new Node(_level, 0, 0);
            Node node2 = new Node(_level, 1, 1);
            double slope = _fovCalculator.CalculateSlope(node1, node2, true);
            Assert.AreEqual(1.0, slope);

            node1 = new Node(_level, 4, 2);
            node2 = new Node(_level, 3, 4);
            slope = _fovCalculator.CalculateSlope(node1, node2, true);
            Assert.AreEqual(-2.0, slope);
        }

        [TestMethod]
        public void FovCalculator_WhenCalculatingTheVisibleDistanceBetweenTwoNodes_CorrectlyUsesSquaredDistanceSpace()
        {
            Node node1 = new Node(_level, 0, 0);
            Node node2 = new Node(_level, 1, 1);
            int visibleDistance = _fovCalculator.CalculateVisibleDistance(node1, node2);
            Assert.AreEqual(2, visibleDistance);

            node1 = new Node(_level, 4, 2);
            node2 = new Node(_level, 3, 4);
            visibleDistance = _fovCalculator.CalculateVisibleDistance(node1, node2);
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
        public void FovCalculator_ForAVisualRangeOf0_ReturnsOnlyTheStartLocationAsAVisiblePoint()
        {
            List<Node> visibleNodes = _fovCalculator.CalculateVisibleNodes(0, 7, 14);
        }
    }
}
