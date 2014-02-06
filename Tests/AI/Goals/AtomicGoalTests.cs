using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Goals;
using Moq;

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
        public void AtomicGoal_WhenProcessing_ActivatesIfInactive()
        {
            Mock<AtomicGoal> atomicGoalMock = new Mock<AtomicGoal>() { CallBase = true };

            atomicGoalMock.Object.Process();
            atomicGoalMock.Verify(ag => ag.Activate());
            Assert.AreEqual(GoalStatus.Active, atomicGoalMock.Object.Status);
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
