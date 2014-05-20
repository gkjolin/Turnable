using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using TurnItUp.Locations;
using TurnItUp.Vision;
using TurnItUp.Pathfinding;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Linq;
using TurnItUp.Interfaces;

namespace Tests.Vision
{
    [TestClass]
    public class LineTests
    {
        private ILevel _level;
        private VisionCalculator _visionCalculator;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _visionCalculator = new VisionCalculator(_level);
        }

        [TestMethod]
        public void Line_WhenConstructedBetweenTwoAdjacentPoints_IsSuccessful()
        {
            Position start = new Position(1, 1);

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Position end = start.InDirection(direction);

                Line line = new Line(start, end);

                Assert.AreEqual(2, line.Points.Count);
                Assert.AreEqual(start, line.Points[0]);
                Assert.AreEqual(end, line.Points[1]);
            }
        }

        [TestMethod]
        public void Line_WhenConstructedBetweenTwoDistantPoints_IsSuccessful()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);

            Line line = new Line(start, end);

            Assert.AreEqual(6, line.Points.Count);
            Assert.AreEqual(start, line.Points[0]);
            Assert.AreEqual(new Position(1, 2), line.Points[1]);
            Assert.AreEqual(new Position(1, 3), line.Points[2]);
            Assert.AreEqual(new Position(1, 4), line.Points[3]);
            Assert.AreEqual(new Position(1, 5), line.Points[4]);
            Assert.AreEqual(end, line.Points[5]);
        }
    }
}
