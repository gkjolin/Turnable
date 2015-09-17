using System;
using NUnit.Framework;
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
    [TestFixture]
    public class LevelTests
    {
        private ILevel _level;

        [SetUp]
        public void SetUp()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [Test]
        public void DefaultConstructor_InitializesAllProperties()
        {
            ILevel level = new Level();

            // TODO: Uncomment these out as we implement these systems
            Assert.That(level.Map, Is.Not.Null);
            Assert.That(level.SpecialLayers, Is.Not.Null);
            Assert.That(level.Pathfinder, Is.Null);
            Assert.That(level.CharacterManager, Is.Null);
            Assert.That(level.ModelManager, Is.Null);
            //Assert.IsNull(level.World);
            //Assert.IsNull(level.VisionCalculator);
            //Assert.IsNull(level.Viewport);
        }

        [Test]
        public void Setup_GivenParametersWithATmxFullFilePath_InitializesTheTiledMap()
        {
            LevelSetupParameters levelSetupParameters = new LevelSetupParameters();
            levelSetupParameters.TmxFullFilePath = "c:/git/Turnable/Tests/Fixtures/FullExample.tmx";

            ILevel level = new Level(levelSetupParameters);

            // TODO: This should really test that Map's constructor was called correctly and assigned to the Level's Map property.
            Assert.That(level.Map, Is.Not.Null);
            Assert.That(level.Map.Layers.Count, Is.EqualTo(4));
        }

        [Test]
        public void IsCollidable_ReturnsTrueIfThereIsAnyTileInTheCollisionSpecialLayerForTheLevel()
        {
            // The example level has a "wall" around the entire 15x15 level
            Assert.That(_level.IsCollidable(new Position(0, 0)), Is.True);
            Assert.That(_level.IsCollidable(new Position(0, 1)), Is.True);
            Assert.That(_level.IsCollidable(new Position(1, 0)), Is.True);
            Assert.That(_level.IsCollidable(new Position(2, 0)), Is.True);

            Assert.That(_level.IsCollidable(new Position(9, 1)), Is.False);
        }

        [Test]
        public void IsCollidable_IfThereIsNoCollisionLayer_AlwaysReturnsFalse()
        {
            // The example level has a "wall" around the entire 15x15 level
            _level.SpecialLayers.Remove(SpecialLayer.Collision);

            Assert.That(_level.IsCollidable(new Position(0, 0)), Is.False);
            Assert.That(_level.IsCollidable(new Position(0, 1)), Is.False);
            Assert.That(_level.IsCollidable(new Position(1, 0)), Is.False);
            Assert.That(_level.IsCollidable(new Position(2, 0)), Is.False);
        }

        // Special layer tests
        [Test]
        public void Map_WhenSet_CallsInitializeSpecialLayers()
        {
            Mock<Level> mockLevel = new Mock<Level>();
            mockLevel.CallBase = true;
            mockLevel.Setup(l => l.InitializeSpecialLayers()).Verifiable();

            mockLevel.Object.Map = new Map("c:/git/Turnable/Tests/Fixtures/FullExample.tmx");

            mockLevel.Verify(l => l.InitializeSpecialLayers());
        }

        [Test]
        public void SpecialLayerPropertyKey_ReturnsTheRightPropertyName()
        {
            foreach (SpecialLayer specialLayer in Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>())
            {
                Assert.That(Level.SpecialLayerPropertyKey(specialLayer), Is.EqualTo("Is" + specialLayer.ToString() + "Layer"));
            }
        }

        [Test]
        public void InitializeSpecialLayers_SetsAllTheSpecialLayersPresentInTheMap()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            _level.InitializeSpecialLayers();
            
            Assert.That(_level.SpecialLayers[SpecialLayer.Background], Is.SameAs(_level.Map.Layers[0]));
            Assert.That(_level.SpecialLayers[SpecialLayer.Collision], Is.SameAs(_level.Map.Layers[1]));
            Assert.That(_level.SpecialLayers[SpecialLayer.Object], Is.SameAs(_level.Map.Layers[2]));
            Assert.That(_level.SpecialLayers[SpecialLayer.Character], Is.SameAs(_level.Map.Layers[3]));
        }

        [Test]
        public void Level_SpecialLayerEnum_Defines4DifferentSpecialLayers()
        {
            Assert.That(Enum.GetValues(typeof(SpecialLayer)).Length, Is.EqualTo(4));
            Assert.That(Enum.IsDefined(typeof(SpecialLayer), "Background"), Is.True);
            Assert.That(Enum.IsDefined(typeof(SpecialLayer), "Collision"), Is.True);
            Assert.That(Enum.IsDefined(typeof(SpecialLayer), "Object"), Is.True);
            Assert.That(Enum.IsDefined(typeof(SpecialLayer), "Character"), Is.True);
        }

        [Test]
        public void SpecialLayers_AllowsSettingASpecialLayerUsingTheSpecialLayerEnumAsKey()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            var values = Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>();

            foreach (SpecialLayer specialLayer in Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>())
            {
                _level.SpecialLayers[specialLayer] = _level.Map.Layers[0];
                Assert.That(_level.Map.Layers[0].Properties["Is" + specialLayer.ToString() + "Layer"], Is.EqualTo("true"));
            }
        }

        [Test]
        public void SpecialLayers_SettingASpecialLayerWhenTheSpecificSpecialLayerAlreadyExists_ThrowsException()
        {
            // Usually special layers requires quite a bit of processing by the framework. For example, processing the character layer sets up teams, NPCs, PCs etc. Once the processing is done for a special layer, there is no easy way currently to undo and redo processing for a new layer. We therefore throw an exception to prevent a special layer being reassigned to another layer.
            _level.SpecialLayers.Clear();
            _level.SpecialLayers[SpecialLayer.Background] = _level.Map.Layers[0];

            Assert.That(() => _level.SpecialLayers[SpecialLayer.Background] = _level.Map.Layers[1], Throws.ArgumentException);
        }

        [Test]
        public void SpecialLayers_WhenASpecialLayerExists_ReturnsTheLayer()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            _level.SpecialLayers[SpecialLayer.Background] = _level.Map.Layers[0];

            Assert.That(_level.SpecialLayers[SpecialLayer.Background], Is.SameAs(_level.Map.Layers[0]));
        }

        [Test]
        public void SpecialLayers_WhenASpecialLayerDoesNotExist_ReturnsNull()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.SpecialLayers.Clear();
            _level.SpecialLayers[SpecialLayer.Character] = _level.Map.Layers[0];

            Assert.That(_level.SpecialLayers[SpecialLayer.Background], Is.Null);
        }

        // Map Manipulation Tests
        [Test]
        public void SetLayer_GivenASpecialLayerEnumValue_CreatesANewSpecialLayer()
        {
            // The Special Layers are already setup which will cause this test to fail unless we clear out the SpecialLayers collection
            _level.Map.Layers.Clear();
            _level.SpecialLayers.Clear();

            _level.SetLayer("Layer 1", 48, 64, SpecialLayer.Character);

            Assert.That(_level.Map.Layers.Count, Is.EqualTo(1));
            Assert.That(_level.SpecialLayers[SpecialLayer.Character], Is.Not.Null);
        }

        [Test]
        public void SetLayer_ForANewlyConstructedLevel_StillWorksCorrectly()
        {
            Level level = new Level();

            level.SetLayer("Layer 1", 48, 64, SpecialLayer.Character);

            Assert.That(level.Map.Layers.Count, Is.EqualTo(1));
            Assert.That(level.SpecialLayers[SpecialLayer.Character], Is.Not.Null);
        }

        // -------
        // Set Ups
        // -------

        // Viewport
        [Test]
        public void SetUpViewport_GivenNoParameters_CreatesAViewportTheSameSizeAsTheLevel()
        {
            _level.SetUpViewport();

            Assert.That(_level.Viewport, Is.Not.Null);
            Assert.That(_level.Viewport.Level, Is.SameAs(_level));
        }

        [Test]
        public void SetUpViewport_GivenAWidthAndHeight_CreatesAViewportForTheLevel()
        {
            _level.SetUpViewport(5, 6);

            Assert.IsNotNull(_level.Viewport);
            Assert.That(_level.Viewport.Level, Is.SameAs(_level));
        }

        [Test]
        public void SetUpViewport_GivenAMapOriginWidthAndHeight_CreatesAViewportForTheLevel()
        {
            _level.SetUpViewport(new Position(8, 8), 5, 5);

            Assert.IsNotNull(_level.Viewport);
            Assert.That(_level.Viewport.Level, Is.SameAs(_level));
        }

        // VisionCalculator
        [Test]
        public void SetUpVisionCalculator_CreatesAVisionCalculatorForTheLevel()
        {
            _level.SetUpVisionCalculator();

            Assert.That(_level.VisionCalculator, Is.Not.Null);
            Assert.That(_level.VisionCalculator.Level, Is.SameAs(_level));
        }

        // CharacterManager
        [Test]
        public void SetUpCharacterManager_CreatesACharacterManagerForTheLevel()
        {
            _level.SetUpCharacterManager();

            Assert.That(_level.CharacterManager, Is.Not.Null);
            Assert.That(_level.CharacterManager.Level, Is.SameAs(_level));
        }

        // ModelManager
        [Test]
        public void SetUpModelManager_CreatesAModelManagerForTheLevel()
        {
            _level.SetUpModelManager();

            Assert.That(_level.ModelManager, Is.Not.Null);
            Assert.That(_level.ModelManager.Level, Is.SameAs(_level));
        }
    }
}

