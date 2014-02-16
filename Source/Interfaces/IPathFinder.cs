using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Pathfinding;

namespace TurnItUp.Interfaces
{
    public interface IPathFinder
    {
        bool AllowDiagonalMovement { get; set; }
        IBoard Board { get; set; }

        List<Node> SeekPath(Node startingNode, Node endingNode);
        int MovementPointCost(Node startingNode, Node endingNode);
        Node GetClosestNode(Node startingNode, List<Node> candidateNodes);
        Node GetClosestNode(Position startingPosition, HashSet<Position> candidatePositions);
    }
}