using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using System.Collections.Generic;

namespace Tests.Pathfinding
{
    [TestClass]
    public class NodeTests
    {
        private Node _node;
        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _node = new Node(0, 0);
            _board = LocationsFactory.BuildBoard();
        }

        [TestMethod]
        public void Node_WithoutParentNode_CanBeConstructed()
        {
            Node node = new Node(0, 0);

            Assert.AreEqual(0, node.X);
            Assert.AreEqual(0, node.Y);
            Assert.IsNull(node.Parent);
        }

        [TestMethod]
        public void Node_WithParentNode_CanBeConstructed()
        {
            Node parentNode = new Node(0, 0);

            Node node = new Node(0, 0, parentNode);

            Assert.AreEqual(0, node.X);
            Assert.AreEqual(0, node.Y);
            Assert.AreEqual(parentNode, node.Parent);
        }

        [TestMethod]
        public void Node_CanCalculateF()
        {
            _node.G = 5;
            _node.H = 5;

            Assert.AreEqual(_node.G + _node.H, _node.F);
        }

        [TestMethod]
        public void Node_WithoutAParentNode_CanCalculateG()
        {
            Assert.AreEqual(0, _node.G);
        }

        [TestMethod]
        public void Node_WithoutAParentNode_CanCalculateH()
        {
            Assert.AreEqual(0, _node.H);
        }

        [TestMethod]
        public void Node_WithOrthogonalParentNode_CanCalculateG()
        {
            // Parent directly above child
            Node parent = new Node(5, 4);
            _node = new Node(5, 5, parent);
            parent.G = 10;

            Assert.AreEqual(parent.G + 10, _node.G);

            // Parent directly below child
            parent.Y = 6;

            Assert.AreEqual(parent.G + 10, _node.G);

            // Parent to left of child
            parent.Y = 5;
            parent.X = 4;

            Assert.AreEqual(parent.G + 10, _node.G);

            // Parent to right of child
            parent.X = 6;

            Assert.AreEqual(parent.G + 10, _node.G);
        }

        [TestMethod]
        public void Node_WithDiagonalParentNode_CanCalculateG()
        {
            // Parent directly above and left of child
            Node parent = new Node(4, 4);
            _node = new Node(5, 5, parent);
            parent.G = 10;

            Assert.AreEqual(parent.G + 14, _node.G);

            // Parent directly below and left of child
            parent.Y = 6;

            Assert.AreEqual(parent.G + 14, _node.G);

            // Parent to below and right of child
            parent.X = 6;

            Assert.AreEqual(parent.G + 14, _node.G);

            // Parent to above and right of child
            parent.X = 4;
            parent.Y = 6;

            Assert.AreEqual(parent.G + 14, _node.G);
        }

        [TestMethod]
        public void Node_CanCalculateH()
        {
            _node = new Node(5, 5, null);

            _node.CalculateH(4, 4);
            Assert.AreEqual(20, _node.H);

            _node.CalculateH(4, 5);
            Assert.AreEqual(10, _node.H);

            _node.CalculateH(5, 4);
            Assert.AreEqual(10, _node.H);
        }

        [TestMethod]
        public void Node_ImplementsEquals()
        {
            Node node = new Node(1, 2);
            Node node2 = new Node(1, 2);

            Assert.AreEqual(node, node2);

            node = new Node(2, 3);
            Assert.AreNotEqual(node, node2);
        }

        [TestMethod]
        public void Node_ImplementsEqualityOperator()
        {
            Node node = new Node(1, 2);
            Node node2 = new Node(1, 2);

            Assert.IsTrue(node == node2);
        }

        [TestMethod]
        public void Node_ImplementsInequalityOperator()
        {
            Node node = new Node(1, 2);
            Node node2 = new Node(1, 3);

            Assert.IsTrue(node != node2);
        }

        [TestMethod]
        public void Node_WhenUsingEqualityOperator_CanBeComparedToNull()
        {
            Node node = null;

            Assert.IsTrue(node == null);
        }

        [TestMethod]
        public void Node_WhenUsingInequalityOperator_CanBeComparedToNull()
        {
            Node node = new Node(1, 2);

            Assert.IsTrue(node != null);
        }

        [TestMethod]
        public void Node_CanDetermineIfItIsWithinBounds()
        {
            _node = new Node(7, 7);
            Assert.IsTrue(_node.IsWithinBounds(_board));

            _node = new Node(0, 0);
            Assert.IsTrue(_node.IsWithinBounds(_board));

            _node = new Node(14, 14);
            Assert.IsTrue(_node.IsWithinBounds(_board));

            _node = new Node(20, 4);
            Assert.IsFalse(_node.IsWithinBounds(_board));

            _node = new Node(-1, -1);
            Assert.IsFalse(_node.IsWithinBounds(_board));

            _node = new Node(16, 16);
            Assert.IsFalse(_node.IsWithinBounds(_board));
        }

        [TestMethod]
        public void Node_CanFindAdjacentNodes()
        {
            _node = new Node(5, 5);

            List<Node> adjacentNodes = _node.GetAdjacentNodes(_board);

            Assert.AreEqual(8, adjacentNodes.Count);
            Assert.IsTrue(adjacentNodes.Contains(new Node(4, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(5, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(6, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(4, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(6, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(4, 6)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(5, 6)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(6, 6)));
        }

        [TestMethod]
        public void Node_CanFindOnlyOrthogonallyAdjacentNodes()
        {
            _node = new Node(5, 5);

            List<Node> adjacentNodes = _node.GetAdjacentNodes(_board, false);

            Assert.AreEqual(4, adjacentNodes.Count);
            Assert.IsTrue(adjacentNodes.Contains(new Node(5, 4)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(4, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(6, 5)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(5, 6)));
        }

        [TestMethod]
        public void Node_CanFindAdjacentNodes_AndDisregardsNodesThatAreOutOfBounds()
        {
            _node = new Node(0, 0);

            List<Node> adjacentNodes = _node.GetAdjacentNodes(_board);

            Assert.AreEqual(3, adjacentNodes.Count);
            Assert.IsTrue(adjacentNodes.Contains(new Node(1, 0)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(0, 1)));
            Assert.IsTrue(adjacentNodes.Contains(new Node(1, 1)));
        }

        [TestMethod]
        public void Node_CanDetermineIfItIsOrthogonalToAnotherNode()
        {
            _node = new Node(5, 5);

            Node node2 = new Node(5, 6);
            Assert.IsTrue(_node.IsOrthogonalTo(node2));

            node2 = new Node(4, 5);
            Assert.IsTrue(_node.IsOrthogonalTo(node2));

            node2 = new Node(6, 6);
            Assert.IsFalse(_node.IsOrthogonalTo(node2));
        }

        [TestMethod]
        public void Node_CanDetermineIfItIsWalkable()
        {
            _node = new Node(0, 0);
            Assert.IsFalse(_node.IsWalkable(_board));

            _node = new Node(0, 1);
            Assert.IsFalse(_node.IsWalkable(_board));

            _node = new Node(-1, 1);
            Assert.IsFalse(_node.IsWalkable(_board));

            _node = new Node(5, 10);
            Assert.IsTrue(_node.IsWalkable(_board));

            _node = new Node(1, 1);
            Assert.IsTrue(_node.IsWalkable(_board));
        }
    }
}
