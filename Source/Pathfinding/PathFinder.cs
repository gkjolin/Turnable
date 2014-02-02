using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;

namespace TurnItUp.Pathfinding
{
    // http://www.policyalmanac.org/games/aStarTutorial.htm
    public class PathFinder
    {
        public bool AllowDiagonalMovement { get; private set; }

        public PathFinder(bool allowDiagonalMovement)
        {
            AllowDiagonalMovement = allowDiagonalMovement;
        }

        public List<Node> SeekPath(Node startingNode, Node endingNode, Board board)
        {
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

                foreach (Node adjacentNode in currentNode.GetAdjacentNodes(board, AllowDiagonalMovement))
                {
                    // If it is not walkable or if it is on the closed list, ignore it.
                    if (closedNodes.Find(x => x == adjacentNode) != null || !(adjacentNode.IsWalkable(board)))
                    {
                        continue;
                    }

                    node = openNodes.Find(x => x == adjacentNode);
                    // If it isn’t on the open list, add it to the open list. Make the current square the parent of this square. Record the G and H costs of the square. 
                    if (node == null)
                    {
                        node = new Node(adjacentNode.X, adjacentNode.Y, currentNode);
                        node.CalculateH(endingNode.X, endingNode.Y);
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
                return Math.Max(Math.Abs(startingNode.X - endingNode.X), Math.Abs(startingNode.Y - endingNode.Y));
            }
            else
            {
                return (Math.Abs(startingNode.X - endingNode.X) + Math.Abs(startingNode.Y - endingNode.Y));
            }
        }

        //public static HashSet<Node> GetPossibleMoveLocations(Node startingNode, int[,] mapData, int movementPoints)
        //{
        //    HashSet<Node> returnValue = new HashSet<Node>();

        //    // TODO: Implement an iterator in Board for each coordinate in a certain layer
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

        public object ClosestNode(Node startingNode, List<Node> candidateNodes)
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
    }
}

//public static class PathFinder
//    public static HashSet<BoardCoordinate> SLOWGetPossibleMoveLocations(BoardCoordinate startingLocation, Board board, int movementPoints)
//    {
//        HashSet<BoardCoordinate> returnValue = new HashSet<BoardCoordinate>();
//        returnValue = SLOWRecursivelyGetPossibleMoveLocations(startingLocation, board, movementPoints);
//        returnValue.Remove(startingLocation);
//        returnValue.RemoveWhere(l => !board.IsWalkable(l));

//        return returnValue;
//    }

//    private static HashSet<BoardCoordinate> SLOWRecursivelyGetPossibleMoveLocations(BoardCoordinate startingLocation, Board board, int movementPoints)
//    {
//        HashSet<BoardCoordinate> returnValue = new HashSet<BoardCoordinate>();

//        if (movementPoints == 1)
//        {
//            returnValue.UnionWith(board.AdjacentCoordinates(startingLocation));
//        }
//        else
//        {
//            board.AdjacentCoordinates(startingLocation).ForEach(b => returnValue.UnionWith(SLOWRecursivelyGetPossibleMoveLocations(b, board, movementPoints - 1)));
//        }

//        return returnValue;
//    }
//}}
