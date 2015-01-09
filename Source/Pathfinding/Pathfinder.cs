using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Pathfinding
{
    public class Pathfinder : IPathfinder
    {
        public bool AllowDiagonalMovement { get; set; }
        public ILevel Level { get; set; }

        public Pathfinder(ILevel level, bool allowDiagonalMovement = true)
        {
            Level = level;
            AllowDiagonalMovement = allowDiagonalMovement;
        }

        public List<Node> FindPath(Node startingNode, Node endingNode)
        {
            // If the endingNode is unwalkable, it's impossible to find a path to this node
            if (!endingNode.IsWalkable())
            {
                throw new InvalidOperationException();
            }

            NodeList openNodes = new NodeList();
            NodeList closedNodes = new NodeList();
            Node node;
            Node currentNode;
            bool? shortestPathFound = null;
            int actualMovementCost = 0;
            List<Node> path = new List<Node>();

            openNodes.Add(startingNode);

            while (shortestPathFound == null)
            {
                // If a path to the endingNode has not yet been found AND there are no openNodes, there is no feasible path to the endingNode
                if (openNodes.Count == 0)
                {
                    return null;
                }

                currentNode = openNodes[0];

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode == endingNode)
                {
                    // Stop when target node has been added to the closed list, in which case the shortest path has been found
                    shortestPathFound = true;
                    break;
                }

                foreach (Node adjacentNode in currentNode.GetAdjacentNodes(AllowDiagonalMovement))
                {
                    // If it is not walkable or if it is on the closed list, ignore it.
                    if (closedNodes.Find(x => x == adjacentNode) != null || !adjacentNode.IsWalkable())
                    {
                        continue;
                    }

                    node = openNodes.Find(x => x == adjacentNode);
                    // If it isn’t on the open list, add it to the open list. Make the current node the parent of this node. Calculate the EstimatedMovementCost of the node.
                    if (node == null)
                    {
                        node = new Node(Level, adjacentNode.Position, currentNode);
                        node.CalculateEstimatedMovementCost(endingNode.Position.X, endingNode.Position.Y);
                        openNodes.Add(node);
                    }
                    else
                    {
                        //If it is on the open list already, check to see if this path to that node is better, using ActualMovementCost as the measure. 
                        actualMovementCost = node.ActualMovementCost - node.Parent.ActualMovementCost + currentNode.ActualMovementCost;
                        if (adjacentNode.IsOrthogonalTo(currentNode))
                        {
                            actualMovementCost += 10;
                        }
                        else  // Nodes diagonal to each other
                        {
                            actualMovementCost += 14;
                        }
                        // A lower ActualMovementCost means that this is a better path. If so, change the parent of the node to the current node. The ActualMovementCost and PathScore will be automatically recalculated when the parent is changed. Our NodeList automatically sorts nodes by PathScore, so we don't have to do any manual resorting.
                        if (actualMovementCost < node.ActualMovementCost)
                        {
                            node.Parent = currentNode;
                        }
                    }
                    //d) Stop when you:
                    //Fail to find the target node, and the open list is empty. In this case, there is no path.
                }
            }

            if (shortestPathFound == true)
            {
                // Save the path: Working backwards from the target node, go from each node to its parent node. This is the shortest path.
                node = closedNodes.Find(x => x == endingNode);

                path.Add(node);

                while (node.Parent != null)
                {
                    node = node.Parent;
                    path.Add(node);
                }

                path.Reverse();
                return path;
            }
            else
            {
                return null;
            }
        }
    }
}
