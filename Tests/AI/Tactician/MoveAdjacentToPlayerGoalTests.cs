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

namespace Tests.AI.Tactician
{
    [TestClass]
    public class MoveAdjacentToPlayerGoalTests
    {
        private Entity _entity;
        private Mock<Board> _mockBoard;
        private Entity _player;

        [TestInitialize]
        public void Initialize()
        {
            _mockBoard = new Mock<Board>();
            _player = EntropyFactory.BuildEntity();
            _player.AddComponent(new Position(5, 5));
            _entity = EntropyFactory.BuildEntity();
            _entity.AddComponent(new OnBoard(_mockBoard.Object));
        }

        [TestMethod]
        public void MoveAdjacentToPlayerGoal_Construction_IsSuccessful()
        {
            MoveAdjacentToPlayerGoal goal = new MoveAdjacentToPlayerGoal(_entity);

            Assert.AreEqual(_entity, goal.Entity);
        }

        [TestMethod]
        public void MoveAdjacentToPlayerGoal_WhenActivated_CreatesAFollowPathGoalWithTheBestPathToMoveAdjacentToThePlayer()
        {
            //_path.Add(new Node(0, 0));
            //_path.Add(new Node(0, 1));

            //FollowPathGoal goal = new FollowPathGoal(_entity, _path);

            //goal.Activate();

            //Assert.AreEqual(1, goal.Subgoals.Count);
            //Assert.IsInstanceOfType(goal.Subgoals[0], typeof(MoveToGoal));
            //Assert.AreEqual(_entity, ((MoveToGoal)goal.Subgoals[0]).Entity);
            //Assert.AreEqual(_path[1], ((MoveToGoal)goal.Subgoals[0]).Destination);
        }
    }
}
