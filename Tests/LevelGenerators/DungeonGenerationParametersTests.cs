using System;
using NUnit.Framework;
using Turnable.Api;
using Turnable.LevelGenerators;

namespace Tests.LevelGenerators
{
    [TestFixture]
    public class DungeonGenerationParametersTests
    {
        [Test]
        public void DungeonGenerationParameters_ImplementsTheISetupParametersInterface()
        {
            Assert.That(new DungeonGenerationParameters(), Is.InstanceOf<ISetupParameters>());
        }
    }
}