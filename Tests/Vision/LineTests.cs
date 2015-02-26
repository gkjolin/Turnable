using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using System.Collections.Generic;
using System.Linq;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Vision;

namespace Tests.Vision
{
    [TestClass]
    public class LineTests
    {
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void Constructor_GivenTwoAdjacentPositions_SuccessfullyCreatesALineWith2Points()
        {
            Position start = new Position(1, 1);

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Position end = start.NeighboringPosition(direction);

                Line line = new Line(start, end);

                Assert.AreEqual(2, line.Points.Count);
                Assert.AreEqual(start, line.Points[0]);
                Assert.AreEqual(end, line.Points[1]);
            }
        }

        [TestMethod]
        public void Constructor_GivenTwoDistantPoints_SuccessfullyCreatesALine()
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
