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
        private bool _eventTriggeredFlag;

        [TestInitialize]
        public void Initialize()
        {
            _eventTriggeredFlag = false;
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
        public void CharacterManager_MovingCharacterToAPosition_MovesCharacterCorrectly()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(newPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPositionOccupiedByAnObstacle_ReturnsHitObstacleMoveResultAndPositionOfObstacleToIndicateFailure()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y - 1);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            // Make sure that character was NOT moved
            Assert.AreEqual(MoveResult.HitObstacle, moveResult.Element1);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(new Position(4, 0), moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterToAPositionOccupiedByAnotherCharacter_ReturnsHitCharacterMoveResultAndPositionOfOtherCharacterToIndicateFailure()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X + 1, currentPosition.Y);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MoveCharacterTo(character, newPosition);

            // Make sure that character was NOT moved
            Assert.AreEqual(MoveResult.HitCharacter, moveResult.Element1);
            Assert.AreEqual(currentPosition, character.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(new Position(6, 1), moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingCharacterByDirection_DelegatesToMoveCharacterTo()
        {
            Entity character = _characterManager.Characters[0];
            Position currentPosition = character.GetComponent<Position>();
            Mock<CharacterManager> characterManagerMock = new Mock<CharacterManager>() { CallBase = true };
            characterManagerMock.Setup(cm => cm.MoveCharacterTo(It.IsAny<Entity>(), It.IsAny<Position>()));

            characterManagerMock.Object.MoveCharacter(character, Direction.Left);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X - 1, currentPosition.Y)));

            characterManagerMock.Object.MoveCharacter(character, Direction.Right);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X + 1, currentPosition.Y)));

            characterManagerMock.Object.MoveCharacter(character, Direction.Up);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X, currentPosition.Y  - 1)));

            characterManagerMock.Object.MoveCharacter(character, Direction.Down);
            characterManagerMock.Verify(cm => cm.MoveCharacterTo(character, new Position(currentPosition.X, currentPosition.Y + 1)));
        }

        [TestMethod]
        public void CharacterManager_TryingToMovePlayer_DelegatesToMoveCharacter()
        {
            Entity player = _characterManager.Player;
            Mock<CharacterManager> characterManagerMock = new Mock<CharacterManager>() { CallBase = true };
            characterManagerMock.Setup(cm => cm.Player).Returns(player);
            characterManagerMock.Setup(cm => cm.MoveCharacter(It.IsAny<Entity>(), It.IsAny<Direction>()));

            characterManagerMock.Object.MovePlayer(Direction.Down);

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

        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
        }

        [TestMethod]
        public void CharacterManager_EndingTurn_RaisesATurnEndedEvent()
        {
            // TODO: How do I check that the EntityEventArgs are correctly set?
            _characterManager.TurnEnded += this.SetEventTriggeredFlag;
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
    }
}
