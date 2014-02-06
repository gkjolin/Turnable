using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.AI.Tactician;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class MoveToGoalTests
    {
        private Node _destination;

        [TestInitialize]
        public void Initialize()
        {
            _destination = new Node(0, 0);
        }

        [TestMethod]
        public void MoveToGoal_Construction_IsSuccessful()
        {
            MoveToGoal goal = new MoveToGoal(_destination);

            Assert.AreEqual(_destination, goal.Destination);
        }

        [TestMethod]
        public void MoveToGoal_Process_MovesTheOwnerToTheDestination()
        {
            MoveToGoal goal = new MoveToGoal(_destination);

            Assert.AreEqual(_destination, goal.Destination);
        }

    }
}
