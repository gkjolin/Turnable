using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.AI.Tactician;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class FollowPathGoalTests
    {
        private List<Node> _path;

        [TestInitialize]
        public void Initialize()
        {
            _path = new List<Node>();
        }

        [TestMethod]
        public void FollowPathGoal_Construction_IsSuccessful()
        {
            List<Node> path = new List<Node>();

            FollowPathGoal goal = new FollowPathGoal(path);

            Assert.AreEqual(path, goal.Path);
        }

        [TestMethod]
        public void FollowPathGoal_WhenActivated_CreatesAMoveToAtomicGoal()
        {
            _path.Add(new Node(0, 0));
            _path.Add(new Node(0, 1));

            FollowPathGoal goal = new FollowPathGoal(_path);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(MoveToGoal));
            Assert.AreEqual(goal.Subgoals[0]


        }
    }
}
