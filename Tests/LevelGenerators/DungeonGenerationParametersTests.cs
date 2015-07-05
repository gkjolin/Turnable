using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Api;
using Turnable.LevelGenerators;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class DungeonGenerationParametersTests
    {
        [TestMethod]
        public void DungeonGenerationParameters_ImplementsTheISetupParametersInterface()
        {
            Assert.IsInstanceOfType(new DungeonGenerationParameters(), typeof(ISetupParameters));
        }
    }
}