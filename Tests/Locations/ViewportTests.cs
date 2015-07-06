using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using Turnable.Api;
using Turnable.Locations;

namespace Tests.Locations
{
    [TestClass]
    public class ViewportTests
    {
        private ILevel _level;
        // private IViewport _viewport;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            //_level.SetUpViewport(8, 8, 5, 5);
            //_viewport = new Viewport(_level, 8, 8, 5, 5);
        }

        [TestMethod]
        public void Constructor_GivenALevel_CreatesAViewportWithMapOriginAtZerpAndSameSizeAsTheLevel()
        {
            Viewport viewport = new Viewport(_level);

            Assert.AreEqual(_level, viewport.Level);
            Assert.AreEqual(0, viewport.MapOrigin.X);
            Assert.AreEqual(0, viewport.MapOrigin.Y);
            Assert.AreEqual(_level.Map.Width, viewport.Width);
            Assert.AreEqual(_level.Map.Height, viewport.Height);
        }
        [TestMethod]
        public void Constructor_GivenALevelAndSize_InitializesAllProperties()
        {
            Viewport viewport = new Viewport(_level, 16, 16);

            Assert.AreEqual(_level, viewport.Level);
            Assert.AreEqual(16, viewport.Width);
            Assert.AreEqual(16, viewport.Height);
        }

        [TestMethod]
        public void Constructor_GivenALevelMapOriginAndSize_InitializesAllProperties()
        {
            Viewport viewport = new Viewport(_level, 54, 53, 16, 16);

            Assert.AreEqual(_level, viewport.Level);
            Assert.AreEqual(54, viewport.MapOrigin.X);
            Assert.AreEqual(53, viewport.MapOrigin.Y);
            Assert.AreEqual(16, viewport.Width);
            Assert.AreEqual(16, viewport.Height);
        }

        // Testing to see if a viewport's MapOrigin is valid
        //[TestMethod]
        //public void Viewport_MapOriginOutOfBounds_IsInvalid()
        //{
        //    // Viewport's bottom left corner out of bounds
        //    _viewport.MapOrigin.X = -1;
        //    _viewport.MapOrigin.Y = 0;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());
        //    _viewport.MapOrigin.X = 0;
        //    _viewport.MapOrigin.Y = -1;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());
        //    _viewport.MapOrigin.X = -1;
        //    _viewport.MapOrigin.Y = -1;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());

        //    // Viewport out of bounds on its bottom right corner
        //    _viewport.MapOrigin.X = 11;
        //    _viewport.MapOrigin.Y = 0;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());
        //    _viewport.MapOrigin.X = 10;
        //    _viewport.MapOrigin.Y = -1;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());

        //    // Viewport out of bounds on its top right side
        //    _viewport.MapOrigin.X = 10;
        //    _viewport.MapOrigin.Y = 11;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());
        //    _viewport.MapOrigin.X = 11;
        //    _viewport.MapOrigin.Y = 10;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());

        //    // Viewport out of bounds on its top left side
        //    _viewport.MapOrigin.X = -1;
        //    _viewport.MapOrigin.Y = 11;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());
        //    _viewport.MapOrigin.X = 0;
        //    _viewport.MapOrigin.Y = 11;
        //    Assert.IsFalse(_viewport.IsMapOriginValid());
        //}

        //// Moving the viewport (Changing the MapOrigin)
        //[TestMethod]
        //public void Viewport_MovingViewportInADirection_MovesTheMapOriginInThatDirection()
        //{
        //    _viewport.Move(Direction.North);
        //    Assert.AreEqual(new Position(8, 9), _viewport.MapOrigin);
        //    _viewport.Move(Direction.South);
        //    Assert.AreEqual(new Position(8, 8), _viewport.MapOrigin);
        //    _viewport.Move(Direction.East);
        //    Assert.AreEqual(new Position(9, 8), _viewport.MapOrigin);
        //    _viewport.Move(Direction.West);
        //    Assert.AreEqual(new Position(8, 8), _viewport.MapOrigin);
        //    _viewport.Move(Direction.NorthWest);
        //    Assert.AreEqual(new Position(7, 9), _viewport.MapOrigin);
        //    _viewport.Move(Direction.NorthEast);
        //    Assert.AreEqual(new Position(8, 10), _viewport.MapOrigin);
        //    _viewport.Move(Direction.SouthWest);
        //    Assert.AreEqual(new Position(7, 9), _viewport.MapOrigin);
        //    _viewport.Move(Direction.SouthEast);
        //    Assert.AreEqual(new Position(8, 8), _viewport.MapOrigin);
        //}

