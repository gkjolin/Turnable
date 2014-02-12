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
        private Mock<Board> _mockBoard;

        [TestInitialize]
        public void Initialize()
        {
            _mockBoard = new Mock<Board>();
            _entity = EntropyFactory.BuildEntity();
            _entity.AddComponent(new OnBoard(_mockBoard.Object));
            _destination = new Node(_mockBoard.Object, 0, 0);
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
            _mockBoard.Verify(b => b.MoveCharacterTo(_entity, new Position(_destination.Position.X, _destination.Position.Y)));
        }
    }
}