//namespace Tests.Locations
//{
//    [TestFixture]
//    public class LevelTests
//    {
//        [SetUp]
//        public void SetUp()
//        {
//            //_eventTriggeredFlag = false;
//            _world = new World();
//            _level = LocationsFactory.BuildLevel();
//            _mockCharacterManager = new Mock<ICharacterManager>();
//        }

//        [Test]
//        public void Level_Construction_IsSuccessful()
//        {
//        }

//        [Test]
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

//        [Test]
//        public void Level_SettingUpNpcs_DelegatesToTheCharacterManager()
//        {
//            Mock<ICharacterManager> characterManagerMock = new Mock<ICharacterManager>();
//            _level.CharacterManager = characterManagerMock.Object;

//            characterManagerMock.Setup(cm => cm.SetUpNpcs());

//            _level.SetUpNpcs();

//            characterManagerMock.Verify(cm => cm.SetUpNpcs());
//        }

//        // Setup methods
//        [Test]
//        public void Level_SettingUpAPathfinder_IsSuccessful()
//        {
//            _level.SetUpPathfinder();

//            Assert.IsNotNull(_level.PathFinder);
//            Assert.IsFalse(_level.PathFinder.AllowDiagonalMovement);
//            Assert.That(_level, _level.PathFinder.Level);
//        }

