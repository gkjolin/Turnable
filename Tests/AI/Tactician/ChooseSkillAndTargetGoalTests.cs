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
    public class ChooseSkillAndTargetGoalTests
    {
        private Board _board;
        private Skill _skill;
        private Entity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
            _entity = _board.World.EntitiesWhere<Position>(p => p.X == 6 && p.Y == 5).Single();
            _skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            _entity.GetComponent<SkillSet>().Add(_skill);
        }

        [TestMethod]
        public void ChooseSkillAndTargetGoal_Construction_IsSuccessful()
        {
            ChooseSkillAndTargetGoal goal = new ChooseSkillAndTargetGoal(_entity, _board);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_board, goal.Board);
        }

        [TestMethod]
        public void ChooseSkillAndTargetGoal_WhenActivated_CreatesAUseSkillGoalWithTheBestSkillAndTarget()
        {
            ChooseSkillAndTargetGoal goal = new ChooseSkillAndTargetGoal(_entity, _board);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(UseSkillGoal));

            UseSkillGoal useSkillGoal = (UseSkillGoal)goal.Subgoals[0];
            Assert.AreEqual(_board.CharacterManager.Player.GetComponent<Position>(), useSkillGoal.Target);
            Assert.AreEqual(_skill, useSkillGoal.Skill);
        }
    }
}
