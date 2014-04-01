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

namespace Tests.Locations
{
    [TestClass]
    public class AreaTests
    {
        [TestMethod]
        public void Area_Construction_IsSuccessful()
        {
            Area area = new Area();

            Assert.IsNotNull(area.Levels);
            // Assert.IsNotNull(area.Connections);
        }
    }
}
