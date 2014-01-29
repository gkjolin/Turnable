using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Characters;
using Entropy;

namespace Tests.Characters
{
    [TestClass]
    public class CharacterTests
    {
        [TestMethod]
        public void Character_Construction_IsSuccessful()
        {
            Character character = new Character(1, 2);

            Assert.IsNotNull(character.Position);
            Assert.AreEqual(1, character.Position.X);
            Assert.AreEqual(2, character.Position.Y);
            Assert.IsFalse(character.IsPlayer);
        }
    }
}
