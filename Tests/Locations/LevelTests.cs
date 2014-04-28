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
        private Mock<ICharacterManager> _mockCharacterManager;
        private IWorld _world;
        //private bool _eventTriggeredFlag;
        //private EventArgs _eventArgs;

        [TestInitialize]
        public void Initialize()
        {
            //_eventTriggeredFlag = false;
            _world = new World();
            _level = LocationsFactory.BuildLevel();
            _mockCharacterManager = new Mock<ICharacterManager>();
        }

        [TestMethod]
        public void Level_Construction_IsSuccessful()
        {
        }

        [TestMethod]
        public void Level_ConstructionWithAWorld_IsSuccessful()
        {
            Level level = new Level(_world);

            Assert.AreEqual(_world, level.World);
        }

        // Setup methods
        [TestMethod]
        public void Level_SettingUpAPathfinder_IsSuccessful()
        {
            _level.SetUpPathfinder();

            Assert.IsNotNull(_level.PathFinder);
            Assert.IsFalse(_level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(_level, _level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_SettingUpAPathfinderThatAllowsDiagonalMovement_IsSuccessful()
        {
            _level.SetUpPathfinder(true);

            Assert.IsNotNull(_level.PathFinder);
            Assert.IsTrue(_level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(_level, _level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_SettingUpTransitionPoints_IsSuccessful()
        {
            _level.SetUpTransitionPoints();

            Assert.IsNotNull(_level.TransitionPointManager);
            Assert.IsNotNull(_level.TransitionPointManager.Entrance);
            Assert.AreEqual(1, _level.TransitionPointManager.Exits.Count);
            Assert.AreEqual(_level, _level.TransitionPointManager.Level);
        }

        [TestMethod]
        public void Level_SettingUpCharacters_IsSuccessful()
        {
            _level.SetUpCharacters();

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.IsNotNull(_level.CharacterManager);
            Assert.AreEqual(9, _level.CharacterManager.Characters.Count);
            Assert.AreEqual(_level, _level.CharacterManager.Level);
            Assert.AreEqual(9, _level.CharacterManager.TurnQueue.Count);
        }

        [TestMethod]
        public void Level_SettingUpAViewport_IsSuccessful()
        {
            _level.SetUpViewport(8, 8, 5, 5);

            Assert.IsNotNull(_level.Viewport);
            Assert.AreEqual(_level, _level.Viewport.Level);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.Y);
            Assert.AreEqual(5, _level.Viewport.Width);
            Assert.AreEqual(5, _level.Viewport.Height);
        }

        [TestMethod]
        public void Level_SettingUpAMap_IsSuccessful()
        {
            _level.SetUpMap("../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(_level.Map);
        }

        [TestMethod]
        public void Level_SettingUpFov_IsSuccessful()
        {
            _level.SetUpFov();

            Assert.IsNotNull(_level.FovCalculator);
            Assert.AreEqual(_level, _level.FovCalculator.Level);
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
            _level.CharacterManager = _mockCharacterManager.Object;

            _level.IsCharacterAt(0, 0);
            _mockCharacterManager.Verify(cm => cm.IsCharacterAt(0, 0));
        }

        [TestMethod]
        public void Level_MovingAPlayer_DelegatesToCharacterManager()
        {
            Entity player = _world.CreateEntity();
            player.AddComponent(new Position(0, 0));
            _mockCharacterManager.SetupGet<Entity>(cm => cm.Player).Returns(player);

            _level.CharacterManager = _mockCharacterManager.Object;

            _level.MovePlayer(Direction.South);
            _mockCharacterManager.Verify(cm => cm.MovePlayer(Direction.South));
        }

        [TestMethod]
        public void Level_MovingACharacterToAPosition_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _mockCharacterManager.Object;

            _level.MoveCharacterTo(null, new Position(0, 0));
            _mockCharacterManager.Verify(cm => cm.MoveCharacterTo(null, new Position(0, 0)));
        }
    }
}
