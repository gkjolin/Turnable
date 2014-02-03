using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Goals;
using Tests.SupportingClasses;

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
