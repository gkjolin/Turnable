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
    public class UseSkillGoalTests
    {
        // The sample board:
        // XXXXXXXXXXXXXXXX
        // X....EEE.......X
        // X..........X...X
        // X.......E......X
        // X.E.X..........X
        // X.....S....E...X
        // X.....o..X.....X
        // X.....o....XXXXX
        // X.....o....X...X
        // X.....o....X...X
        // X.....oX.......X
        // X.X...o....X...X
        // X.....o....X...X
        // X.....oo...X...X
        // X......P...X...X
        // XXXXXXXXXXXXXXXX
        // X - Obstacles, P - Player, E - Enemies, S - Enemy that is going to use the skill, o - Expected path
        private Board _board;
        private Skill _skill;
        private Entity _entity;
        private Position _target;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
            _entity = _board.World.EntitiesWhere<Position>(p => p.X == 6 && p.Y == 5).Single();
            _skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            _target = _board.CharacterManager.Player.GetComponent<Position>();
        }

        [TestMethod]
        public void UseSkillGoal_Construction_IsSuccessful()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _board, _skill, _target);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_board, goal.Board);
            Assert.AreEqual(_skill, goal.Skill);
            Assert.AreEqual(_board.CharacterManager.Player.GetComponent<Position>(), goal.Target);
        }

        [TestMethod]
        public void UseSkillGoal_WhenActivatedAndSkillCannotBeUsedFromOwnersPosition_CreatesAFollowPathGoalWithTheBestPathToApplySkillToTarget()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _board, _skill, _target);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(FollowPathGoal));

            FollowPathGoal followPathGoal = (FollowPathGoal)goal.Subgoals[0];
            Assert.AreEqual(10, followPathGoal.Path.Count);
            Assert.AreEqual(new Node(_board, 6, 5), followPathGoal.Path[0]);
            Assert.AreEqual(new Node(_board, 7, 13), followPathGoal.Path[followPathGoal.Path.Count - 1]);
        }

        [TestMethod]
        public void UseSkillGoal_WhenActivatedAndSkillCanBeUsedOnTargetFromOwnersPosition_CreatesAnApplySkillGoal()
        {
            _entity.GetComponent<Position>().X = 7;
            _entity.GetComponent<Position>().Y = 13;
            UseSkillGoal goal = new UseSkillGoal(_entity, _board, _skill, _target);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(ApplySkillGoal));

            ApplySkillGoal applySkillGoal = (ApplySkillGoal)goal.Subgoals[0];
            Assert.AreEqual(_skill, applySkillGoal.Skill);
            Assert.AreEqual(_board.CharacterManager.Player, applySkillGoal.Target);
        }

        [TestMethod]
        public void UseSkillGoal_WhenActivatedAndThereIsNoViablePathToThePlayer_Fails()
        {
            _board.CharacterManager.Player.GetComponent<Position>().X = 12;
            _board.CharacterManager.Player.GetComponent<Position>().Y = 10;
            _board.Map.Layers["Obstacles"].Tiles.Add(new System.Tuples.Tuple<int, int>(11, 10), new Tile(1, 11, 10));
            UseSkillGoal goal = new UseSkillGoal(_entity, _board, _skill, new Position(12, 10));

            goal.Activate();

            Assert.AreEqual(0, goal.Subgoals.Count);
            Assert.AreEqual(GoalStatus.Failed, goal.Status);
        }
    }
}
