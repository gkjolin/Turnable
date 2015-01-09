using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entropy.Core;
using Turnable.Locations;
using Turnable.Tiled;
using Tests.Factories;
using Turnable.Pathfinding;
using Turnable.Api;
using Turnable.Components;
using System.Linq;
using Moq;

namespace Tests.Locations
{
    [TestClass]
    public class LevelTests
    {
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void DefaultConstructor_InitializesAllLevelComponentsToNull()
        {
            ILevel level = new Level();

            // TODO: Uncomment these out as we implement these systems
            Assert.IsNull(level.Map);
            Assert.IsNull(level.Pathfinder);
            Assert.IsNotNull(level.SpecialLayers);
            //Assert.IsNull(level.World);
            //Assert.IsNull(level.CharacterManager);
            //Assert.IsNull(level.VisionCalculator);
            //Assert.IsNull(level.Viewport);
        }

        // TODO: Write unit test to ensure IsCollisionReturnsFalseIfNoCollisionLayerExists
        [TestMethod]
        public void IsCollision_ReturnsTrueIfThereIsAnyTileInTheCollisionSpecialLayerForTheLevel()
        {
            // The example level has a "wall" around the entire 15x15 level
            Assert.IsTrue(_level.IsCollision(new Position(0, 0)));
            Assert.IsTrue(_level.IsCollision(new Position(0, 1)));
            Assert.IsTrue(_level.IsCollision(new Position(1, 0)));
            Assert.IsTrue(_level.IsCollision(new Position(2, 0)));

            Assert.IsFalse(_level.IsCollision(new Position(10, 1)));
        }

        // Special layer tests
        [TestMethod]
        public void Map_WhenSet_CallsInitializeSpecialLayers()
        {
            Mock<ILevel> mockLevel = new Mock<ILevel>();
            mockLevel.CallBase = true;
            mockLevel.Setup(l => l.InitializeSpecialLayers()).Verifiable();

            mockLevel.Object.Map = new Map("../../Fixtures/FullExample.tmx");

            mockLevel.Verify(l => l.InitializeSpecialLayers());
        }

        [TestMethod]
        public void SpecialLayerPropertyKey_ReturnsTheRightPropertyName()
        {
            foreach (Level.SpecialLayer specialLayer in Enum.GetValues(typeof(Level.SpecialLayer)).Cast<Level.SpecialLayer>())
            {
                Assert.AreEqual("Is" + specialLayer.ToString() + "Layer", Level.SpecialLayerPropertyKey(specialLayer));
            }
        }

        [TestMethod]
        public void InitializeSpecialLayers_SetsAllTheSpecialLayersPresentInTheMap()
        {
            _level.InitializeSpecialLayers();

            Assert.AreEqual(_level.Map.Layers[0], _level.SpecialLayers[Level.SpecialLayer.Background]);
            Assert.AreEqual(_level.Map.Layers[1], _level.SpecialLayers[Level.SpecialLayer.Collision]);
            Assert.AreEqual(_level.Map.Layers[2], _level.SpecialLayers[Level.SpecialLayer.Object]);
            Assert.AreEqual(_level.Map.Layers[3], _level.SpecialLayers[Level.SpecialLayer.Character]);
        }

        [TestMethod]
        public void Level_SpecialLayerEnum_Defines4DifferentSpecialLayers()
        {
            Assert.AreEqual(4, Enum.GetValues(typeof(Level.SpecialLayer)).Length);
            Assert.IsTrue(Enum.IsDefined(typeof(Level.SpecialLayer), "Background"));
            Assert.IsTrue(Enum.IsDefined(typeof(Level.SpecialLayer), "Collision"));
            Assert.IsTrue(Enum.IsDefined(typeof(Level.SpecialLayer), "Object"));
            Assert.IsTrue(Enum.IsDefined(typeof(Level.SpecialLayer), "Character"));
        }

