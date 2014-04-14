using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using Moq;
using TurnItUp.Components;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Tmx;
using System.Tuples;
using Tests.Factories;

namespace Tests.Locations
{
    [TestClass]
    public class LevelFactoryTests
    {
        [TestMethod]
        public void LevelFactory_Construction_IsSuccessful()
        {
            LevelFactory levelFactory = new LevelFactory();
        }
    }
}
