using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.AI.Tactician;
using Moq;
using TurnItUp.Locations;
using Entropy;
using Tests.Factories;
using TurnItUp.Components;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class MoveToGoalTests
    {
        private Entity _entity;
        private Node _destination;
        private Mock<Level> _mockLevel;

        [TestInitialize]
        public void Initialize()
        {
            _mockLevel = new Mock<Level>();
            _entity = EntropyFactory.BuildEntity();
            _entity.AddComponent(new OnLevel(_mockLevel.Object));
            _destination = new Node(_mockLevel.Object, 0, 0);
        }

        [TestMethod]
        public void MoveToGoal_Construction_IsSuccessful()
        {
            MoveToGoal goal = new MoveToGoal(_entity, _destination);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_destination, goal.Destination);
        }

        [TestMethod]
        public void MoveToGoal_Processing_IsSuccessful()
        {
            MoveToGoal goal = new MoveToGoal(_entity, _destination);

            goal.Process();
            _mockLevel.Verify(b => b.MoveCharacterTo(_entity, new Position(_destination.Position.X, _destination.Position.Y)));
        }
    }
}
