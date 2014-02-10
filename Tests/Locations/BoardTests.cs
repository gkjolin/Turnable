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
            _board = new Board();
            _board.Initialize(_world, "../../Fixtures/FullExample.tmx");
            _characterManagerMock = new Mock<ICharacterManager>();
        }

        [TestMethod]
        public void Board_Initialization_LoadsMapAndSetsParentReferences()
        {
            Board board = new Board();
            board.Initialize(_world, "../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(board.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.IsNotNull(board.CharacterManager);
            Assert.AreEqual(9, board.CharacterManager.Characters.Count);
            Assert.AreEqual(board, board.CharacterManager.Board);
            Assert.AreEqual(9, board.CharacterManager.TurnQueue.Count);
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

        [TestMethod]
        public void Board_HasAHelperFunction_ToFindTheBestPathToANodeAdjacentToThePlayer()
        {
            List<Node> bestPath = _board.FindBestPathToMoveAdjacentToPlayer(new Position(1, 1));

            Assert.AreEqual(19, bestPath.Count);
            Assert.AreEqual(new Node(1, 1), bestPath[0]);
            Assert.AreEqual(new Node(_board.CharacterManager.Player.GetComponent<Position>().X, _board.CharacterManager.Player.GetComponent<Position>().Y - 1), bestPath[18]);
        }

        [TestMethod]
        public void Board_WhenThereIsNoPathToThePlayer_ReturnsNullWhenAskedToFindTheBestPathToANodeAdjacentToThePlayer()
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
            // X.....NNN..X...X
            // X.....NPN..X...X
            // XXXXXXXXXXXXXXXX
            // X - Obstacles, P - Player, E - Enemies, N - New Obstacles
            _board.Map.Layers["Obstacles"].Tiles.Add(new System.Tuples.Tuple<int,int>(6, 14), new Tile(1, 6, 14));
            _board.Map.Layers["Obstacles"].Tiles.Add(new System.Tuples.Tuple<int,int>(8, 14), new Tile(1, 8, 14));
            _board.Map.Layers["Obstacles"].Tiles.Add(new System.Tuples.Tuple<int,int>(6, 13), new Tile(1, 6, 13));
            _board.Map.Layers["Obstacles"].Tiles.Add(new System.Tuples.Tuple<int,int>(7, 13), new Tile(1, 7, 13));
            _board.Map.Layers["Obstacles"].Tiles.Add(new System.Tuples.Tuple<int,int>(8, 13), new Tile(1, 8, 13));

            List<Node> bestPath = _board.FindBestPathToMoveAdjacentToPlayer(new Position(1, 1));

            Assert.IsNull(bestPath);
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
