using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using System.Collections.Generic;
using System.Tuples;
using Entropy;
using Moq;
using Turnable.Characters;
using Turnable.Api;
using Entropy.Core;
using Turnable.Components;
using Turnable.Locations;

namespace Tests.Characters
{
    [TestClass]
    public class CharacterManagerTests
    {
        private ILevel _level;
        private ICharacterManager _characterManager;
        private bool _characterMovedEventTriggeredFlag;
        private CharacterMovedEventArgs _characterMovedEventArgs;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _characterManager = new CharacterManager(_level);
            _characterManager.SetUpPcs();
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            CharacterManager characterManager = new CharacterManager(_level);

            Assert.AreEqual(_level, characterManager.Level);
            Assert.IsNotNull(characterManager.Characters);
            Assert.IsInstanceOfType(characterManager.Characters, typeof(List<Entity>));
        }

        [TestMethod]
        public void SetUpPcs_InitializesThePositionOfAllPcs()
        {
            CharacterManager characterManager = new CharacterManager(_level);
            characterManager.SetUpPcs();

            Assert.IsNotNull(characterManager.Player);
            Assert.AreEqual(new Position(6, 1), characterManager.Player.Get<Position>());
        }

        //private IWorld _world;
        //private ILevel _levelWithoutCharacters;
        //private CharacterManager _characterManager;
        //private bool _eventTriggeredFlag;
        //private EntityEventArgs _eventArgs;

        //[TestMethod]
        //public void CharacterManager_SettingUpNpcs_IsSuccessful()
        //{
        //    // TODO: Check that the position of the characters is set correctly
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpNpcs();

        //    Assert.IsNotNull(characterManager.Characters);
        //    Assert.AreEqual(8, characterManager.Characters.Count);

        //    // Are all Characters set up with a Model?
        //    foreach (Entity character in characterManager.Characters)
        //    {
        //        Assert.IsNotNull(character.GetComponent<Model>());
        //        // TODO: Test that the models are set up correctly for each character
        //        Assert.IsTrue(new List<String> { "Skeleton", "Skeleton Archer", "Pharaoh" }.Contains(character.GetComponent<Model>().Name));
        //    }

        //    // Is a TurnQueue setup with the Player taking the first turn?
        //    Assert.IsNotNull(characterManager.TurnQueue);
        //    Assert.AreEqual(8, characterManager.TurnQueue.Count);

        //    foreach (Entity character in characterManager.Characters)
        //    {
        //        Assert.AreEqual(_levelWithoutCharacters, character.GetComponent<OnLevel>().Level);
        //    }
        //}

        //[TestMethod]
        //public void CharacterManager_SettingUpNpcs_IgnoresCharacterTilesThatHaveNoModelPropertySet()
        //{
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpNpcs();

        //    Assert.IsNotNull(characterManager.Characters);
        //    Assert.AreEqual(8, characterManager.Characters.Count);

        //    // Are all Characters set up with a Model IF they had a model property in the reference tile?
        //    foreach (Entity character in characterManager.Characters)
        //    {
        //        if (character.GetComponent<Model>() != null)
        //        {
        //            // TODO: Test that the models are set up correctly for each character
        //            Assert.IsTrue(new List<String> { "Knight M", "Skeleton", "Skeleton Archer", "Pharaoh" }.Contains(character.GetComponent<Model>().Name));
        //        }
        //    }
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void CharacterManager_SettingUpWithAnInvalidStartingPosition_Fails()
        //{
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpPc("Knight M", -1, -1);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void CharacterManager_SettingUpWithAnInvalidPlayerModel_Fails()
        //{
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpPc("Missing Model", 1, 1);
        //}

        private void AssertSuccessfulMovement(Movement movement, Entity character, Position startingPosition, Position newPosition)
        {
            Assert.AreEqual(MovementStatus.Success, movement.Status);
            Assert.AreEqual(newPosition, character.Get<Position>());
            Assert.AreEqual(2, movement.Path.Count);
            Assert.AreEqual(startingPosition, movement.Path[0]);
            Assert.AreEqual(newPosition, movement.Path[1]);

            // Check to see if the tile in the map was moved as well
            Assert.IsFalse(_level.SpecialLayers[SpecialLayer.Character].Tiles.ContainsKey(new Tuple<int, int>(startingPosition.X, startingPosition.Y)));
            Assert.IsTrue(_level.SpecialLayers[SpecialLayer.Character].Tiles.ContainsKey(new Tuple<int, int>(newPosition.X, newPosition.Y)));
        }

