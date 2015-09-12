using System;
using NUnit.Framework;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Pathfinding;
using Turnable.Components;
using Tests.Factories;
using System.Collections.Generic;

namespace Tests.Pathfinding
{
    [TestFixture]
    public class NodeTests
    {
        private Node _node;
        private ILevel _level;

        [SetUp]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _node = new Node(_level, 0, 0);
        }

        [Test]
        public void Constructor_ForNodeWithoutParentNode_InitializesNode()
        {
            Node node = new Node(_level, 0, 0);

            Assert.AreEqual(_level, node.Level);
            Assert.AreEqual(0, node.Position.X);
            Assert.AreEqual(0, node.Position.Y);
            Assert.IsNull(node.Parent);
        }

        [Test]
        public void Constructor_ForNodeWithParentNode_InitializesNode()
        {
            Node parentNode = new Node(_level, 0, 0);

            Node node = new Node(_level, 0, 0, parentNode);

            Assert.AreEqual(0, node.Position.X);
            Assert.AreEqual(0, node.Position.Y);
            Assert.AreEqual(parentNode, node.Parent);           
        }

        [Test]
        public void Constructor_ForNodeWithParentNodeAndPosition_InitializesNode()
        {
            Node parentNode = new Node(_level, 0, 0);
            Position position = new Position(1, 2);
            
            Node node = new Node(_level, position, parentNode);

            Assert.AreEqual(position, node.Position);
            Assert.AreEqual(parentNode, node.Parent);
        }

        // Calculating values for A* algorithm (PathScore, EstimatedMovementCost and ActualMovementCost)
        [Test]
        public void PathScore_IsCalculatedAsTheSumOfActualMovementCostAndEstimatedMovementCost()
        {
            _node.ActualMovementCost = 4;
            _node.EstimatedMovementCost = 5;

            Assert.AreEqual(_node.ActualMovementCost + _node.EstimatedMovementCost, _node.PathScore);
        }

        [Test]
        public void ActualMovementCost_ForANodeWithoutAParent_Is0()
        {
            Assert.AreEqual(0, _node.ActualMovementCost);
        }

        [Test]
        public void EstimatedMovementCost_ForANodeWithoutAParent_Is0()
        {
            Assert.AreEqual(0, _node.EstimatedMovementCost);
        }

        [Test]
        public void ActualMovementCost_WithOrthogonalParentNode_HasAnOrthogonalMovementCost()
        {
            Node parent = new Node(_level, 5, 5);
            Direction[] orthogonalDirections = new Direction[4] {Direction.North, Direction.East, Direction.South, Direction.East};

            foreach(Direction direction in orthogonalDirections)
            {
                Node node = new Node(_level, parent.Position.NeighboringPosition(direction), parent);
                
                Assert.AreEqual(parent.ActualMovementCost + Node.OrthogonalMovementCost, node.ActualMovementCost);
            }
        }

        [Test]
        public void ActualMovementCost_WithDiagonalParentNode_HasADiagonalMovementCost()
        {
            Node parent = new Node(_level, 5, 5);
            Direction[] diagonalDirections = new Direction[4] { Direction.NorthEast, Direction.SouthEast, Direction.SouthWest, Direction.NorthWest };

            foreach (Direction direction in diagonalDirections)
            {
                Node node = new Node(_level, parent.Position.NeighboringPosition(direction), parent);

                Assert.AreEqual(parent.ActualMovementCost + Node.DiagonalMovementCost, node.ActualMovementCost);
            }
        }

        [Test]
        public void EstimatedMovementCost_CanBeSetToAValue()
        {
            _node.EstimatedMovementCost = 10;

            Assert.AreEqual(10, _node.EstimatedMovementCost);
        }

        [Test]
        public void EstimatedMovementCost_IsCalculatedAsTheManhattanDistanceBetweenTwoPositions()
        {
            // Manhattan distance = (Simple sum of the horizontal and vertical components) * OrthogonalMovementCost
            Node node = new Node(_level, 5, 5, null);

            node.CalculateEstimatedMovementCost(4, 4);
            Assert.AreEqual(20, node.EstimatedMovementCost);

            node.CalculateEstimatedMovementCost(4, 5);
            Assert.AreEqual(10, node.EstimatedMovementCost);

            node.CalculateEstimatedMovementCost(5, 4);
            Assert.AreEqual(10, node.EstimatedMovementCost);
        }

        [Test]
        public void IsOrthogonalTo_ReturnsWhetherOtherNodeIsOrthogonalToTheNode()
        {
            Node node = new Node(_level, 5, 5);

            Node node2 = new Node(_level, 5, 6);
            Assert.IsTrue(node.IsOrthogonalTo(node2));

            node2 = new Node(_level, 4, 5);
            Assert.IsTrue(node.IsOrthogonalTo(node2));

            node2 = new Node(_level, 6, 6);
            Assert.IsFalse(node.IsOrthogonalTo(node2));
        }

        [Test]
        public void IsDiagonalTo_ReturnsWhetherOtherNodeIsDiagonalToTheNode()
        {
            Node node = new Node(_level, 5, 5);

            Node node2 = new Node(_level, 5, 6);
            Assert.IsFalse(node.IsDiagonalTo(node2));

            node2 = new Node(_level, 4, 5);
            Assert.IsFalse(node.IsDiagonalTo(node2));

            node2 = new Node(_level, 6, 6);
            Assert.IsTrue(node.IsDiagonalTo(node2));
        }

