using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using System.Collections.Generic;
using TurnItUp.Interfaces;

namespace Tests.Pathfinding
{
    [TestClass]
    public class NodeListTests
    {
        private Node _node;
        private NodeList _nodeList;
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _node = new Node(_level, 0, 0);
            _nodeList = new NodeList();
        }

        [TestMethod]
        public void NodeList_CanAddNewNode()
        {
            _nodeList.Add(_node);

            Assert.AreEqual(1, _nodeList.Count);
            Assert.AreEqual(_node, _nodeList[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void NodeList_WhenInserting_ThrowsException()
        {
            _nodeList.Insert(0, _node);
        }

        [TestMethod]
        public void NodeList_WhenAddingNodes_AddsThemSortedByF()
        {
            Node node2 = new Node(_level, 5, 5);
            Node node3 = new Node(_level, 5, 5);

            _node.G = 10;
            _node.H = 10;
            node2.G = 5;
            node2.H = 5;
            node3.G = 3;
            node3.H = 3;

            _nodeList.Add(_node);
            _nodeList.Add(node2);
            _nodeList.Add(node3);

            Assert.AreEqual(node3, _nodeList[0]);
            Assert.AreEqual(node2, _nodeList[1]);
            Assert.AreEqual(_node, _nodeList[2]);
        }

        [TestMethod]
        public void NodeList_WhenAddingNodesWithSameF_AddsThemSuccessfully()
        {
            Node node2 = new Node(_level, 5, 5);
            Node node3 = new Node(_level, 5, 5);

            _node.G = 5;
            _node.H = 5;
            node2.G = 5;
            node2.H = 5;
            node3.G = 5;
            node3.H = 5;

            _nodeList.Add(_node);
            _nodeList.Add(node2);
            _nodeList.Add(node3);

            Assert.IsTrue(_nodeList.Contains(_node));
            Assert.IsTrue(_nodeList.Contains(node2));
            Assert.IsTrue(_nodeList.Contains(node3));
        }
    }
}