        //[TestMethod]
        //public void Viewport_MovingTheMapOriginOutOfBounds_MovesAsMuchAsPossible()
        //{
        //    _viewport.MapOrigin.X = 0;
        //    _viewport.MapOrigin.Y = 0;

        //    // Viewport at lower left of the Map
        //    _viewport.Move(Direction.West);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.NorthWest);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.SouthWest);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.South);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _viewport.MapOrigin.Y);

        //    // Viewport at bottom right of the Map
        //    _viewport.MapOrigin.X = 10;
        //    _viewport.MapOrigin.Y = 0;
        //    _viewport.Move(Direction.East);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.NorthEast);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.SouthEast);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.South);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _viewport.MapOrigin.Y);

        //    // Viewport at top right of the Map
        //    _viewport.MapOrigin.X = 10;
        //    _viewport.MapOrigin.Y = 10;
        //    _viewport.Move(Direction.East);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(10, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.NorthEast);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(10, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.SouthEast);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(9, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.North);
        //    Assert.AreEqual(10, _viewport.MapOrigin.X);
        //    Assert.AreEqual(10, _viewport.MapOrigin.Y);

        //    // Viewport at top left of the Map
        //    _viewport.MapOrigin.X = 0;
        //    _viewport.MapOrigin.Y = 10;
        //    _viewport.Move(Direction.West);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(10, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.NorthWest);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(10, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.SouthWest);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(9, _viewport.MapOrigin.Y);
        //    _viewport.Move(Direction.North);
        //    Assert.AreEqual(0, _viewport.MapOrigin.X);
        //    Assert.AreEqual(10, _viewport.MapOrigin.Y);
        //}

        //// Testing the automatic movement of MapOrigin when the player moves
        //// Enough space on all sides to allow movement of MapOrigin
        //// Viewport with odd height and width
        //[TestMethod]
        //public void Viewport_MovingPlayerLocateadAtTheExactCenterOfTheViewport_MovesTheMapOriginOfViewportInThatSameDirection()
        //{
        //    _level.SetUpViewport(5, 1, 5, 5);
        //    _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(7, 3));

        //    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        //    {
        //        _level.MovePlayer(direction);
        //        Assert.AreEqual(2, Math.Abs(_level.Viewport.MapOrigin.X - _level.CharacterManager.Player.GetComponent<Position>().X));
        //        Assert.AreEqual(2, Math.Abs(_level.Viewport.MapOrigin.Y - _level.CharacterManager.Player.GetComponent<Position>().Y));

        //        // Reset viewport and player's position
        //        _level.Viewport.MapOrigin.X = 5;
        //        _level.Viewport.MapOrigin.Y = 1;
        //        _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(7, 3));
        //    }
        //}

        //[TestMethod]
        //public void Viewport_MovingPlayerWhenLocatedAtTheCentralRowInAnyColumnOfTheViewport_MovesTheMapOriginCorrectly()
        //{
        //    _level.SetUpViewport(5, 1, 5, 5);
        //    _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(5, 3));

        //    // If player moves North or South, the viewport should shift North or South respectively
        //    _level.MovePlayer(Direction.North);
        //    Assert.AreEqual(5, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(2, _level.Viewport.MapOrigin.Y);
        //    _level.MovePlayer(Direction.South);
        //    Assert.AreEqual(5, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);

        //    // If player Moves NorthEast Or SouthEast, the viewport just shifts North or South respectively
        //    // It should not shift to the East as the player is still not located at the central column of the viewport
        //    _level.MovePlayer(Direction.NorthEast);
        //    Assert.AreEqual(5, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(2, _level.Viewport.MapOrigin.Y);
        //    _level.MovePlayer(Direction.SouthEast);
        //    Assert.AreEqual(5, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);
        //}

        //[TestMethod]
        //public void Viewport_MovingPlayerWhenLocatedAtTheCentralColumnInAnyRowOfTheViewport_MovesTheMapOriginCorrectly()
        //{
        //    _level.SetUpViewport(5, 1, 5, 5);
        //    _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(7, 1));

