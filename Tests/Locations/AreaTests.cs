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
    public class AreaTests
    {
        private Area _area;
        private Level _level;

        [TestInitialize]
        public void Initialize()
        {
            _area = new Area();
            _level = LocationsFactory.BuildLevel("../../Fixtures/HubExample.tmx");
        }

        [TestMethod]
        public void Area_Construction_IsSuccessful()
        {
            Area area = new Area();

            Assert.IsNotNull(area.Levels);
            Assert.IsNotNull(area.Connections);
        }

        [TestMethod]
        public void Area_SettingTheHubLevel_CorrectlyCreatesAllConnections()
        {
            _area.SetHubLevel(_level);

            Assert.AreEqual(4, _area.Connections.Count);

            foreach (Connection connection in _area.Connections)
            {
                Assert.AreEqual(_level, connection.StartNode.Level);
                Assert.IsNull(connection.EndNode);
            }
        }
    }
}
