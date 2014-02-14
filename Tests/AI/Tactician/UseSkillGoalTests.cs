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
        private Mock<ISkill> _mockSkill;
        private TargetMap _targetMap;
        private Entity _entity;
        private Position _target;
        private List<Node> _bestPath;
        private Mock<IPathFinder> _mockPathFinder;
        private Mock<IBoard> _mockBoard;

        [TestInitialize]
        public void Initialize()
        {
            _mockBoard = new Mock<IBoard>();
            _bestPath = new List<Node>();
            _bestPath.Add(new Node(_mockBoard.Object, 0, 0));
            _bestPath.Add(new Node(_mockBoard.Object, 0, 1));
            _bestPath.Add(new Node(_mockBoard.Object, 0, 2));
            _mockPathFinder = new Mock<IPathFinder>();
            _mockPathFinder.Setup(pf => pf.SeekPath(It.IsAny<Node>(), It.IsAny<Node>())).Returns(_bestPath);
            _targetMap = new TargetMap();
            _targetMap.Add(new System.Tuples.Tuple<int,int>(5, 5), new HashSet<Position> { new Position(4, 5), new Position(4, 3), new Position(4, 6) });
            _mockSkill = new Mock<ISkill>();
            _mockSkill.Setup(s => s.CalculateTargetMap(It.IsAny<Board>())).Returns(_targetMap);
            _entity = EntropyFactory.BuildEntity();
            _target = new Position(0, 0);
        }

        [TestMethod]
        public void UseSkillGoal_Construction_IsSuccessful()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _mockSkill.Object, _target);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_mockSkill.Object, goal.Skill);
            Assert.AreEqual(_target, goal.Target);
        }

        [TestMethod]
        public void UseSkillGoal_WhenActivated_CreatesAFollowPathGoalWithTheBestPathInOrderToApplyASkillToTarget()
        {
            UseSkillGoal goal = new UseSkillGoal(_entity, _mockSkill.Object, _target);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.AreEqual(_bestPath.Count, ((FollowPathGoal)goal.Subgoals[0]).Path.Count);
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
