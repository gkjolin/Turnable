using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Turnable.Api;
using Tests.Factories;
using Turnable.Characters;
using Turnable.Time;

namespace Tests.Time
{
    [TestFixture]
    public class GameLoopTests
    {
        private ILevel _level;
        private ICharacterManager _characterManager;

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
            GameLoop gameLoop = new GameLoop(_characterManager);

            Assert.That(gameLoop.CharacterManager, Is.SameAs(_characterManager));
        }
    }
}