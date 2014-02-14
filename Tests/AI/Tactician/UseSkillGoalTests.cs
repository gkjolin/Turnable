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

namespace Tests.AI.Tactician
{
    [TestClass]
    public class UseSkillGoalTests
    {
        // The sample board:
        // XXXXXXXXXXXXXXXX
        // X....EEE.......X
        // X..........X...X
        // X.......E......X
        // X.E.X..........X
        // X.....S....E...X
        // X........X.....X
        // X..........XXXXX
        // X..........X...X
        // X..........X...X
        // X......X.......X
        // X.X........X...X
        // X..........X...X
        // X..........X...X
        // X......P...X...X
        // XXXXXXXXXXXXXXXX
        // X - Obstacles, P - Player, E - Enemies, S - Enemy that is going to use the skill
        private Board _board;
        private Skill _skill;
        private Entity _entity;
        private Position _target;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
            _entity = _board.World.EntitiesWhere<Position>(p => p.X == 7 && p.Y == 10).Single();
            _skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            _target = _board.CharacterManager.Player.GetComponent<Position>();
        }

        [TestMethod]
        public void UseSkillGoal_Construction_IsSuccessful()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _skill, _target);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_skill, goal.Skill);
            Assert.AreEqual(_board.CharacterManager.Player.GetComponent<Position>(), goal.Target);
        }

        [TestMethod]
        public void UseSkillGoal_WhenActivated_CreatesAFollowPathGoalWithTheBestPathInOrderToApplyASkillToTarget()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _skill, _target);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(FollowPathGoal));
        }

        //[TestMethod]
        //public void MoveAdjacentToPlayerGoal_WhenActivatedAndThereIsNoViablePathToThePlayer_Fails()
        //{
        //    _bestPath = null;
        //    _mockBoard.Setup(b => b.FindBestPathToMoveAdjacentToPlayer(It.IsAny<Position>())).Returns(_bestPath);
        //    MoveAdjacentToPlayerGoal goal = new MoveAdjacentToPlayerGoal(_entity);

        //    goal.Activate();

        //    Assert.AreEqual(0, goal.Subgoals.Count);
        //    Assert.AreEqual(GoalStatus.Failed, goal.Status);
        //}
    }
}
