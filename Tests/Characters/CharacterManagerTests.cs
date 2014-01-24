using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;

namespace Tests.Characters
{
    [TestClass]
    public class CharacterManagerTests
    {
        private Board _board;
        private CharacterManager _characterManager;
        private int _currentX;
        private int _currentY;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
            _characterManager = new CharacterManager(_board.Map.Tilesets["Characters"], _board.Map.Layers["Characters"]);
            _currentX = _characterManager.PlayerCharacter.Position.X;
            _currentY = _characterManager.PlayerCharacter.Position.Y;
        }

        [TestMethod]
        public void CharacterManager_Construction_IsSuccessful()
        {
            CharacterManager characterManager = new CharacterManager(_board.Map.Tilesets["Characters"], _board.Map.Layers["Characters"]);

            Assert.IsNotNull(characterManager.Characters);
            Assert.AreEqual(9, characterManager.Characters.Count);
            Assert.IsNotNull(characterManager.PlayerCharacter);
            Assert.IsTrue(characterManager.PlayerCharacter.IsPlayer);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerLeft_MovesPlayerCorrectly()
        {
            _characterManager.MovePlayer(Direction.Left);

            Assert.AreEqual(_currentX - 1, _characterManager.PlayerCharacter.Position.X);
            Assert.AreEqual(_currentY, _characterManager.PlayerCharacter.Position.Y);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerRight_MovesPlayerCorrectly()
        {
            _characterManager.MovePlayer(Direction.Right);

            Assert.AreEqual(_currentX + 1, _characterManager.PlayerCharacter.Position.X);
            Assert.AreEqual(_currentY, _characterManager.PlayerCharacter.Position.Y);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerDown_MovesPlayerCorrectly()
        {
            _characterManager.MovePlayer(Direction.Down);

            Assert.AreEqual(_currentX, _characterManager.PlayerCharacter.Position.X);
            Assert.AreEqual(_currentY + 1, _characterManager.PlayerCharacter.Position.Y);
        }

        [TestMethod]
        public void CharacterManager_MovingPlayerUp_MovesPlayerCorrectly()
        {
            _characterManager.MovePlayer(Direction.Up);

            Assert.AreEqual(_currentX, _characterManager.PlayerCharacter.Position.X);
            Assert.AreEqual(_currentY - 1, _characterManager.PlayerCharacter.Position.Y);
        }
    }
}
