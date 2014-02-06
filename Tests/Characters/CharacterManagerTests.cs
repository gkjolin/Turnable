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

namespace Tests.Characters
{
    [TestClass]
    public class CharacterManagerTests
    {
        private World _world;
        private Board _board;
        private CharacterManager _characterManager;
        private int _currentX;
        private int _currentY;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            _board = LocationsFactory.BuildBoard();
            _characterManager = (CharacterManager)_board.CharacterManager;
        }

        [TestMethod]
        public void CharacterManager_Construction_IsSuccessful()
        {
            // TODO: Check that the position of the characters is set correctly
            CharacterManager characterManager = new CharacterManager(_world, _board);

            Assert.AreEqual(characterManager.Board, _board);
            Assert.IsNotNull(characterManager.Characters);
            Assert.AreEqual(9, characterManager.Characters.Count);
            Assert.IsNotNull(characterManager.Player);
            Assert.IsNotNull(characterManager.TurnQueue);
            Assert.AreEqual(9, characterManager.TurnQueue.Count);
            Assert.AreEqual(characterManager.Player, characterManager.TurnQueue[0]);

            foreach (Entity character in characterManager.Characters)
            {
                Assert.AreEqual(_board, character.GetComponent<OnBoard>().Board);
            }
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterLeft_MovesCharacterCorrectly()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacter(character, Direction.Left);

            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(newPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterRight_MovesPlayerCorrectly()
        {
            Entity character = _characterManager.Characters[0];

            // In the test map, the character we've selected has another character directly to the right
            // We need to move the character down before we can test moving right
            _characterManager.MoveCharacter(character, Direction.Down);
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X + 1, currentPosition.Y);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacter(character, Direction.Right);

            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(newPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterDown_MovesCharacterCorrectly()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X, currentPosition.Y + 1);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacter(character, Direction.Down);

            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(newPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterUp_MovesCharacterCorrectly()
        {
            Entity character = _characterManager.Characters[0];

            // In the test map, the character we've selected has an obstacle direcly above it
            // We need to move the character down before we can test moving up
            _characterManager.MoveCharacter(character, Direction.Down);
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X, currentPosition.Y - 1);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacter(character, Direction.Up);

            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(newPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_TryingToMoveCharacterIntoAnObstacle_ReturnsHitObstacleMoveResultAndPositionOfObstacleToIndicateMoveWasUnsuccessful()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacter(character, Direction.Up);

            // Make sure that character was NOT moved
            Assert.AreEqual(MoveResult.HitObstacle, moveResult.Element1);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(new Position(5, 1), moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_TryingToMoveCharacterIntoAnotherCharacter_ReturnsHitCharacterMoveResultAndPositionOfCharacterToIndicateMoveWasUnsuccessful()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacter(character, Direction.Right);

            // Make sure that player was NOT moved
            Assert.AreEqual(MoveResult.HitCharacter, moveResult.Element1);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(new Position(5, 1), moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_TryingToMovePlayer_DelegatesToMoveCharacter()
        {
            Entity player = _characterManager.Player;
            Mock<ICharacterManager> characterManagerMock = new Mock<ICharacterManager>() { CallBase = true };
            characterManagerMock.Setup(cm => cm.Player).Returns(player);

            characterManagerMock.MovePlayer(Direction.Down);

            characterManagerMock.Verify(cm => cm.MoveCharacter(player, Direction.Down));
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

    }
}
