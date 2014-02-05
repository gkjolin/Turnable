using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entropy;
using TurnItUp.AI.Brains;
using TurnItUp.AI.Tactician;

namespace Tests.AI.Brains
{
    [TestClass]
    public class BrainTests
    {
        [TestMethod]
        public void Brain_Construction_IsSuccessful()
        {
            Brain brain = new Brain();
        }

        [TestMethod]
        public void Brain_IsAnEntropyComponent()
        {
            Brain brain = new Brain();

            Assert.IsInstanceOfType(brain, typeof(IComponent));
        }

        [TestMethod]
        public void Brain_WhenAskedToWakeUp_SetsItsCurrentGoalToMoveNextToThePlayer()
        {
            Brain brain = new Brain();

            brain.WakeUp();

            Assert.IsNotNull(brain.CurrentGoal);
            Assert.IsInstanceOfType(brain.CurrentGoal, typeof(MoveNextToCharacterGoal));
        }
    }
}
