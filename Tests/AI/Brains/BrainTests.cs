using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Brains;
using Tests.Factories;
using Entropy;
using TurnItUp.AI.Tactician;

namespace Tests.AI.Brains
{
    [TestClass]
    public class BrainTests
    {
        private Brain _brain;
        private Entity _character;

        [TestInitialize]
        public void Initialize()
        {
            _character = EntropyFactory.BuildEntity();
            _brain = new Brain(_character);
        }

        [TestMethod]
        public void Brain_HasADefaultConstructorWithNoParameters()
        {
            Brain brain = new Brain();

            Assert.IsNull(brain.Owner);
            Assert.IsNull(brain.CurrentGoal);
        }

        [TestMethod]
        public void Brain_Construction_IsSuccessful()
        {
            Brain brain = new Brain(_character);

            Assert.AreEqual(_character, brain.Owner);
            Assert.IsNull(brain.CurrentGoal);
        }

        [TestMethod]
        public void Brain_Thinking_DecidesOnTheMostDesirableGoal()
        {
            _brain.Think();

            Assert.IsInstanceOfType(_brain.CurrentGoal, typeof(MoveAdjacentToPlayerGoal));
            Assert.AreEqual(_character, _brain.CurrentGoal.Owner);
        }
    }
}
