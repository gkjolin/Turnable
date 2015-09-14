using System;
using Tests.Factories;
using System.Collections.Generic;
using Turnable.Characters;
using Turnable.Api;
using Entropy.Core;
using Turnable.Components;
using Turnable.Locations;
using NUnit.Framework;

namespace Tests.Characters
{
    [TestFixture]
    public class CharacterManagerTests
    {
        private ILevel _level;
        private ICharacterManager _characterManager;
        private bool _characterMovedEventTriggeredFlag;
        private CharacterMovedEventArgs _characterMovedEventArgs;

        [SetUp]
        public void SetUp()
        {
            _level = LocationsFactory.BuildLevel();
            _characterManager = new CharacterManager(_level);
            _characterManager.SetUpPcs();
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            CharacterManager characterManager = new CharacterManager(_level);

            Assert.That(_level, Is.EqualTo(characterManager.Level));
            Assert.That(characterManager.Pcs, Is.Not.Null);
            Assert.That(characterManager.Pcs, Is.InstanceOf<List<Entity>>());
            Assert.That(characterManager.Npcs, Is.Not.Null);
            Assert.That(characterManager.Npcs, Is.InstanceOf<List<Entity>>());
        }

        [Test]
        public void SetUpPcs_InitializesTheLocationOfAllPcs()
        {
            CharacterManager characterManager = new CharacterManager(_level);
            characterManager.SetUpPcs();

            Assert.That(characterManager.Pcs, Is.Not.Null);
            Assert.That(characterManager.Pcs.Count, Is.EqualTo(3));
            Assert.That(characterManager.Pcs[0].Get<Position>(), Is.EqualTo(new Position(13, 10)));
            Assert.That(characterManager.Pcs[1].Get<Position>(), Is.EqualTo(new Position(13, 8)));
            Assert.That(characterManager.Pcs[2].Get<Position>(), Is.EqualTo(new Position(6, 1)));
        }

        //[Test]
        //public void CharacterManager_SettingUpNpcs_IsSuccessful()
        //{
        //    // TODO: Check that the position of the characters is set correctly
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpNpcs();

        //    Assert.IsNotNull(characterManager.Characters);
        //    Assert.That(8, characterManager.Characters.Count);

        //    // Are all Characters set up with a Model?
        //    foreach (Entity character in characterManager.Characters)
        //    {
        //        Assert.IsNotNull(character.GetComponent<Model>());
        //        // TODO: Test that the models are set up correctly for each character
        //        Assert.IsTrue(new List<String> { "Skeleton", "Skeleton Archer", "Pharaoh" }.Contains(character.GetComponent<Model>().Name));
        //    }

        //    // Is a TurnQueue setup with the Player taking the first turn?
        //    Assert.IsNotNull(characterManager.TurnQueue);
        //    Assert.That(8, characterManager.TurnQueue.Count);

        //    foreach (Entity character in characterManager.Characters)
        //    {
        //        Assert.That(_levelWithoutCharacters, character.GetComponent<OnLevel>().Level);
        //    }
        //}

        //[Test]
        //public void CharacterManager_SettingUpNpcs_IgnoresCharacterTilesThatHaveNoModelPropertySet()
        //{
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpNpcs();

        //    Assert.IsNotNull(characterManager.Characters);
        //    Assert.That(8, characterManager.Characters.Count);

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

        //[Test]
        //[ExpectedException(typeof(ArgumentException))]
        //public void CharacterManager_SettingUpWithAnInvalidStartingPosition_Fails()
        //{
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpPc("Knight M", -1, -1);
        //}

        //[Test]
        //[ExpectedException(typeof(ArgumentException))]
        //public void CharacterManager_SettingUpWithAnInvalidPlayerModel_Fails()
        //{
        //    CharacterManager characterManager = new CharacterManager(_levelWithoutCharacters);
        //    characterManager.SetUpPc("Missing Model", 1, 1);
        //}