//        [Test]
//        public void Level_SettingUpAPathfinderThatAllowsDiagonalMovement_IsSuccessful()
//        {
//            _level.SetUpPathfinder(true);

//            Assert.IsNotNull(_level.PathFinder);
//            Assert.IsTrue(_level.PathFinder.AllowDiagonalMovement);
//            Assert.That(_level, _level.PathFinder.Level);
//        }

//        [Test]
//        public void Level_SettingUpTransitionPoints_IsSuccessful()
//        {
//            _level.SetUpTransitionPoints();

//            Assert.IsNotNull(_level.TransitionPointManager);
//            Assert.IsNotNull(_level.TransitionPointManager.Entrance);
//            Assert.That(1, _level.TransitionPointManager.Exits.Count);
//            Assert.That(_level, _level.TransitionPointManager.Level);
//        }



//        [Test]
//        public void Level_SettingUpAMap_IsSuccessful()
//        {
//            _level.SetUpMap("../../Fixtures/FullExample.tmx");

//            Assert.IsNotNull(_level.Map);
//        }



//        [Test]
//        public void Level_CalculatingAllWalkablePositions_ReturnsAListOfAllWalkablePositionsInTheLevel()
//        {
//            List<Position> allWalkablePositions = _level.CalculateWalkablePositions();

//            Assert.That(172, allWalkablePositions.Count);

//            foreach (Position position in allWalkablePositions)
//            {
//                Assert.IsTrue(new Node(_level, position.X, position.Y).IsWalkable());
//            }
//        }

//        // Facade implementation tests
//        [Test]
//        public void Level_DeterminingIfCharacterIsAtAPosition_DelegatesToCharacterManager()
//        {
//            _level.CharacterManager = _mockCharacterManager.Object;

//            _level.IsCharacterAt(0, 0);
//            _mockCharacterManager.Verify(cm => cm.IsCharacterAt(0, 0));
//        }

//        [Test]
//        public void Level_MovingAPlayer_DelegatesToCharacterManager()
//        {
//            Entity player = _world.CreateEntity();
//            player.AddComponent(new Position(0, 0));
//            _mockCharacterManager.SetupGet<Entity>(cm => cm.Player).Returns(player);

//            _level.CharacterManager = _mockCharacterManager.Object;

//            _level.MovePlayer(Direction.South);
//            _mockCharacterManager.Verify(cm => cm.MovePlayer(Direction.South));
//        }

//        [Test]
//        public void Level_MovingACharacterToAPosition_DelegatesToCharacterManager()
//        {
//            _level.CharacterManager = _mockCharacterManager.Object;

//            _level.MoveCharacterTo(null, new Position(0, 0));
//            _mockCharacterManager.Verify(cm => cm.MoveCharacterTo(null, new Position(0, 0)));
//        }
//    }
//}
