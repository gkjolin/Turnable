using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Goals;
using Tests.SupportingClasses;
using Moq;
using Entropy;

namespace Tests.AI.Goals
{
    [TestClass]
    public class CompositeGoalTests
    {
        [TestMethod]
        public void CompositeGoal_Construction_IsSuccessful()
        {
            CompositeGoal compositeGoal = new CompositeGoal();

            Assert.IsNotNull(compositeGoal.Subgoals);
            Assert.AreEqual(GoalStatus.Inactive, compositeGoal.Status);
        }

        [TestMethod]
        public void CompositeGoal_IsAnEntropyComponent()
        {
            CompositeGoal compositeGoal = new CompositeGoal();

            Assert.IsInstanceOfType(compositeGoal, typeof(IComponent));
        }

        [TestMethod]
        public void CompositeGoal_WhenProcessing_ActivatesIfInactive()
        {
            Mock<CompositeGoal> compositeGoalMock = new Mock<CompositeGoal>() { CallBase = true };

            compositeGoalMock.Object.Process();
            compositeGoalMock.Verify(ag => ag.Activate());
            Assert.AreEqual(GoalStatus.Active, compositeGoalMock.Object.Status);
        }

        [TestMethod]
        public void CompositeGoal_AddingASubgoal_AddsSubgoalToFrontOfSubgoals()
        {
            CompositeGoal compositeGoal = new CompositeGoal();
            Goal subgoal = new AtomicGoalType1();

            compositeGoal.AddSubgoal(subgoal);
            Assert.AreEqual(subgoal, compositeGoal.Subgoals[0]);
        }
    }
}
