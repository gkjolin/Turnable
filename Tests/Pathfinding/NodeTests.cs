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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        //[TestMethod]
        //public void IsWithinBounds_ReturnsWhetherTheNodesPositionsAreWithinTheBoundsOfTheLevel()
        //{
        //    Node node = new Node(_level, 7, 7);
        //    Assert.IsTrue(node.IsWithinBounds());

        //    node = new Node(_level, 0, 0);
        //    Assert.IsTrue(node.IsWithinBounds());

        //    node = new Node(_level, 14, 14);
        //    Assert.IsTrue(node.IsWithinBounds());

        //    node = new Node(_level, 20, 4);
        //    Assert.IsFalse(node.IsWithinBounds());

        //    node = new Node(_level, -1, -1);
        //    Assert.IsFalse(node.IsWithinBounds());

        //    node = new Node(_level, 16, 16);
        //    Assert.IsFalse(node.IsWithinBounds());
        //}
    }
}



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