        //    // If player moves East or West, the viewport should shift East or West respectively
        //    _level.MovePlayer(Direction.East);
        //    Assert.AreEqual(6, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);
        //    _level.MovePlayer(Direction.West);
        //    Assert.AreEqual(5, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);

        //    // If player Moves NorthEast Or NorthWest, the viewport just shifts East or West respectively
        //    // It should not shift to the North as the player is still not located at the central row of the viewport
        //    _level.MovePlayer(Direction.NorthEast);
        //    Assert.AreEqual(6, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);
        //    _level.MovePlayer(Direction.NorthWest);
        //    Assert.AreEqual(5, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);
        //}

        //// Enough space on all sides to allow movement of MapOrigin
        //// Test Viewport with even height and width
        //[TestMethod]
        //public void Viewport_MovingPlayerWhenLocatedAtTheCenterOfAViewportWithEvenWidthAndHeight_MovesTheMapOriginCorrectly()
        //{
        //    _level.SetUpViewport(3, 4, 6, 6);
        //    _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(6, 7));

        //    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        //    {
        //        _level.MovePlayer(direction);
        //        Assert.AreEqual(3, Math.Abs(_level.Viewport.MapOrigin.X - _level.CharacterManager.Player.GetComponent<Position>().X));
        //        Assert.AreEqual(3, Math.Abs(_level.Viewport.MapOrigin.Y - _level.CharacterManager.Player.GetComponent<Position>().Y));

        //        // Reset viewport and player's position
        //        _level.Viewport.MapOrigin.X = 3;
        //        _level.Viewport.MapOrigin.Y = 4;
        //        _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(6, 7));
        //    }
        //}

        //// Test Viewport does not move when player hits obstacle or player hits character
        //[TestMethod]
        //public void Viewport_WhenAMovingPlayerHitsAnObstacleOrCharacter_DoesNotMoveItself()
        //{
        //    _level.SetUpViewport(4, 1, 6, 6);
        //    _level.MoveCharacterTo(_level.CharacterManager.Player, new Position(7, 4));

        //    _level.MovePlayer(Direction.North);

        //    Assert.AreEqual(4, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(1, _level.Viewport.MapOrigin.Y);
        //}

        //// Centering a viewport
        //[TestMethod]
        //public void Viewport_WithEvenSizeWhenCenteringOnAPositionWithPlentyOfSpaceAroundTheCenter_IsSuccessful()
        //{
        //    _level.SetUpViewport(6, 6);

        //    _level.Viewport.CenterOn(new Position(5, 5));

        //    Assert.AreEqual(2, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(2, _level.Viewport.MapOrigin.Y);
        //}

        //[TestMethod]
        //public void Viewport_WithOddSizeWhenCenteringOnAPositionWithPlentyOfSpaceAroundTheCenter_IsSuccessful()
        //{
        //    _level.SetUpViewport(5, 5);

        //    _level.Viewport.CenterOn(new Position(5, 5));

        //    Assert.AreEqual(3, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(3, _level.Viewport.MapOrigin.Y);
        //}

        //[TestMethod]
        //public void Viewport_WhenCenteringOnAPositionWithNotEnoughSpaceAroundCenter_IsSuccessful()
        //{
        //    _level.SetUpViewport(5, 5);

        //    // Bottom left
        //    _level.Viewport.CenterOn(new Position(0, 0));
        //    Assert.AreEqual(0, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _level.Viewport.MapOrigin.Y);

        //    // Bottom right
        //    _level.Viewport.CenterOn(new Position(_level.Map.Width - 1, 0));
        //    Assert.AreEqual(_level.Map.Width - _level.Viewport.Width, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(0, _level.Viewport.MapOrigin.Y);

        //    // Top right
        //    _level.Viewport.CenterOn(new Position(_level.Map.Width - 1, _level.Map.Height - 1));
        //    Assert.AreEqual(_level.Map.Width - _level.Viewport.Width, _level.Viewport.MapOrigin.X, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(_level.Map.Height - _level.Viewport.Height, _level.Viewport.MapOrigin.Y);

        //    // Top left
        //    _level.Viewport.CenterOn(new Position(0, _level.Map.Height - 1));
        //    Assert.AreEqual(0, _level.Viewport.MapOrigin.X);
        //    Assert.AreEqual(_level.Map.Height - _level.Viewport.Height, _level.Viewport.MapOrigin.Y);
        //}
    }
}
