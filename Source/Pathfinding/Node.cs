using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;

namespace Turnable.Pathfinding
{
    public class Node
    {
        public ILevel Level { get; set; }
        public Node Parent { get; set; }
        public Position Position { get; private set; }
        private int _actualMovementCost;
        public const int OrthogonalMovementCost = 10;
        public const int DiagonalMovementCost = 14;

        public Node(ILevel level, Position position, Node parent = null)
        {
            Level = level;
            Position = position;
            Parent = parent;
        }

        public Node(ILevel level, int x, int y, Node parent = null)
            : this(level, new Position(x, y), parent)
        {
        }

        public int PathScore { 
            get
            {
                return ActualMovementCost + EstimatedMovementCost;
            }
        }

        public int ActualMovementCost { 
            get
            {
                if (Parent == null) return 0;
                if (IsOrthogonalTo(Parent)) return Parent.ActualMovementCost + OrthogonalMovementCost;
                return Parent.ActualMovementCost + DiagonalMovementCost;
            }
            set
            {
            }
        }

        public int EstimatedMovementCost { get; set; }

        public void CalculateEstimatedMovementCost(int destinationX, int destinationY)
        {
            EstimatedMovementCost = (Math.Abs(destinationX - Position.X) + Math.Abs(destinationY - Position.Y)) * OrthogonalMovementCost;
        }

        public bool IsOrthogonalTo(Node other)
        {
            return (other.Position.X == Position.X || other.Position.Y == Position.Y);
        }

        public bool IsDiagonalTo(Node other)
        {
            return !IsOrthogonalTo(other);
        }

        public bool IsWithinBounds()
        {
            return (Position.X >= 0 && Position.X <= (Level.Map.Width - 1) &&
                    Position.Y >= 0 && Position.Y <= (Level.Map.Height - 1));
        }

        public override string ToString()
        {
            if (Parent == null)
            {
                return String.Format("({0}, {1}); Parent null", Position.X, Position.Y);
            }
            else
            {
                return String.Format("({0}, {1}); Parent ({2}, {3})", Position.X, Position.Y, Parent.Position.X, Parent.Position.Y);
            }
            
        }

        public List<Node> GetAdjacentNodes()
        {
            List<Node> adjacentNodes = new List<Node>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                adjacentNodes.Add(new Node(Level, Position.NeighboringPosition(direction)));
            }

            return adjacentNodes;
        }
    }
}
