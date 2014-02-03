using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;

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
            _characterManager = new CharacterManager(_world, _board);
            _currentX = _characterManager.Player.GetComponent<Position>().X;
            _currentY = _characterManager.Player.GetComponent<Position>().Y;
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
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerLeft_MovesPlayerCorrectly()
        {
            Position currentPosition = _characterManager.Player.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MovePlayer(Direction.Left);

            Assert.AreEqual(newPosition, _characterManager.Player.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerRight_MovesPlayerCorrectly()
        {
            Position currentPosition = _characterManager.Player.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X + 1, currentPosition.Y);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MovePlayer(Direction.Right);

            Assert.AreEqual(newPosition, _characterManager.Player.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerDown_MovesPlayerCorrectly()
        {
            // In the test map, the player has an obstacle right below, so we need to move the player up before we can test moving down.
            _characterManager.MovePlayer(Direction.Up);

            Position currentPosition = _characterManager.Player.GetComponent<Position>().DeepClone(); 
            Position newPosition = new Position(currentPosition.X, currentPosition.Y + 1);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MovePlayer(Direction.Down);

            Assert.AreEqual(newPosition, _characterManager.Player.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerUp_MovesPlayerCorrectly()
        {
            Position currentPosition = _characterManager.Player.GetComponent<Position>().DeepClone();
            Position newPosition = new Position(currentPosition.X, currentPosition.Y - 1);

            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MovePlayer(Direction.Up);

            Assert.AreEqual(newPosition, _characterManager.Player.GetComponent<Position>());
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(MoveResult.Success, moveResult.Element1);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(newPosition, moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_TryingToMovePlayerIntoAnObstacle_ReturnsHitObstacleMoveResultAndPositionOfObstacleToIndicateMoveWasUnsuccessful()
        {
            _characterManager.MovePlayer(Direction.Up);
            _characterManager.MovePlayer(Direction.Up);
            _characterManager.MovePlayer(Direction.Up);

            Position currentPosition = _characterManager.Player.GetComponent<Position>().DeepClone();
            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MovePlayer(Direction.Up);

            // Make sure that player was NOT moved
            Assert.AreEqual(currentPosition, _characterManager.Player.GetComponent<Position>());
            Assert.AreEqual(MoveResult.HitObstacle, moveResult.Element1);
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(new Position(7, 10), moveResult.Element2[1]);
        }

        [TestMethod]
        public void CharacterManager_TryingToMovePlayerIntoAnotherCharacter_ReturnsHitCharacterMoveResultAndPositionOfCharacterToIndicateMoveWasUnsuccessful()
        {
            _characterManager.MovePlayer(Direction.Up);
            _characterManager.MovePlayer(Direction.Up);
            _characterManager.MovePlayer(Direction.Up);
            _characterManager.MovePlayer(Direction.Right);
            _characterManager.MovePlayer(Direction.Up);
            _characterManager.MovePlayer(Direction.Up);

            Position currentPosition = _characterManager.Player.GetComponent<Position>().DeepClone();
            Tuple<MoveResult, List<Position>> moveResult = _characterManager.MovePlayer(Direction.Up);

            // Make sure that player was NOT moved
            Assert.AreEqual(currentPosition, _characterManager.Player.GetComponent<Position>());
            Assert.AreEqual(MoveResult.HitCharacter, moveResult.Element1);
            Assert.AreEqual(2, moveResult.Element2.Count);
            Assert.AreEqual(currentPosition, moveResult.Element2[0]);
            Assert.AreEqual(new Position(8, 8), moveResult.Element2[1]);
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
