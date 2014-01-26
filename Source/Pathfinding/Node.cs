using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;

namespace TurnItUp.Pathfinding
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Node Parent { get; set; }
        public int H { get; set; }
        private int _g;

        public Node(int x, int y, Node parent = null)
        {
            X = x;
            Y = y;
            Parent = parent;
        }

        public int G
        {
            get
            {
                if (Parent == null)
                {
                    return _g;
                }
                else
                {
                    if (X == Parent.X || Y == Parent.Y)
                    {
                        return Parent.G + 10;
                    }
                    else
                    {
                        return Parent.G + 14;
                    }
                }
            }

            set
            {
                _g = value;
            }
        }

        public int F
        {
            get
            {
                return G + H;
            }
        }

        public void CalculateH(int endingX, int endingY)
        {
            H = (Math.Abs(endingX - X) + Math.Abs(endingY - Y)) * 10;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;

            return this == (Node)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static bool operator ==(Node node1, Node node2)
        {
            if (object.ReferenceEquals(node1, null))
            {
                return object.ReferenceEquals(node2, null);
            }
            if (object.ReferenceEquals(node2, null))
            {
                return object.ReferenceEquals(node1, null);
            }

            return node1.X == node2.X && node1.Y == node2.Y;
        }

        public static bool operator !=(Node node1, Node node2)
        {
            return !(node1 == node2);
        }

        public List<Node> GetAdjacentNodes(Board board)
        {
            List<Node> returnValue = new List<Node>();

            for (int x = X - 1; x <= X + 1; x++)
            {
                for (int y = Y - 1; y <= Y + 1; y++)
                {
                    if (x == X && y == Y) continue;

                    returnValue.Add(new Node(x, y));
                }
            }

            return returnValue.FindAll(n => n.IsWithinBounds(board));
        }

        public bool IsWithinBounds(Board board)
        {
            return (X >= 0 && X <= (board.Map.Width - 1) &&
                    Y >= 0 && Y <= (board.Map.Height - 1));
        }

        public bool IsWalkable(Board board)
        {
            return (IsWithinBounds(board) && !board.IsObstacle(X, Y));
        }

        public bool IsOrthogonalTo(Node other)
        {
            return (X == other.X || Y == other.Y);
        }
    }
}
