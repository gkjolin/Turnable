using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Pathfinding;

namespace Turnable.Api
{
    public interface IPathfinder
    {
        // Public interface
        List<Node> FindPath(Node startingNode, Node endingNode);

        // ----------
        // Properties
        // ----------
        bool AllowDiagonalMovement { get; set; }
        ILevel Level { get; set; }
    }
}
