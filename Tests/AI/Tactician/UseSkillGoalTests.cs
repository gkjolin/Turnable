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

namespace Tests.AI.Tactician
{
    [TestClass]
    public class UseSkillGoalTests
    {
        private Skill _skill;
        private Entity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = EntropyFactory.BuildEntity();    
            _skill = new Skill("Melee Attack");
        }

        [TestMethod]
        public void UseSkillGoal_Construction_IsSuccessful()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _skill);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_skill, goal.Skill);
        }

        //[TestMethod]
        //public void UseSkill_WhenActivated_CreatesAMoveToClosestSkillOriginGoal()
        //{
        //    UseSkillGoal goal = new UseSkillGoal(_entity, _skill);

        //    Assert.AreEqual(_entity, goal.Owner);
        //    Assert.AreEqual(_skill, goal.Skill);

        //    goal.Activate();

        //    Assert.AreEqual(1, goal.Subgoals.Count);
        //    Assert.IsInstanceOfType(goal.Subgoals[0], typeof(MoveToClosestSkillOriginGoal));
        //}
    }
}