        [Test]
        public void IsWithinBounds_ReturnsWhetherTheNodeIsPositionedWithinTheBoundsOfTheLevel()
        {
            Node node = new Node(_level, 7, 7);
            Assert.IsTrue(node.IsWithinBounds());

            node = new Node(_level, 0, 0);
            Assert.IsTrue(node.IsWithinBounds());

            node = new Node(_level, 14, 14);
            Assert.IsTrue(node.IsWithinBounds());

            node = new Node(_level, 20, 4);
            Assert.IsFalse(node.IsWithinBounds());

            node = new Node(_level, -1, -1);
            Assert.IsFalse(node.IsWithinBounds());

            node = new Node(_level, 16, 16);
            Assert.IsFalse(node.IsWithinBounds());
        }

        [Test]
        public void GetAdjacentNodes_FindsAllAdjacentNodes()
        {
            Node node = new Node(_level, 5, 5);

            List<Node> adjacentNodes = node.GetAdjacentNodes();

            Assert.AreEqual(8, adjacentNodes.Count);
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 6)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 6)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 6)));
        }

        [Test]
        public void GetAdjacentNodes_DisregardsNodesThatAreOutOfBound()
        {
            _node = new Node(_level, 0, 0);

            List<Node> adjacentNodes = _node.GetAdjacentNodes();

            Assert.AreEqual(3, adjacentNodes.Count);
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 1, 0)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 0, 1)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 1, 1)));
        }

        [Test]
        public void GetAdjacentNodes_WithoutDiagonalMovement_FindsAllAdjacentOrthogonalNodes()
        {
            Node node = new Node(_level, 5, 5);

            List<Node> adjacentNodes = node.GetAdjacentNodes(false);

            Assert.AreEqual(4, adjacentNodes.Count);
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 6)));
        }

        [Test]
        public void IsWalkable_DeterminesIfThereIsAnObstacleAtTheNodesPosition()
        {
            // Anything out of bounds is unwalkable
            _node = new Node(_level, -1, 1);
            Assert.IsFalse(_node.IsWalkable());

            // Any obstacle is unwalkable
            _node = new Node(_level, 0, 0);
            Assert.IsFalse(_node.IsWalkable());
            _node = new Node(_level, 0, 1);
            Assert.IsFalse(_node.IsWalkable());

            // TODO: Test the unwalkability of characters (NPCs and PCs)
            //_node = new Node(_level, 5, 14);
            //Assert.IsFalse(_node.IsWalkable());

            // Empty spaces are walkable
            _node = new Node(_level, 5, 5);
            Assert.IsTrue(_node.IsWalkable());
            _node = new Node(_level, 1, 13);
            Assert.IsTrue(_node.IsWalkable());
        }

        // Equals Tests
        [Test]
        public void Equals_FromIEquatableTInterface_CanCompareNodes()
        {
            Node node = new Node(_level, 1, 2);
            Node node2 = new Node(_level, 1, 2);

            Assert.IsTrue(node.Equals(node2));

            ILevel anotherLevel = LocationsFactory.BuildLevel();
            node2 = new Node(anotherLevel, 1, 2);
            Assert.IsFalse(node.Equals(node2));

            node2 = new Node(_level, 2, 3);
            Assert.IsFalse(node.Equals(node2));
        }

        [Test]
        public void Equals_FromIEquatableTInterface_CanCompareNodeToNull()
        {
            Node node = new Node(_level, 1, 2);

            Assert.IsFalse(node.Equals((Node)null));
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanCompareNodes()
        {
            Object node = new Node(_level, 1, 2);
            Object node2 = new Node(_level, 1, 2);

            Assert.IsTrue(node.Equals(node2));

            ILevel anotherLevel = LocationsFactory.BuildLevel();
            node2 = new Node(anotherLevel, 1, 2);
            Assert.IsFalse(node.Equals(node2));

            node2 = new Node(_level, 2, 3);
            Assert.IsFalse(node.Equals(node2));
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanCompareNodeToNull()
        {
            Object node = new Node(_level, 1, 2);

            Assert.IsFalse(node.Equals(null));
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_ReturnsFalseIfOtherObjectIsNotANode()
        {
            Object node = new Node(_level, 1, 2);

            Assert.IsFalse(node.Equals(new Object()));
        }

        [Test]
        public void EqualityOperator_IsImplemented()
        {
            Node node = new Node(_level, 1, 2);
            Node node2 = new Node(_level, 1, 2);

            Assert.IsTrue(node == node2);
        }

        [Test]
        public void InequalityOperator_IsImplemented()
        {
            Node node = new Node(_level, 1, 2);
            Node node2 = new Node(_level, 2, 3);

            Assert.IsTrue(node != node2);
        }

        [Test]
        public void EqualityOperator_CanComparePositionToNull()
        {
            Node node = null;

            Assert.IsTrue(node == null);
        }

        [Test]
        public void InequalityOperator_CanComparePositionToNull()
        {
            Node node = new Node(_level, 1, 2);

            Assert.IsTrue(node != null);
        }

        [Test]
        public void GetHashCode_UsesThePositionsHashCode()
        {
            Node node = new Node(_level, 1, 2);

            Assert.AreEqual(node.Position.GetHashCode(), node.GetHashCode());
        }

        [Test]
        public void ToString_DisplaysXAndYCoordinatesOfNodeAndParent()
        {
            Node parent = new Node(_level, 8, 8);
            Node node = new Node(_level, 7, 7, parent);

            Assert.AreEqual("(7, 7); Parent (8, 8)", node.ToString());
        }

        [Test]
        public void ToString_ForNodeWithNullParent_DisplaysNullForParent()
        {
            Node node = new Node(_level, 8, 8);

            Assert.AreEqual("(8, 8); Parent null", node.ToString());
        }
    }
}