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
        [TestMethod]
        public void CharacterManager_Construction_IsSuccessful()
        {
            Board board = LocationsFactory.BuildBoard();

            CharacterManager characterManager = new CharacterManager(board.Map.Tilesets["Characters"], board.Map.Layers["Characters"]);

            Assert.IsNotNull(characterManager.Characters);
            Assert.AreEqual(9, characterManager.Characters.Count);
            Assert.IsNotNull(characterManager.PlayerCharacter);
            Assert.IsTrue(characterManager.PlayerCharacter.IsPlayer);
        }
    }
}
