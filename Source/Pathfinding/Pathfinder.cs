using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Pathfinding
{
    public class Pathfinder
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
            NodeList openNodes = new NodeList();
            NodeList closedNodes = new NodeList();
            //Node node;
            Node currentNode;
            bool? shortestPathFound = null;
            //int temporaryActualMovementCost = 0;
            List<Node> path = new List<Node>();

            openNodes.Add(startingNode);

            while (shortestPathFound == null)
            {
                //// If a path to the endingNode has not yet been found AND there are no openNodes, there is no feasible path to the endingNode
                //if (openNodes.Count == 0)
                //{
                //    return null;
                //}

                currentNode = openNodes[0];

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode == endingNode)
                {
                    // Stop when target square has been added to the closed list, in which case the shortest path has been found
                    shortestPathFound = true;
                    break;
                }

                foreach (Node adjacentNode in currentNode.GetAdjacentNodes())
                {
                    // If it is not walkable or if it is on the closed list, ignore it.
                    if (closedNodes.Contains(adjacentNode))
                    {
                        continue;
                    }

                    node = openNodes.Find(x => x == adjacentNode);
                    // If it isn’t on the open list, add it to the open list. Make the current square the parent of this square. Record the G and H costs of the square. 
                    if (node == null)
                    {
                        node = new Node(Level, adjacentNode.Position.X, adjacentNode.Position.Y, currentNode);
                        node.CalculateH(endingNode.Position.X, endingNode.Position.Y);
                        openNodes.Add(node);
                    }
                    else
                    {
                        //If it is on the open list already, check to see if this path to that square is better, using G cost as the measure. 
                        temporaryG = node.G - node.Parent.G + currentNode.G;
                        if (adjacentNode.IsOrthogonalTo(currentNode))
                        {
                            temporaryG += 10;
                        }
                        else  // Nodes diagonal to each other
                        {
                            temporaryG += 14;
                        }
                        // A lower G cost means that this is a better path. If so, change the parent of the square to the current square, and recalculate the G and F scores of the square. If you are keeping your open list sorted by F score, you may need to resort the list to account for the change.
                        if (temporaryG < node.G)
                        {
                            node.Parent = currentNode;
                            node.G = temporaryG;
                        }
                    }
                    //d) Stop when you:
                    //Fail to find the target square, and the open list is empty. In this case, there is no path.   
                }
            }

            return path;
        }

        //{
        //    // If the endingNode is unwalkable, it's impossible to find a path to this node
        //    if (!endingNode.IsWalkable())
        //    {
        //        throw new InvalidOperationException("<PathFinder::SeekPath> : ending node is unwalkable. Cannot calculate path to this node.");
        //    }




        //    if (shortestPathFound == true)
        //    {
        //        // Save the path. Working backwards from the target square, go from each square to its parent square until you reach the starting square. That is your path. 
        //        node = closedNodes.Find(x => x == endingNode);

        //        path.Add(node);

        //        while (node.Parent != null)
        //        {
        //            node = node.Parent;
        //            path.Add(node);
        //        }

        //        path.Reverse();
        //        return path;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
