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
    public class ConnectionTests
    {
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void Connection_Construction_IsSuccessful()
        {
            Node startNode = new Node(_level, 10, 10);
            Node endNode = new Node(_level, 10, 10);

            Connection connection = new Connection(startNode, endNode);

            Assert.AreEqual(connection.StartNode, startNode);
            Assert.AreEqual(connection.EndNode, endNode);
        }
    }
}
