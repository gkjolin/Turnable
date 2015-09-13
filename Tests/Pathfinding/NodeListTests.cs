using System;
using NUnit.Framework;
using Tests.Factories;
using System.Collections.Generic;
using Turnable.Pathfinding;
using Turnable.Api;
using System.Linq;

namespace Tests.Pathfinding
{
    [TestFixture]
    public class NodeListTests
    {
        private Node[] _nodes;
        private NodeList _nodeList;
        private ILevel _level;

        [SetUp]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _nodes = new Node[3];
            _nodes[0] = new Node(_level, 1, 1);
            _nodes[1] = new Node(_level, 2, 2);
            _nodes[2] = new Node(_level, 3, 2);
            _nodeList = new NodeList();
        }

        [Test]
        public void Add_AddsNode()
        {
            _nodeList.Add(_nodes[0]);

            Assert.That(_nodeList.Count, Is.EqualTo(1));
            Assert.That(_nodeList[0], Is.SameAs(_nodes[0]));
        }

        [Test]
        public void Insert_ThrowsException_InOrderToPreventOutOfOrderInsertion()
        {
            Assert.That(() => _nodeList.Insert(0, _nodes[0]), Throws.TypeOf<NotImplementedException>());
        }

        [Test]
        public void Add_AddsNodesSortedByEstimatedMovementCost()
        {
            _nodes[0].EstimatedMovementCost = 10;
            _nodes[1].EstimatedMovementCost = 5;
            _nodes[2].EstimatedMovementCost = 3;

            // NOTE: AddRange is not overriden in NodeList, so each node has to be added one by one in order to maintain the sorted order
            foreach (Node node in _nodes)
            {
                _nodeList.Add(node);
            }
            
            Assert.That(_nodeList[0], Is.EqualTo(_nodes[2]));
            Assert.That(_nodeList[1], Is.EqualTo(_nodes[1]));
            Assert.That(_nodeList[2], Is.EqualTo(_nodes[0]));
        }

        [Test]
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
                Assert.That(_nodeList.Contains(node), Is.True);
            }
        }
    }
}
