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

namespace Tests.Locations
{
    [TestClass]
    public class BoardTests
    {
        // The sample board:
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

        private Board _board;
        private Mock<ICharacterManager> _characterManagerMock;
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            _board = new Board(_world, "../../Fixtures/FullExample.tmx");
            _characterManagerMock = new Mock<ICharacterManager>();
        }

        [TestMethod]
        public void Board_Construction_IsSuccessful()
        {
            Board board = new Board(_world, "../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(board.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _board.World);
            Assert.IsNotNull(board.CharacterManager);
            Assert.AreEqual(9, board.CharacterManager.Characters.Count);
            Assert.AreEqual(board, board.CharacterManager.Board);
            Assert.AreEqual(9, board.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(board.PathFinder);
            Assert.IsFalse(board.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(board, board.PathFinder.Board);
        }

        [TestMethod]
        public void Board_ConstructionWithAPathFinderThatAllowsDiagonalMovement_IsSuccessful()
        {
            Board board = new Board(_world, "../../Fixtures/FullExample.tmx", true);

            Assert.IsNotNull(board.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _board.World);
            Assert.IsNotNull(board.CharacterManager);
            Assert.AreEqual(9, board.CharacterManager.Characters.Count);
            Assert.AreEqual(board, board.CharacterManager.Board);
            Assert.AreEqual(9, board.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(board.PathFinder);
            Assert.IsTrue(board.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(board, board.PathFinder.Board);
        }

        [TestMethod]
        public void Board_DeterminingObstacles_TakesIntoAccountLayerHavingTrueForIsCollisionProperty()
        {
            // The example board has a "wall" around the entire 15x15 level
            Assert.IsTrue(_board.IsObstacle(0, 0));
            Assert.IsTrue(_board.IsObstacle(0, 1));
            Assert.IsTrue(_board.IsObstacle(1, 0));
            Assert.IsTrue(_board.IsObstacle(2, 0));

            Assert.IsFalse(_board.IsObstacle(1, 1));
        }

        // Facade implementation tests
        [TestMethod]
        public void Board_DeterminingIfCharacterIsAtAPosition_DelegatesToCharacterManager()
        {
            _board.CharacterManager = _characterManagerMock.Object;

            _board.IsCharacterAt(0, 0);
            _characterManagerMock.Verify(cm => cm.IsCharacterAt(0, 0));
        }

        [TestMethod]
        public void Board_MovingAPlayer_DelegatesToCharacterManager()
        {
            _board.CharacterManager = _characterManagerMock.Object;

            _board.MovePlayer(Direction.Down);
            _characterManagerMock.Verify(cm => cm.MovePlayer(Direction.Down));
        }

        [TestMethod]
        public void Board_MovingACharacterToAPosition_DelegatesToCharacterManager()
        {
            _board.CharacterManager = _characterManagerMock.Object;

            _board.MoveCharacterTo(null, new Position(0, 0));
            _characterManagerMock.Verify(cm => cm.MoveCharacterTo(null, new Position(0, 0)));
        }
    }
}
