using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Pathfinding;
using Turnable.Components;

namespace Tests.Pathfinding
{
    [TestClass]
    public class NodeTests
    {
        private Node _node;
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = new Level();
            _node = new Node(_level, 0, 0);
        }

        [TestMethod]
        public void Constructor_ForNodeWithoutParentNode_InitializesNode()
        {
            Node node = new Node(_level, 0, 0);

            Assert.AreEqual(_level, node.Level);
            Assert.AreEqual(0, node.Position.X);
            Assert.AreEqual(0, node.Position.Y);
            Assert.IsNull(node.Parent);
        }

        [TestMethod]
        public void Constructor_ForNodeWithParentNode_InitializesNode()
        {
            Node parentNode = new Node(_level, 0, 0);

            Node node = new Node(_level, 0, 0, parentNode);

            Assert.AreEqual(0, node.Position.X);
            Assert.AreEqual(0, node.Position.Y);
            Assert.AreEqual(parentNode, node.Parent);           
        }

        [TestMethod]
        public void Constructor_ForNodeWithParentNodeAndPosition_InitializesNode()
        {
            Node parentNode = new Node(_level, 0, 0);
            Position position = new Position(1, 2);
            
            Node node = new Node(_level, position, parentNode);

            Assert.AreEqual(position, node.Position);
            Assert.AreEqual(parentNode, node.Parent);
        }

        // Calculating values for A* algorithm (PathScore, EstimatedMovementCost and ActualMovementCost)
        [TestMethod]
        public void PathScore_IsCalculatedAsTheSumOfActualMovementCostAndEstimatedMovementCost()
        {
            _node.ActualMovementCost = 4;
            _node.EstimatedMovementCost = 5;

            Assert.AreEqual(_node.ActualMovementCost + _node.EstimatedMovementCost, _node.PathScore);
        }

        [TestMethod]
        public void ActualMovementCost_ForANodeWithoutAParent_Is0()
        {
            Assert.AreEqual(0, _node.ActualMovementCost);
        }

        [TestMethod]
        public void EstimatedMovementCost_ForANodeWithoutAParent_Is0()
        {
            Assert.AreEqual(0, _node.EstimatedMovementCost);
        }

        [TestMethod]
        public void ActualMovementCost_WithOrthogonalParentNode_HasACostOf10()
        {
            Node node = new Node(_level, 5, 5);
            Direction[] orthogonalDirections = new Direction[4] {Direction.North, Direction.East, Direction.South, Direction.East};

            foreach(Direction direction in orthogonalDirections)
            {
                Node parent = new Node(_level, node.Position.NeighboringPosition(direction));

                Assert.AreEqual(parent.ActualMovementCost + 10, _node.ActualMovementCost);
            }
        }

        [TestMethod]
        public void ActualMovementCost_WithDiagonalParentNode_HasACostOf14()
        {
            Node node = new Node(_level, 5, 5);
            Direction[] diagonalDirections = new Direction[4] { Direction.NorthEast, Direction.SouthEast, Direction.SouthWest, Direction.NorthWest };

            foreach (Direction direction in diagonalDirections)
            {
                Node parent = new Node(_level, node.Position.NeighboringPosition(direction));

                Assert.AreEqual(parent.ActualMovementCost + 14, _node.ActualMovementCost);
            }
        }
    }
}


//    [TestMethod]
//    public void Node_CanCalculateH()
//    {
//        _node = new Node(_level, 5, 5, null);

//        _node.CalculateH(4, 4);
//        Assert.AreEqual(20, _node.H);

//        _node.CalculateH(4, 5);
//        Assert.AreEqual(10, _node.H);

//        _node.CalculateH(5, 4);
//        Assert.AreEqual(10, _node.H);
//    }

//    [TestMethod]
//    public void Node_CanDetermineIfItIsWithinBounds()
//    {
//        _node = new Node(_level, 7, 7);
//        Assert.IsTrue(_node.IsWithinBounds());

//        _node = new Node(_level, 0, 0);
//        Assert.IsTrue(_node.IsWithinBounds());

//        _node = new Node(_level, 14, 14);
//        Assert.IsTrue(_node.IsWithinBounds());

//        _node = new Node(_level, 20, 4);
//        Assert.IsFalse(_node.IsWithinBounds());

//        _node = new Node(_level, -1, -1);
//        Assert.IsFalse(_node.IsWithinBounds());

//        _node = new Node(_level, 16, 16);
//        Assert.IsFalse(_node.IsWithinBounds());
//    }

//    [TestMethod]
//    public void Node_CanFindAdjacentNodes()
//    {
//        _node = new Node(_level, 5, 5);

//        List<Node> adjacentNodes = _node.GetAdjacentNodes();

//        Assert.AreEqual(8, adjacentNodes.Count);
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 4)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 4)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 4)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 5)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 5)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 6)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 6)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 6)));
//    }

//    [TestMethod]
//    public void Node_CanFindOnlyOrthogonallyAdjacentNodes()
//    {
//        _node = new Node(_level, 5, 5);

//        List<Node> adjacentNodes = _node.GetAdjacentNodes(false);

//        Assert.AreEqual(4, adjacentNodes.Count);
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 4)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 4, 5)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 6, 5)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 5, 6)));
//    }

//    [TestMethod]
//    public void Node_CanFindAdjacentNodes_AndDisregardsNodesThatAreOutOfBounds()
//    {
//        _node = new Node(_level, 0, 0);

//        List<Node> adjacentNodes = _node.GetAdjacentNodes();

//        Assert.AreEqual(3, adjacentNodes.Count);
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 1, 0)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 0, 1)));
//        Assert.IsTrue(adjacentNodes.Contains(new Node(_level, 1, 1)));
//    }

//    [TestMethod]
//    public void Node_CanDetermineIfItIsOrthogonalToAnotherNode()
//    {
//        _node = new Node(_level, 5, 5);

//        Node node2 = new Node(_level, 5, 6);
//        Assert.IsTrue(_node.IsOrthogonalTo(node2));

//        node2 = new Node(_level, 4, 5);
//        Assert.IsTrue(_node.IsOrthogonalTo(node2));

//        node2 = new Node(_level, 6, 6);
//        Assert.IsFalse(_node.IsOrthogonalTo(node2));
//    }

//    [TestMethod]
//    public void Node_CanDetermineIfItIsWalkable()
//    {
//        // Anything out of bounds is unwalkable
//        _node = new Node(_level, -1, 1);
//        Assert.IsFalse(_node.IsWalkable());

//        // Any obstacle is unwalkable
//        _node = new Node(_level, 0, 0);
//        Assert.IsFalse(_node.IsWalkable());
//        _node = new Node(_level, 0, 1);
//        Assert.IsFalse(_node.IsWalkable());

//        // Any character is unwalkable
//        _node = new Node(_level, 5, 14);
//        Assert.IsFalse(_node.IsWalkable());

//        _node = new Node(_level, 5, 5);
//        Assert.IsTrue(_node.IsWalkable());

//        _node = new Node(_level, 1, 14);
//        Assert.IsTrue(_node.IsWalkable());
//    }

//    [TestMethod]
//    public void Node_ToString_DisplaysPositionToString()
//    {
//        _node = new Node(_level, 4, 5);
//        Assert.AreEqual(_node.Position.ToString(), _node.ToString());
//    }
//}