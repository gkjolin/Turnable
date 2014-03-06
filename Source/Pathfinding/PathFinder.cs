using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Pathfinding
{
    // http://www.policyalmanac.org/games/aStarTutorial.htm
    public class PathFinder : IPathFinder
    {
        public bool AllowDiagonalMovement { get; set; }
        public ILevel Level { get; set; }

        public PathFinder(ILevel level, bool allowDiagonalMovement)
        {
            Level = level;
            AllowDiagonalMovement = allowDiagonalMovement;
        }

        public List<Node> SeekPath(Node startingNode, Node endingNode)
        {
            // If the endingNode is unwalkable, it's impossible to find a path to this node
            if (!endingNode.IsWalkable())
            {
                throw new InvalidOperationException("<PathFinder::SeekPath> : ending node is unwalkable. Cannot calculate path to this node.");
            }

            NodeList openNodes = new NodeList();
            NodeList closedNodes = new NodeList();
            Node node;
            Node currentNode;
            bool? shortestPathFound = null;
            int temporaryG = 0;
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
                    // Stop when you:
                    // Add the target square to the closed list, in which case the path has been found
                    shortestPathFound = true;
                    break;
                }

                foreach (Node adjacentNode in currentNode.GetAdjacentNodes(AllowDiagonalMovement))
                {
                    // If it is not walkable or if it is on the closed list, ignore it.
                    if (closedNodes.Find(x => x == adjacentNode) != null || !(adjacentNode.IsWalkable()))
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

            if (shortestPathFound == true)
            {
                // Save the path. Working backwards from the target square, go from each square to its parent square until you reach the starting square. That is your path. 
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

        public int MovementPointCost(Node startingNode, Node endingNode)
        {
            if (AllowDiagonalMovement)
            {
                return Math.Max(Math.Abs(startingNode.Position.X - endingNode.Position.X), Math.Abs(startingNode.Position.Y - endingNode.Position.Y));
            }
            else
            {
                return (Math.Abs(startingNode.Position.X - endingNode.Position.X) + Math.Abs(startingNode.Position.Y - endingNode.Position.Y));
            }
        }

        //public static HashSet<Node> GetPossibleMoveLocations(Node startingNode, int[,] mapData, int movementPoints)
        //{
        //    HashSet<Node> returnValue = new HashSet<Node>();

        //    // TODO: Implement an iterator in Level for each coordinate in a certain layer
        //    for (int col = 0; col <= mapData.GetUpperBound(0); col++)
        //    {
        //        for (int row = 0; row <= mapData.GetUpperBound(1); row++)
        //        {
        //            Node endingNode = new Node(col, row);
        //            if (MovementPointCost(startingNode, endingNode) <= movementPoints) returnValue.Add(endingNode);
        //        }
        //    }

        //    // Remove the startingLocation and the unwalkable locations from the returnValue
        //    returnValue.Remove(startingNode);
        //    returnValue.RemoveWhere(n => !n.IsWalkable(mapData));
        //    return returnValue;
        //}

        public Node GetClosestNode(Node startingNode, List<Node> candidateNodes)
        {
            Node returnValue = candidateNodes[0];

            foreach (Node node in candidateNodes)
            {
                if (MovementPointCost(startingNode, node) <= MovementPointCost(startingNode, returnValue))
                {
                    returnValue = node;
                }
            }

            return returnValue;
        }

        public Node GetClosestNode(Position startingPosition, HashSet<Position> candidatePositions)
        {
            List<Node> candidateNodes = candidatePositions.Select<Position, Node>(p => new Node(Level, p.X, p.Y)).ToList<Node>();

            return GetClosestNode(new Node(Level, startingPosition.X, startingPosition.Y), candidateNodes);
        }
    }
}

//public static class PathFinder
//    public static HashSet<LevelCoordinate> SLOWGetPossibleMoveLocations(LevelCoordinate startingLocation, Level level, int movementPoints)
//    {
//        HashSet<LevelCoordinate> returnValue = new HashSet<LevelCoordinate>();
//        returnValue = SLOWRecursivelyGetPossibleMoveLocations(startingLocation, level, movementPoints);
//        returnValue.Remove(startingLocation);
//        returnValue.RemoveWhere(l => !level.IsWalkable(l));

//        return returnValue;
//    }

//    private static HashSet<LevelCoordinate> SLOWRecursivelyGetPossibleMoveLocations(LevelCoordinate startingLocation, Level level, int movementPoints)
//    {
//        HashSet<LevelCoordinate> returnValue = new HashSet<LevelCoordinate>();

//        if (movementPoints == 1)
//        {
//            returnValue.UnionWith(level.AdjacentCoordinates(startingLocation));
//        }
//        else
//        {
//            level.AdjacentCoordinates(startingLocation).ForEach(b => returnValue.UnionWith(SLOWRecursivelyGetPossibleMoveLocations(b, level, movementPoints - 1)));
//        }

//        return returnValue;
//    }
//}}