        [TestMethod]
        public void SpecialLayers_AllowsSettingASpecialLayerUsingTheSpecialLayerEnumAsKey()
        {
            var values = Enum.GetValues(typeof(Level.SpecialLayer)).Cast<Level.SpecialLayer>();

            foreach (Level.SpecialLayer specialLayer in Enum.GetValues(typeof(Level.SpecialLayer)).Cast<Level.SpecialLayer>())
            {
                _level.SpecialLayers[specialLayer] = _level.Map.Layers[0];
                Assert.AreEqual("true", _level.Map.Layers[0].Properties["Is" + specialLayer.ToString() + "Layer"]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SpecialLayers_SettingASpecialLayerWhenTheSpecificSpecialLayerAlreadyExists_ThrowsException()
        {
            // Usually special layers requires quite a bit of processing by the framework. For example, processing the character layer sets up teams, NPCs, PCs etc. Once the processing is done for a special layer, there is no easy way currently to undo and redo processing for a new layer. We therefore throw an exception to prevent a special layer being reassigned to another layer.
            _level.SpecialLayers[Level.SpecialLayer.Background] = _level.Map.Layers[0];
            _level.SpecialLayers[Level.SpecialLayer.Background] = _level.Map.Layers[1];
        }

        [TestMethod]
        public void SpecialLayers_WhenASpecialLayerExists_ReturnsTheLayer()
        {
            _level.SpecialLayers[Level.SpecialLayer.Background] = _level.Map.Layers[0];

            Assert.AreEqual(_level.Map.Layers[0], _level.SpecialLayers[Level.SpecialLayer.Background]);
        }

        [TestMethod]
        public void SpecialLayers_WhenASpecialLayerDoesNotExist_ReturnsNull()
        {
            _level.SpecialLayers[Level.SpecialLayer.Character] = _level.Map.Layers[0];

            Assert.IsNull(_level.SpecialLayers[Level.SpecialLayer.Background]);
        }
    }
}

//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TurnItUp.Locations;
//using TurnItUp.Interfaces;
//using Moq;
//using TurnItUp.Components;
//using System.Collections.Generic;
//using Entropy;
//using TurnItUp.Pathfinding;
//using TurnItUp.Tmx;
//using System.Tuples;
//using Tests.Factories;

//namespace Tests.Locations
//{
//    [TestClass]
//    public class LevelTests
//    {
//        // The sample level:
//        // XXXXXXXXXXXXXXXX
//        // X....EEE.......X
//        // X..........X...X
//        // X.......E......X
//        // X.E.X..........X
//        // X.....E....E...X
//        // X........X.....X
//        // X..........XXXXX
//        // X..........X...X
//        // X..........X...X
//        // X......X.......X
//        // X.X........X...X
//        // X..........X...X
//        // X..........X...X
//        // X......P...X...X
//        // XXXXXXXXXXXXXXXX
//        // X - Obstacles, P - Player, E - Enemies

//        private ILevel _level;
//        private Mock<ICharacterManager> _mockCharacterManager;
//        private IWorld _world;
//        //private bool _eventTriggeredFlag;
//        //private EventArgs _eventArgs;

//        [TestInitialize]
//        public void Initialize()
//        {
//            //_eventTriggeredFlag = false;
//            _world = new World();
//            _level = LocationsFactory.BuildLevel();
//            _mockCharacterManager = new Mock<ICharacterManager>();
//        }

//        [TestMethod]
//        public void Level_Construction_IsSuccessful()
//        {
//        }

//        [TestMethod]
//        public void Level_SettingUpCharacters_DelegatesToTheCharacterManager()
//        {
//            Mock<ICharacterManager> characterManagerMock = new Mock<ICharacterManager>();
//            _level.CharacterManager = characterManagerMock.Object;

//            characterManagerMock.Setup(cm => cm.SetUpNpcs());
//            characterManagerMock.Setup(cm => cm.SetUpPc("Knight M", 7, 2));

//            _level.SetUpCharacters("Knight M", 7, 2);

//            characterManagerMock.Verify(cm => cm.SetUpNpcs());
//            characterManagerMock.Verify(cm => cm.SetUpPc("Knight M", 7, 2));
//        }

//        [TestMethod]
//        public void Level_SettingUpNpcs_DelegatesToTheCharacterManager()
//        {
//            Mock<ICharacterManager> characterManagerMock = new Mock<ICharacterManager>();
//            _level.CharacterManager = characterManagerMock.Object;

//            characterManagerMock.Setup(cm => cm.SetUpNpcs());

//            _level.SetUpNpcs();

//            characterManagerMock.Verify(cm => cm.SetUpNpcs());
//        }

//        // Setup methods
//        [TestMethod]
//        public void Level_SettingUpAPathfinder_IsSuccessful()
//        {
//            _level.SetUpPathfinder();

//            Assert.IsNotNull(_level.PathFinder);
//            Assert.IsFalse(_level.PathFinder.AllowDiagonalMovement);
//            Assert.AreEqual(_level, _level.PathFinder.Level);
//        }

//        [TestMethod]
//        public void Level_SettingUpAPathfinderThatAllowsDiagonalMovement_IsSuccessful()
//        {
//            _level.SetUpPathfinder(true);

//            Assert.IsNotNull(_level.PathFinder);
//            Assert.IsTrue(_level.PathFinder.AllowDiagonalMovement);
//            Assert.AreEqual(_level, _level.PathFinder.Level);
//        }

//        [TestMethod]
//        public void Level_SettingUpTransitionPoints_IsSuccessful()
//        {
//            _level.SetUpTransitionPoints();

//            Assert.IsNotNull(_level.TransitionPointManager);
//            Assert.IsNotNull(_level.TransitionPointManager.Entrance);
//            Assert.AreEqual(1, _level.TransitionPointManager.Exits.Count);
//            Assert.AreEqual(_level, _level.TransitionPointManager.Level);
//        }

//        [TestMethod]
//        public void Level_SettingUpAViewportWithJustAWidthAndHeight_IsSuccessful()
//        {
//            _level.SetUpViewport(5, 6);

//            Assert.IsNotNull(_level.Viewport);
//            Assert.AreEqual(_level, _level.Viewport.Level);
//            Assert.AreEqual(5, _level.Viewport.Width);
//            Assert.AreEqual(6, _level.Viewport.Height);
//        }

//        [TestMethod]
//        public void Level_SettingUpAViewport_IsSuccessful()
//        {
//            _level.SetUpViewport(8, 8, 5, 5);

//            Assert.IsNotNull(_level.Viewport);
//            Assert.AreEqual(_level, _level.Viewport.Level);
//            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
//            Assert.AreEqual(8, _level.Viewport.MapOrigin.Y);
//            Assert.AreEqual(5, _level.Viewport.Width);
//            Assert.AreEqual(5, _level.Viewport.Height);
//        }

//        [TestMethod]
//        public void Level_SettingUpAMap_IsSuccessful()
//        {
//            _level.SetUpMap("../../Fixtures/FullExample.tmx");

//            Assert.IsNotNull(_level.Map);
//        }



//        [TestMethod]
//        public void Level_CalculatingAllWalkablePositions_ReturnsAListOfAllWalkablePositionsInTheLevel()
//        {
//            List<Position> allWalkablePositions = _level.CalculateWalkablePositions();

//            Assert.AreEqual(172, allWalkablePositions.Count);

//            foreach (Position position in allWalkablePositions)
//            {
//                Assert.IsTrue(new Node(_level, position.X, position.Y).IsWalkable());
//            }
//        }

//        // Facade implementation tests
//        [TestMethod]
//        public void Level_DeterminingIfCharacterIsAtAPosition_DelegatesToCharacterManager()
//        {
//            _level.CharacterManager = _mockCharacterManager.Object;

//            _level.IsCharacterAt(0, 0);
//            _mockCharacterManager.Verify(cm => cm.IsCharacterAt(0, 0));
//        }

//        [TestMethod]
//        public void Level_MovingAPlayer_DelegatesToCharacterManager()
//        {
//            Entity player = _world.CreateEntity();
//            player.AddComponent(new Position(0, 0));
//            _mockCharacterManager.SetupGet<Entity>(cm => cm.Player).Returns(player);

//            _level.CharacterManager = _mockCharacterManager.Object;

//            _level.MovePlayer(Direction.South);
//            _mockCharacterManager.Verify(cm => cm.MovePlayer(Direction.South));
//        }

//        [TestMethod]
//        public void Level_MovingACharacterToAPosition_DelegatesToCharacterManager()
//        {
//            _level.CharacterManager = _mockCharacterManager.Object;

//            _level.MoveCharacterTo(null, new Position(0, 0));
//            _mockCharacterManager.Verify(cm => cm.MoveCharacterTo(null, new Position(0, 0)));
//        }
//    }
//}
