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

            Assert.That(node.Level, Is.SameAs(_level));
            Assert.That(node.Position, Is.EqualTo(new Position(0, 0)));
            Assert.That(node.Parent, Is.Null);
        }

        [Test]
        public void Constructor_ForNodeWithParentNode_InitializesNode()
        {
            Node parentNode = new Node(_level, 0, 0);

            Node node = new Node(_level, 0, 0, parentNode);

            Assert.That(node.Level, Is.SameAs(_level));
            Assert.That(node.Position, Is.EqualTo(new Position(0, 0)));
            Assert.That(node.Parent, Is.SameAs(parentNode));           
        }

        [Test]
        public void Constructor_ForNodeWithParentNodeAndPosition_InitializesNode()
        {
            Node parentNode = new Node(_level, 0, 0);
            Position position = new Position(1, 2);
            
            Node node = new Node(_level, position, parentNode);

            Assert.That(node.Position, Is.EqualTo(position));
            Assert.That(node.Parent, Is.SameAs(parentNode));
        }

        // Calculating values for A* algorithm (PathScore, EstimatedMovementCost and ActualMovementCost)
        [Test]
        public void PathScore_IsCalculatedAsTheSumOfActualMovementCostAndEstimatedMovementCost()
        {
            _node.ActualMovementCost = 4;
            _node.EstimatedMovementCost = 5;

            Assert.That(_node.PathScore, Is.EqualTo(_node.ActualMovementCost + _node.EstimatedMovementCost));
        }

        [Test]
        public void ActualMovementCost_ForANodeWithoutAParent_Is0()
        {
            Assert.That(_node.ActualMovementCost, Is.EqualTo(0));
        }

        [Test]
        public void EstimatedMovementCost_ForANodeWithoutAParent_Is0()
        {
            Assert.That(_node.EstimatedMovementCost, Is.EqualTo(0));
        }

        [Test]
        public void ActualMovementCost_WithOrthogonalParentNode_HasAnOrthogonalMovementCost()
        {
            Node parent = new Node(_level, 5, 5);
            Direction[] orthogonalDirections = new Direction[4] {Direction.North, Direction.East, Direction.South, Direction.East};

            foreach(Direction direction in orthogonalDirections)
            {
                Node node = new Node(_level, parent.Position.NeighboringPosition(direction), parent);
                
                Assert.That(node.ActualMovementCost, Is.EqualTo(parent.ActualMovementCost + Node.OrthogonalMovementCost));
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

                Assert.That(node.ActualMovementCost, Is.EqualTo(parent.ActualMovementCost + Node.DiagonalMovementCost));
            }
        }

        [Test]
        public void EstimatedMovementCost_CanBeSetToAValue()
        {
            _node.EstimatedMovementCost = 10;

            Assert.That(_node.EstimatedMovementCost, Is.EqualTo(10));
        }

        [Test]
        public void EstimatedMovementCost_IsCalculatedAsTheManhattanDistanceBetweenTwoPositions()
        {
            // Manhattan distance = (Simple sum of the horizontal and vertical components) * OrthogonalMovementCost
            Node node = new Node(_level, 5, 5, null);

            node.CalculateEstimatedMovementCost(4, 4);
            Assert.That(node.EstimatedMovementCost, Is.EqualTo(20));

            node.CalculateEstimatedMovementCost(4, 5);
            Assert.That(node.EstimatedMovementCost, Is.EqualTo(10));

            node.CalculateEstimatedMovementCost(5, 4);
            Assert.That(node.EstimatedMovementCost, Is.EqualTo(10));
        }

        [Test]
        public void IsOrthogonalTo_ReturnsWhetherOtherNodeIsOrthogonalToTheNode()
        {
            Node node = new Node(_level, 5, 5);

            Node node2 = new Node(_level, 5, 6);
            Assert.That(node.IsOrthogonalTo(node2), Is.True);

            node2 = new Node(_level, 4, 5);
            Assert.That(node.IsOrthogonalTo(node2), Is.True);

            node2 = new Node(_level, 6, 6);
            Assert.That(node.IsOrthogonalTo(node2), Is.False);
        }

        [Test]
        public void IsDiagonalTo_ReturnsWhetherOtherNodeIsDiagonalToTheNode()
        {
            Node node = new Node(_level, 5, 5);

            Node node2 = new Node(_level, 5, 6);
            Assert.That(node.IsDiagonalTo(node2), Is.False);

            node2 = new Node(_level, 4, 5);
            Assert.That(node.IsDiagonalTo(node2), Is.False);

            node2 = new Node(_level, 6, 6);
            Assert.That(node.IsDiagonalTo(node2), Is.True);
        }

        [Test]
        public void IsWithinBounds_ReturnsWhetherTheNodeIsPositionedWithinTheBoundsOfTheLevel()
        {
            Node node = new Node(_level, 7, 7);
            Assert.That(node.IsWithinBounds(), Is.True);

            node = new Node(_level, 0, 0);
            Assert.That(node.IsWithinBounds(), Is.True);

            node = new Node(_level, 14, 14);
            Assert.That(node.IsWithinBounds(), Is.True);

            node = new Node(_level, 20, 4);
            Assert.That(node.IsWithinBounds(), Is.False);

            node = new Node(_level, -1, -1);
            Assert.That(node.IsWithinBounds(), Is.False);

            node = new Node(_level, 16, 16);
            Assert.That(node.IsWithinBounds(), Is.False);
        }

        [Test]
        public void GetAdjacentNodes_FindsAllAdjacentNodes()
        {
            Node node = new Node(_level, 5, 5);

            List<Node> adjacentNodes = node.GetAdjacentNodes();

            Assert.That(adjacentNodes.Count, Is.EqualTo(8));
            Assert.That(adjacentNodes.Contains(new Node(_level, 4, 4)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 5, 4)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 6, 4)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 4, 5)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 6, 5)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 4, 6)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 5, 6)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 6, 6)), Is.True);
        }

        [Test]
        public void GetAdjacentNodes_DisregardsNodesThatAreOutOfBound()
        {
            _node = new Node(_level, 0, 0);

            List<Node> adjacentNodes = _node.GetAdjacentNodes();

            Assert.That(adjacentNodes.Count, Is.EqualTo(3));
            Assert.That(adjacentNodes.Contains(new Node(_level, 1, 0)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 0, 1)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 1, 1)), Is.True);
        }

        [Test]
        public void GetAdjacentNodes_WithoutDiagonalMovement_FindsAllAdjacentOrthogonalNodes()
        {
            Node node = new Node(_level, 5, 5);

            List<Node> adjacentNodes = node.GetAdjacentNodes(false);

            Assert.That(adjacentNodes.Count, Is.EqualTo(4));
            Assert.That(adjacentNodes.Contains(new Node(_level, 5, 4)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 4, 5)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 6, 5)), Is.True);
            Assert.That(adjacentNodes.Contains(new Node(_level, 5, 6)), Is.True);
        }

        [Test]
        public void IsWalkable_DeterminesIfThereIsAnObstacleAtTheNodesPosition()
        {
            // Anything out of bounds is unwalkable
            _node = new Node(_level, -1, 1);
            Assert.That(_node.IsWalkable(), Is.False);

            // Any obstacle is unwalkable
            _node = new Node(_level, 0, 0);
            Assert.That(_node.IsWalkable(), Is.False);
            _node = new Node(_level, 0, 1);
            Assert.That(_node.IsWalkable(), Is.False);

            // TODO: Test the unwalkability of characters (NPCs and PCs)
            //_node = new Node(_level, 5, 14);
            //Assert.IsFalse(_node.IsWalkable());

            // Empty spaces are walkable
            _node = new Node(_level, 5, 5);
            Assert.That(_node.IsWalkable(), Is.True);
            _node = new Node(_level, 1, 13);
            Assert.That(_node.IsWalkable(), Is.True);
        }

        // Equals Tests
        [Test]
        public void Equals_FromIEquatableTInterface_CanCompareNodes()
        {
            Node node = new Node(_level, 1, 2);
            Node node2 = new Node(_level, 1, 2);

            Assert.That(node.Equals(node2), Is.True);

            ILevel anotherLevel = LocationsFactory.BuildLevel();
            node2 = new Node(anotherLevel, 1, 2);
            Assert.That(node.Equals(node2), Is.False);

            node2 = new Node(_level, 2, 3);
            Assert.(node.Equals(node2), Is.False);
        }

        [Test]
        public void Equals_FromIEquatableTInterface_CanCompareNodeToNull()
        {
            Node node = new Node(_level, 1, 2);

            Assert.That(node.Equals((Node)null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanCompareNodes()
        {
            Object node = new Node(_level, 1, 2);
            Object node2 = new Node(_level, 1, 2);

            Assert.That(node.Equals(node2), Is.True);

            ILevel anotherLevel = LocationsFactory.BuildLevel();
            node2 = new Node(anotherLevel, 1, 2);
            Assert.That(node.Equals(node2), Is.False);

            node2 = new Node(_level, 2, 3);
            Assert.That(node.Equals(node2), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanCompareNodeToNull()
        {
            Object node = new Node(_level, 1, 2);

            Assert.That(node.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_ReturnsFalseIfOtherObjectIsNotANode()
        {
            Object node = new Node(_level, 1, 2);

            Assert.That(node.Equals(new Object()), Is.False);
        }

        [Test]
        public void EqualityOperator_IsImplemented()
        {
            Node node = new Node(_level, 1, 2);
            Node node2 = new Node(_level, 1, 2);

            Assert.That(node == node2, Is.True);
        }

        [Test]
        public void InequalityOperator_IsImplemented()
        {
            Node node = new Node(_level, 1, 2);
            Node node2 = new Node(_level, 2, 3);

            Assert.That(node != node2, Is.True);
        }

        [Test]
        public void EqualityOperator_CanComparePositionToNull()
        {
            Node node = null;

            Assert.That(node == null, Is.True);
        }

        [Test]
        public void InequalityOperator_CanComparePositionToNull()
        {
            Node node = new Node(_level, 1, 2);

            Assert.That(node != null, Is.True);
        }

        [Test]
        public void GetHashCode_UsesThePositionsHashCode()
        {
            Node node = new Node(_level, 1, 2);

            Assert.That(node.GetHashCode(), Is.EqualTo(node.Position.GetHashCode()));
        }

        [Test]
        public void ToString_DisplaysXAndYCoordinatesOfNodeAndParent()
        {
            Node parent = new Node(_level, 8, 8);
            Node node = new Node(_level, 7, 7, parent);

            Assert.That(node.ToString(), Is.EqualTo("(7, 7); Parent (8, 8)"));
        }

        [Test]
        public void ToString_ForNodeWithNullParent_DisplaysNullForParent()
        {
            Node node = new Node(_level, 8, 8);

            Assert.That(node.ToString(), Is.EqualTo("(8, 8); Parent null"));
        }
    }
}