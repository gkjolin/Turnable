using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.AI.Tactician;
using Entropy;
using Tests.Factories;
using Moq;
using TurnItUp.Locations;
using TurnItUp.Components;
using TurnItUp.AI.Goals;
using TurnItUp.Skills;
using TurnItUp.Interfaces;
using TurnItUp.Tmx;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class DoNothingGoalTests
    {
        private Entity _entity;

        [TestInitialize]
        public void Initialize()
        {
            World world = new World();
            _entity = world.CreateEntity();
        }

        [TestMethod]
        public void DoNothingGoal_Construction_IsSuccessful()
        {
            DoNothingGoal goal = new DoNothingGoal(_entity);

            Assert.AreEqual(_entity, goal.Owner);
        }
    }
}