        private void AssertSuccessfulMovement(Movement movement, Entity character, Position startingPosition, Position newPosition)
        {
            Assert.That(movement.Status, Is.EqualTo(MovementStatus.Success));
            Assert.That(character.Get<Position>(), Is.EqualTo(newPosition));
            Assert.That(movement.Path.Count, Is.EqualTo(2));
            Assert.That(movement.Path[0], Is.EqualTo(startingPosition));
            Assert.That(movement.Path[1], Is.EqualTo(newPosition));

            // Check to see if the tile in the map was moved as well
            Assert.That(_level.SpecialLayers[SpecialLayer.Character].IsTileAt(startingPosition), Is.False);
            Assert.That(_level.SpecialLayers[SpecialLayer.Character].IsTileAt(newPosition), Is.True);
        }

        [Test]
        public void MoveCharacter_GivenACharacterAndAPosition_MovesTheCharacterToTheNewPosition()
        {
            Entity character = _characterManager.Pcs[0];
            Position currentPosition = character.Get<Position>();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y);

            Movement movement = _characterManager.MoveCharacter(character, newPosition);

            AssertSuccessfulMovement(movement, character, currentPosition, newPosition);
        }

        [Test]
        public void MoveCharacter_GivenACharacterAndAPositionOccupiedByAnObstacle_ReturnsHitObstacleMoveResultAndPositionOfObstacle()
        {
            Entity character = _characterManager.Pcs[2];
            Position currentPosition = character.Get<Position>();
            Position newPosition = new Position(currentPosition.X, currentPosition.Y - 1);

            Movement movement = _characterManager.MoveCharacter(character, newPosition);

            // Make sure that character was NOT moved
            Assert.That(movement.Status, Is.EqualTo(MovementStatus.HitObstacle));
            Assert.That(character.Get<Position>(), Is.EqualTo(currentPosition));
            Assert.That(movement.Path.Count, Is.EqualTo(2));
            Assert.That(movement.Path[0], Is.EqualTo(currentPosition));
            Assert.That(movement.Path[1], Is.EqualTo(new Position(6, 0)));

            // Check to see if the tile in the map was NOT moved
            Assert.That(_level.SpecialLayers[SpecialLayer.Character].IsTileAt(currentPosition), Is.True);
            Assert.That(_level.SpecialLayers[SpecialLayer.Character].IsTileAt(newPosition), Is.False);
        }

        //[Test]
        //public void CharacterManager_MovingCharacterToAPositionOccupiedByAnotherCharacter_ReturnsHitCharacterMoveResultAndPositionOfOtherCharacterToIndicateFailure()
        //{
        //    Entity character = _characterManager.Characters[0];
        //    Position currentPosition = character.GetComponent<Position>().DeepClone();
        //    Position newPosition = new Position(currentPosition.X + 1, currentPosition.Y);

        //    MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

        //    // Make sure that character was NOT moved
        //    Assert.That(MoveResultStatus.HitCharacter, moveResult.Status);
        //    Assert.That(currentPosition, character.GetComponent<Position>());
        //    Assert.That(2, moveResult.Path.Count);
        //    Assert.That(currentPosition, moveResult.Path[0]);
        //    Assert.That(new Position(6, 14), moveResult.Path[1]);

        //    // Check to see if the tile in the map was NOT moved
        //    Assert.IsTrue(_level.Map.Layers["Characters"].IsTileAt(currentPosition));
        //}

        //[Test]
        //public void CharacterManager_MovingCharacterToAPositionOutOfBoundsOfTheMap_ReturnsOutOfBoundsMoveResultAndPositionOfOutOfBoundsLocationToIndicateFailure()
        //{
        //    Entity character = _characterManager.Characters[0];
        //    Position currentPosition = character.GetComponent<Position>().DeepClone();
        //    Position newPosition = new Position(-1, -1);

        //    MoveResult moveResult = _characterManager.MoveCharacterTo(character, newPosition);

        //    // Make sure that character was NOT moved
        //    Assert.That(MoveResultStatus.OutOfBounds, moveResult.Status);
        //    Assert.That(currentPosition, character.GetComponent<Position>());
        //    Assert.That(2, moveResult.Path.Count);
        //    Assert.That(currentPosition, moveResult.Path[0]);
        //    Assert.That(new Position(-1, -1), moveResult.Path[1]);

        //    // Check to see if the tile in the map was NOT moved
        //    Assert.IsTrue(_level.Map.Layers["Characters"].IsTileAt(currentPosition));
        //}

