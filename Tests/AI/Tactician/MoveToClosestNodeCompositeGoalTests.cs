using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Tactician;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class MoveToClosestNodeCompositeGoalTests
    {
        [TestMethod]
        public void MoveToClosestNodeGoal_IsComposedOfTheCorrectAtomicGoals()
        {
            MoveToClosestNodeInListOfNodesGoal goal = new MoveToClosestNodeInListOfNodesGoal();

            Assert.AreEqual(3, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(FindClosestNodeInListOfNodesGoal));
            Assert.IsInstanceOfType(goal.Subgoals[1], typeof(FindPathBetweenNodesGoal));
            Assert.IsInstanceOfType(goal.Subgoals[2], typeof(MoveAlongPathGoal));
        }
    }
}
