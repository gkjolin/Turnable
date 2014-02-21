using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Pathfinding
{
    public class NodeList : IList<Node>
    {
        private List<Node> list = new List<Node>();

        public Node Find(Predicate<Node> match)
        {
            return list.Find(match);
        }

        public int IndexOf(Node item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, Node item)
        {
            throw new NotImplementedException("<NodeList::Insert> : cannot insert at index; must preserve order.");
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public Node this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list.RemoveAt(index);
                list.Add(value);
            }
        }

        public void Add(Node item)
        {
            list.Add(item);
            list.Sort(new NodeComparer());
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Node item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Node[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Node item)
        {
            return list.Remove(item);
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node node1, Node node2)
            {
                return node1.F.CompareTo(node2.F);
            }
        }
    }
}
