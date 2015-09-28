using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Pathfinding;

namespace Turnable.Api
{
    public interface IPathfinder
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        List<Node> FindPath(Node startingNode, Node endingNode);
        int MovementCost(Node startingNode, Node endingNode);

        // Properties
        bool AllowDiagonalMovement { get; set; }
        ILevel Level { get; set; }
    }
}
