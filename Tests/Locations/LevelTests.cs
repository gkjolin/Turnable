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

namespace Tests.Locations
{
    [TestClass]
    public class LevelTests
    {
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

        private Level _level;
        private Mock<ICharacterManager> _characterManagerMock;
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            _level = new Level(_world, "../../Fixtures/FullExample.tmx");
            _characterManagerMock = new Mock<ICharacterManager>();
        }

        [TestMethod]
        public void Level_Construction_IsSuccessful()
        {
            Level level = new Level(_world, "../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(9, level.CharacterManager.Characters.Count);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.AreEqual(9, level.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsFalse(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_Initialization_IsSuccessful()
        {
            Level level = new Level();
            level.Initialize(_world, "../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(9, level.CharacterManager.Characters.Count);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.AreEqual(9, level.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsFalse(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_InitializationAndRandomizationWhileInitializing_IsSuccessful()
        {
            Level level = new Level();
            level.Initialize(_world, "../../Fixtures/FullExample.tmx", false, true);

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsFalse(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);

            // Check to see if the characters in the level have been randomized. Right now randomization adds anywhere from 1 to 10 random characters to the level.
            Assert.IsTrue(level.CharacterManager.Characters.Count > 9);
            Assert.IsTrue(level.CharacterManager.TurnQueue.Count > 9);
            Assert.IsTrue(level.CharacterManager.Characters.Count <= 19);
            Assert.IsTrue(level.CharacterManager.TurnQueue.Count <= 19);
        }

        [TestMethod]
        public void Level_ConstructionWithAPathFinderThatAllowsDiagonalMovement_IsSuccessful()
        {
            Level level = new Level(_world, "../../Fixtures/FullExample.tmx", true);

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(9, level.CharacterManager.Characters.Count);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.AreEqual(9, level.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsTrue(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_SettingUpAViewport_IsSuccessful()
        {
            _level.SetupViewport(8, 8, 5, 5);

            Assert.IsNotNull(_level.Viewport);
            Assert.AreEqual(_level, _level.Viewport.Level);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.Y);
            Assert.AreEqual(5, _level.Viewport.Width);
            Assert.AreEqual(5, _level.Viewport.Height);
        }

        [TestMethod]
        public void Level_DeterminingObstacles_TakesIntoAccountLayerHavingTrueForIsCollisionProperty()
        {
            // The example level has a "wall" around the entire 15x15 level
            Assert.IsTrue(_level.IsObstacle(0, 0));
            Assert.IsTrue(_level.IsObstacle(0, 1));
            Assert.IsTrue(_level.IsObstacle(1, 0));
            Assert.IsTrue(_level.IsObstacle(2, 0));

            Assert.IsFalse(_level.IsObstacle(1, 1));
        }

        [TestMethod]
        public void Level_CalculatingAllWalkablePositions_ReturnsAListOfAllWalkablePositionsInTheLevel()
        {
            List<Position> allWalkablePositions = _level.CalculateWalkablePositions();

            Assert.AreEqual(172, allWalkablePositions.Count);

            foreach (Position position in allWalkablePositions)
            {
                Assert.IsTrue(new Node(_level, position.X, position.Y).IsWalkable());
            }
        }

        // Facade implementation tests
        [TestMethod]
        public void Level_DeterminingIfCharacterIsAtAPosition_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _characterManagerMock.Object;

            _level.IsCharacterAt(0, 0);
            _characterManagerMock.Verify(cm => cm.IsCharacterAt(0, 0));
        }

        [TestMethod]
        public void Level_MovingAPlayer_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _characterManagerMock.Object;

            _level.MovePlayer(Direction.South);
            _characterManagerMock.Verify(cm => cm.MovePlayer(Direction.South));
        }

        [TestMethod]
        public void Level_MovingACharacterToAPosition_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _characterManagerMock.Object;

            _level.MoveCharacterTo(null, new Position(0, 0));
            _characterManagerMock.Verify(cm => cm.MoveCharacterTo(null, new Position(0, 0)));
        }
    }
}