        [TestMethod]
        public void MoveCharacter_GivenACharacterAndAPosition_MovesTheCharacterToTheNewPosition()
        {
            Entity character = _characterManager.Player;
            Position currentPosition = character.Get<Position>();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y);

            Movement movement = _characterManager.MoveCharacter(character, newPosition);

            AssertSuccessfulMovement(movement, character, currentPosition, newPosition);
        }

        [TestMethod]
        public void MoveCharacter_GivenACharacterAndAPositionOccupiedByAnObstacle_ReturnsHitObstacleMoveResultAndPositionOfObstacle()
        {
            Entity character = _characterManager.Player;
            Position currentPosition = character.Get<Position>();
            Position newPosition = new Position(currentPosition.X, currentPosition.Y - 1);

            Movement movement = _characterManager.MoveCharacter(character, newPosition);

            // Make sure that character was NOT moved
            Assert.AreEqual(MovementStatus.HitObstacle, movement.Status);
            Assert.AreEqual(currentPosition, character.Get<Position>());
            Assert.AreEqual(2, movement.Path.Count);
            Assert.AreEqual(currentPosition, movement.Path[0]);
            Assert.AreEqual(new Position(6, 0), movement.Path[1]);

            // Check to see if the tile in the map was NOT moved
            Assert.IsTrue(_level.SpecialLayers[SpecialLayer.Character].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
            Assert.IsFalse(_level.SpecialLayers[SpecialLayer.Character].Tiles.ContainsKey(new Tuple<int, int>(newPosition.X, newPosition.Y)));
        }

        //[TestMethod]
        //public void CharacterManager_MovingCharacterToAPositionOccupiedByAnotherCharacter_ReturnsHitCharacterMoveResultAndPositionOfOtherCharacterToIndicateFailure()
        //{
        //    Entity character = _characterManager.Characters[0];
        //    Position currentPosition = character.GetComponent<Position>().DeepClone();
        //    Position newPosition = new Position(currentPosition.X + 1, currentPosition.Y);

        //    MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

        //    // Make sure that character was NOT moved
        //    Assert.AreEqual(MoveResultStatus.HitCharacter, moveResult.Status);
        //    Assert.AreEqual(currentPosition, character.GetComponent<Position>());
        //    Assert.AreEqual(2, moveResult.Path.Count);
        //    Assert.AreEqual(currentPosition, moveResult.Path[0]);
        //    Assert.AreEqual(new Position(6, 14), moveResult.Path[1]);

        //    // Check to see if the tile in the map was NOT moved
        //    Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
        //}

        //[TestMethod]
        //public void CharacterManager_MovingCharacterToAPositionOutOfBoundsOfTheMap_ReturnsOutOfBoundsMoveResultAndPositionOfOutOfBoundsLocationToIndicateFailure()
        //{
        //    Entity character = _characterManager.Characters[0];
        //    Position currentPosition = character.GetComponent<Position>().DeepClone();
        //    Position newPosition = new Position(-1, -1);

        //    MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

        //    // Make sure that character was NOT moved
        //    Assert.AreEqual(MoveResultStatus.OutOfBounds, moveResult.Status);
        //    Assert.AreEqual(currentPosition, character.GetComponent<Position>());
        //    Assert.AreEqual(2, moveResult.Path.Count);
        //    Assert.AreEqual(currentPosition, moveResult.Path[0]);
        //    Assert.AreEqual(new Position(-1, -1), moveResult.Path[1]);

        //    // Check to see if the tile in the map was NOT moved
        //    Assert.IsTrue(_level.Map.Layers["Characters"].Tiles.ContainsKey(new Tuple<int, int>(currentPosition.X, currentPosition.Y)));
        //}

        [TestMethod]
        public void MoveCharacter_GivenACharacterAndADirection_MovesTheCharacterOneStepInTheGivenDirection()
        {
            // TODO: Use a character here instead of the player again
            Entity character = _characterManager.Player;

            // Move character north twice so that there are no obstacles nearby
            _characterManager.MoveCharacter(character, Direction.North);
            _characterManager.MoveCharacter(character, Direction.North);

            Position currentPosition = character.Get<Position>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Movement movement = _characterManager.MoveCharacter(character, direction);
                AssertSuccessfulMovement(movement, character, currentPosition, currentPosition.NeighboringPosition(direction));
                currentPosition = character.Get<Position>();
            }
        }

        [TestMethod]
        public void MovePlayer_GivenADirectionMovesThePlayerOneStepInTheGivenDirection()
        {
            Entity character = _characterManager.Player;

            // Move player north twice so that there are no obstacles nearby
            _characterManager.MoveCharacter(character, Direction.North);
            _characterManager.MoveCharacter(character, Direction.North);

            Position currentPosition = character.Get<Position>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Movement movement = _characterManager.MovePlayer(direction);
                AssertSuccessfulMovement(movement, character, currentPosition, currentPosition.NeighboringPosition(direction));
                currentPosition = character.Get<Position>();
            }
        }

        [TestMethod]
        public void MoveCharacter_RaisesACharacterMovedEvent()
        {
            _characterManager.CharacterMoved += this.SetCharacterMovedEventTriggeredFlag;

            _characterManager.MoveCharacter(_characterManager.Player, new Position(1, 1));

            Assert.IsTrue(_characterMovedEventTriggeredFlag);
            Assert.AreEqual(_characterMovedEventArgs.Character, _characterManager.Player);
            Assert.IsNotNull(_characterMovedEventArgs.Movement);
        }

        //[TestMethod]
        //public void CharacterManager_CanDetermineIfThereIsACharacterAtALocation()
        //{
        //    Assert.IsTrue(_characterManager.IsCharacterAt(_characterManager.Characters[0].GetComponent<Position>().X, _characterManager.Characters[0].GetComponent<Position>().Y));
        //}

        //[TestMethod]
        //public void CharacterManager_EndingTurn_MovesTheCurrentCharacterToTheEndOfTheTurnQueue()
        //{
        //    Entity firstCharacter = _characterManager.TurnQueue[0];
        //    Entity secondCharacter = _characterManager.TurnQueue[1];

        //    _characterManager.EndTurn();

        //    Assert.AreEqual(secondCharacter, _characterManager.TurnQueue[0]);
        //    Assert.AreEqual(firstCharacter, _characterManager.TurnQueue[_characterManager.TurnQueue.Count - 1]);
        //}

        //[TestMethod]
        //public void CharacterManager_EndingTurn_RaisesACharacterTurnEndedEvent()
        //{
        //    // TODO: How do I check that the EntityEventArgs are correctly set?
        //    _characterManager.CharacterTurnEnded += this.SetEventTriggeredFlag;
        //    _characterManager.EndTurn();
        //    Assert.IsTrue(_eventTriggeredFlag);
        //}

        //[TestMethod]
        //public void CharacterManager_DestroyingACharacter_RaisesACharacterDestroyedEvent()
        //{
        //    Entity characterToDestroy = _characterManager.Characters[0];

        //    // TODO: How do I check that the EntityEventArgs are correctly set?
        //    _characterManager.CharacterDestroyed += this.SetEventTriggeredFlag;
        //    _characterManager.DestroyCharacter(characterToDestroy);
        //    Assert.IsTrue(_eventTriggeredFlag);
        //    Assert.AreEqual(characterToDestroy, ((EntityEventArgs)_eventArgs).Entity);
        //}

        //[TestMethod]
        //public void CharacterManager_DestroyingACharacter_RemovesItFromCharactersAndTheTurnQueue()
        //{
        //    Entity characterToDestroy = _characterManager.Characters[0];

        //    _characterManager.DestroyCharacter(characterToDestroy);

        //    Assert.AreEqual(8, _characterManager.Characters.Count);
        //    Assert.IsFalse(_characterManager.Characters.Contains(characterToDestroy));
        //    Assert.IsFalse(_characterManager.TurnQueue.Contains(characterToDestroy));
        //    Assert.IsFalse(_world.EntityManager.Entities.Contains(characterToDestroy));
        //}

        //[TestMethod]
        //public void CharacterManager_WhenAHealthStatIsReducedToZero_RaisesTheCharacterDestroyedEventCorrectly()
        //{
        //    Entity entityToDestroy = _level.CharacterManager.Characters[0];

        //    _level.CharacterManager.CharacterDestroyed += this.SetEventTriggeredFlag;

        //    Stat stat = entityToDestroy.GetComponent<StatManager>().GetStat("Health");
        //    stat.Value -= 100;

        //    Assert.IsTrue(_eventTriggeredFlag);
        //    Assert.AreEqual(entityToDestroy, ((EntityEventArgs)_eventArgs).Entity);
        //}

        private void SetCharacterMovedEventTriggeredFlag(object sender, CharacterMovedEventArgs e)
        {
            _characterMovedEventTriggeredFlag = true;
            _characterMovedEventArgs = (CharacterMovedEventArgs)e;
        }
    }
}