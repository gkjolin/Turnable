using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;

namespace Turnable.Pathfinding
{
    public class Node
    {
        public ILevel Level { get; set; }
        public Node Parent { get; set; }
        public Position Position { get; private set; }
        public int ActualMovementCost { get; set; }
        public int EstimatedMovementCost { get; set; }

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
    }
}
