using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;

namespace Turnable.Pathfinding
{
    public class NodeList : List<Node>
    {
        public new void Add(Node item)
        {
            // WARNING! Because of this new method, you should never cast NodeList to a List
            base.Add(item);
            Sort(new NodeComparer());
        }

        public new void Insert(int index, Node item)
        {
            throw new NotImplementedException();
        }

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node node1, Node node2)
            {
                return node1.PathScore.CompareTo(node2.PathScore);
            }
        }
    }
}