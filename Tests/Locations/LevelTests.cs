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
        public void DefaultConstructor_InitializesAllProperties()
        {
            ILevel level = new Level();

            // TODO: Uncomment these out as we implement these systems
            Assert.IsNull(level.Map);
            Assert.IsNull(level.Pathfinder);
            Assert.IsNull(level.CharacterManager);
            Assert.IsNull(level.ModelManager);
            Assert.IsNotNull(level.SpecialLayers);
            //Assert.IsNull(level.World);
            //Assert.IsNull(level.VisionCalculator);
            //Assert.IsNull(level.Viewport);
        }

        [TestMethod]
        public void Setup_GivenParametersWithATmxFullFilePath_InitializesTheTiledMap()
        {
            LevelSetupParameters levelSetupParameters = new LevelSetupParameters();
            levelSetupParameters.TmxFullFilePath = "../../Fixtures/FullExample.tmx";

            ILevel level = new Level(levelSetupParameters);

            // TODO: This should really test that Map's constructor was called correctly and assigned to the Level's Map property.
            Assert.IsNotNull(level.Map);
            Assert.AreEqual(4, level.Map.Layers.Count);
        }

        [TestMethod]
        public void IsCollidable_ReturnsTrueIfThereIsAnyTileInTheCollisionSpecialLayerForTheLevel()
        {
            // The example level has a "wall" around the entire 15x15 level
            Assert.IsTrue(_level.IsCollidable(new Position(0, 0)));
            Assert.IsTrue(_level.IsCollidable(new Position(0, 1)));
            Assert.IsTrue(_level.IsCollidable(new Position(1, 0)));
            Assert.IsTrue(_level.IsCollidable(new Position(2, 0)));

            Assert.IsFalse(_level.IsCollidable(new Position(9, 1)));
        }

        [TestMethod]
        public void IsCollidable_IfThereIsNoCollisionLayer_AlwaysReturnsFalse()
        {
            // The example level has a "wall" around the entire 15x15 level
            _level.SpecialLayers.Remove(SpecialLayer.Collision);

            Assert.IsFalse(_level.IsCollidable(new Position(0, 0)));
            Assert.IsFalse(_level.IsCollidable(new Position(0, 1)));
            Assert.IsFalse(_level.IsCollidable(new Position(1, 0)));
            Assert.IsFalse(_level.IsCollidable(new Position(2, 0)));
        }

        // Special layer tests
        [TestMethod]
        public void Map_WhenSet_CallsInitializeSpecialLayers()
        {
            Mock<Level> mockLevel = new Mock<Level>();
            mockLevel.CallBase = true;
            mockLevel.Setup(l => l.InitializeSpecialLayers()).Verifiable();

            mockLevel.Object.Map = new Map("../../Fixtures/FullExample.tmx");

            mockLevel.Verify(l => l.InitializeSpecialLayers());
        }

        [TestMethod]
        public void SpecialLayerPropertyKey_ReturnsTheRightPropertyName()
        {
            foreach (SpecialLayer specialLayer in Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>())
            {
                Assert.AreEqual("Is" + specialLayer.ToString() + "Layer", Level.SpecialLayerPropertyKey(specialLayer));
            }
        }

        [TestMethod]
        public void InitializeSpecialLayers_SetsAllTheSpecialLayersPresentInTheMap()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            _level.InitializeSpecialLayers();
            
            Assert.AreEqual(_level.Map.Layers[0], _level.SpecialLayers[SpecialLayer.Background]);
            Assert.AreEqual(_level.Map.Layers[1], _level.SpecialLayers[SpecialLayer.Collision]);
            Assert.AreEqual(_level.Map.Layers[2], _level.SpecialLayers[SpecialLayer.Object]);
            Assert.AreEqual(_level.Map.Layers[3], _level.SpecialLayers[SpecialLayer.Character]);
        }

        [TestMethod]
        public void Level_SpecialLayerEnum_Defines4DifferentSpecialLayers()
        {
            Assert.AreEqual(4, Enum.GetValues(typeof(SpecialLayer)).Length);
            Assert.IsTrue(Enum.IsDefined(typeof(SpecialLayer), "Background"));
            Assert.IsTrue(Enum.IsDefined(typeof(SpecialLayer), "Collision"));
            Assert.IsTrue(Enum.IsDefined(typeof(SpecialLayer), "Object"));
            Assert.IsTrue(Enum.IsDefined(typeof(SpecialLayer), "Character"));
        }

        [TestMethod]
        public void SpecialLayers_AllowsSettingASpecialLayerUsingTheSpecialLayerEnumAsKey()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            var values = Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>();

            foreach (SpecialLayer specialLayer in Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>())
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
            _level.SpecialLayers[SpecialLayer.Background] = _level.Map.Layers[0];
            _level.SpecialLayers[SpecialLayer.Background] = _level.Map.Layers[1];
        }

        [TestMethod]
        public void SpecialLayers_WhenASpecialLayerExists_ReturnsTheLayer()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            _level.SpecialLayers[SpecialLayer.Background] = _level.Map.Layers[0];

            Assert.AreEqual(_level.Map.Layers[0], _level.SpecialLayers[SpecialLayer.Background]);
        }

        [TestMethod]
        public void SpecialLayers_WhenASpecialLayerDoesNotExist_ReturnsNull()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            _level.SpecialLayers[SpecialLayer.Character] = _level.Map.Layers[0];

            Assert.IsNull(_level.SpecialLayers[SpecialLayer.Background]);
        }

        // -------
        // Set Ups
        // -------

        // Viewport
        [TestMethod]
        public void SetUpViewport_GivenNoParamters_CreatesAViewportTheSameSizeAsTheLevel()
        {
            _level.SetUpViewport();

            Assert.IsNotNull(_level.Viewport);
            Assert.AreEqual(_level, _level.Viewport.Level);
            Assert.AreEqual(0, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(0, _level.Viewport.MapOrigin.Y);
            Assert.AreEqual(_level.Map.Width, _level.Viewport.Width);
            Assert.AreEqual(_level.Map.Height, _level.Viewport.Height);
        }

        [TestMethod]
        public void SetUpViewport_GivenAWidthAndHeight_CreatesAViewportForTheLevel()
        {
            _level.SetUpViewport(5, 6);

            Assert.IsNotNull(_level.Viewport);
            Assert.AreEqual(_level, _level.Viewport.Level);
            Assert.AreEqual(0, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(0, _level.Viewport.MapOrigin.Y);
            Assert.AreEqual(5, _level.Viewport.Width);
            Assert.AreEqual(6, _level.Viewport.Height);
        }

        [TestMethod]
        public void SetUpViewport_GivenAMapOriginWidthAndHeight_CreatesAViewportForTheLevel()
        {
            _level.SetUpViewport(8, 8, 5, 5);

            Assert.IsNotNull(_level.Viewport);
            Assert.AreEqual(_level, _level.Viewport.Level);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.X);
            Assert.AreEqual(8, _level.Viewport.MapOrigin.Y);
            Assert.AreEqual(5, _level.Viewport.Width);
            Assert.AreEqual(5, _level.Viewport.Height);
        }

        // VisionCalculator
        [TestMethod]
        public void SetUpVisionCalculator_CreatesAVisionCalculatorForTheLevel()
        {
            _level.SetUpVisionCalculator();

            Assert.IsNotNull(_level.VisionCalculator);
            Assert.AreEqual(_level, _level.VisionCalculator.Level);
        }

        // CharacterManager
        [TestMethod]
        public void SetUpCharacterManager_CreatesACharacterManagerForTheLevel()
        {
            _level.SetUpCharacterManager();

            Assert.IsNotNull(_level.CharacterManager);
            Assert.AreEqual(_level, _level.CharacterManager.Level);
        }

        // ModelManager
        [TestMethod]
        public void SetUpModelManager_CreatesAModelManagerForTheLevel()
        {
            _level.SetUpModelManager();

            Assert.IsNotNull(_level.ModelManager);
            Assert.AreEqual(_level, _level.ModelManager.Level);
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
