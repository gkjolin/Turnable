using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Brains;
using Tests.Factories;
using Entropy;
using TurnItUp.AI.Tactician;
using TurnItUp.Interfaces;
using TurnItUp.Components;

namespace Tests.AI.Brains
{
    [TestClass]
    public class BrainTests
    {
        private Brain _brain;
        private Entity _character;
        private IBoard _board;

        [TestInitialize]
        public void Initialize()
        {
            _character = EntropyFactory.BuildEntity();
            _board = LocationsFactory.BuildBoard();
            _brain = new Brain(_character, _board);
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
            Brain brain = new Brain(_character, _board);

            Assert.AreEqual(_character, brain.Owner);
            Assert.AreEqual(_board, brain.Board);
            Assert.IsNull(brain.CurrentGoal);
        }

        [TestMethod]
        public void Brain_Thinking_DecidesOnTheMostDesirableGoal()
        {
            _brain.Think();

            Assert.IsInstanceOfType(_brain.CurrentGoal, typeof(ChooseSkillAndTargetGoal));
            Assert.AreEqual(_character, _brain.CurrentGoal.Owner);
            ChooseSkillAndTargetGoal goal = (ChooseSkillAndTargetGoal)_brain.CurrentGoal;
            Assert.AreEqual(_board, goal.Board);
        }
    }
}
