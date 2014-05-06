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
    public class ApplySkillGoalTests
    {
        private ILevel _level;
        private Skill _skill;
        private Entity _entity;
        private Position _target;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _entity = _level.World.EntitiesWhere<Position>(p => p.X == 6 && p.Y == 5).Single();
            _skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            _target = _level.CharacterManager.Player.GetComponent<Position>();
        }

        [TestMethod]
        public void ApplySkillGoal_Construction_IsSuccessful()
        {
            ApplySkillGoal goal = new ApplySkillGoal(_entity, _skill, _level.CharacterManager.Player);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_skill, goal.Skill);
            Assert.AreEqual(_level.CharacterManager.Player, goal.Target);
        }

        [TestMethod]
        public void ApplySkillGoal_Processing_SuccessfullyAppliesSkill()
        {
            Mock<ISkill> skill = new Mock<ISkill>();

            ApplySkillGoal goal = new ApplySkillGoal(_entity, skill.Object, _level.CharacterManager.Player);

            goal.Process();

            skill.Verify(s => s.Apply(_entity, _level.CharacterManager.Player));
        }
    }
}
