using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.AI.Tactician;
using Moq;
using TurnItUp.Locations;
using Entropy;
using Tests.Factories;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class MoveToGoalTests
    {
        private Entity _entity;
        private Node _destination;

        [TestInitialize]
        public void Initialize()
        {
            _entity = EntropyFactory.BuildEntity();
            _destination = new Node(0, 0);
        }

        [TestMethod]
        public void MoveToGoal_Construction_IsSuccessful()
        {
            MoveToGoal goal = new MoveToGoal(_entity, _destination);

            Assert.AreEqual(_entity, goal.Entity);
            Assert.AreEqual(_destination, goal.Destination);
        }
    }
}
