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
        private CompositeGoal _compositeGoal;
        private Mock<AtomicGoal>[] _atomicGoalMocks;

        [TestInitialize]
        public void Initialize()
        {
            _compositeGoal = new CompositeGoal();
            _atomicGoalMocks = new Mock<AtomicGoal>[4];

            _atomicGoalMocks[0] = new Mock<AtomicGoal>();
            _atomicGoalMocks[1] = new Mock<AtomicGoal>();
            _atomicGoalMocks[2] = new Mock<AtomicGoal>();
            _atomicGoalMocks[3] = new Mock<AtomicGoal>();
        }

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
            Assert.IsInstanceOfType(_compositeGoal, typeof(IComponent));
        }

        [TestMethod]
        public void CompositeGoal_Activating_SetsStatusToActive()
        {
            Mock<CompositeGoal> compositeGoalMock = new Mock<CompositeGoal>() { CallBase = true };

            compositeGoalMock.Object.Activate();
            Assert.AreEqual(GoalStatus.Active, compositeGoalMock.Object.Status);
        }

        [TestMethod]
        public void CompositeGoal_Processing_ActivatesIfInactive()
        {
            Mock<CompositeGoal> compositeGoalMock = new Mock<CompositeGoal>() { CallBase = true };

            compositeGoalMock.Object.Process();
            compositeGoalMock.Verify(cg => cg.Activate());
            Assert.AreEqual(GoalStatus.Completed, compositeGoalMock.Object.Status);
        }

        [TestMethod]
        public void CompositeGoal_Processing_TerminatesAllCompletedAndFailedSubgoals()
        {
            _atomicGoalMocks[0].Setup(ag => ag.Status).Returns(GoalStatus.Completed);
            _atomicGoalMocks[1].Setup(ag => ag.Status).Returns(GoalStatus.Failed);
            _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Active);

            _compositeGoal.AddSubgoal(_atomicGoalMocks[2].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[1].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[0].Object);

            _compositeGoal.Process();
            Assert.AreEqual(GoalStatus.Active, _compositeGoal.Status);

            // Every completed and failed subgoal should be terminated
            _atomicGoalMocks[0].Verify(ag => ag.Terminate());
            _atomicGoalMocks[1].Verify(ag => ag.Terminate());
            // Every completed and failed subgoal should also be removed
            Assert.AreEqual(1, _compositeGoal.Subgoals.Count);
            Assert.AreEqual(_atomicGoalMocks[2].Object, _compositeGoal.Subgoals[0]);
        }

        [TestMethod]
        public void CompositeGoal_Processing_ProcessesTheFrontmostSubgoalRemaining()
        {
            _atomicGoalMocks[0].Setup(ag => ag.Status).Returns(GoalStatus.Completed);
            _atomicGoalMocks[1].Setup(ag => ag.Status).Returns(GoalStatus.Failed);
            _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Inactive);
            _atomicGoalMocks[2].Setup(ag => ag.Process()).Callback(() => _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Active));
            _atomicGoalMocks[3].Setup(ag => ag.Status).Returns(GoalStatus.Inactive);

            _compositeGoal.AddSubgoal(_atomicGoalMocks[3].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[2].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[1].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[0].Object);

            _compositeGoal.Process();
            Assert.AreEqual(GoalStatus.Active, _compositeGoal.Status);

            _atomicGoalMocks[2].Verify(ag => ag.Process());
        }

        [TestMethod]
        public void CompositeGoal_Processing_SetsStatusToActiveIfTheFrontmostSubgoalIsCompletedButFurtherSubgoalsRemain()
        {
            _atomicGoalMocks[0].Setup(ag => ag.Status).Returns(GoalStatus.Completed);
            _atomicGoalMocks[1].Setup(ag => ag.Status).Returns(GoalStatus.Failed);
            _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Inactive);
            _atomicGoalMocks[2].Setup(ag => ag.Process()).Callback(() => _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Completed));
            _atomicGoalMocks[3].Setup(ag => ag.Status).Returns(GoalStatus.Inactive);

            _compositeGoal.AddSubgoal(_atomicGoalMocks[3].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[2].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[1].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[0].Object);

            _compositeGoal.Process();
            Assert.AreEqual(GoalStatus.Active, _compositeGoal.Status);

            _atomicGoalMocks[2].Verify(ag => ag.Process());
        }

        [TestMethod]
        public void CompositeGoal_Processing_SetsStatusToFailedIfTheFrontmostSubgoalFailsEvenIfFurtherSubgoalsRemain()
        {
            _atomicGoalMocks[0].Setup(ag => ag.Status).Returns(GoalStatus.Completed);
            _atomicGoalMocks[1].Setup(ag => ag.Status).Returns(GoalStatus.Failed);
            _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Inactive);
            _atomicGoalMocks[2].Setup(ag => ag.Process()).Callback(() => _atomicGoalMocks[2].Setup(ag => ag.Status).Returns(GoalStatus.Failed));
            _atomicGoalMocks[3].Setup(ag => ag.Status).Returns(GoalStatus.Inactive);

            _compositeGoal.AddSubgoal(_atomicGoalMocks[3].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[2].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[1].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[0].Object);

            _compositeGoal.Process();
            Assert.AreEqual(GoalStatus.Failed, _compositeGoal.Status);

            _atomicGoalMocks[2].Verify(ag => ag.Process());
        }

        [TestMethod]
        public void CompositeGoal_Processing_HasAStatusOfCompletedIfNoSubgoalsRemain()
        {
            _compositeGoal.Process();
            Assert.AreEqual(GoalStatus.Completed, _compositeGoal.Status);
        }

        [TestMethod]
        public void CompositeGoal_AddingASubgoal_AddsSubgoalToFrontOfSubgoals()
        {
            Goal subgoal = new AtomicGoalType1();

            _compositeGoal.AddSubgoal(subgoal);
            Assert.AreEqual(subgoal, _compositeGoal.Subgoals[0]);
        }

        [TestMethod]
        public void CompositeGoal_RemovingAllSubgoals_TerminatesAndRemovesAllSubgoals()
        {
            _compositeGoal.AddSubgoal(_atomicGoalMocks[3].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[2].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[1].Object);
            _compositeGoal.AddSubgoal(_atomicGoalMocks[0].Object);

            _compositeGoal.RemoveAllSubgoals();

            _atomicGoalMocks[0].Verify(ag => ag.Terminate());
            _atomicGoalMocks[1].Verify(ag => ag.Terminate());
            _atomicGoalMocks[2].Verify(ag => ag.Terminate());
            _atomicGoalMocks[3].Verify(ag => ag.Terminate());

            Assert.AreEqual(0, _compositeGoal.Subgoals.Count);
        }
    }
}
