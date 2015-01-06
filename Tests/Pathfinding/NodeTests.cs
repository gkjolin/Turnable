using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Pathfinding
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}

//[TestClass]
//public class NodeTests
//{
//    private Node _node;
//    private ILevel _level;

//    [TestInitialize]
//    public void Initialize()
//    {
//        _level = LocationsFactory.BuildLevel();
//        _node = new Node(_level, 0, 0);
//    }

//    [TestMethod]
//    public void Node_WithoutParentNode_CanBeConstructed()
//    {
//        Node node = new Node(_level, 0, 0);

//        Assert.AreEqual(_level, node.Level);
//        Assert.AreEqual(0, node.Position.X);
//        Assert.AreEqual(0, node.Position.Y);
//        Assert.IsNull(node.Parent);
//    }

//    [TestMethod]
//    public void Node_WithParentNode_CanBeConstructed()
//    {
//        Node parentNode = new Node(_level, 0, 0);

//        Node node = new Node(_level, 0, 0, parentNode);

//        Assert.AreEqual(0, node.Position.X);
//        Assert.AreEqual(0, node.Position.Y);
//        Assert.AreEqual(parentNode, node.Parent);
//    }

//    [TestMethod]
//    public void Node_CanCalculateF()
//    {
//        _node.G = 5;
//        _node.H = 5;

//        Assert.AreEqual(_node.G + _node.H, _node.F);
//    }

//    [TestMethod]
//    public void Node_WithoutAParentNode_CanCalculateG()
//    {
//        Assert.AreEqual(0, _node.G);
//    }

//    [TestMethod]
//    public void Node_WithoutAParentNode_CanCalculateH()
//    {
//        Assert.AreEqual(0, _node.H);
//    }

//    [TestMethod]
//    public void Node_WithOrthogonalParentNode_CanCalculateG()
//    {
//        // Parent directly above child
//        Node parent = new Node(_level, 5, 4);
//        _node = new Node(_level, 5, 5, parent);
//        parent.G = 10;

//        Assert.AreEqual(parent.G + 10, _node.G);

//        // Parent directly below child
//        parent.Position.Y = 6;

//        Assert.AreEqual(parent.G + 10, _node.G);

//        // Parent to left of child
//        parent.Position.Y = 5;
//        parent.Position.X = 4;

//        Assert.AreEqual(parent.G + 10, _node.G);

//        // Parent to right of child
//        parent.Position.X = 6;

//        Assert.AreEqual(parent.G + 10, _node.G);
//    }

//    [TestMethod]
//    public void Node_WithDiagonalParentNode_CanCalculateG()
//    {
//        // Parent directly above and left of child
//        Node parent = new Node(_level, 4, 4);
//        _node = new Node(_level, 5, 5, parent);
//        parent.G = 10;

//        Assert.AreEqual(parent.G + 14, _node.G);

//        // Parent directly below and left of child
//        parent.Position.Y = 6;

//        Assert.AreEqual(parent.G + 14, _node.G);

//        // Parent to below and right of child
//        parent.Position.X = 6;

//        Assert.AreEqual(parent.G + 14, _node.G);

//        // Parent to above and right of child
//        parent.Position.X = 4;
//        parent.Position.Y = 6;

//        Assert.AreEqual(parent.G + 14, _node.G);
//    }

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
//    public void Node_ImplementsEquals()
//    {
//        Node node = new Node(_level, 1, 2);
//        Node node2 = new Node(_level, 1, 2);

//        Assert.AreEqual(node, node2);

//        node = new Node(_level, 2, 3);
//        Assert.AreNotEqual(node, node2);
//    }

//    [TestMethod]
//    public void Node_ImplementsEqualityOperator()
//    {
//        Node node = new Node(_level, 1, 2);
//        Node node2 = new Node(_level, 1, 2);

//        Assert.IsTrue(node == node2);
//    }

//    [TestMethod]
//    public void Node_ImplementsInequalityOperator()
//    {
//        Node node = new Node(_level, 1, 2);
//        Node node2 = new Node(_level, 1, 3);

//        Assert.IsTrue(node != node2);
//    }

//    [TestMethod]
//    public void Node_WhenUsingEqualityOperator_CanBeComparedToNull()
//    {
//        Node node = null;

//        Assert.IsTrue(node == null);
//    }

//    [TestMethod]
//    public void Node_WhenUsingInequalityOperator_CanBeComparedToNull()
//    {
//        Node node = new Node(_level, 1, 2);

//        Assert.IsTrue(node != null);
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