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
        private Viewport _viewport;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _level.SetupViewport(8, 8, 5, 5);
            _viewport = new Viewport(_level, 8, 8, 5, 5);
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

        // Testing the calculation of anchor points (the Global (Map) position at which the viewport "locks" onto the player)
        // Once locked, movements by the player cause the level to scroll
        // Since a viewport can have an even width or even height (even widths and heights don't have an exact middle),
        // the anchor point can be 1, 2 or 4 positions
        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthAndHeightAreOdd_CalculatesTheExactCenterPositionOfTheViewport()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 15, 15);

            Assert.AreEqual(1, viewport.AnchorPoints.Count);
            Assert.AreEqual(new Position(54 + 7, 53 + 7), viewport.AnchorPoints[0]);
        }

        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthIsEvenAndItsHeightIsOdd_CalculatesTwoCentralPositions()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 16, 15);

            Assert.AreEqual(2, viewport.AnchorPoints.Count);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 7, 53 + 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 8, 53 + 7)));
        }

        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthIsOddAndItsHeightIsEven_CalculatesTwoCentralPositions()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 15, 16);

            Assert.AreEqual(2, viewport.AnchorPoints.Count);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 7, 53 + 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 7, 53 + 8)));
        }

        [TestMethod]
        public void Viewport_CalculatingTheAnchorPointWhenItsWidthAndHeightAreBothEven_CalculatesFourCentralPositions()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 16, 16);

            Assert.AreEqual(4, viewport.AnchorPoints.Count);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 7, 53 + 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 7, 53 + 8)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 8, 53 + 7)));
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(54 + 8, 53 + 8)));
        }

        [TestMethod]
        public void Viewport_WhenMoved_RecalculatesTheAnchorPoints()
        {
            Viewport viewport = new Viewport(_level, 8, 8, 5, 5);

            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(8 + 2, 8 + 2)));

            viewport.Move(Direction.North);
            Assert.IsTrue(viewport.AnchorPoints.Contains(new Position(8 + 2, 9 + 2)));
        }

        // Testing to see if a viewport's MapOrigin is valid
        [TestMethod]
        public void Viewport_MapOriginOutOfBounds_IsInvalid()
        {
            // Viewport's bottom left corner out of bounds
            _viewport.MapOrigin.X = -1;
            _viewport.MapOrigin.Y = 0;
            Assert.IsFalse(_viewport.IsMapOriginValid());
            _viewport.MapOrigin.X = 0;
            _viewport.MapOrigin.Y = -1;
            Assert.IsFalse(_viewport.IsMapOriginValid());
            _viewport.MapOrigin.X = -1;
            _viewport.MapOrigin.Y = -1;
            Assert.IsFalse(_viewport.IsMapOriginValid());

            // Viewport out of bounds on its bottom right corner
            _viewport.MapOrigin.X = 11;
            _viewport.MapOrigin.Y = 0;
            Assert.IsFalse(_viewport.IsMapOriginValid());
            _viewport.MapOrigin.X = 10;
            _viewport.MapOrigin.Y = -1;
            Assert.IsFalse(_viewport.IsMapOriginValid());

            // Viewport out of bounds on its top right side
            _viewport.MapOrigin.X = 10;
            _viewport.MapOrigin.Y = 11;
            Assert.IsFalse(_viewport.IsMapOriginValid());
            _viewport.MapOrigin.X = 11;
            _viewport.MapOrigin.Y = 10;
            Assert.IsFalse(_viewport.IsMapOriginValid());

            // Viewport out of bounds on its top left side
            _viewport.MapOrigin.X = -1;
            _viewport.MapOrigin.Y = 11;
            Assert.IsFalse(_viewport.IsMapOriginValid());
            _viewport.MapOrigin.X = 0;
            _viewport.MapOrigin.Y = 11;
            Assert.IsFalse(_viewport.IsMapOriginValid());
        }

        // Moving the viewport (Changing the MapOrigin)
        [TestMethod]
        public void Viewport_MovingViewportInADirection_MovesTheMapOriginInThatDirection()
        {
            _viewport.Move(Direction.North);
            Assert.AreEqual(new Position(8, 9), _viewport.MapOrigin);
            _viewport.Move(Direction.South);
            Assert.AreEqual(new Position(8, 8), _viewport.MapOrigin);
            _viewport.Move(Direction.East);
            Assert.AreEqual(new Position(9, 8), _viewport.MapOrigin);
            _viewport.Move(Direction.West);
            Assert.AreEqual(new Position(8, 8), _viewport.MapOrigin);
            _viewport.Move(Direction.NorthWest);
            Assert.AreEqual(new Position(7, 9), _viewport.MapOrigin);
            _viewport.Move(Direction.NorthEast);
            Assert.AreEqual(new Position(8, 10), _viewport.MapOrigin);
            _viewport.Move(Direction.SouthWest);
            Assert.AreEqual(new Position(7, 9), _viewport.MapOrigin);
            _viewport.Move(Direction.SouthEast);
            Assert.AreEqual(new Position(8, 8), _viewport.MapOrigin);
        }

        [TestMethod]
        public void Viewport_MovingTheMapOriginOutOfBounds_FailsQuietly()
        {
            _viewport.MapOrigin.X = 0;
            _viewport.MapOrigin.Y = 0;

            // Viewport at lower left of the Map
            _viewport.Move(Direction.West);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.NorthWest);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.SouthWest);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.South);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);

            // Viewport at bottom right of the Map
            _viewport.MapOrigin.X = 10;
            _viewport.MapOrigin.Y = 0;
            _viewport.Move(Direction.East);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.NorthEast);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.SouthEast);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.South);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(0, _viewport.MapOrigin.Y);

            // Viewport at top right of the Map
            _viewport.MapOrigin.X = 10;
            _viewport.MapOrigin.Y = 10;
            _viewport.Move(Direction.East);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.NorthEast);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.SouthEast);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.North);
            Assert.AreEqual(10, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);

            // Viewport at top left of the Map
            _viewport.MapOrigin.X = 0;
            _viewport.MapOrigin.Y = 10;
            _viewport.Move(Direction.West);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.NorthWest);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.SouthWest);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
            _viewport.Move(Direction.North);
            Assert.AreEqual(0, _viewport.MapOrigin.X);
            Assert.AreEqual(10, _viewport.MapOrigin.Y);
        }

        // Testing the automatic movement of MapOrigin when the player moves
        // Enough space on all sides to allow movement of MapOrigin
        [TestMethod]
        public void Viewport_MovingPlayerLocateadAtTheOnlyViewportAnchorPoint_MovesTheMapOriginOfViewportInThatSameDirection()
        {
            _level.SetupViewport(8, 8, 5, 5);
            _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(10, 10));

            _level.MovePlayer(Direction.North);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(9, _level.Viewport.MapOrigin.Y);
        }

        [TestMethod]
        public void Viewport_MovingPlayerLocateadAtAnyOfTheViewportsAnchorPoints_MovesTheMapOriginOfViewportInThatSameDirection()
        {
            _level.SetupViewport(8, 8, 6, 6);
            _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(10, 11));

            _level.MovePlayer(Direction.North);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(9, _level.Viewport.MapOrigin.Y);
        }

        [TestMethod]
        public void Viewport_MovingPlayerNotLocateadAtAnyOfTheViewportsAnchorPoints_DoesNotMoveTheMapOriginOfViewport()
        {
            _level.SetupViewport(8, 8, 6, 6);
            _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(3, 3));

            _level.MovePlayer(Direction.North);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.Y);
        }
    }
}