        [Test]
        public void MoveCharacter_GivenACharacterAndADirection_MovesTheCharacterOneStepInTheGivenDirection()
        {
            Entity character = _characterManager.Pcs[2];

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

        [Test]
        public void MoveCharacter_RaisesACharacterMovedEvent()
        {
            _characterManager.CharacterMoved += this.SetCharacterMovedEventTriggeredFlag;

            _characterManager.MoveCharacter(_characterManager.Pcs[0], new Position(1, 1));

            Assert.That(_characterMovedEventTriggeredFlag, Is.True);
            Assert.That(_characterMovedEventArgs.Character, Is.EqualTo(_characterManager.Pcs[0]));
            Assert.That(_characterMovedEventArgs.Movement, Is.Not.Null);
        }

        //[Test]
        //public void CharacterManager_CanDetermineIfThereIsACharacterAtALocation()
        //{
        //    Assert.IsTrue(_characterManager.IsCharacterAt(_characterManager.Characters[0].GetComponent<Position>().X, _characterManager.Characters[0].GetComponent<Position>().Y));
        //}

        //[Test]
        //public void CharacterManager_EndingTurn_MovesTheCurrentCharacterToTheEndOfTheTurnQueue()
        //{
        //    Entity firstCharacter = _characterManager.TurnQueue[0];
        //    Entity secondCharacter = _characterManager.TurnQueue[1];

        //    _characterManager.EndTurn();

        //    Assert.That(secondCharacter, _characterManager.TurnQueue[0]);
        //    Assert.That(firstCharacter, _characterManager.TurnQueue[_characterManager.TurnQueue.Count - 1]);
        //}

        //[Test]
        //public void CharacterManager_EndingTurn_RaisesACharacterTurnEndedEvent()
        //{
        //    // TODO: How do I check that the EntityEventArgs are correctly set?
        //    _characterManager.CharacterTurnEnded += this.SetEventTriggeredFlag;
        //    _characterManager.EndTurn();
        //    Assert.IsTrue(_eventTriggeredFlag);
        //}

        //[Test]
        //public void CharacterManager_DestroyingACharacter_RaisesACharacterDestroyedEvent()
        //{
        //    Entity characterToDestroy = _characterManager.Characters[0];

        //    // TODO: How do I check that the EntityEventArgs are correctly set?
        //    _characterManager.CharacterDestroyed += this.SetEventTriggeredFlag;
        //    _characterManager.DestroyCharacter(characterToDestroy);
        //    Assert.IsTrue(_eventTriggeredFlag);
        //    Assert.That(characterToDestroy, ((EntityEventArgs)_eventArgs).Entity);
        //}

        //[Test]
        //public void CharacterManager_DestroyingACharacter_RemovesItFromCharactersAndTheTurnQueue()
        //{
        //    Entity characterToDestroy = _characterManager.Characters[0];

        //    _characterManager.DestroyCharacter(characterToDestroy);

        //    Assert.That(8, _characterManager.Characters.Count);
        //    Assert.IsFalse(_characterManager.Characters.Contains(characterToDestroy));
        //    Assert.IsFalse(_characterManager.TurnQueue.Contains(characterToDestroy));
        //    Assert.IsFalse(_world.EntityManager.Entities.Contains(characterToDestroy));
        //}

        //[Test]
        //public void CharacterManager_WhenAHealthStatIsReducedToZero_RaisesTheCharacterDestroyedEventCorrectly()
        //{
        //    Entity entityToDestroy = _level.CharacterManager.Characters[0];

        //    _level.CharacterManager.CharacterDestroyed += this.SetEventTriggeredFlag;

        //    Stat stat = entityToDestroy.GetComponent<StatManager>().GetStat("Health");
        //    stat.Value -= 100;

        //    Assert.IsTrue(_eventTriggeredFlag);
        //    Assert.That(entityToDestroy, ((EntityEventArgs)_eventArgs).Entity);
        //}

        private void SetCharacterMovedEventTriggeredFlag(object sender, CharacterMovedEventArgs e)
        {
            _characterMovedEventTriggeredFlag = true;
            _characterMovedEventArgs = (CharacterMovedEventArgs)e;
        }
    }
}