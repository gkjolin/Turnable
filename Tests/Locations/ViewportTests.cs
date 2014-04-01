using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using Moq;
using TurnItUp.Components;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Tmx;
using System.Tuples;
using Tests.Factories;

namespace Tests.Locations
{
    [TestClass]
    public class ViewportTests
    {
        private Level _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }
           
        [TestMethod]
        public void Viewport_Construction_IsSuccessful()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 16, 16);

            Assert.AreEqual(_level, viewport.Level);
            Assert.AreEqual(54, viewport.MapOrigin.X);
            Assert.AreEqual(53, viewport.MapOrigin.Y);
            Assert.AreEqual(16, viewport.Width);
            Assert.AreEqual(16, viewport.Height);

            // Make sure that the anchor points for this viewport are calculated on anchor points
            Assert.IsNotNull(viewport.AnchorPoints);
            Assert.AreNotEqual(0, viewport.AnchorPoints.Count);
        }

        // Calculating anchor points (the LOCAL position at which the viewport "locks" onto the player)
        // Once locked, movements by the player cause the level to scroll
        // Since a viewport can have an even width or even height (even widths and heights don't have an exact middle),
        // the anchor point can be 1, 2 or 4 positions
        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthAndHeightAreOdd_CalculatesTheExactCenterPositionOfTheViewport()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 15, 15);

            Assert.AreEqual(1, viewport.AnchorPoints.Count);
            Assert.AreEqual(new Position(7, 7), viewport.AnchorPoints[0]);
        }

        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthIsEvenAndItsHeightIsOdd_CalculatesTwoCentralPositions()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 16, 15);

            Assert.AreEqual(2, viewport.AnchorPoints.Count);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(7, 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(8, 7)));
        }

        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthIsOddAndItsHeightIsEven_CalculatesTwoCentralPositions()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 15, 16);

            Assert.AreEqual(2, viewport.AnchorPoints.Count);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(7, 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(7, 8)));
        }

        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthAndHeightAreBothEven_CalculatesFourCentralPositions()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 16, 16);

            Assert.AreEqual(4, viewport.AnchorPoints.Count);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(7, 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(7, 8)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(8, 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(8, 8)));
        }
    }
}
