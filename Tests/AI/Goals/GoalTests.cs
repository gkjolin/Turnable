using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Goals;

namespace Tests.AI.Goals
{
    [TestClass]
    public class GoalTests
    {
        [TestMethod]
        public void Goal_Construction_IsSuccessful()
        {
            Goal goal = new Goal();

            Assert.IsNotNull(goal.Subgoals);
        }
    }
}
