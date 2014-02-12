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

namespace Tests.AI.Tactician
{
    [TestClass]
    public class MoveAdjacentToPlayerGoalTests
    {
        private Entity _entity;
        private Mock<Board> _mockBoard;
        private PathFinder _pathFinder;
        private Entity _player;
        private List<Node> _bestPath;

        [TestInitialize]
        public void Initialize()
        {
            _pathFinder = new PathFinder(_mockBoard.Object, false);
            _bestPath = new List<Node>();
            _bestPath.Add(new Node(_mockBoard.Object, 0, 0));
            _bestPath.Add(new Node(_mockBoard.Object, 0, 1));
            _bestPath.Add(new Node(_mockBoard.Object, 0, 2));
            _mockBoard = new Mock<Board>();
            _mockBoard.Setup(b => b.FindBestPathToMoveAdjacentToPlayer(It.IsAny<Position>())).Returns(_bestPath);
            _mockBoard.Setup(b => b.PathFinder).Returns(_pathFinder);
            _player = EntropyFactory.BuildEntity();
            _player.AddComponent(new Position(5, 5));
            _entity = EntropyFactory.BuildEntity();
            _entity.AddComponent(new OnBoard(_mockBoard.Object));
        }

        [TestMethod]
        public void MoveAdjacentToPlayerGoal_Construction_IsSuccessful()
        {
            MoveAdjacentToPlayerGoal goal = new MoveAdjacentToPlayerGoal(_entity);

            Assert.AreEqual(_entity, goal.Owner);
        }

        [TestMethod]
        public void MoveAdjacentToPlayerGoal_WhenActivated_CreatesAFollowPathGoalWithTheBestPathToMoveAdjacentToThePlayer()
        {
            MoveAdjacentToPlayerGoal goal = new MoveAdjacentToPlayerGoal(_entity);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.AreEqual(_bestPath.Count, ((FollowPathGoal)goal.Subgoals[0]).Path.Count);
        }

        [TestMethod]
        public void MoveAdjacentToPlayerGoal_WhenActivatedAndThereIsNoViablePathToThePlayer_Fails()
        {
            _bestPath = null;
            _mockBoard.Setup(b => b.FindBestPathToMoveAdjacentToPlayer(It.IsAny<Position>())).Returns(_bestPath);
            MoveAdjacentToPlayerGoal goal = new MoveAdjacentToPlayerGoal(_entity);

            goal.Activate();

            Assert.AreEqual(0, goal.Subgoals.Count);
            Assert.AreEqual(GoalStatus.Failed, goal.Status);
        }
    }
}
