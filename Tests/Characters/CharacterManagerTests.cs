using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;
using Moq;
using TurnItUp.Interfaces;
using TurnItUp.Tmx;
using TurnItUp.Stats;

namespace Tests.Characters
{
    [TestClass]
    public class CharacterManagerTests
    {
        private IWorld _world;
        private ILevel _level;
        private CharacterManager _characterManager;
        private bool _eventTriggeredFlag;
        private EntityEventArgs _eventArgs;

        [TestInitialize]
        public void Initialize()
        {
            _eventTriggeredFlag = false;
            _level = LocationsFactory.BuildLevel();
            _world = _level.World;
            _characterManager = (CharacterManager)_level.CharacterManager;
        }

        [TestMethod]
        public void CharacterManager_Construction_IsSuccessful()
        {
            CharacterManager characterManager = new CharacterManager(_level);

            Assert.AreEqual(_level, characterManager.Level);
        }

        [TestMethod]
        public void CharacterManager_SettingUpNpcs_IsSuccessful()
        {
            // TODO: Check that the position of the characters is set correctly
            CharacterManager characterManager = new CharacterManager(_level);
            characterManager.SetUpNpcs();

            Assert.IsNotNull(characterManager.Characters);
            Assert.AreEqual(8, characterManager.Characters.Count);

            // Are all Characters set up with a Model?
            foreach (Entity character in characterManager.Characters)
            {
                Assert.IsNotNull(character.GetComponent<Model>());
                // TODO: Test that the models are set up correctly for each character
                Assert.IsTrue(new List<String> { "Skeleton", "Skeleton Archer", "Pharaoh" }.Contains(character.GetComponent<Model>().Name));
            }

            // Is a TurnQueue setup with the Player taking the first turn?
            Assert.IsNotNull(characterManager.TurnQueue);
            Assert.AreEqual(8, characterManager.TurnQueue.Count);

            foreach (Entity character in characterManager.Characters)
            {
                Assert.AreEqual(_level, character.GetComponent<OnLevel>().Level);
            }
        }

        [TestMethod]
        public void CharacterManager_SettingUpNpcs_IgnoresCharacterTilesThatHaveNoModelPropertySet()
        {
            _level = LocationsFactory.BuildLevel("../../Fixtures/FullExampleWithUnsetModelForSomeCharacters.tmx");

            CharacterManager characterManager = new CharacterManager(_level);
            characterManager.SetUpNpcs();

            Assert.AreEqual(characterManager.Level, _level);
            Assert.IsNotNull(characterManager.Characters);
            Assert.AreEqual(8, characterManager.Characters.Count);

            // Are all Characters set up with a Model IF they had a model property in the reference tile?
            foreach (Entity character in characterManager.Characters)
            {
                if (character.GetComponent<Model>() != null)
                {
                    // TODO: Test that the models are set up correctly for each character
                    Assert.IsTrue(new List<String> { "Knight M", "Skeleton", "Skeleton Archer", "Pharaoh" }.Contains(character.GetComponent<Model>().Name));
                }
            }
        }

        [TestMethod]
        public void CharacterManager_SettingUpPc_IsSuccessful()
        {
            _level = LocationsFactory.BuildLevel("../../Fixtures/FullExample.tmx", false);
            CharacterManager characterManager = new CharacterManager(_level);
            characterManager.SetUpPc("Knight M", 7, 1);

            Assert.IsNotNull(characterManager.Player);
            Assert.AreEqual(9, characterManager.Characters.Count);
            Assert.AreEqual(new Position(7, 1), characterManager.Player.GetComponent<Position>());
            Assert.AreEqual("Knight M", characterManager.Player.GetComponent<Model>());
            Assert.AreEqual(_level, characterManager.Player.GetComponent<OnLevel>().Level);

            // Is a TurnQueue setup with the Player taking the first turn?
            Assert.IsNotNull(characterManager.TurnQueue);
            Assert.AreEqual(characterManager.Player, characterManager.TurnQueue[0]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPosition_MovesCharacterCorrectly()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y);

            MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            Assert.AreEqual(MoveResultStatus.Success, moveResult.Status);
            Assert.AreEqual(newPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Path.Count);
            Assert.AreEqual(currentPosition, moveResult.Path[0]);
            Assert.AreEqual(newPosition, moveResult.Path[1]);

            // Check to see if the tile in the map was moved as well
            Assert.IsFalse(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
            Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(newPosition.X, newPosition.Y)));
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPositionOccupiedByAnObstacle_ReturnsHitObstacleMoveResultAndPositionOfObstacleToIndicateFailure()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y + 1);

            MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            // Make sure that character was NOT moved
            Assert.AreEqual(MoveResultStatus.HitObstacle, moveResult.Status);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Path.Count);
            Assert.AreEqual(currentPosition, moveResult.Path[0]);
            Assert.AreEqual(new Position(4, 15), moveResult.Path[1]);

            // Check to see if the tile in the map was NOT moved
            Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
            Assert.IsFalse(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(newPosition.X, newPosition.Y)));
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPositionOccupiedByAnotherCharacter_ReturnsHitCharacterMoveResultAndPositionOfOtherCharacterToIndicateFailure()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X + 1, currentPosition.Y);

            MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            // Make sure that character was NOT moved
            Assert.AreEqual(MoveResultStatus.HitCharacter, moveResult.Status);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Path.Count);
            Assert.AreEqual(currentPosition, moveResult.Path[0]);
            Assert.AreEqual(new Position(6, 14), moveResult.Path[1]);

            // Check to see if the tile in the map was NOT moved
            Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPositionOutOfBoundsOfTheMap_ReturnsOutOfBoundsMoveResultAndPositionOfOutOfBoundsLocationToIndicateFailure()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(-1, -1);

            MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            // Make sure that character was NOT moved
            Assert.AreEqual(MoveResultStatus.OutOfBounds, moveResult.Status);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Path.Count);
            Assert.AreEqual(currentPosition, moveResult.Path[0]);
            Assert.AreEqual(new Position(-1, -1), moveResult.Path[1]);

            // Check to see if the tile in the map was NOT moved
            Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterByDirection_DelegatesToMoveCharacterTo()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>();
            Mock<CharacterManager> characterManagerMock = new Mock<CharacterManager>() { CallBase = true };
            characterManagerMock.Setup(cm => cm.MoveCharacterTo(It.IsAny<Entity>(), It.IsAny<Position>()));

            characterManagerMock.Object.MoveCharacter(character, Direction.West);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X - 1, currentPosition.Y)));

            characterManagerMock.Object.MoveCharacter(character, Direction.East);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X + 1, currentPosition.Y)));

            characterManagerMock.Object.MoveCharacter(character, Direction.North);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X, currentPosition.Y + 1)));

            characterManagerMock.Object.MoveCharacter(character, Direction.South);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X, currentPosition.Y - 1)));

            characterManagerMock.Object.MoveCharacter(character, Direction.NorthWest);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X - 1, currentPosition.Y + 1)));

            characterManagerMock.Object.MoveCharacter(character, Direction.NorthEast);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X + 1, currentPosition.Y + 1)));

            characterManagerMock.Object.MoveCharacter(character, Direction.SouthWest);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X - 1, currentPosition.Y - 1)));

            characterManagerMock.Object.MoveCharacter(character, Direction.SouthEast);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X + 1, currentPosition.Y - 1)));
        }

        [TestMethod]
        public void CharacterManager_TryingToMovePlayer_DelegatesToMoveCharacter()
        {
            Entity player = _characterManager.Player;
            Mock<CharacterManager> characterManagerMock = new Mock<CharacterManager>() { CallBase = true };
            characterManagerMock.Setup(cm => cm.Player).Returns(player);
            characterManagerMock.Setup(cm => cm.MoveCharacter(It.IsAny<Entity>(), It.IsAny<Direction>()));

            characterManagerMock.Object.MovePlayer(Direction.South);

            characterManagerMock.Verify(cm => cm.MoveCharacter(player, Direction.South));
        }

        [TestMethod]
        public void CharacterManager_CanDetermineIfThereIsACharacterAtALocation()
        {
            Assert.IsTrue(_characterManager.IsCharacterAt(_characterManager.Characters[0].GetComponent<Position>().X, _characterManager.Characters[0].GetComponent<Position>().Y));
        }

        [TestMethod]
        public void CharacterManager_EndingTurn_MovesTheCurrentCharacterToTheEndOfTheTurnQueue()
        {
            Entity firstCharacter = _characterManager.TurnQueue[0];
            Entity secondCharacter = _characterManager.TurnQueue[1];

            _characterManager.EndTurn();

            Assert.AreEqual(secondCharacter, _characterManager.TurnQueue[0]);
            Assert.AreEqual(firstCharacter, _characterManager.TurnQueue[_characterManager.TurnQueue.Count - 1]);
        }

        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
            _eventArgs = (EntityEventArgs)e;
        }

        [TestMethod]
        public void CharacterManager_EndingTurn_RaisesACharacterTurnEndedEvent()
        {
            // TODO: How do I check that the EntityEventArgs are correctly set?
            _characterManager.CharacterTurnEnded += this.SetEventTriggeredFlag;
            _characterManager.EndTurn();
            Assert.IsTrue(_eventTriggeredFlag);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPosition_RaisesACharacterMovedEvent()
        {
            // TODO: How do I check that the EntityEventArgs are correctly set?
            _characterManager.CharacterMoved += this.SetEventTriggeredFlag;
            _characterManager.MoveCharacterTo(_characterManager.Characters[0], new Position(1, 1));
            Assert.IsTrue(_eventTriggeredFlag);
        }

        [TestMethod]
        public void CharacterManager_DestroyingACharacter_RaisesACharacterDestroyedEvent()
        {
            Entity characterToDestroy = _characterManager.Characters[0];

            // TODO: How do I check that the EntityEventArgs are correctly set?
            _characterManager.CharacterDestroyed += this.SetEventTriggeredFlag;
            _characterManager.DestroyCharacter(characterToDestroy);
            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(characterToDestroy, ((EntityEventArgs)_eventArgs).Entity);
        }

        [TestMethod]
        public void CharacterManager_DestroyingACharacter_RemovesItFromCharactersAndTheTurnQueue()
        {
            Entity characterToDestroy = _characterManager.Characters[0];

            _characterManager.DestroyCharacter(characterToDestroy);

            Assert.AreEqual(8, _characterManager.Characters.Count);
            Assert.IsFalse(_characterManager.Characters.Contains(characterToDestroy));
            Assert.IsFalse(_characterManager.TurnQueue.Contains(characterToDestroy));
            Assert.IsFalse(_world.EntityManager.Entities.Contains(characterToDestroy));
        }

        [TestMethod]
        public void CharacterManager_WhenAHealthStatIsReducedToZero_RaisesTheCharacterDestroyedEventCorrectly()
        {
            Entity entityToDestroy = _level.CharacterManager.Characters[0];

            _level.CharacterManager.CharacterDestroyed += this.SetEventTriggeredFlag;

            Stat stat = entityToDestroy.GetComponent<StatManager>().GetStat("Health");
            stat.Value -= 100;

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(entityToDestroy, ((EntityEventArgs)_eventArgs).Entity);
        }

    }
}