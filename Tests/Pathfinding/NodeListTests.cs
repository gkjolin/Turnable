using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using System.Collections.Generic;
using Turnable.Pathfinding;
using Turnable.Api;
using System.Linq;

namespace Tests.Pathfinding
{
    [TestClass]
    public class NodeListTests
    {
        private Node[] _nodes;
        private NodeList _nodeList;
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _nodes = new Node[3];
            _nodes[0] = new Node(_level, 0, 0);
            _nodes[1] = new Node(_level, 5, 5);
            _nodes[2] = new Node(_level, 5, 5);
            _nodeList = new NodeList();
        }

        [TestMethod]
        public void Add_AddsNode()
        {
            _nodeList.Add(_nodes[0]);

            Assert.AreEqual(1, _nodeList.Count);
            Assert.AreEqual(_nodes[0], _nodeList[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Insert_ThrowsException_InOrderToPreventOutOfOrderInsertion()
        {
            _nodeList.Insert(0, _nodes[0]);
        }

        [TestMethod]
        public void Add_AddsNodesSortedByPathScore()
        {
            _nodes[0].ActualMovementCost = 10;
            _nodes[0].EstimatedMovementCost = 10;
            _nodes[1].ActualMovementCost = 5;
            _nodes[1].EstimatedMovementCost = 5;
            _nodes[2].ActualMovementCost = 3;
            _nodes[2].EstimatedMovementCost = 3;

            _nodeList.AddRange(_nodes);

            Assert.AreEqual(_nodes[2], _nodeList[0]);
            Assert.AreEqual(_nodes[1], _nodeList[1]);
            Assert.AreEqual(_nodes[0], _nodeList[2]);
        }

        [TestMethod]
        public void Add_WhenAddingNodesWithSamePathScore_Succeeds()
        {
            _nodes[0].ActualMovementCost = 5;
            _nodes[0].EstimatedMovementCost = 5;
            _nodes[1].ActualMovementCost = 5;
            _nodes[1].EstimatedMovementCost = 5;
            _nodes[2].ActualMovementCost = 5;
            _nodes[2].EstimatedMovementCost = 5;

            _nodeList.AddRange(_nodes);

            foreach (Node node in _nodes)
            {
                Assert.IsTrue(_nodeList.Contains(node));
            }
        }
    }
}
