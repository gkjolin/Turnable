using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Goals;

namespace Tests.AI.Goals
{
    [TestClass]
    public class AtomicGoalTests
    {
        [TestMethod]
        public void AtomicGoal_Construction_IsSuccessful()
        {
            AtomicGoal atomicGoal = new AtomicGoal();

            Assert.AreEqual(GoalStatus.Inactive, atomicGoal.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void AtomicGoal_AddSubgoalMethod_IsNotImplemented()
        {
            AtomicGoal atomicGoal = new AtomicGoal();

            atomicGoal.AddSubgoal(null);
        }
    }
}
